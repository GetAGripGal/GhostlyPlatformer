using Godot;

namespace GhostlyPlatformer.Scripts
{
    public class Interactable : Area2D
    {
        public virtual void Interact(global::Player player){}
    }
}