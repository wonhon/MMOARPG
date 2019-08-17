using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-08-11-21:21:13
//备    注：
//===================================================

namespace SuperBiomass
{
    public partial class GameLoadingForm : UGuiForm
    {
        private ProcedureChangeScene m_ProcedureChangeScene;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureChangeScene = userData as ProcedureChangeScene;

            if (m_ProcedureChangeScene == null)
            {
                Log.Warning("ProcedureChangeScene is null");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_Progressbar.value = m_ProcedureChangeScene.LoadingProgress;
        }
    }
}

