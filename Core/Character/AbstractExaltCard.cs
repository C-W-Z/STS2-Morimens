using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.Character;

public abstract class AbstractExaltCard() : ModCardTemplate(0, CardType.None, CardRarity.None, TargetType.None)
{
    public string GetDescription() => GetDescriptionForPile(PileType.Hand);
    public abstract Task Execute();
}

public abstract class AbstractExaltCard<TCardPool> : AbstractExaltCard
    where TCardPool : CardPoolModel
{
    protected AbstractExaltCard() : base()
    {
        _pool = ModelDb.Get<TCardPool>();
    }
}
