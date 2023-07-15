namespace Raydevs.Ray
{
    using UnityEngine;

    public static class CombatUtils
    {
        public static int GetDirectionBetweenPoints(Vector2 src, Vector2 dst) => src.x > dst.x ? 1 : -1;

        public static int GetRandomHitDamage(int min, int max) => Random.Range(min, max);
    }
}