using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using Morimens.UI;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Patches.UI;

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
            // 劫持傳回值，換成我們全域管理器當前指向的 .tscn
            __result = CharacterBgManager.GetCurrentBackgroundPath();
        }
    }
}
