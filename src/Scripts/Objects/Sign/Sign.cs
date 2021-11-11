using Godot;
using System;
using GhostlyPlatformer.Scripts;

public class Sign : Interactable
{
    [Export()] private string _dialogueFile;
    [Export()] private string _facesFile;

    private dynamic _dialogue;
    private Dialogue _dialogueBox;

    private Player _player;
    private bool _playerColliding;

    private Node2D _interactionIcon;
    private AnimationPlayer _interactIndicator;
    
    public override void _Ready()
    {
        _dialogue = GhostlyFunctions.ReadJson(_dialogueFile);
        
        _interactIndicator = GetNode<AnimationPlayer>("SpeakIcon/AnimationPlayer");
        _interactionIcon = GetNode<Node2D>("SpeakIcon");
        _interactIndicator.Play("Default");
        _dialogueBox = (Dialogue) GD.Load<PackedScene>("res://src/Scripts/DialougeBox/DialogueBox.tscn").Instance();

    }

    public override void Interact(Player player)
    {
        if (_dialogueBox.IsInsideTree() && !player.CanMove) return;
        InitDialogue();
    }
    

    public void InitDialogue()
    {
        _dialogueBox = (Dialogue) GD.Load<PackedScene>("res://src/Scripts/DialougeBox/DialogueBox.tscn").Instance();
        _dialogueBox.FacesFile = _facesFile;
        _dialogueBox.DialogueFiles = new[] {_dialogueFile};
        _dialogueBox.PlayerBody = _player;
        GD.Print(_dialogueBox);
        AddChild(_dialogueBox);
    }

    private void BodyEntered(PhysicsBody2D Body)
    {
        if (!(Body is Player body)) return;
        _player = body;
        _playerColliding = true;
        _interactionIcon.Visible = true;
    }
    private void BodyExited(PhysicsBody2D Body)
    {
        if (!(Body is Player body)) return;
        _player = body;
        _playerColliding = false;
        _interactionIcon.Visible = false;
    }
}
