using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using Morimens.Characters;
using Morimens.UI;
using STS2RitsuLib.Patching.Models;
using STS2RitsuLib.Scaffolding.Godot.NodeAttachments;

namespace Morimens.Patches.UI;

public class CharacterSelectBgBtnPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_character_selec_bg_btn";

    public static string Description => "";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCharacterSelectButton), nameof(NCharacterSelectButton.Select))];

    public static void Postfix(NCharacterSelectButton __instance)
    {
        if (__instance.Character is not IAwaker)
            return;

        NCharacterSelectScreen? screen = FindAncestor<NCharacterSelectScreen>(__instance);

        if (screen == null)
            return;

        // 安全地從 RitsuLib 撈出按鈕並刷新
        if (ModNodeAttachmentRegistry.For(Entry.ModId).TryGetAttached(screen, "CharacterBgCycleButton", out CharacterBgCycleButton button))
        {
            // 3. 成功撈出按鈕，執行刷新顯示邏輯
            button.RefreshVisibility();
        }
    }

    private static T? FindAncestor<T>(Node node) where T : Node
	{
		for (Node parent = node.GetParent(); parent != null; parent = parent.GetParent())
		{
			T? val = (parent is T t) ? t : null;
			if (val != null)
			{
				return val;
			}
		}
		return default;
	}
}
