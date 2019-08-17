using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================================
//作    者：WonHon
//创建时间：2019-08-11-23:17:27
//备    注：
//===================================================

namespace SuperBiomass
{
    public class GameUtility
    {
        public static void SetActive(GameObject gameObject, bool active)
        {
            if (active != gameObject.activeInHierarchy)
            {
                gameObject.SetActive(active);
            }
        }
    }
}

