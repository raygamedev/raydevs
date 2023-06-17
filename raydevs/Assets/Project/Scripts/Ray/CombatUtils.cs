namespace Raydevs.Ray
{
    using UnityEngine;

    public static class CombatUtils
    {
        public static int GetDirectionBetweenPoints(Vector2 src, Vector2 dst)
        {
            return src.x > dst.x ? 1 : -1;
        }
    }
}