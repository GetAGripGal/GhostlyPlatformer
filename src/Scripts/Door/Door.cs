using Godot;
using System;
using GhostlyPlatformer.Scripts;

public class Door : Interactable
{
   
    [Export()] private Vector2 _teleportingPosition;
    
    private Player _playerBody;
    
    private AnimationPlayer _animationPlayer;
    private AnimationPlayer _transitionPlayer;
    private Timer _transitionTimer;

    private string _currentAnimation;
    
    public override void _Ready()
    {
        _transitionPlayer = GetNode<AnimationPlayer>("Transition/AnimationPlayer");
        _transitionTimer = GetNode<Timer>("Transition/Timer");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_playerBody == null || _currentAnimation != "Open" || !_animationPlayer.IsPlaying()) return;
        _playerBody.AnimatedSprite.Play("Idle");
        _playerBody.MoveAndSlide(Vector2.Down * _playerBody.Controller.Gravity * 14);
    }

    public override void Interact(Player player)
    {
        _playerBody = player;
        _currentAnimation = "Open";
        _animationPlayer.Play("Open");
        _playerBody.CanMove = false;
    }

    public void AnimationFinished(string animName)
    {
        _currentAnimation = animName;
        if (animName != "Open") return;
        _transitionPlayer.Play("Transition");
        _transitionTimer.Start();

    }

    public void TransitionFinished()
    {
        _playerBody.Position = _teleportingPosition;
        _currentAnimation = "Close";
        _animationPlayer.Play("Close");
        _playerBody.CanMove = true;
    }
}
