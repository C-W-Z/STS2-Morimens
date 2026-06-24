using Godot;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Scaffolding.Godot.NodeAttachments;
using Morimens.Core.Character;
using Morimens.Core.ExEnergy.UI;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Morimens.Core.ExEnergy;

public static class ExEnergyRegistry
{
    public static SecondaryResourceDefinition AliemusDefinition { get; private set; } = null!;
    public static SecondaryResourceDefinition KeyflareDefinition { get; private set; } = null!;
    public static string AliemusId { get; private set; } = string.Empty;
    public static string KeyflareId { get; private set; } = string.Empty;
    public const string ConfirmationUiLocalId = "confirmation_ui";

    public static void Register()
    {
        var registry = RitsuLibFramework.GetSecondaryResourceRegistry(Entry.ModId);

        // 註冊狂氣
        AliemusDefinition = registry.Register("aliemus", new SecondaryResourceDefinition(
            defaultAmount: 0, baseMaxAmount: 100,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.Run,
            smallIconPath: $"{Entry.ImagePath}/Shared/ui/AliemusText.png",
            largeIconPath: $"{Entry.ImagePath}/Shared/ui/Aliemus.png"
        ));
        AliemusId = AliemusDefinition.Id;

        // 註冊鑰令
        KeyflareDefinition = registry.Register("keyflare", new SecondaryResourceDefinition(
            defaultAmount: 0, baseMaxAmount: 1000,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.Run,
            smallIconPath: $"{Entry.ImagePath}/Shared/ui/KeyflareText.png",
            largeIconPath: $"{Entry.ImagePath}/Shared/ui/Keyflare.png"
        ));
        KeyflareId = KeyflareDefinition.Id;

        registry.RegisterCombatUiAlwaysVisibleWhen(AliemusDefinition.LocalId, context => context.Player.Character is IAwaker);
        registry.RegisterCombatUiAlwaysVisibleWhen(KeyflareDefinition.LocalId, context => context.Player.Character is IAwaker);

        // 註冊 狂氣 UI
        registry.RegisterCombatUi("aliemus_combat_ui", parent =>
        {
            var row = NSecondaryResourceCounter.Create(AliemusDefinition, CreateDefaultStyle(Colors.Yellow));
            var energyCounter = parent.GetNode<Control>("%EnergyCounterContainer");
            row.Position = energyCounter.Position + new Vector2(-80, -240);
            ExEnergyUiController.SetupExEnergyUi(row); // 轉交給 UI 控制器處理
            return row;
        }, ctx => ctx.Node.Bind(ctx.Player));

        // TODO: NSecondaryResourceIcon._Ready()時將Icon改成各個鑰令的圖案
        // 註冊 鑰令 UI
        registry.RegisterCombatUi("keyflare_combat_ui", parent =>
        {
            var row = NSecondaryResourceCounter.Create(KeyflareDefinition, CreateDefaultStyle(Colors.Silver));
            var energyCounter = parent.GetNode<Control>("%EnergyCounterContainer");
            row.Position = energyCounter.Position + new Vector2(-80, -120);
            ExEnergyUiController.SetupExEnergyUi(row); // 轉交給 UI 控制器處理
            return row;
        }, ctx => ctx.Node.Bind(ctx.Player));

        RegisterSkillConfirmationUi();
    }

    private static SecondaryResourceCounterStyle CreateDefaultStyle(Color color) => new()
    {
        FontSize = 32,
        PositiveColor = color,
        AmountLabelOffset = new Vector2(100, 20),
        IconStyle = SecondaryResourceIconStyle.Default with
        {
            Size = new Vector2(80, 80),
            HoverTip = SecondaryResourceHoverTipStyle.Default with { ScreenOffset = new Vector2(240, -120) }
        }
    };

    private static void RegisterSkillConfirmationUi()
    {
        ModNodeAttachmentRegistry.For(Entry.ModId)
            .RegisterReadyChild<NCombatUi, ConfirmationUi>(
                ConfirmationUiLocalId, static _ => new ConfirmationUi(),
                static (parent, node) => { node.Position = Vector2.Zero; node.Size = parent.Size; },
                new NodeAttachmentOptions { Name = "ConfirmationUi", Order = 99, DuplicatePolicy = NodeAttachmentDuplicatePolicy.ReuseExistingByName });
    }
}
