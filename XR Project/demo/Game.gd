extends Spatial


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var VR = ARVRServer.find_interface("OpenXR")
	if VR and VR.initialize():
		get_viewport().arvr = true
		get_viewport().keep_3d_linear = true;
		# TODO: find display frame rate and set the physics fps to it.
		OS.vsync_enabled = false
		Engine.target_fps = 90
		# Also, the physics FPS in the project settings is also 90 FPS. This makes the physics
		# run at the same frame rate as the display, which makes things look smoother in VR!


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if (Input.is_key_pressed(KEY_ESCAPE)):
		get_tree().quit()
	elif (Input.is_key_pressed(KEY_SPACE)):
		# Calling center_on_hmd will cause the ARVRServer to adjust all tracking data so the player is centered on the origin point looking forward
		ARVRServer.center_on_hmd(true, true)
	pass
