
using System;

namespace Raydevs
{
    using UnityEngine;
    public class Interactable: MonoBehaviour
    {
        public bool IsCollectable { get; set; }
        public bool HasMessageBox { get; set; }
        public virtual void Interact()
        {
        }

        public virtual void Interact(GameObject ray)
        {
            
        }
        public virtual void OnInteractLeave()
        {
        }
    }
}