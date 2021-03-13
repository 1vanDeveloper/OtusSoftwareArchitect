sudo helm install identity ./identity
sudo helm install account ./account
sudo helm install gateway ./gateway
sudo kubectl create clusterrolebinding permissive-binding \
  --clusterrole=cluster-admin \
  --user=admin \
  --user=kubelet \
  --group=system:serviceaccounts