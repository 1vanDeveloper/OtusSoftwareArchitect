cd prometheus || exit
./install.sh
# shellcheck disable=SC2103
cd ..
cd helm || exit
./install.sh
cd ..
kubectl get all