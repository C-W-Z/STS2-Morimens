using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace Morimens.Core.ExEnergy.Actions;

public struct NetExaltAction : INetAction
{
    internal int cost;
    internal bool isOverExalt;

    // 當 Host 或 Client 收到網路封包後，會呼叫此方法將其還原為可在本地排隊執行的 GameAction
    public readonly GameAction ToGameAction(Player player)
    {
        return new ExaltAction(player, cost, isOverExalt);
    }

    // 將數據寫入網路流
    public readonly void Serialize(PacketWriter writer)
    {
        writer.WriteInt(cost);
        writer.WriteBool(isOverExalt);
    }

    // 從網路流讀取數據
    public void Deserialize(PacketReader reader)
    {
        cost = reader.ReadInt();
        isOverExalt = reader.ReadBool();
    }

    public override readonly string ToString()
    {
        return $"NetExaltAction (Cost: {cost}, IsOverExalt: {isOverExalt})";
    }
}
