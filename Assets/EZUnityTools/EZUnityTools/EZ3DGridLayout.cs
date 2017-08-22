/*
 * Author:      熊哲
 * CreateTime:  1/12/2017 5:28:48 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnityTools
{
    [ExecuteInEditMode]
    public class EZ3DGridLayout : MonoBehaviour
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

        protected virtual void Update()
        {
            if (updateMode == UpdateMode.Update) ResetChildren();
        }
        protected virtual void OnTransformChildrenChanged()
        {
            if (updateMode == UpdateMode.OnChange) ResetChildren();
        }

        public virtual void ResetChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int axis1 = i % constraint1;
                int axis2 = (i % (constraint1 * constraint2)) / constraint1;
                int axis3 = i / (constraint1 * constraint2);
                switch (axisOrder)
                {
                    case AxisOrder.XYZ:
                        transform.GetChild(i).localPosition = new Vector3(axis1 * distance.x, axis2 * distance.y, axis3 * distance.z) + offset;
                        break;
                    case AxisOrder.XZY:
                        transform.GetChild(i).localPosition = new Vector3(axis1 * distance.x, axis3 * distance.y, axis2 * distance.z) + offset;
                        break;
                    case AxisOrder.YXZ:
                        transform.GetChild(i).localPosition = new Vector3(axis2 * distance.x, axis1 * distance.y, axis3 * distance.z) + offset;
                        break;
                    case AxisOrder.YZX:
                        transform.GetChild(i).localPosition = new Vector3(axis3 * distance.x, axis1 * distance.y, axis2 * distance.z) + offset;
                        break;
                    case AxisOrder.ZXY:
                        transform.GetChild(i).localPosition = new Vector3(axis2 * distance.x, axis3 * distance.y, axis1 * distance.z) + offset;
                        break;
                    case AxisOrder.ZYX:
                        transform.GetChild(i).localPosition = new Vector3(axis3 * distance.x, axis2 * distance.y, axis1 * distance.z) + offset;
                        break;
                }
            }
        }

        protected virtual void SetProperty<T>(ref T currentValue, T newValue)
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue))) return;
            currentValue = newValue;
            ResetChildren();
        }
    }
}