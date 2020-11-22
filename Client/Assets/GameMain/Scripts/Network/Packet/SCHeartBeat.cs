using ProtoBuf;
using System;
using System.Collections.Generic;

//===================================================
//作    者：WonHon
//创建时间：2019-12-07-20:31:04
//备    注：
//===================================================

namespace SuperBiomass.Network
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public class SCHeartBeat : SCPacketBase
    {
        public SCHeartBeat()
        {
        }

        public override int Id
        {
            get
            {
                return 10002;
            }
        }

        /// <summary>
        /// 客户端发送的本地时间(毫秒)
        /// </summary>
        [ProtoMember(1, Name = @"LocalTime")]
        public float LocalTime;

        /// <summary>
        /// 服务器时间(毫秒)
        /// </summary>
        [ProtoMember(2, Name = @"ServerTime")]
        public long ServerTime;
        
        public override void Clear()
        {
        }
    }
}