# Домашнее задание (Занятие № 5)
## Основы работы с Kubernetes (часть 2)

1. В папке `kubernetes` располагаются манифесты, в т.ч. Deployment, Service, Ingress . 
Для их запуска в директории `kubernetes` нужно выполнить команду:
```shell
kubectl apply -f deployment.yml -f service.yml -f ingress.yml
```

2. Запрос можно открыть по ссылке:
[http://arch.homework/otusapp/ivan-shpyakin/health](http://arch.homework/otusapp/ivan-shpyakin/health])


3. В сервисе доступны запросы:
- "/" - корень сервиса. Возвращает:
```json
Status 200 OK
{
    "version": "2"
}
```
- "/health" - адрес запроса готовности сервиса. Возвращает:
```json
Status 200 OK
{
    "status": "OK"
}
```