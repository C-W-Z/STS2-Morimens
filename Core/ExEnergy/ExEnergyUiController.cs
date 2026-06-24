using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;
using Morimens.Core.Character;
using Morimens.Core.ExEnergy.Actions;
using Morimens.Core.ExEnergy.UI;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Scaffolding.Godot.NodeAttachments;
using STS2RitsuLib.Ui.Toast;

namespace Morimens.Core.ExEnergy;

internal static class ExEnergyUiController
{
    private const string ToastLocTable = "gameplay_ui";

    private static readonly LocString AliemusInsufficientTitle = new(ToastLocTable, "MORIMENS_ALIEMUS_INSUFFICIENT.title");
    private static readonly LocString AliemusInsufficientBody = new(ToastLocTable, "MORIMENS_ALIEMUS_INSUFFICIENT.description");
    private static readonly LocString KeyflareInsufficientTitle = new(ToastLocTable, "MORIMENS_KEYFLARE_INSUFFICIENT.title");
    private static readonly LocString KeyflareInsufficientBody = new(ToastLocTable, "MORIMENS_KEYFLARE_INSUFFICIENT.description");

    internal static void SetupExEnergyUi(NSecondaryResourceCounter counter)
    {
        counter.Ready += () => OnCounterReady(counter);
    }

    private static void OnCounterReady(NSecondaryResourceCounter counter)
    {
        string? energyId = GetResourceDefinitionId(counter);
        if (string.IsNullOrEmpty(energyId)) return;

        var realIcon = FindChildIcon(counter);
        if (realIcon is null) return;

        realIcon.GuiInput += (@event) => OnIconGuiInput(@event, counter, realIcon, energyId);
    }

    private static void OnIconGuiInput(InputEvent @event, NSecondaryResourceCounter counter, NSecondaryResourceIcon icon, string energyId)
    {
        if (@event is not InputEventMouseButton { Pressed: true }) return;

        icon.AcceptEvent();

        // 基礎安全性檢查：戰鬥中、且必須是玩家的回合
        if (!CombatManager.Instance.IsInProgress || CombatManager.Instance._state?.CurrentSide != CombatSide.Player) return;

        Player? player = LocalContext.GetMe(CombatManager.Instance._state);
        if (player is null || player.Character is not IAwaker awaker) return;

        // 1. 獲取當前能量數據
        int currentAmount = SecondaryResourceCmd.Get(player, energyId);
        int baseMaxAmount = SecondaryResourceCmd.GetMax(player, energyId) ?? (energyId == ExEnergyRegistry.AliemusId ? awaker.BaseAliemus : awaker.BaseKeyflare);

        // 2. 根據點擊的是狂氣還是鑰令，走不同的 UI 呈現策略
        if (energyId == ExEnergyRegistry.AliemusId)
        {
            HandleAliemusClick(player, awaker, counter, currentAmount, baseMaxAmount);
        }
        else if (energyId == ExEnergyRegistry.KeyflareId)
        {
            HandleKeyflareClick(player, awaker, counter, currentAmount, baseMaxAmount);
        }
    }

    private static void HandleAliemusClick(Player player, IAwaker awaker, NSecondaryResourceCounter counter, int currentAmount, int baseMaxAmount)
    {
        // 核心公式策略計算
        bool isOverExalt = currentAmount >= baseMaxAmount * 2;
        int requiredAmount = isOverExalt ? baseMaxAmount * 2 : baseMaxAmount + Math.Max(0, currentAmount - baseMaxAmount) / 2;
        bool isSufficient = currentAmount >= requiredAmount;

        var combatUi = FindParentCombatUi(counter);
        if (combatUi is null || !TryGetConfirmationDialog(combatUi, out var dialog))
            return;

        string title = isOverExalt ? awaker.OverExalt.Title : awaker.Exalt.Title;
        string description = isOverExalt ? awaker.OverExalt.GetDescription() : awaker.Exalt.GetDescription();

        dialog.Open(title, description, isSufficient, () =>
        {
            if (isSufficient)
            {
                // UI 職責結束：打包同步動作，推入 STS2 排隊執行鏈
                var action = new ExaltAction(player, requiredAmount, isOverExalt);
                RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(action);
            }
            else
            {
                ShowInsufficientToast(AliemusInsufficientTitle, AliemusInsufficientBody, requiredAmount);
            }
        });
    }

    private static void HandleKeyflareClick(Player player, IAwaker awaker, NSecondaryResourceCounter counter, int currentAmount, int baseMaxAmount)
    {
        int requiredAmount = baseMaxAmount; // 永遠消耗 1 倍上限
        bool isSufficient = currentAmount >= requiredAmount;

        var combatUi = FindParentCombatUi(counter);
        if (combatUi is null || !TryGetConfirmationDialog(combatUi, out var dialog)) return;

        // 目前暫用 OverExalt 代表，未來改為讀取裝備的 Posse 卡片
        string title = awaker.OverExalt.Title;
        string description = awaker.OverExalt.GetDescription();

        dialog.Open(title, description, isSufficient, () =>
        {
            if (isSufficient)
            {
                // 未來你的 PosseAction 推送點
                // var action = new PosseAction(player, requiredAmount);
                // RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(action);
                Entry.Logger.Debug("鑰令點擊確認 - 待接入 PosseAction");
            }
            else
            {
                ShowInsufficientToast(KeyflareInsufficientTitle, KeyflareInsufficientBody, requiredAmount);
            }
        });
    }

    // ======= 輔助用 Godot 節點尋找方法 =======
    private static string? GetResourceDefinitionId(NSecondaryResourceCounter counter) =>
        counter.GetType().GetField("_definition", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(counter) is SecondaryResourceDefinition def ? def.Id : null;

    private static NSecondaryResourceIcon? FindChildIcon(Node parent) =>
        parent.GetChildren().OfType<NSecondaryResourceIcon>().FirstOrDefault();

    private static NCombatUi? FindParentCombatUi(Node? node)
    {
        while (node is not null && node is not NCombatUi) node = node.GetParent();
        return node as NCombatUi;
    }

    private static bool TryGetConfirmationDialog(NCombatUi combatUi, out ConfirmationUi dialog) =>
        ModNodeAttachmentRegistry.For(Entry.ModId).TryGetAttached(combatUi, ExEnergyRegistry.ConfirmationUiLocalId, out dialog);

    private static void ShowInsufficientToast(LocString titleLoc, LocString bodyLoc, int cost)
    {
        bodyLoc.AddObj("Cost", cost);
        RitsuToastService.Show(new RitsuToastRequest(
            body: bodyLoc.GetFormattedText(), title: titleLoc.GetFormattedText(),
            level: RitsuToastLevel.Warning, durationSeconds: 3.0, animationOverride: RitsuToastAnimationPreset.FadeSlide
        ));
    }
}
