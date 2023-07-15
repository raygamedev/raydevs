namespace Raydevs.Interactable
{
    using UnityEngine;
    using UnityEngine.Events;

    public class HealthProbe : MonoBehaviour
    {
        [SerializeField] private int _healthAmount;
        [SerializeField] private UnityEvent<int> OnHealthProbeTaken;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;

            OnHealthProbeTaken.Invoke(_healthAmount);
            Destroy(gameObject);
        }
    }
}