using Godot;
using System;
using GhostlyPlatformer;
using GhostlyPlatformer.Items;
using GhostlyPlatformer.Scripts;

public class Chest : Interactable
{
    private AnimatedSprite _animationSprite;
    private Dialogue _dialogueBox;
    
    [Export()] private string _itemHeld;
    [Export()] private string _key;
    [Export()] public bool IsLocked;

    private bool _isOpened;
    
    public override void _Ready()
    {
        _animationSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _animationSprite.Play("Closed");
        _dialogueBox = GetNode<Dialogue>("DialogueBox");
    }

    public override void Interact(Player player)
    {
        if (_isOpened) return;
        if (IsLocked)
        {
            if (player.Inventory.Contains(Database.Items[_key]))
            {
                _dialogueBox.PlayerBody = player;
                _dialogueBox.Announce("You unlocked the chest", GD.Load<Texture>("res://src/Scripts/Chest/Icon.png"));
                IsLocked = false;
                return;
            }
            _dialogueBox.PlayerBody = player;
            _dialogueBox.Announce("The chest is locked", GD.Load<Texture>("res://src/Scripts/Chest/Icon.png"));
            return;    
        }
        Open(player);
        
    }

    public void Open(Player player)
    {
        _animationSprite.Play("Opened");
        player.ObtainItem(Database.Items[_itemHeld]);
        _isOpened = true;
    }

    //private void BodyEntered(PhysicsBody2D Body)
    //{
    //    if (Body is Player)
    //    {
    //        PlayerBody = (Player)Body;
    //        PlayerColliding = true;
    //    }
    //}

    //private void BodyExited(PhysicsBody2D Body)
    //{
    //    if (Body is Player)
    //    {
    //        PlayerBody = null;
    //        PlayerColliding = false;
    //    }
    //}
}
