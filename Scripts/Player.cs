using Godot;
using System;

namespace GameDev.Scripts;
public partial class Player : CharacterBody2D
{
	// player movement variables
	[Export]
	public float Speed {get; set; } = 150.0f;
	[Export]
	public float Gravity {get; set; } = 100.0f; 
	[Export]
	public float Jump_height {get; set; } = -60f; 

	private Vector2 _velocity ;
	private Vector2 _position ;
	private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _collisionShape;

	// movement states.
	// ref. https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html
	private PlayerVariables _playerVariables;
	public override void _Ready()
	{	
		// GD.Print("Ready : ");
		// get a singleton from AutoLoad.
		_playerVariables  = 
			GetNode<PlayerVariables>("/root/PlayerVariables");
		_velocity = Velocity;
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_position = _collisionShape.Position;
		// register the event handle function.
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}
	public override void _PhysicsProcess(double delta)
	{
		// GD.Print(delta);
		
		// vertical movement velocity (down)
		_velocity.Y += Gravity * (float)delta;
		
		// horizontal movement processing (left, right)
		Horizontal_movement();
		
		// applies movement
		Velocity = _velocity;
		MoveAndSlide();

		// applies animations
		if (!_playerVariables.IsAttacking || !_playerVariables.IsClimbing)
		{			
			Player_animations();
			_collisionShape.Position = _position;
		}
    }

	private void Horizontal_movement()
	{
		// if keys are pressed it will return 1 for ui_right, -1 for ui_left, and 0 for neither
		var Horizontal_input = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        // horizontal velocity which moves player left or right based on input 
        _velocity.X = Horizontal_input * Speed;
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
			_position.X = 7f;
		}

		// on right (add is_action_just_released so you continue running after jumping)
		if (Input.IsActionPressed("ui_right") || Input.IsActionJustReleased("ui_jump"))
		{
			//GD.Print("ui_right.");
			_animatedSprite.FlipH = false;
			_animatedSprite.Play("run");
			_position.X = -7f;
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
				// GD.Print("Attack Key pressed.");
				_playerVariables.IsAttacking = true;
				_animatedSprite.Play("attack");			
			} 

			// on jump
			if (eventKey.IsActionPressed("ui_jump") && IsOnFloor())
			{
				//GD.Print("Jump Key pressed.");
				_velocity.Y = Jump_height;
				_animatedSprite.Play("jump");			
			} 
			
			// on climbing ladders			
			if (_playerVariables.IsClimbing)
			{
				if (eventKey.IsActionPressed("ui_up"))
				{
					//GD.Print("Climb Up Key pressed.");
					_animatedSprite.Play("climb");
					Gravity = 50f;
					_velocity.Y = -60f;
				}				
			}
			else	// reset gravity
				{
					Gravity = 100f;
					_playerVariables.IsClimbing = false;
				}
		}		           
	}

	// ref. https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html#doc-signals
	// reset animation variables.
	private void OnAnimationFinished()
	{
		//GD.Print("On Animation Finished.");
		// Timer Delay : ToSignal(GetTree().CreateTimer(1.5f), "timeout");
		_playerVariables.IsAttacking = false;
		//_playerVariables.IsClimbing = false;
	}
}
