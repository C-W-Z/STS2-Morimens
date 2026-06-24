using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.Character;

public abstract class AbstractExaltCard<TCardPool> : ModCardTemplate, IExaltCard
    where TCardPool : CardPoolModel
{
    protected AbstractExaltCard() : base(0, CardType.None, CardRarity.None, TargetType.None)
    {
        _pool = ModelDb.Get<TCardPool>();
    }

    public abstract Task Execute();
}
