using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore.Components
{
    [Serializable]
    public class VisualTreeHider : Component
    {
        public bool IsTreeVisibile
        {
            get { return _treeVisible; }
            set
            {
                _treeVisible = value;
                ChangeTreeVisibility(); 
            }
        }

        private bool _treeVisible;

        public VisualTreeHider()
        {
            _treeVisible = true;
        }

        private void ChangeTreeVisibility()
        {
            foreach (Component c in GameObj.GetComponentsInChildren(typeof(ICmpRenderer)))
            {
                c.Active = _treeVisible;
            }
        }

        public void ToggleVisibility()
        {
            IsTreeVisibile = !IsTreeVisibile;
        }
    }
}
