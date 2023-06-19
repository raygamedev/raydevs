using System;

namespace Raydevs.VFX
{
    using UnityEngine;

    public class DestroyImpactVFX : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject,
                GetComponent<Animator>().GetCurrentAnimatorStateInfo(0)
                    .length); // Destroy the game object after the animation is done
        }
    }
}