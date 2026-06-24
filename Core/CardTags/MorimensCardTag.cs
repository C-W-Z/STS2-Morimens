using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Core.CardTags;

[RegisterOwnedCardTag(nameof(Buff))]
[RegisterOwnedCardTag(nameof(Heal))]
public static class MorimensCardTag
{
    public static readonly CardTag Buff = ModContentRegistry.GetQualifiedCardTagId(Entry.ModId, nameof(Buff)).GetModCardTag(); // 增益卡
    public static readonly CardTag Heal = ModContentRegistry.GetQualifiedCardTagId(Entry.ModId, nameof(Heal)).GetModCardTag(); // 所有回血的卡
}
