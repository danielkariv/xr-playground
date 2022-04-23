using Godot;
using System;

public class ControllerService : ARVRController
{
	private SpatialVelocityTracker velocityTracker;
	public Vector3 Velocity = Vector3.Zero;
	// Buttons, Joysticks and such ( Input variables)
	public Vector2 PrimaryJoystick = Vector2.Zero;
	public bool IsJoystickPressed = false, IsJoystickJustPressed = false, IsJoystickJustReleased = false;
	public float TriggerAnalog = -1.0f;
	public bool IsTriggerPressed = false, IsTriggerJustPressed = false, IsTriggerJustReleased = false;
	public float GripAnalog = -1.0f;
	public bool IsGripPressed = false, IsGripJustPressed = false, IsGripJustReleased = false;
	public bool IsAXPressed = false, IsAXJustPressed = false, IsAXJustReleased = false;
	public bool IsBYPressed = false, IsBYJustPressed = false, IsBYJustReleased = false;
	public bool IsMenuPressed = false, IsMenuJustPressed = false, IsMenuJustReleased = false;
	public bool IsSelectPressed = false, IsSelectJustPressed = false, IsSelectJustReleased = false;

	public override void _Ready()
	{
		velocityTracker = new SpatialVelocityTracker();
		velocityTracker.TrackPhysicsStep = true;
		velocityTracker.Reset(GlobalTransform.origin);
		
	}
	public override void _Process(float delta)
	{
		if (this.GetIsActive()){
			TriggerAnalog = this.GetJoystickAxis((int)JoystickList.VrAnalogTrigger); // Trigger Axis (-1 to 1)
			bool triggerbutton = this.IsButtonPressed((int)JoystickList.VrTrigger) == 1;
			IsTriggerJustPressed = (!IsTriggerPressed && triggerbutton && !IsTriggerJustPressed);
			IsTriggerJustReleased = (IsTriggerPressed && !triggerbutton && !IsTriggerJustReleased);
			IsTriggerPressed = triggerbutton; // Trigger button
			
			GripAnalog = this.GetJoystickAxis((int)JoystickList.VrAnalogGrip); // Grip Axis (-1 to 1)
			bool gripbutton = this.IsButtonPressed((int)JoystickList.VrGrip) == 1;
			IsGripJustPressed = (!IsGripPressed && gripbutton && !IsGripJustPressed);
			IsGripJustReleased = (IsGripPressed && !gripbutton && !IsGripJustReleased);
			IsGripPressed = gripbutton; // Grip button

			PrimaryJoystick.x = this.GetJoystickAxis((int)JoystickList.Axis0); // X axis Primary Joystick
			PrimaryJoystick.y = this.GetJoystickAxis((int)JoystickList.Axis1); // Y axis Primary Joystick
			bool joystickbutton = this.IsButtonPressed((int)JoystickList.VrPad) == 1;
			IsJoystickJustPressed = (!IsJoystickPressed && joystickbutton && !IsJoystickJustPressed);
			IsJoystickJustReleased = (IsJoystickPressed && !joystickbutton && !IsJoystickJustReleased);
			IsJoystickPressed = joystickbutton; // Joystick button

			bool axbutton = this.IsButtonPressed((int)JoystickList.Button7) == 1;
			IsAXJustPressed = (!IsAXPressed && axbutton && !IsAXJustPressed);
			IsAXJustReleased = (IsAXPressed && !axbutton && !IsAXJustReleased);
			IsAXPressed = axbutton; // A/X button ( depends on controller)
			
			bool bybutton = this.IsButtonPressed((int)JoystickList.Button1) == 1;
			IsBYJustPressed = (!IsBYPressed && bybutton && !IsBYJustPressed);
			IsBYJustReleased = (IsBYPressed && !bybutton && !IsBYJustReleased);
			IsBYPressed = bybutton; // B/Y button ( depends on controller)

			bool menubutton = this.IsButtonPressed((int)JoystickList.Button3) == 1;
			IsMenuJustPressed = (!IsMenuPressed && menubutton && !IsMenuJustPressed);
			IsMenuJustReleased = (IsMenuPressed && !menubutton && !IsMenuJustReleased);
			IsMenuPressed = menubutton; // Menu button

			bool selectbutton = this.IsButtonPressed((int)JoystickList.Button4) == 1;
			IsSelectJustPressed = (!IsSelectPressed && selectbutton && !IsSelectJustPressed);
			IsSelectJustReleased = (IsSelectPressed && !selectbutton && !IsSelectJustReleased);
			IsSelectPressed = selectbutton;  // Select button
			
			velocityTracker.UpdatePosition(GlobalTransform.origin);
			Velocity = velocityTracker.GetTrackedLinearVelocity();
		}else {
			Velocity = Vector3.Zero;
		}
	}
	
}
