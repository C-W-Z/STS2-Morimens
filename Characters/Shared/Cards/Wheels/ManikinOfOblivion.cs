using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Morimens.Characters.Shared.Definition;
using Morimens.Core.Card;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Shared.Cards.Wheels;

[RegisterCard(typeof(SharedCardPool))]
public sealed class ManikinOfOblivion() : AbstractWheelCard(CardRarity.Rare)
{
    public override WheelFrameType WheelType => WheelFrameType.Chaos;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

    }
}
