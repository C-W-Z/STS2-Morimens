using MegaCrit.Sts2.Core.Nodes.Cards;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class CustomCardTypeTextPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_custom_card_type_text";

    public static string Description => "自訂卡片類型文字";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(NCard), nameof(NCard.UpdateTypePlaque))];

    public static void Postfix(NCard __instance)
    {
        if (__instance.Model is not AbstractMorimensCard card || card.TypeLocString is null)
            return;

        __instance._typeLabel.SetTextAutoSize(card.TypeLocString.GetFormattedText());
    }
}
