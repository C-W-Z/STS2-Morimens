using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class UniqueWheelTransformationPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_UniqueWheelTransformation";

    public static string Description => "防止變化出已經有的命輪";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(CardFactory), nameof(CardFactory.GetFilteredTransformationOptions))];

    public static void Prefix(CardModel original, ref IEnumerable<CardModel> originalOptions, bool isInCombat)
    {
        Player owner = original.Owner;
        if (owner == null)
            return;

        if (originalOptions.Any())
        {
            var array = WheelSingleton.FilterInDeck(owner, originalOptions);
            if (array.Count() != originalOptions.Count())
            {
                Entry.Logger.Debug($"[NoWheelTransformationPatch] original={original.Id.Entry} inCombat={isInCombat} beforeCount={originalOptions.Count()} afterCount={array.Count()} playerNetId={owner.NetId}");
            }
            originalOptions = array;
        }
    }
}
