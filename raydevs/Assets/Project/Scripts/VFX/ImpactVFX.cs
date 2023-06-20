namespace Raydevs.VFX
{
    using System.Collections;
    using UnityEngine;

    public class ImpactVFX : MonoBehaviour
    {
        private Animator _animator;
        private Transform _transform;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = transform;
        }


        public virtual void PlayImpactVFX(Vector3 position)
        {
            _transform.position = position;
            gameObject.SetActive(true);
            _animator.Play("Impact", 0, 0);
            float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(DisableAfterAnimation(animationLength));
        }

        private IEnumerator DisableAfterAnimation(float time)
        {
            yield return new WaitForSeconds(time); // Wait for the length of the animation

            gameObject.SetActive(false); // Disable GameObject
        }
    }
}