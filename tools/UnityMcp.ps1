param([string]$Method = 'tools/list', [string]$ParamsJson = '{}')
$ErrorActionPreference = 'Stop'
$uri = 'http://127.0.0.1:8080/mcp'
$headers = @{Accept='application/json, text/event-stream'}
function Invoke-Mcp($payload) {
    $response = Invoke-WebRequest -UseBasicParsing -Uri $uri -Method Post -ContentType 'application/json' -Headers $headers -Body ($payload | ConvertTo-Json -Depth 60 -Compress) -TimeoutSec 120
    foreach ($line in ($response.Content -split "`n")) {
        if ($line.StartsWith('data: ')) { $message = $line.Substring(6) | ConvertFrom-Json; if ($null -ne $message.id) { return $message } }
    }
    return ($response.Content | ConvertFrom-Json)
}
$initial = Invoke-WebRequest -UseBasicParsing -Uri $uri -Method Post -ContentType 'application/json' -Headers $headers -Body '{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"Codex-workspace","version":"1.0"}}}' -TimeoutSec 30
$headers['Mcp-Session-Id'] = [string]($initial.Headers['mcp-session-id'] | Select-Object -First 1)
$null = Invoke-WebRequest -UseBasicParsing -Uri $uri -Method Post -ContentType 'application/json' -Headers $headers -Body '{"jsonrpc":"2.0","method":"notifications/initialized"}' -TimeoutSec 30
Invoke-Mcp @{jsonrpc='2.0';id=2;method=$Method;params=($ParamsJson | ConvertFrom-Json)} | ConvertTo-Json -Depth 60
