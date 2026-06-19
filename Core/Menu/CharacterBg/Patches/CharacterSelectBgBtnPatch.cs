using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using Morimens.Core.Character;
using Morimens.Core.Menu.CharacterBg.UI;
using Morimens.Core.Utils;
using STS2RitsuLib.Patching.Models;
using STS2RitsuLib.Scaffolding.Godot.NodeAttachments;

namespace Morimens.Core.Menu.CharacterBg.Patches;

public sealed class CharacterSelectBgBtnPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_character_selec_bg_btn";

    public static string Description => "讓切換角色選擇頁面背景的按鈕只會在Morimens角色選擇頁面顯示";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCharacterSelectButton), nameof(NCharacterSelectButton.Select))];

    public static void Postfix(NCharacterSelectButton __instance)
    {
        NCharacterSelectScreen? screen = NodeUtils.FindAncestor<NCharacterSelectScreen>(__instance);

        if (screen is null)
            return;

        // 安全地從 RitsuLib 撈出按鈕並刷新
        if (ModNodeAttachmentRegistry.For(Entry.ModId).TryGetAttached(screen, "CharacterBgCycleButton", out CharacterBgCycleButton button))
        {
            // 執行刷新顯示邏輯
            button.RefreshVisibility();
        }
    }
}
