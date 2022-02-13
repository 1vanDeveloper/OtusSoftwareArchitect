# prometheus setting
cd prometheus
./install.sh
cd ..
helm install otus-app helm
kubectl get servicemonitors.monitoring.coreos.com
kubectl describe servicemonitors.monitoring.coreos.com  otus-app-helm
