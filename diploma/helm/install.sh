# need for gateway configuration
# kubectl create namespace otus-services
kubectl config set-context --current --namespace=otus-services
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

# install services
helm install rabbitmq-service bitnami/rabbitmq -f ./rabbit/values.yaml
helm install identity ./identity
helm install billing ./billing
helm install account ./account
helm install order ./order
helm install gateway ./gateway
helm install notification ./notification

# add for gateway service discovery https://ocelot.readthedocs.io/en/latest/features/kubernetes.html
kubectl create clusterrolebinding permissive-binding \
  --clusterrole=cluster-admin \
  --user=admin \
  --user=kubelet \
  --group=system:serviceaccounts
