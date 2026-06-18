using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.Card;

public abstract class AbstractMorimensCard(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true) : ModCardTemplate(baseCost, type, rarity, target, showInCardLibrary)
{
    private CardAssetProfile? _cachedAssetProfile;

    public override CardAssetProfile AssetProfile
    {
        get
        {
            // 🚀 核心魔法：如果已經算過了，直接秒回傳！效能直接變成 O(1)
            if (_cachedAssetProfile != null)
            {
                return _cachedAssetProfile;
            }

            // --- 只有第一次被存取時，才會執行下面這堆重度邏輯 ---

            // 1. 自動抓取當前卡牌類別的命名空間，例如 "Morimens.Characters.Doll.Cards"
            string? ns = GetType().Namespace;
            string folder = "Shared"; // 預設防呆

            if (!string.IsNullOrEmpty(ns))
            {
                // 依據 "." 切開字串：["Morimens", "Characters", "Doll", "Cards"]
                string[] segments = ns.Split('.');

                // 檢查是否符合標準的角色路徑結構，並撈出第三個段落 "Doll"
                if (segments.Length >= 3 && segments[1] == "Characters")
                {
                    folder = segments[2];
                }
            }

            // 2. 組合出動態路徑：$"{Entry.ResPath}/images/cards/Doll/{GetType().Name}.png"
            string targetPath = $"{Entry.ResPath}/images/cards/{folder}/{GetType().Name}.png";

            // 3. 檢查檔案是否存在
            if (!Godot.ResourceLoader.Exists(targetPath))
            {
                Entry.Logger.VeryDebug($"Missing card art for '{GetType().Name}'. Falling back to placeholder. (Expected path: {targetPath})");
                targetPath = $"{Entry.ResPath}/images/cards/missing.png";
            }

#pragma warning disable RITSU013
            // 3. 把計算好的結果存進快取欄位中
            _cachedAssetProfile = new CardAssetProfile(PortraitPath: targetPath);
#pragma warning restore RITSU013

            // 4. 回傳新建立的快取
            return _cachedAssetProfile;
        }
    }
}
