//===================================================
//作    者：WonHon
//创建时间：2019-12-07-20:31:04
//备    注：协议编号定义
//===================================================

namespace GameServer.Network
{
    public class ProtoCodeDef
    {
        /// <summary>
        /// 客户端发送心跳
        /// </summary>
        public const ushort CSHeartBeat = 10001;

        /// <summary>
        /// 服务器返回心跳
        /// </summary>
        public const ushort SCHeartBeat = 10002;

        /// <summary>
        /// 请求邮件列表
        /// </summary>
        public const ushort CSMailList = 11001;
    }
}