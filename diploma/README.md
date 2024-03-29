 # Проектная работа

## Тема «Биржевая торговля с использованием микросервисной архитектуры»

## Теоретическая часть

Спроектировать архитектуру. Декомпозировать архитектуру на микросервисы.

![Микросервисная архитектура системы](img/arch.png)

### Описание архитектуры

#### 1. Типы информационных соединений

- синие - REST API,
- голубые - SignalR (на основе WebSocket),
- коричневые - HTTP-запросы проверки токенов аутентификации,
- желтые - передача сообщений через очереди брокера сообщений,
- зеленые - соединения сервисов и СУБД.

#### 2. Общие положения

Система настроена на работу в среде `Kubernetes`, позволяет снимать метрики при помощи сервисов `Prometheus` и просматривать/проверять их в `Grafana`.

Все разрабатываемые сервисы выполнены на платформе `.NET 6`, использованы сопутствующие технологии: `ASP.NET`, `SignalR`, `Blazor`, `Identity Server`, `Ocelot` и т.д.

Для хранения и обработки данных выбрана СУБД `PostgreSQL`. Данная СУБД универсальна и подходит для широкого спектра задач.

Брокер сообщений `Rabbit MQ`. `Rabbit MQ` прост в установке, настройке, его возможностей вполне достаточно для выполнения возложенных на него функций.

Кэш, распределенная память и stateful-решение `Redis`. Основная задача данной службы - поддержание stateful состояния постоянных соединений `SignalR`, а также распределенная память для синхронной коммуникации некольких экземпляров `Notification Service` при работе с общим пулом соединений пользователей.

#### 3. Описание микросервисов

__Frontend Platform Service__

Сервис позволяет пользователям запустить клиентскую часть системы в браузере. Работает на основе технологии `Blazor (Web Assemby)`. Реализует графический интерфейс, поддерживает коммуникацию с серверной частью системы на основе `SignalR` и `REST`.

__Gateway Service__

Точка входа серверного API. Проксирует запросы пользователей до нужных микросервисов, проверяет аутентификацию.

__Identity Service__

Сервис регистрации и аутентификации на основе IdentityServer4 - OpenID Connect, OAuth 2.0 фреймворк для ASP.NET. Отвечает за регистрацию, аутентификацию, разграничение доступа к сервисам.

__Account Service__

Сервис работы с профилями пользователей, в т.ч. с персональными данными.

__Order Service__

Сервис работы с заказами пользователей: покупки и продажи ценных бумаг.

__Billing Service__

Сервис обработки платежей: управление средствами на счете при покупке и продаже ценных бумаг, добавлении и выводе средств на счет.

__Notification Service__

Осуществляет уведомление пользователей о событиях в системе, инициирует и передает данные по рынку в реальном времени. 

__Fin. Signals Hub__

Хаб подключения к стороннему API для получения данных по рынку (сигналы), выполнения команд по покупке и продаже ценных бумаг на рынке.

### Практическая часть

#### 1. Установку приложения можно осуществить по команде

- Полная установка с настройкой Prometheus, Grafana и Ingress. В этом случае создается `namespace otus-services`, в котором должно находится приложение, т.к. на него настроен Gateway. Для установки нужно выполнить скрипт, находясь в директории `diploma`:

```shell
diploma > ./install.sh
```

> Для работы Gateway во время установки чартов выполняется настройка [Permissive RBAC Permissions](https://kubernetes.io/docs/reference/access-authn-authz/rbac/#permissive-rbac-permissions).

#### 2. После того, как все сервисы будут установлены и запущены можно выполнить тесты Postman, выполнив команду

```shell
diploma > newman run tests/otus.postman_collection.json
```

Переменная `{{baseUrl}}` настроена на домен `arch.homework`.
