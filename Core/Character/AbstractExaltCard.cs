using MegaCrit.Sts2.Core.Entities.Cards;
using Morimens.Core.Card;

namespace Morimens.Core.Character;

public abstract class AbstractExaltCard() : AbstractMorimensCard(0, CardType.None, CardRarity.None, TargetType.None, false)
{
    public override bool IsFullArt => false;
    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
    protected override bool IsPlayable => false;
    public override int MaxUpgradeLevel => 0; // 無法升級
    public override bool CanBeEnchanted => false; // 不能被附魔
    public string GetDescription() => GetDescriptionForPile(PileType.Hand);
    public abstract Task Execute();
}
