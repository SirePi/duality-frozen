using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Chebychev : Distance
    {
        private static Chebychev __instance;

        private Chebychev()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static Chebychev Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new Chebychev();

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override float GetDistance(Vector2 a, Vector2 b)
        {
            float dx = MathF.Abs(a.X - b.X);
            float dy = MathF.Abs(a.Y - b.Y);
            return MathF.Max(dx, dy);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class Distance
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public abstract float GetDistance(Vector2 a, Vector2 b);
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Euclidean : Distance
    {
        private static Euclidean __instance;

        private Euclidean()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static Euclidean Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new Euclidean();

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override float GetDistance(Vector2 a, Vector2 b)
        {
            return (a - b).Length;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class EuclideanSquared : Distance
    {
        private static EuclideanSquared __instance;

        private EuclideanSquared()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static EuclideanSquared Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new EuclideanSquared();

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override float GetDistance(Vector2 a, Vector2 b)
        {
            return (a - b).LengthSquared;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Manhattan : Distance
    {
        private static Manhattan __instance;

        private Manhattan()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static Manhattan Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new Manhattan();

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override float GetDistance(Vector2 a, Vector2 b)
        {
            float dx = MathF.Abs(a.X - b.X);
            float dy = MathF.Abs(a.Y - b.Y);
            return dx + dy;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Minkowski : Distance
    {
        private static Minkowski __instance;
        private float _coefficient;

        /// <summary>
        ///
        /// </summary>
        /// <param name="coefficient"></param>
        public Minkowski(float coefficient)
        {
            _coefficient = coefficient;
        }

        /// <summary>
        ///
        /// </summary>
        public static Minkowski Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new Minkowski(4);

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override float GetDistance(Vector2 a, Vector2 b)
        {
            float dx = MathF.Abs(a.X - b.X);
            float dy = MathF.Abs(a.Y - b.Y);

            float ddx = MathF.Pow(dx, _coefficient);
            float ddy = MathF.Pow(dy, _coefficient);
            return MathF.Pow(ddx + ddy, 1 / _coefficient);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Quadratic : Distance
    {
        private static Quadratic __instance;

        private Quadratic()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static Quadratic Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new Quadratic();

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override float GetDistance(Vector2 a, Vector2 b)
        {
            float dx = MathF.Abs(a.X - b.X);
            float dy = MathF.Abs(a.Y - b.Y);
            return dx * dx + dy * dy + dx * dy;
        }
    }
}