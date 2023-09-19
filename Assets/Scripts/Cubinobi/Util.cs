using System;
using UnityEngine;

namespace Cubinobi
{
    public static class Util
    {
        public static bool IsZero(float x, float precision = 0.001f)
        {
            return Math.Abs(x) < precision;
        }

        public static bool IsNotZero(float x, float precision = 0.001f)
        {
            return Math.Abs(x) > precision;
        }

        public static bool FloatEquals(float x, float y, float precision = 0.001f)
        {
            return Math.Abs(x - y) < precision;
        }

        public static Vector2 Swap(Vector2 v)
        {
            return new Vector2(v.y, v.x);
        }
    }
}