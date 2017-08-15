/*
 * Author:      熊哲
 * CreateTime:  1/12/2017 5:28:48 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZUnityTools
{
    [ExecuteInEditMode]
    public class EZ3DGridLayout : MonoBehaviour
    {
        public enum AxisOrder { XYZ = 0, XZY = 1, YXZ = 2, YZX = 3, ZXY = 4, ZYX = 5 }

        [SerializeField]
        private AxisOrder m_AxisOrder = AxisOrder.XYZ;
        public AxisOrder axisOrder { get { return m_AxisOrder; } set { m_AxisOrder = value; } }

        [SerializeField]
        private int m_Constraint1 = 5;
        public int constraint1 { get { return m_Constraint1; } set { m_Constraint1 = value; } }

        [SerializeField]
        private int m_Constraint2 = 5;
        public int constraint2 { get { return m_Constraint2; } set { m_Constraint2 = value; } }

        [SerializeField]
        private Vector3 m_Offset = Vector3.zero;
        public Vector3 offset { get { return m_Offset; } set { m_Offset = value; } }

        [SerializeField]
        private Vector3 m_Distance = Vector3.one;
        public Vector3 distance { get { return m_Distance; } set { m_Distance = value; } }

        protected virtual void LateUpdate()
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
    }
}