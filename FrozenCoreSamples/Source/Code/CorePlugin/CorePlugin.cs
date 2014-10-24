using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;

namespace FrozenCoreSamples
{
    /// <summary>
    /// Defines a Duality core plugin.
    /// </summary>
    public class FrozenCoreSamplesCorePlugin : CorePlugin
    {
        private EventHandler _sceneEnteredHandler;

        public FrozenCoreSamplesCorePlugin()
        {
            _sceneEnteredHandler = new EventHandler(Scene_Entered);
        }

        protected override void InitPlugin()
        {
            base.InitPlugin();
        }

        void Scene_Entered(object sender, System.EventArgs e)
        {
        }
        // Override methods here for global logic
    }
}
