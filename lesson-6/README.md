# Домашнее задание (Занятие № 5)

1. В папке `kubernetes` располагаются манифесты, в т.ч. Deployment, Service, Ingress . 
Для их запуска в директории `kubernetes` нужно выполнить команду:
```shell
kubectl apply -f .\deployment.yml -f.\service.yml -f .\ingress.yml
```

2. Тест Postman для проверки запроса можно открыть по ссылке:
[https://www.getpostman.com/collections/b95a09271533990df0cc](https://www.getpostman.com/collections/b95a09271533990df0cc)

3. В запросе нужно изменить ip, на новый, который получит Ingress.
Посмотреть ip можно по команде:
```shell
kubectl get ing
```

4. В сервисе доступны запросы:
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