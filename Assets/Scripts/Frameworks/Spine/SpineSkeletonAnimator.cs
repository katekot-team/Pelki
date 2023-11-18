using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;
using AnimationState = Spine.AnimationState;

namespace Pelki.Frameworks.Spine
{
    public class SpineSkeletonAnimator : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private List<StateNameToAnimationReference> statesAndAnimations;
        [SerializeField] private List<AnimationTransition> transitions;

        private Animation targetAnimation;

        public void Initialize()
        {
            foreach (StateNameToAnimationReference entry in statesAndAnimations)
                entry.Initialize();

            foreach (AnimationTransition entry in transitions)
                entry.Initialize();
        }

        /// <summary>
        /// Sets the horizontal flip state of the skeleton based on a nonzero float.
        /// If negative, the skeleton is flipped. If positive, the skeleton is not flipped.
        /// </summary>
        public void SetFlip(float horizontal)
        {
            if (horizontal == 0)
            {
                return;
            }

            skeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
        }

        /// <summary>Plays an animation based on the state name. </summary>
        public void PlayAnimationForState(string stateShortName, int trackIndex = 0)
        {
            PlayAnimationForState(StringToHash(stateShortName), trackIndex);
        }

        /// <summary>Gets a Spine Animation based on the state name. </summary>
        public Animation GetAnimationForState(string stateShortName)
        {
            return GetAnimationForState(StringToHash(stateShortName));
        }

        /// <summary>Gets a Spine Animation based on the hash of the state name. </summary>
        public Animation GetAnimationForState(int shortNameHash)
        {
            StateNameToAnimationReference foundState = statesAndAnimations
                .Find(entry => StringToHash(entry.StateName) == shortNameHash);
            return foundState?.Animation;
        }

        /// <summary>
        /// Play an animation. If a transition animation is defined and transition allowed,
        /// the transition is played before the target animation being passed.
        /// </summary>
        public void PlayNewAnimation(Animation target, int trackIndex)
        {
            Animation transition = null;
            Animation current = GetCurrentAnimation(trackIndex);

            if (current != null)
            {
                transition = TryGetTransition(current, target);
            }

            if (transition != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(trackIndex, transition, false);
                skeletonAnimation.AnimationState.AddAnimation(trackIndex, target, true, 0f);
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(trackIndex, target, true);
            }

            targetAnimation = target;
        }

        /// <summary> Play a non-looping animation once then continue playing the state animation. </summary>
        public void PlayOneShot(string shortName, int trackIndex = 0)
        {
            Animation oneShotAnimation = GetAnimationForState(shortName);

            if (oneShotAnimation == null)
            {
                return;
            }

            AnimationState state = skeletonAnimation.AnimationState;
            Animation transition = TryGetTransition(oneShotAnimation, targetAnimation);

            state.SetAnimation(trackIndex, oneShotAnimation, false);

            if (transition != null)
            {
                state.AddAnimation(trackIndex, transition, false, 0f);
            }

            state.AddEmptyAnimation(trackIndex, 0, oneShotAnimation.Duration);
        }

        private void PlayAnimationForState(int shortNameHash, int trackIndex)
        {
            Animation foundAnimation = GetAnimationForState(shortNameHash);

            if (foundAnimation == null)
            {
                return;
            }

            PlayNewAnimation(foundAnimation, trackIndex);
        }

        private Animation TryGetTransition(Animation from, Animation to)
        {
            foreach (AnimationTransition transition in transitions)
            {
                if (transition.From.Animation == from && transition.To.Animation == to)
                {
                    return transition.Transition.Animation;
                }
            }

            return null;
        }

        private Animation GetCurrentAnimation(int layerIndex)
        {
            TrackEntry currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
            return currentTrackEntry?.Animation;
        }

        private int StringToHash(string s) =>
            Animator.StringToHash(s);

        [Serializable]
        private class StateNameToAnimationReference
        {
            [SerializeField] private string stateName;
            [SerializeField] private AnimationReferenceAsset animation;

            public string StateName => stateName;
            public AnimationReferenceAsset Animation => animation;

            public void Initialize()
            {
                Animation.Initialize();
            }
        }

        [Serializable]
        private class AnimationTransition
        {
            [SerializeField] private AnimationReferenceAsset from;
            [SerializeField] private AnimationReferenceAsset to;
            [SerializeField] private AnimationReferenceAsset transition;

            public AnimationReferenceAsset From => from;
            public AnimationReferenceAsset To => to;
            public AnimationReferenceAsset Transition => transition;

            public void Initialize()
            {
                From.Initialize();
                To.Initialize();
                Transition.Initialize();
            }
        }
    }
}