using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Morimens.Characters.Doll.Minions;
using Morimens.Core.Power;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Powers;

[RegisterPower]
public sealed class MinionDeathToDoomEnemiesPower : AbstractMorimensPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (Owner.Player is null || wasRemovalPrevented || creature.Monster is not DollMinion)
            return;

        await PowerCmd.Apply<DoomPower>(choiceContext, CombatState.HittableEnemies, Amount, Owner, null);
    }
}
