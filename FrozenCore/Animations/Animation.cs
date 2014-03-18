using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore.Animations
{
    public abstract class Animation
    {
        public bool IsComplete { get; protected set; }
        public abstract void Animate(float inSecondsPast, GameObject inGameObject);
    }

    public abstract class Animation<T> : Animation where T : Component
    {
        protected T GetComponent(GameObject inGameObject)
        {
            T component = inGameObject.GetComponent<T>();

            if (component == null)
            {
                throw new InvalidOperationException(String.Format("{0} requires a {1} in {2}", this.GetType().Name, typeof(T).Name, inGameObject.FullName));
            }

            return component;
        }
    }

    public abstract class ActiveAnimation<T> : Animation<T> where T : Component
    {
        protected float _timePast;
        protected float _timeToComplete;

        public Animation<T> FixedSpeed(float inSpeed)
        {
            _timeToComplete = GetAnimationLength() / inSpeed;
            return this;
        }

        public Animation<T> Timed(float inTime)
        {
            _timeToComplete = inTime;
            return this;
        }

        protected abstract float GetAnimationLength();
    }
}
