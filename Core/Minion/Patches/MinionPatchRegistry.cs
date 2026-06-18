using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core.Minion.Patches;

public static class MinionPatchRegistry
{
    public static void Register()
    {
        ModPatcher minionPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "minion-patches");
        minionPatcher.RegisterPatch<MinionGuardianPatch>();
        minionPatcher.RegisterPatch<MinionTurnEndPatch>();
        if (!minionPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical minion-patches failed!");
    }
}
