/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-06 17:50:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    [System.Serializable]
    public class EZTransformConstraintMixer : PlayableBehaviour
    {
        public bool positionConstraint;
        public bool rotationConstraint;
        public bool scaleConstraint;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Transform binding = playerData as Transform;
            if (binding == null) return;

            int inputCount = playable.GetInputCount();
            if (inputCount == 0) return;

            float totalWeight = 0;
            Vector3 position = new Vector3();
            Quaternion rotation = new Quaternion();
            Vector3 scale = new Vector3();
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZTransformConstraintBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();
                if (inputBehaviour.target == null) continue;

                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight == 0) continue;

                totalWeight += inputWeight;
                position += inputBehaviour.target.position * inputWeight;
                rotation = QuaternionExt.Cumulate(rotation, inputBehaviour.target.rotation.Scale(inputWeight));
                scale += inputBehaviour.target.lossyScale * inputWeight;
            }

            if (totalWeight < 1e-5) return;
            if (positionConstraint)
            {
                binding.position = position / totalWeight;
            }
            if (rotationConstraint)
            {
                binding.rotation = rotation;
            }
            if (scaleConstraint)
            {
                binding.localScale = scale / totalWeight;
            }
        }
    }
}
