using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Patches.Hooks;

public class CharacterModelHookPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_character_model_hooks";

    public static string Description => "將Player.Character放入CombatState.IterateHookListeners()的結果中，以觸發CharacterModel身上的Hooks";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(CombatState), nameof(CombatState.IterateHookListeners))];

    public static IEnumerable<AbstractModel> Postfix(IEnumerable<AbstractModel> __result, CombatState __instance)
    {
        // 先把原版迭代器原本就會吐出的所有 Listeners 依序回傳
        foreach (var item in __result)
        {
            yield return item;
        }

        // 補上原版遺漏的 Player.Character
        if (__instance.Players is not null)
        {
            for (int i = 0; i < __instance.Players.Count; i++)
            {
                var player = __instance.Players[i];

                // 嚴格遵循原版的安全檢查：過濾 null、檢查 IsActiveForHooks, PlayerCombatState
                if (player is not null &&
                    player.IsActiveForHooks &&
                    player.Character is not null &&
                    player.PlayerCombatState is not null)
                {
                    yield return player.Character;
                }
            }
        }
    }
}
