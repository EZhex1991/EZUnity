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
    public class EZMaterialPropertyBehaviour : PlayableBehaviour
    {
        public EZMaterialFloatClipInfo[] floatClips;
        public EZMaterialColorClipInfo[] colorClips;
        public EZMaterialVectorClipInfo[] vectorClips;
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

            for (int i = 0; i < floatClips.Length; i++)
            {
                floatClips[i].SetBlockValue(propertyBlock, process);
            }
            for (int i = 0; i < colorClips.Length; i++)
            {
                colorClips[i].SetBlockValue(propertyBlock, process);
            }
            for (int i = 0; i < vectorClips.Length; i++)
            {
                vectorClips[i].SetBlockValue(propertyBlock, process);
            }
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
