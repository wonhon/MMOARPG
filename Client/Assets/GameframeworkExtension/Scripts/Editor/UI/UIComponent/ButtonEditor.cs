using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityGameFramework.Runtime.UI;

namespace UnityGameFramework.Editor.UI
{
    [CustomEditor(typeof(ButtonT),true)]
    [CanEditMultipleObjects]
    public class ButtonTEditor : ButtonEditor
    {
        #region Public Attributes

        #endregion

        #region Private Attributes
        #endregion

        #region Unity Messages
        //	void Awake()
        //	{
        //
        //	}
        //	void OnEnable()
        //  {
        //
        //  }
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("IntervalTime"), new GUILayoutOption[0]);
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private Methods

        #endregion

        #region Inner

        #endregion
    }
}
