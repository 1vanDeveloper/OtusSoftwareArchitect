# disable standerd ingress controller
minikube addons disable ingress
# setting otus-services namespace
kubectl create namespace otus-services
kubectl config set-context --current --namespace=otus-services

# add prometheus and grapfana repository for installing
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
# install prometheus and grafana
helm install prom prometheus-community/kube-prometheus-stack -f prometheus.yaml #--atomic

# add ingress-nginx repository for installing
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
# install ingress-controller and postgres exporter like helm application
helm install nginx ingress-nginx/ingress-nginx -f nginx-ingress.yaml --skip-crds #--atomic

sleep 30s