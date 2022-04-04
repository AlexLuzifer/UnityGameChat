# Unity Game Chat Client v1.0

# Краткое описание.
При нажатии на кнопку **Y** появляется чат с фокусом на поле ввода.
После ввода сообщения и отправки его путем нажатия на Enter(или любым другим способом, которое запустит событие OnSubmit у поля ввода), сообщение добавится в чат.
Отправляя пустое сообщение, поле ввода скрывается без добавления пустого сообщения в чат.
Сообщения скрываются по истечению определенного времени(по дефолту: 5 секунд).
При добавлении нового сообщения Окно с сообщениями появляется если было скрыто и сбрасывается таймер показа.

P.S. Логики отправки на сервер нет. Необходимо добавлять свой метод в событие поля ввода "OnSubmit".
P.S. Для добавления сообщения полученного с сервера, необходимо получить объект **Messenger** и вызвать метод **AddMessage(string message)**.

## Содержимое проекта

- Prefabs/
- - Message.prefab // Префаб самого сообщения
- - Messenger.prefab // Префаб всего чата

- Scenes/
- - Start.scene // Стартовая сцена для демонстрации

- Scripts/
- - InputMessageManager.cs // Менеджер ввода сообщения
- - Message.cs // Сообщение. Для записи полученного сообщения в компонент Текст
- - MessageListManager.cs // Менеджер сообщений
- - Messenger.cs // Главный файл чата. Отлавливает нажатия
- - MessengerPart.cs // Используется для анимации появления/скрывания частей чата(Чат-лист, менеджер ввода)

## Строение в инспекторе (в Canvas)

- Messenger
- - MessageList
- - - Message
- - - - Text
- - InputMessage
- - - InputField(Стандартный)

По нажатию на клавишу **Y** открывается **Чат-лист** и **Менеджер ввода**.
Чат-лист имеет определенное время показа, по истечению которого скрывается.
Настроить время можно в объекте Чат-листа.
// On Hierarchy: Canvas -> Messenger -> MessageList
// On Inpector: MessageListManager -> Life Timer Max
// Life Timer Max имеет диапозон от 3 до 10.

Менеджер ввода не скроется до тех пор, пока не будет отправлено сообщение или не скрыт принудительно.

По нажатию сочетания клавиш **Ctrl + Y** Чат закроется принудительно.

Анимации появления/исчезания работают через Lerp. В классе **MessengerPart**.

**Отправка сообщений**
При нажатии на Enter, срабатывает событие "OnSubmit" в InputField
Если сообщение было пустое, то поле ввода скроется без каких-либо других действий.
Если в сообщении было

##Full-Info
Чат-клиент (далее, Ассет, Проект, Чат), состоит из двух частей(содержит два объекта): **MessageList**(далее, messageList, Chat-list, Чат-лист) и **InputMessage** (далее, Менеджер ввода).
Чат-лист хранит в себе список сообщений.
Менеджер ввода хранит в себе поле ввода.

**Messenger**
Объект Image. Компонент отключен.
Растянут по левому краю. Отступ по X - 15. Ширина - 600, отступ снизу 15.

**MessageList**
RectTransform. Растянут по верху.
Изначально перемещен на -670 по Х.
Компонент Image отключен.
Сообщения сортируются с помощью Vertical Layout Group:
Spacing: 5
Child Alignment: Lower Center
Child Force Expand: width-true

**Сообщение**
Сообщение это обычный Image содержащий в себе объект Text.
Image является background'ом сообщения.
Image изменен только цвет: Color(0f, 0f, 0f, 0.5f)
Ширина: 600, Высота: 55.
**P.S. Редактируя высоту, необходимо проверять количество сообщений которые вместятся в "высоту экрана - высота поля ввода".**

**Поле ввода**
Поле ввода растянуто на весь объект InputMessage.
Image стандартный.
В зависимости от того закрыт чат или нет активируется/дезактивируется Interactable.
Все состояния (hover, pressed) одинаковые по цветам - Color(0f, 0f, 0f, 0.5f) за исключением normal - Color(0f, 0f, 0f, 0.1f)
Лимит символов (Character Limit): 80
LineType: Multi Line Submit - нам необходимо писать в несколько строк, но по нажатию Enter отправлять сообщение.
OnSubmit - InputMessenger.SendMessage(string message);



## Scripts

**Messenger.cs**
```
# Объявляем переменные двух классов для чат-листа и Менеджера поля ввода

private MessageListManager messageList;
private InputMessageManager inputMessage;


# При загрузке сцены сразу же определим эти объекты

private void Awake()
{
    GetMessageListManager();
    GetInputMessage();
}

private void Update()
{
    # Отлов нажатий нужных клавиш

    if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl)) CloseMessenger();
    else if (Input.GetKeyDown(KeyCode.Y)) OpenMessenger();
}

# Публичный метод для добавления сообщения в чат-лист. Вызываем его когда срабатывает OnSubmit в поле ввода. Его же вызываем когда получили сообщение с сервера.

public void AddMessage(string message, Color color = new Color()) => messageList.AddMessage(message, color);


# Открыть мессенджер - открыть чат-лист и поле ввода вместе

private void OpenMessenger()
{
    messageList.OpenMessageList();
    inputMessage.OpenInputMessage();
}

# Закрыть мессенджер - закрыть чат-лист и поле ввода вместе

private void CloseMessenger()
{
    messageList.CloseMessageList();
    inputMessage.CloseInputMessage();
}


# Два метода для получения компонентов у дочерних объектов

private void GetMessageListManager() => messageList = transform.GetChild(0).GetComponent<MessageListManager>();
private void GetInputMessage() => inputMessage = transform.GetChild(1).GetComponent<InputMessageManager>();
```

**InputMessageManager.cs**

```
# Наследуем от MessengerPart для анимации открывания/закрывания поля ввода
public class InputMessageManager : MessengerPart
{

# Имя игрока если ответ с сервера по типу (string namePlayer, string message)
[SerializeField]
private string namePlayer = "AlexLuzifer";

# Наш главный мессенджер для добавления нового сообщения и поле ввода для обнуления записи после отправки и активации фокуса.
private Messenger messenger;
private InputField inputField;

# При старте получаем компоненты чата и поля ввода в переменные
private void Start() => GetComponents();


# Публичный метод отправки сообщения. Срабатывает по событию OnSubmit у поля ввода
public new void SendMessage(string message)
{

# Если сообщение пустое, то закрываем поле ввода и возвращаем метод.
    if (message == "" || message == null)
    {
        CloseInputMessage();
        return;
    }

# Если мессенджер не получен по каким-либо причинам, то получаем и запускаем метод AddMessage с параметрами: ИмяИгрока + разделитель + сообщение
    if (messenger == null) GetMessenger();
    messenger.AddMessage(namePlayer + ": " + message);

# Стираем старый текст и закрываем поле ввода
    inputField.text = "";
    CloseInputMessage();
}


# Два публичных метода для закрытия/открытия поля ввода
public void OpenInputMessage() => VisibleInsputMessage(true);
public void CloseInputMessage() => VisibleInsputMessage(false);


# Приватный метод изменения видимости поля ввода
private void VisibleInsputMessage(bool set)
{

# Проверка на существование поля ввода, задаем значение для Interactable равное переданому значению set
    if (inputField == null) GetInputField();
    inputField.interactable = set;

# Так как мы наследовали класс MessengerPart, то вызываем метод открытия поля ввода и активируем фокус на нем. Иначе - закрываем.
    if (set)
    {
        OpenMessengerPart();
        inputField.ActivateInputField();
    }
    else CloseMessengerPart();
}

# Получение всех компонентов с которыми будем работать
private void GetComponents()
{
    GetInputField();
    GetMessenger();
}

private void GetInputField() => inputField = transform.GetChild(0).GetComponent<InputField>();
private void GetMessenger() => messenger = transform.parent.GetComponent<Messenger>();
}
```

**MessageListManager.cs**

```
# Чат-лист тоже наследуем от MessengerPart.
public class MessageListManager : MessengerPart
{

# Время показа чат-листа
[Range(3f, 120f)]
[SerializeField]
private float lifeTimerMax = 5f;


# Лимит количества сообщений. Будет больше - все новые будут внизу за пределами экрана и перекроют поле ввода.
[SerializeField]
private int limitMessages = 16;

# Префаб сообщения для спавна новых сообщений
[SerializeField]
private GameObject messagePrefab;

# Таймер для отчета показа
private float lifeTimer = 5f;

# Блокер запуска таймера. | Unity ругается на нэйминг isShow из-за наследования
private bool isShowMessageList = false;

# Update используется в MessengerPart, поэтому базируемся на уже существующем.
protected override void Update()
{
    base.Update();

# Запускаем таймер если чат-лист показывается
    if (isShowMessageList) TimerShow();
}


# Публичный метод для добавления сообщения в чат-лист. Второй параметр необязательный, но можно использовать для изменения цвета сообщения.
public void AddMessage(string message, Color color = new Color())
{

# Проверка на пустое сообщение
    if (message == "" || message == null) return;

# Проверка на видимость чат-листа. Если закрыт, то открываем. 
    if (!isShowMessageList) OpenMessageList();

# Сбрасываем таймер для прочтения нового сообщения
    lifeTimer = lifeTimerMax;

# Проверяем количество сообщений. Если достигает лимита - удаляем старое сообщение, то есть самое первое дочернее.
    if (transform.childCount >= limitMessages) Destroy(transform.GetChild(0).gameObject);

# Создаем новый экземпляр сообщения, задаем ему родителя - Чат-лист и задаем ему сообщение с цветом.
    GameObject newMessage = Instantiate(messagePrefab);
    newMessage.transform.SetParent(transform, false);

    newMessage.GetComponent<Message>().SetDataMessage(message, color);
}


# Публичные методы - Открыть/Закрыть чат-лист
public void OpenMessageList() => VisibleMessageList(true);
public void CloseMessageList() => VisibleMessageList(false);

# Задает видимость чата
private void VisibleMessageList(bool set)
{
    isShowMessageList = set;
    lifeTimer = lifeTimerMax;

    if (set) OpenMessengerPart();
    else CloseMessengerPart();
}

# Таймер для закрытия чат-листа
private void TimerShow()
{
    if (lifeTimer > 0f) lifeTimer -= Time.deltaTime;
    else CloseMessageList();
}
}
```


**MessengerPart.cs**

Так как чат-лист и поле ввода являются разными частями мессенджера, но при этом выполняют одинаковую логику анимации, то был создан этот класс.

```
# Интерполятор для Lerp
private float interpolator = 0f;

# Насколько сдвигать часть мессенджера влево когда выключен
[SerializeField]
private float endPositionX = -650f;

# Определитель показывается ли часть или нет
private bool isShow = false; 

# Определяет проигрывается ли анимация
private bool isPlayed = false;

# Для сдвига берем RectTransform
private RectTransform rectTransform;

# Во время запуска получаем RectTransform и сдвигает влево, так как по умолчанию чат выключен. 
private void Awake()
{
    GetRectTransform();
    rectTransform.anchoredPosition = new Vector2(endPositionX, 0f);
}

# Запускаем метод проигрывания анимации
protected virtual void Update() => AnimationPlay();

# Если анимация играет, то увеличиваем интерполятор и с помощью Lerp'а двигаем нашу часть, иначе проверяем не остановилась ли наша часть на половине пути
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

# Публичные методы Открыть/Закрыть часть
public void OpenMessengerPart() => SetVisibleMessengerPart(true);
public void CloseMessengerPart() => SetVisibleMessengerPart(false);

# Задает значение открыть или закрыть часть
private void SetVisibleMessengerPart(bool set)
{
    if (rectTransform == null) GetRectTransform();
    isShow = set;
    SetPlayed(true);
}

# Задаем статус проигрывания анимации
private void SetPlayed(bool set)
{
    isPlayed = set;
    interpolater = 0f;
}

# Отдельный метод для получения rectTransform
private void GetRectTransform() => rectTransform = GetComponent<RectTransform>();
```


**Message**
```
# Объявляем переменную компонента Текст
private Text messageText;

# В старте получаем в переменную компонент Текста
private void Start() => GetMessageText();

# Публичный метод, к которому обращается Чат-лист при создании нового сообщения, задает текст сообщения и меняет цвет если был новый
public void SetDataMessage(string message, Color color = new Color())
{
    if (messageText == null) GetMessageText();
    messageText.text = message;

    if (color != new Color()) messageText.color = color;
}

# Отдельный метод для получения компонента Текст.
private void GetMessageText() => messageText = transform.GetChild(0).GetComponent<Text>();
```