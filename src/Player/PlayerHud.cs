using Godot;
using System;

public class PlayerHud : Control
{

    private Player PlayerBody;
    
    private TextureProgress HealthBar;
    private RichTextLabel HealthLabel;

    private AnimationPlayer animationPlayer;

    private double OldHealth;
    
    public override void _Ready()
    {
        PlayerBody = GetParent().GetParent<Player>();
        HealthBar = GetNode<TextureProgress>("Health/TextureProgress");
        HealthLabel = GetNode<RichTextLabel>("Health/RichTextLabel");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        animationPlayer.Play("MoveOut");
        HealthBar.Value = PlayerBody.Health;
    }

    public override void _Process(float delta)
    {
        HealthBar.MaxValue = PlayerBody.MaxHealth;

        HealthLabel.BbcodeText = $"{HealthBar.Value}/{HealthBar.MaxValue}";
    }
    
    
    
    public void OnHealthDelay()
    {
        HealthBar.Value = Mathf.Lerp((float)HealthBar.Value, PlayerBody.Health, .4f);
    }
}
