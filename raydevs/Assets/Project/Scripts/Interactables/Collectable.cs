using UnityEngine;

namespace Raydevs
{
    public class Collectable: Interactable
    {
        [SerializeField] private CollectableType _collectableType;
        private Collectable()
        {
            IsCollectable = true;
        }
        public override void Interact(GameObject ray)
        {
            base.Interact();
            ICollectable collectable = ray.GetComponent<ICollectable>();
            if(collectable != null)
                collectable.SetCollectableInteracted(_collectableType);
            else
                Debug.LogError("No ICollectable component found on SwordCollectable");
                
            Destroy(gameObject);
        }

    }
}