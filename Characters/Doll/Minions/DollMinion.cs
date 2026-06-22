using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MinionLib.Minion;
using MinionLib.Powers;
using Morimens.Characters.Doll.Definition;
using Morimens.Core.Minion;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Characters.Doll.Minions;

public class DollMinion : ModMinionTemplate
{
    public override int MinInitialHp => 1; // 作为敌方方怪物生成时的血量，通常无需在意
    public override int MaxInitialHp => 1; // 作为敌方方怪物生成时的血量，通常无需在意

    public const decimal BASE_MAX_HP = 5m;
    public const decimal BASE_ATK = 1m;

    // 預設的數量上限
    public const int BASE_LIMIT = 4;

    private const string SceneRoot = $"{Entry.ScenePath}/Doll";

    public override MonsterAssetProfile AssetProfile => new(
        VisualsScenePath: $"{SceneRoot}/minion.tscn"
    );

    // 召唤时执行的代码，通常用来设置血量、应用初始能力等，options 是在召唤随从时传入的参数
    // 注意使用 self 而非 this
    public override async Task OnSummon(PlayerChoiceContext choiceContext, Player owner, MinionSummonOptions options)
    {

        if (options.MaxHp is decimal maxHp && maxHp > 0)
            await CreatureCmd.SetMaxHp(Creature, maxHp);
        else
            await CreatureCmd.SetMaxHp(Creature, BASE_MAX_HP);

        if (options.PrimaryStatAmount is decimal hp && hp > 0m)
            await CreatureCmd.SetCurrentHp(Creature, hp);
        else
            await CreatureCmd.SetCurrentHp(Creature, Creature.MaxHp);

        if (options.SecondaryStatAmount is decimal strength && strength > 0m)
            await PowerCmd.Apply<StrengthPower>(choiceContext, Creature, strength, owner.Creature, options.Source);
        else
            await PowerCmd.Apply<StrengthPower>(choiceContext, Creature, BASE_ATK, owner.Creature, options.Source);

        await PowerCmd.Apply<MinionGuardianPower>(choiceContext, Creature, 1, owner.Creature, options.Source);
    }

    public override async Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Creature.Side || !participants.Contains(Creature))
            return;

        await DollMinionCmd.AttackRandomEnemy(choiceContext, this, null);
    }

    protected override CreatureAnimator? SetupCustomCreatureAnimator(MegaSprite controller)
    {
        return DollSpine.GetCreatureAnimator(controller);
    }
}
