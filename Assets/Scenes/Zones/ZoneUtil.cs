using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ZoneUtil
{
	public const int NumberOfZones = 5;

	public static void GetZones(out List<Transform> left, out List<Transform> right)
	{
		left = new List<Transform>();
		right = new List<Transform>();

		for (int i = 0; i < NumberOfZones; i++)
		{
			var leftZoneName = $"Zone L{i}";
			var leftTransform = FindTransformByName(leftZoneName);
			left.Add(leftTransform);

			var rightZoneName = $"Zone R{i}";
			var rightTransform = FindTransformByName(rightZoneName);
			right.Add(rightTransform);
		}
	}

	public static Transform GetZoneTransform(Side side, int zoneIndex)
	{
		GetZones(out var left, out var right);

		var zoneTransforms = side == Side.Left ? left : right;
		var clampedZoneIndex = Mathf.Clamp(zoneIndex, 0, NumberOfZones - 1);
		return zoneTransforms[clampedZoneIndex];
	}

	static Transform FindTransformByName(string name)
	{
		return Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
			.First(x => x.name == name);
	}
}