/* Author:          熊哲
 * CreateTime:      2018-05-07 16:19:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity
{
    public class EZDictionary : MonoBehaviour, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public class Element
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
            }

            [SerializeField]
            private Vector4 m_Vector4Value;
            public Vector4 vector4Value
            {
                get
                {
                    CheckType(typeof(Vector4));
                    return m_Vector4Value;
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
            }

            private bool CheckType(System.Type type)
            {
                if (typeName != type.FullName)
                {
                    Debug.LogErrorFormat("Type mismatch: expect {0}, current {1}", type.FullName, typeName);
                    return false;
                }
                return true;
            }
        }

        [SerializeField]
        private Element[] m_Elements;

        public Dictionary<string, Element> dictionary = new Dictionary<string, Element>();

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            dictionary.Clear();
            for (int i = 0; i < m_Elements.Length; i++)
            {
                string key = m_Elements[i].key;
                if (string.IsNullOrEmpty(key)) continue;
                if (dictionary.ContainsKey(key)) continue;
                dictionary.Add(key, m_Elements[i]);
            }
        }

        public Element this[string key]
        {
            get
            {
                return dictionary[key];
            }
        }

        public bool Contains(string key)
        {
            return dictionary.ContainsKey(key);
        }
        public Element Get(string key)
        {
            return dictionary[key];
        }
        public T Get<T>(string key) where T : Object
        {
            return dictionary[key].objectValue as T;
        }
    }
}
