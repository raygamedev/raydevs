namespace Raydevs
{
    using UnityEngine;

    public abstract class InteractableBase : MonoBehaviour
    {
        public abstract void OnInteractEnter();

        public abstract void OnInteractExit();
    }
}