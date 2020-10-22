using Godot;
using System;
using GhostlyPlatformer.Player;

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

    private Timer HurtTimer;
    
    public override void _Ready()
    {
        Controller = new PlayerController(this);
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        Camera = GetNode<Camera2D>("Camera2D");

        HurtTimer = GetNode<Timer>("Timers/HurtTimer");
        
        Health *= PlayerLevel + .25f;

        if (CurrentCamera)
        {
            Camera.Current = true;
        }
    }

    public override void _Process(float delta)
    {
        PlayerLogic();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (CanMove)
        {
            HandleAnimations();
            Controller.MovePlayer(delta);
        }
    }

    public void HandleAnimations()
    {
        if ((Input.IsActionPressed("PlayerLeft") || Input.IsActionPressed("PlayerRight")) && IsOnFloor())
        {
            AnimatedSprite.Play("Walk");
        }

        if ((Input.IsActionPressed("PlayerCrouch") || Controller.topRay.IsColliding()) && IsOnFloor())
        {
            if(Input.IsActionPressed("PlayerLeft") || Input.IsActionPressed("PlayerRight"))
                AnimatedSprite.Play("CrouchWalk");
            else
                AnimatedSprite.Play("CrouchIdle");
        }

        if (Controller.velocity.y < 0 && !(IsClimbing || IsOnFloor()))
        {
           AnimatedSprite.Play("Jump");
        }
        else if (Controller.velocity.y > 0 && !(IsClimbing || IsOnFloor()))
        {
            AnimatedSprite.Play("Fall");
        }
        else if (!(Input.IsActionPressed("PlayerLeft") || Input.IsActionPressed("PlayerRight") ||
                   Input.IsActionPressed("PlayerCrouch") || Controller.topRay.IsColliding()) && IsOnFloor())
        {
            AnimatedSprite.Play("Idle");
        }

        if (Input.IsActionPressed("PlayerLeft"))
            AnimatedSprite.FlipH = true;
        else if (Input.IsActionPressed("PlayerRight"))
            AnimatedSprite.FlipH = false;
    }

    public void PlayerLogic()
    {
        if (Health <= 0)
        {
            GetTree().ReloadCurrentScene();
        }
    }
    

    public void AttackCollider(PhysicsBody2D Body)
    {
        if (Body is Enemy)
        {
            Controller.BounceOff();
        }
    }

    public void Hurt(float damageMultiplier)
    {
        if (CanGetHurt)
        {
            Health -= PlayerLevel * damageMultiplier;
            AnimationPlayer.Play("Hurt");
            HurtTimer.Start();
            CanGetHurt = false;
        }
    }

    private void OnHurtTimer()
    {
        CanGetHurt = true;
        AnimationPlayer.Play("Idle");
    }
}
