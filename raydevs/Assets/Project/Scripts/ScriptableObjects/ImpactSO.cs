using UnityEngine;

namespace Raydevs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Impact", menuName = "Scriptable Objects/Impact", order = 0)]
    public class ImpactSO : ScriptableObject
    {
        [field: SerializeField] public GameObject ImpactVFX { get; private set; }

        public void PlayImpactVFX(Vector3 position)
        {
            Instantiate(ImpactVFX, position, Quaternion.identity);
        }
    }
}