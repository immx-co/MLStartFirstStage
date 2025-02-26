### Основные приложения:<br><br> ThirdStage - клиент;<br><br> SignalRApplication - сервер, который обрабатывает запросы от клиента (должен быть развернут на отдельном сервере, а не как реализовано сейчас в учебных целях - у клиента);<br><br> EmailConfirmationApp - сервер, который обрабатывает входные запросы от клиента и верифицирует аккаунт (По аналогии с SignalRApplication должен быть развернут на отдельном сервере);<br><br> neural_net_service - нейросетевой Python сервис.

### Описание проектов

ThirdStage - клиент. Взаимодействует с SignalRApplication посредством SignalR хабов и делегирует всю бизнес логику на SignalRApplication сервис.

SignalRApplication - сервис, который обрабатывает запросы от ThirdStage - клиента.

EmailConfirmationApp - сервис, который обрабатывает запрос от клиента - верификацию аккаунта. Чтобы верифицировать свой аккаунт и иметь полный доступ к neural_net_service нужно зарегистрироваться с действующей почтой (желательно mail.ru или yandex.ru) и перейти по ссылке, которая отправится письмом при успешной регистрации пользователя.

neural_net_service - нейросетевой Python сервис, который в теории должен выполнять обработку изображений (инференс) нейросетевыми моделями.

### Конфигурация ThirdStage

Конфигурация проекта ThirdStage выглядит следующим образом:

```
{
    "N": "8", 
    "L": "5",
    "ConnectionStrings": {
        "stringConnection": "Host=localhost;Port=5432;Database=avaloniadb6;Username=postgres;Password=default"
    },
}
```

ConnectionStrings.stringConnection указывается свой.

Конфигурация хранится в папке bin\Debug\net8.0

### Конфигурация SignalRApplication

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "stringConnection": "Host=localhost;Port=5432;Database=avaloniadb6;Username=postgres;Password=default"
  },
  "SmtpSettings": {
    "Server": "smtp.yandex.ru",
    "Port": 587,
    "Username": "immxxx@yandex.ru",
    "Password": "default",
    "EnableSsl": true
  }
}
```

Где SmtpSettings.Password нужно запросить у меня, либо при желании настроить свою почту (секцию SmtpSettings) с поддержкой Smtp.

### Конфигурация EmailConfirmationApp

Конфигурация проекта EmailConfirmationApp выглядит следующим образом:

```
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:7575"
      },
      "Https": {
        "Url": "https://localhost:7576"
      }
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "stringConnection": "Host=localhost;Port=5432;Database=avaloniadb6;Username=postgres;Password=default"
  }
}
```

Где ConnectionStrings.stringConnection должна соответствовать ConnectionStrings.stringConnection из проекта ThirdStage и SignalRApplication.

Конфигурация хранится в EmailConfirmationApp.

### Использование

Запустить 3 проекта одновременно из Visual Studio: пкм -> Debug -> Start New Instance по каждому из трех проектов.

Проект ThirdStage также взаимодействует с нейросетевым сервисом, написанным на Python. Для успешного обращения к нейросетевому сервису из третьего главного окна приложения следует:

1. Развернуть нейросетевой Python сервис;
2. Прописать в TextBox на третьем главном окне приложения адрес нейросетевого сервиса (например http://localhost:8000, если нейросетевой сервис развернут на host - localhost, port - 8000).

### Как запустить нейросетевой сервис?

Запустить нейросетевой сервис (neural_net_service) можно из vscode. Достаточно будет открыть проект в vscode и запустить в дебаге "Python Debugger: Service API". Нейросетевой сервис развернется на http://localhost:8000

# TODO

1. Вынести порт и хост SignalRApplication в конфиг клиенту (ThirdStage);
2. Http.Url EmailConfirmationApp проекта пробросить в конфиг проекта ThirdStage;
3. Адрес нейросетевого сервиса пробросить в конфиг проекта ThirdStage. 

# Patch Notes

### 0.0.1:

- Частично перенесена логика работы запросов от клиента в SignalRApplication сервис.