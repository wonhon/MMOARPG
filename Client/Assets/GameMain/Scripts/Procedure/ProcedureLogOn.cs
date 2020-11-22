//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SuperBiomass
{
    public class ProcedureLogOn : ProcedureBase
    {
        private LogOnForm m_LogOnForm;
        private bool m_SelectRole = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void SelectRole()
        {
            m_SelectRole = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_SelectRole = false;
            GameEntry.UI.OpenUIForm(UIFormId.LogOn, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_LogOnForm != null)
            {
                m_LogOnForm.Close(isShutdown);
                m_LogOnForm = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_SelectRole)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NEXT_SCENE_ID, (int)SceneType.SelectRole);
                procedureOwner.SetData<VarType>(Constant.ProcedureData.NEXT_PROCEDURE, typeof(ProcedureMenu));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LogOnForm = (LogOnForm)ne.UIForm.Logic;
        }
    }
}
