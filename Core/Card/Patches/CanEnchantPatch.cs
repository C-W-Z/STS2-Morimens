using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class CanEnchantPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_can_enchant";

    public static string Description => "讓某些牌(例如增益和命輪)不能被附魔";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchant))];

    public static void Postfix(CardModel card, ref bool __result)
    {
        // 如果原廠或其他 Mod 已經判定這張卡「不能」附魔（__result 為 false）
        // 我們就直接放行，因為原廠註明了「不要移除限制」，我們不能把它改回 true。
        if (!__result) return;

        // 檢查這張卡片是不是我們「忘卻前夜」的卡片且在卡片上設定了「禁止附魔」
        if (card is AbstractMorimensCard morimensCard && !morimensCard.CanBeEnchanted)
            // 補上限制，強制改為 false
            __result = false;
    }
}
