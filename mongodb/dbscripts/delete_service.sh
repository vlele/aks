#!/bin/sh
##
# Script to just undeploy the MongoDB Service & StatefulSet but nothing else.
##

# Just delete mongod stateful set + mongodb service onlys (keep rest of k8s environment in place)
kubectl delete statefulsets mongod   --namespace dev
kubectl delete services mongodb-service  --namespace dev
kubectl delete -f ../dbresources/mongodb-service.yaml  --namespace dev
kubectl delete pvc -l role=mongo  --namespace dev
kubectl delete secrets shared-bootstrap-data  --namespace dev


# Show persistent volume claims are still reserved even though mongod stateful-set has been undeployed
kubectl get persistentvolumes  --namespace dev

