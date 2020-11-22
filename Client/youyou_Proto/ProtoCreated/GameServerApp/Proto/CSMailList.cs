using ProtoBuf;
using System;
using System.Collections.Generic;

//===================================================
//作    者：WonHon
//创建时间：2019-12-07-20:31:04
//备    注：请求邮件列表
//===================================================

namespace GameServer.Proto
{
    [Serializable, ProtoContract(Name = @"CSMailList")]
    public class CSMailList : CSPacketBase
    {
        public CSMailList()
        {
        }

        public override int Id
        {
            get
            {
                return 11001;
            }
        }

        /// <summary>
        /// 邮件数量
        /// </summary>
        [ProtoMember(1, Name = @"MailCount")]
        public int MailCount;

        /// <summary>
        /// 邮件信息
        /// </summary>
        /// </summary>
        [ProtoMember(2, Name = @"Mail")]
        public List<MailInfo> MailList;
        
        /// <summary>
        /// 邮件信息
        /// </summary>
        [Serializable, ProtoContract(Name = @"MailInfo")]
        public class MailInfo
        {
            /// <summary>
            /// 邮件编号
            /// </summary>
            [ProtoMember(1, Name = @"MailId")]
            public int MailId;

            /// <summary>
            /// 邮件类型
            /// </summary>
            [ProtoMember(2, Name = @"MailType")]
            public int MailType;

        }

        public override void Clear()
        {
        }
    }
}