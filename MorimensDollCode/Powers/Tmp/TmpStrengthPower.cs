using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace MorimensDoll.Powers.Tmp;

[RegisterPower]
public class TmpStrengthPower : ModTemporaryAppliedPowerTemplate<AbstractModel, StrengthPower>
{
    // 自定义图标路径
#pragma warning disable RITSU013
    public override PowerAssetProfile AssetProfile => new(
        IconPath: ImageHelper.GetImagePath("atlases/power_atlas.sprites/strength.tres"),
        BigIconPath: ImageHelper.GetImagePath("powers/strength.png")
    );
#pragma warning restore RITSU013

    protected override bool IsPositive => true; // 正面效果还是负面

    public override bool AllowNegative => false; // 是否可以是負的

    protected override bool UntilEndOfOtherSideTurn => false; // 为 true 时，在另一方回合结束时过期；否则在拥有者一方回合结束时过期。

    protected override int LastForXExtraTurns => 0; // 额外持续回合数
}
