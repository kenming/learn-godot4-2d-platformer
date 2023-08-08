using Godot;
using System;

namespace GameDev.Scripts;
public partial class Player : CharacterBody2D
{
	// player movement variables
	[Export]
	public float Speed {get; set; } = 100.0f;
	[Export]
	public float Gravity {get; set; } = 200.0f; 

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print(delta);
		
		Vector2 velocity = Velocity;
		// vertical movement velocity (down)
		velocity.Y += Gravity * (float)delta;
		// horizontal movement processing (left, right)
		Horizontal_movement(velocity);
		// applies movement
		Velocity = velocity;
		MoveAndSlide();
    }

	private void Horizontal_movement(Vector2 velocity)
	{
		// if keys are pressed it will return 1 for ui_right, -1 for ui_left, and 0 for neither
		var Horizontal_input = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        // horizontal velocity which moves player left or right based on input 
        velocity.X = Horizontal_input * Speed;
    }
}
