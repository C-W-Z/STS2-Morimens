using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Commands;
using MinionLib.Minion;
using MinionLib.Targeting;
using MorimensDoll.Characters;
using MorimensDoll.Minions;
using STS2RitsuLib.Interop.AutoRegistration;

namespace MorimensDoll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionAttack() : AbstractDollCard(1, CardType.Skill, CardRarity.Common, MinionTargetTypes.AllMinions)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        bool hasPet = false;
        foreach (var ally in CombatState.Allies)
        {
            if (ally.IsAlive && ally.IsPet && ally.Monster is MinionModel)
            {
                hasPet = true;
                Creature? enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                if (enemy == null)
                    break;
                await MinionAnimCmd.PlayBumpAttackAsync(ally, enemy); // 播放撞击动画
                await CreatureCmd.Damage(choiceContext, enemy, 0m, ValueProp.Move, ally, this); // 造成伤害，方法和原版类似
            }
        }

        if (hasPet)
            return;

        await MinionCmd.AddMinion<DollMinion>(choiceContext, Owner, new MinionSummonOptions(
            MaxHp: DollMinion.MAX_HP / 2,           // 血量
            PrimaryStatAmount: 1m,                  // 主要参数（具体内容在随从的 OnSummon 里定义），还有次要参数等可以按需传入
            Source: this,                           // 召唤来源（通常是这张牌）
            Position: MinionPosition.Front));       // 站位（见后文，默认是前排）
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
