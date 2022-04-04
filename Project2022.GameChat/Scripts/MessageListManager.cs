using UnityEngine;

namespace GameChat
{
    public class MessageListManager : MessengerPart
    {
        [Range(3f, 120f)]
        [SerializeField]
        private float lifeTimerMax = 5f;

        [SerializeField]
        private int limitMessages = 16;

        [SerializeField]
        private GameObject messagePrefab;

        private float lifeTimer = 5f;

        private bool isShowMessageList = false;

        private Color defaultColor = new Color(1f, 1f, 1f, 1f);

        protected override void Update()
        {
            base.Update();
            if (isShowMessageList) TimerShow();
        }


        public void AddMessage(string message, Color color = new Color())
        {
            if (message == "" || message == null) return;

            if (!isShowMessageList) OpenMessageList();
            lifeTimer = lifeTimerMax;

            if (transform.childCount >= limitMessages) Destroy(transform.GetChild(0).gameObject);

            GameObject newMessage = Instantiate(messagePrefab);
            newMessage.transform.SetParent(transform, false);

            newMessage.GetComponent<Message>().SetDataMessage(message, color);
        }

        public void OpenMessageList() => VisibleMessageList(true);
        public void CloseMessageList() => VisibleMessageList(false);

        private void VisibleMessageList(bool set)
        {
            isShowMessageList = set;
            lifeTimer = lifeTimerMax;

            if (set) OpenMessengerPart();
            else CloseMessengerPart();
        }

        private void TimerShow()
        {
            if (lifeTimer > 0f) lifeTimer -= Time.deltaTime;
            else CloseMessageList();
        }
    }
}