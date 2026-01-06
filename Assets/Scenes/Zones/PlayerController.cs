using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	enum Side
	{
		Left, 
		Right
	}

	[SerializeField] InputActionAsset inputActions;
	[SerializeField] Transform aimTarget;
	[SerializeField] Side side;

	List<Transform> leftZones;
	List<Transform> rightZones;
	
	const int numberOfZones = 5;
	int currentZone;
	bool aiming;
	int targetZoneOffset;

	void OnEnable()
	{
		BindInput();
	}

	void OnDisable()
	{
		UnbindInput();
	}

	void Start()
	{
		// Find zone objects
		leftZones = new List<Transform>();
		rightZones = new List<Transform>();
		for (int i = 0; i < numberOfZones; i++)
		{
			var leftZoneName = $"Zone L{i}";
			var leftTransform = FindTransformByName(leftZoneName);
			leftZones.Add(leftTransform);

			var rightZoneName = $"Zone R{i}";
			var rightTransform = FindTransformByName(rightZoneName);
			rightZones.Add(rightTransform);
		}

		// Start at center zone
		MoveToZone(2);
	}

	// Move this player to the given zone index
	void MoveToZone(int zone)
	{
		var zoneTransforms = side == Side.Left ? leftZones : rightZones;

		currentZone = Mathf.Clamp(zone, 0, numberOfZones - 1);
		var currentPosition = zoneTransforms[currentZone].position;

		transform.position = new Vector3(
			currentPosition.x,
			transform.position.y,
			transform.position.z
		);
	}

	// Move this players target to the given zone index on the other players side
	void MoveTargetToZone(int zone)
	{
		// Inverse
		var zoneTransforms = side == Side.Left ? rightZones : leftZones;
		var clampedZoneIndex = Mathf.Clamp(zone, 0, numberOfZones - 1);
		var targetZone = zoneTransforms[clampedZoneIndex];

		aimTarget.position = new Vector3(
			targetZone.position.x,
			aimTarget.position.y,
			aimTarget.position.z
		);
	}

	// Input - Move closer to net
	void Input_MoveCloser(InputAction.CallbackContext ctx)
	{
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
	void Input_MoveAway(InputAction.CallbackContext ctx)
	{
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

	void Input_Aim(InputAction.CallbackContext ctx)
	{
		// Reset target position
		targetZoneOffset = 0;
		MoveTargetToZone(currentZone + targetZoneOffset);

		if (ctx.started)
		{
			aiming = true;
		}
		else if (ctx.canceled)
		{
			aiming = false;
		}

		// Visual
		float yScale = aiming ? 0.75f : 1f;
		transform.localScale = new Vector3(1, yScale, 1);
    }

	void BindInput()
	{
		var actionMap = inputActions.FindActionMap("ActionMap");
		actionMap.Enable();

		var moveLeft = actionMap.FindAction("MoveCloser");
		moveLeft.started += Input_MoveCloser;

		var moveRight = actionMap.FindAction("MoveAway");
		moveRight.started += Input_MoveAway;

		var aim = actionMap.FindAction("Aim");
		aim.started += Input_Aim;
		aim.canceled += Input_Aim;
	}

	void UnbindInput()
	{
		var actionMap = inputActions.FindActionMap("ActionMap");
		actionMap.Enable();

		var moveLeft = actionMap.FindAction("MoveCloser");
		moveLeft.started -= Input_MoveCloser;

		var moveRight = actionMap.FindAction("MoveAway");
		moveRight.started -= Input_MoveAway;

		var aim = actionMap.FindAction("Aim");
		aim.started -= Input_Aim;
		aim.canceled -= Input_Aim;
	}

	static Transform FindTransformByName(string name)
	{
		return FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
			.First(x => x.name == name);
	}
}
