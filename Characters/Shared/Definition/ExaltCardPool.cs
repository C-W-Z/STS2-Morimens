using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Morimens.Characters.Shared.Definition;

[RegisterSharedCardPool]
public sealed class ExaltCardPool : TypeListCardPoolModel
{
    private static readonly Material? PoolFrameTintMaterial = MaterialUtils.CreateUnmodulatedHsvShaderMaterial();

    // Title 和 EnergyColorName 是池子的稳定标识，不是玩家看到的角色名。
    // 自定义角色卡、遗物、药水池保持同一个 EnergyColorName，方便实验室和文本统一读取能量图标。
    public override string Title => "MorimensExalt";
    public override string EnergyColorName => "colorless"; // 使用遊戲的無色能量
    public override Color DeckEntryCardColor => new("A3A3A3FF"); // 跟 Colorless 一樣
    public override Material? PoolFrameMaterial => PoolFrameTintMaterial;
    public override bool IsColorless => true;
}
