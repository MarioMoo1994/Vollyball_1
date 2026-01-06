using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball
{
	public Transform Transform;
	public int Zone;
	public int ZoneAfterSwitch;
	public Side Side;
	public Direction MoveDirection;
}

public class GameManager : MonoBehaviour
{
	[SerializeField] float playerHitY = 3;
	[SerializeField] float floorHitY = 1;
	[SerializeField] float ceilingY = 10;
	[SerializeField] float ballMoveSpeed = 10;

	[SerializeField] PlayerController player1;
	[SerializeField] PlayerController player2;

	[SerializeField] GameObject ballPrefab;

	[Header("Events")]
	[SerializeField] UnityEvent Player1_Point;
	[SerializeField] UnityEvent Player2_Point;

	readonly List<Ball> balls = new();

	float leftSpeedModifier = 1f;
	float rightSpeedModifier = 1f;

	Side servingSide;
	bool serveStarted = false;
	bool serveCompleted = false;

	public void SetSpeedModifier(Side side, float modifier)
	{
		if (side == Side.Left) leftSpeedModifier = modifier;
		else if (side == Side.Right) rightSpeedModifier = modifier;
	}

	void Start()
	{
		servingSide = Side.Left;
		var player = servingSide == Side.Left ? player1 : player2;
		player.OnAllowServe();
	}

	void Update()
	{
		var ballsToDestroy = new List<Ball>();

		foreach (var ball in balls)
		{
			// Update state
			if (ball.MoveDirection == Direction.Up)
			{
				// Switch sides if above ceiling
				if (ball.Transform.position.y > ceilingY)
				{
					ball.Side = ball.Side == Side.Left ? Side.Right : Side.Left;
					ball.MoveDirection = Direction.Down;

					var zoneTransform = ZoneUtil.GetZoneTransform(ball.Side, ball.ZoneAfterSwitch);
					ball.Transform.position = new Vector3(zoneTransform.position.x, ceilingY, 0);

					ball.Zone = ball.ZoneAfterSwitch;
				}
			}
			else
			{
				// Player hit
				var relevantPlayer = ball.Side == player1.Side ? player1 : player2;
				if (ball.Zone == relevantPlayer.CurrentZone
					&& ball.Transform.position.y < playerHitY)
				{
					ball.MoveDirection = Direction.Up;
					ball.ZoneAfterSwitch = relevantPlayer.TargetZone;
				}
				// Floor hit
				else if (ball.Transform.position.y < floorHitY)
				{
					if (ball.Side == Side.Right) Player1_Point.Invoke();
					else Player2_Point.Invoke();

					// Reset
					ballsToDestroy.Add(ball);

					serveStarted = false;
					serveCompleted = false;
					servingSide = ball.Side;

					var player = servingSide == Side.Left ? player1 : player2;
					player.OnAllowServe();
				}
			}

			// Move
			var directionModifier = ball.MoveDirection == Direction.Up ? 1f : -1f;
			var speedModifier = ball.Side == Side.Left ? leftSpeedModifier : rightSpeedModifier;
			var speed = ballMoveSpeed * speedModifier * directionModifier;

			ball.Transform.position += speed * Time.deltaTime * Vector3.up;
		}

		// Destroy
		foreach (var ballToDestroy in ballsToDestroy)
		{
			balls.Remove(ballToDestroy);
			Destroy(ballToDestroy.Transform.gameObject);
		}
	}

	public void SpawnBallCeiling(Side side, int zone)
	{
		var clampedZoneIndex = Mathf.Clamp(zone, 0, ZoneUtil.NumberOfZones - 1);
		var zoneTransform = ZoneUtil.GetZoneTransform(side, zone);

		var obj = Instantiate(ballPrefab);
		obj.transform.position = new Vector3(zoneTransform.position.x, ceilingY, 0);

		var ball = new Ball()
		{
			Transform = obj.transform,
			Zone = clampedZoneIndex,
			ZoneAfterSwitch = clampedZoneIndex,
			Side = side,
			MoveDirection = Direction.Down
		};
		balls.Add(ball);
	}

	public void SpawnBallPlayer(Side side, int targetZone)
	{
		var player = side == Side.Left ? player1 : player2;
		var zoneTransform = ZoneUtil.GetZoneTransform(side, player.CurrentZone);

		var obj = Instantiate(ballPrefab);
		obj.transform.position = new Vector3(zoneTransform.position.x, playerHitY, 0);

		var ball = new Ball()
		{
			Transform = obj.transform,
			Zone = player.CurrentZone,
			ZoneAfterSwitch = targetZone,
			Side = side,
			MoveDirection = Direction.Up
		};
		balls.Add(ball);
	}

	public bool PlayerServeStart(PlayerController player)
	{
		if (serveCompleted || serveStarted) return false;

		serveStarted = true;

		return true;
	}

	public bool PlayerServeRelease(PlayerController player, int targetZone)
	{
		if (serveCompleted) return false;

		serveCompleted = true;
		SpawnBallPlayer(player.Side, targetZone);

		return true;
	}
}
