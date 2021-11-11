using System;
using System.Collections.Generic;
using GhostlyPlatformer.Items;
using Godot;

namespace GhostlyPlatformer
{
    public static class Database
    {
        public static Dictionary<string, Item> Items = new Dictionary<string, Item>()
        {
            /* -----Weapons----- */
            {"Weapons//Sword", new Item("Sword", GD.Load<Texture>("res://src/Items/Sword/Texture.png"))}, // Sword
            
            /* -----Keys----- */
            // World1
            {"Keys//SwordKey", new Item("ChestKey", GD.Load<Texture>("res://src/Worlds/World1/Key/Texture.png")) }
        };
    }
}