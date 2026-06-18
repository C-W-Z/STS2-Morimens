using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Morimens.Characters.Doll.Minions;
using Morimens.Core.Power;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Powers;

[RegisterPower]
public sealed class SummonOnTurnEndPower : AbstractDollPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner) || Owner.Player is null)
            return;

        await DollMinionCmd.Summon(choiceContext, Owner.Player, null);
        await PowerCmd.TickDownDuration(this);
    }
}
