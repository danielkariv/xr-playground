using Godot;
using System;

public class XRBodyController : ARVROrigin
{
    private XRController LController, RController;
	private ARVRCamera camera;

	private float speed = 5.0f;
	public override void _Ready()
	{
		LController = (XRController)(ARVRController)GetNode("LeftController");
		RController = (XRController)(ARVRController)GetNode("RightController");
		camera = (ARVRCamera)GetNode("ARVRCamera");

		if (!(LController is XRController))
			GD.PrintErr("Couldn't find Left Controller.");
		if (!(RController is XRController))
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
		Vector3 joystickPos = new Vector3(LController.Input.JoystickXAxis, 0, LController.Input.JoystickYAxis);
		joystickPos = Vector3.Zero;
		joystickPos += -camera.Transform.basis.z * LController.Input.JoystickYAxis;
		joystickPos += camera.Transform.basis.x * LController.Input.JoystickXAxis;
		this.Transform = this.Transform.Translated(joystickPos * speed * delta);
	}
	/// <summary>
	/// Update X and Z axis position based on Left Controller's joystick and Left Controller's forward vector.
	/// </summary>
	/// <param name="delta">Delta time</param>
	public void updateFreeMoveController(float delta)
	{
		Vector3 joystickPos = new Vector3(LController.Input.JoystickXAxis, 0, LController.Input.JoystickYAxis);
		joystickPos = Vector3.Zero;
		joystickPos += -LController.Transform.basis.z * LController.Input.JoystickYAxis;
		joystickPos += LController.Transform.basis.x * LController.Input.JoystickXAxis;
		this.Transform = this.Transform.Translated(joystickPos * speed * delta);
	}
	/// <summary>
	/// Update Y axis rotation based on Right Controller's joystick.
	/// </summary>
	/// <param name="delta">Delta time</param>
	public void updateSmoothRotation(float delta)
    {
		if (RController.Input.JoystickXAxis > 0.5f)
		{
			this.Rotate(Vector3.Up, -2 * delta);
		}
		else if (RController.Input.JoystickXAxis < -0.5f)
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
		if (RController.Input.JoystickYAxis > 0.5f)
		{
			this.Transform = this.Transform.Translated(this.Transform.basis.y * speed * delta);
		}
		else if (RController.Input.JoystickYAxis < -0.5f)
		{
			this.Transform = this.Transform.Translated(-this.Transform.basis.y * speed * delta);
		}
	}
}
