using Godot;
using System;

namespace GameDev.Scripts;
public partial class Ladder : Area2D
{
	private PlayerVariables _playerVariables;

	public override void _Ready()
	{
		// get a singleton from AutoLoad.
		_playerVariables  = 
			GetNode<PlayerVariables>("/root/PlayerVariables");
	} 
	private void OnBodyEntered(Node2D body)
	{
		//GD.Print("On Body Entered.");
		// sets is_climbing to true to simulate climbing
		if ( body.Name == "Player" )
			_playerVariables.IsClimbing = true;		
	}

	private void OnBodyExited(Node2D body)
	{
		//GD.Print("On Body Existed.");
		// sets is_climbing to false to simulate climbing
		if ( body.Name == "Player" )
			_playerVariables.IsClimbing = false;		
	}
}
