using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Morimens.Characters.Shared.Definition;
using Morimens.Core.Card;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Shared.Cards.Posse;

[RegisterCard(typeof(SharedCardPool))]
public sealed class AMousesWisdom : AbstractPosseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];

    public override async Task Execute()
    {

    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}
