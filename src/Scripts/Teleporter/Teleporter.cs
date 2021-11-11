using Godot;
using System;

public class Teleporter : Area2D
{
    [Export()] private Vector2 _teleportingPosition;
    [Export()] private bool _setPlayerCameraCurrent;

    [Export()] private Vector2 _cameraZoom;
    
    [Export()] private NodePath _newCameraPath;
    private Camera2D _newCamera;

    private AnimationPlayer _transition;
    private Player _player;
    // private bool _playerColliding;
    
    public override void _Ready()
    {
        _transition = GetNode<AnimationPlayer>("Transition/AnimationPlayer");
        
        if (_setPlayerCameraCurrent) return;
        try { _newCamera = GetNode<Camera2D>(_newCameraPath); }
        catch { throw new Exception("Invalid path to camera node"); }
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if (body is Player player)
        {
            _player = player;
            _transition.Play("Transition");
            _player.CanMove = false;
            ((Timer)GetNode("Transition/Timer")).Start();
        }
    }

    public void OnTimer()
    {
        _player.Position = _teleportingPosition;
        if (_setPlayerCameraCurrent)
        {
            _player.Camera.Zoom = _cameraZoom;
            _player.CurrentCamera= true;
        }
        else if (_newCamera != null)
        {
            _player.CurrentCamera = false;
            _newCamera.Current = true;
        }
        _player.CanMove = true;
    }
}
