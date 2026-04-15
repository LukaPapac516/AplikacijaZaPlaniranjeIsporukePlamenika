param (
    [string]$Message,
    [string]$LogPrefix = "EVENT"
)

$logPath = "C:\Users\lukap\Desktop\Aplikacija za isporuku plamenika\lab-1\agent_log.txt"
$statePath = "C:\Users\lukap\Desktop\Aplikacija za isporuku plamenika\lab-1\agent_log_state.json"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

function Format-HookMessage {
    param(
        [string]$RawMessage,
        [string]$Prefix
    )

    $text = "$RawMessage".Trim()
    if ([string]::IsNullOrWhiteSpace($text)) {
        return "<empty>"
    }

    # Keep plain user/assistant text as-is.
    if (-not ($text.StartsWith("{") -or $text.StartsWith("["))) {
        return $text
    }

    try {
        $obj = $text | ConvertFrom-Json -ErrorAction Stop
    }
    catch {
        if ($text -match '"?hook_event_name"?\s*[:=]\s*"?PreToolUse"?') {
            $toolName = ""
            $goal = ""

            if ($text -match '"?tool_name"?\s*[:=]\s*"?([A-Za-z0-9_]+)"?') {
                $toolName = $Matches[1]
            }

            if ($text -match '"?goal"?\s*[:=]\s*"?([^",}\]]+)"?') {
                $goal = $Matches[1].Trim().Trim('"')
            }

            if (-not [string]::IsNullOrWhiteSpace($goal)) {
                return "PreToolUse tool=$toolName goal=$goal"
            }

            return "PreToolUse tool=$toolName"
        }

        return ($text -replace "\s+", " ")
    }

    if ($obj -and $obj.hook_event_name) {
        $event = [string]$obj.hook_event_name
        if ($event -eq "PreToolUse") {
            $toolName = [string]$obj.tool_name
            $goal = ""
            if ($obj.tool_input -and $obj.tool_input.goal) {
                $goal = [string]$obj.tool_input.goal
            }

            if (-not [string]::IsNullOrWhiteSpace($goal)) {
                return "$event tool=$toolName goal=$goal"
            }

            return "$event tool=$toolName"
        }

        if ($event -eq "Stop") {
            if ($obj.stop_reason) {
                return "$event reason=$($obj.stop_reason)"
            }

            return $event
        }

        return $event
    }

    # Fallback for arbitrary JSON payloads.
    return ($text -replace "\s+", " ")
}

function Get-TranscriptPathFromMessage {
    param([string]$RawMessage)

    if ([string]::IsNullOrWhiteSpace($RawMessage)) {
        return ""
    }

    try {
        $obj = $RawMessage | ConvertFrom-Json -ErrorAction Stop
        if ($obj -and $obj.transcript_path) {
            $p = [string]$obj.transcript_path
            if (Test-Path $p) {
                return $p
            }
        }
    }
    catch {
        if ($RawMessage -match '"transcript_path"\s*:\s*"([^"]+\.jsonl)"') {
            $p = $Matches[1] -replace '\\\\', '\\'
            if (Test-Path $p) {
                return $p
            }
        }

        if ($RawMessage -match 'transcript_path\s*[:=]\s*([A-Za-z]:\\[^\s,\}]+\.jsonl)') {
            $p = $Matches[1]
            if (Test-Path $p) {
                return $p
            }
        }
    }

    return ""
}

function Get-LatestAssistantMessage {
    param([string]$RawMessage)

    $transcriptPath = Get-TranscriptPathFromMessage -RawMessage $RawMessage
    if ([string]::IsNullOrWhiteSpace($transcriptPath)) {
        $workspaceRoot = Join-Path $env:APPDATA "Code\User\workspaceStorage"
        if (-not (Test-Path $workspaceRoot)) {
            return $null
        }

        $transcriptFiles = Get-ChildItem -Path $workspaceRoot -Recurse -File -Filter "*.jsonl" -ErrorAction SilentlyContinue |
            Where-Object { $_.FullName -like "*GitHub.copilot-chat*\\transcripts\\*" } |
            Sort-Object LastWriteTime -Descending

        $latestTranscript = $transcriptFiles | Select-Object -First 1
        if (-not $latestTranscript) {
            return $null
        }

        $transcriptPath = $latestTranscript.FullName
    }

    $tailLines = Get-Content -Path $transcriptPath -Tail 600 -ErrorAction SilentlyContinue
    if (-not $tailLines) {
        return $null
    }

    $latest = $null
    foreach ($line in $tailLines) {
        try {
            $obj = $line | ConvertFrom-Json -ErrorAction Stop
        }
        catch {
            continue
        }

        if (-not $obj -or [string]$obj.type -ne "assistant.message") {
            continue
        }

        if (-not $obj.data -or -not $obj.data.messageId) {
            continue
        }

        $content = ""
        if ($obj.data.content -is [string]) {
            $content = [string]$obj.data.content
        }
        else {
            try {
                $content = ($obj.data.content | ConvertTo-Json -Depth 6 -Compress)
            }
            catch {
                $content = [string]$obj.data.content
            }
        }

        if ([string]::IsNullOrWhiteSpace($content)) {
            continue
        }

        $latest = [pscustomobject]@{
            Id = [string]$obj.data.messageId
            Content = $content.Trim()
        }
    }

    return $latest
}

function Get-LastLoggedAssistantId {
    if (-not (Test-Path $statePath)) {
        return ""
    }

    try {
        $state = Get-Content -Path $statePath -Raw | ConvertFrom-Json -ErrorAction Stop
        return [string]$state.LastAssistantMessageId
    }
    catch {
        return ""
    }
}

function Set-LastLoggedAssistantId {
    param([string]$MessageId)

    $state = [pscustomobject]@{
        LastAssistantMessageId = $MessageId
        UpdatedAt = (Get-Date).ToString("o")
    }

    $state | ConvertTo-Json | Set-Content -Path $statePath
}

function Start-AssistantReplyWatcher {
    param([string]$LastLoggedId)
    $watcherScriptPath = Join-Path $PSScriptRoot "log_watcher.ps1"
    if (-not (Test-Path $watcherScriptPath)) {
        return
    }

    Start-Process -FilePath "powershell.exe" -WindowStyle Hidden -ArgumentList @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $watcherScriptPath,
        "-LogPath",
        $logPath,
        "-StatePath",
        $statePath,
        "-LastLoggedId",
        $LastLoggedId
    ) | Out-Null
}

function Is-AssistantIdAlreadyLogged {
    param([string]$MessageId)

    if ([string]::IsNullOrWhiteSpace($MessageId)) {
        return $true
    }

    if (-not (Test-Path $logPath)) {
        return $false
    }

    $marker = "AI_REPLY_ID:$MessageId"
    return [bool](Select-String -Path $logPath -Pattern $marker -SimpleMatch -Quiet)
}

function Try-LogLatestAssistantReply {
    param([string]$RawMessage)

    $lastLoggedId = Get-LastLoggedAssistantId

    for ($i = 0; $i -lt 8; $i++) {
        $latestAssistant = Get-LatestAssistantMessage -RawMessage $RawMessage
        if (-not $latestAssistant) {
            Start-Sleep -Seconds 1
            continue
        }

        if ([string]$latestAssistant.Id -eq $lastLoggedId) {
            return
        }

        $replyTimestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        $markerLine = "[$replyTimestamp] AI_REPLY_ID:$([string]$latestAssistant.Id)"
        Add-Content -Path $logPath -Value $markerLine

        $assistantLine = "[$replyTimestamp] AI_REPLY: $($latestAssistant.Content)"
        Add-Content -Path $logPath -Value $assistantLine
        Set-LastLoggedAssistantId -MessageId ([string]$latestAssistant.Id)
        return
    }
}

# Fallback: if Message is not provided, capture stdin content.
if ([string]::IsNullOrWhiteSpace($Message)) {
    $Message = ($input | Out-String).Trim()
}

$rawHookMessage = $Message
$Message = Format-HookMessage -RawMessage $Message -Prefix $LogPrefix

$line = "[$timestamp] ${LogPrefix}: $Message"
Add-Content -Path $logPath -Value $line

if ($LogPrefix -eq "USER") {
    $currentAssistant = Get-LatestAssistantMessage -RawMessage $rawHookMessage
    if ($currentAssistant) {
        Set-LastLoggedAssistantId -MessageId ([string]$currentAssistant.Id)
    }

    Start-AssistantReplyWatcher -LastLoggedId (Get-LastLoggedAssistantId)
}

Try-LogLatestAssistantReply -RawMessage $rawHookMessage
