param(
    [Parameter(Mandatory = $true)]
    [string]$LogPath,

    [Parameter(Mandatory = $true)]
    [string]$StatePath,

    [Parameter(Mandatory = $true)]
    [string]$LastLoggedId
)

function Get-TranscriptPathFromWorkspace {
    $workspaceRoot = Join-Path $env:APPDATA "Code\User\workspaceStorage"
    if (-not (Test-Path $workspaceRoot)) {
        return ""
    }

    $transcriptFiles = Get-ChildItem -Path $workspaceRoot -Recurse -File -Filter "*.jsonl" -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -like "*GitHub.copilot-chat*\transcripts\*" } |
        Sort-Object LastWriteTime -Descending

    $latestTranscript = $transcriptFiles | Select-Object -First 1
    if (-not $latestTranscript) {
        return ""
    }

    return $latestTranscript.FullName
}

function Get-LatestAssistantMessage {
    param([string]$TranscriptPath)

    if ([string]::IsNullOrWhiteSpace($TranscriptPath) -or -not (Test-Path $TranscriptPath)) {
        return $null
    }

    $tailLines = Get-Content -Path $TranscriptPath -Tail 600 -ErrorAction SilentlyContinue
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

for ($i = 0; $i -lt 12; $i++) {
    Start-Sleep -Seconds 1

    $transcriptPath = Get-TranscriptPathFromWorkspace
    if ([string]::IsNullOrWhiteSpace($transcriptPath)) {
        continue
    }

    $latestAssistant = Get-LatestAssistantMessage -TranscriptPath $transcriptPath
    if (-not $latestAssistant) {
        continue
    }

    if ([string]$latestAssistant.Id -eq $LastLoggedId) {
        continue
    }

    $alreadyLogged = $false
    if (Test-Path $StatePath) {
        try {
            $state = Get-Content -Path $StatePath -Raw | ConvertFrom-Json -ErrorAction Stop
            if ([string]$state.LastAssistantMessageId -eq [string]$latestAssistant.Id) {
                $alreadyLogged = $true
            }
        }
        catch {
            $alreadyLogged = $false
        }
    }

    if ($alreadyLogged) {
        return
    }

    $replyTimestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    Add-Content -Path $LogPath -Value "[$replyTimestamp] AI_REPLY_ID:$([string]$latestAssistant.Id)"
    Add-Content -Path $LogPath -Value "[$replyTimestamp] AI_REPLY: $($latestAssistant.Content)"

    $state = [pscustomobject]@{
        LastAssistantMessageId = [string]$latestAssistant.Id
        UpdatedAt = (Get-Date).ToString("o")
    }
    $state | ConvertTo-Json | Set-Content -Path $StatePath
    return
}
