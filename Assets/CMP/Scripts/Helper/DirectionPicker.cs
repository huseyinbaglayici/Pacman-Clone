using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CMP.Scripts.Helper
{
    public static class DirectionPicker
    {
        public static Direction PickRandom(Vector2Int pos, Direction heading, Func<Vector2Int, bool> isWalkable)
        {
            Direction reverse = heading.Reverse();
            var candidates = new List<Direction>();

            foreach (var dir in GameSettings.DirectionsToCheck)
            {
                if (dir == reverse)
                    continue;
                if (isWalkable(pos + dir.ToVector2Int()))
                    candidates.Add(dir);
            }

            if (candidates.Count > 0)
                return candidates[Random.Range(0, candidates.Count)];

            return isWalkable(pos + reverse.ToVector2Int()) ? reverse : Direction.None;
        }
    }
}