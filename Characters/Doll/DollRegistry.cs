using MinionLib.Layout;
using Morimens.Characters.Doll.Minions;

namespace Morimens.Characters.Doll;

public static class DollRegistry
{
    public static void Register()
    {
        // 註冊佈局器，設定 priority: 1 確保它在 DefaultMinionLayout (0) 之前執行
        MinionLayoutManager.Register(new DollMinionLayout(), priority: 1);
    }
}
