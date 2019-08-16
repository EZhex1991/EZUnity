/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-24 14:03:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    [Serializable]
    public class EZMaterialPropertyBlockBehaviour : PlayableBehaviour
    {
        public EZMaterialFloatRange[] floatRanges;
        public EZMaterialColorRange[] colorRanges;
        public EZMaterialVectorRange[] vectorRanges;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        public float process { get; set; }

        public MaterialPropertyBlock m_PropertyBlock;
        private MaterialPropertyBlock propertyBlock
        {
            get
            {
                if (m_PropertyBlock == null)
                {
                    m_PropertyBlock = new MaterialPropertyBlock();
                }
                return m_PropertyBlock;
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            process = curve.Evaluate((float)(playable.GetTime() / playable.GetDuration()));
        }
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Renderer renderer = playerData as Renderer;
            if (renderer == null) return;

            renderer.GetPropertyBlock(propertyBlock);
            for (int i = 0; i < floatRanges.Length; i++)
            {
                floatRanges[i].SetBlockValue(propertyBlock, process);
            }
            for (int i = 0; i < colorRanges.Length; i++)
            {
                colorRanges[i].SetBlockValue(propertyBlock, process);
            }
            for (int i = 0; i < vectorRanges.Length; i++)
            {
                vectorRanges[i].SetBlockValue(propertyBlock, process);
            }
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
