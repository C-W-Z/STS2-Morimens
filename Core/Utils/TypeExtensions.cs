namespace Morimens.Core.Utils;

public static class TypeExtensions
{
    /// <summary>
    /// 依據類別的命名空間自動推導其所屬的角色資料夾名稱。
    /// 若不屬於特定角色，則回傳預設的 "shared"。
    /// </summary>
    public static string GetCharacterFolderName(this Type type)
    {
        string? ns = type.Namespace;

        if (string.IsNullOrEmpty(ns))
        {
            return "shared";
        }

        // 依據 "." 切開字串：["Morimens", "Characters", "Doll", "Cards"]
        string[] segments = ns.Split('.');

        // 嚴謹檢查結構：長度足夠且第二層是 "Characters"
        if (segments.Length >= 3 && segments[1] == "Characters")
        {
            return segments[2]; // 回傳 "Doll"
        }

        return "shared";
    }
}
