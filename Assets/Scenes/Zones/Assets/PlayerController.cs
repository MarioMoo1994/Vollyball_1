using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Transform aimTarget;
	[SerializeField] GameObject serveBall;
	[SerializeField] int playerNumber;
	public int PlayerNumber => playerNumber;
	
	public int CurrentPlayerZone { get; private set; }
	public int CurrentTargetZone {  get; private set; }

	GameManager gameManager;

	void Start()
	{
		gameManager = FindFirstObjectByType<GameManager>();

		// Start at center zone
		MoveToZone(2);
		MoveTargetToZone(2);
	}

	// Move this player to the given zone index
	void MoveToZone(int zone)
	{
		CurrentPlayerZone = gameManager.ClampZoneIndex(playerNumber, zone);
		var zoneTransform = gameManager.GetZoneTransform(playerNumber, CurrentPlayerZone);

		transform.position = new Vector3(
			zoneTransform.position.x,
			transform.position.y,
			transform.position.z
		);
	}

	// Move this players target to the given zone index on the other players side
	void MoveTargetToZone(int zone)
	{
		var otherPlayerNumber = playerNumber == 1 ? 2 : 1;

		CurrentTargetZone = gameManager.ClampZoneIndex(otherPlayerNumber, zone);
		var zoneTransform = gameManager.GetZoneTransform(otherPlayerNumber, CurrentTargetZone);

		aimTarget.position = new Vector3(
			zoneTransform.position.x,
			aimTarget.position.y,
			aimTarget.position.z
		);
	}

	public void OnAllowServe()
	{
		serveBall.SetActive(true);
	}

	void SetPlayerSquashed(bool squashed)
	{
		float yScale = squashed ? 0.75f : 1f;
		transform.localScale = new Vector3(1, yScale, 1);
	}

	public void Input_MovePlayerLeft(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		MoveToZone(CurrentPlayerZone - 1);
	}

	public void Input_MovePlayerRight(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		MoveToZone(CurrentPlayerZone + 1);
	}

	public void Input_MoveTargetLeft(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		MoveTargetToZone(CurrentTargetZone - 1);
	}

	public void Input_MoveTargetRight(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		MoveTargetToZone(CurrentTargetZone + 1);
	}

	public void Input_Serve(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			gameManager.PlayerServeStart(this);
			SetPlayerSquashed(true);
		}
		else if (ctx.canceled)
		{
			if (gameManager.PlayerServeRelease(this))
			{
				// Disable ball visuals
				serveBall.SetActive(false);
			}

			SetPlayerSquashed(false);
		}
		else
		{
			// If phase is not started or cancelled, ignore
			return;
		}
	}
}
