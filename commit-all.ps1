param(
    [Parameter(Mandatory=$true)]
    [string]$CommitMessage
)

# Get the current directory
$currentDir = Get-Location

# Add all changes
Write-Host "Adding all changes..." -ForegroundColor Yellow
git add .

# Show status before commit
Write-Host "`nCurrent status:" -ForegroundColor Cyan
git status

# Commit with message
Write-Host "`nCommitting changes..." -ForegroundColor Green
git commit -m $CommitMessage

# Show latest commit
Write-Host "`nLatest commit:" -ForegroundColor Magenta
git log -1

Write-Host "`nCommit completed successfully!" -ForegroundColor Green
