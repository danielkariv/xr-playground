using Godot;
using System;

public class Panel3D : Area, IIntractable
{
	// TODO: set panel size from script, building viewport and mesh size to fit.
	private MeshInstance _mesh;
	private CollisionShape _collisionShape;
	private Viewport _viewport;

	
	[Export]
	private float _viewportWidth = 720.0f;
	[Export]
	private float _depth = 0.05f;
	[Export]
	private float _clickDistance = 0.033f;
	private Vector2 _screenSize;
	
	private ControllerService controller = null;
	private Node originalParent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mesh = (MeshInstance)this.GetNode("Mesh");
		_collisionShape = (CollisionShape)this.GetNode("CollisionShape");
		_viewport = (Viewport)this.GetNode("Mesh/Viewport");
		originalParent = this.GetParent();
		InitPanel();
	}
	public override void _PhysicsProcess(float delta)
	{
		
	}
	/// <summary>
	/// Initilized the UI Panel. 
	/// Set viewport resoultion based on panel sizing and viewport width in pixels.
	/// </summary>
	private void InitPanel()
	{
		if (_mesh.Mesh is QuadMesh quad)
		{
			_screenSize = quad.Size;
			if (_collisionShape.Shape is BoxShape shape)
			{
				shape.Extents = new Vector3(_screenSize.x/2, _screenSize.y/2, _depth);
				_collisionShape.Transform = _collisionShape.Transform.Translated(new Vector3(0, 0, -_depth));
			}
		}
		_viewport.Size = new Vector2(_viewportWidth, (_viewportWidth / _screenSize.x) * _screenSize.y);
	}

	/// <summary>
	/// Adds mouse input event in viewport based on digital touching the panel.
	/// </summary>
	/// <param name="pos">3D position of touching the panel</param>
	/// <param name="distance">the distance of tool/finger to position</param>
	public void TouchPanelFromPosition(Vector3 pos, float distance)
	{
		Vector2 uiPos = GetPositionToViewport(pos);

		// Mouse motion event, deals with moving the mosue around in panel.
		InputEventMouseMotion inputEvent = new InputEventMouseMotion();
		inputEvent.ButtonMask = (int)ButtonList.Left;
		inputEvent.Pressure = distance;
		inputEvent.Position = uiPos;
		inputEvent.GlobalPosition = uiPos;
		if (_viewport != null)
			_viewport.Input(inputEvent);

		// Based on distanct, calling left button's pressed or released action.
		if (distance < _clickDistance)
		{
			InputEventMouseButton mouseButton = new InputEventMouseButton();
			mouseButton.ButtonIndex = (int)ButtonList.Left;
			mouseButton.Pressed = true;
			mouseButton.Position = uiPos;
			mouseButton.GlobalPosition = uiPos;
			if (_viewport != null)
				_viewport.Input(mouseButton);
		}
		else
		{
			InputEventMouseButton mouseButton = new InputEventMouseButton();
			mouseButton.ButtonIndex = (int)ButtonList.Left;
			mouseButton.Pressed = false;
			mouseButton.Position = uiPos;
			mouseButton.GlobalPosition = uiPos;
			if (_viewport != null)
				_viewport.Input(mouseButton);
		}
}

	/// <summary>
	/// Get position in viewport world based on 3D position of contact.
	/// Works only with place-like models.
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public Vector2 GetPositionToViewport(Vector3 pos)
	{
		Transform colTransform = _collisionShape.GlobalTransform;
		Vector3 uiPos = colTransform.XformInv(pos);
		uiPos.x = ((uiPos.x / _screenSize.x) + 0.5f) * _viewport.Size.x;
		uiPos.y = (0.5f - (uiPos.y / _screenSize.y)) * _viewport.Size.y;
		return new Vector2(uiPos.x, uiPos.y);
	}

	public void Pickup(ControllerService controller){
		GD.Print("Panel3D picked up by controller.");
		Transform global = this.GlobalTransform;
		this.controller = controller;
		this.GetParent().RemoveChild(this);
		this.controller.AddChild(this);
		this.GlobalTransform = global;
	}

	public void Drop(ControllerService controller){
		GD.Print("Panel3D will now be dropped.");
		if (this.controller == controller){
			Transform global = this.GlobalTransform;
			this.controller = null;
			this.GetParent().RemoveChild(this);
			this.originalParent.AddChild(this);
			this.GlobalTransform = global;
		}
		else
			GD.PrintErr("Wrong Controller tries to remove itself from interactable object.");
	}
}
