using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================================
//作    者：WonHon
//创建时间：2019-08-11-20:52:43
//备    注：
//===================================================

namespace SuperBiomass
{
    /// <summary>
    /// bool 变量类。
    /// </summary>
    public class VarType : Variable<Type>
    {
        /// <summary>
        /// 初始化 ProcedureBase 变量类的新实例。
        /// </summary>
        public VarType()
        {
        }

        /// <summary>
        /// 初始化 ProcedureBase 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarType(Type value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 VarProcedure 到 ProcedureBase 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarType(Type value)
        {
            return new VarType(value);
        }

        /// <summary>
        /// 从 ProcedureBase 变量类到 VarProcedure 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Type(VarType value)
        {
            return value.Value;
        }
    }
}