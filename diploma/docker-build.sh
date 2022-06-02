cp docker/Account/dockerfile .
sudo docker build -t 1vandev/account:v2 .
sudo docker push 1vandev/account:v2

cp docker/ApiGateway/dockerfile .
sudo docker build -t 1vandev/gateway:v5 .
sudo docker push 1vandev/gateway:v5

cp docker/Billing/dockerfile .
sudo docker build -t 1vandev/billing:v1 .
sudo docker push 1vandev/billing:v1

cp docker/Identity/dockerfile .
sudo docker build -t 1vandev/identity:v2 .
sudo docker push 1vandev/identity:v2

cp docker/Order/dockerfile .
sudo docker build -t 1vandev/order:v1 .
sudo docker push 1vandev/order:v1

cp docker/Notification/dockerfile .
sudo docker build -t 1vandev/notification:v1 .
sudo docker push 1vandev/notification:v1