using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using Morimens.Core.CardTags;

namespace Morimens.Core.Card;

public abstract class AbstractBuffCard(int baseCost, CardType type, CardRarity rarity, TargetType target) : AbstractMorimensCard(baseCost, type, rarity, target)
{
    public override LocString? TypeLocString => new("gameplay_ui", "MORIMENS_CARD_TYPE.BUFF");

    protected override HashSet<CardTag> CanonicalTags => [MorimensCardTag.Buff];

    public override int MaxUpgradeLevel => 0; // 無法升級

    public override bool CanBeEnchanted => false; // 不能被附魔
}
