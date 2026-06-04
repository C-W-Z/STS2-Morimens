using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Minion;
using MinionLib.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace MorimensDoll.Powers;

[RegisterPower]
public sealed class DollMinionGuardianPower : AbstractDollPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override bool IsVisibleInternal => false;

#pragma warning disable RITSU013
    public override PowerAssetProfile AssetProfile => new(
        // 使用原版遊戲的DieForYouPower的Icon
        IconPath: ImageHelper.GetImagePath("atlases/power_atlas.sprites/die_for_you_power.tres"),
        BigIconPath: ImageHelper.GetImagePath("powers/die_for_you_power.png")
    );
#pragma warning restore RITSU013

    public override Creature ModifyUnblockedDamageTarget(Creature target, decimal amount, ValueProp props, Creature? dealer)
    {
        // 1. 如果隨從不在前排，不攔截
        if (Owner.Monster is MinionModel minion && minion.Position != MinionPosition.Front) return target;

        // 2. 如果受擊目標不是主人（代表傷害已經被其他隨從攔截過去了）
        if (target != Owner.PetOwner?.Creature)
        {
            var flag = true;

            // 關鍵：檢查目標身上是否有「原生的」或「我們自訂的」守衛 Power
            if (target.PetOwner == Owner.PetOwner && Owner.PetOwner != null &&
                (target.GetPower<DollMinionGuardianPower>() != null ||
                target.GetPower<MinionGuardianPower>() != null ||
                target.GetPower<DieForYouPower>() != null))
            {
                var pets = target.PetOwner!.PlayerCombatState!.Pets;
                // 如果自己在寵物隊伍中的順位比對方更靠前，就把傷害從對方手中「搶過來」
                if (pets.IndexOf(Owner) < pets.IndexOf(target))
                    flag = false;
            }

            if (flag) return target;
        }

        // 3. 基本死亡與傷害類型檢查
        if (Owner.IsDead) return target;
        if (!props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered)) return target;

        // 完美攔截，傷害轉移到自己身上
        return Owner;
    }
}
