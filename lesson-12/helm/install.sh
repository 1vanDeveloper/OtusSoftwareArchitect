# need for gateway configuration
sudo kubectl config set-context --current --namespace=otus-services

# install services
sudo helm install identity ./identity
sudo helm install account ./account
sudo helm install gateway ./gateway

# add for gateway service discovery https://ocelot.readthedocs.io/en/latest/features/kubernetes.html
sudo kubectl create clusterrolebinding permissive-binding \
  --clusterrole=cluster-admin \
  --user=admin \
  --user=kubelet \
  --group=system:serviceaccounts