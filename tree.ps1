# =========================================================
# 1. 定義支援過濾的 Tree 渲染函式（完全相容 PowerShell 5.1 語法）
# =========================================================
function Get-GodotTree($Path, $Indent = "") {
    # 抓取該層目錄下的所有檔案與資料夾，並排除 .uid 和 .import 檔案
    $items = Get-ChildItem -Path $Path | Where-Object { $_.Name -notmatch '\.(uid|import)$' }
    $count = $items.Count

    for ($i = 0; $i -lt $count; $i++) {
        $item = $items[$i]
        $isLast = ($i -eq $count - 1)

        if ($isLast) {
            $junction = "└── "
        } else {
            $junction = "├── "
        }

        Write-Host "$Indent$junction$($item.Name)"

        # 如果是資料夾，就遞迴進去繼續畫
        if ($item.PSIsContainer) {
            # ✨ 修正：改用傳統 if-else 判定下一層的縮排線條
            if ($isLast) {
                $nextIndent = $Indent + "    "
            } else {
                $nextIndent = $Indent + "│   "
            }
            Get-GodotTree $item.FullName $nextIndent
        }
    }
}

# =========================================================
# 2. 指定只對 "Core" 和 "Characters" 資料夾執行
# =========================================================
"Core", "Characters" | Where-Object { Test-Path $_ } | ForEach-Object {
    Write-Host $_ -ForegroundColor Cyan  # 用青色醒目顯示根資料夾名稱
    Get-GodotTree $_ " "
}
