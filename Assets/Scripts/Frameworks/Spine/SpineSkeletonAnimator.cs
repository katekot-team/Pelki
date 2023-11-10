using System;
using System.Collections;
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
        private bool isStateChangingLocked;
        private Coroutine sateLockRoutine;
        private string lastInvokedAnimationName;

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
                return;

            skeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
        }

        /// <summary>Plays an animation based on the state name and cash invocation for after-lock state</summary>
        public void PlayAnimationForState(string stateShortName, int layerIndex)
        {
            lastInvokedAnimationName = stateShortName;
            PlayAnimationForState(StringToHash(stateShortName), layerIndex);
        }

        /// <summary>Gets a Spine Animation based on the state name.</summary>
        public Animation GetAnimationForState(string stateShortName) =>
            GetAnimationForState(StringToHash(stateShortName));

        /// <summary>Gets a Spine Animation based on the hash of the state name.</summary>
        public Animation GetAnimationForState(int shortNameHash)
        {
            StateNameToAnimationReference foundState =
                statesAndAnimations.Find(entry => StringToHash(entry.stateName) == shortNameHash);
            return foundState?.animation;
        }

        /// <summary>
        /// Play an animation. If a transition animation is defined and transition allowed,
        /// the transition is played before the target animation being passed.
        /// </summary>
        public void PlayNewAnimation(Animation target, int layerIndex, bool withoutTransition = false)
        {
            Animation transition = null;
            Animation current = GetCurrentAnimation(layerIndex);

            if (current != null && withoutTransition == false)
                transition = TryGetTransition(current, target);

            if (transition != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(layerIndex, transition, false);
                skeletonAnimation.AnimationState.AddAnimation(layerIndex, target, true, 0f);
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(layerIndex, target, true);
            }

            targetAnimation = target;
        }

        /// <summary>
        /// Play a non-looping animation once then continue playing the state animation.
        /// Can lock state changing.
        /// </summary>
        public void PlayOneShot(string shortNameHash, bool lockStateChanging, int trackIndex = 0)
        {
            Animation oneShotAnimation = GetAnimationForState(shortNameHash);

            if (oneShotAnimation == null)
                return;

            AnimationState state = skeletonAnimation.AnimationState;
            state.SetAnimation(trackIndex, oneShotAnimation, false);
            state.AddEmptyAnimation(trackIndex, 0, oneShotAnimation.Duration);

            // Animation transition = TryGetTransition(oneShotAnimation, targetAnimation);
            // if (transition != null)
            //     state.AddAnimation(trackIndex, transition, false, 0f);
            //
            // state.AddAnimation(trackIndex, targetAnimation, true, 0f);

            if (lockStateChanging == false)
                return;

            if (sateLockRoutine != null)
                StopCoroutine(sateLockRoutine);

            sateLockRoutine = StartCoroutine(AnimationStateChangingCooldown(oneShotAnimation.Duration));
        }

        private void PlayAnimationForState(int shortNameHash, int layerIndex)
        {
            if (isStateChangingLocked)
                return;

            Animation foundAnimation = GetAnimationForState(shortNameHash);
            if (foundAnimation == null)
                return;

            PlayNewAnimation(foundAnimation, layerIndex);
        }

        private Animation TryGetTransition(Animation from, Animation to)
        {
            foreach (AnimationTransition transition in transitions)
                if (transition.from.Animation == from && transition.to.Animation == to)
                    return transition.transition.Animation;

            return null;
        }

        private Animation GetCurrentAnimation(int layerIndex)
        {
            TrackEntry currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
            return currentTrackEntry?.Animation;
        }

        private int StringToHash(string s) =>
            Animator.StringToHash(s);

        private IEnumerator AnimationStateChangingCooldown(float duration)
        {
            isStateChangingLocked = true;

            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            isStateChangingLocked = false;
            PlayAnimationForState(StringToHash(lastInvokedAnimationName), 0);
        }

        //TODO: klavikus: public->private
        [Serializable]
        private class StateNameToAnimationReference
        {
            public string stateName;
            public AnimationReferenceAsset animation;

            public void Initialize()
            {
                animation.Initialize();
            }
        }

        //TODO: klavikus: public->private
        [Serializable]
        private class AnimationTransition
        {
            public AnimationReferenceAsset from;
            public AnimationReferenceAsset to;
            public AnimationReferenceAsset transition;

            public void Initialize()
            {
                from.Initialize();
                to.Initialize();
                transition.Initialize();
            }
        }
    }
}