using GameFramework.Network;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-12-01-18:34:47
//备    注：
//===================================================

namespace SuperBiomass.Network
{
    public class SCHeartBeatHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return 10002;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCHeartBeat packetImpl = (SCHeartBeat)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
        }
    }
}