using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;

namespace Morimens.Core.Card;

public abstract class AbstractPosseCard() : AbstractMorimensCard(0, CardType.None, CardRarity.None, TargetType.None)
{
    public override LocString? TypeLocString => new("gameplay_ui", "MORIMENS_CARD_TYPE.POSSE");
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    protected override bool IsPlayable => false; // 可以打出
    public override int MaxUpgradeLevel => 0; // 無法升級
    public override bool CanBeEnchanted => false; // 不能被附魔
    public abstract Task Execute();
}
