using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Menu.CharacterBg.Patches;

public class CharacterSelectBgPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_character_selec_bg";

    public static string Description => "替換Morimens角色選擇頁面的背景";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectBg), MethodType.Getter)];

    public static void Postfix(CharacterModel __instance, ref string __result)
    {
        if (CharacterBgManager.IsSupportedCharacter(__instance))
        {
            // 根據不同的角色實例，去存檔讀取屬於它的專屬背景
            __result = CharacterBgManager.GetCurrentBackgroundPath(__instance) ?? __result;
        }
    }
}
