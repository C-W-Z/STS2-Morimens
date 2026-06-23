using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using Morimens.Core.Utils;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Morimens.Core.Card;

public abstract class AbstractMorimensCard(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true) : ModCardTemplate(baseCost, type, rarity, target, showInCardLibrary)
{
    public virtual bool IsFullArt => true;
    public virtual bool HideTypePlaque => false; // 卡面上 CardType 的標籤
    public virtual LocString? TypeLocString => null;

    protected virtual string GetMissingCardFileName() => $"missing{(IsFullArt ? "_full": "")}.png";

    protected CardAssetProfile? _cachedAssetProfile;

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
                targetPath = $"{Entry.ImagePath}/{folder}/cards/{GetMissingCardFileName()}";
            }

            if (IsFullArt)
            {
                _cachedAssetProfile = new CardAssetProfile(
                    PortraitPath: targetPath,
                    FramePath: $"{Entry.ImagePath}/Shared/ui/card_frame{(Type == CardType.Power ? "_power" : "")}.png",
                    FrameMaterial: MaterialUtils.CreateUnmodulatedHsvShaderMaterial()
                );
            }
            else
            {
                _cachedAssetProfile = new CardAssetProfile(PortraitPath: targetPath);
            }
            return _cachedAssetProfile;
        }
    }
}
