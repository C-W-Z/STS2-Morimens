using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using Morimens.Core.CardKeywords;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Morimens.Core.Card;

public abstract class AbstractWheelCard(CardRarity rarity) : AbstractMorimensCard(0, CardType.None, rarity, TargetType.None)
{
    public override bool HideTypePlaque => true;

    public override LocString? TypeLocString => new("gameplay_ui", "MORIMENS_CARD_TYPE.WHEEL");

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MorimensCardKeywords.Wheel.GetModCardKeyword()];

    public override bool CanBeGeneratedInCombat => false;

    public override bool CanBeGeneratedByModifiers => false;

    protected override bool IsPlayable => false;

    public override int MaxUpgradeLevel => 0; // 無法升級

    public abstract WheelFrameType WheelType { get; }

    public override CardAssetProfile AssetProfile
    {
        get
        {
            if (_cachedAssetProfile != null)
                return _cachedAssetProfile;

            // 例如："res://Morimens/images/Shared/cards/RewindingTime.png"
            string targetPath = $"{Entry.ImagePath}/Shared/cards/{GetType().Name}.png";

            if (!Godot.ResourceLoader.Exists(targetPath))
            {
                Entry.Logger.Debug($"Missing card art for '{GetType().Name}'. Falling back to placeholder. (Expected path: {targetPath})");
                targetPath = $"{Entry.ImagePath}/Shared/cards/{GetMissingCardFileName()}";
            }

            string? framePath = GetFramePath();

#pragma warning disable RITSU013
            if (framePath != null)
            {
                _cachedAssetProfile = new CardAssetProfile(
                    PortraitPath: targetPath,
                    FramePath: $"{Entry.ImagePath}/Shared/ui/{framePath}",
                    FrameMaterial: MaterialUtils.CreateUnmodulatedHsvShaderMaterial()
                );
            }
            else
            {
                _cachedAssetProfile = new CardAssetProfile(PortraitPath: targetPath);
            }
#pragma warning restore RITSU013

            return _cachedAssetProfile;
        }
    }

    protected virtual string? GetFramePath()
    {
        switch (WheelType)
        {
            case WheelFrameType.Caro:
                return "wheel_frame_caro.png";
            case WheelFrameType.Chaos:
                return "wheel_frame_chaos.png";
            case WheelFrameType.Ocean:
                return "wheel_frame_ocean.png";
            case WheelFrameType.Ultra:
                return "wheel_frame_ultra.png";
            case WheelFrameType.SSR:
                return "wheel_frame_ssr.png";
            case WheelFrameType.SR:
                return "wheel_frame_sr.png";
            default:
                return null;
        }
    }
}
