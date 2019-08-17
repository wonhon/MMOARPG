//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SuperBiomass
{
    public class ProcedureSplash : ProcedureBase
    {
        private GameObject m_GameInit;
        private float m_StartTime;

        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GameInit = GameObject.Find("UI_GameInit");
            m_StartTime = Time.realtimeSinceStartup;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_GameInit)
            {
                if (Time.realtimeSinceStartup - m_StartTime > 5.0f)
                {
                    GameUtility.SetActive(m_GameInit, false);
                }
                else
                {
                    return;
                }
            }

            // TODO: 增加一个 Splash 动画，这里先跳过
            // 编辑器模式下，直接进入预加载流程；否则，检查一下版本
            ChangeState(procedureOwner, GameEntry.Base.EditorResourceMode ? typeof(ProcedurePreload) : typeof(ProcedureCheckVersion));
        }
    }
}
