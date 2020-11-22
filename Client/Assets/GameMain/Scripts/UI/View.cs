using GameFramework.Event;
using System;
using UnityEngine;

//===================================================
//作    者：WonHon
//创建时间：2019-08-26-23:43:06
//备    注：
//===================================================

namespace SuperBiomass
{
    public class UIEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UIEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SerialId;
        public string MsgName;

        public override void Clear()
        {
            MsgName = string.Empty;
        }
    }

    public class UIEventArgs<TData> : GameEventArgs
    {
        public static readonly int EventId = typeof(UIEventArgs<TData>).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SerialId;
        public string MsgName;
        public TData UserData;

        public override void Clear()
        {
            MsgName = string.Empty;
            UserData = default;
        }
    }

    public abstract class View : MonoBehaviour
    {
        public const string CLOSE_BTN = "Close Button";
        private int m_SerialId = 0;

        #region virtual
        public virtual void OnOpen(int serialId)
        {
            m_SerialId = serialId;
        }

        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        protected virtual void OnClose()
        {
            Fire(CLOSE_BTN);
        }
        #endregion

        #region event
        public void Fire(string msgName)
        {
            GameEntry.Event.Fire(UIEventArgs.EventId, new UIEventArgs
            {
                SerialId = m_SerialId,
                MsgName = msgName
            });
        }

        public void Fire<TData>(string msgName, TData data)
        {
            GameEntry.Event.Fire(UIEventArgs<TData>.EventId, new UIEventArgs<TData>
            {
                SerialId = m_SerialId,
                MsgName = msgName,
                UserData = data
            });
        }

        public void AddListener(EventHandler<GameEventArgs> action)
        {
            GameEntry.Event.Subscribe(UIEventArgs.EventId, action);
        }

        public void RemoveListener(EventHandler<GameEventArgs> action)
        {
            GameEntry.Event.Unsubscribe(UIEventArgs.EventId, action);
        }

        public void AddListener<TData>(EventHandler<GameEventArgs> action)
        {
            GameEntry.Event.Subscribe(UIEventArgs<TData>.EventId, action);
        }

        public void RemoveListener<TData>(EventHandler<GameEventArgs> action)
        {
            GameEntry.Event.Unsubscribe(UIEventArgs<TData>.EventId, action);
        }
        #endregion

        protected void SetActive(GameObject gameObject, bool isActive)
        {
            if (gameObject.activeSelf != isActive)
            {
                gameObject.SetActive(isActive);
            }
        }
    }
}

