/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-12 17:28:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class EZGridLayout3D : MonoBehaviour
    {
        public enum AxisOrder { XYZ = 0, XZY = 1, YXZ = 2, YZX = 3, ZXY = 4, ZYX = 5 }
        public enum UpdateMode { OnChange = 0, Update = 1, Manual = 2 }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.OnChange;
        public UpdateMode updateMode { get { return m_UpdateMode; } set { SetProperty(ref m_UpdateMode, value); } }

        [SerializeField]
        private AxisOrder m_AxisOrder = AxisOrder.XYZ;
        public AxisOrder axisOrder { get { return m_AxisOrder; } set { SetProperty(ref m_AxisOrder, value); } }

        [SerializeField]
        private int m_Constraint1 = 5;
        public int constraint1 { get { return m_Constraint1; } set { SetProperty(ref m_Constraint1, value); } }

        [SerializeField]
        private int m_Constraint2 = 5;
        public int constraint2 { get { return m_Constraint2; } set { SetProperty(ref m_Constraint2, value); } }

        [SerializeField]
        private Vector3 m_Offset = Vector3.zero;
        public Vector3 offset { get { return m_Offset; } set { SetProperty(ref m_Offset, value); } }

        [SerializeField]
        private Vector3 m_Distance = Vector3.one;
        public Vector3 distance { get { return m_Distance; } set { SetProperty(ref m_Distance, value); } }

        [SerializeField]
        private bool m_ActiveObjectsOnly = true;
        public bool activeObjectsOnly { get { return m_ActiveObjectsOnly; } set { SetProperty(ref m_ActiveObjectsOnly, value); } }

        protected virtual void Update()
        {
            if (updateMode == UpdateMode.Update) ResetChildren();
        }
        protected virtual void OnTransformChildrenChanged()
        {
            if (!this.isActiveAndEnabled) return;
            if (updateMode == UpdateMode.OnChange) ResetChildren();
        }

        public virtual void ResetChildren()
        {
            int index = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (activeObjectsOnly && !child.gameObject.activeSelf) continue;
                int axis1 = index % constraint1;
                int axis2 = (index % (constraint1 * constraint2)) / constraint1;
                int axis3 = index / (constraint1 * constraint2);
                switch (axisOrder)
                {
                    case AxisOrder.XYZ:
                        child.localPosition = new Vector3(axis1 * distance.x, axis2 * distance.y, axis3 * distance.z) + offset;
                        break;
                    case AxisOrder.XZY:
                        child.localPosition = new Vector3(axis1 * distance.x, axis3 * distance.y, axis2 * distance.z) + offset;
                        break;
                    case AxisOrder.YXZ:
                        child.localPosition = new Vector3(axis2 * distance.x, axis1 * distance.y, axis3 * distance.z) + offset;
                        break;
                    case AxisOrder.YZX:
                        child.localPosition = new Vector3(axis3 * distance.x, axis1 * distance.y, axis2 * distance.z) + offset;
                        break;
                    case AxisOrder.ZXY:
                        child.localPosition = new Vector3(axis2 * distance.x, axis3 * distance.y, axis1 * distance.z) + offset;
                        break;
                    case AxisOrder.ZYX:
                        child.localPosition = new Vector3(axis3 * distance.x, axis2 * distance.y, axis1 * distance.z) + offset;
                        break;
                }
                index++;
            }
        }

        protected virtual void SetProperty<T>(ref T currentValue, T newValue)
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue))) return;
            currentValue = newValue;
            ResetChildren();
        }
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!this.isActiveAndEnabled) return;
            ResetChildren();
        }
#endif
    }
}