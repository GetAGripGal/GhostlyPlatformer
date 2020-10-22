using Godot;
using System;

public class SceneLoader : Area2D
{
    [Export()] private PackedScene loadedScene;

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if(body is Player)
            LoadScene();
    }

    private void LoadScene()
    {
        if (loadedScene != null)
            GetTree().ChangeSceneTo(loadedScene);
        else throw new Exception("No scene set to load");
    }
}
