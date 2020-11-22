using UnityEngine;
using UnityEngine.UI;

public class UICheckRaycastTargetFlag : MonoBehaviour {

    private Vector3[] m_FourConers = new Vector3[4];

    private float m_StartTime;

    private void Start()
    {
        m_StartTime = Time.realtimeSinceStartup;
    }

    private void OnDrawGizmos()
    {
        if (Time.realtimeSinceStartup - m_StartTime > 1)
        {
            m_StartTime = Time.realtimeSinceStartup;
        }

        Check();
    }

    private void Check()
    {
        foreach (MaskableGraphic g in FindObjectsOfType<MaskableGraphic>())
        {
            if (g.raycastTarget)
            {
                RectTransform rectTransform = g.rectTransform;
                rectTransform.GetWorldCorners(m_FourConers);

                Gizmos.color = Color.blue;
                for (int i = 0; i < m_FourConers.Length; i++)
                {
                    Gizmos.DrawLine(m_FourConers[i], m_FourConers[(i + 1) % 4]);
                }
            }
        }
    }
}
