using Godot;
using System;

namespace GameDev.Scripts;
public partial class Platform : Area2D
{
	private State _currentState;
	private Timer _timer;		// Timer Node.

	// movement position and movement progress value
	private Vector2 _position;
	private float _initialPosition;
	private float _progress = 0f;

	// platforms movement speed and range
	[Export]
	public float MovementSpeed {get; set; } = 50.0f;
	[Export]
	public float MovementRange {get; set; } = 50f;

	// wait times
	[Export]
	public float WaitTimeAtTop {get; set; } = 3.0f;
	[Export]
	public float WaitTimeAtBottom {get; set; } = 3.0f;	

	public override void _Ready()
	{
		_position = Position;
		// sets platforms y position and state on game start.
		//_currentState = State.WAIT_AT_BOTTOM;
		_initialPosition = _position.Y;
		SwitchState(State.MOVING_UP);

		// register the Timer.Timeout signal to handle.		
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		float weight = _progress / (MovementRange / MovementSpeed);
		float to = _initialPosition - MovementRange;

		// about lerp function, see : https://blog.csdn.net/weixin_37818081/article/details/112427780
		switch (_currentState)
		{
			case State.MOVING_UP:
				_progress += (float)delta;
				// change its position
				_position.Y =  Mathf.Lerp(_initialPosition, to, weight);
				if (_progress >= (MovementRange / MovementSpeed))
					SwitchState(State.WAIT_AT_TOP);
				Position = _position;
				break;

			case State.MOVING_DOWN:
				_progress -= (float)delta;
				// change its position
				_position.Y =  Mathf.Lerp(_initialPosition, to, weight);
				if (_progress <= 0)
					SwitchState(State.WAIT_AT_BOTTOM);
				Position = _position;			
				break;
		}

	}
	
	private void SwitchState(State new_state)
	{
		_currentState = new_state;

		switch (new_state)
		{
			case State.MOVING_UP:		// if state is moving up, reset progress
				_progress = 0.0f;
				break;
			
			// if state is waiting at top, start the timer to change the state
			case State.WAIT_AT_TOP:
				_timer.WaitTime = WaitTimeAtTop;
				_timer.Start();
				break;
			
			// if state is waiting at bottom, start the timer to change the state
			case State.WAIT_AT_BOTTOM:
				_timer.WaitTime = WaitTimeAtBottom;
				_timer.Start();
				break;
			
			// if state is moving down, move the platform via the speed and range defined
			case State.MOVING_DOWN:
				_progress = MovementRange / MovementSpeed;
				break;
		}
	}

	// signal : platform direction changes on timer timeout
	private void OnTimerTimeout()
	{
		//GD.Print("On Timer Timeout.");

		if (_currentState == State.WAIT_AT_TOP)
			SwitchState(State.MOVING_DOWN);

		if (_currentState == State.WAIT_AT_BOTTOM)
			SwitchState(State.MOVING_UP);
	}
	
}

// platform movement states
enum State {WAIT_AT_BOTTOM, MOVING_UP, WAIT_AT_TOP, MOVING_DOWN}
