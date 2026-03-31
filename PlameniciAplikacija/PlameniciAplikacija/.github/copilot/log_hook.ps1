param (
    [string]$Message,
    [string]$LogPrefix = "EVENT"
)

$logPath = "C:\Users\lukap\Desktop\Aplikacija za isporuku plamenika\lab-1\agent_log.txt"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# Fallback: if Message is not provided, capture stdin content.
if ([string]::IsNullOrWhiteSpace($Message)) {
    $Message = ($input | Out-String).Trim()
}

if ([string]::IsNullOrWhiteSpace($Message)) {
    $Message = "<empty>"
}

$line = "[$timestamp] ${LogPrefix}: $Message"
Add-Content -Path $logPath -Value $line
