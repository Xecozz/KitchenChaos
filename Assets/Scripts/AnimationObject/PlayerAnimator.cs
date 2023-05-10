using UnityEngine;

namespace AnimationObject
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Player player;
        private const string IS_WALKING = "IsWalking";
        private UnityEngine.Animator animator;

        private void Awake() {
            animator = GetComponent<UnityEngine.Animator>();
        }

        private void Update() {
            animator.SetBool(IS_WALKING, player.IsWalking());
        }
    }
}