using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MinionLib.Utilities;
using MorimensDoll.Minion;

namespace MorimensDoll.Cards;

public abstract class AbstractMinionCard(int baseCost, CardType type, CardRarity rarity, TargetType target) : AbstractDollCard(baseCost, type, rarity, target)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected async Task<List<Creature>> CheckMinionExistAndSummon(PlayerChoiceContext choiceContext)
    {
        List<Creature>? pets = PetsOrderAccessor.GetRawPetsList(Owner);
        if (pets == null || pets.Count == 0)
        {
            await DollMinionCmd.SummonHalfHP(choiceContext, Owner, this);
            return [];
        }
        return pets;
    }
}
