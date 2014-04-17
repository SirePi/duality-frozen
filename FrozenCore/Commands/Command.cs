// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore.Commands
{
    public abstract class Command
    {
        public bool IsComplete { get; protected set; }
        public abstract void Execute(float inSecondsPast, GameObject inGameObject);
    }

    public abstract class Command<T> : Command where T : Component
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

    public abstract class TimedCommand<T> : Command<T> where T : Component
    {
        protected float _timePast;
        protected float _timeToComplete;

        public Command<T> FixedSpeed(float inSpeed)
        {
            _timeToComplete = MathF.Abs(GetCommandLength() / inSpeed);
            return this;
        }

        public Command<T> Timed(float inTime)
        {
            _timeToComplete = inTime;
            return this;
        }

        protected abstract float GetCommandLength();
    }
}
