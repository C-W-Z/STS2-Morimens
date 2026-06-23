using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Nodes.Cards;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class FullCardArtPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_full_card_art";

    public static string Description => "全圖卡面";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCard), nameof(NCard.Reload))];

    public static void Postfix(NCard __instance)
    {
        if (!__instance.IsNodeReady() || __instance.Model == null)
            return;

        if (__instance.Model is not AbstractMorimensCard card || !card.IsFullArt)
            return;

        __instance._portraitBorder.Visible = false;
        __instance._portrait.Visible = false;
        __instance._frame.Visible = true;
        __instance._ancientPortrait.Visible = true;
        __instance._ancientBorderGlassOverlay.Visible = false;
        __instance._ancientBorder.Visible = false;
        __instance._ancientTextBg.Visible = false; // 這個可以考慮開啟，但需要解決Error問題
        __instance._ancientBanner.Visible = false;
        __instance._banner.Visible = false;
        if (__instance.Visibility != ModelVisibility.Visible)
        {
            __instance._canvasGroupMaskBlurMaterial ??= PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_blur_material.tres");
            __instance._portraitCanvasGroup.Material = __instance._canvasGroupMaskBlurMaterial;
        }
        else
        {
            __instance._canvasGroupMaskMaterial ??= PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_material.tres");
            __instance._portraitCanvasGroup.Material = __instance._canvasGroupMaskMaterial;
        }
        __instance._frame.Texture = __instance.Model.Frame;
        // 這裡開啟會遇到 CardModel.AncientTextBgPath 的 get 裡的 throw error
        // __instance._ancientTextBg.Texture = __instance.Model.AncientTextBg;
        __instance._ancientPortrait.Texture = __instance.Model.Portrait;
        __instance._frame.Material = __instance.Model.FrameMaterial;
        __instance.ReloadOverlay();
    }
}
