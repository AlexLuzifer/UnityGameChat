using UnityEngine;
using UnityEngine.UI;

namespace GameChat
{
    public class Message : MonoBehaviour
    {
        private Text messageText;

        private void Start() => GetMessageText();

        public void SetDataMessage(string message, Color color = new Color())
        {
            if (messageText == null) GetMessageText();
            messageText.text = message;

            if (color != new Color()) messageText.color = color;
        }

        private void GetMessageText() => messageText = transform.GetChild(0).GetComponent<Text>();
    }
}