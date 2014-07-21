// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;

namespace FrozenCore.Commands
{
    /// <summary>
    /// Command used to destroy (dispose and remove from the Scene) a GameObject
    /// </summary>
    public sealed class Destroy : Command
    {
        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            inGameObject.DisposeLater();
            inGameObject.ParentScene.RemoveObject(inGameObject);

            IsComplete = true;
        }
    }
}
