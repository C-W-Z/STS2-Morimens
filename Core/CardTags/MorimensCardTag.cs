using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Core.CardTags;

[RegisterOwnedCardTag(nameof(Heal))]
public static class MorimensCardTag
{
    public static readonly CardTag Heal = ModContentRegistry.GetQualifiedCardTagId(Entry.ModId, nameof(Heal)).GetModCardTag();
}
