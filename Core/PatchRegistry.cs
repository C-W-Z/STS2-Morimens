using Morimens.Core.Character.Patches;
using Morimens.Core.ExEnergy.Patches;
using Morimens.Core.Menu.CharacterBg.Patches;
using Morimens.Core.Minion.Patches;

namespace Morimens.Core;

public static class PatchRegistry
{
    public static void Register()
    {
        CardPatchRegistry.Register();
        CharacterPatchRegistry.Register();
        ExEnergyPatchRegistry.Register();
        CharacterBgPatchRegistry.Register();
        MinionPatchRegistry.Register();
    }
}
