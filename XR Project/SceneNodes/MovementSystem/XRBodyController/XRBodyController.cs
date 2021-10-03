using Godot;
using System;

public class XRBodyController : ARVROrigin
{
	private ControllerService _lController, _rController;
	private ARVRCamera camera;

	private float _speed = 5.0f;
	public override void _Ready()
	{
		_lController = (ControllerService)(ARVRController)GetNode("LeftController");
		_rController = (ControllerService)(ARVRController)GetNode("RightController");
		camera = (ARVRCamera)GetNode("ARVRCamera");

		if (!(_lController is ControllerService))
			GD.PrintErr("Couldn't find Left Controller.");
		if (!(_rController is ControllerService))
			GD.PrintErr("Couldn't find Right Controller.");
		if (!(camera is ARVRCamera))
			GD.PrintErr("Couldn't find Camera(HMD).");
	}

	public override void _PhysicsProcess(float delta)
	{
		// Free form movement ( based on Camera forward)
		//updateFreeMoveCamera(delta);
		// Free form movement ( based on Left Controller forward)
		updateFreeMoveController(delta);
		// Smooth Rotation ( based on Right Controller joystick)
		updateSmoothRotation(delta);

		updateRiseFallMovement(delta);
	}
	/// <summary>
	/// Update X and Z axis position based on Left Controller's joystick and Camera\HMD's forward vector.
	/// </summary>
	/// <param name="delta">Delta time</param>
	public void updateFreeMoveCamera(float delta)
	{
		Vector3 joystickPos = new Vector3(_lController.Input.JoystickXAxis, 0, _lController.Input.JoystickYAxis);
		joystickPos = Vector3.Zero;
		joystickPos += -camera.Transform.basis.z * _lController.Input.JoystickYAxis;
		joystickPos += camera.Transform.basis.x * _lController.Input.JoystickXAxis;
		this.Transform = this.Transform.Translated(joystickPos * _speed * delta);
	}
	/// <summary>
	/// Update X and Z axis position based on Left Controller's joystick and Left Controller's forward vector.
	/// </summary>
	/// <param name="delta">Delta time</param>
	public void updateFreeMoveController(float delta)
	{
		Vector3 joystickPos = new Vector3(_lController.Input.JoystickXAxis, 0, _lController.Input.JoystickYAxis);
		joystickPos = Vector3.Zero;
		joystickPos += -_lController.Transform.basis.z * _lController.Input.JoystickYAxis;
		joystickPos += _lController.Transform.basis.x * _lController.Input.JoystickXAxis;
		this.Transform = this.Transform.Translated(joystickPos * _speed * delta);
	}
	/// <summary>
	/// Update Y axis rotation based on Right Controller's joystick.
	/// </summary>
	/// <param name="delta">Delta time</param>
	public void updateSmoothRotation(float delta)
	{
		if (_rController.Input.JoystickXAxis > 0.5f)
		{
			this.Rotate(Vector3.Up, -2 * delta);
		}
		else if (_rController.Input.JoystickXAxis < -0.5f)
		{
			this.Rotate(Vector3.Up, 2 * delta);
		}
	}
	/// <summary>
	/// Update Y axis position based on Right Controller's joystick. 
	/// </summary>
	/// <param name="delta">Delta time</param>
	public void updateRiseFallMovement(float delta)
	{
		if (_rController.Input.JoystickYAxis > 0.5f)
		{
			this.Transform = this.Transform.Translated(this.Transform.basis.y * _speed * delta);
		}
		else if (_rController.Input.JoystickYAxis < -0.5f)
		{
			this.Transform = this.Transform.Translated(-this.Transform.basis.y * _speed * delta);
		}
	}
}
