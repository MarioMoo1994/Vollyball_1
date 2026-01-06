using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

	readonly List<Ball> balls = new();

	void Start()
	{
		SpawnBallCeiling(Side.Left, 2);
	}

	void Update()
	{
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
					ball.MoveDirection = Direction.Up;
				}
			}

			// Move
			var directionModifier = ball.MoveDirection == Direction.Up ? 1f : -1f;
			ball.Transform.position += directionModifier * ballMoveSpeed * Time.deltaTime * Vector3.up;
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
}
