//===================================================
//作    者：WonHon
//创建时间：2019-07-21 21:21:32
//备    注：相机控制类
//===================================================
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace SuperBiomass
{
    /// <summary>
    /// 相机控制器
    /// </summary>
    public class CameraCtrl : MonoBehaviour
    {
        public static CameraCtrl Instance;

        /// <summary>
        /// 控制摄像机上下
        /// </summary>
        [SerializeField]
        private Transform m_CameraUpAndDown;

        /// <summary>
        /// 摄像机缩放父物体
        /// </summary>
        [SerializeField]
        private Transform m_CameraZoomContainer;

        /// <summary>
        /// 摄像机容器
        /// </summary>
        [SerializeField]
        private Transform m_CameraContainer;
        
        private Camera m_Camera;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_Camera = GetComponentInChildren<Camera>();
        }

        public Vector3? GetCameraRayAndGoundCrossPoint(Vector2 screenPosition)
        {
            Ray ray = m_Camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }
            return null;
        }
            
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Vector3 angles = m_CameraUpAndDown.localEulerAngles;
            angles.x = Mathf.Clamp(m_CameraUpAndDown.localEulerAngles.x, 20f, 80f);
            m_CameraUpAndDown.localEulerAngles = angles;
        }

        /// <summary>
        /// 设置摄像机旋转 0=左 1=右
        /// </summary>
        /// <param name="type"></param>
        public void SetCameraRotate(int type)
        {
            transform.Rotate(0, 100 * Time.deltaTime * (type == 0 ? -1 : 1), 0);
        }

        /// <summary>
        /// 设置摄像机上下 0=上 1=下
        /// </summary>
        /// <param name="type"></param>
        public void SetCameraUpAndDown(int type)
        {
            m_CameraUpAndDown.Rotate(50 * Time.deltaTime * (type == 1 ? -1 : 1), 0, 0);
            m_CameraUpAndDown.localEulerAngles = new Vector3(Mathf.Clamp(m_CameraUpAndDown.localEulerAngles.x, 20f, 80f), 0, 0);
        }

        /// <summary>
        /// 设置摄像机缩放 0=拉近 1=拉远
        /// </summary>
        /// <param name="type"></param>
        public void SetCameraZoom(int type)
        {
            m_CameraZoomContainer.AddLocalPositionZ(10f * Time.deltaTime * ((type == 1 ? -1 : 1)));
            m_CameraZoomContainer.SetLocalPositionZ(Mathf.Clamp(m_CameraZoomContainer.localPosition.z, -15f, -1.5f));
        }

        public void LookAt(Vector3 target)
        {
            transform.LookAt(target);
        }

        public Vector3 GetCameraEulerAngles()
        {
            return m_CameraContainer.eulerAngles;
        }

        /// <summary>
        /// //震屏
        /// </summary>
        /// <param name="delay">延迟时间</param>
        /// <param name="duration">持续时间</param>
        /// <param name="strength">强度</param>
        /// <param name="vibrato">震幅</param>
        /// <returns></returns>
        public void CameraShake(float delay = 0, float duration = 0.5f, float strength = 1, int vibrato = 10)
        {
            StartCoroutine(DOCameraShake(delay, duration, strength, vibrato));
        }

        /// <summary>
        /// //震屏
        /// </summary>
        /// <param name="delay">延迟时间</param>
        /// <param name="duration">持续时间</param>
        /// <param name="strength">强度</param>
        /// <param name="vibrato">震幅</param>
        /// <returns></returns>
        private IEnumerator DOCameraShake(float delay = 0, float duration = 0.5f, float strength = 1, int vibrato = 10)
        {
            yield return new WaitForSeconds(delay);

            m_CameraContainer.DOShakePosition(0.3f, 1f, 100);
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, 15f);

        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(transform.position, 14f);

        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(transform.position, 12f);
        //}
    }
}