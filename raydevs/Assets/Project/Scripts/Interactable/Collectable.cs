using UnityEngine;
using UnityEngine.Events;

namespace Raydevs
{
    public class Collectable : InteractableBase
    {
        [SerializeField] private UnityEvent<CollectableType> OnCollectableInteracted;
        [SerializeField] private CollectableType _collectableType;

        public override void OnInteractEnter()
        {
            OnCollectableInteracted.Invoke(_collectableType);
            Destroy(gameObject);
        }

        public override void OnInteractExit()
        {
        }
    }
}