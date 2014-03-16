using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;
using Duality.Drawing;
using Duality.Components.Renderers;

namespace FrozenCore.Components
{
    [RequiredComponent(typeof(Transform))]
    public class Animator : Component, ICmpUpdatable
    {
        public abstract class Animation
        {
            protected float _speed;
            public bool IsComplete { get; protected set; }

            internal Animation(float inSpeed)
            {
                _speed = inSpeed;
            }

            internal abstract void DoTransform(float inSecondsPast, GameObject inGameObject);
        }

        private class Rotate : Animation
        {
            private float _target { get; set; }

            internal Rotate(float inRotationSpeed, float inTargetAngle)
                : base(inRotationSpeed)
            {
                _target = inTargetAngle;
            }

            internal override void DoTransform(float inSecondsPast, GameObject inGameObject)
            {
                Transform t = inGameObject.Transform;

                if (_speed == 0)
                {
                    t.Angle = _target;
                    IsComplete = true;
                }
                else
                {
                    float delta = _target - t.Angle;
                    float maxRotation = inSecondsPast * _speed;

                    if (delta <= maxRotation)
                    {
                        t.Angle = _target;
                        IsComplete = true;
                    }
                    else
                    {
                        t.Angle = t.Angle + (delta * maxRotation);
                    }
                }
            }
        }

        private class Move : Animation
        {
            private Vector3 _target;

            internal Move(float inMovingSpeed, Vector3 inTargetPosition)
                : base(inMovingSpeed)
            {
                _target = inTargetPosition;
            }

            internal override void DoTransform(float inSecondsPast, GameObject inGameObject)
            {
                Transform t = inGameObject.Transform;

                if (_speed <= 0)
                {
                    t.Pos = _target;
                    IsComplete = true;
                }
                else
                {
                    Vector3 delta = _target - t.Pos;
                    float maxDistanceTraveled = inSecondsPast * _speed;

                    if (delta.Length <= maxDistanceTraveled)
                    {
                        t.Pos = _target;
                        IsComplete = true;
                    }
                    else
                    {
                        t.Pos = t.Pos + (delta.Normalized * maxDistanceTraveled);
                    }
                }
            }
        }

        private class ColorizeSprite : Animation
        {
            private ColorRgba _target;
            private ColorRgba? _origin;
            private float _timeToComplete;

            internal ColorizeSprite(float inColorizeSpeed, ColorRgba inTargetColor)
                : base(inColorizeSpeed)
            {
                _target = inTargetColor;
                _origin = null;
            }

            internal override bool DoTransform(float inSecondsPast, GameObject inGameObject)
            {
                SpriteRenderer sr = inGameObject.GetComponent<SpriteRenderer>();

                if (sr == null)
                {
                    throw new InvalidOperationException("ColorizeSprite requires a SpriteRenderer in " + inGameObject.FullName);
                }
                else
                {
                    if (!_origin.HasValue)
                    {
                        _origin = sr.ColorTint;
                        _timeToComplete
                    }

                    if (_speed <= 0)
                    {
                        sr.ColorTint = _target;
                        IsComplete = true;
                    }
                    else
                    {
                        float maxColorChange = inSecondsPast * _speed;

                        if (delta.Length <= maxDistanceTraveled)
                        {
                            t.Pos = _target;
                            IsComplete = true;
                        }
                        else
                        {
                            sr.ColorTint = sr.ColorTint;
                        }
                    }
                }
            }
        }

        [NonSerialized]
        private List<Animation> _operations;

        [NonSerialized]
        private Animation _currentOperation;

        public Animator()
        {
            _operations = new List<Animation>();
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
                _currentOperation.DoTransform(secondsPast, this.GameObj);

                if (_currentOperation.IsComplete)
                {
                    _currentOperation = null;
                }
            }
        }

        public void MoveTo(Vector2 inTarget, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, new Vector3(inTarget, this.GameObj.Transform.Pos.Z)));
        }

        public void MoveTo(Vector3 inTarget, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, inTarget));
        }

        public void RotateTo(float inTarget, float inSpeed)
        {
            _operations.Add(new Rotate(inSpeed, inTarget));
        }

        public void MoveBy(Vector2 inAmount, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, this.GameObj.Transform.Pos + new Vector3(inAmount, 0)));
        }

        public void MoveBy(Vector3 inAmount, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, this.GameObj.Transform.Pos + inAmount));
        }

        public void RotateBy(float inAmount, float inSpeed)
        {
            _operations.Add(new Rotate(inSpeed, this.GameObj.Transform.Angle + inAmount));
        }
        
        public void AddCustomAnimation(Animation inAnimation)
        {
            _operations.Add(inAnimation);
        }
    }
}
