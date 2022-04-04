using UnityEngine;

namespace GameChat
{
    public class Messenger : MonoBehaviour
    {
        private MessageListManager messageList;
        private InputMessageManager inputMessage;

        private void Awake()
        {
            GetMessageListManager();
            GetInputMessage();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl)) CloseMessenger();
            else if (Input.GetKeyDown(KeyCode.Y)) OpenMessenger();
        }

        public void AddMessage(string message, Color color = new Color()) => messageList.AddMessage(message, color);

        private void OpenMessenger()
        {
            messageList.OpenMessageList();
            inputMessage.OpenInputMessage();
        }

        private void CloseMessenger()
        {
            messageList.CloseMessageList();
            inputMessage.CloseInputMessage();
        }

        private void GetMessageListManager() => messageList = transform.GetChild(0).GetComponent<MessageListManager>();
        private void GetInputMessage() => inputMessage = transform.GetChild(1).GetComponent<InputMessageManager>();
    }
}