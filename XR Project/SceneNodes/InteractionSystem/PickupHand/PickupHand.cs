using Godot;
using System;

public class PickupHand : Spatial
{

	[Export(PropertyHint.ResourceType,"ControllerService")]
	private NodePath _controllerPath = null;
	[Export(PropertyHint.ResourceType, "Shape")]
	private SphereShape _shape = null;

	private ControllerService _controller;

	private CollisionObject holdingTarget = null;
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
		_physicsShape.CollideWithAreas = true;
	}
	
	 public override void _PhysicsProcess(float delta)
	{
		if(_controller == null)
			return;
		if(_controller.IsGripJustReleased){
			((IIntractable)holdingTarget).Drop(_controller);
			holdingTarget = null;
		}else if (_controller.IsGripPressed){
			if(holdingTarget == null){
				PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
				_physicsShape.Transform = this.GlobalTransform;
				Godot.Collections.Array result = spaceState.IntersectShape(_physicsShape);
				// if we got results, checking for closest object and targeting it.
				if (result.Count > 0)
				{
					holdingTarget = null;
					float distance = float.MaxValue;
					foreach (Godot.Collections.Dictionary item in result)
					{
						// check the item for Interactable interface, and if it is a CollisionObject.
						// then choose the closest to the controller.
						if (item["collider"] is IIntractable && item["collider"] is CollisionObject body)
						{
							float dist = body.GlobalTransform.origin.DistanceTo(GlobalTransform.origin);
							if (distance >= dist)
							{
								distance = dist;
								holdingTarget = body;
								((IIntractable)holdingTarget).Pickup(_controller);
							}
						}
					}
				}
			}
		}
	}
}
