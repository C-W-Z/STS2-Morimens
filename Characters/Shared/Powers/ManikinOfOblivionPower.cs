using MegaCrit.Sts2.Core.Entities.Powers;
using Morimens.Core.Power;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Shared.Powers;

[RegisterPower]
public sealed class ManikinOfOblivionPower : AbstractMorimensPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    // TODO: 實作效果，還需要做狂氣爆發相關的 Hook
}
