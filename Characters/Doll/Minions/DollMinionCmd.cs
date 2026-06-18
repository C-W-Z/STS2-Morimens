using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Commands;
using MinionLib.Minion;
using Morimens.Characters.Doll.Definition;
using Morimens.Characters.Doll.Powers;
using Morimens.Utils;

namespace Morimens.Characters.Doll.Minions;

public static class DollMinionCmd
{
    public static async Task<Creature> Summon(PlayerChoiceContext choiceContext, Player player, CardModel? cardSource, decimal? maxHp = null, decimal? hp = null, decimal? atk = null)
    {
        // 達上限再召喚的話最早的人偶會自爆對所有敵人造成它當前血量的傷害
        var minions = GetAllDollMinions(player);
        if (minions.Count >= GetDollMinionLimit(player) && minions.Count > 0)
        {
            await AttackAllEnemy(choiceContext, minions[0], cardSource);
            await CreatureCmd.Kill(minions[0].Creature);
            await Cmd.Wait(1); // 等死亡動畫播完
        }

        return await MinionCmd.AddMinion<DollMinion>(choiceContext, player, new MinionSummonOptions(
            MaxHp: maxHp,               // 血量
            PrimaryStatAmount: hp,      // 目前血量
            SecondaryStatAmount: atk,   // 力量
            Source: cardSource,         // 召喚來源（牌）
            Position: MinionPosition.Front)); // 站位但不重要因為 DollMinionLayout 會自動調整
    }

    public static async Task<Creature> SummonCopy(PlayerChoiceContext choiceContext, Player player, DollMinion origin, CardModel? cardSource)
    {
        var powers = origin.Creature.Powers.ToList();
        Creature newMinion = await Summon(choiceContext, player, cardSource, origin.Creature.MaxHp, origin.Creature.CurrentHp, 0);
        // TODO: 用PowerCmd.Apply會受到能力加成影響（如力量加成、災厄加成等），要改成更底層的賦予powers
        await PowerUtils.ApplyPowersDynamically(choiceContext, newMinion, powers, player, cardSource);
        return newMinion;
    }

    public static async Task<Creature> MergeAllDollMinions(PlayerChoiceContext choiceContext, Player player, CardModel? cardSource)
    {
        IEnumerable<DollMinion> minions = GetAllDollMinions(player);
        decimal maxHp = 0;
        decimal hp = 0;
        List<PowerModel> powers = [];

        foreach (var minion in minions)
        {
            maxHp += minion.Creature.MaxHp;
            hp += minion.Creature.CurrentHp;
            powers.AddRange([.. minion.Creature.Powers]); // 收集所有能力
            // TODO: 是否要用殺死這件事情有待商榷，可能會有意外情況，或者導致太強？
            await CreatureCmd.Kill(minion.Creature);
        }

        Creature newMinion = await Summon(choiceContext, player, cardSource, maxHp, hp, 0);
        // TODO: 用PowerCmd.Apply會受到能力加成影響（如力量加成、災厄加成等），要改成更底層的賦予powers
        await PowerUtils.ApplyPowersDynamically(choiceContext, newMinion, powers, player, cardSource);
        return newMinion;
    }

    public static async Task AttackRandomEnemy(PlayerChoiceContext choiceContext, DollMinion minion, CardModel? cardSource)
    {
        Creature? enemy = minion.Creature.PetOwner?.RunState.Rng.CombatTargets.NextItem(minion.CombatState.HittableEnemies);
        if (enemy is null)
            return;

        await CreatureCmd.TriggerAnim(minion.Creature, DollSpine.State.Attack, DollSpine.AttackAnimDelay);
        await CreatureCmd.Damage(choiceContext, enemy, 0m, ValueProp.Move, minion.Creature, cardSource);
    }

    public static async Task AttackAllEnemy(PlayerChoiceContext choiceContext, DollMinion minion, CardModel? cardSource)
    {
        if (minion.Creature.CombatState is null || minion.Creature.CombatState.HittableEnemies.Count == 0)
            return;

        await MinionAnimCmd.PlayBumpAttackAsync(minion.Creature, minion.Creature.CombatState.HittableEnemies[0]); // 播放撞击动画
        await CreatureCmd.Damage(choiceContext, minion.Creature.CombatState.HittableEnemies, minion.Creature.CurrentHp, ValueProp.Move | ValueProp.Unpowered, minion.Creature, cardSource);
    }

    public static List<DollMinion> GetAllDollMinions(Player player)
    {
        IEnumerable<Creature>? pets = player.PlayerCombatState?.Pets;
        if (pets is null)
            return [];
        return [.. pets.Select(p => p.Monster).OfType<DollMinion>()];
    }

    // TODO: 做成 interface 的 hook 形式，讓遺物之類的也能加上限
    public static int GetDollMinionLimit(Player player)
    {
        return DollMinion.BASE_LIMIT + player.Creature.GetPowerAmount<MinionLimitUpPower>();
    }
}
