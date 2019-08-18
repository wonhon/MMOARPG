using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-08-11-22:55:42
//备    注：登陆界面
//===================================================

namespace SuperBiomass
{
    public partial class LogOnForm : UGuiForm
    {
        private ProcedureLogOn m_ProcedureLogOn;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_LogOn.onClick.RemoveAllListeners();
            m_LogOn.onClick.AddListener(OnLogOn);

            m_ToReg.onClick.RemoveAllListeners();
            m_ToReg.onClick.AddListener(OnToReg);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureLogOn = userData as ProcedureLogOn;
            if (m_ProcedureLogOn == null)
            {
                Log.Warning("ProcedureLogOn is null");
                return;
            }
        }

        private void OnLogOn()
        {
            m_ProcedureLogOn.SelectRole();
        }

        private void OnToReg()
        {
            m_ProcedureLogOn.SelectRole();
        }
    }
}