using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime.UI
{
    public class ScrollRectT : ScrollRect
    {
		private bool m_Isdragging = false;
		override protected void SetContentAnchoredPosition(Vector2 position) {	
			if (m_Isdragging || velocity.magnitude > 2f) {
				base.SetContentAnchoredPosition (position);
			}
		}
		override public void OnBeginDrag(PointerEventData eventData) {
			m_Isdragging = true;
			base.OnBeginDrag (eventData);
		}
		override public void OnEndDrag(PointerEventData eventData) {
			m_Isdragging = false;
			base.OnEndDrag (eventData);
		}
    }

}