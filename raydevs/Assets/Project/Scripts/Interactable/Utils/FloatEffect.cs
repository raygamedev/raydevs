namespace Raydevs.Utils
{
    using UnityEngine;

    public class FloatEffect : MonoBehaviour
    {
        private const float Amplitude = 0.05f;
        public const float Frequency = 2f;
        private float _originalY;

        private void Start() => _originalY = transform.position.y;

        private void Update() =>
            transform.position = new Vector3(transform.position.x,
                _originalY + Mathf.Sin(Time.time * Frequency) * Amplitude, transform.position.z);
    }
}