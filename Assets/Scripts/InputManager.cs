using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[SerializeField] InputActionAsset inputActions;

	[SerializeField] PlayerMovement player1;
	[SerializeField] PlayerMovement player2;

	int? gamepad1ID;
	int? gamepad2ID;

	void OnEnable()
	{
		var inputActionMap = inputActions.FindActionMap("ActionMap", true);
		inputActionMap.Enable();

		BindInput(inputActionMap.FindAction("Move_Player1", true), Input_Move_Player1);
		BindInput(inputActionMap.FindAction("Move_Player2", true), Input_Move_Player2);
		BindInput(inputActionMap.FindAction("Move_Gamepad", true), Input_Move_Gamepad);

		BindInput(inputActionMap.FindAction("Jump_Player1", true), Input_Jump_Player1);
		BindInput(inputActionMap.FindAction("Jump_Player2", true), Input_Jump_Player2);
		BindInput(inputActionMap.FindAction("Jump_Gamepad", true), Input_Jump_Gamepad);
	}

	void OnDisable()
	{
		var inputActionMap = inputActions.FindActionMap("ActionMap", true);
		inputActionMap.Disable();

		UnbindInput(inputActionMap.FindAction("Move_Player1", true), Input_Move_Player1);
		UnbindInput(inputActionMap.FindAction("Move_Player2", true), Input_Move_Player2);
		UnbindInput(inputActionMap.FindAction("Move_Gamepad", true), Input_Move_Gamepad);

		UnbindInput(inputActionMap.FindAction("Jump_Player1", true), Input_Jump_Player1);
		UnbindInput(inputActionMap.FindAction("Jump_Player2", true), Input_Jump_Player2);
		UnbindInput(inputActionMap.FindAction("Jump_Gamepad", true), Input_Jump_Gamepad);
	}

	int GetPlayerNumberFromGamepadInput(int deviceId)
	{
		if (gamepad1ID == null) gamepad1ID = deviceId;
		else if (gamepad2ID == null) gamepad2ID = deviceId;

		if (deviceId == gamepad1ID) return 1;
		else if (deviceId == gamepad2ID) return 2;
		else return -1;
	}

	static void BindInput(InputAction action, Action<InputAction.CallbackContext> callback)
	{
		action.started += callback;
		action.performed += callback;
		action.canceled += callback;
	}

	static void UnbindInput(InputAction action, Action<InputAction.CallbackContext> callback)
	{
		action.started -= callback;
		action.performed -= callback;
		action.canceled -= callback;
	}

	void Input_Move_Player1(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();
		player1.Input_Move(value);
	}

	void Input_Move_Player2(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();
		player2.Input_Move(value);
	}

	void Input_Move_Gamepad(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();

		if (ctx.control.device is not Gamepad gamepad) return;
		var playerNumber = GetPlayerNumberFromGamepadInput(gamepad.deviceId);

		if (playerNumber == 1) player1.Input_Move(value);
		else if (playerNumber == 2) player2.Input_Move(value);
	}

	void Input_Jump_Player1(InputAction.CallbackContext ctx)
	{
		var value = ctx.phase == InputActionPhase.Performed;
		player1.Input_Jump(value);
	}

	void Input_Jump_Player2(InputAction.CallbackContext ctx)
	{
		var value = ctx.phase == InputActionPhase.Performed;
		player2.Input_Jump(value);
	}

	void Input_Jump_Gamepad(InputAction.CallbackContext ctx)
	{
		var value = ctx.phase == InputActionPhase.Performed;

		if (ctx.control.device is not Gamepad gamepad) return;
		var playerNumber = GetPlayerNumberFromGamepadInput(gamepad.deviceId);

		if (playerNumber == 1) player1.Input_Jump(value);
		else if (playerNumber == 2) player2.Input_Jump(value);
	}
}
