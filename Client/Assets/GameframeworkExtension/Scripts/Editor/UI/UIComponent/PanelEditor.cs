using System.Text;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime.UI;

namespace UnityGameFramework.Editor.UI
{
    [CustomEditor(typeof(Panel))]
    public class PanelEditor : UnityEditor.Editor
    {
        #region Public Attributes
        #endregion

        #region Private Attributes
        private bool haveBackCover = false;
        private const string backCoverName = "BackCover_Panel";
        #endregion

        #region Unity Messages
        //	void Awake()
        //	{
        //
        //	}
        void OnEnable()
        {
            var source = target as Panel;
            if (source.transform.Find(backCoverName) != null)
            {
                haveBackCover = true;
            }
            else
            {
                haveBackCover = false;
            }
        }
        //
        //	void Start() 
        //	{
        //	
        //	}
        //	
        //	void Update() 
        //	{
        //	
        //	}
        //
        //	void OnDisable()
        //	{
        //
        //	}
        //
        //	void OnDestroy()
        //	{
        //
        //	}

        #endregion

        #region Public Methods

        #endregion

        #region Override Methods


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var source = target as Panel;

            EditorGUI.BeginChangeCheck();
            haveBackCover = EditorGUILayout.Toggle("BackCover",haveBackCover);
            if (EditorGUI.EndChangeCheck())
            {
                if (haveBackCover)
                {
                    GameObject go = new GameObject(backCoverName,typeof(RectTransform));

                    // Set RectTransform to stretch
                    RectTransform rectTransform = go.GetComponent<RectTransform>();
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.sizeDelta = Vector2.zero;

                    ImageT image = go.AddComponent<ImageT>();
                    image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
                    image.type = ImageT.Type.Sliced;
                    image.color = new Color(0f, 0f, 0f, 0.392f);
                    image.raycastTarget = true;

                    go.transform.SetParent(source.transform, false);
                    rectTransform.SetAsFirstSibling();
                }
                else
                {
                    var go = source.transform.Find(backCoverName);
                    GameObject.DestroyImmediate(go.gameObject);
                }
            }
        }

        public override bool HasPreviewGUI()
        {
            return true;
        }

        private GUIStyle m_PreviewLabelStyle;

        protected GUIStyle previewLabelStyle
        {
            get
            {
                if (m_PreviewLabelStyle == null)
                {
                    m_PreviewLabelStyle = new GUIStyle("PreOverlayLabel")
                    {
                        richText = true,
                        alignment = TextAnchor.UpperLeft,
                        fontStyle = FontStyle.Normal
                    };
                }

                return m_PreviewLabelStyle;
            }
        }


        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
            var source = target as Panel;
            StringBuilder sb = new StringBuilder();
            sb.Append("Panel Info:\n ").Append(source.ToString());

            GUI.Label(rect, sb.ToString(), previewLabelStyle);
        }
        #endregion

        #region Private Methods

        #endregion

        #region Inner

        #endregion
    }
}
