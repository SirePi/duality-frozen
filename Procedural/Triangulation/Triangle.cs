using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Triangle : IDisposable
    {
        private List<Site> _sites;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Triangle(Site a, Site b, Site c)
        {
            _sites = new List<Site>() { a, b, c };
        }

        /// <summary>
        ///
        /// </summary>
        public List<Site> sites
        {
            get { return this._sites; }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            _sites.Clear();
            _sites = null;
        }
    }
}