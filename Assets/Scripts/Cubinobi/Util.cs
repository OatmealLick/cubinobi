using System;

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
    }
}