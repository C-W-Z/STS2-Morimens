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

    private static Vector2? NormalPosition = null;

    public static void Postfix(NCard __instance)
    {
        NormalPosition ??= __instance._typePlaque.Position;

        if (__instance.Model is not AbstractMorimensCard card || !card.IsFullArt)
        {
            // 原版和其他模組的牌複用到我們卡牌的Plaque時位置要調整回來
            __instance._typePlaque.Position = NormalPosition.Value;
            return;
        }

        Vector2 currentPos = NormalPosition.Value;
        // Godot 的座標系中，(0,0) 在左上角。
        currentPos.Y += 170f;
        __instance._typePlaque.Position = currentPos;
    }
}
