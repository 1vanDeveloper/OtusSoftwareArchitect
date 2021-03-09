cd prometheus
sudo ./install.sh
cd ..
sudo helm install identity ./helm/identity
sudo helm install account ./helm/account
sudo kubectl get all