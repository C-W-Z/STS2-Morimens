using STS2RitsuLib;
using STS2RitsuLib.RunData;

namespace Morimens.Core.Character.Data;

public static class AwakerRunStateRegistry
{
    public static PlayerRunSavedData<AwakerRunState> Data { get; private set; } = null!;

    public static void Register()
    {
        using (RitsuLibFramework.BeginModDataRegistration(Entry.ModId))
        {
            var store = RitsuLibFramework.GetRunSavedDataStore(Entry.ModId);

            // 注册按玩家独立的配置
            Data = store.RegisterPerPlayer(
                key: AwakerRunState.DataStoreKey,
                defaultFactory: () => new AwakerRunState(),
                options: new RunSavedDataOptions
                {
                    WritePolicy = RunSavedDataWritePolicy.WhenSet,
                    SyncLobbyOnChange = true, // 允许在大厅修改时同步给队友
                });
        }
    }
}
