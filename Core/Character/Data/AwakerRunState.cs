namespace Morimens.Core.Character.Data;

public sealed class AwakerRunState
{
    public const string DataStoreKey = "awaker_run_state";

    // spine是朵爾還是熔朵，是拉蒙娜/環拉/塔薇
    public int AwakerVisualState { get; set; } = 0;
}
