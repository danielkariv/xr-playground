using Godot;
using System;

public class XRController : ARVRController
{
	/* Stuff we could do with Controllers:
	* .get_is_active = return true when controller active
	* .get_joystick_axis (int axis) = return value of given axis
	* .is_button_pressed = return if pressed button
	* Not working:
	* .get_mesh() = return Mesh of controller? (return nulls, so it doesn't working)
	* Rumble isn't working, there is a built-in one, when pressing trigger or grip, but not custom one ( need to research it)
	* No velocity or acceleration function at all ( need to built one myself)
	*/
	public struct ControllerInput
	{
		public float TriggerAxis, GripAxis, JoystickXAxis, JoystickYAxis;
		public int AXButton, BYButton, TriggerButton, GripButton;
	}
	private struct ControllerPositionTime
	{
		public Vector3 Position;
		public float delta;
	}
	private ControllerInput _controllerInput;
	public ControllerInput Input
	{ 
		get => _controllerInput; 
		set=> _controllerInput=value; 
	}
	private ControllerPositionTime _oldPositionTime;
	private Vector3 _velocity;
	public Vector3 Velocity
	{
		get => _velocity;
		set => _velocity = value;
	}
	public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(float delta)
    {
		_velocity = GetVelocity(delta);
	}
    public override void _Input(InputEvent @event)
	{
		if(@event is InputEventJoypadMotion || @event is InputEventJoypadButton)
			UpdateControllerInput();
	}
	/// <summary>
	/// Update controller inputs values.
	/// </summary>
	private void UpdateControllerInput()
	{
		_controllerInput.TriggerAxis = this.GetJoystickAxis((int)JoystickList.VrAnalogTrigger); // Trigger Axis (-1 to 1)
		_controllerInput.GripAxis = this.GetJoystickAxis((int)JoystickList.VrAnalogGrip); // Grip Axis (-1 to 1)
		_controllerInput.JoystickXAxis = this.GetJoystickAxis((int)JoystickList.Axis0); // X axis joystick
		_controllerInput.JoystickYAxis = this.GetJoystickAxis((int)JoystickList.Axis1); // Y axis joystick
		_controllerInput.AXButton = this.IsButtonPressed((int)JoystickList.Button7); // A/X button ( depends on controller)
		_controllerInput.BYButton = this.IsButtonPressed((int)JoystickList.Button1); // B/Y button ( depends on controller)
		_controllerInput.TriggerButton = this.IsButtonPressed((int)JoystickList.VrTrigger); // Trigger button
		_controllerInput.GripButton = this.IsButtonPressed((int)JoystickList.VrGrip); // Grip button
	}
	/// <summary>
	/// Calculate average velocity from last and current frame positions and delta times.
	/// </summary>
	/// <param name="delta">Delta time</param>
	/// <returns></returns>
	private Vector3 GetVelocity(float delta)
    {
		Vector3 velocity = _oldPositionTime.Position - this.GlobalTransform.origin;
		velocity /= _oldPositionTime.delta + delta;

		_oldPositionTime.Position = this.GlobalTransform.origin;
		_oldPositionTime.delta = delta;
		return velocity;
	}

	/// <summary>
	/// Prints controllers inputs values (DEBUG USAGE ONLY)
	/// </summary>
	private void debugPrintControllerInput()
	{
		GD.Print(string.Format("Pos: {0},{1} Trigger(Val:{2},Pressed:{3}) Grip(Val:{4},Pressed:{5}) AX={6} BY={7}",
				_controllerInput.JoystickXAxis, _controllerInput.JoystickYAxis,
				_controllerInput.TriggerAxis, _controllerInput.TriggerButton,
				_controllerInput.GripAxis, _controllerInput.GripButton,
				_controllerInput.AXButton, _controllerInput.BYButton));
	}
}
