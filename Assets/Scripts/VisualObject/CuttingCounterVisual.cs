using System;
using Counters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Animator
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        private const string CUT = "Cut";
        [SerializeField] private CuttingCounter cuttingCounter;
        private UnityEngine.Animator animator;

        private void Awake() {
            animator = GetComponent<UnityEngine.Animator>();
        }

        private void Start() {
            cuttingCounter.OnCut += CuttingCounterOnOnCut;
        }
    
        private void CuttingCounterOnOnCut(object sender, EventArgs e) {
            animator.SetTrigger(CUT);
        }
    }
}
