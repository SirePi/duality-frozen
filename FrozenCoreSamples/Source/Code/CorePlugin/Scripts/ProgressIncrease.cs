using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrozenCore;
using FrozenCore.Widgets;
using Duality.Resources;
using Duality;

namespace FrozenCoreSamples.Scripts
{
    [Serializable]
    public class ProgressIncrease : Script
    {
        public int IncreaseAmount { get; set; }

        public override void Execute(Duality.GameObject inSource, object inParameter)
        {
            GameObject go = Scene.Current.FindGameObject("SkinnedProgressBar");

            if (go != null)
            {
                SkinnedProgressBar progress = go.GetComponent<SkinnedProgressBar>();
                progress.Value = ((progress.Value + IncreaseAmount) % 100);
            }
        }
    }
}
