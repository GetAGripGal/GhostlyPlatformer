using Godot;
using System;

public class WindowControls : Node2D
{
    private Timer _mouseTimer;
    public readonly bool debug = true;

    public override void _Ready()
    {
        _mouseTimer = GetNode<Timer>("MouseTimer");
    }

    public override void _Process(float delta)
    {
        OS.SetWindowTitle($"Postmortem | FPS: {Engine.GetFramesPerSecond()}");
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("WindowFullscreen"))
            OS.WindowFullscreen = !OS.WindowFullscreen;
        
        if(Input.IsActionJustPressed("WindowEscape"))
            GetTree().Quit();

        if (debug)
        {
            if (Input.IsActionJustPressed("DebugReload"))
                GetTree().ReloadCurrentScene();
        }

        if (Input.IsActionJustPressed("MouseClick"))
        {
            Input.SetCustomMouseCursor(GD.Load("res://src/Scripts/MouseCursor/Mouse2.png"));
            _mouseTimer.Start();
        }
    }

    private void OnMouseTimer()
    {
        Input.SetCustomMouseCursor(GD.Load("res://src/Scripts/MouseCursor/Mouse1.png"));
    }
}
