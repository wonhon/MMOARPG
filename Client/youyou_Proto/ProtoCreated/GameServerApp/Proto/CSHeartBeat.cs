using ProtoBuf;
using System;
using System.Collections.Generic;

//===================================================
//作    者：WonHon
//创建时间：2019-12-07-20:31:04
//备    注：
//===================================================

namespace GameServer.Proto
{
    [Serializable, ProtoContract(Name = @"CSHeartBeat")]
    public class CSHeartBeat : CSPacketBase
    {
        public CSHeartBeat()
        {
        }

        public override int Id
        {
            get
            {
                return 10001;
            }
        }

        /// <summary>
        /// 本地时间(毫秒)
        /// </summary>
        [ProtoMember(1, Name = @"LocalTime")]
        public float LocalTime;
        
        public override void Clear()
        {
        }
    }
}