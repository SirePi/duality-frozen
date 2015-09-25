using Duality;
using System;
using System.Collections.Generic;


namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    internal sealed class HalfedgePriorityQueue : IDisposable // also known as heap
    {
        private int _count;
        private float _deltay;
        private Halfedge[] _hash;
        private int _hashsize;
        private int _minBucket;
        private float _ymin;

        public HalfedgePriorityQueue(float ymin, float deltay, int sqrt_nsites)
        {
            _ymin = ymin;
            _deltay = deltay;
            _hashsize = 4 * sqrt_nsites;

            Initialize();
        }

        public void Dispose()
        {
            // get rid of dummies
            for (int i = 0; i < _hashsize; ++i)
            {
                _hash[i].Dispose();
                _hash[i] = null;
            }
            _hash = null;
        }

        public bool Empty()
        {
            return _count == 0;
        }

        public Halfedge ExtractMin()
        {
            Halfedge answer;

            // get the first real Halfedge in _minBucket
            answer = _hash[_minBucket].NextInPriorityQueue;

            _hash[_minBucket].NextInPriorityQueue = answer.NextInPriorityQueue;
            _count--;
            answer.NextInPriorityQueue = null;

            return answer;
        }

        public void Insert(Halfedge halfEdge)
        {
            Halfedge previous, next;

            int insertionBucket = Bucket(halfEdge);
            if (insertionBucket < _minBucket)
            {
                _minBucket = insertionBucket;
            }

            previous = _hash[insertionBucket];

            while ((next = previous.NextInPriorityQueue) != null &&
                (halfEdge.YStar > next.YStar || (halfEdge.YStar == next.YStar && halfEdge.Vertex.Position.X > next.Vertex.Position.X)))
            {
                previous = next;
            }

            halfEdge.NextInPriorityQueue = previous.NextInPriorityQueue;
            previous.NextInPriorityQueue = halfEdge;

            ++_count;
        }

        public Vector2 Min()
        {
            AdjustMinBucket();
            Halfedge answer = _hash[_minBucket].NextInPriorityQueue;
            return new Vector2(answer.Vertex.Position.X, answer.YStar);
        }

        public void Remove(Halfedge halfEdge)
        {
            Halfedge previous;
            int removalBucket = Bucket(halfEdge);

            if (halfEdge.Vertex != null)
            {
                previous = _hash[removalBucket];
                while (previous.NextInPriorityQueue != halfEdge)
                {
                    previous = previous.NextInPriorityQueue;
                }

                previous.NextInPriorityQueue = halfEdge.NextInPriorityQueue;
                _count--;

                halfEdge.Vertex = null;
                halfEdge.NextInPriorityQueue = null;
                halfEdge.Dispose();
            }
        }

        private void AdjustMinBucket()
        {
            while (_minBucket < _hashsize - 1 && IsEmpty(_minBucket))
            {
                ++_minBucket;
            }
        }

        private int Bucket(Halfedge halfEdge)
        {
            int theBucket = (int)((halfEdge.YStar - _ymin) / _deltay * _hashsize);

            if (theBucket < 0)
                theBucket = 0;
            if (theBucket >= _hashsize)
                theBucket = _hashsize - 1;

            return theBucket;
        }

        private void Initialize()
        {
            int i;

            _count = 0;
            _minBucket = 0;
            _hash = new Halfedge[_hashsize];

            // dummy Halfedge at the top of each hash
            for (i = 0; i < _hashsize; ++i)
            {
                _hash[i] = Halfedge.CreateDummy();
                _hash[i].NextInPriorityQueue = null;
            }
        }

        private bool IsEmpty(int bucket)
        {
            return (_hash[bucket].NextInPriorityQueue == null);
        }
    }
}