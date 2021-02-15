# disable standerd ingress controller
minikube addons disable ingress
# setting monitoring namespace
kubectl create namespace monitoring
kubectl config set-context --current --namespace=monitoring
# add prometheus and grapfana repository for installing
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo add stable https://charts.helm.sh/stable
helm repo update
# install prometheus and grafana
helm install prom prometheus-community/kube-prometheus-stack -f prometheus.yaml --atomic
# add ingress-nginx repository for installing
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
# install ingress-controller and postgres exporter like helm application
helm install nginx ingress-nginx/ingress-nginx -f nginx-ingress.yaml --atomic
helm install postgres-exporter prometheus-community/prometheus-postgres-exporter
kubectl get all