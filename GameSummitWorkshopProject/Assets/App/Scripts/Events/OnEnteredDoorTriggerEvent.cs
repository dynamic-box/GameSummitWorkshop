using DynamicBox.EventManagement;

namespace DynamicBox.Events
{
    public class OnEnteredDoorTriggerEvent : GameEvent
    {
        public readonly string KeyID;

        public OnEnteredDoorTriggerEvent(string keyID)
        {
            KeyID = keyID;
        }
    }
}