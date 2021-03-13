# disable standerd ingress controller
sudo minikube addons disable ingress
# setting monitoring namespace
sudo kubectl create namespace monitoring
sudo kubectl config set-context --current --namespace=monitoring
# add prometheus and grapfana repository for installing
sudo helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
sudo helm repo add stable https://charts.helm.sh/stable
sudo helm repo update
# install prometheus and grafana
sudo helm install prom prometheus-community/kube-prometheus-stack -f prometheus.yaml #--atomic
# add ingress-nginx repository for installing
sudo helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
sudo helm repo update
# install ingress-controller and postgres exporter like helm application
sudo helm install nginx ingress-nginx/ingress-nginx -f nginx-ingress.yaml #--atomic
sudo kubectl get all