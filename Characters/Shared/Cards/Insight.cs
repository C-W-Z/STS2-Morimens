using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Morimens.Characters.Shared.Definition;
using Morimens.Core.Card;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Morimens.Characters.Shared.Cards;

[RegisterCard(typeof(SharedCardPool))]
public sealed class Insight() : AbstractMorimensCard(0, CardType.Skill, CardRarity.Token, TargetType.None)
{
    public override CardAssetProfile AssetProfile
    {
        get
        {
            return base.AssetProfile with
            {
                FramePath = $"{Entry.ImagePath}/Shared/ui/card_frame.png",
                FrameMaterial = MaterialUtils.CreateUnmodulatedHsvShaderMaterial(),
            };
        }
    }

    // 無法升級
    public override int MaxUpgradeLevel => 0;

    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1),
        new CardsVar(1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}
