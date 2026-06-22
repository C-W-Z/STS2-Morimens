using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core.Character.Patches;

public static class CharacterPatchRegistry
{
    public static void Register()
    {
        ModPatcher characterPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "character-patches");
        characterPatcher.RegisterPatch<CharacterModelHookPatch>();
        if (!characterPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical character-patches failed!");
    }
}
