using GameFramework;
using Pathfinding;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBiomass
{
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(IAstarAI))]
    public class Player : TargetableObject
    {
        [SerializeField]
        private PlayerData m_PlayerData;

        private Animator m_Animator;
        private CharacterController m_CharacterController;
        private Seeker m_Seeker;
        private IAstarAI m_AI;

        private bool m_IsAtDestination;
        private string m_CurrentTrigger;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("Aircraft data is invalid.");
                return;
            }

            Name = Utility.Text.Format("Player ({0})", Id.ToString());

            CachedTransform.position = m_PlayerData.Position;
            m_Animator = GetComponentInChildren<Animator>();
            m_CharacterController = GetComponent<CharacterController>();
            m_Seeker = GetComponent<Seeker>();
            m_AI = GetComponent<IAstarAI>();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            m_Animator = null;
            m_CharacterController = null;
            m_Seeker = null;
            m_AI = null;

            base.OnHide(isShutdown, userData);
        }

        protected override void OnHurt(Entity attacker)
        {
            base.OnHurt(attacker);

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_PlayerData.HurtEffectId)
            {
                Position = CachedTransform.localPosition,
            });
            GameEntry.Sound.PlaySound(m_PlayerData.HurtSoundId);
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_PlayerData.Camp, m_PlayerData.HP, m_PlayerData.Attack, m_PlayerData.Defense);
        }

        public void Init(Vector3 initPosition)
        {
            CachedTransform.position = initPosition;
            m_CharacterController.enabled = true;
        }

        public void Move()
        {
            CameraCtrl.Instance.transform.position = CachedTransform.position;
            CameraCtrl.Instance.LookAt(CachedTransform.position);

            if (m_Seeker != null && m_AI != null)
            {
                if (m_AI.reachedEndOfPath)
                {
                    if (!m_IsAtDestination)
                    {
                        PlayIdleNormal();
                        m_IsAtDestination = true;
                    }
                }
                else
                {
                    m_IsAtDestination = false;
                }

                CachedTransform.InverseTransformDirection(m_AI.velocity);
            }
        }

        public void MoveByClickGround(Vector3 target)
        {
            PlayIdleRun();
            m_Seeker.StartPath(CachedTransform.position, target);
            m_AI.destination = target;
        }

        public void DisableAIMoveWhenUserControl()
        {
            PlayIdleRun();
            m_AI.canMove = false;
            m_AI.canSearch = false;
            m_Seeker.enabled = false;
        }

        public void EnableAIMoveWhenUserControl()
        {
            PlayIdleNormal();
            m_AI.canMove = true;
            m_AI.canSearch = true;
            m_Seeker.enabled = true;
            m_AI.destination = CachedTransform.position;
        }

        private void PlayIdleNormal()
        {
            if (m_Animator)
            {
                PlayAnimation("idle_normal");
            }
        }

        private void PlayIdleRun()
        {
            if (m_Animator)
            {
                PlayAnimation("run");
            }
        }

        private void PlayAnimation(string triggerName)
        {
            if (!triggerName.Equals(m_CurrentTrigger))
            {
                m_CurrentTrigger = triggerName;
                m_Animator.SetTrigger(m_CurrentTrigger);
            }
        }
    }
}
