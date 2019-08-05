using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBiomass
{
    public abstract class PlayerData : TargetableObjectData
    {
        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private int m_Attack = 0;

        [SerializeField]
        private int m_Defense = 0;

        [SerializeField]
        private int m_HurtEffectId;

        [SerializeField]
        private int m_HurtSoundId = 0;

        [SerializeField]
        private DRPlayer m_DRPlayer;

        public PlayerData(int entityId, int typeId, CampType camp)
            : base(entityId, typeId, camp)
        {
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            m_DRPlayer = dtPlayer.GetDataRow(TypeId);
            if (m_DRPlayer == null)
            {
                return;
            }

            m_HurtEffectId = m_DRPlayer.HurtEffectId;
            m_HurtSoundId = m_DRPlayer.HurtSoundId;

            m_MaxHP = m_DRPlayer.HP;
            m_Attack = m_DRPlayer.Attack;
            m_Defense = m_DRPlayer.Defense;
            
            HP = m_MaxHP;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        public int Attack
        {
            get { return m_Attack; }
        }

        public int Defense
        {
            get { return m_Defense; }
        }

        public int HurtEffectId
        {
            get { return m_HurtEffectId; }
        }

        public int HurtSoundId
        {
            get { return m_HurtSoundId; }
        }
    }
}
