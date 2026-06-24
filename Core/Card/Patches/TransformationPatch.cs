using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Patching.Models;

namespace Morimens.Core.Card.Patches;

public sealed class TransformationPatch : IPatchMethod
{
    public static string PatchId => "MORIMENS_Transformation";

    public static string Description => "防止變化出已經有的命輪或者任何鑰令";

    public static bool IsCritical => true;

    public static ModPatchTarget[] GetTargets() => [new(typeof(CardFactory), nameof(CardFactory.GetFilteredTransformationOptions))];

    public static void Prefix(CardModel original, ref IEnumerable<CardModel> originalOptions, bool isInCombat)
    {
        Player owner = original.Owner;
        if (owner == null)
            return;

        if (originalOptions.Any())
        {
            var array = PosseSingleton.FilterPosse(owner, originalOptions);
            if (array.Count() != originalOptions.Count())
            {
                Entry.Logger.Debug($"[TransformationPatch] PosseSingleton.FilterPosse original={original.Id.Entry} inCombat={isInCombat} beforeCount={originalOptions.Count()} afterCount={array.Count()} playerNetId={owner.NetId}");
            }
            var array2 = WheelSingleton.FilterInDeck(owner, array);
            if (array2.Count() != array.Count())
            {
                Entry.Logger.Debug($"[TransformationPatch] WheelSingleton.FilterInDeck original={original.Id.Entry} inCombat={isInCombat} beforeCount={array.Count()} afterCount={array2.Count()} playerNetId={owner.NetId}");
            }
            originalOptions = array2;
        }
    }
}
