using Godot;
using System;

public class SampleBall : RigidBody, IIntractable
{
	ControllerService holdTarget = null;
  
	public override void _Ready()
	{
		
	}

	public void Pickup(ControllerService controller){
		GD.Print("SampleBall picked up by controller.");
		holdTarget = controller;
	}

	public void Drop(ControllerService controller){
		GD.Print("SampleBall will now be dropped.");
		if ( controller == holdTarget){
			holdTarget = null;
			this.LinearVelocity = controller.Velocity;
		}
		else
			GD.PrintErr("Wrong Controller tries to remove itself from interactable object.");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (holdTarget != null){
			this.GlobalTransform = holdTarget.GlobalTransform;
			this.LinearVelocity = Vector3.Zero;
		}
	}
}
