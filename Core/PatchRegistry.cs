using Morimens.Core.Character.Patches;
using Morimens.Core.ExEnergy.Patches;
using Morimens.Core.Menu.CharacterBg.Patches;
using Morimens.Patches.Minion;
using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core;

public static class PatchRegistry
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

        ModPatcher exEnergyPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "ex-energy-patches");
        exEnergyPatcher.RegisterPatch<ExEnergyFontSizePatch>();
        if (!exEnergyPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical hook-patches failed!");

        ModPatcher uiPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "ui-patches");
        uiPatcher.RegisterPatch<CharacterSelectBgBtnPatch>();
        uiPatcher.RegisterPatch<CharacterSelectBgPatch>();
        if (!uiPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical ui-patches failed!");
    }
}
