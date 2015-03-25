// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// Abstract class acting as the base for all Commands
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Indicates if the command completed its execution
        /// </summary>
        public bool IsComplete { get; protected set; }

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public abstract void Execute(float inSecondsPast, GameObject inGameObject);

        /// <summary>
        /// Initializes the command
        /// </summary>
        public abstract void Initialize(GameObject inGameObject);
    }

    /// <summary>
    /// Abstract class used in order to allow the Command to target a specific Component.
    /// An Exception is thrown if the required Component is not present inside the Commanded GameObject.
    /// </summary>
    /// <typeparam name="T">The Type of the required Component</typeparam>
    public abstract class Command<T> : Command where T : Component
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inGameObject"></param>
        /// <returns></returns>
        protected T GetComponent(GameObject inGameObject)
        {
            T component = inGameObject.GetComponent<T>();

            if (component == null)
            {
                throw new ArgumentException(String.Format("{0} requires a {1} in {2}", this.GetType().Name, typeof(T).Name, inGameObject.FullName));
            }

            return component;
        }
    }

    /// <summary>
    /// Abstract class used in order to allow the Command to target a specific Component.
    /// An Exception is thrown if the required Component is not present inside the Commanded GameObject.
    /// Provides utility methods to define the duration of the Command based on a time, or a speed.
    /// </summary>
    /// <typeparam name="T">The Type of the required Component</typeparam>
    public abstract class TimedCommand<T> : Command<T> where T : Component
    {
        /// <summary>
        ///
        /// </summary>
        protected float _timePast;

        /// <summary>
        ///
        /// </summary>
        protected float _timeToComplete;

        /// <summary>
        /// Calculates the time to completion depending on the desired speed and command length
        /// </summary>
        /// <param name="inSpeed"></param>
        /// <returns></returns>
        public Command<T> FixedSpeed(float inSpeed)
        {
            _timeToComplete = MathF.Abs(GetCommandLength() / inSpeed);
            return this;
        }

        /// <summary>
        /// Sets the completion time
        /// </summary>
        /// <param name="inTime"></param>
        /// <returns></returns>
        public Command<T> Timed(float inTime)
        {
            _timeToComplete = inTime;
            return this;
        }

        /// <summary>
        /// Returns the length, in abstract, of the command before it reaches its completion. Used for FixedSpeed commands
        /// </summary>
        /// <returns></returns>
        protected abstract float GetCommandLength();
    }
}