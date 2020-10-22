using Godot;
using System;

public class WindowControls : Node2D
{
    private Timer MouseTimer;

    public override void _Ready()
    {
        MouseTimer = GetNode<Timer>("MouseTimer");
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("WindowFullscreen"))
            OS.WindowFullscreen = !OS.WindowFullscreen;
        if(Input.IsActionJustPressed("WindowEscape"))
            GetTree().Quit();

        if (Input.IsActionJustPressed("MouseClick"))
        {
            Input.SetCustomMouseCursor(GD.Load("res://src/Scripts/MouseCursor/Mouse2.png"));
            MouseTimer.Start();
        }
    }

    private void OnMouseTimer()
    {
        Input.SetCustomMouseCursor(GD.Load("res://src/Scripts/MouseCursor/Mouse1.png"));
    }
}
