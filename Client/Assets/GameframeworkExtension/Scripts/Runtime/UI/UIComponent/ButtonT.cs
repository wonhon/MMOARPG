using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;

namespace UnityGameFramework.Runtime.UI
{
    public class ButtonT : Button
    {
        #region Public Attributes
        public float IntervalTime = 1.0f;
        #endregion

        #region Private Attributes
        private float m_clickStartTime;
        private bool m_canClick;
        #endregion

        #region Unity Messages
        //	void Awake()
        //	{
        //
        //	}

        protected override void OnEnable()
        {
            m_canClick = true;
        }

        //	void Start() 
        //	{
        //	
        //	}

        private void Update()
        {
            if (!m_canClick)
            {
                if (Time.realtimeSinceStartup >= m_clickStartTime + IntervalTime)
                {
                    m_canClick = true;
                }
            }
        }

        protected override void OnDisable()
        {
            m_canClick = false;
        }

        //	void OnDestroy()
        //	{
        //
        //	}

        #endregion

        #region Public Methods

        #endregion

        #region Override Methods

        public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (m_canClick)
            {
                m_canClick = false;
                m_clickStartTime = Time.realtimeSinceStartup;
                base.OnPointerClick(eventData);
            }
        }
        #endregion

        #region Private Methods
        #endregion

        #region Inner
        #endregion
    }
}
