﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using FrozenCore.Widgets;
using Duality.Resources;

namespace FrozenCore.Resources.DefaultScripts
{
    public class CloseWindow : Script
    {
        public override void Execute(Duality.GameObject inSource, object inParameter)
        {
            inSource.FindAncestorWithComponent<Widget>().GetComponent<Widget>().Close();
        }
    }
}