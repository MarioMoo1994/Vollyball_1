using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball
{
	public Transform Transform;
	public int Zone;
	public bool MovingUp;
	public int CurrentSidePlayerNumber;
	public int LastHitPlayerNumber;
	public Vector3 RotationAxis;
}

public class GameManager : MonoBehaviour
{
	ScoreScript scoreScript;

	[SerializeField] float playerHitY = 3;
	[SerializeField] float floorHitY = 1;
	[SerializeField] float ceilingY = 10;
	[SerializeField] float ballMoveSpeed = 10;
	[SerializeField] float ballRotationSpeed = 500;

	[SerializeField] PlayerController player1;
	[SerializeField] PlayerController player2;

	[SerializeField] GameObject ballPrefab;

	// Left to right
	[Header("Zone Transforms (LEFT TO RIGHT!!!)")]
	[SerializeField] Transform[] player1Zones;
	[SerializeField] Transform[] player2Zones;

	[Header("Events")]
	[SerializeField] UnityEvent Player1_Point;
	[SerializeField] UnityEvent Player2_Point;

	readonly List<Ball> balls = new();

	float player1SpeedModifier = 1f;
	float player2SpeedModifier = 1f;

	int servingPlayer;
	bool serveStarted = false;
	bool serveCompleted = false;

	public void SetSpeedModifier(int playerNumber, float modifier)
	{
		if (playerNumber == 1) player1SpeedModifier = modifier;
		else if (playerNumber == 2) player2SpeedModifier = modifier;
	}

	void Start()
	{
		scoreScript = FindFirstObjectByType<ScoreScript>();
		servingPlayer = 1;

		var player = servingPlayer == 1 ? player1 : player2;
		player.OnAllowServe();
	}

	void Update()
	{
		var ballsToDestroy = new List<Ball>();

		if (!scoreScript.VictoryAchived)
		{
			foreach (var ball in balls)
			{
				// Update state
				if (ball.MovingUp)
				{
					// Switch sides if above ceiling
					if (ball.Transform.position.y > ceilingY)
					{
						// Switch side and movement direction
						ball.CurrentSidePlayerNumber = ball.CurrentSidePlayerNumber == 1 ? 2 : 1;
						ball.MovingUp = false;

						// Set zone to current target of the player that last hit it
						var relevantPlayer = ball.LastHitPlayerNumber == player1.PlayerNumber ? player1 : player2;
						ball.Zone = relevantPlayer.CurrentTargetZone;

						// Move gameobject to zone on other side
						var zoneTransform = GetZoneTransform(ball.CurrentSidePlayerNumber, ball.Zone);
						ball.Transform.position = new Vector3(zoneTransform.position.x, ceilingY, 0);
					}
				}
				else
				{
					var relevantPlayer = ball.CurrentSidePlayerNumber == player1.PlayerNumber ? player1 : player2;

					// Player hit
					if (ball.Zone == relevantPlayer.CurrentPlayerZone
						&& ball.Transform.position.y < playerHitY)
					{
						ball.MovingUp = true;
						ball.LastHitPlayerNumber = relevantPlayer.PlayerNumber;

						ball.RotationAxis = Random.onUnitSphere;
					}
					// Floor hit
					else if (ball.Transform.position.y < floorHitY)
					{
						if (ball.CurrentSidePlayerNumber == 2) Player1_Point.Invoke();
						else Player2_Point.Invoke();

						// Reset
						ballsToDestroy.Add(ball);

						serveStarted = false;
						serveCompleted = false;
						servingPlayer = ball.CurrentSidePlayerNumber;

						var player = servingPlayer == 1 ? player1 : player2;
						player.OnAllowServe();
					}
				}

				// Move
				var directionModifier = ball.MovingUp ? 1f : -1f;
				var speedModifier = ball.CurrentSidePlayerNumber == 1 ? player1SpeedModifier : player2SpeedModifier;
				var speed = ballMoveSpeed * speedModifier * directionModifier;

				ball.Transform.position += speed * Time.deltaTime * Vector3.up;

				// Rotate
				ball.Transform.Rotate(ballRotationSpeed * Time.deltaTime * ball.RotationAxis, Space.Self);
			}
		}
		else {
			foreach (var ball in balls) {
				ballsToDestroy.Add(ball);
			}

        }


		// Destroy
		foreach (var ballToDestroy in ballsToDestroy)
		{
			balls.Remove(ballToDestroy);
			Destroy(ballToDestroy.Transform.gameObject);
		}
	}

	public int ClampZoneIndex(int playerNumber, int zoneIndex)
	{
		var zoneTransforms = playerNumber == 1 ? player1Zones : player2Zones;
		return Mathf.Clamp(zoneIndex, 0, zoneTransforms.Length - 1);
	}

	public Transform GetZoneTransform(int playerNumber, int zoneIndex)
	{
		var zoneTransforms = playerNumber == 1 ? player1Zones : player2Zones;
		return zoneTransforms[ClampZoneIndex(playerNumber, zoneIndex)];
	}

	public void SpawnBallPlayer(int playerNumber)
	{
		var player = playerNumber == 1 ? player1 : player2;
		var zoneTransform = GetZoneTransform(playerNumber, player.CurrentPlayerZone);

		var obj = Instantiate(ballPrefab);
		obj.transform.position = new Vector3(zoneTransform.position.x, playerHitY, 0);

		var ball = new Ball()
		{
			Transform = obj.transform,
			Zone = player.CurrentPlayerZone,
			CurrentSidePlayerNumber = playerNumber,
			LastHitPlayerNumber = playerNumber,
			MovingUp = true,
			RotationAxis = Random.onUnitSphere
		};
		balls.Add(ball);
	}

	public bool PlayerServeStart(PlayerController player)
	{
		if (serveCompleted || serveStarted) return false;
		if (player.PlayerNumber != servingPlayer) return false;

		serveStarted = true;

		return true;
	}

	public bool PlayerServeRelease(PlayerController player)
	{
		if (serveCompleted) return false;
		if (player.PlayerNumber != servingPlayer) return false;

		serveCompleted = true;
		SpawnBallPlayer(player.PlayerNumber);

		return true;
	}
}
