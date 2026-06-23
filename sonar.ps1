# =========================================================
# 1. 自動讀取 .env 檔案並載入為環境變數
# =========================================================
$envFilePath = Join-Path $PSScriptRoot ".env"
if (Test-Path $envFilePath) {
    Get-Content $envFilePath | ForEach-Object {
        # 跳過註解和空行，只抓取 KEY=VALUE
        if ($_ -notmatch "^\s*#" -and $_ -match "^\s*([^=]+)\s*=\s*(.*)\s*$") {
            $key = $Matches[1].Trim()
            $value = $Matches[2].Trim()
            Set-Item "env:$key" $value
        }
    }
}

# =========================================================
# 2. 原本的防錯與安全協定
# =========================================================
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12 -bor [Net.SecurityProtocolType]::Tls13

$token = $env:SONAR_TOKEN
if ([string]::IsNullOrEmpty($token)) {
    Write-Error "❌ 錯誤：找不到環境變數或 .env 中的 SONAR_TOKEN！"
    exit 1
}

# =========================================================
# 3. 執行掃描
# =========================================================
dotnet sonarscanner begin `
    /o:"c-w-z" `
    /k:"C-W-Z_STS2-Morimens" `
    /d:sonar.host.url="https://sonarcloud.io" `
    /d:sonar.token="$token"

if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet build
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet sonarscanner end /d:sonar.token="$token"
