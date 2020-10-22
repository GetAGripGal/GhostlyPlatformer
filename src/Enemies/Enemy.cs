using Godot;
using System;

public class Enemy : KinematicBody2D
{

    [Export()] public float Health;
    [Export()] public float DamageMultiplier;
    [Export()] public float AttackMultiplier;

    public virtual void HurtCollision(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            var player = (Player) Body;
            
            Health -= player.PlayerLevel * AttackMultiplier;
        }
    }

    public virtual void AttackCollision(PhysicsBody2D Body)
    {

        if (Body is Player)
        {
            var player = (Player) Body;

            player.Hurt(AttackMultiplier);
        }
    }

    public void DeathCheck()
    {
        if(Health <= 0)
            QueueFree();
    }
}
