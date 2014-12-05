// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Noise
{
    /// <summary>
    /// A Voronoi-type Noise generator
    /// </summary>
    public sealed class CellNoise : Noise
    {
        private static readonly int BUCKETS = 10;
        private static readonly Vector2[] EMPTY_BUCKET = new Vector2[0];

        private List<List<List<Vector2>>> _buckets;
        private Distance _distanceFunction;
        private int _numPoints;
        private List<Vector2> _sites;
        private bool _wrapping;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inNumPoints"></param>
        /// <param name="inWrapping"></param>
        public CellNoise(int inNumPoints, bool inWrapping)
        {
            _wrapping = inWrapping;
            _numPoints = inNumPoints;

            _buckets = new List<List<List<Vector2>>>();
            _sites = new List<Vector2>();

            for (int i = 0; i < BUCKETS; i++)
            {
                _buckets.Add(new List<List<Vector2>>());
                for (int j = 0; j < BUCKETS; j++)
                {
                    _buckets[i].Add(new List<Vector2>());
                }
            }
        }

        /// <summary>
        /// [GET / SET] the list of Vector2 acting as generation sites
        /// </summary>
        public List<Vector2> Sites
        {
            get
            {
                if (_sites.Count == 0)
                    GenerateSites();

                return _sites;
            }
            set
            {
                _sites = value;
                _numPoints = _sites.Count;

                FillBuckets();
            }
        }

        /// <summary>
        /// Randomly generates the sites
        /// </summary>
        public void GenerateSites()
        {
            _sites.Clear();

            for (int i = 0; i < _numPoints; i++)
            {
                _sites.Add(_rnd.NextVector2(0, 0, 1, 1));
            }

            FillBuckets();
        }

        /// <summary>
        /// Sets the Distance function to be used for calculation
        /// </summary>
        /// <param name="inDistance"></param>
        public void SetDistanceFunction(Distance inDistance)
        {
            _distanceFunction = inDistance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        protected override void _Generate(int inWidth, int inHeight)
        {
            if (_distanceFunction == null)
            {
                _distanceFunction = Euclidean.Instance;
            }

            SortedList<float, Vector2> list = new SortedList<float, Vector2>();

            float bucketWidth = (float)inWidth / BUCKETS;
            float bucketHeight = (float)inHeight / BUCKETS;
            float bucketWidthNorm = 1f / BUCKETS;
            float bucketHeightNorm = 1f / BUCKETS;

            //get started
            for (int x = 0; x < inWidth; x++)
            {
                for (int y = 0; y < inHeight; y++)
                {
                    //Console.WriteLine("{0}, {1}", x, y);
                    Vector2 pt = new Vector2((float)x / inWidth, (float)y / inHeight);
                    //clear out the old distance
                    list.Clear();

                    //get the immediate bucket
                    int bx = FastMath.FastFloor(x / bucketWidth);
                    int by = FastMath.FastFloor(y / bucketHeight);

                    int surrounding = 0;
                    bool couldFindBetter = false;
                    float lastBestDistance = float.PositiveInfinity;

                    do
                    {
                        List<KeyValuePair<float, Vector2>> nearestSources = FindNearestInSurrounding(pt, bx, by, surrounding);
                        foreach (KeyValuePair<float, Vector2> kvp in nearestSources)
                        {
                            if (!list.Keys.Contains(kvp.Key))
                            {
                                list.Add(kvp.Key, kvp.Value);
                            }
                        }

                        if (list.Count > 0)
                        {
                            lastBestDistance = list.ElementAt(0).Key;

                            float dx = bx * bucketWidthNorm;
                            float dy = by * bucketHeightNorm;
                            float radiusx = bucketWidthNorm * surrounding;
                            float radiusy = bucketHeightNorm * surrounding;

                            couldFindBetter = false;
                            couldFindBetter |= GetDistance(pt, new Vector2(dx - radiusx, pt.Y)) < lastBestDistance;
                            couldFindBetter |= GetDistance(pt, new Vector2(dx + bucketWidthNorm + radiusx, pt.Y)) < lastBestDistance;
                            couldFindBetter |= GetDistance(pt, new Vector2(pt.X, dy - radiusy)) < lastBestDistance;
                            couldFindBetter |= GetDistance(pt, new Vector2(pt.X, dy + bucketHeightNorm + radiusy)) < lastBestDistance;
                        }
                        else
                            couldFindBetter = true;

                        //Console.WriteLine("surrounding: {0}, bestDistance: {1}, list.Count: {2}", surrounding, lastBestDistance, list.Count);
                        surrounding++;
                    } while ((couldFindBetter || list.Count == 0) && surrounding < BUCKETS);

                    float value = list.ElementAt(0).Key;

                    //now that we have the value, put it in
                    NoiseMap[x][y] = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnSeedChange()
        {
            base.OnSeedChange();
            GenerateSites();
        }

        private void FillBuckets()
        {
            //fill up the buckets
            for (int i = 0; i < BUCKETS; i++)
            {
                for (int j = 0; j < BUCKETS; j++)
                {
                    _buckets[i][j].Clear();
                }
            }

            for (int i = 0; i < _sites.Count; i++)
            {
                Vector2 pt = _sites[i];
                int bx = FastMath.FastFloor(pt.X * BUCKETS);
                int by = FastMath.FastFloor(pt.Y * BUCKETS);
                _buckets[bx][by].Add(pt);
            }
        }

        private List<KeyValuePair<float, Vector2>> FindNearestInSurrounding(Vector2 inPoint, int inBucketX, int inBucketY, int inSurrounding)
        {
            List<KeyValuePair<float, Vector2>> result = new List<KeyValuePair<float, Vector2>>();

            for (int x = -inSurrounding; x < inSurrounding + 1; x++)
            {
                foreach (Vector2 src in GetBucket(inBucketX + x, inBucketY - inSurrounding))
                {
                    result.Add(new KeyValuePair<float, Vector2>(GetDistance(inPoint, src), src));
                }

                if (inSurrounding != 0)
                {
                    foreach (Vector2 src in GetBucket(inBucketX + x, inBucketY + inSurrounding))
                    {
                        result.Add(new KeyValuePair<float, Vector2>(GetDistance(inPoint, src), src));
                    }
                }
            }

            if (inSurrounding != 0)
            {
                for (int y = -(inSurrounding - 1); y < inSurrounding; y++)
                {
                    foreach (Vector2 src in GetBucket(inBucketX - inSurrounding, inBucketY + y))
                    {
                        result.Add(new KeyValuePair<float, Vector2>(GetDistance(inPoint, src), src));
                    }

                    foreach (Vector2 src in GetBucket(inBucketX + inSurrounding, inBucketY + y))
                    {
                        result.Add(new KeyValuePair<float, Vector2>(GetDistance(inPoint, src), src));
                    }
                }
            }

            return result;
        }

        private Vector2[] GetBucket(int x, int y)
        {
            Wrap wrap = Wrap.None;

            if (_wrapping)
            {
                wrap |= x < 0 ? Wrap.Left : Wrap.None;
                wrap |= x >= BUCKETS ? Wrap.Right : Wrap.None;
                wrap |= y < 0 ? Wrap.Top : Wrap.None;
                wrap |= y >= BUCKETS ? Wrap.Bottom : Wrap.None;

                x = (x + BUCKETS) % BUCKETS;
                y = (y + BUCKETS) % BUCKETS;
            }

            Vector2[] result = (x < 0 || x >= BUCKETS || y < 0 || y >= BUCKETS) ? EMPTY_BUCKET : _buckets[x][y].ToArray();

            if (wrap != Wrap.None)
            {
                Vector2 pt;

                for (int i = 0; i < result.Length; i++)
                {
                    pt = result[i];

                    if ((wrap & Wrap.Left) != Wrap.None)
                        pt.X -= 1;
                    if ((wrap & Wrap.Right) != Wrap.None)
                        pt.X += 1;
                    if ((wrap & Wrap.Top) != Wrap.None)
                        pt.Y -= 1;
                    if ((wrap & Wrap.Bottom) != Wrap.None)
                        pt.Y += 1;

                    result[i] = pt;
                }
            }

            return result;
        }

        private float GetDistance(Vector2 a, Vector2 b)
        {
            return _distanceFunction.GetDistance(a, b);
        }
    }
}