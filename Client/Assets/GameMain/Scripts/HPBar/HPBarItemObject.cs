//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using UnityEngine;

namespace SuperBiomass
{
    public class HPBarItemObject : ObjectBase
    {
        public HPBarItemObject(object target)
            : base()
        {

        }

        protected override void Release(bool isShutdown)
        {
            HPBarItem hpBarItem = (HPBarItem)Target;
            if (hpBarItem == null)
            {
                return;
            }

            Object.Destroy(hpBarItem.gameObject);
        }
    }
}
