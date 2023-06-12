namespace Raydevs.VFX
{
    using UnityEngine;
    public class SwordImpactVFX: MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                Destroy(gameObject);
        }
    }
}