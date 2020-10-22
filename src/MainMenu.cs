using Godot;
using System;

public class MainMenu : Control
{
    [Export()] private PackedScene SceneToPlay;

    public void PlayButton()
    {
        if(SceneToPlay != null)
            GetTree().ChangeSceneTo(SceneToPlay);
        else
            throw new Exception("No scene set to load");
    }

    public void QuitButton()
    {
        GetTree().Quit();
    }

}
