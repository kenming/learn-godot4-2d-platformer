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
	[Export]
	public float Jump_height {get; set; } = -100f; 

	private Vector2 velocity ;
	//private Vector2 position ;
	private AnimatedSprite2D _animatedSprite;
	//private CollisionShape2D _collisionShape;

	// movement states.
	private bool Is_attacking;
	private bool Is_climbing;

	public override void _Ready()
	{
		Is_climbing = false;
		Is_attacking = false;
		velocity = Velocity;
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		//_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		// register the event handle function.
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}
	public override void _PhysicsProcess(double delta)
	{
		// GD.Print(delta);		
		// vertical movement velocity (down)
		velocity.Y += Gravity * (float)delta;
		
		// horizontal movement processing (left, right)
		Horizontal_movement(velocity);
		
		// applies movement
		Velocity = velocity;
		MoveAndSlide();

		// applies animations
		if (!Is_attacking)
			Player_animations();
    }

	private void Horizontal_movement(Vector2 velocity)
	{
		// if keys are pressed it will return 1 for ui_right, -1 for ui_left, and 0 for neither
		var Horizontal_input = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        // horizontal velocity which moves player left or right based on input 
        velocity.X = Horizontal_input * Speed;
    }

	// ref. https://docs.godotengine.org/en/4.1/tutorials/2d/2d_sprite_animation.html
	private void Player_animations()
	{		
		// on left (add is_action_just_released so you continue running after jumping)
		if (Input.IsActionPressed("ui_left") || Input.IsActionJustReleased("ui_jump"))
		{
			//GD.Print("ui_left.");
			_animatedSprite.FlipH = true;
			_animatedSprite.Play("run");
			//position.X = 7f;
			//_collisionShape.Position = position;
		}

		// on right (add is_action_just_released so you continue running after jumping)
		if (Input.IsActionPressed("ui_right") || Input.IsActionJustReleased("ui_jump"))
		{
			//GD.Print("ui_right.");
			_animatedSprite.FlipH = false;
			_animatedSprite.Play("run");
			//position.X = -7f;
			//_collisionShape.Position = position;
		}

		// on idle if nothing is being pressed
		if (!Input.IsAnythingPressed())
		{	
			_animatedSprite.Play("idle");
		}
	}

	// ref. https://docs.godotengine.org/en/stable/tutorials/inputs/inputevent.html
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			// on attack
			if (eventKey.IsActionPressed("ui_attack"))
			{
				//GD.Print("Attack Key pressed.");
				Is_attacking = true;
				_animatedSprite.Play("attack");			
			} 

			// on jump
			if (eventKey.IsActionPressed("ui_jump") && IsOnFloor())
			{
				//GD.Print("Jump Key pressed.");
				velocity.Y = Jump_height;
				_animatedSprite.Play("jump");			
			} 

			// on climbing ladders
			if (Is_climbing)
				if (eventKey.IsActionPressed("ui_up"))
					{
						//GD.Print("Climb Up Key pressed.");
						_animatedSprite.Play("climb");
						Gravity = 100f;
						velocity.Y = -200;
					}
				else	// reset gravity
					{
						Gravity = 200;
						Is_climbing = false;
					}
		}
		           
	}

	// ref. https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html#doc-signals
	// trigger event to handle.
	private void OnAnimationFinished()
	{
		// Timer Delay : ToSignal(GetTree().CreateTimer(1.5f), "timeout");
		Is_attacking = false;
		Is_climbing = false;
	}
}
