using Godot;
using System;

public class PanelInteractor : Spatial
{

	private RayCast _rayCast;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rayCast = (RayCast)GetNode("RayCast");
	}

	// TODO: a little bit hacky, probably could use signals.
	public override void _PhysicsProcess(float delta)
	{
		_rayCast.ForceRaycastUpdate();
		Godot.Object obj = _rayCast.GetCollider();
		if (obj != null && obj is Panel3D panel)
		{
			Vector3 pos = _rayCast.GetCollisionPoint();
			float distance = _rayCast.GlobalTransform.origin.DistanceTo(pos);
			panel.TouchPanelFromPosition(pos, distance);
		}
	}
}
