using GameFramework.Network;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-12-07-20:31:04
//备    注：
//===================================================

namespace GameServer.Network
{
    public class CSHeartBeatHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return 10001;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            CSHeartBeat packetImpl = (CSHeartBeat)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
        }
    }
}