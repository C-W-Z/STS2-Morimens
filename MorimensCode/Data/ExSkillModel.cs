using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Morimens.ExEnergy;

public class ExSkillModel(string table, string key, IEnumerable<DynamicVar> variables)
{
    public string Table { get; } = table;
    public string Key { get; } = key;

    public DynamicVarSet DynamicVars { get; } = new DynamicVarSet(variables);

    /// <summary>
    /// 統一生成格式化描述的管線
    /// </summary>
    public string GetDescription(CardModel? dummyCard = null, Creature? target = null)
    {
        LocString description = new(Table, Key);

        // 🔴 遵照原版規範，直接呼叫 DynamicVarSet 的 AddTo 方法！
        DynamicVars.AddTo(description);

        bool inCombat = CombatManager.Instance.IsInProgress;
        description.Add("InCombat", inCombat);

        if (inCombat)
        {
            // 統一觸發預覽數值更新
            UpdateVariablesPreview(dummyCard, target);
        }
        else
        {
            // 🔴 直接使用原版 DynamicVarSet 的 ClearPreview() 清除預覽，回復成 BaseValue
            DynamicVars.ClearPreview();
        }

        // 執行原版格式化與自動變色
        return description.GetFormattedText();
    }

    private void UpdateVariablesPreview(CardModel? dummyCard, Creature? target)
    {
        if (dummyCard == null) return;

        // 🔴 完全對齊原版 CardModel.UpdateDynamicVarPreview 的遍歷邏輯
        foreach (DynamicVar item in DynamicVars.Values)
        {
            // 因為原生的 DamageVar / BlockVar 內部強依賴 CardModel 來跑 Global Hooks
            // 這裡傳入為該技能準備的虛擬卡牌（或隱藏卡牌實例），就能完美觸發原版所有力量、遺物的加成計算
            item.UpdateCardPreview(dummyCard, CardPreviewMode.Normal, target, runGlobalHooks: true);
        }
    }
}
