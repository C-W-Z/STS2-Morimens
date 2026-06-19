using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using Morimens.Core.Utils;

namespace Morimens.Core.Menu.CharacterBg.UI;

public sealed partial class CharacterBgCycleButton : Button
{
    public override void _Ready()
    {
        // 基礎外觀設定（可依需求調整）
        Flat = true;
        CustomMinimumSize = new Vector2(40f, 40f);
        FocusMode = FocusModeEnum.None;

        // 載入按鈕圖標（例如一個循環箭頭的圖示）
        Icon = ResourceLoader.Load<Texture2D>($"{Entry.ImagePath}/shared/ui/switch_button.png");

        ButtonDown += OnClicked;
        RefreshVisibility();
    }

    public void RefreshVisibility()
    {
        var screen = NodeUtils.FindAncestor<NCharacterSelectScreen>(this);
        if (screen is null)
            return;

        // 只有選中你的 Mod 角色時，這個切換按鈕才會露出來
        Visible = screen._selectedButton is not null &&
                CharacterBgManager.IsSupportedCharacter(screen._selectedButton.Character);

        Entry.Logger.Debug($"screen._selectedButton is null: {screen._selectedButton is null}");
        Entry.Logger.Debug($"screen._selectedButton.Character = {screen._selectedButton?.Character.Id.Entry}");
        Entry.Logger.Debug($"CharacterBgCycleButton.Visible = {Visible}");
    }

    private void OnClicked()
    {
        var screen = NodeUtils.FindAncestor<NCharacterSelectScreen>(this);
        if (screen is null || screen._selectedButton is null)
            return;

        CharacterBgManager.CycleToNext(screen._selectedButton.Character);

        // 利用原版私有方法重新選取一次當前角色，這會逼迫遊戲重新加載背景場景
        screen.SelectCharacter(screen._selectedButton, screen._selectedButton.Character);
    }
}
