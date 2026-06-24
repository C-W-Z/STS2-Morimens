using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Morimens.Characters.Shared.Definition;
using Morimens.Core.Character;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Definition;

[RegisterCard(typeof(ExaltCardPool))]
public sealed class DollOverExalt : AbstractExaltCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(10m),
        ModCardVars.Int("Aliemus", 20),
        ModCardVars.Int("Turn", 3),
    ];

    public override async Task Execute()
    {
        await ModelDb.Get<DollExalt>().Execute();
        // TODO: 接下來三回合開始時全體回10血
    }
}
