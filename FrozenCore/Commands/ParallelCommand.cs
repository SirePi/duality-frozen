using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrozenCore.Commands
{
    public class ParallelCommand : Command
    {
        private List<Command> _commands;

        internal ParallelCommand()
        {
            _commands = new List<Command>();
        }

        public ParallelCommand Add(Command inCommand)
        {
            if (inCommand is Wait || inCommand is WaitFor || inCommand is ParallelCommand)
            {
                throw new InvalidOperationException("A ParallelCommand cannot contain this kind of Command");
            }
            _commands.Add(inCommand);

            return this;
        }

        public override void Execute(float inSecondsPast, Duality.GameObject inGameObject)
        {
            IsComplete = false;
            foreach (Command c in _commands)
            {
                if (!c.IsComplete)
                {
                    c.Execute(inSecondsPast, inGameObject);
                }
                IsComplete &= c.IsComplete;
            }
        }
    }
}
