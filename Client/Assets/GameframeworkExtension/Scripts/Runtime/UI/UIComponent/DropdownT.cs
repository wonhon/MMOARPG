using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime.UI
{
    public class DropdownT : Dropdown
    {
        
        #region Public Attributes
        public System.Action<bool> onFold;
        public Image m_image;
        #endregion

        #region Private Attributes

        #endregion

        #region Public Methods
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onFold != null)
                onFold(true);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (onFold != null)
                onFold(false);
        }
        #endregion

        #region Override Methods

        #endregion

        #region Private Methods

        #endregion

        #region Inner

        #endregion
    }
}
