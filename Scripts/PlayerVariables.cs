using Godot;
using System;

namespace GameDev.Scripts;
public partial class PlayerVariables : Node
{
    public bool IsAttacking {get; set;}
	public bool IsClimbing  {get; set;}
    public bool IsJumping  {get; set;}

    public override void _Ready()
	{
        // initialize.
		IsAttacking = false;
        IsClimbing = false;
        IsJumping = false;
	} 
}
