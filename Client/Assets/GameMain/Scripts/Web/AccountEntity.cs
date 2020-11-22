using System;

namespace SuperBiomass.Web
{
    public class AccountEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///用户名 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///密码 
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 元宝
        /// </summary>
        public int Yuanbao { get; set; }

        ///最后登录服务器Id 
        /// </summary>
        public int LastLogOnServerId { get; set; }

        /// <summary>
        ///最后登录服务器名称 
        /// </summary>
        public string LastLogOnServerName { get; set; }

        /// <summary>
        ///创建时间 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///更新时间 
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
