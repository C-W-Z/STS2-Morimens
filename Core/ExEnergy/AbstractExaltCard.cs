using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.ExEnergy;

public abstract class AbstractExaltCard<TCardPool>() : ModCardTemplate(0, CardType.None, CardRarity.None, TargetType.None), IExaltCard
    where TCardPool : CardPoolModel
{
    public override CardPoolModel Pool => ModelDb.Get<TCardPool>();
    public abstract Task Execute();
}
