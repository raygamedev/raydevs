
namespace Raydevs
{
    using UnityEngine;
    public class BulletinBoard : Interactable
    {
        private BulletinBoard()
        {
            HasMessageBox = true;
        }
        
        [SerializeField] private bool _hasNewQuest = true;
        
        private GameObject _questionMark;
        private GameObject _msgBox;

        private void Awake()
        {
            _questionMark = transform.Find("QuestionMark").gameObject;
            _msgBox = transform.Find("MessageBox").gameObject;
        }

        private void Update()
        {
            if (!_questionMark.activeInHierarchy)
                _questionMark.SetActive(_hasNewQuest);
        }

        public override void Interact()
        {
            base.Interact();
            _msgBox.SetActive(true);
            if (!_hasNewQuest) return;
            _hasNewQuest = false;
            _questionMark.SetActive(false);
        }

        public override void OnInteractLeave()
        {
            base.OnInteractLeave();
            _msgBox.SetActive(false);
        }
    }
}
