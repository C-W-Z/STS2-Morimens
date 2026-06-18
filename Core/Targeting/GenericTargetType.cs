using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MinionLib.Targeting;
using MinionLib.Targeting.Utilities;

namespace Morimens.Core.Targeting;

public static class GenericTargetType
{
    /// <summary>
    /// 所有友方生物
    /// </summary>
    public static readonly TargetType AllAllies = CustomTargetTypeManager.Register(
        new LambdaTargetType(
            false, // IsSingleTarget = false
            target => target is { IsAlive: true, Side: CombatSide.Player },
            (card, target) => target is { IsAlive: true, Side: CombatSide.Player }
        ),
        Entry.ModId,
        nameof(AllAllies)
    );
}
