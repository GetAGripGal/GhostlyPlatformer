using Godot;
using System;
using GhostlyPlatformer.Scripts;

public class Sign : Area2D
{
    [Export()] private string DialogueFile;
    [Export()] private string Facesfile;

    private dynamic Dialogue;
    private Dialogue DialogueBox;

    private Player player;
    private bool PlayerColliding;
    
    public override void _Ready()
    {
        Dialogue = GhostlyFunctions.ReadJson(DialogueFile);
        
        DialogueBox = GetNode<Dialogue>("DialogueBox");
        DialogueBox.FacesFile = Facesfile;
        DialogueBox.DialogueFiles = new[] {DialogueFile};
        
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("PlayerAction") && PlayerColliding && !DialogueBox.IsShown && player.CanMove)
        {
            DialogueBox.PlayerBody = player;
            DialogueBox.Show();
        }
    }

    private void BodyEntered(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            player = (Player) Body;
            PlayerColliding = true;
        }
    }
    private void BodyExited(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            player = (Player) Body;
            PlayerColliding = false;
        }
    }
}
