using Godot;
using System;
using GhostlyPlatformer;
using GhostlyPlatformer.Items;
using GhostlyPlatformer.Scripts;
using Godot.Collections;

public class Dialogue : CanvasLayer
{
    public string[] DialogueFiles;
    public string FacesFile;
    public int DialogueAmount;
    public Player PlayerBody;

    public Control ControlNode;
    private RichTextLabel _label;
    private Sprite _face;

    public bool IsShown = false;
    public int Page = 0;
    public int CurrentFile = 0;
    public int AmountInFile;
    private dynamic _currentDialogueFile;
    private dynamic _currentDialogue;
    
    private enum State 
    {
        Dialogue = 0, 
        Announcement
    }

    private State _currentState;

    public override void _Ready()
    {
        ControlNode = GetNode<Control>("Control");
        _label = ControlNode.GetNode<RichTextLabel>("RichTextLabel");
        _face = ControlNode.GetNode<Sprite>("Face");
        Load();
    }

    public void Load()
    {
        if (DialogueFiles == null) return;
        ControlNode.Visible = true;
        
        _currentState = State.Dialogue;
        _currentDialogueFile = GhostlyFunctions.ReadJson(DialogueFiles[CurrentFile]);
        _currentDialogue =
            _currentDialogueFile["Dialogue"][Page.ToString()]["Text"];
        _face.Texture = GD.Load<Texture>(GhostlyFunctions.ReadJson(FacesFile)["Faces"][
            _currentDialogueFile["Dialogue"][Page.ToString()]["Face"]]);
        AmountInFile = (int) GhostlyFunctions.ReadJson(DialogueFiles[CurrentFile])["Amount"];
        DisplayDialogue(_currentDialogue);
    }

    public void ShowItemObtained(Item item)
    {
        _currentState = State.Announcement;
        _currentDialogue = $"{item.Name} Obtained!";
        _face.Texture = item.Icon;
        AmountInFile = 0;
        DisplayDialogue(_currentDialogue);
    }
    
    public void Announce(string announcement, Texture icon)
    {
        ControlNode.Visible = true;
        _currentState = State.Announcement;
        _currentDialogue = announcement;
        _face.Texture = icon;
        AmountInFile = 0;
        DisplayDialogue(_currentDialogue);
    }

    private void LabelTimout()
    {
        _label.VisibleCharacters++;
    }

    private void DisplayDialogue(dynamic what)
    {
        _label.VisibleCharacters = 0;
        _label.BbcodeText = what.ToString();
        IsShown = true;
        if(PlayerBody != null) PlayerBody.CanMove = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (!Input.IsActionJustPressed("PlayerAction") || PlayerBody == null) return;
        
        PlayerBody.CanMove = false;
        if (_label.VisibleCharacters < ((string) _currentDialogue).Length) _label.VisibleCharacters = ((string) _currentDialogue).Length;
        else if (Page < AmountInFile)
        {
            Page++;
            Load();
        }
        else
        {
            if (_currentDialogueFile.Contains("Item") && Database.Items.ContainsKey(_currentDialogueFile["Item"]))
                PlayerBody.ObtainItem(Database.Items[_currentDialogueFile["Item"]]);
            PlayerBody.CanMove = true;
            if (IsInstanceValid(this)) QueueFree();
        }
    }
}
