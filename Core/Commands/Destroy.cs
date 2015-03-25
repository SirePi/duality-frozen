// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// Command used to destroy (dispose and remove from the Scene) a GameObject
    /// </summary>
    public sealed class Destroy : Command
    {
        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            inGameObject.DisposeLater();
            inGameObject.ParentScene.RemoveObject(inGameObject);

            IsComplete = true;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="inGameObject"></param>
        public override void Initialize(GameObject inGameObject)
        {
            // nothing to initialize
        }
    }
}