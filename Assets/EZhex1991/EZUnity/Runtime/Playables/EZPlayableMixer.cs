/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-12-20 13:26:21
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZPlayableMixer<BindingType, BehaviourType> : PlayableBehaviour
        where BehaviourType : class, IPlayableBehaviour, new()
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var binding = (BindingType)playerData;
            if (binding == null) return;

            int inputCount = playable.GetInputCount();
            if (inputCount == 0) return;

            float totalWeight = 0;
            Init(binding);
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<BehaviourType>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();
                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;
                Input(binding, inputBehaviour, inputWeight);
            }
            Output(binding, totalWeight);
        }

        protected virtual void Init(BindingType binding)
        {

        }
        protected virtual void Input(BindingType binding, BehaviourType behaviour, float inputWeight)
        {

        }
        protected virtual void Output(BindingType binding, float totalWeight)
        {

        }
    }
}
