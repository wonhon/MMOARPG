using UnityEngine;

//===================================================
//作    者：WonHon
//创建时间：2019-07-28-17:44:10
//备    注：镜头跟随
//===================================================

namespace SuperBiomass
{
    /// <summary>
    /// 
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float distance = 3.0f;
        public float height = 3.0f;
        public float damping = 5.0f;
        public bool smoothRotation = true;
        public bool followBehind = true;
        public float rotationDamping = 10.0f;
        public bool staticOffset = false;

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 wantedPosition;

            if (staticOffset)
            {
                wantedPosition = target.position + new Vector3(0, height, distance);
            }
            else
            {
                if (followBehind)
                    wantedPosition = target.TransformPoint(0, height, -distance);
                else
                    wantedPosition = target.TransformPoint(0, height, distance);
            }
            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

            if (smoothRotation)
            {
                Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
            }
            else
            {
                transform.LookAt(target, target.up);
            }
        }
    }
}
