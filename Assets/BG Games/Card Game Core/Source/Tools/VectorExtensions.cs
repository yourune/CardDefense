using UnityEngine;

namespace BG_Games.Card_Game_Core.Tools
{
    public static class VectorExtensions
    {
        public static Vector3 MultiplyByCoordinates(this Vector3 vector1, Vector3 vector2)
        {
            float x = vector1.x * vector2.x;
            float y = vector1.y * vector2.y;
            float z = vector1.z * vector2.z;

            return new Vector3(x, y, z);
        }

        public static Vector3 DevideByCoordinates(this Vector3 vector1, Vector3 vector2)
        {
            float x = vector1.x / vector2.x;
            float y = vector1.y / vector2.y;
            float z = vector1.z / vector2.z;

            return new Vector3(x, y, z);
        }
    }
}
