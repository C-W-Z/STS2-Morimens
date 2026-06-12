using Godot;
using static Godot.TextServer;

namespace Morimens.ExEnergy;

public sealed partial class SkillConfirmationDialog : Control
{
    private Panel _backgroundPanel = null!;
    private Label _titleLabel = null!;
    private RichTextLabel _descriptionLabel = null!; // 🔴 1. 將 Label 改為 RichTextLabel
    private Button _confirmButton = null!;
    private Button _cancelButton = null!;
    private Action? _onConfirmAction;

    public override void _Ready()
    {
        _backgroundPanel = new Panel
        {
            Size = new Vector2(450f, 250f),
            Position = new Vector2(735f, 415f)
        };
        AddChild(_backgroundPanel);

        _titleLabel = new Label
        {
            Position = new Vector2(30f, 20f),
            Size = new Vector2(390f, 30f),
            HorizontalAlignment = HorizontalAlignment.Center
        };
        _backgroundPanel.AddChild(_titleLabel);

        // 🔴 2. 實例化改為 RichTextLabel
        _descriptionLabel = new RichTextLabel
        {
            Position = new Vector2(40f, 65f),
            Size = new Vector2(370f, 100f),
            BbcodeEnabled = true,   // 🔴 核心：啟用 BBCode 標籤支援
            ScrollActive = false,   // 關閉滾動條
            AutowrapMode = TextServer.AutowrapMode.WordSmart // 自動換行
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
