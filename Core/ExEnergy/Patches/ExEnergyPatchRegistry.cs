using STS2RitsuLib;
using STS2RitsuLib.Patching.Core;

namespace Morimens.Core.ExEnergy.Patches;

public static class ExEnergyPatchRegistry
{
    public static void Register()
    {
        ModPatcher exEnergyPatcher = RitsuLibFramework.CreatePatcher(Entry.ModId, "ex-energy-patches");
        exEnergyPatcher.RegisterPatch<ExEnergyFontSizePatch>();
        if (!exEnergyPatcher.PatchAll())
            throw new InvalidOperationException("Morimens critical ex-energy-patches failed!");
    }
}
