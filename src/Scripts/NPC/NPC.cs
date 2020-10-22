using Godot;
using System;

public class NPC : Area2D
{
    [Export()] private string[] DialogueFiles;
    [Export()] private string Facesfile;
    [Export()] private int DialogueAmount;
    [Export()] private bool hasDialouge;
    [Export()] private bool restartAtBeginning;

    private int FileIndex = 0;
    private Dialogue DialogueBox;

    private Player player;
    private bool playerColliding;

    private AnimationPlayer InteractIndicator;
    private Sprite InteractionIcon;

    public override void _Ready()
    {
        InteractIndicator = GetNode<AnimationPlayer>("AnimationPlayer");
        InteractionIcon = GetNode<Sprite>("Sprite");
        
        DialogueBox = GetNode<Dialogue>("DialogueBox");
        DialogueBox.DialogueFiles = DialogueFiles;
        DialogueBox.DialogueAmount = DialogueAmount;
        DialogueBox.FacesFile = Facesfile;
        
        InteractIndicator.Play("Default");
        if(InteractionIcon != null) InteractionIcon.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("PlayerAction") && playerColliding && !DialogueBox.IsShown && player.CanMove)
        {
            DialogueBox.PlayerBody = player;
            
            if(restartAtBeginning) DialogueBox.Page = 0;
            DialogueBox.Show();
        }
    }

    public void OnBodyEntered(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            playerColliding = true;
            player = (Player)Body;
            InteractionIcon.Visible = true;
        }
    }
    
    public void OnBodyExited(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            playerColliding = false;
            player = null;
            InteractionIcon.Visible = false;
        }
    }

}
