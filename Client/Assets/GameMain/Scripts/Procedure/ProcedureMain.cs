//-----------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Network;
using HedgehogTeam.EasyTouch;
using SuperBiomass.Network;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SuperBiomass
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;

        private MyPlayer m_MyPlayer;
        private Transform m_Target;
        private ETCJoystick m_ETCJoystick;
        private bool m_IsMoveStart;
        private INetworkChannel m_NetworkChannel;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu()
        {
            m_GotoMenu = true;
            m_GotoMenuDelaySeconds = 0;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            m_NetworkChannel = GameEntry.Network.GetNetworkChannel("Main Server");

            EasyTouch.On_Swipe += OnSwipe;
            EasyTouch.On_PinchIn += OnPinchIn;
            EasyTouch.On_PinchOut += OnPinchOut;
            EasyTouch.On_SimpleTap += OnSimpleTap;

            m_ETCJoystick = Object.FindObjectOfType<ETCJoystick>();
            m_ETCJoystick.onMoveStart.AddListener(OnMoveStart);
            m_ETCJoystick.onMove.AddListener(OnMove);
            m_ETCJoystick.onMoveEnd.AddListener(OnMoveEnd);

            GameEntry.Entity.ShowMyPlayer(new MyPlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "My Aircraft",
                Position = new Vector3(101.67f, 6.7f, 32.21f),
            });
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            EasyTouch.On_Swipe -= OnSwipe;
            EasyTouch.On_PinchIn -= OnPinchIn;
            EasyTouch.On_PinchOut -= OnPinchOut;
            EasyTouch.On_SimpleTap -= OnSimpleTap;

            m_ETCJoystick.onMoveStart.RemoveListener(OnMoveStart);
            m_ETCJoystick.onMove.RemoveListener(OnMove);
            m_ETCJoystick.onMoveEnd.RemoveListener(OnMoveEnd);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_GotoMenu)
            {
                m_GotoMenuDelaySeconds += elapseSeconds;
                if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
                {
                    procedureOwner.SetData<VarInt>(Constant.ProcedureData.NEXT_SCENE_ID, (int)SceneType.SelectRole);
                    procedureOwner.SetData<VarType>(Constant.ProcedureData.NEXT_PROCEDURE, typeof(ProcedureMenu));
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                    return;
                }
            }

            if (m_MyPlayer != null)
            {
                if (m_Target == null)
                {
                    m_Target = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;
                    Object.Destroy(m_Target.GetComponent<Collider>());
                }

                m_MyPlayer.Move();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_NetworkChannel.Send(new CSMailList() {
                     MailCount = 100,
                });
            }
        }

        private void OnMoveStart()
        {
            if (!m_IsMoveStart)
            {
                m_IsMoveStart = true;
                m_MyPlayer.DisableAIMoveWhenUserControl();
            }
        }

        private void OnMove(Vector2 delatePosition)
        {
            m_ETCJoystick.tmAdditionnalRotation = Mathf.Lerp(m_ETCJoystick.tmAdditionnalRotation, CameraCtrl.Instance.GetCameraEulerAngles().y, Time.deltaTime * 5);
        }

        private void OnMoveEnd()
        {
            if (m_IsMoveStart)
            {
                m_IsMoveStart = false;
                m_MyPlayer.EnableAIMoveWhenUserControl();
            }
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = e as ShowEntitySuccessEventArgs;
            if (ne.EntityLogicType.Equals(typeof(MyPlayer)))
            {
                m_MyPlayer = ne.Entity.Logic as MyPlayer;
                m_MyPlayer.Init(GameObject.Find("WroldMapSceneCtrl/PlayerBornPos").transform.position);
                
                m_ETCJoystick.axisX.directTransform = m_MyPlayer.CachedTransform;
                m_ETCJoystick.axisY.directTransform = m_MyPlayer.CachedTransform;

                CameraCtrl.Instance.Init();
            }
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = e as ShowEntityFailureEventArgs;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        #region EasyTouch
        private void OnSwipe(Gesture gesture)
        {
            if (gesture.touchCount > 1) return;

            switch (gesture.swipe)
            {
                case EasyTouch.SwipeDirection.Up:
                    {
                        _OnSwipeUp();
                    }
                    break;
                case EasyTouch.SwipeDirection.Down:
                    {
                        _OnSwipeDown();
                    }
                    break;
                case EasyTouch.SwipeDirection.Right:
                    {
                        _OnSwipeRight();
                    }
                    break;
                case EasyTouch.SwipeDirection.Left:
                    {
                        _OnSwipeLeft();
                    }
                    break;
            }
        }

        private void _OnSwipeUp()
        {
            CameraCtrl.Instance.SetCameraUpAndDown(0);
        }

        private void _OnSwipeDown()
        {
            CameraCtrl.Instance.SetCameraUpAndDown(1);
        }

        private void _OnSwipeRight()
        {
            CameraCtrl.Instance.SetCameraRotate(1);
        }

        private void _OnSwipeLeft()
        {
            CameraCtrl.Instance.SetCameraRotate(0);
        }

        private void OnPinchIn(Gesture gesture)
        {
            CameraCtrl.Instance.SetCameraZoom(1);
        }

        private void OnPinchOut(Gesture gesture)
        {
            CameraCtrl.Instance.SetCameraZoom(0);
        }

        private void OnSimpleTap(Gesture gesture)
        {
            Vector3? target = CameraCtrl.Instance.GetCameraRayAndGoundCrossPoint(gesture.position);
            if (target != null)
            {
                m_Target.position = (Vector3)target;
                m_MyPlayer.MoveByClickGround(m_Target.position);
            }
        }
        #endregion
    }
}
