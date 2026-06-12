using Godot;
using static Godot.TextServer;

namespace Morimens.ExEnergy;

public sealed partial class SkillConfirmationDialog : Control
{
    private Panel _backgroundPanel = null!;
    private Label _titleLabel = null!;       // 🔴 新增：技能標題
    private Label _descriptionLabel = null!; // 🔴 新增：技能描述
    private Button _confirmButton = null!;
    private Button _cancelButton = null!;
    private Action? _onConfirmAction;

    public override void _Ready()
    {
        // 1. 建立置中面板（加大尺寸以容納描述）
        _backgroundPanel = new Panel
        {
            Size = new Vector2(450f, 250f),
            Position = new Vector2(735f, 415f)
        };
        AddChild(_backgroundPanel);

        // 2. 建立標題 Label
        _titleLabel = new Label
        {
            Position = new Vector2(30f, 20f),
            Size = new Vector2(390f, 30f),
            HorizontalAlignment = HorizontalAlignment.Center
        };
        // 這裡可以透過設定主題或微調字型讓標題變粗/變大
        _backgroundPanel.AddChild(_titleLabel);

        // 3. 建立描述 Label
        _descriptionLabel = new Label
        {
            Position = new Vector2(40f, 65f),
            Size = new Vector2(370f, 100f),
            AutowrapMode = AutowrapMode.Word, // 讓長文字自動縮放或換行
            HorizontalAlignment = HorizontalAlignment.Center
        };
        _backgroundPanel.AddChild(_descriptionLabel);

        // 4. 確認按鈕
        _confirmButton = new Button
        {
            Text = "確認釋放",
            Position = new Vector2(50f, 180f),
            Size = new Vector2(120f, 40f)
        };
        _confirmButton.Pressed += OnConfirmPressed;
        _backgroundPanel.AddChild(_confirmButton);

        // 5. 取消按鈕
        _cancelButton = new Button
        {
            Text = "取消",
            Position = new Vector2(280f, 180f),
            Size = new Vector2(120f, 40f)
        };
        _cancelButton.Pressed += OnCancelPressed;
        _backgroundPanel.AddChild(_cancelButton);

        Visible = false;
    }

    // 🔴 讓 Open 方法接收 標題、描述 與 回調動作
    public void Open(string title, string description, Action onConfirm)
    {
        _titleLabel.Text = title;
        _descriptionLabel.Text = description;
        _onConfirmAction = onConfirm;
        Visible = true;
    }

    private void OnConfirmPressed()
    {
        Visible = false;
        _onConfirmAction?.Invoke();
    }

    private void OnCancelPressed()
    {
        Visible = false;
    }
}
