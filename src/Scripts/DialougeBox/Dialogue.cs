using Godot;
using System;
using GhostlyPlatformer.Scripts;

public class Dialogue : CanvasLayer
{
    public string[] DialogueFiles;
    public string FacesFile;
    public int DialogueAmount;
    public Player PlayerBody;

    private Control ControlNode;
    private RichTextLabel Label;
    private Sprite Face;

    public bool IsShown = false;
    public int Page = 0;
    public int CurrentFile = 0;
    public int AmountInFile;
    private dynamic CurrentDialogue;

    public override void _Ready()
    {
        ControlNode = GetNode<Control>("Control");
        Label = ControlNode.GetNode<RichTextLabel>("RichTextLabel");
        Face = ControlNode.GetNode<Sprite>("Face");
    }

    public override void _Process(float delta)
    {
        if (IsShown)
        {
            if(PlayerBody != null) PlayerBody.CanMove = false;
            ControlNode.Visible = true;
        }
        else
        {
            if(PlayerBody != null) PlayerBody.CanMove = true;
            ControlNode.Visible = false;
        }
    }

    public void Show()
    {
        CurrentDialogue = GhostlyFunctions.ReadJson(DialogueFiles[CurrentFile])["Dialogue"][Page.ToString()]["Text"];
        Face.Texture = GD.Load<Texture>(GhostlyFunctions.ReadJson(FacesFile)["Faces"][GhostlyFunctions.ReadJson(DialogueFiles[CurrentFile])["Dialogue"][Page.ToString()]["Face"]]);
        AmountInFile = (int)GhostlyFunctions.ReadJson(DialogueFiles[CurrentFile])["Amount"];
        DisplayDialogue(CurrentDialogue);
    }

    private void LabelTimout()
    {
        Label.VisibleCharacters++;
    }

    private void DisplayDialogue(dynamic what)
    {
        Label.VisibleCharacters = 0;
        Label.BbcodeText = CurrentDialogue.ToString();
        IsShown = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("PlayerAction") && IsShown)
        {
            if (Label.VisibleCharacters < ((string) CurrentDialogue).Length)
            {
                Label.VisibleCharacters = ((string) CurrentDialogue).Length;
            }
            else if (Page < AmountInFile)
            {
                Page++;
                Show();
            }
            else
            {
                IsShown = false;
            }
        }
    }
}
