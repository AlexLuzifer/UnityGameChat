using UnityEngine;

namespace GameChat
{
    public class MessengerPart : MonoBehaviour
    {
        private float interpolator = 0f;

        [SerializeField]
        private float endPositionX = -650f;

        private bool isShow = false; 
        private bool isPlayed = false;

        private RectTransform rectTransform;

        private void Awake()
        {
            GetRectTransform();

            isShow = false;
            isPlayed = false;
            rectTransform.anchoredPosition = new Vector2(endPositionX, 0f);
        }

        protected virtual void Update() => AnimationPlay();


        private void AnimationPlay()
        {
            if (isPlayed)
            {
                interpolator += Time.deltaTime;

                float positionX = rectTransform.anchoredPosition.x;
                positionX = Mathf.Lerp(positionX, isShow ? 0f : endPositionX, interpolator);

                if ((isShow && positionX > -1f) || (!isShow && positionX < endPositionX + 1))
                {
                    positionX = isShow ? 0f : endPositionX;
                    SetPlayed(false);
                }

                rectTransform.anchoredPosition = new Vector2(positionX, 0f);
            }
            else
            {
                float positionX = rectTransform.anchoredPosition.x;

                if (isShow && positionX != 0f) rectTransform.anchoredPosition = new Vector2(0f, 0f);
                else if (!isShow && positionX != endPositionX) rectTransform.anchoredPosition = new Vector2(endPositionX, 0f);
            }
        }


        public void OpenMessengerPart() => SetVisibleMessengerPart(true);
        public void CloseMessengerPart() => SetVisibleMessengerPart(false);

        private void SetVisibleMessengerPart(bool set)
        {
            if (rectTransform == null) GetRectTransform();
            isShow = set;
            SetPlayed(true);
        }

        private void SetPlayed(bool set)
        {
            isPlayed = set;
            interpolator = 0f;
        }

        private void GetRectTransform() => rectTransform = GetComponent<RectTransform>();
    }
}