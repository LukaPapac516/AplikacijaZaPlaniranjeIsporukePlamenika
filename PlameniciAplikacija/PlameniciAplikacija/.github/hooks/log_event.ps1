param(
    [string]$EventName = "Unknown",
    [string]$LogPath = "C:\Users\lukap\Desktop\Aplikacija za isporuku plamenika\lab-1\agent_log.txt"
)

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$raw = [Console]::In.ReadToEnd()
$message = ""

if (-not [string]::IsNullOrWhiteSpace($raw)) {
    try {
        $payload = $raw | ConvertFrom-Json -ErrorAction Stop

        if ($payload.prompt) {
            $message = [string]$payload.prompt
        }
        elseif ($payload.toolName) {
            $message = "tool=$($payload.toolName)"
            if ($payload.toolInput) {
                $message = "$message input=$([string]($payload.toolInput | ConvertTo-Json -Compress))"
            }
        }
        else {
            $message = [string]($payload | ConvertTo-Json -Compress)
        }
    }
    catch {
        $message = $raw.Trim()
    }
}

if ([string]::IsNullOrWhiteSpace($message)) {
    $message = "<empty>"
}

# Keep one log line per event.
$message = $message -replace "\r?\n", " "
$line = "[$timestamp] ${EventName}: $message"
Add-Content -Path $LogPath -Value $line
