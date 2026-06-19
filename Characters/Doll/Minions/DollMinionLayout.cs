using MinionLib.Layout;
using MinionLib.Minion;

namespace Morimens.Characters.Doll.Minions;

public class DollMinionLayout : IMinionLayout
{
    // 讓這個佈局器一直保持啟用
    public bool IsActive => true;

    public void ApplyLayout(MinionLayoutContext context)
    {
        // 找出當前戰鬥房間中所有未處理隨從的主人
        var owners = context.UnhandledMinions
            .Select(c => c.Entity.PetOwner)
            .Where(o => o is not null)
            .Distinct();

        foreach (var owner in owners)
        {
            if (owner?.PlayerCombatState?.Pets is null) continue;

            int dollIndex = 0; // 追蹤「真正人偶」的自然召喚順序索引

            // 依照 Pets 隊列的自然順序（召喚順序）重新分組
            foreach (var petEntity in owner.PlayerCombatState.Pets)
            {
                // 💡 這裡精準過濾 DollMinion，完美避開了 PaelsLegion 等原版遺物幽靈隨從的干擾！
                if (petEntity.Monster is DollMinion minion)
                {
                    // 交替策略：第 1, 3, 5... 隻（索引 0, 2, 4...）優先放前排，且不能超過前排上限
                    if (dollIndex % 2 == 0)
                    {
                        minion.Position = MinionPosition.Front;
                    }
                    else
                    {
                        // 第 2, 4, 6... 隻（索引 1, 3, 5...）或是前排滿了時，放後排
                        minion.Position = MinionPosition.Back;
                    }

                    dollIndex++; // 只有真正的人偶才會推進索引
                }
            }
        }

        // 關鍵點：我們在這個方法裡「不」向 context.Positions 寫入任何資料。
        // 這會讓所有隨從維持在 "Unhandled" 狀態，留給下一個優先級低的 DefaultMinionLayout 繪製坐標。
    }
}
