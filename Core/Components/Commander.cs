using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core.Commands;
using SnowyPeak.Duality.Plugin.Frozen.Core.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Components
{
    /// <summary>
    /// The Commander is a Component in charge of managing the Commands for a single GameObject.
    /// It provides a series of predefined methods to generate and manage SnowyPeak.Duality.Plugin.Frozen.Core's included Commands,
    /// but also allows the user to register its own custom Commands.
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageCommander)]
    [EditorHintCategory(typeof(Res), ResNames.Category)]
    public class Commander : Component, ICmpUpdatable
    {
        [NonSerialized]
        private Command _currentOperation;

        [NonSerialized]
        private ParallelCommand _currentParallel;

        [NonSerialized]
        private string _currentSignal;

        [NonSerialized]
        private List<Command> _operations;

        /// <summary>
        /// Constructor
        /// </summary>
        public Commander()
        {
            _operations = new List<Command>();
        }

        /// <summary>
        /// The Last Signal emitted by a Signal Command
        /// </summary>
        public string CurrentSignal
        {
            get { return _currentSignal; }
        }

        /// <summary>
        /// Indicates if there are no current or pending Commands to execute
        /// </summary>
        public bool IsIdle
        {
            get { return (_currentOperation == null && _operations.Count == 0); }
        }

        /// <summary>
        /// Adds a custom Command to the execution list or to the currently open parallel Command.
        /// </summary>
        /// <typeparam name="T">The Type of the Command that will be added</typeparam>
        /// <param name="inCommand">The Command</param>
        /// <returns>The Command added</returns>
        public T Add<T>(T inCommand) where T : Command
        {
            if (_currentParallel != null)
            {
                _currentParallel.Add(inCommand);
            }
            else
            {
                _operations.Add(inCommand);
            }

            return inCommand;
        }

        /// <summary>
        /// Tells the Commander that all the subsequent Commands will have to be carried out in parallel, until the
        /// EndParallel method is called
        /// </summary>
        public void BeginParallel()
        {
            if (_currentParallel == null)
            {
                _currentParallel = new ParallelCommand();
            }
        }

        /// <summary>
        /// Adds a ColorizeSprite Command, from the current to the desired Color
        /// </summary>
        /// <param name="inColor">Target color</param>
        /// <returns>The ColorizeSprite Command</returns>
        public ColorizeSprite ColorizeSprite(ColorRgba inColor)
        {
            return Add(new ColorizeSprite(this.GameObj, inColor));
        }

        /// <summary>
        /// Adds a ColorizeText Command, from the current to the desired Color
        /// </summary>
        /// <param name="inColor">Target color</param>
        /// <returns>The ColorizeText Command</returns>
        public ColorizeText ColorizeText(ColorRgba inColor)
        {
            return Add(new ColorizeText(this.GameObj, inColor));
        }

        /// <summary>
        /// Adds a Destroy Command
        /// </summary>
        /// <returns>The Destroy Command</returns>
        public Destroy Destroy()
        {
            return Add(new Destroy());
        }

        /// <summary>
        /// Stops the addition of parallel Commands and marks them as ready for execution
        /// </summary>
        public void EndParallel()
        {
            if (_currentParallel != null)
            {
                _operations.Add(_currentParallel);
                _currentParallel = null;
            }
        }

        void ICmpUpdatable.OnUpdate()
        {
            float secondsPast = Time.LastDelta / 1000;

            if (_currentOperation == null && _operations.Count > 0)
            {
                _currentOperation = _operations[0];
                _operations.RemoveAt(0);
            }

            if (_currentOperation != null)
            {
                _currentOperation.Execute(secondsPast, this.GameObj);

                if (_currentOperation.IsComplete)
                {
                    _currentSignal = null;

                    if (_currentOperation is Signal)
                    {
                        _currentSignal = (_currentOperation as Signal).Value;
                    }

                    _currentOperation = null;
                }
            }
        }

        /// <summary>
        /// Adds a XY Move Command to the desired position, relative to the current one
        /// </summary>
        /// <param name="inAmount">XY position</param>
        /// <returns>The Move Command</returns>
        public Move MoveBy(Vector2 inAmount)
        {
            return Add(new Move(this.GameObj, this.GameObj.Transform.Pos + new Vector3(inAmount, 0), false));
        }

        /// <summary>
        /// Adds a XYZ Move Command to the desired position, relative to the current one
        /// </summary>
        /// <param name="inAmount">XYZ position</param>
        /// <returns>The Move Command</returns>
        public Move MoveBy(Vector3 inAmount)
        {
            return Add(new Move(this.GameObj, this.GameObj.Transform.Pos + inAmount, false));
        }

        /// <summary>
        /// Adds a XY Move Command to the desired absolute position
        /// </summary>
        /// <param name="inTarget">Target XY position</param>
        /// <returns>The Move Command</returns>
        public Move MoveTo(Vector2 inTarget)
        {
            return Add(new Move(this.GameObj, new Vector3(inTarget, this.GameObj.Transform.Pos.Z), false));
        }

        /// <summary>
        /// Adds a XYZ Move Command to the desired absolute position
        /// </summary>
        /// <param name="inTarget">Target XYZ position</param>
        /// <returns>The Move Command</returns>
        public Move MoveTo(Vector3 inTarget)
        {
            return Add(new Move(this.GameObj, inTarget, false));
        }

        /// <summary>
        /// Adds a XY Move Command to the desired position, relative to the parent GameObject
        /// </summary>
        /// <param name="inTarget">Target XY position</param>
        /// <returns>The Move Command</returns>
        public Move MoveToRelative(Vector2 inTarget)
        {
            return Add(new Move(this.GameObj, new Vector3(inTarget, 0), true));
        }

        /// <summary>
        /// Adds a XYZ Move Command to the desired position, relative to the parent GameObject
        /// </summary>
        /// <param name="inTarget">Target XYZ position</param>
        /// <returns>The Move Command</returns>
        public Move MoveToRelative(Vector3 inTarget)
        {
            return Add(new Move(this.GameObj, inTarget, true));
        }

        /// <summary>
        /// Adds a Rotate Command to the desired angle, relative to the current one
        /// </summary>
        /// <param name="inAmount">angle</param>
        /// <returns>The Rotate Command</returns>
        public Rotate RotateBy(float inAmount)
        {
            return Add(new Rotate(this.GameObj, this.GameObj.Transform.Angle + inAmount, false));
        }

        /// <summary>
        /// Adds a Rotate Command to the desired absolute angle
        /// </summary>
        /// <param name="inTarget">Target angle</param>
        /// <returns>The Rotate Command</returns>
        public Rotate RotateTo(float inTarget)
        {
            return Add(new Rotate(this.GameObj, inTarget, false));
        }

        /// <summary>
        /// Adds a Rotate Command to the desired angle, relative to the parent GameObject
        /// </summary>
        /// <param name="inTarget">Target angle</param>
        /// <returns>The Rotate Command</returns>
        public Rotate RotateToRelative(float inTarget)
        {
            return Add(new Rotate(this.GameObj, inTarget, true));
        }

        /// <summary>
        /// Adds a Scale Command to the desired absolute scale
        /// </summary>
        /// <param name="inTarget">Target scale</param>
        /// <returns>The Scale Command</returns>
        public Scale Scale(float inTarget)
        {
            return Add(new Scale(this.GameObj, inTarget, false));
        }

        /// <summary>
        /// Adds a Scale Command to the desired scale, relative to the current one
        /// </summary>
        /// <param name="inTarget">Target relative scale</param>
        /// <returns>The Scale Command</returns>
        public Scale ScaleRelative(float inTarget)
        {
            return Add(new Scale(this.GameObj, inTarget, true));
        }

        /// <summary>
        /// Adds a Signal Command
        /// </summary>
        /// <param name="inSignal">The Signal to emit</param>
        /// <returns>The Signal Command</returns>
        public Signal Signal(string inSignal)
        {
            return Add(new Signal(inSignal));
        }

        /// <summary>
        /// Adds a Wait Command
        /// </summary>
        /// <param name="inTimeToWait">The duration, in seconds, of the wait</param>
        /// <returns>The Wait Command</returns>
        public Wait Wait(float inTimeToWait)
        {
            return Add(new Wait(inTimeToWait));
        }

        /// <summary>
        /// Adds a WaitFor Command, waiting until the other object has completed all its Commands
        /// </summary>
        /// <param name="inGameObject">The GameObject to wait for</param>
        /// <returns>The WaitFor Command</returns>
        public WaitFor WaitFor(GameObject inGameObject)
        {
            return Add(new WaitFor(inGameObject, null));
        }

        /// <summary>
        /// Adds a WaitFor Command, waiting until the other object issues a determined Signal
        /// </summary>
        /// <param name="inGameObject">The GameObject to wait for</param>
        /// <param name="inSignal">The Signal to wait for</param>
        /// <returns>The WaitFor Command</returns>
        public WaitFor WaitFor(GameObject inGameObject, string inSignal)
        {
            return Add(new WaitFor(inGameObject, inSignal));
        }
    }
}