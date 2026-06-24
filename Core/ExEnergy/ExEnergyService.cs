using MegaCrit.Sts2.Core.Entities.Players;
using STS2RitsuLib.Combat.SecondaryResources;
using Morimens.Core.Character;

namespace Morimens.Core.ExEnergy;

// 所有的 Hook 觸發（Before/After）、扣除次要資源、播放特效，通通都在這裡安全同步執行
public static class ExEnergyService
{
    /// <summary>
    /// 狂氣爆發與超限爆發的核心執行點 (全網同步)
    /// </summary>
    public static async Task ExecuteExaltAsync(Player player, IAwaker awaker, int cost, bool isOverExalt)
    {
        // 1. 扣除資源 (因為已經在同步的 GameAction 內部，全體聯機玩家會在此時同步扣除)
        await SecondaryResourceCmd.Lose(player, ExEnergyRegistry.AliemusId, cost);

        // 2. 觸發 BeforeExalt Hook (未來可以在這裡擴充遺物或能力觸發)
        // foreach (var subscriber in GetSubscribers(player)) await subscriber.BeforeExalt(...);

        // 3. 執行角色身上的變身邏輯
        if (isOverExalt)
        {
            Entry.Logger.Debug($"[Sync] 玩家 {player.NetId} 執行了超限爆發 OverExalt");
            await awaker.OverExalt.Execute();
        }
        else
        {
            Entry.Logger.Debug($"[Sync] 玩家 {player.NetId} 執行了普通狂氣爆發 Exalt");
            await awaker.Exalt.Execute();
        }

        // 4. 觸發 AfterExalt Hook
        // foreach (var subscriber in GetSubscribers(player)) await subscriber.AfterExalt(...);
    }

    /// <summary>
    /// 鑰令釋放的核心執行點 (全網同步) - 留給未來 Posse 擴充
    /// </summary>
    public static async Task ExecutePosseAsync(Player player, IAwaker awaker, int cost)
    {
        await SecondaryResourceCmd.Lose(player, ExEnergyRegistry.KeyflareId, cost);
        // 執行你的 Posse 卡牌化效果...
    }
}
