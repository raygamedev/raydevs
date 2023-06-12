
namespace Raydevs.UI.MiniUI
{
    using UnityEngine;
    public class TutorialKeys : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag($"Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}