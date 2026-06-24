using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace Morimens.Core.Card;

[RegisterSingleton]
public sealed class PosseSingleton() : HookedSingletonModel(HookType.Run)
{
    public override bool ShouldAddToDeck(CardModel card)
    {
        return !IsPosse(card);
    }

    public override CardCreationOptions ModifyCardRewardCreationOptionsLate(Player player, CardCreationOptions options)
    {
        // 無視 options.Flags
        var previous = options.CardPoolFilter;
        if (options.CustomCardPool != null)
        {
            CardModel[] array = [.. options.GetPossibleCards(player).Where(Combined)];
            if (array.Length == 0)
                return options;
            return options.WithCustomPool(array, options.RarityOdds);
        }
        List<CardPoolModel> list = [.. options.CardPools];
        return options.WithCardPools(list, Combined);

        bool Combined(CardModel c)
        {
            if (previous != null && !previous(c))
                return false;
            if (IsPosse(c))
                return false;
            return true;
        }
    }

    public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions)
    {
        // 無視 options.Flags
        return FilterCardCreationResults(player, cardRewardOptions);
    }

    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        return options.Where(c => !IsPosse(c));
    }

    public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
    {
        FilterCardCreationResults(player, cards);
    }

    internal static bool IsPosse(CardModel card)
    {
        return card is AbstractPosseCard;
    }

    internal static bool FilterCardCreationResults(Player player, List<CardCreationResult> results)
    {
        bool hasRemoved = false;
        for (int num = results.Count - 1; num >= 0; num--)
        {
            CardModel card = results[num].Card;
            if (IsPosse(card))
            {
                results.RemoveAt(num);
                hasRemoved = true;
            }
        }
        return hasRemoved;
    }

    internal static IEnumerable<CardModel> FilterPosse(Player player, IEnumerable<CardModel> candidates)
    {
        IEnumerable<CardModel> array = candidates.Where(c => !IsPosse(c));
        if (!array.Any() && candidates.Any())
        {
            Entry.Logger.Warn($"[PosseSingleton] FilterInDeck fallback: all {candidates.Count()} candidates blocked for player {player.NetId}; restoring unfiltered pool (may allow duplicate Unique).");
        }
        if (!array.Any())
            return candidates;
        return array;
    }
}
