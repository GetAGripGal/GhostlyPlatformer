using Godot;
using System;

public class DevilledEgg : Enemy
{
    private AnimatedSprite animator;
    private Timer hurtTimer;
    private Timer attackTimer;

    private bool PlayerNear;
    private bool AttackPlayer;
    private bool Hurting;
    private Player playerBody;

    private float speed = 20;
    private float directionalSpeed;
    private Vector2 velocity;
    
    public const float Gravity = 5;

    public override void _Ready()
    {
        hurtTimer = GetNode<Timer>("HurtTimer");
        attackTimer = GetNode<Timer>("AttackTimer");
        
        attackTimer.WaitTime = Position.x / 200;
        
        animator = GetNode<AnimatedSprite>("AnimatedSprite");
        animator.Play("Idle");

        Health = 6;
        DamageMultiplier = 2;
        AttackMultiplier = 2;
    }

    public override void AttackCollision(PhysicsBody2D Body)
    {
        base.AttackCollision(Body);
    }

    public override void _Process(float delta)
    {
        HandleAnimations();
    }

    public override void _PhysicsProcess(float delta)
    {

        if (AttackPlayer)
        {
            if (IsOnFloor())
            {
                if (playerBody?.Position.x < Position.x)
                {
                    directionalSpeed = -speed;
                    animator.FlipH = true;
                }
                else if (playerBody?.Position.x > Position.x)
                {
                    directionalSpeed = speed;
                    animator.FlipH = false;
                }
                else
                {
                    speed = 0;
                    animator.FlipH = true;
                }
            }
            
            velocity.x = directionalSpeed;
        }
        else
        {
            velocity.x = 0;
            if (Hurting)
                animator.Play("In air");
            else
                animator.Play("Idle");
        }
        velocity.y += Gravity;

        velocity = MoveAndSlide(velocity, Vector2.Up);
    }

    private void HandleAnimations()
    {
        if (!IsOnFloor() && !Hurting)
        {
            animator.Play("In air");
        }
    }

    public override void HurtCollision(PhysicsBody2D Body)
    {
        base.HurtCollision(Body);
        if (Body is Player)
        {
            Hurting = true;
            velocity = Vector2.Zero;
            animator.Play("Hurt");
            AttackPlayer = false;
            hurtTimer.Start();
        }
    }

    private void HurtTimeout()
    {
        DeathCheck();
        if(PlayerNear) 
            AttackPlayer = true;
        Hurting = true;
        animator.Play("Idle");
    }

    private void AttackTimeout()
    {
        attackTimer.WaitTime = Position.x / 200;
        
        if(IsOnFloor())
            velocity += Vector2.Up * 100;
    }

    private void PlayerDetectorEntered(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            AttackPlayer = true;
            PlayerNear = true;
            if(playerBody == null)
                playerBody = (Player) Body;
        }
    }

    private void PlayerDetectorExited(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            PlayerNear = false;
            AttackPlayer = false;
        }
    }

}
