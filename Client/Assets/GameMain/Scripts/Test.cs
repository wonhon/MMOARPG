using GameFramework;
using GameFramework.Network;
using ProtoBuf;
using ProtoBuf.Meta;
using SuperBiomass.Network;
using System.IO;
using UnityEngine;

//===================================================
//作    者：#Author#
//创建时间：#CreateTime#
//备    注：
//===================================================

namespace SuperBiomass
{
    public class Test : MonoBehaviour
    {
        private MemoryStream cachedStream = new MemoryStream(1024);

        private void Start()
        {
            var header = ReferencePool.Acquire<SCPacketHeader>();
            header.Id = 111;
            header.PacketLength = 10;
            Serializer.Serialize(cachedStream, header);
            ReferencePool.Release(header);

            var packet = ReferencePool.Acquire<SCHeartBeat>();
            packet.LocalTime = 10000.0f;
            packet.ServerTime = 123;
            Serializer.SerializeWithLengthPrefix(cachedStream, packet, PrefixStyle.Fixed32);
            ReferencePool.Release(packet);

            cachedStream.Position = 0;
            cachedStream.SetLength(7);

            var deHeader = (IPacketHeader)RuntimeTypeModel.Default.Deserialize(cachedStream, ReferencePool.Acquire<SCPacketHeader>(), typeof(SCPacketHeader));
            var dePacket = (Packet)RuntimeTypeModel.Default.DeserializeWithLengthPrefix(cachedStream, ReferencePool.Acquire<SCHeartBeat>(), typeof(SCHeartBeat), PrefixStyle.Fixed32, 0); ;
        }
    }
}
