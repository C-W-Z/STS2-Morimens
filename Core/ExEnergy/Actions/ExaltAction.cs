using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Morimens.Core.Character;

namespace Morimens.Core.ExEnergy.Actions;

public sealed class ExaltAction(Player player, int cost, bool isOverExalt) : GameAction
{
    // 綁定發起這項動作的玩家 NetId
    public override ulong OwnerId => Player.NetId;

    // 設定動作類型。通常玩家回合主動執行的動作設定為 CombatPlayPhaseOnly
    public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

    public Player Player { get; } = player;
    public int Cost { get; } = cost;
    public bool IsOverExalt { get; } = isOverExalt;

    /// <summary>
    /// 核心同步執行點：不論是 Host 還是 Client，最終都會在同一個時間點安全地觸發此方法
    /// </summary>
    protected override async Task ExecuteAction()
    {
        if (Player.Character is IAwaker awaker)
        {
            // 所有的 Hook 觸發（Before/After）、扣除次要資源、播放特效，通通都在這裡安全同步執行
            await ExEnergyService.ExecuteExaltAsync(Player, awaker, Cost, IsOverExalt);
        }
    }

    // 當 Client 提交此 Action 時，底層會透過這個方法轉型成網路封包傳給 Host
    public override INetAction ToNetAction()
    {
        return new NetExaltAction
        {
            cost = Cost,
            isOverExalt = IsOverExalt
        };
    }

    public override string ToString()
    {
        return $"ExaltAction [Player: {Player.NetId}] Cost: {Cost}, IsOverExalt: {IsOverExalt}";
    }
}
