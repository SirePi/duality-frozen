// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    
    [EditorHintImage(ResNames.ImageScript)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public abstract class Script : Resource
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inSource"></param>
        public void Execute(GameObject inSource)
        {
            Execute(inSource, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSource"></param>
        /// <param name="inParameter"></param>
        public abstract void Execute(GameObject inSource, object inParameter);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ContentRef<Script> ToScriptContentRef()
        {
            return new ContentRef<Script>(this);
        }
    }
}