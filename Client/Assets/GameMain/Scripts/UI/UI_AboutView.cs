//===================================================
//作    者：WonHon
//创建时间：2019-09-01-15:04:08
//备    注：
//===================================================

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SuperBiomass
{
    public partial class UI_AboutView : View
    {
        [SerializeField]
        private float m_ScrollSpeed = 1f;

        private float m_InitPosition = 0f;

        public void Init()
        {
            CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Log.Warning("Can not find CanvasScaler component.");
                return;
            }

            m_InitPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;

            m_BackButtonBtn.onClick.AddListener(OnClose);
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_ContentRectTsf.AddLocalPositionY(m_ScrollSpeed * elapseSeconds);
            if (m_ContentRectTsf.localPosition.y > m_ContentRectTsf.sizeDelta.y - m_InitPosition)
            {
                m_ContentRectTsf.SetLocalPositionY(m_InitPosition);
            }
        }
    }
}
