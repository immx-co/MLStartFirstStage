# Шаблон сервиса FastAPI  

Данный проект представляет собой шаблон сервиса FastAPI, который принимает изображение, выполняет его ресайз и отдает информацию о размерности преобразованного изображения

На верхнем уровне данный проект состоит из следующих компонентов:

* `src` - реализует логику работы сервиса
* `.env` - хранит значение пути, являющегося стартовой точкой для импорта внутренних модулей
* `pyproject.toml` -- хранит настройки для Python-инструментов (ruff, pytest, mypy)
* `requirements.txt` -- хранит необходимые для проекта зависимости

## Установка зависимостей  

1. Установка Python с официального сайта (<https://www.python.org/downloads/>) либо с MS Store:
2. Создание виртуального окружения с помощью `venv`. Для этого в корневой папке с сервисом нужно открыть терминал и написать следующую команду:
   `python -m venv <name_of_env>`
3. Активация виртуального окружения: `<name_of_env>\Scripts\activate.bat` (cmd), `<name_of_env>\Scripts\activate.ps1` (powershell):  
   ![activated_fastapi_env](./readme_images/activated_fastapi_env.jpg)
4. Установка зависимостей для сервиса: `pip install -r requirements.txt`
   ![installed_dependencies](./readme_images/installed_dependencies.jpg)  

## Запуск сервиса

В случае успешного выполнения пункта по установке зависимостей можно приступать к тестированию сервиса  
Чтобы его запустить, необходимо в терминале, открытом по корневой папке, написать следующую команду:

```powershell
(ml_course_env) PS E:\Repositories\fastapi_service_example> uvicorn src.service:app --host localhost --port 8000 --log-config=log_config.yaml
```

В случае успешного запуска в терминале появится следующий лог:

```cmd
INFO     2024-10-08 17:42:48,180 - fastapi - INFO - Логи работы сервиса не сохраняются                                                                logging_tools.py:85
INFO     2024-10-08 17:42:48,668 - fastapi - INFO - Конфигурация сервиса: src\configs\service_config.json                                                    router.py:24
INFO     2024-10-08 17:42:48,674 - uvicorn.error - INFO - Started server process [30196]                                                                     server.py:77
INFO     2024-10-08 17:42:48,676 - uvicorn.error - INFO - Waiting for application startup.                                                                       on.py:48
INFO     2024-10-08 17:42:48,677 - uvicorn.error - INFO - Application startup complete.                                                                          on.py:62
INFO     2024-10-08 17:42:48,683 - uvicorn.error - INFO - Uvicorn running on http://localhost:8000 (Press CTRL+C to quit)                                   server.py:209
```

## Тестирование сервиса

### Тестирование с помощью простого Python-клиента, использующего requests

Для тестирования сервиса с помощью Python-клиента необходимо в терминале ввести следующую команду:

```powershell
(ml_course_env) PS E:\Repositories\fastapi_service_example_with_routers> python -m src.client
```

После запуска данной команды в логах консоли клиента должно появиться следующее:

```powershell
{'width': 1920, 'height': 1080, 'channels': 3}
```

В логах консоли сервиса должно появиться следующее:

```powershell
INFO     2024-10-08 17:53:14,409 - fastapi - INFO - Принята картинка размерности: (480, 640, 3)                                                              router.py:50
INFO     2024-10-08 17:53:14,421 - fastapi - INFO - Выполнен ресайз картинки до целевой размерности (1080, 1920, 3)                                          router.py:57
INFO     2024-10-08 17:53:14,423 - uvicorn.access - INFO - 127.0.0.1:55684 - "POST /resize_image HTTP/1.1" 200                                      httptools_impl.py:484
```

### Тестирование с помощью Swagger UI

Для тестирования сервиса с помощью Swagger UI нужно перейти по ссылке `http://localhost:8000/docs`:
![swagger_ui](./readme_images/swagger_ui.jpg)

Затем нужно раскрыть секцию `default` и нажать `Try it out`:  
![try_it_out](./readme_images/try_it_out.png)

Далее нужно выбрать нужное изображение и нажать `Execute`
![execute](./readme_images/execute.png)  

Если все сделано верно, то сервис примет изображение, отресайзит его и выдаст JSON о размерности отресайзнутого изображения  
![successfull_response](./readme_images/successfull_response.png)
