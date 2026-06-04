using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MinionLib.Minion;
using MinionLib.Powers.Patches;
using MorimensDoll.Powers;

namespace MorimensDoll.Patches;

[HarmonyPatch]
public static class MinionGuardianPatch
{
    // 透過字串和參數型別，精準定位原模組的 private static bool IsFrontGuardian 方法
    [HarmonyTargetMethod]
    private static System.Reflection.MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(MinionGuardianOverkillPatch), "IsFrontGuardian", [typeof(Creature)]);
    }

    // 使用 Postfix（後置補丁）來擴充原方法的傳回值
    [HarmonyPostfix]
    private static void Postfix(Creature creature, ref bool __result)
    {
        // 如果原本的檢測已經是 true（代表它是原生的 MinionGuardianPower），就不管它
        if (__result) return;

        // 如果不是，檢查它有沒有掛載我們自訂的新 Power
        // 並且同樣要符合「它是隨從，且站在前排」的規則
        if (creature.GetPower<DollMinionGuardianPower>() != null &&
            (creature.Monster is not MinionModel minion || minion.Position == MinionPosition.Front))
        {
            __result = true; // 強行改成 true，欺騙原模組的溢出演算法！
        }
    }
}
