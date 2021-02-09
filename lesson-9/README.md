# Домашнее задание (Занятие № 9)
## Prometheus. Grafana

1. В папке `helm` располагается чарт запуска приложения, в т.ч. Deployment, Service, Ingress, ConfigMap, Secret. Также чарт содержит зависимость `postgresql` из репозитория `bitnami`
Для установки чарта в папке `lesson-6` нужно выполнить команду:
```shell
helm install otus-app .\helm
```
где `otus-app` изменяемое название приложения.

При инсталляции чарта автоматически применяется миграция БД в виде `Job`.

2. Тесты Postman для проверки запроса можно открыть по ссылке (папка <b>lesson-6</b>):
[https://www.getpostman.com/collections/b95a09271533990df0cc](https://www.getpostman.com/collections/b95a09271533990df0cc)

3. Перед началом работы с запросами нужно изменить значение переменной `host`, на новое, которое можно получить по команде:
```shell
kubectl get ing
```

4. В сервисе доступны пробы:
- "/" - корень сервиса. Возвращает:
```json
Status 200 OK
{
    "version": "3"
}
```
- "/health" - адрес запроса готовности сервиса. Возвращает:
```json
Status 200 OK
{
    "status": "OK"
}
```