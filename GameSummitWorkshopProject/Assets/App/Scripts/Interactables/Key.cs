using UnityEngine;

namespace DynamicBox.Interactables
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private string keyID;

        public string KeyID { get => keyID; }
    }
}

