// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;

namespace FrozenCore.Components
{
    [Serializable]
    public class VisualTreeHider : Component
    {
        private bool _treeVisible;
        public bool IsTreeVisibile
        {
            get { return _treeVisible; }
            set
            {
                _treeVisible = value;
                ChangeTreeVisibility();
            }
        }

        public VisualTreeHider()
        {
            _treeVisible = true;
        }

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