using DynamicBox.EventManagement;
using DynamicBox.Interactables;
using UnityEngine;

namespace DynamicBox.Player
{
    public class PlayerDoorHandler : MonoBehaviour
    {
        [SerializeField] private string keyID = "";
        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Door":
                    break;

                case "Key":
                    Key key = other.GetComponent<Key>();
                    PickUpKey(key);
                    break;
            }
        }

        //private void CheckDoor(Door door)
        //{
        //    if (door.IsDoorUnlocked)
        //    {
        //        door.LoadNextLevel();
        //    }
        //    else
        //    {
        //        if (door.CheckDoorKeyID(keyID))
        //        {
        //            Debug.Log("Door unlocked");
        //        }
        //    }
        //}

        private void PickUpKey(Key key)
        {
            keyID = key.KeyID;

            Destroy(key.gameObject);
        }

        #endregion
    }
}


