using MegaCrit.Sts2.Core.Nodes.Cards;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class HideEnergyCostVisualsPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_hide_energy_cost_visuals";

    public static string Description => "隱藏命輪的費用圖標";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCard), nameof(NCard.UpdateEnergyCostVisuals))];

    public static void Postfix(NCard __instance)
    {
        if (!__instance.IsNodeReady() || __instance.Model == null)
            return;

        if (__instance.Model is not AbstractMorimensCard card || !card.IsFullArt)
            return;

        if (card is AbstractWheelCard)
        {
            __instance._energyIcon.Visible = false;
            __instance._unplayableEnergyIcon.Visible = false;
        }
    }
}
