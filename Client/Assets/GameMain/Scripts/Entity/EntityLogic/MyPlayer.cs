using Pathfinding;

using UnityEngine;

namespace SuperBiomass
{
    public sealed class MyPlayer : Player
    {
        [SerializeField]
        private MyPlayerData m_MyPlayerData = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_MyPlayerData = userData as MyPlayerData;
        }
    }
}
