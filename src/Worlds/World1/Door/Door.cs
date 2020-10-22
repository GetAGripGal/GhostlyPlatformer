using Godot;
using System;

public class Door : Area2D
{
    private Player playerBody;
    private bool PlayerColliding;

    public AnimationPlayer Player;
    
    public override void _Ready()
    {
        Player = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("PlayerAction") && PlayerColliding)
        {
            Player.Play("Open");
        }
    }


    private void BodyEntered(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            playerBody = (Player)Body;
            PlayerColliding = true;
        }
    }
    
    private void BodyExited(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            playerBody = null;
            PlayerColliding = false;
        }
    }
}
