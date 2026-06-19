using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Random;

namespace Morimens.Core.Character;

public partial class NMerchantAwaker : NMerchantCharacter
{
    public override void _Ready()
    {
        this.RunWhenSpineReady(new MegaSprite(GetChild(0)), delegate
        {
            PlayAnimation("Idle_1", loop: true);
        });
    }
}
