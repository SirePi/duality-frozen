// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.Core.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Components
{
    /// <summary>
    /// Component that can Mark an entire GameObject, its Childrens and all their Rendering Components as Active or Inactive
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageVisualTreeHider)]
    [EditorHintCategory(typeof(Res), ResNames.Category)]
    public class VisualTreeHider : Component
    {
        private bool _treeVisible;

        /// <summary>
        ///
        /// </summary>
        public VisualTreeHider()
        {
            _treeVisible = true;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsTreeVisibile
        {
            get { return _treeVisible; }
            set
            {
                _treeVisible = value;
                ChangeTreeVisibility();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ToggleVisibility()
        {
            IsTreeVisibile = !IsTreeVisibile;
        }

        private void ChangeTreeVisibility()
        {
            foreach (Component c in GameObj.GetComponentsInChildren(typeof(ICmpRenderer)))
            {
                c.Active = _treeVisible;
            }
        }
    }
}