using System;
using Counters;
using UnityEngine;

namespace AnimationObject
{
    public class ContainerCounterVisual : MonoBehaviour
    {
        private const string OPEN_CLOSE = "OpenClose";
        [SerializeField] private ContainerCounter containerCounter;
        private UnityEngine.Animator animator;

        private void Awake() {
            animator = GetComponent<UnityEngine.Animator>();
        }

        private void Start() {
            containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
        }
    
        private void ContainerCounter_OnPlayerGrabbedObject(object sender, EventArgs e) {
            animator.SetTrigger(OPEN_CLOSE);
        }
    }
}
