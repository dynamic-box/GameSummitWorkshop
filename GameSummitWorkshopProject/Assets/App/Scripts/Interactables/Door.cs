using DynamicBox.EventManagement;
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

        #endregion

        #region Event Handlers

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