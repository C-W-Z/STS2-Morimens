using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core.Card.Patches;

public static class CardPatchRegistry
{
    public static void Register()
    {
        ModPatcher cardPathcer = RitsuLibFramework.CreatePatcher(Entry.ModId, "card-patches");
        cardPathcer.RegisterPatch<FullCardArtPatch>();
        cardPathcer.RegisterPatch<FullCardArtOverlayPatch>();
        cardPathcer.RegisterPatch<FullCardArtTypePlaquePatch>();
        if (!cardPathcer.PatchAll())
            throw new InvalidOperationException("Morimens critical card-patches failed!");
    }
}
