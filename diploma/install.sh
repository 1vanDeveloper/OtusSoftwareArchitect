cd prometheus || exit
./install.sh
cd ..
cd helm || exit
./install.sh
cd ..
kubectl get all
