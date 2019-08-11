using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================================
//作    者：WonHon
//创建时间：2019-08-11-00:25:48
//备    注：
//===================================================

namespace SuperBiomass
{
	public partial class TestForm : UGuiForm
	{
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Text.text = "Hello World!";
        }
    }
}

