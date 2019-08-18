//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-18 14:18:22.791
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBiomass
{
    /// <summary>
    /// 角色表。
    /// </summary>
    public class DRPlayer : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取玩家编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取血量。
        /// </summary>
        public int HP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击。
        /// </summary>
        public int Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取防御。
        /// </summary>
        public int Defense
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取受伤特效编号。
        /// </summary>
        public int HurtEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取受伤声音编号。
        /// </summary>
        public int HurtSoundId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
        {
            // Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            string[] columnTexts = dataRowSegment.Source.Substring(dataRowSegment.Offset, dataRowSegment.Length).Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnTexts.Length; i++)
            {
                columnTexts[i] = columnTexts[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnTexts[index++]);
            index++;
            HP = int.Parse(columnTexts[index++]);
            Attack = int.Parse(columnTexts[index++]);
            Defense = int.Parse(columnTexts[index++]);
            HurtEffectId = int.Parse(columnTexts[index++]);
            HurtSoundId = int.Parse(columnTexts[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
        {
            // Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            using (MemoryStream memoryStream = new MemoryStream(dataRowSegment.Source, dataRowSegment.Offset, dataRowSegment.Length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.ReadInt32();
                    HP = binaryReader.ReadInt32();
                    Attack = binaryReader.ReadInt32();
                    Defense = binaryReader.ReadInt32();
                    HurtEffectId = binaryReader.ReadInt32();
                    HurtSoundId = binaryReader.ReadInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
        {
            Log.Warning("Not implemented ParseDataRow(GameFrameworkSegment<Stream>)");
            return false;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
