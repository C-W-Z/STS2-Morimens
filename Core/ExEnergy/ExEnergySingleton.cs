using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Powers.Patches;
using Morimens.Core.Character;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace Morimens.Core.ExEnergy;

// TODO: 測試這些到底要不要加上 LocalContext.IsMe(target.Player)
[RegisterSingleton]
public sealed class ExEnergySingleton() : HookedSingletonModel(HookType.Combat), ISecondaryResourceHookListener
{
    // 根據角色改變基礎狂氣/銀鑰
    public decimal ModifyMaxSecondaryResource(SecondaryResourceMaxContext context, decimal amount)
    {
        if (context.Player.Character is not IAwaker awaker)
            return amount;
        if (context.Definition.Id == ExEnergyRegistry.AliemusId)
            return amount + awaker.BaseAliemus - (ExEnergyRegistry.AliemusDefinition.BaseMaxAmount ?? 0);
        if (context.Definition.Id == ExEnergyRegistry.KeyflareId)
            return amount + awaker.BaseKeyflare - (ExEnergyRegistry.KeyflareDefinition.BaseMaxAmount ?? 0);
        return amount;
    }

    // 2倍上限狂氣，2倍或以上銀鑰
    public decimal ModifySecondaryResourceGain(SecondaryResourceContext context, decimal amount)
    {
        if (context.Player.Character is not IAwaker awaker)
            return amount;
        if (context.Definition.Id == ExEnergyRegistry.AliemusId)
        {
            int currentAmt = SecondaryResourceCmd.Get(context.Player, ExEnergyRegistry.AliemusId);
            int maxAmt = SecondaryResourceCmd.GetMax(context.Player, ExEnergyRegistry.AliemusId) ?? awaker.BaseAliemus;
            if (currentAmt + amount > 2 * maxAmt)
            {
                Entry.Logger.Debug($"[ExEnergySingleton] currentAmt = {currentAmt}; maxAmt = {maxAmt}; 獲得 {2 * maxAmt - currentAmt} 點狂氣");
                return 2 * maxAmt - currentAmt;
            }
        }
        else if (context.Definition.Id == ExEnergyRegistry.KeyflareId)
        {
            int currentAmt = SecondaryResourceCmd.Get(context.Player, ExEnergyRegistry.KeyflareId);
            int maxAmt = SecondaryResourceCmd.GetMax(context.Player, ExEnergyRegistry.KeyflareId) ?? awaker.BaseKeyflare;
            int maxAmtMultiply = 2; // TODO: 如果有隱世轉輪則為3
            if (currentAmt + amount > maxAmtMultiply * maxAmt)
            {
                Entry.Logger.Debug($"[ExEnergySingleton] currentAmt = {currentAmt}; maxAmt = {maxAmt}; maxAmtMultiply = {maxAmtMultiply}; 獲得 {maxAmtMultiply * maxAmt - currentAmt} 點銀鑰");
                return maxAmtMultiply * maxAmt - currentAmt;
            }
        }
        return amount;
    }

    // 被攻擊獲得1點狂氣
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        Entry.Logger.Debug($"[ExEnergySingleton] AfterDamageReceived: {dealer?.Name} deals to {target.Name}");
        if (dealer?.Side != CombatSide.Enemy)
            return;

        if (target.Player?.Character is not IAwaker)
            return;

        // 1. MinionLib 正在處理溢傷流程中 (IsHandling.Value == true)
        // 2. 當前觸發 Hook 的目標，正好是第一階段被壓制傷害的玩家 (SuppressedOwner.Value == originalTarget)
        // 3. 這次傷害結算是原版塞進去的 0 傷幽靈結果 (UnblockedDamage <= 0)
        if (MinionGuardianOverkillPatch.IsHandling.Value
            && MinionGuardianOverkillPatch.SuppressedOwner.Value == target
            && result.UnblockedDamage <= 0
            && !result.WasFullyBlocked)
        {
            Entry.Logger.Debug($"[ExEnergySingleton] 阻止 MinionGuardianOverkillPatch 的幽靈傷害觸發狂氣+1");
            return;
        }

        // 获得 1 点狂氣
        // TODO: 会经过 Gain Hook 修正，要改掉
        await SecondaryResourceCmd.Gain(target.Player, ExEnergyRegistry.AliemusId, 1, this);
    }

    // 回合結束獲得5狂氣
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        foreach (var p in participants)
        {
            Entry.Logger.Debug($"[ExEnergySingleton] AfterSideTurnEnd participant: {p.Name}, is Awaker = {p.Player?.Character is IAwaker}");
            if (p.Player?.Character is IAwaker)
            {
                // TODO: 会经过 Gain Hook 修正，要改掉
                await SecondaryResourceCmd.Gain(p.Player!, ExEnergyRegistry.AliemusId, 5, null);
            }
        }
    }

    // 打牌耗費獲得銀鑰
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Character is not IAwaker awaker)
            return;
        await SecondaryResourceCmd.Gain(cardPlay.Card.Owner, ExEnergyRegistry.KeyflareId, cardPlay.Resources.EnergySpent * awaker.KeyflareGain, this);
    }
}
