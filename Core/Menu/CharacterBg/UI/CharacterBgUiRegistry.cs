using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using STS2RitsuLib.Scaffolding.Godot.NodeAttachments;

namespace Morimens.Core.Menu.CharacterBg.UI;

public static class CharacterBgUiRegistry
{
    public static void Register()
    {
        ModNodeAttachmentRegistry.For(Entry.ModId)
            .RegisterReadyChild<NCharacterSelectScreen, CharacterBgCycleButton>(
                "CharacterBgCycleButton",
                _ => new CharacterBgCycleButton(),
                (screen, button) =>
                {
                    // 定位在資訊欄的右上角
                    button.AnchorLeft = 1f;
                    button.AnchorTop = 0f;
                    button.AnchorRight = 1f;
                    button.AnchorBottom = 0f;
                    button.OffsetLeft = -100f;
                    button.OffsetTop = 20f;
                    button.OffsetRight = -20f;
                    button.OffsetBottom = 60f;
                },
                new NodeAttachmentOptions
                {
                    Name = "CharacterBgCycleButton",
                    DuplicatePolicy = NodeAttachmentDuplicatePolicy.ReuseExistingByName
                });
    }
}
