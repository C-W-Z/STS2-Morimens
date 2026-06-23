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

    private const string MetaTypePlaqueName = "morimens_type_plaque";
    private const string MetaOriginPosName = "morimens_type_plaque_orig_pos";
    private const string MetaOriginVisibleName = "morimens_type_plaque_orig_visible";

    // --- Prefix 前置清理 ---
    // 在原廠和所有 Mod 執行前，先把我們「上一次」借用這個節點時留下的修改還原，讓這一輪的主人能乾淨地計算
    public static void Prefix(NCard __instance)
    {
        // 檢查這張卡片節點，先前是不是被我們忘卻前夜「污染」過
        if (__instance.HasMeta(MetaTypePlaqueName) && __instance.GetMeta(MetaTypePlaqueName).AsBool())
        {
            // 還原成我們動手「前」該節點專屬的原始座標（這可能是原版，也可能是其他 Mod 調整後的精美座標）
            if (__instance.HasMeta(MetaOriginPosName))
                __instance._typePlaque.Position = __instance.GetMeta(MetaOriginPosName).As<Vector2>();

            // 還原成我們動手「前」該節點專屬的原始可見度
            if (__instance.HasMeta(MetaOriginVisibleName))
                __instance._typePlaque.Visible = __instance.GetMeta(MetaOriginVisibleName).AsBool();

            // 清理完畢，移除我們蓋的動態印章，拍拍屁股離場
            __instance.SetMeta(MetaTypePlaqueName, false);
            __instance.RemoveMeta(MetaOriginPosName);
            __instance.RemoveMeta(MetaOriginVisibleName);
        }
    }

    // --- Postfix 後置修改 ---
    public static void Postfix(NCard __instance)
    {
        // 如果這張卡不是忘卻前夜的卡，我們「絕對、完全」不要碰它！
        // 因為 Prefix 已經在這一格清乾淨了，此時上面的座標是原版或其他 Mod 剛剛執行完的完美心血。
        if (__instance.Model is not AbstractMorimensCard card)
        {
            return;
        }

        // 走到這裡，代表這必定是忘卻前夜的卡片
        // 且此時原廠與其他 Mod 對這張卡片的計算已經全部結束。

        // 把別人（或原廠）在此時此刻、對這張卡算好的原始 pristine 狀態記在節點的 Meta 裡
        // 改用 Meta 記錄，每張卡片各記各的，就不需要全域靜態變數了，也支援不同卡片有不同的初始高度！
        if (!__instance.HasMeta(MetaTypePlaqueName) || !__instance.GetMeta(MetaTypePlaqueName).AsBool())
        {
            // 保留你的 Debug Log，方便你在控制台查看原始位置
            Entry.Logger.Debug($"[FullCardArtTypePlaquePatch] Original Pos: {__instance._typePlaque.Position}");

            __instance.SetMeta(MetaOriginPosName, __instance._typePlaque.Position);
            __instance.SetMeta(MetaOriginVisibleName, __instance._typePlaque.Visible);
            __instance.SetMeta(MetaTypePlaqueName, true); // 蓋上髒品印章，通知 Prefix 下次回收時要幫忙還原
        }

        // 執行忘卻前夜的特定卡片隱藏邏輯
        if (card.HideTypePlaque)
        {
            __instance._typePlaque.Visible = false;
            return;
        }

        // 執行忘卻前夜的全圖卡移位邏輯
        if (card.IsFullArt)
        {
            __instance._typePlaque.Visible = true;
            Vector2 currentPos = __instance._typePlaque.Position;

            // 採用你測試出來最完美的絕對 Y 座標（原廠 1f + 向下偏移 170f = 171f）
            currentPos.Y = 171f;

            __instance._typePlaque.Position = currentPos;
        }
        else
        {
            // 如果是忘卻前夜的卡，但既不隱藏也不是全圖卡，就讓它維持原本被記錄下來的外觀（不干涉）
            __instance._typePlaque.Visible = __instance.GetMeta(MetaOriginVisibleName).AsBool();
            __instance._typePlaque.Position = __instance.GetMeta(MetaOriginPosName).As<Vector2>();
        }
    }
}
