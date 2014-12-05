// This code is provided under Creative Commons Attribution Share Alike license. Originally by Ilya Suzdalnitski.
// http://wiki.unity3d.com/index.php/Tileable_Noise

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Noise
{
    /// <summary>
    ///
    /// </summary>
    public class SimplexNoise : Noise
    {
        /// <summary>
        ///
        /// </summary>
        public static SimplexNoise CLOUDS = new SimplexNoise(4, 2, .5f, 16, 1f / 16);

        private static readonly int[][] GRAD3 = new int[][]{
                        new int[]{1, 1, 0},
                        new int[]{-1, 1, 0},
                        new int[]{1, -1, 0},
                        new int[]{-1, -1, 0},
                        new int[]{1, 0, 1},
                        new int[]{-1, 0, 1},
                        new int[]{1, 0, -1},
                        new int[]{-1, 0, -1},
                        new int[]{0, 1, 1},
                        new int[]{0, -1, 1},
                        new int[]{0, 1, -1},
                        new int[]{0, -1, -1} };

        private static readonly int[][] GRAD4 = new int[][]{
                        new int[]{ 0, 1, 1, 1 },
                        new int[]{ 0, 1, 1, -1 },
                        new int[]{ 0, 1, -1, 1 },
                        new int[]{ 0, 1, -1, -1 },
                        new int[]{ 0, -1, 1, 1 },
                        new int[]{ 0, -1, 1, -1 },
                        new int[]{ 0, -1, -1, 1 },
                        new int[]{ 0, -1, -1, -1 },
                        new int[]{ 1, 0, 1, 1 },
                        new int[]{ 1, 0, 1, -1 },
                        new int[]{ 1, 0, -1, 1 },
                        new int[]{ 1, 0, -1, -1 },
                        new int[]{ -1, 0, 1, 1 },
                        new int[]{ -1, 0, 1, -1 },
                        new int[]{ -1, 0, -1, 1 },
                        new int[]{ -1, 0, -1, -1 },
                        new int[]{ 1, 1, 0, 1 },
                        new int[]{ 1, 1, 0, -1 },
                        new int[]{ 1, -1, 0, 1 },
                        new int[]{ 1, -1, 0, -1 },
                        new int[]{ -1, 1, 0, 1 },
                        new int[]{ -1, 1, 0, -1 },
                        new int[]{ -1, -1, 0, 1 },
                        new int[]{ -1, -1, 0, -1 },
                        new int[]{ 1, 1, 1, 0 },
                        new int[]{ 1, 1, -1, 0 },
                        new int[]{ 1, -1, 1, 0 },
                        new int[]{ 1, -1, -1, 0 },
                        new int[]{ -1, 1, 1, 0 },
                        new int[]{ -1, 1, -1, 0 },
                        new int[]{ -1, -1, 1, 0 },
                        new int[]{ -1, -1, -1, 0 } };

        // A lookup table to traverse the simplex around a given point in 4D.
        // Details can be found where this table is used, in the 4D Noise method.
        private static readonly int[][] simplex = new int[][]{
            new int[]{0,1,2,3}, new int[]{0,1,3,2}, new int[]{0,0,0,0}, new int[]{0,2,3,1}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{1,2,3,0},
            new int[]{0,2,1,3}, new int[]{0,0,0,0}, new int[]{0,3,1,2}, new int[]{0,3,2,1}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{1,3,2,0},
            new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0},
            new int[]{1,2,0,3}, new int[]{0,0,0,0}, new int[]{1,3,0,2}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{2,3,0,1}, new int[]{2,3,1,0},
            new int[]{1,0,2,3}, new int[]{1,0,3,2}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{2,0,3,1}, new int[]{0,0,0,0}, new int[]{2,1,3,0},
            new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0},
            new int[]{2,0,1,3}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{3,0,1,2}, new int[]{3,0,2,1}, new int[]{0,0,0,0}, new int[]{3,1,2,0},
            new int[]{2,1,0,3}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{3,1,0,2}, new int[]{0,0,0,0}, new int[]{3,2,0,1}, new int[]{3,2,1,0}
        };

        private float _gain;
        private float _lacunarity;
        private int _octaves;

        // To remove the need for index wrapping, float the permutation table length
        private int[] _permutationTable = new int[512];

        private float _startAmplitude;
        private float _startFrequency;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inOctaves"></param>
        /// <param name="inGain"></param>
        /// <param name="inLacunarity"></param>
        /// <param name="inStartFrequency"></param>
        /// <param name="inStartAmplitude"></param>
        public SimplexNoise(int inOctaves, float inGain, float inLacunarity, float inStartFrequency, float inStartAmplitude)
        {
            _octaves = inOctaves;
            _gain = inGain;
            _lacunarity = inLacunarity;
            _startFrequency = inStartFrequency;
            _startAmplitude = inStartAmplitude;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stepSize"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float BlurredNoise(float stepSize, float x, float y)
        {
            int totalIterations = 0;
            float noiseSum = 0.0f;

            for (float xx = x - stepSize; xx <= x + stepSize; xx += stepSize)
            {
                for (float yy = y - stepSize; yy <= y + stepSize; yy += stepSize)
                {
                    noiseSum += Noise(xx, yy);
                    totalIterations++;
                }
            }

            return noiseSum / (float)totalIterations;
        }

        /// <summary>
        /// 2D Simplex Noise
        /// </summary>
        /// <param name="xin"></param>
        /// <param name="yin"></param>
        /// <returns></returns>
        public float Noise(float xin, float yin)
        {
            float n0, n1, n2; // Noise contributions from the three corners
            // Skew the input space to determine which simplex cell we're in
            float F2 = 0.5f * (MathF.Sqrt(3.0f) - 1.0f);
            float s = (xin + yin) * F2; // Hairy factor for 2D
            int i = FastMath.FastFloor(xin + s);
            int j = FastMath.FastFloor(yin + s);
            float G2 = (3.0f - MathF.Sqrt(3.0f)) / 6.0f;
            float t = (i + j) * G2;
            float X0 = i - t; // Unskew the cell origin back to (x,y) space
            float Y0 = j - t;
            float x0 = xin - X0; // The x,y distances from the cell origin
            float y0 = yin - Y0;
            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0)
            {
                i1 = 1;
                j1 = 0;
            } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else
            {
                i1 = 0;
                j1 = 1;
            } // upper triangle, YX order: (0,0)->(0,1)->(1,1)
            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6
            float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            float y1 = y0 - j1 + G2;
            float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            float y2 = y0 - 1.0f + 2.0f * G2;
            // Work out the hashed gradient indices of the three simplex corners
            int ii = i & 255;
            int jj = j & 255;
            int gi0 = _permutationTable[ii + _permutationTable[jj]] % 12;
            int gi1 = _permutationTable[ii + i1 + _permutationTable[jj + j1]] % 12;
            int gi2 = _permutationTable[ii + 1 + _permutationTable[jj + 1]] % 12;
            // Calculate the contribution from the three corners
            float t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0)
                n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * dot(GRAD3[gi0], x0, y0); // (x,y) of grad3 used for 2D gradient
            }
            float t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0)
                n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * dot(GRAD3[gi1], x1, y1);
            }
            float t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0)
                n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * dot(GRAD3[gi2], x2, y2);
            }
            // Add contributions from each corner to get the final Noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 70.0f * (n0 + n1 + n2);
        }

        /// <summary>
        /// 3D Simplex Noise
        /// </summary>
        /// <param name="xin"></param>
        /// <param name="yin"></param>
        /// <param name="zin"></param>
        /// <returns></returns>
        public float Noise(float xin, float yin, float zin)
        {
            float n0, n1, n2, n3; // Noise contributions from the four corners
            // Skew the input space to determine which simplex cell we're in
            const float F3 = 1.0f / 3.0f;
            float s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D
            int i = FastMath.FastFloor(xin + s);
            int j = FastMath.FastFloor(yin + s);
            int k = FastMath.FastFloor(zin + s);
            const float G3 = 1.0f / 6.0f; // Very nice and simple unskew factor, too
            float t = (i + j + k) * G3;
            float X0 = i - t; // Unskew the cell origin back to (x,y,z) space
            float Y0 = j - t;
            float Z0 = k - t;
            float x0 = xin - X0; // The x,y,z distances from the cell origin
            float y0 = yin - Y0;
            float z0 = zin - Z0;
            // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
            // Determine which simplex we are in.
            int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                } // X Y Z order
                else if (x0 >= z0)
                {
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                } // X Z Y order
                else
                {
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                } // Z X Y order
            }
            else
            { // x0<y0
                if (y0 < z0)
                {
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                } // Z Y X order
                else if (x0 < z0)
                {
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                } // Y Z X order
                else
                {
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                } // Y X Z order
            }
            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
            // c = 1/6.
            float x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
            float y1 = y0 - j1 + G3;
            float z1 = z0 - k1 + G3;
            float x2 = x0 - i2 + 2.0f * G3; // Offsets for third corner in (x,y,z) coords
            float y2 = y0 - j2 + 2.0f * G3;
            float z2 = z0 - k2 + 2.0f * G3;
            float x3 = x0 - 1.0f + 3.0f * G3; // Offsets for last corner in (x,y,z) coords
            float y3 = y0 - 1.0f + 3.0f * G3;
            float z3 = z0 - 1.0f + 3.0f * G3;
            // Work out the hashed gradient indices of the four simplex corners
            int ii = i & 255;
            int jj = j & 255;
            int kk = k & 255;
            int gi0 = _permutationTable[ii + _permutationTable[jj + _permutationTable[kk]]] % 12;
            int gi1 = _permutationTable[ii + i1 + _permutationTable[jj + j1 + _permutationTable[kk + k1]]] % 12;
            int gi2 = _permutationTable[ii + i2 + _permutationTable[jj + j2 + _permutationTable[kk + k2]]] % 12;
            int gi3 = _permutationTable[ii + 1 + _permutationTable[jj + 1 + _permutationTable[kk + 1]]] % 12;
            // Calculate the contribution from the four corners
            float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 < 0)
                n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * dot(GRAD3[gi0], x0, y0, z0);
            }
            float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 < 0)
                n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * dot(GRAD3[gi1], x1, y1, z1);
            }
            float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 < 0)
                n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * dot(GRAD3[gi2], x2, y2, z2);
            }
            float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 < 0)
                n3 = 0.0f;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * dot(GRAD3[gi3], x3, y3, z3);
            }
            // Add contributions from each corner to get the final Noise value.
            // The result is scaled to stay just inside [-1,1]
            return 32.0f * (n0 + n1 + n2 + n3);
        }

        /// <summary>
        /// 4D Simplex Noise
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public float Noise(float x, float y, float z, float w)
        {
            // The skewing and unskewing factors are hairy again for the 4D case
            float F4 = (MathF.Sqrt(5.0f) - 1.0f) / 4.0f;
            float G4 = (5.0f - MathF.Sqrt(5.0f)) / 20.0f;
            float n0, n1, n2, n3, n4; // Noise contributions from the five corners
            // Skew the (x,y,z,w) space to determine which cell of 24 simplices we're in
            float s = (x + y + z + w) * F4; // Factor for 4D skewing
            int i = FastMath.FastFloor(x + s);
            int j = FastMath.FastFloor(y + s);
            int k = FastMath.FastFloor(z + s);
            int l = FastMath.FastFloor(w + s);
            float t = (i + j + k + l) * G4; // Factor for 4D unskewing
            float X0 = i - t; // Unskew the cell origin back to (x,y,z,w) space
            float Y0 = j - t;
            float Z0 = k - t;
            float W0 = l - t;
            float x0 = x - X0; // The x,y,z,w distances from the cell origin
            float y0 = y - Y0;
            float z0 = z - Z0;
            float w0 = w - W0;

            // For the 4D case, the simplex is a 4D shape I won't even try to describe.
            // To find out which of the 24 possible simplices we're in, we need to
            // determine the magnitude ordering of x0, y0, z0 and w0.
            // The method below is a good way of finding the ordering of x,y,z,w and
            // then find the correct traversal order for the simplex we’re in.
            // First, six pair-wise comparisons are performed between each possible pair
            // of the four coordinates, and the results are used to add up binary bits
            // for an integer index.
            int c1 = (x0 > y0) ? 32 : 0;
            int c2 = (x0 > z0) ? 16 : 0;
            int c3 = (y0 > z0) ? 8 : 0;
            int c4 = (x0 > w0) ? 4 : 0;
            int c5 = (y0 > w0) ? 2 : 0;
            int c6 = (z0 > w0) ? 1 : 0;
            int c = c1 + c2 + c3 + c4 + c5 + c6;
            int i1, j1, k1, l1; // The integer offsets for the second simplex corner
            int i2, j2, k2, l2; // The integer offsets for the third simplex corner
            int i3, j3, k3, l3; // The integer offsets for the fourth simplex corner

            // simplex[c] is a 4-vector with the numbers 0, 1, 2 and 3 in some order.
            // Many values of c will never occur, since e.g. x>y>z>w makes x<z, y<w and x<w
            // impossible. Only the 24 indices which have non-zero entries make any sense.
            // We use a thresholding to set the coordinates in turn from the largest magnitude.
            // The number 3 in the "simplex" array is at the position of the largest coordinate.
            i1 = simplex[c][0] >= 3 ? 1 : 0;
            j1 = simplex[c][1] >= 3 ? 1 : 0;
            k1 = simplex[c][2] >= 3 ? 1 : 0;
            l1 = simplex[c][3] >= 3 ? 1 : 0;
            // The number 2 in the "simplex" array is at the second largest coordinate.
            i2 = simplex[c][0] >= 2 ? 1 : 0;
            j2 = simplex[c][1] >= 2 ? 1 : 0;
            k2 = simplex[c][2] >= 2 ? 1 : 0;
            l2 = simplex[c][3] >= 2 ? 1 : 0;
            // The number 1 in the "simplex" array is at the second smallest coordinate.
            i3 = simplex[c][0] >= 1 ? 1 : 0;
            j3 = simplex[c][1] >= 1 ? 1 : 0;
            k3 = simplex[c][2] >= 1 ? 1 : 0;
            l3 = simplex[c][3] >= 1 ? 1 : 0;
            // The fifth corner has all coordinate offsets = 1, so no need to look that up.
            float x1 = x0 - i1 + G4; // Offsets for second corner in (x,y,z,w) coords
            float y1 = y0 - j1 + G4;
            float z1 = z0 - k1 + G4;
            float w1 = w0 - l1 + G4;
            float x2 = x0 - i2 + 2.0f * G4; // Offsets for third corner in (x,y,z,w) coords
            float y2 = y0 - j2 + 2.0f * G4;
            float z2 = z0 - k2 + 2.0f * G4;
            float w2 = w0 - l2 + 2.0f * G4;
            float x3 = x0 - i3 + 3.0f * G4; // Offsets for fourth corner in (x,y,z,w) coords
            float y3 = y0 - j3 + 3.0f * G4;
            float z3 = z0 - k3 + 3.0f * G4;
            float w3 = w0 - l3 + 3.0f * G4;
            float x4 = x0 - 1.0f + 4.0f * G4; // Offsets for last corner in (x,y,z,w) coords
            float y4 = y0 - 1.0f + 4.0f * G4;
            float z4 = z0 - 1.0f + 4.0f * G4;
            float w4 = w0 - 1.0f + 4.0f * G4;
            // Work out the hashed gradient indices of the five simplex corners
            int ii = i & 255;
            int jj = j & 255;
            int kk = k & 255;
            int ll = l & 255;
            int gi0 = _permutationTable[ii + _permutationTable[jj + _permutationTable[kk + _permutationTable[ll]]]] % 32;
            int gi1 = _permutationTable[ii + i1 + _permutationTable[jj + j1 + _permutationTable[kk + k1 + _permutationTable[ll + l1]]]] % 32;
            int gi2 = _permutationTable[ii + i2 + _permutationTable[jj + j2 + _permutationTable[kk + k2 + _permutationTable[ll + l2]]]] % 32;
            int gi3 = _permutationTable[ii + i3 + _permutationTable[jj + j3 + _permutationTable[kk + k3 + _permutationTable[ll + l3]]]] % 32;
            int gi4 = _permutationTable[ii + 1 + _permutationTable[jj + 1 + _permutationTable[kk + 1 + _permutationTable[ll + 1]]]] % 32;
            // Calculate the contribution from the five corners
            float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0 - w0 * w0;
            if (t0 < 0)
                n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * dot(GRAD4[gi0], x0, y0, z0, w0);
            }
            float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1 - w1 * w1;
            if (t1 < 0)
                n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * dot(GRAD4[gi1], x1, y1, z1, w1);
            }
            float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2 - w2 * w2;
            if (t2 < 0)
                n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * dot(GRAD4[gi2], x2, y2, z2, w2);
            }
            float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3 - w3 * w3;
            if (t3 < 0)
                n3 = 0.0f;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * dot(GRAD4[gi3], x3, y3, z3, w3);
            }
            float t4 = 0.6f - x4 * x4 - y4 * y4 - z4 * z4 - w4 * w4;
            if (t4 < 0)
                n4 = 0.0f;
            else
            {
                t4 *= t4;
                n4 = t4 * t4 * dot(GRAD4[gi4], x4, y4, z4, w4);
            }
            // Sum up and scale the result to cover the range [-1,1]
            return 27.0f * (n0 + n1 + n2 + n3 + n4);
        }

        /// <summary>
        /// Tileable Noise
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="xyOffset"></param>
        /// <returns></returns>
        public float SeamlessNoise(float x, float y, float dx, float dy, float xyOffset)
        {
            float nx = xyOffset + MathF.Cos(x * 2.0f * MathF.Pi) * dx / (2.0f * MathF.Pi);
            float ny = xyOffset + MathF.Cos(y * 2.0f * MathF.Pi) * dy / (2.0f * MathF.Pi);
            float nz = xyOffset + MathF.Sin(x * 2.0f * MathF.Pi) * dx / (2.0f * MathF.Pi);
            float nw = xyOffset + MathF.Sin(y * 2.0f * MathF.Pi) * dy / (2.0f * MathF.Pi);

            return Noise(nx, ny, nz, nw);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        protected override void _Generate(int inWidth, int inHeight)
        {
            int[] premutationTemp = new int[256];

            for (int i = 0; i < 256; i++)
            {
                premutationTemp[i] = i;
            }
            for (int i = 0; i < 256; i++)
            {
                int j = _rnd.Next(256);
                int k = premutationTemp[i];

                premutationTemp[i] = premutationTemp[j];
                premutationTemp[j] = k;
            }

            for (int i = 0; i < 512; i++)
                _permutationTable[i] = premutationTemp[i & 255];

            float frequency = _startFrequency;
            float amplitude = _startAmplitude;
            int seed = (byte)Seed;

            for (int octave = 0; octave < _octaves; octave++)
            {
                for (int x = 0; x < inWidth; x++)
                {
                    for (int y = 0; y < inHeight; y++)
                    {
                        NoiseMap[x][y] += (((SeamlessNoise((float)x / inWidth, (float)y / inHeight, frequency, frequency, seed) + 1) * .5f) * amplitude);
                    }
                }

                seed = (byte)(seed * 2);
                frequency *= _lacunarity;
                amplitude *= _gain;
            }
        }

        private static float dot(int[] g, float x, float y)
        {
            return g[0] * x + g[1] * y;
        }

        private static float dot(int[] g, float x, float y, float z)
        {
            return g[0] * x + g[1] * y + g[2] * z;
        }

        private static float dot(int[] g, float x, float y, float z, float w)
        {
            return g[0] * x + g[1] * y + g[2] * z + g[3] * w;
        }
    }
}