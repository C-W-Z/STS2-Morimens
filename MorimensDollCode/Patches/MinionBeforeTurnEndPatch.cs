using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Combat;
using STS2RitsuLib.Patching.Models;

namespace MorimensDoll.Patches;

public class MinionBeforeTurnEndPatch : IPatchMethod
{
    // 1. 填寫 RitsuLib 要求的唯一 ID 與說明
    public static string PatchId => "morimens_doll_minion_before_turn_end";

    public static string Description => "將戰鬥中存活的Minions放入回合結束的參與者名單中";

    public static bool IsCritical => false;

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(Hook), nameof(Hook.BeforeTurnEnd))];

    // 3. 實作 Prefix 補丁 (維持 RitsuLib 的規範，名稱必須為 Prefix)
    public static void Prefix(ICombatState combatState, CombatSide side, ref IEnumerable<Creature> participants)
    {
        if (participants == null) return;

        Entry.Logger.Debug($"MinionBeforeTurnEndPatch: original participants");
        foreach (var p in participants)
            Entry.Logger.Debug(p.Name.ToString());

        var updatedParticipants = participants.ToList();
        bool updated = false;

        foreach (var creature in participants)
        {
            // 只有玩家的回合結束需要把隨從拉進來結算
            if (!creature.IsPlayer || creature.Pets == null) continue;

            Entry.Logger.Debug($"MinionBeforeTurnEndPatch: Find Player {creature.Name}, Pet Count {creature.Pets.Count}");

            // 走訪收集到的所有隨從
            foreach (var pet in creature.Pets)
            {
                if (pet == null || pet.Monster == null || pet.IsDead) continue;

                Entry.Logger.Debug($"MinionBeforeTurnEndPatch: Find Pet {pet.Monster?.GetType().Namespace}");

                // 修正：核心關鍵！原版遊戲自己的召喚物（如 Osty，屬於 MegaCrit 命名空間）
                // 本來就不需要進這個名單就能運作。強行塞入會破壞原版的回合清理機制。
                // 我們直接利用命名空間排除原版和其他模組的召喚物，只保留我們 Mod 的自訂召喚物！
                if (pet.Monster?.GetType().Namespace?.StartsWith(Entry.ModId) == false) continue;

                if (!updatedParticipants.Contains(pet))
                {
                    updatedParticipants.Add(pet);
                    updated = true;
                }
            }
        }

        // 如果名單有變動，透過 ref 關鍵字把修改後的新名單覆蓋回去
        if (updated)
        {
            participants = updatedParticipants;
        }

        Entry.Logger.Debug($"MinionBeforeTurnEndPatch: new participants");
        foreach (var p in participants)
            Entry.Logger.Debug(p.Name.ToString());
    }
}
