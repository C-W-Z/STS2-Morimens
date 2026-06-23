using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class FullCardArtTypePlaquePatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_full_card_art_type_plaque";

    public static string Description => "";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCard), nameof(NCard.UpdateTypePlaqueSizeAndPosition))];

    private static float? NormalPositionY = null;

    public static void Postfix(NCard __instance)
    {
        if (NormalPositionY == null)
        {
            // (-30.5, 1)
            Entry.Logger.Debug($"[FullCardArtTypePlaquePatch] {__instance._typePlaque.Position}");
            NormalPositionY ??= __instance._typePlaque.Position.Y;
        }

        if (__instance.Model is not AbstractMorimensCard card || !card.IsFullArt)
        {
            // 原版和其他模組的牌複用到我們卡牌的Plaque時位置和可見度要調整回來
            Vector2 tmp = __instance._typePlaque.Position;
            tmp.Y = NormalPositionY.Value;
            __instance._typePlaque.Position = tmp;
            __instance._typePlaque.Visible = true;
            return;
        }

        if (card.HideTypePlaque)
        {
            __instance._typePlaque.Visible = false;
            return;
        }

        __instance._typePlaque.Visible = true;
        Vector2 currentPos = __instance._typePlaque.Position;
        // Godot 的座標系中，(0,0) 在左上角。
        currentPos.Y = 171f; // NormalPositionY + 170f
        __instance._typePlaque.Position = currentPos;
    }
}
