using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;
using Duality.Drawing;
using Duality.Components.Renderers;
using FrozenCore.Commands;

namespace FrozenCore.Components
{
    [Serializable]
    public class Commander : Component, ICmpUpdatable
    {
        [NonSerialized]
        private List<Command> _operations;

        [NonSerialized]
        private Command _currentOperation;

        [NonSerialized]
        private string _currentSignal;

        public Commander()
        {
            _operations = new List<Command>();
        }

        public string CurrentSignal
        {
            get { return _currentSignal; }
        }

        public bool IsIdle
        {
            get { return (_currentOperation == null && _operations.Count == 0); }
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

        public Move MoveTo(Vector2 inTarget)
        {
            return Add(new Move(this.GameObj, new Vector3(inTarget, this.GameObj.Transform.Pos.Z), false));
        }

        public Move MoveTo(Vector3 inTarget)
        {
            return Add(new Move(this.GameObj, inTarget, false));
        }

        public Move MoveToRelative(Vector2 inTarget)
        {
            return Add(new Move(this.GameObj, new Vector3(inTarget, 0), true));
        }

        public Move MoveToRelative(Vector3 inTarget)
        {
            return Add(new Move(this.GameObj, inTarget, true));
        }

        public Rotate RotateTo(float inTarget)
        {
            return Add(new Rotate(this.GameObj, inTarget, false));
        }

        public Rotate RotateToRelative(float inTarget)
        {
            return Add(new Rotate(this.GameObj, inTarget, true));
        }

        public Move MoveBy(Vector2 inAmount)
        {
            return Add(new Move(this.GameObj, this.GameObj.Transform.Pos + new Vector3(inAmount, 0), false));
        }

        public Move MoveBy(Vector3 inAmount)
        {
            return Add(new Move(this.GameObj, this.GameObj.Transform.Pos + inAmount, false));
        }

        public Rotate RotateBy(float inAmount)
        {
            return Add(new Rotate(this.GameObj, this.GameObj.Transform.Angle + inAmount, false));
        }

        public ColorizeSprite ColorizeSprite(ColorRgba inColor)
        {
            return Add(new ColorizeSprite(this.GameObj, inColor));
        }

        public Signal Signal(string inSignal)
        {
            return Add(new Signal(inSignal));
        }

        public Wait Wait(float inTimeToWait)
        {
            return Add(new Wait(inTimeToWait));
        }

        public WaitFor WaitFor(GameObject inGameObject)
        {
            return Add(new WaitFor(inGameObject, null));
        }

        public WaitFor WaitFor(GameObject inGameObject, string inSignal)
        {
            return Add(new WaitFor(inGameObject, inSignal));
        }

        public ChangeWidgetStatus ChangeWidgetStatus(Widgets.Widget.WidgetStatus inStatus)
        {
            return Add(new ChangeWidgetStatus(inStatus));
        }

        public T Add<T>(T inCommand) where T : Command
        {
            _operations.Add(inCommand);

            return inCommand;
        }
    }
}
