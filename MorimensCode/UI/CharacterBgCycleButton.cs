using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace Morimens.UI;

public sealed partial class CharacterBgCycleButton : Button
{
    // 快取官方的私有反射方法與欄位，用於通知介面重繪背景
    private static readonly MethodInfo? SelectCharacterMethod =
        AccessTools.Method(typeof(NCharacterSelectScreen), "SelectCharacter", [typeof(NCharacterSelectButton), typeof(CharacterModel)]);

    private static readonly FieldInfo? SelectedButtonField =
        AccessTools.Field(typeof(NCharacterSelectScreen), "_selectedButton");

    public override void _Ready()
    {
        // 基礎外觀設定（可依需求調整）
        Flat = true;
        CustomMinimumSize = new Vector2(40f, 40f);
        FocusMode = FocusModeEnum.None;

        // 載入按鈕圖標（例如一個循環箭頭的圖示）
        Icon = ResourceLoader.Load<Texture2D>("res://Morimens/images/ui/switch_button.png");

        ButtonDown += OnClicked;
        RefreshVisibility();
    }

    public void RefreshVisibility()
    {
        var screen = FindAncestorScreen();
        if (screen == null)
            return;

        // 只有選中你的 Mod 角色時，這個切換按鈕才會露出來
        Visible = SelectedButtonField?.GetValue(screen) is NCharacterSelectButton selectedBtn &&
                CharacterBgManager.IsSupportedCharacter(selectedBtn.Character);
    }

    private void OnClicked()
    {
        var screen = FindAncestorScreen();
        if (screen == null) return;

        if (SelectedButtonField?.GetValue(screen) is not NCharacterSelectButton selectedBtn)
            return;

        // 1. 切換到下一個背景路徑
        CharacterBgManager.CycleToNext();

        // 2. 核心魔法：利用原版私有方法重新選取一次當前角色，這會逼迫遊戲重新加載背景場景！
        SelectCharacterMethod?.Invoke(screen, [selectedBtn, selectedBtn.Character]);
    }

    private NCharacterSelectScreen? FindAncestorScreen()
    {
        Node? parent = GetParent();
        while (parent != null && parent is not NCharacterSelectScreen)
        {
            parent = parent.GetParent();
        }
        return parent as NCharacterSelectScreen;
    }
}
