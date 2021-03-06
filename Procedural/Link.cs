﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural
{
    /// <summary>
    /// Models a Link between two INodes of a Graph
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Link<T> where T : class, INode, new()
    {
        /// <summary>
        /// [GET / SET] The starting INode of the Link
        /// </summary>
        public T From { get; set; }
        /// <summary>
        /// [GET / SET] Indicates if the Link is Directed
        /// </summary>
        public bool IsDirected { get; set; }
        /// <summary>
        /// [GET / SET] The ending INode of the Link
        /// </summary>
        public T To { get; set; }
        /// <summary>
        /// [GET / SET] The Weight of the Link
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is Link<T>)
            {
                Link<T> other = obj as Link<T>;

                result = (this.IsDirected == other.IsDirected);
                result &= ((this.From == other.From && this.To == other.To) || (this.From == other.To && this.To == other.From && !this.IsDirected));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return From.GetHashCode() | To.GetHashCode();
        }
    }
}