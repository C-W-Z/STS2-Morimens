using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Powers.Patches;
using Morimens.ExEnergy;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Scaffolding.Characters;

namespace Morimens.Characters;

public abstract class Awaker<TCardPool, TRelicPool, TPotionPool> : ModCharacterTemplate<TCardPool, TRelicPool, TPotionPool>, ISecondaryResourceHookListener, IAwaker
    where TCardPool : CardPoolModel
    where TRelicPool : RelicPoolModel
    where TPotionPool : PotionPoolModel
{
    public virtual int BaseAliemus => 100;
    public virtual int BaseKeyflare => 1000;
    public abstract string ExaltTitle { get; }
    public abstract string ExaltDescription { get; }
    public abstract string SuperExaltTitle { get; }
    public abstract string SuperExaltDescription { get; }
    public virtual async Task Exalt(Player player) { }
    public virtual async Task SuperExalt(Player player) { }

    public decimal ModifyMaxSecondaryResource(SecondaryResourceMaxContext context, decimal amount)
    {
        if (context.Definition.Id == ExEnergyManager.AliemusId)
            return amount + BaseAliemus - (ExEnergyManager.AliemusDefinition.BaseMaxAmount ?? 0);
        return amount;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        Entry.Logger.Debug($"AfterDamageReceived: {dealer?.Name} deals to {target.Name}");
        if (dealer?.Side != CombatSide.Enemy)
            return;

        // 1. MinionLib 正在處理溢傷流程中 (IsHandling.Value == true)
        // 2. 當前觸發 Hook 的目標，正好是第一階段被壓制傷害的玩家 (SuppressedOwner.Value == originalTarget)
        // 3. 這次傷害結算是原版塞進去的 0 傷幽靈結果 (UnblockedDamage <= 0)
        if (MinionGuardianOverkillPatch.IsHandling.Value
            && MinionGuardianOverkillPatch.SuppressedOwner.Value == target
            && result.UnblockedDamage <= 0
            && !result.WasFullyBlocked)
        {
            Entry.Logger.Debug($"阻止 MinionGuardianOverkillPatch 的幽靈傷害觸發狂氣+1");
            return;
        }

        // 获得 1 点狂氣
        // TODO: 会经过 Gain Hook 修正，要改掉
        if (target.Player != null && LocalContext.IsMe(target.Player))
            await SecondaryResourceCmd.Gain(target.Player, ExEnergyManager.AliemusId, 1, this);
    }
}
