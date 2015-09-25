using System;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    internal sealed class EdgeList : IDisposable
    {
        private float _deltax;
        private Halfedge[] _hash;
        private int _hashsize;
        private Halfedge _leftEnd;
        private Halfedge _rightEnd;
        private float _xmin;

        public EdgeList(float xmin, float deltax, int sqrt_nsites)
        {
            _xmin = xmin;
            _deltax = deltax;
            _hashsize = 2 * sqrt_nsites;

            _hash = new Halfedge[_hashsize];

            // two dummy Halfedges:
            _leftEnd = Halfedge.CreateDummy();
            _rightEnd = Halfedge.CreateDummy();
            _leftEnd.EdgeListLeftNeighbor = null;
            _leftEnd.EdgeListRightNeighbor = _rightEnd;
            _rightEnd.EdgeListLeftNeighbor = _leftEnd;
            _rightEnd.EdgeListRightNeighbor = null;
            _hash[0] = _leftEnd;
            _hash[_hashsize - 1] = _rightEnd;
        }

        public Halfedge leftEnd
        {
            get { return _leftEnd; }
        }
        public Halfedge rightEnd
        {
            get { return _rightEnd; }
        }

        public void Dispose()
        {
            Halfedge halfEdge = _leftEnd;
            Halfedge prevHe;
            while (halfEdge != _rightEnd)
            {
                prevHe = halfEdge;
                halfEdge = halfEdge.EdgeListRightNeighbor;
                prevHe.Dispose();
            }
            _leftEnd = null;
            _rightEnd.Dispose();
            _rightEnd = null;

            int i;
            for (i = 0; i < _hashsize; ++i)
            {
                _hash[i] = null;
            }
            _hash = null;
        }

        /**
         * Insert newHalfedge to the right of lb
         * @param lb
         * @param newHalfedge
         *
         */

        public Halfedge EdgeListLeftNeighbor(Vector2 p)
        {
            int i, bucket;
            Halfedge halfEdge;

            /* Use hash table to get close to desired halfedge */
            bucket = (int)((p.X - _xmin) / _deltax * _hashsize);
            if (bucket < 0)
            {
                bucket = 0;
            }
            if (bucket >= _hashsize)
            {
                bucket = _hashsize - 1;
            }
            halfEdge = GetHash(bucket);
            if (halfEdge == null)
            {
                for (i = 1; true; ++i)
                {
                    if ((halfEdge = GetHash(bucket - i)) != null)
                        break;
                    if ((halfEdge = GetHash(bucket + i)) != null)
                        break;
                }
            }
            /* Now search linear list of halfedges for the correct one */
            if (halfEdge == leftEnd || (halfEdge != rightEnd && halfEdge.IsLeftOf(p)))
            {
                do
                {
                    halfEdge = halfEdge.EdgeListRightNeighbor;
                } while (halfEdge != rightEnd && halfEdge.IsLeftOf(p));
                halfEdge = halfEdge.EdgeListLeftNeighbor;
            }
            else
            {
                do
                {
                    halfEdge = halfEdge.EdgeListLeftNeighbor;
                } while (halfEdge != leftEnd && !halfEdge.IsLeftOf(p));
            }

            /* Update hash table and reference counts */
            if (bucket > 0 && bucket < _hashsize - 1)
            {
                _hash[bucket] = halfEdge;
            }
            return halfEdge;
        }

        public void Insert(Halfedge lb, Halfedge newHalfedge)
        {
            newHalfedge.EdgeListLeftNeighbor = lb;
            newHalfedge.EdgeListRightNeighbor = lb.EdgeListRightNeighbor;
            lb.EdgeListRightNeighbor.EdgeListLeftNeighbor = newHalfedge;
            lb.EdgeListRightNeighbor = newHalfedge;
        }

        /**
         * This function only removes the Halfedge from the left-right list.
         * We cannot dispose it yet because we are still using it.
         * @param halfEdge
         *
         */

        public void Remove(Halfedge halfEdge)
        {
            halfEdge.EdgeListLeftNeighbor.EdgeListRightNeighbor = halfEdge.EdgeListRightNeighbor;
            halfEdge.EdgeListRightNeighbor.EdgeListLeftNeighbor = halfEdge.EdgeListLeftNeighbor;
            halfEdge.Edge = Edge.DELETED;
            halfEdge.EdgeListLeftNeighbor = halfEdge.EdgeListRightNeighbor = null;
        }

        /**
         * Find the rightmost Halfedge that is still left of p
         * @param p
         * @return
         *
         */
        /* Get entry from hash table, pruning any deleted nodes */

        private Halfedge GetHash(int b)
        {
            Halfedge halfEdge;

            if (b < 0 || b >= _hashsize)
            {
                return null;
            }
            halfEdge = _hash[b];
            if (halfEdge != null && halfEdge.Edge == Edge.DELETED)
            {
                /* Hash table points to deleted halfedge.  Patch as necessary. */
                _hash[b] = null;
                // still can't dispose halfEdge yet!
                return null;
            }
            else
            {
                return halfEdge;
            }
        }
    }
}