using NaughtyAttributes;
using UnityEngine;

namespace Pelki.Gameplay.Characters.Animations
{
    public class CharacterAnimatorSwitcher : MonoBehaviour
    {
        private static readonly int idle = Animator.StringToHash("Idle");
        private static readonly int move = Animator.StringToHash("Move");
        private static readonly int moveSpeed = Animator.StringToHash("MoveSpeed");

        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private Animator animator;
        [Header("Debug")]
        [SerializeField] private bool isDebugLog;

        [Button("Debug" + nameof(DoIdle))]
        public void DoIdle()
        {
            animator.ResetTrigger(move);
            animator.SetTrigger(idle);
            ThisDebugLog(nameof(DoIdle));
        }

        [Button("Debug" + nameof(DoMove))]
        public void DoMove()
        {
            animator.ResetTrigger(idle);
            animator.SetTrigger(move);
            ThisDebugLog(nameof(DoMove));
        }

        public void ChangeStateCarryObjects(bool isCarryObject)
        {
            ThisDebugLog($"{nameof(ChangeStateCarryObjects)} ({isCarryObject})");
        }

        public void SetMoveSpeed(float speedFactor)
        {
            animator.SetFloat(moveSpeed, speedFactor * maxMoveSpeed);
            ThisDebugLog($"{nameof(SetMoveSpeed)} ({speedFactor})");
        }

        [Button()]
        private void DebugSetZeroSpeedMove()
        {
            SetMoveSpeed(0);
        }

        [Button()]
        private void DebugSetMaxSpeedMove()
        {
            SetMoveSpeed(maxMoveSpeed);
        }

        [Button("Auto Fill")]
        private void DebugAutoFill()
        {
            animator = GetComponentInChildren<Animator>();
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private void ThisDebugLog(string log)
        {
            if (isDebugLog)
            {
                Debug.Log(log, this);
            }
        }
    }
}