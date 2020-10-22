using Godot;
using System;

public class Ladder : Area2D
{
    private Player playerBody;
    private bool playerColliding;

    private float Gravity = 0;

    private float speed = 20;

    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (playerBody != null)
        {
            if (playerColliding && !playerBody.IsClimbing && (Input.IsActionPressed("PlayerClimb") || Input.IsActionPressed("PlayerJump") ||
                                    Input.IsActionPressed("PlayerCrouch")))
            {
                playerBody.Controller.GravityEnabled = false;
                playerBody.IsClimbing = true;
                playerBody.Controller.velocity.y = 0;
            }
            

            if (playerBody.IsClimbing)
            {
                if (Input.IsActionPressed("PlayerClimb") || Input.IsActionPressed("PlayerJump"))
                {
                    playerBody.AnimatedSprite.Play("Climb");
                    playerBody.Controller.velocity = playerBody.MoveAndSlide(Vector2.Up * speed);
                }
                else if (Input.IsActionPressed("PlayerCrouch"))
                {
                    playerBody.AnimatedSprite.Play("Climb");
                    playerBody.Controller.velocity = playerBody.MoveAndSlide(Vector2.Down * speed);
                }
                else
                {
                    playerBody.AnimatedSprite.Play("ClimbIdle");
                    playerBody.Controller.velocity = playerBody.MoveAndSlide(Vector2.Zero * speed);
                }
            }
        }

    }

    private void BodyEntered(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            playerBody = (Player) Body;
            playerColliding = true;
            playerBody.Controller.velocity.y = 0;
        }
    }

    private void BodyExited(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            playerColliding = false;
            playerBody.Controller.GravityEnabled = true;
            playerBody.IsClimbing = false;
        }
    }
    
}
