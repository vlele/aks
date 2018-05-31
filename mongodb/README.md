# Install Mongo DB as a Statefulset
This is the readme document of the Mongo DB Deployment as a Statefulset. A namespace "dev" is created as part of Azure DevOps project creation. Both mongo db and taskapi application will be run in "dev" namespace in AKS cluster. The authorization configuration scripts are given for three namespaces('dev', 'qa' and 'prod'). The demonstration is going to be done using the script ending with name "_dev". This is used for most of the  proof of concepts on AKS and Azure DevOps series.

**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and running and a 'dev' namespace is created in the worker node

**Steps:**

0) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
1) Change directory to “..\GitHub\ais-taskapi-aks\mongodb\dbscripts” and execute the shell script to install Mongo DB as shown below:
> .\generate.sh

2) Execute the shell script to configure authentication in Mongo DB as shown below:
> .\configure_repset_auth_dev.sh abc123

Note: Please make sure to use the "configure_repset_auth_dev.sh" when mongodb is created in 'dev' namespace.

**Verification Steps:**

> kubectl get statefulset --namespace dev  

> kubectl get pvc --namespace dev


**Cleanup Steps:**
Execute the shell script to cleanup Mongo DB deployment as shown below:
> .\delete_service.sh

statefulset "mongod" deleted

service "mongodb-service" deleted

error: the path "“..resourcesmongodb-service.yaml”" does not exist

persistentvolumeclaim "mongodb-persistent-storage-claim-mongod-0" deleted

persistentvolumeclaim "mongodb-persistent-storage-claim-mongod-1" deleted

secret "shared-bootstrap-data" deleted