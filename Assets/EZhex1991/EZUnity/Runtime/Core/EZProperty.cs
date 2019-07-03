/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-14 15:17:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [System.Serializable]
    public class EZProperty
    {
        [SerializeField]
        private string m_TypeName = typeof(Object).FullName;
        public string typeName { get { return m_TypeName; } }

        [SerializeField]
        private string m_Key;
        public string key { get { return m_Key; } }

        [SerializeField]
        private Object m_ObjectValue;
        public Object objectValue
        {
            get
            {
                return m_ObjectValue;
            }
            set
            {
                m_ObjectValue = value;
            }
        }

        [SerializeField]
        private int m_IntValue;
        public int intValue
        {
            get
            {
                CheckType(typeof(int));
                return m_IntValue;
            }
            set
            {
                m_IntValue = value;
            }
        }

        [SerializeField]
        private long m_LongValue;
        public long longValue
        {
            get
            {
                CheckType(typeof(long));
                return m_LongValue;
            }
            set
            {
                m_LongValue = value;
            }
        }

        [SerializeField]
        private bool m_BoolValue;
        public bool boolValue
        {
            get
            {
                CheckType(typeof(bool));
                return m_BoolValue;
            }
            set
            {
                m_BoolValue = value;
            }
        }

        [SerializeField]
        private float m_FloatValue;
        public float floatValue
        {
            get
            {
                CheckType(typeof(float));
                return m_FloatValue;
            }
            set
            {
                m_FloatValue = value;
            }
        }

        [SerializeField]
        private double m_DoubleValue;
        public double doubleValue
        {
            get
            {
                CheckType(typeof(double));
                return m_DoubleValue;
            }
            set
            {
                m_DoubleValue = value;
            }
        }

        [SerializeField]
        private string m_StringValue;
        public string stringValue
        {
            get
            {
                CheckType(typeof(string));
                return m_StringValue;
            }
            set
            {
                m_StringValue = value;
            }
        }

        [SerializeField]
        private Color m_ColorValue;
        public Color colorValue
        {
            get
            {
                CheckType(typeof(Color));
                return m_ColorValue;
            }
            set
            {
                m_ColorValue = value;
            }
        }

        [SerializeField]
        private AnimationCurve m_AnimationCurveValue;
        public AnimationCurve animationCurveValue
        {
            get
            {
                CheckType(typeof(AnimationCurve));
                return m_AnimationCurveValue;
            }
            set
            {
                m_AnimationCurveValue = value;
            }
        }

        [SerializeField]
        private Vector2 m_Vector2Value;
        public Vector2 vector2Value
        {
            get
            {
                CheckType(typeof(Vector2));
                return m_Vector2Value;
            }
            set
            {
                m_Vector2Value = value;
            }
        }

        [SerializeField]
        private Vector3 m_Vector3Value;
        public Vector3 vector3Value
        {
            get
            {
                CheckType(typeof(Vector3));
                return m_Vector3Value;
            }
            set
            {
                m_Vector3Value = value;
            }
        }

        [SerializeField]
        private Vector2Int m_Vector2IntValue;
        public Vector2Int vector2IntValue
        {
            get
            {
                CheckType(typeof(Vector2Int));
                return m_Vector2IntValue;
            }
            set
            {
                m_Vector2IntValue = value;
            }
        }

        [SerializeField]
        private Vector3Int m_Vector3IntValue;
        public Vector3Int vector3IntValue
        {
            get
            {
                CheckType(typeof(Vector3Int));
                return m_Vector3IntValue;
            }
            set
            {
                m_Vector3IntValue = value;
            }
        }

        private bool CheckType(System.Type type)
        {
            if (type.FullName != typeName)
            {
                Debug.LogErrorFormat("Type mismatch: expect {0}, current {1}", type.FullName, typeName);
                return false;
            }
            return true;
        }
    }
}
