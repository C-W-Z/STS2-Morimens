using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using Morimens.Core.Card;
using Morimens.Core.CardTags;

namespace Morimens.Characters.Shared.Cards.Abstracts;

public abstract class AbstractBuffCard(int baseCost, CardType type, CardRarity rarity, TargetType target) : AbstractMorimensCard(baseCost, type, rarity, target)
{
    public override LocString? TypeLocString => new("gameplay_ui", "MORIMENS_CARD_TYPE.BUFF");

    public override int MaxUpgradeLevel => 0; // 無法升級

    protected override HashSet<CardTag> CanonicalTags => [MorimensCardTag.Buff];
}
