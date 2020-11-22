//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Network;
using ProtoBuf;
using System;

namespace SuperBiomass
{
    [ProtoInclude(100, typeof(CSPacketHeader))]
    [ProtoInclude(101, typeof(SCPacketHeader))]
    [Serializable, ProtoContract(Name = @"PacketHeaderBase")]
    public abstract class PacketHeaderBase : IPacketHeader, IReference
    {
        public abstract PacketType PacketType
        {
            get;
        }

        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public int PacketLength
        {
            get;
            set;
        }

        public bool IsValid
        {
            get
            {
                return PacketType != PacketType.Undefined && Id > 0 && PacketLength >= 0;
            }
        }

        public void Clear()
        {
            Id = 0;
            PacketLength = 0;
        }
    }
}
