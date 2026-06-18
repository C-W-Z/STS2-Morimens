using MegaCrit.Sts2.Core.Entities.Cards;
using Morimens.Core.Utils;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.Card;

public abstract class AbstractMorimensCard(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true) : ModCardTemplate(baseCost, type, rarity, target, showInCardLibrary)
{
    private CardAssetProfile? _cachedAssetProfile;

    public override CardAssetProfile AssetProfile
    {
        get
        {
            if (_cachedAssetProfile != null)
                return _cachedAssetProfile;

            string folder = GetType().GetCharacterFolderName();

            // 例如："res://Morimens/images/Doll/cards/Strike.png"
            string targetPath = $"{Entry.ImagePath}/{folder}/cards/{GetType().Name}.png";

            if (!Godot.ResourceLoader.Exists(targetPath))
            {
                Entry.Logger.Debug($"Missing card art for '{GetType().Name}'. Falling back to placeholder. (Expected path: {targetPath})");
                targetPath = $"{Entry.ImagePath}/{folder}/cards/missing.png";
            }

            _cachedAssetProfile = new CardAssetProfile(PortraitPath: targetPath);
            return _cachedAssetProfile;
        }
    }
}
