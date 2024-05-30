using DynamicBox.EventManagement;
using DynamicBox.Events;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DynamicBox.Interactables
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private string doorKeyID;
        [SerializeField] private Collider doorCollider;
        [SerializeField] private Animation doorOpenAnimation;

        public string DoorKeyID
        {
            get => doorKeyID;
        }
        public bool IsDoorUnlocked { get => isDoorUnlocked; }

        [SerializeField] private bool isDoorUnlocked;

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.Instance.AddListener<OnEnteredDoorTriggerEvent>(OnEnteredDoorTriggerHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<OnEnteredDoorTriggerEvent>(OnEnteredDoorTriggerHandler);
        }

        #endregion

        #region Event Handlers

        private void OnEnteredDoorTriggerHandler(OnEnteredDoorTriggerEvent eventDetails)
        {
            CheckDoorKeyID(eventDetails.KeyID);
        }

        #endregion

        #region Custom Methods


        public bool CheckDoorKeyID(string keyID)
        {
            if (keyID == doorKeyID)
            {
                isDoorUnlocked = true;
                LoadNextLevel();
                return true;
            }

            return false;
        }

        public async void LoadNextLevel()
        {
            doorCollider.enabled = false;
            doorOpenAnimation.Play();
            await Task.Delay(2000);

            Game.NextLevel();
        }

        #endregion
    }
}