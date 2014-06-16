using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrozenCore.Commands
{
    public class ParallelCommand : Command
    {
        private List<Command> _commands;

        public ParallelCommand()
        {
            _commands = new List<Command>();
        }

        public void AddCommand(Command inCommand)
        {
            if (inCommand is Wait || inCommand is WaitFor || inCommand is ParallelCommand)
            {
                throw new InvalidOperationException("A ParallelCommand cannot contain this kind of Command");
            }
            _commands.Add(inCommand);
        }

        public override void Execute(float inSecondsPast, Duality.GameObject inGameObject)
        {
            foreach (Command c in _commands)
            {
                c.Execute(inSecondsPast, inGameObject);
            }
        }
    }
}
