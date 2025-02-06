### Основные приложения: ThirdStage - клиент, EmailConfirmationApp - сервер (должен быть развернут на отдельном сервере, а не как реализовано сейчас в учебных целях - у клиента), neural_net_service - нейросетевой Python сервис.

### Конфигурация ThirdStage

Конфигурация проекта ThirdStage выглядит следующим образом:

```
{
    "N": "8", 
    "L": "5",
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

Где SmtpSettings нужно запросить у меня, либо при желании настроить свою почту с поддержкой Smtp.

ConnectionStrings.stringConnection также указывается свой.

Конфигурация хранится в папке bin\Debug\net8.0

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

Где ConnectionStrings.stringConnection должна соответствовать ConnectionStrings.stringConnection из проекта ThirdStage.

Конфигурация хранится в EmailConfirmationApp.

### Использование

Запустить 2 проекта одновременно: пкм -> Debug -> Start New Instance по каждому из двух проектов.

Проект ThirdStage также взаимодействует с нейросетевым сервисом, написанным на Python. Для успешного обращения к нейросетевому сервису из третьего главного окна приложения следует:

1. Развернуть нейросетевой Python сервис;
2. Прописать в TextBox на третьем главном окне приложения адрес нейросетевого сервиса (например http://localhost:8000, если нейросетевой сервис развернут на host - localhost, port - 8000).

# TODO

1. Http.Url EmailConfirmationApp проекта пробросить в конфиг проекта ThirdStage;
2. Адрес нейросетевого сервиса пробросить в конфиг проекта ThirdStage. 