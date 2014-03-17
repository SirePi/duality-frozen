using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;
using Duality.Drawing;
using Duality.Components.Renderers;
using FrozenCore.Animations;

namespace FrozenCore.Components
{
    public class Animator : Component, ICmpUpdatable
    {
        [NonSerialized]
        private List<Animation> _operations;

        [NonSerialized]
        private Animation _currentOperation;

        [NonSerialized]
        private string _currentSignal;

        public Animator()
        {
            _operations = new List<Animation>();
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
                _currentOperation.Animate(secondsPast, this.GameObj);

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
            return Add(new Move(this.GameObj, new Vector3(inTarget, this.GameObj.Transform.Pos.Z)));
        }

        public Move MoveTo(Vector3 inTarget)
        {
            return Add(new Move(this.GameObj, inTarget));
        }

        public Move MoveToRelative(Vector2 inTarget)
        {
            Vector3 target = new Vector3(inTarget, this.GameObj.Transform.Pos.Z);
            Move movement = null;

            if (this.GameObj.Parent == null || this.GameObj.Parent.Transform == null)
            {
                movement = new Move(this.GameObj, target);
            }
            else
            {
                movement = new Move(this.GameObj, target + this.GameObj.Parent.Transform.Pos);
            }

            return Add(movement);
        }

        public Move MoveToRelative(Vector3 inTarget)
        {
            Move movement = null;

            if (this.GameObj.Parent == null || this.GameObj.Parent.Transform == null)
            {
                movement = new Move(this.GameObj, inTarget);
            }
            else
            {
                movement = new Move(this.GameObj, inTarget + this.GameObj.Parent.Transform.Pos);
            }

            return Add(movement);
        }

        public Rotate RotateTo(float inTarget)
        {
            return Add(new Rotate(this.GameObj, inTarget));
        }

        public Rotate RotateToRelative(float inTarget)
        {
            Rotate rotation = null;

            if (this.GameObj.Parent == null || this.GameObj.Parent.Transform == null)
            {
                rotation = new Rotate(this.GameObj, inTarget);
            }
            else
            {
                rotation = new Rotate(this.GameObj, inTarget + this.GameObj.Parent.Transform.Angle);
            }

            return Add(rotation);
        }

        public Move MoveBy(Vector2 inAmount)
        {
            return Add(new Move(this.GameObj, this.GameObj.Transform.Pos + new Vector3(inAmount, 0)));
        }

        public Move MoveBy(Vector3 inAmount)
        {
            return Add(new Move(this.GameObj, this.GameObj.Transform.Pos + inAmount));
        }

        public Rotate RotateBy(float inAmount)
        {
            return Add(new Rotate(this.GameObj, this.GameObj.Transform.Angle + inAmount));
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
            return Add(new ChangeWidgetStatus(this.GameObj, inStatus));
        }

        public T Add<T>(T inAnimation) where T : Animation
        {
            _operations.Add(inAnimation);

            return inAnimation;
        }
    }
}
