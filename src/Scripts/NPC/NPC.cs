using Godot;
using System;
using GhostlyPlatformer.Scripts;

public class NPC : Interactable
{
    [Export()] private string[] _dialogueFiles;
    [Export()] private string _facesfile;
    [Export()] private int _dialogueAmount;
    [Export()] public int CurrentDialogueFile;
    [Export()] private bool _hasDialouge;
    [Export()] private bool _restartAtBeginning;

    private int _fileIndex = 0;
    private Dialogue _dialogueBox;

    private Player _player;
    private bool _playerColliding;

    private AnimationPlayer _interactIndicator;
    private Node2D _interactionIcon;

    public override void _Ready()
    {
        _interactIndicator = GetNode<AnimationPlayer>("SpeakIcon/AnimationPlayer");
        _interactionIcon = GetNode<Node2D>("SpeakIcon");
        
        _dialogueBox = (Dialogue) GD.Load<PackedScene>("res://src/Scripts/DialougeBox/DialogueBox.tscn").Instance();
        _dialogueBox.DialogueFiles = _dialogueFiles;
        _dialogueBox.DialogueAmount = _dialogueAmount;
        _dialogueBox.FacesFile = _facesfile;
        
        _interactIndicator.Play("Default");
        if(_interactionIcon != null && _hasDialouge) _interactionIcon.Visible = false;
    }

    public override void Interact(Player player)
    {
        if (_dialogueBox.IsInsideTree() && !_player.CanMove) return;

        _dialogueBox.PlayerBody = player;
        _dialogueBox.CurrentFile = CurrentDialogueFile;
            
        if (_restartAtBeginning) _dialogueBox.Page = 0;
        AddChild(_dialogueBox);
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if (body is Player)
        {
            _interactionIcon.Visible = true;
        }
    }
    
    public void OnBodyExited(PhysicsBody2D Body)
    {
        if (Body is Player)
        {
            _interactionIcon.Visible = false;
        }
    }

}
