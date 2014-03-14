using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;

namespace FrozenCore.Components
{
    [RequiredComponent(typeof(Transform))]
    public class Mover : Component, ICmpUpdatable
    {
        private abstract class Operation
        {
            protected float _movingSpeed;
            protected float _rotationSpeed;

            internal Operation(float inMovingSpeed, float inRotationSpeed)
            {
                _movingSpeed = inMovingSpeed;
                _rotationSpeed = inRotationSpeed;
            }

            internal abstract bool DoTransform(float inSecondsPast, Transform inTransform);
        }

        private class Rotate : Operation
        {
            private float _target { get; set; }

            internal Rotate(float inRotationSpeed, float inTargetAngle)
                : base(0, inRotationSpeed)
            {
                _target = inTargetAngle;
            }

            internal override bool DoTransform(float inSecondsPast, Transform inTransform)
            {
                bool transformComplete = false;

                if (_rotationSpeed == 0)
                {
                    transformComplete = true;
                }
                else
                {
                    float delta = _target - inTransform.Angle;
                    float maxRotation = inSecondsPast * _rotationSpeed;

                    if (delta <= maxRotation)
                    {
                        inTransform.Angle = _target;
                        transformComplete = true;
                    }
                    else
                    {
                        inTransform.Angle = inTransform.Angle + (delta * maxRotation);
                    }
                }

                return transformComplete;
            }
        }

        private class Move : Operation
        {
            private Vector3 _target;

            internal Move(float inMovingSpeed, Vector3 inTargetPosition)
                : base(inMovingSpeed, 0)
            {
                _target = inTargetPosition;
            }

            internal override bool DoTransform(float inSecondsPast, Transform inTransform)
            {
                bool transformComplete = false;

                if (_movingSpeed == 0)
                {
                    transformComplete = true;
                }
                else
                {
                    Vector3 delta = _target - inTransform.Pos;
                    float maxDistanceTraveled = inSecondsPast * _movingSpeed;

                    if (delta.Length <= maxDistanceTraveled)
                    {
                        inTransform.Pos = _target;
                        transformComplete = true;
                    }
                    else
                    {
                        inTransform.Pos = inTransform.Pos + (delta.Normalized * maxDistanceTraveled);
                    }
                }

                return transformComplete;
            }
        }

        [NonSerialized]
        private List<Operation> _operations;

        [NonSerialized]
        private Operation _currentOperation;

        public Mover()
        {
            _operations = new List<Operation>();
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
                if (_currentOperation.DoTransform(secondsPast, this.GameObj.Transform))
                {
                    _currentOperation = null;
                }
            }
        }

        public void MoveTo(Vector2 inTarget, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, new Vector3(inTarget, 0)));
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
    }
}
