using System;
using System.Collections.Generic;
using Duality;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public sealed class SiteList : IDisposable
    {
        private int _currentIndex;
        private List<Site> _sites;
        private bool _sorted;

        /// <summary>
        ///
        /// </summary>
        public SiteList()
        {
            _sites = new List<Site>();
            _sorted = false;
        }

        /// <summary>
        ///
        /// </summary>
        public int Count
        {
            get { return _sites.Count; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public int Add(Site site)
        {
            _sorted = false;
            _sites.Add(site);
            return _sites.Count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Circle> Circles()
        {
            List<Circle> circles = new List<Circle>();
            Site site;
            for (int i = 0; i < _sites.Count; i++)
            {
                site = _sites[i];
                float radius = 0f;
                Edge nearestEdge = site.NearestEdge();

                if (!nearestEdge.IsPartOfConvexHull())
                {
                    radius = nearestEdge.SitesDistance() * 0.5f;
                }
                circles.Add(new Circle(site.Position, radius));
            }
            return circles;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (_sites != null)
            {
                for (int i = 0; i < _sites.Count; i++)
                {
                    Site site = _sites[i];
                    site.Dispose();
                }
                _sites.Clear();
                _sites = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Nullable<Vector2> NearestSitePoint(/*proximityMap:BitmapData,*/float x, float y)
        {
            //			uint index = proximityMap.getPixel(x, y);
            //			if (index > _sites.length - 1)
            //			{
            return null;
            //			}
            //			return _sites[index].coord;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Site Next()
        {
            if (_sorted == false)
            {
                Log.Game.WriteError("SiteList::next():  sites have not been sorted");
            }
            if (_currentIndex < _sites.Count)
            {
                return _sites[_currentIndex++];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plotBounds"></param>
        /// <returns></returns>
        public List<List<Vector2>> Regions(Rect plotBounds)
        {
            List<List<Vector2>> regions = new List<List<Vector2>>();
            Site site;
            for (int i = 0; i < _sites.Count; i++)
            {
                site = _sites[i];
                regions.Add(site.Region(plotBounds));
            }
            return regions;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Vector2> SiteCoords()
        {
            List<Vector2> coords = new List<Vector2>();
            Site site;
            for (int i = 0; i < _sites.Count; i++)
            {
                site = _sites[i];
                coords.Add(site.Position);
            }
            return coords;
        }

        internal Rect GetSitesBounds()
        {
            if (_sorted == false)
            {
                Site.SortSites(_sites);
                _currentIndex = 0;
                _sorted = true;
            }

            float xmin, xmax, ymin, ymax;
            if (_sites.Count == 0)
            {
                return new Rect(0, 0, 0, 0);
            }

            xmin = float.MaxValue;
            xmax = float.MinValue;
            for (int i = 0; i < _sites.Count; i++)
            {
                Site site = _sites[i];
                if (site.Position.X < xmin)
                {
                    xmin = site.Position.X;
                }
                if (site.Position.X > xmax)
                {
                    xmax = site.Position.X;
                }
            }
            // here's where we assume that the sites have been sorted on y:
            ymin = _sites[0].Position.Y;
            ymax = _sites[_sites.Count - 1].Position.Y;

            return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
        }
    }
}