using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Morimens.Characters.Shared.Definition;
using Morimens.Characters.Shared.Powers;
using Morimens.Core.Card;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Shared.Cards.Wheels;

[RegisterCard(typeof(SharedCardPool))]
public sealed class ManikinOfOblivion() : AbstractWheelCard(CardRarity.Rare)
{
    public override WheelFrameType WheelType => WheelFrameType.Chaos;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ManikinOfOblivionPower>(20m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ManikinOfOblivionPower>(choiceContext, Owner.Creature, DynamicVars["ManikinOfOblivionPower"].BaseValue, null, this);
    }
}
