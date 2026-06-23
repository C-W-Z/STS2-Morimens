using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using Morimens.Core.CardKeywords;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Morimens.Core.Card;

public abstract class AbstractWheelCard(CardRarity rarity) : AbstractMorimensCard(0, CardType.Power, rarity, TargetType.None)
{
    public override bool HideTypePlaque => true;

    public override LocString? TypeLocString => new("gameplay_ui", "MORIMENS_CARD_TYPE.WHEEL");

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MorimensCardKeywords.WheelOfDestiny.GetModCardKeyword()];

    public override bool CanBeGeneratedInCombat => false;

    public override bool CanBeGeneratedByModifiers => false;

    protected override bool IsPlayable => false;

    public override int MaxUpgradeLevel => 0; // 無法升級

    public override bool CanBeEnchanted => false; // 不能被附魔

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
        return WheelType switch
        {
            WheelFrameType.Caro => "wheel_frame_caro.png",
            WheelFrameType.Chaos => "wheel_frame_chaos.png",
            WheelFrameType.Ocean => "wheel_frame_ocean.png",
            WheelFrameType.Ultra => "wheel_frame_ultra.png",
            WheelFrameType.SSR => "wheel_frame_ssr.png",
            WheelFrameType.SR => "wheel_frame_sr.png",
            _ => null,
        };
    }

    // 戰鬥開始時自動打出
    public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner && player.PlayerCombatState!.TurnNumber <= 1)
            await CardCmd.AutoPlay(choiceContext, this, null, AutoPlayType.Default, false, true);
    }
}
