using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Minion.Patches;

public sealed class PersonalHivePowerPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_personal_hive_power";

    public static string Description => "因為PersonalHivePower硬編碼只判斷Osty，需要擴充成所有Pets以防止被Minion攻擊時產生NullReferenceException";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(PersonalHivePower), nameof(PersonalHivePower.AfterDamageReceived))];

    public static void Prefix(ref Creature? dealer)
    {
        if (dealer is not null && dealer.Monster is not Osty && dealer.PetOwner?.Creature is not null)
        {
            dealer = dealer.PetOwner.Creature;
        }
    }
}
