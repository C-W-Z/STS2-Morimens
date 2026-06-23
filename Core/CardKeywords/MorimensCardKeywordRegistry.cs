using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Morimens.Core.CardKeywords;

[RegisterOwnedCardKeyword(nameof(Wheel), IconPath = $"{Entry.ImagePath}/Shared/ui/wheel.png", CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.None)]
// 由于写法和ritsulib标准不同，这里不能用static静态类！！
public sealed class MorimensCardKeywordRegistry
{
    public static readonly CardKeyword Wheel = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Wheel)).GetModCardKeyword();
}
