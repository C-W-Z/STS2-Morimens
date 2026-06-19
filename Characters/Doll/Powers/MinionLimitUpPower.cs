using MegaCrit.Sts2.Core.Entities.Powers;
using Morimens.Core.Power;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Powers;

[RegisterPower]
public sealed class MinionLimitUpPower : AbstractMorimensPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
