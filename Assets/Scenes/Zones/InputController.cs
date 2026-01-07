using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI;

public class InputController : MonoBehaviour
{
	[SerializeField] InputActionAsset inputActions_player1;
	[SerializeField] InputActionAsset inputActions_player2;

	[Header("Player1")]
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_moveCloser;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_moveAway;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_aim;

	[Header("Player2")]
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_moveCloser;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_moveAway;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_aim;

	int? gamepad1ID;
	int? gamepad2ID;

	void OnEnable()
	{
		var inputActionMap_player1 = inputActions_player1.FindActionMap("ActionMap", true);
		inputActionMap_player1.Enable();

		BindInput(inputActionMap_player1.FindAction("MoveCloser", true), Input_MoveCloser_Player1);
		BindInput(inputActionMap_player1.FindAction("MoveAway", true), Input_MoveAway_Player1);
		BindInput(inputActionMap_player1.FindAction("Aim", true), Input_Aim_Player1);

		var inputActionMap_player2 = inputActions_player2.FindActionMap("ActionMap", true);
		inputActionMap_player2.Enable();

		BindInput(inputActionMap_player2.FindAction("MoveCloser", true), Input_MoveCloser_Player2);
		BindInput(inputActionMap_player2.FindAction("MoveAway", true), Input_MoveAway_Player2);
		BindInput(inputActionMap_player2.FindAction("Aim", true), Input_Aim_Player2);
	}

	void OnDisable()
	{
		var inputActionMap_player1 = inputActions_player1.FindActionMap("ActionMap", true);
		inputActionMap_player1.Enable();

		UnbindInput(inputActionMap_player1.FindAction("MoveCloser", true), Input_MoveCloser_Player1);
		UnbindInput(inputActionMap_player1.FindAction("MoveAway", true), Input_MoveAway_Player1);
		UnbindInput(inputActionMap_player1.FindAction("Aim", true), Input_Aim_Player1);

		var inputActionMap_player2 = inputActions_player2.FindActionMap("ActionMap", true);
		inputActionMap_player2.Enable();

		UnbindInput(inputActionMap_player2.FindAction("MoveCloser", true), Input_MoveCloser_Player2);
		UnbindInput(inputActionMap_player2.FindAction("MoveAway", true), Input_MoveAway_Player2);
		UnbindInput(inputActionMap_player2.FindAction("Aim", true), Input_Aim_Player2);
	}

	int GetPlayerNumberFromDeviceId(int deviceId)
	{
		var s = "GetPlayerNumberFromDeviceId";
		s += "\ndeviceId: " + deviceId;

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

	void Input_MoveCloser_Player1(InputAction.CallbackContext ctx)
	{
		// If gamepad is not mapped to this player, ignore
		if (ctx.control.device is Gamepad gamepad)
		{
			var playerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (playerNumber != 1) return;
		}

		player1_moveCloser.Invoke(ctx);
	}

	void Input_MoveCloser_Player2(InputAction.CallbackContext ctx)
	{
		// If gamepad is not mapped to this player, ignore
		if (ctx.control.device is Gamepad gamepad)
		{
			var playerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (playerNumber != 2) return;
		}

		player2_moveCloser.Invoke(ctx);
	}

	void Input_MoveAway_Player1(InputAction.CallbackContext ctx)
	{
		// If gamepad is not mapped to this player, ignore
		if (ctx.control.device is Gamepad gamepad)
		{
			var playerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (playerNumber != 1) return;
		}

		player1_moveAway.Invoke(ctx);
	}

	void Input_MoveAway_Player2(InputAction.CallbackContext ctx)
	{
		// If gamepad is not mapped to this player, ignore
		if (ctx.control.device is Gamepad gamepad)
		{
			var playerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (playerNumber != 2) return;
		}

		player2_moveAway.Invoke(ctx);
	}

	void Input_Aim_Player1(InputAction.CallbackContext ctx)
	{
		// If gamepad is not mapped to this player, ignore
		if (ctx.control.device is Gamepad gamepad)
		{
			var playerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (playerNumber != 1) return;
		}

		player1_aim.Invoke(ctx);
	}

	void Input_Aim_Player2(InputAction.CallbackContext ctx)
	{
		// If gamepad is not mapped to this player, ignore
		if (ctx.control.device is Gamepad gamepad)
		{
			var playerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (playerNumber != 2) return;
		}

		player2_aim.Invoke(ctx);
	}

}
