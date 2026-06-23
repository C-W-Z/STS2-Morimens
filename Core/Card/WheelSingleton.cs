using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace Morimens.Core.Card;

[RegisterSingleton]
public sealed class WheelSingleton() : HookedSingletonModel(HookType.Run)
{
    public static bool IsInDeck(CardModel card)
    {
        return card is AbstractWheelCard && card.Owner.Deck.Cards.Any(c => c.Id.Equals(card.Id));
    }

    public override bool ShouldAddToDeck(CardModel card)
    {
        return !IsInDeck(card);
    }
}
