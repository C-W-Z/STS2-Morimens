using MegaCrit.Sts2.Core.Entities.Cards;
using Morimens.Core.Card;

namespace Morimens.Characters.Doll.Cards;

public abstract class AbstractDollCard(int baseCost, CardType type, CardRarity rarity, TargetType target) : AbstractMorimensCard(baseCost, type, rarity, target)
{
    protected override string GetMissingCardFileName()
    {
        if (IsFullArt)
        {
            if (Type == CardType.Attack)
                return "missing_full_attack.png";
            else
                return "missing_full_skill.png";
        }
        return "missing.png";
    }
}
