using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Transform aimTarget;
	[SerializeField] Side side;
	[SerializeField] GameObject serveBall;

	public Side Side => side;

	List<Transform> leftZones;
	List<Transform> rightZones;
	
	int currentZone;
	public int CurrentZone => currentZone;
	public int TargetZone => Mathf.Clamp(currentZone + targetZoneOffset, 0, ZoneUtil.NumberOfZones);

	bool aiming;
	int targetZoneOffset;

	GameManager gameManager;

	void Start()
	{
		gameManager = FindFirstObjectByType<GameManager>();

		ZoneUtil.GetZones(out leftZones, out rightZones);

		// Start at center zone
		MoveToZone(2);
	}

	// Move this player to the given zone index
	void MoveToZone(int zone)
	{
		currentZone = Mathf.Clamp(zone, 0, ZoneUtil.NumberOfZones - 1);
		var zoneTransform = ZoneUtil.GetZoneTransform(side, currentZone);

		transform.position = new Vector3(
			zoneTransform.position.x,
			transform.position.y,
			transform.position.z
		);
	}

	// Move this players target to the given zone index on the other players side
	void MoveTargetToZone(int zone)
	{
		// Inverse
		var zoneTransforms = side == Side.Left ? rightZones : leftZones;
		var clampedZoneIndex = Mathf.Clamp(zone, 0, ZoneUtil.NumberOfZones - 1);
		var targetZone = zoneTransforms[clampedZoneIndex];

		aimTarget.position = new Vector3(
			targetZone.position.x,
			aimTarget.position.y,
			aimTarget.position.z
		);
	}

	// Input - Move closer to net
	public void Input_MoveCloser(InputAction.CallbackContext ctx)
	{
		if (!ctx.started) return;

		if (aiming)
		{
			// If aiming, add to target offset in opposite direction
			targetZoneOffset += 1;
		}
		else
		{
			// Move player
			MoveToZone(currentZone - 1);
			targetZoneOffset = 0;
		}

		// Update target position
		MoveTargetToZone(currentZone + targetZoneOffset);
	}

	// Input - Move away from net
	public void Input_MoveAway(InputAction.CallbackContext ctx)
	{
		if (!ctx.started) return;

		if (aiming)
		{
			targetZoneOffset -= 1; // Flip left/right
		}
		else
		{
			MoveToZone(currentZone + 1);
			targetZoneOffset = 0;
		}

		MoveTargetToZone(currentZone + targetZoneOffset);
	}

	public void Input_Aim(InputAction.CallbackContext ctx)
	{
		if (ctx.started)
		{
			aiming = true;

			gameManager.PlayerServeStart(this);
		}
		else if (ctx.canceled)
		{
			aiming = false;

			if (gameManager.PlayerServeRelease(this, TargetZone))
			{
				// Disable visuals
				serveBall.SetActive(false);
			}
		}
		else
		{
			// If phase is not started or cancelled, ignore
			return;
		}

		// Reset target position
		targetZoneOffset = 0;
		MoveTargetToZone(currentZone + targetZoneOffset);

		// Visual
		float yScale = aiming ? 0.75f : 1f;
		transform.localScale = new Vector3(1, yScale, 1);
	}

	public void OnAllowServe()
	{
		serveBall.SetActive(true);
	}
}
