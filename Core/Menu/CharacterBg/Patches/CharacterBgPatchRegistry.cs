using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core.Menu.CharacterBg.Patches;

public static class CharacterBgPatchRegistry
{
    public static void Register()
    {
        ModPatcher characterBgPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "character-bg-patches");
        characterBgPatcher.RegisterPatch<CharacterSelectBgBtnPatch>();
        characterBgPatcher.RegisterPatch<CharacterSelectBgPatch>();
        if (!characterBgPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical character-bg-patches failed!");
    }
}
