using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Morimens.Characters.Doll.CardTags;
using Morimens.Characters.Doll.Minions;
using Morimens.Core.Card;

namespace Morimens.Characters.Doll.Cards.Abstracts;

public abstract class AbstractDollMinionCard(int baseCost, CardType type, CardRarity rarity, TargetType target) : AbstractMorimensCard(baseCost, type, rarity, target)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected async Task<List<DollMinion>> CheckMinionExistAndSummon(PlayerChoiceContext choiceContext)
    {
        ArgumentNullException.ThrowIfNull(Owner);
        List<DollMinion> minions = DollMinionCmd.GetAllDollMinions(Owner);
        if (minions.Count == 0)
            await DollMinionCmd.Summon(choiceContext, Owner, this);
        return minions;
    }
}
