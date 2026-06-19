using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core.Character.Patches;

public static class CharacterPatchRegistry
{
    public static void Register()
    {
        ModPatcher hookPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "character-patches");
        hookPatcher.RegisterPatch<CharacterModelHookPatch>();
        if (!hookPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical character-patches failed!");
    }
}
