# Домашнее задание (Занятие № 9)
## Prometheus. Grafana

1. Сформирован дашборд в Графане, в котором созданы метрики с разбивкой по API методам:
- Latency (response time) с квантилями по 0.5, 0.95, 0.99, max,
- RPS,
- Error Rate - количество 500ых ответов.

2. Добавлены в дашборд графики с метрикам в целом по сервису, взятые с nginx-ingress-controller:
- Latency (response time) с квантилями по 0.5, 0.95, 0.99, max,
- RPS,
- Error Rate - количество 500ых ответов.

3. Настроен алертинг в графане на Error Rate и Latency.

4. Используя существующие системные метрики из Kubernetes, добавлены на дашборд графики с метриками:
- Потребление подами приложения памяти,
- Потребление подами приолжения CPU.

5. Инструментирована база данных Postgres с помощью экспортера для Prometheus. Добавлены в общий Dashboard графики с метриками работы БД.

### На выходе:
1. Скриншот Dashboard с графиками в момент стресс-тестирования сервиса, после 5-10 минут нагрузки.
[Ссылка](https://raw.githubusercontent.com/1vanDeveloper/OtusSoftwareArchitect/main/lesson-9/results/grafana-dashboard.png)
1. Json-дашборды.
[Ссылка](https://raw.githubusercontent.com/1vanDeveloper/OtusSoftwareArchitect/main/lesson-9/results/grafana-dashboard.json)