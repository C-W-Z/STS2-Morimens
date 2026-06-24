using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Morimens.Core.ExEnergy;
using STS2RitsuLib.Cards.DynamicVars;

namespace Morimens.Characters.Doll.Definition;

public sealed class MorimensOverExaltDoll : AbstractExaltCard<DollCardPool>
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(10m),
        ModCardVars.Int("Turn", 3),
    ];

    public override async Task Execute()
    {
        await ModelDb.Get<MorimensExaltDoll>().Execute();
        // TODO: 接下來三回合開始時全體回10血
    }
}
