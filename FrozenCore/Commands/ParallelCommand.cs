using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrozenCore.Commands
{
    /// <summary>
    /// Command encapsulating a list of Commands that will be carried out in parallel on a single GameObject.
    /// The ParallelCommand will be considered Complete once all subCommands are completed.
    /// </summary>
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
