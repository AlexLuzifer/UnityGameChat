using UnityEngine;

namespace GameChat
{
    public class MessengerPart : MonoBehaviour
    {
        // Для интерполяции
        private float interpolater = 0f;

        [SerializeField]
        private float endPositionX = -650f; // Конечная позиция для скрытия

        private bool isShow = false; // Скрыта ли часть мессенджера 
        private bool isPlayed = false; // Проигрывается ли анимация скрытия/открытия

        private RectTransform rectTransform; // Для анимации сдвига

        private void Awake()
        {
            GetRectTransform();

            isShow = false;
            isPlayed = false;
            rectTransform.anchoredPosition = new Vector3(endPositionX, 0f);
        }

        private void Start() => GetRectTransform();

        protected virtual void Update() => AnimationPlay();


        private void AnimationPlay()
        {
            if (isPlayed)
            {
                interpolater += Time.deltaTime;

                float positionX = rectTransform.anchoredPosition.x;
                positionX = Mathf.Lerp(positionX, isShow ? 0f : endPositionX, interpolater);

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


        public void OpenMessengerPart() => VisibleMessengerPart(true);
        public void CloseMessengerPart() => VisibleMessengerPart(false);

        private void VisibleMessengerPart(bool set)
        {
            if (rectTransform == null) GetRectTransform();
            isShow = set;
            SetPlayed(true);
        }

        private void SetPlayed(bool set)
        {
            isPlayed = set;
            interpolater = 0f;
        }

        private void GetRectTransform() => rectTransform = GetComponent<RectTransform>();
    }
}