using Godot;
using System;

public class Teleporter : Area2D
{
    [Export()] private Vector2 teleportedPosition;
    [Export()] private bool setPlayerCameraCurrent;

    [Export()] private Vector2 CameraZoom;
    
    [Export()] private string newCameraPath;
    private Camera2D newCamera;

    private AnimationPlayer Transition;
    private Player player;
    private bool playerColliding;
    
    public override void _Ready()
    {
        Transition = GetNode<AnimationPlayer>("Transition/AnimationPlayer");
        if (!setPlayerCameraCurrent)
        {
            try
            {
                newCamera = GetTree().Root.GetNode<Camera2D>(newCameraPath);
            }
            catch
            {
                throw new Exception("Invalid path to camera node");
            }
        }
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if (body is Player)
        {
            player = (Player) body;
            Transition.Play("Transition");
            player.CanMove = false;
            ((Timer)GetNode("Transition/Timer")).Start();
        }
    }

    public void OnTimer()
    {
        player.Position = teleportedPosition;
        player.Camera.Zoom = CameraZoom;
        player.Camera.Current = setPlayerCameraCurrent;
        if(newCamera != null)
            newCamera.Current = true;
        player.CanMove = true;
    }
}
