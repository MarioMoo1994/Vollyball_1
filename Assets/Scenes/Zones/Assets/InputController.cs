using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
	[SerializeField] InputActionAsset inputActions_player1;
	[SerializeField] InputActionAsset inputActions_player2;

	[Header("Player1")]
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_movePlayerLeft;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_movePlayerRight;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_moveTargetLeft;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_moveTargetRight;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player1_serve;

	[Header("Player2")]
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_movePlayerLeft;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_movePlayerRight;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_moveTargetLeft;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_moveTargetRight;
	[SerializeField] UnityEvent<InputAction.CallbackContext> player2_serve;

	[Header("Any Controller")]
	[SerializeField] UnityEvent<InputAction.CallbackContext> controller_Start;
	[SerializeField] UnityEvent<InputAction.CallbackContext> controller_Back;

	readonly List<int> gamepad1Ids = new();
	readonly List<int> gamepad2Ids = new();

	void OnEnable()
	{
		var inputActionMap_player1 = inputActions_player1.FindActionMap("ActionMap", true);
		inputActionMap_player1.Enable();

		BindInput(inputActionMap_player1.FindAction("MovePlayerLeft", true), Input_MovePlayerLeft_Player1);
		BindInput(inputActionMap_player1.FindAction("MovePlayerRight", true), Input_MovePlayerRight_Player1);
		BindInput(inputActionMap_player1.FindAction("MoveTargetLeft", true), Input_MoveTargetLeft_Player1);
		BindInput(inputActionMap_player1.FindAction("MoveTargetRight", true), Input_MoveTargetRight_Player1);
		BindInput(inputActionMap_player1.FindAction("Serve", true), Input_Serve_Player1);
		BindInput(inputActionMap_player1.FindAction("Start", true), Input_Start);
		BindInput(inputActionMap_player1.FindAction("Back", true), Input_Back);

		var inputActionMap_player2 = inputActions_player2.FindActionMap("ActionMap", true);
		inputActionMap_player2.Enable();

		BindInput(inputActionMap_player2.FindAction("MovePlayerLeft", true), Input_MovePlayerLeft_Player2);
		BindInput(inputActionMap_player2.FindAction("MovePlayerRight", true), Input_MovePlayerRight_Player2);
		BindInput(inputActionMap_player2.FindAction("MoveTargetLeft", true), Input_MoveTargetLeft_Player2);
		BindInput(inputActionMap_player2.FindAction("MoveTargetRight", true), Input_MoveTargetRight_Player2);
		BindInput(inputActionMap_player2.FindAction("Serve", true), Input_Serve_Player2);
		BindInput(inputActionMap_player2.FindAction("Start", true), Input_Start);
		BindInput(inputActionMap_player2.FindAction("Back", true), Input_Back);
	}

	void OnDisable()
	{
		var inputActionMap_player1 = inputActions_player1.FindActionMap("ActionMap", true);
		inputActionMap_player1.Enable();

		UnbindInput(inputActionMap_player1.FindAction("MovePlayerLeft", true), Input_MovePlayerLeft_Player1);
		UnbindInput(inputActionMap_player1.FindAction("MovePlayerRight", true), Input_MovePlayerRight_Player1);
		UnbindInput(inputActionMap_player1.FindAction("MoveTargetLeft", true), Input_MoveTargetLeft_Player1);
		UnbindInput(inputActionMap_player1.FindAction("MoveTargetRight", true), Input_MoveTargetRight_Player1);
		UnbindInput(inputActionMap_player1.FindAction("Serve", true), Input_Serve_Player1);
		UnbindInput(inputActionMap_player1.FindAction("Start", true), Input_Start);
		UnbindInput(inputActionMap_player1.FindAction("Back", true), Input_Back);

		var inputActionMap_player2 = inputActions_player2.FindActionMap("ActionMap", true);
		inputActionMap_player2.Enable();

		UnbindInput(inputActionMap_player2.FindAction("MovePlayerLeft", true), Input_MovePlayerLeft_Player2);
		UnbindInput(inputActionMap_player2.FindAction("MovePlayerRight", true), Input_MovePlayerRight_Player2);
		UnbindInput(inputActionMap_player2.FindAction("MoveTargetLeft", true), Input_MoveTargetLeft_Player2);
		UnbindInput(inputActionMap_player2.FindAction("MoveTargetRight", true), Input_MoveTargetRight_Player2);
		UnbindInput(inputActionMap_player2.FindAction("Serve", true), Input_Serve_Player2);
		UnbindInput(inputActionMap_player2.FindAction("Start", true), Input_Start);
		UnbindInput(inputActionMap_player2.FindAction("Back", true), Input_Back);
	}

	int GetPlayerNumberFromDeviceId(int deviceId)
	{
		// If bound to a player, return that player number
		if (gamepad1Ids.Contains(deviceId)) return 1;
		if (gamepad2Ids.Contains(deviceId)) return 2;

		// Bind to player with fewest devices bound to them
		// Return player number device was bound to
		// Map to player 1 first
		if (gamepad1Ids.Count <= gamepad2Ids.Count)
		{
			gamepad1Ids.Add(deviceId);
			return 1;
		}
		else
		{
			gamepad2Ids.Add(deviceId);
			return 2;
		}
	}

	bool InputAllowedForPlayer(InputAction.CallbackContext ctx, int playerNumber)
	{
		// If gamepad is not mapped to this player, dont allow
		if (ctx.control.device is Gamepad gamepad)
		{
			var deviceMappedPlayerNumber = GetPlayerNumberFromDeviceId(gamepad.deviceId);
			if (deviceMappedPlayerNumber != playerNumber) return false;
		}
		
		return true;
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

	// Player 1

	void Input_MovePlayerLeft_Player1(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 1)) return;

		player1_movePlayerLeft.Invoke(ctx);
	}

	void Input_MovePlayerRight_Player1(InputAction.CallbackContext ctx)
	{

		if (!InputAllowedForPlayer(ctx, 1)) return;

		player1_movePlayerRight.Invoke(ctx);
	}

	void Input_MoveTargetLeft_Player1(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 1)) return;

		player1_moveTargetLeft.Invoke(ctx);
	}

	void Input_MoveTargetRight_Player1(InputAction.CallbackContext ctx)
	{

		if (!InputAllowedForPlayer(ctx, 1)) return;

		player1_moveTargetRight.Invoke(ctx);
	}

	void Input_Serve_Player1(InputAction.CallbackContext ctx)
	{

		if (!InputAllowedForPlayer(ctx, 1)) return;

		player1_serve.Invoke(ctx);
	}

	// Player 2

	void Input_MovePlayerLeft_Player2(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 2)) return;

		player2_movePlayerLeft.Invoke(ctx);
	}

	void Input_MovePlayerRight_Player2(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 2)) return;

		player2_movePlayerRight.Invoke(ctx);
	}

	void Input_MoveTargetLeft_Player2(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 2)) return;

		player2_moveTargetLeft.Invoke(ctx);
	}

	void Input_MoveTargetRight_Player2(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 2)) return;

		player2_moveTargetRight.Invoke(ctx);
	}

	void Input_Serve_Player2(InputAction.CallbackContext ctx)
	{
		if (!InputAllowedForPlayer(ctx, 2)) return;

		player2_serve.Invoke(ctx);
	}

	// Any controller

	void Input_Start(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		controller_Start.Invoke(ctx);
	}

	void Input_Back(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		controller_Back.Invoke(ctx);
	}
}
