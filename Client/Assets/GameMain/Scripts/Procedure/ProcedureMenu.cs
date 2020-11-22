//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using SuperBiomass.Network;
using SuperBiomass.Web;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;
using NetworkConnectedEventArgs = UnityGameFramework.Runtime.NetworkConnectedEventArgs;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SuperBiomass
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;
        private SelectRoleForm m_SelectRoleForm;
        private int m_RequestAccountInfoSerialId;
        private int m_RegisterAccountSerialId;
        private INetworkChannel m_NetworkChannel;

        public class AccountEvent : UnityEvent<AccountEntity> { }
        public AccountEvent m_AccountEvent;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            if (m_AccountEvent == null)
            {
                m_AccountEvent = new AccountEvent();
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);

            m_AccountEvent.AddListener(OnAccountWebRequset);

            m_StartGame = false;

            RequsetAccountInfo();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);

            m_AccountEvent.RemoveListener(OnAccountWebRequset);

            if (m_AccountEvent != null)
            {
                m_AccountEvent.RemoveAllListeners();
                m_AccountEvent = null;
            }

            m_NetworkChannel = null;

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NEXT_SCENE_ID, (int)SceneType.ChangAn);
                procedureOwner.SetData<VarType>(Constant.ProcedureData.NEXT_PROCEDURE, typeof(ProcedureMain));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        #region Public
        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            GameEntry.UI.CloseUIForm(m_SelectRoleForm);
            m_StartGame = true;
        }

        public void StartGame()
        {
            if (m_NetworkChannel == null)
            {
                m_NetworkChannel = GameEntry.Network.CreateNetworkChannel("Main Server", new NetworkChannelHelper());
                m_NetworkChannel.HeartBeatInterval = 3000;
            }

            if (!m_NetworkChannel.Connected)
            {
                m_NetworkChannel.Connect(IPAddress.Parse("192.168.102.128"), 8877);
            }
        }
        #endregion

        public void Register()
        {
            WWWForm form = new WWWForm();
            LitJson.JsonData jsonData = new LitJson.JsonData();
            jsonData["UserName"] = "wonhon";
            jsonData["Passworld"] = "123";
            form.AddField("", jsonData.ToJson());

            m_RegisterAccountSerialId = GameEntry.WebRequest.AddWebRequest(Constant.Url.ACCOUNT_WEB_SITE, form);
        }

        private void RequsetAccountInfo()
        {
            m_RequestAccountInfoSerialId = GameEntry.WebRequest.AddWebRequest(Constant.Url.ACCOUNT_WEB_SITE + "?id=2");
        }

        private void OnAccountWebRequset(AccountEntity accountEntity)
        {
            Log.Info("玩家{0}帐号信息", accountEntity.UserName);
            GameEntry.UI.OpenUIForm(UIFormId.SelectRole, this);
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs args = e as WebRequestSuccessEventArgs;

            if (args.SerialId == m_RequestAccountInfoSerialId)
            {
                string responseJson = Utility.Converter.GetString(args.GetWebResponseBytes());
                var accountEntity = Utility.Json.ToObject<AccountEntity>(responseJson);
                m_AccountEvent.Invoke(accountEntity);
                return;
            }
            else if (args.SerialId == m_RegisterAccountSerialId)
            {
                string responseJson = Utility.Converter.GetString(args.GetWebResponseBytes());
                Log.Info("net data = {0}", responseJson);
            }
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs args = e as WebRequestFailureEventArgs;
            Log.Error("请求Web信息失败：{0}", args.ErrorMessage);
            return;
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_SelectRoleForm = ne.UIForm.Logic as SelectRoleForm;
        }
    }
}
