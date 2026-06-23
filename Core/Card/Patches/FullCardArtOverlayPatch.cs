using MegaCrit.Sts2.Core.Nodes.Cards;
using Morimens.Characters.Shared.Cards;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class FullCardArtOverlayPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_full_card_art_overlay";

    public static string Description => "";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCard), nameof(NCard.ReloadOverlay))];

    public static void Postfix(NCard __instance)
    {
        if (__instance.Model is not AbstractMorimensCard card || !card.IsFullArt)
            return;

        __instance._frame.Visible = true;
        __instance._ancientBorder.Visible = false;
    }
}
