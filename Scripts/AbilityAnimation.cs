using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class TriggerAndWaitAnimationTime: CustomYieldInstruction
    {
        public override bool keepWaiting => Time.time >= EndTime;

        public float EndTime { get; }

        public TriggerAndWaitAnimationTime(string trigger, float seconds, Animator animatorTarget)
        {
            this.EndTime = Time.time + seconds;
            animatorTarget.SetTrigger(trigger);
        }
    }

    public class PlayAndWaitAnimationTime : CustomYieldInstruction
    {
        public override bool keepWaiting => Time.time >= EndTime;

        public float EndTime { get; }

        public PlayAndWaitAnimationTime(Animator animationTarget, float time, AnimationPlayOptions options, bool normalizedTime = false)
        {
            animationTarget.CrossFade(options.StateName, options.NormalizedTransitionDuration, options.Layer, options.NormalizedTimeOffset);
            this.EndTime = Time.time + (normalizedTime ? animationTarget.GetCurrentAnimatorStateInfo(options.Layer).length * time : time);
        }
    }

    public struct AnimationPlayOptions
    {
        public string StateName;
        public float NormalizedTransitionDuration;
        public int Layer;
        public float NormalizedTimeOffset;

        public AnimationPlayOptions(string stateName, float normalizedTransitionDuration = 0.2f, int layer = 0, float normalizedTimeOffset = 0)
        {
            StateName = stateName;
            NormalizedTransitionDuration = normalizedTransitionDuration;
            Layer = layer;
            NormalizedTimeOffset = normalizedTimeOffset;
        }
    }
}
