// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
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

        /// <summary>
        /// Adds a Command to the parallel execution list
        /// </summary>
        /// <param name="inCommand"></param>
        /// <returns></returns>
        public ParallelCommand Add(Command inCommand)
        {
            if (inCommand is Wait || inCommand is WaitFor || inCommand is ParallelCommand)
            {
                throw new ArgumentException(String.Format("A ParallelCommand cannot contain a {0} Command", inCommand.GetType().Name));
            }
            _commands.Add(inCommand);

            return this;
        }

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
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