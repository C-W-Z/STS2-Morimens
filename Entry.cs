using System.Reflection;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Morimens.Characters.Doll;
using Morimens.Characters.Shared;
using Morimens.Core;
using Morimens.Core.ExEnergy;
using STS2RitsuLib;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Interop;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace Morimens;

[ModInitializer(nameof(Initialize))]
public static partial class Entry
{
    // ModId 需要和 Morimens.json 里的 id 保持一致。
    // res://Morimens/... 里的 Morimens 是 PCK 资源目录，不是 C# namespace。
    public const string ModId = "Morimens";
    public const string ResPath = $"res://{ModId}";
    public const string ImagePath = $"{ResPath}/images";
    public const string ScenePath = $"{ResPath}/scenes";
    public const string AudioPath = $"{ResPath}/audio";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 以下示例默认已经在 Entry.Initialize() 中调用了
        // RitsuLibFramework.EnsureGodotScriptsRegistered(...) 和
        // ModTypeDiscoveryHub.RegisterModAssembly(...)，否则自动注册不会生效。
        //
        // Godot C# 脚本注册只负责让 pck 中的脚本类型能被 Godot 找到。
        // 这一步和 RitsuLib 的内容自动注册不是同一件事，两个都需要保留。
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);

        // 自动注册扫描会读取当前程序集里的 RegisterCard/RegisterRelic 等 attribute。
        // 新增内容类后，只要 attribute 写对，通常不需要在入口里手动逐个注册。
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);

        DataRegistry.Register();
        PatchRegistry.Register();
        UiRegistry.Register();

        ExEnergyManager.Register();

        FmodStudioDeferredBankRegistration.RegisterBank($"{AudioPath}/Morimens.bank");
        FmodStudioDeferredBankRegistration.RegisterStudioGuidMappings($"{AudioPath}/GUIDs.txt");

        SharedRegistry.Register();
        DollRegistry.Register();

        Logger.Info("Morimens initialized.");
    }
}
