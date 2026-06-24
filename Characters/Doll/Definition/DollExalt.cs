using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Morimens.Characters.Shared.Definition;
using Morimens.Core.Character;
using Morimens.Core.ExEnergy;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Definition;

[RegisterCard(typeof(ExaltCardPool))]
public sealed class DollExalt : AbstractExaltCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(10m),
        ModCardVars.Int("Aliemus", 20),
    ];

    public override async Task Execute()
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await CreatureCmd.TriggerAnim(Owner.Creature, DollSpine.State.ExSkill, DollSpine.ExSkillAnimDelay);
        // 驅散友方易傷狀態，全體友方回10血，全體友方+20狂
        foreach (var ally in CombatState.Allies)
            await PowerCmd.Remove<VulnerablePower>(ally);
        foreach (var ally in CombatState.Allies)
            await CreatureCmd.Heal(ally, DynamicVars.Heal.BaseValue);
        foreach (var ally in CombatState.Players)
            if (!LocalContext.IsMe(ally) && ally.Character is IAwaker)
                await SecondaryResourceCmd.Gain(ally, ExEnergyRegistry.AliemusId, DynamicVars["Aliemus"].IntValue, this);
    }
}
