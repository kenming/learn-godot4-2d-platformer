using Godot;
using System;
using System.Runtime.CompilerServices;


namespace GameDev.Scripts;
public partial class Bomb : Area2D
{
    Timer _timer;		// Timer Node.
    private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _collisionShape;

    public override void _Ready()
	{
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");        		
        _timer = GetNode<Timer>("Timer");

        // register the signals to handle.		
		this.BodyEntered += OnBodyEntered;
		_timer.Timeout += OnTimerTimeout;
	}

    private void OnBodyEntered(Node2D body)
    {
        // GD.Print("On Body Entered : " + body.Name);
        // if the bomb collides with the player, play the explosion animation and start the timer
        if ( body.Name == "Player" )
            Explode();

        // OPTION 1 :
        // if the bomb collides with our Level Tilemap (floor and walls).
        /*
        if (body.Name == "Level")
            Explode();
        */

        // OPTION 2 :
        // if the bomb collides with our Wall scene, explode and remove
        if (body.Name.ToString().StartsWith("Wall"))
            Explode();
    }

    private void Explode()
    {
        _animatedSprite.Play("explode");
        _timer.Start();
    }

    // remove the bomb from the scene only if the Bomb exists
    private void OnTimerTimeout()
    {
        // GD.Print("On Timer Timeout.");
        // remove the bomb from the scene only if the Bomb exists
        if (GodotObject.IsInstanceValid(this))
        {
            this.QueueFree();
        }
    }
}
