using Godot;

namespace Morimens.Core.Utils;

public static class NodeUtils
{
    public static T? FindAncestor<T>(Node node) where T : Node
    {
        for (Node parent = node.GetParent(); parent is not null; parent = parent.GetParent())
        {
            T? val = (parent is T t) ? t : null;
            if (val is not null)
            {
                return val;
            }
        }
        return default;
    }
}
