using Godot;
using System;

public class PickupHand : Spatial
{

	[Export(PropertyHint.ResourceType,"ControllerService")]
	private NodePath _controllerPath = null;
	[Export(PropertyHint.ResourceType, "Shape")]
	private SphereShape _shape = null;

	private ControllerService _controller;

	private RigidBody holdingTarget = null;
	private PhysicsShapeQueryParameters _physicsShape;

	public override void _Ready()
	{
		Node node = GetNode(_controllerPath);
		if (node == null || !(node is ControllerService))
		{
			GD.PrintErr("Failed to get controller reference");
		}
		else
		{
			_controller = (ControllerService)node;
		}

		if (_shape == null)
		{
			GD.PrintErr("Failed to get shape reference");
		}
		_physicsShape = new PhysicsShapeQueryParameters();
		_physicsShape.ShapeRid = _shape.GetRid();
		_physicsShape.Margin = _shape.Margin;
		// TODO: would be better to pick a layer for pickup objects only. right now set for everything.
		//_physicsShape.CollisionMask = 0x7fffffff;
		_physicsShape.CollideWithBodies = true;
		_physicsShape.CollideWithAreas = false;

		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _PhysicsProcess(float delta)
	{
		bool grabstatus = false;
		if (_controller != null)
			grabstatus = _controller.IsGripPressed;
		if (!grabstatus)
		{
			PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
			_physicsShape.Transform = this.GlobalTransform;
			Godot.Collections.Array result = spaceState.IntersectShape(_physicsShape, 16);
			// if we got results, checking for closest object and targeting it.
			if (result.Count > 0)
			{
				holdingTarget = null;
				float distance = float.MaxValue;
				
				foreach (Godot.Collections.Dictionary item in result)
				{
					
					if (item["collider"] is RigidBody body)
					{
						float dist = body.GlobalTransform.origin.DistanceTo(GlobalTransform.origin);
						if (distance >= dist)
						{
							distance = dist;
							holdingTarget = body;
						}
					}
				}
			}
			else
			{
				holdingTarget = null;
			}
		}
		else
		{
			if(holdingTarget != null)
			{
				// adjust velocity to move toward hand.
				//holdingTarget.LinearVelocity = (Transform.origin - holdingTarget.Transform.origin) / delta;
				//GD.Print("Velo: " + holdingTarget.LinearVelocity);
				holdingTarget.GlobalTransform = GlobalTransform;
				
				// adjust angular velocity to rotate to hand.
				//holdingTarget.AngularDamp

			}
		}
	}
}
