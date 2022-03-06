# need for gateway configuration
kubectl config set-context --current --namespace=otus-services

# install services
helm install identity ./identity
helm install account ./account
helm install gateway ./gateway

# add for gateway service discovery https://ocelot.readthedocs.io/en/latest/features/kubernetes.html
kubectl create clusterrolebinding permissive-binding \
  --clusterrole=cluster-admin \
  --user=admin \
  --user=kubelet \
  --group=system:serviceaccounts