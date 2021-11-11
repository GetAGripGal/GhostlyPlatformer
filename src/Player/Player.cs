using Godot;
using System;
using System.Collections.Generic;
using GhostlyPlatformer;
using GhostlyPlatformer.Items;
using GhostlyPlatformer.Player;
using GhostlyPlatformer.Scripts;

public class Player : KinematicBody2D
{
    public PlayerController Controller;
    public AnimatedSprite AnimatedSprite;
    public AnimationPlayer AnimationPlayer;

    public Camera2D Camera;
    [Export()] public bool CurrentCamera;
    
    [Export()] public bool CanMove = true;
    public bool IsClimbing = false;

    [Export()] public float PlayerLevel = 1;
    public float MaxHealth = 20;
    public float Health = 20;
    public bool CanGetHurt = true;

    private PlayerHud _ui;
    private Dialogue _itemBox;

    private Area2D _swipeAttacker;

    private Timer _hurtTimer;
    private Area2D _interactCollider;

    [Export()] private string[] _itemIDs = new string[5];
    public List<Item> Inventory = new List<Item>(5);
    
    public override void _Ready()
    {
        Controller = new PlayerController(this);
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        
        Camera = GetNode<Camera2D>("Camera2D");

        _ui = GetNode<PlayerHud>("UI/Control");
        _itemBox = _ui.GetNode<Dialogue>("ItemAlert/DialogueBox");

        _hurtTimer = GetNode<Timer>("Timers/HurtTimer");
        _swipeAttacker = GetNode<Area2D>("SwipeAttacker");

        _interactCollider = GetNode<Area2D>("InteractCollider");
        
        MaxHealth = Health = 20;

        if (Inventory.Count <= 0) return;
        foreach (var id in _itemIDs)
        {
            Inventory.Add(Database.Items[id]);
        }
    }

    public override void _Process(float delta)
    {
        PlayerLogic();
        Camera.Current = CurrentCamera;
        
        foreach (var item in Inventory)
        {
            GD.Print(item.Name);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!CanMove) return;
        
        AttackHandler();
        HandleAnimations();
        Controller.MovePlayer(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (!CanMove) return;
        if (!Input.IsActionJustPressed("PlayerAction")) return;
        
        var collidingInteractable = _interactCollider.GetOverlappingAreas()[0];
        if (!(collidingInteractable is Interactable interactable)) return;
        interactable.Interact(this);
    }

    private void HandleAnimations()
    {
        if ((Input.IsActionPressed("PlayerCrouch") || Controller.topRay.IsColliding()) && IsOnFloor()) 
            AnimatedSprite.Play (Controller.velocity.x != 0 ? "CrouchWalk" : "CrouchIdle");
        else if ((Input.IsActionPressed("PlayerLeft") || Input.IsActionPressed("PlayerRight")) && IsOnFloor()) 
            AnimatedSprite.Play("Walk");

        if (Controller.velocity.y < 0 && !(IsClimbing || IsOnFloor()))
            AnimatedSprite.Play("Jump");
        else if (Controller.velocity.y > 0 && !(IsClimbing || IsOnFloor()))
            AnimatedSprite.Play("Fall");
        else if (!(Input.IsActionPressed("PlayerLeft") || Input.IsActionPressed("PlayerRight") ||
                   Input.IsActionPressed("PlayerCrouch") || Controller.topRay.IsColliding()) && IsOnFloor())
            AnimatedSprite.Play("Idle");

        if (Input.IsActionPressed("PlayerLeft")) AnimatedSprite.FlipH = true;
        else if (Input.IsActionPressed("PlayerRight")) AnimatedSprite.FlipH = false;
    }

    private void PlayerLogic()
    {
        if (Health <= 0) GetTree().ReloadCurrentScene();
    }

    private void AttackHandler()
    {
        if (Inventory.Contains(Database.Items["Weapons//Sword"]))
        {
            _swipeAttacker.Visible = true;
            _swipeAttacker.LookAt(GetGlobalMousePosition());
            return;
        }
        
        _swipeAttacker.Visible = false;
    }

    public void AttackCollider(PhysicsBody2D body)
    {
        if (body is Enemy)
        {
            Controller.BounceOff();
        }
    }

    public void Hurt(float damageMultiplier)
    {
        if (!CanGetHurt) return;
        
        Health -= PlayerLevel * damageMultiplier;
        AnimationPlayer.Play("Hurt");
        _hurtTimer.Start();
        CanGetHurt = false;
    }

    public void ObtainItem(Item item)
    {
        _itemBox.PlayerBody = this;
        _itemBox.ShowItemObtained(item);
        Inventory.Add(item);
    }

    private void OnHurtTimer()
    {
        CanGetHurt = true;
        AnimationPlayer.Play("Idle");
    }
}
