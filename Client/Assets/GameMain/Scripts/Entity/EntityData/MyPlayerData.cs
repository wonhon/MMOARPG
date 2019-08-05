using UnityEngine;

namespace SuperBiomass
{
    public class MyPlayerData : PlayerData
    {
        [SerializeField]
        private string m_Name = null;

        public MyPlayerData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {

        }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
    }
}
