using System;
using UnityEngine.Events;

namespace Raydevs
{
    using UnityEngine;

    public class BulletinBoard : InteractableBase
    {
        [SerializeField] private bool _hasNewQuest = true;
        [SerializeField] private GameObject _questionMark;
        [SerializeField] private GameObject _msgBox;


        private void Update()
        {
            if (!_questionMark.activeInHierarchy)
                _questionMark.SetActive(_hasNewQuest);
        }

        public override void OnInteractEnter()
        {
            Debug.Log("Interacted with bulletin board");
            _msgBox.SetActive(true);
            if (!_hasNewQuest) return;
            _hasNewQuest = false;
            _questionMark.SetActive(false);
        }

        public override void OnInteractExit()
        {
            _msgBox.SetActive(false);
        }
    }
}