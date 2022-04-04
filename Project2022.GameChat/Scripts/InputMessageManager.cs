using UnityEngine.UI;
using UnityEngine;

namespace GameChat
{
    public class InputMessageManager : MessengerPart
    {
        [SerializeField]
        private string namePlayer = "AlexLuzifer";

        private Messenger messenger;
        private InputField inputField;

        private void Start() => GetAllVars();

        public new void SendMessage(string message)
        {
            if (message == "" || message == null)
            {
                CloseInputMessage();
                return;
            }

            if (messenger == null) GetMessenger();
            messenger.AddMessage(namePlayer + ": " + message);

            inputField.text = "";
            CloseInputMessage();
        }

        public void OpenInputMessage() => VisibleInsputMessage(true);
        public void CloseInputMessage() => VisibleInsputMessage(false);

        private void VisibleInsputMessage(bool set)
        {

            if (inputField == null) GetInputField();
            inputField.interactable = set;

            if (set)
            {
                OpenMessengerPart();
                inputField.ActivateInputField();
            }
            else CloseMessengerPart();
        }

        private void GetAllVars()
        {
            GetInputField();
            GetMessenger();
        }

        private void GetInputField() => inputField = transform.GetChild(0).GetComponent<InputField>();
        private void GetMessenger() => messenger = transform.parent.GetComponent<Messenger>();
    }
}