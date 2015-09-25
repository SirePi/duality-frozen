// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// A utility Command used to send a signal to the other Commands. Used for command synchronization via WaitFor
    /// </summary>
    public sealed class Signal : Command
    {
        internal Signal(string inSignal)
        {
            Value = inSignal;
        }

        /// <summary>
        /// The Value of the Signal
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            IsComplete = true;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="inGameObject"></param>
        public override void Initialize(GameObject inGameObject)
        {
            // nohting to initialize
        }
    }
}