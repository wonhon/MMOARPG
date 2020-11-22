using System;

namespace UnityGameFramework.Runtime.UI
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct,
                       AllowMultiple = true)
    ] 
    public class ExtForUIAttribute : System.Attribute
    {
        #region Public Attributes

        #endregion

        #region Private Attributes
        Type m_targetType;
        #endregion

        #region Public Methods
        public ExtForUIAttribute(Type type)
        {
            m_targetType = type;
        }

        public Type GetTargetType()
        {
            return m_targetType;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Inner

        #endregion
    }

}