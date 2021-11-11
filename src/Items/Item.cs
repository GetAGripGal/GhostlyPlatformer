using Godot;

namespace GhostlyPlatformer.Items
{
    public class Item
    {
        public readonly string Name;
        public readonly Texture Icon;

        public Item(string Name, Texture Icon)
        {
            this.Name = Name;
            this.Icon = Icon;
        }
    }
}