using Morimens.Patches.Minion;
using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Patches;

public static class PatchRegister
{
    public static void Register()
    {
        ModPatcher minionPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "minion-patches");
        minionPatcher.RegisterPatch<MinionGuardianPatch>();
        minionPatcher.RegisterPatch<MinionTurnEndPatch>();
        if (!minionPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical minion-patches failed!");

        ModPatcher hookPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "hook-patches");
        hookPatcher.RegisterPatch<CharacterModelHookPatch>();
        if (!hookPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical hook-patches failed!");
    }
}
