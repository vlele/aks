# Auto Scaling of AKS Cluster
This is the readme document for the Auto Scaling of AKS Cluster. The demonstration is going to be done in the same AKS Cluster that was built by of Azure DevOps module("aisazdevops-taskapi"). There we created a single node AKS cluster and now we will increase the number of nodes from 1 to 2 and then bring it down from 2 to 1. 
As per the MS documentation in https://docs.microsoft.com/en-us/azure/aks/scale-cluster  
“ …When scaling down, nodes will be carefully cordoned and drained to minimize disruption to running applications. When scaling up, the az command waits until nodes are marked Ready by the Kubernetes cluster.”
We will see the same.


**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and Sample “Hello World” application running under 'prod' namespace

**Steps:**

0) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
1) Change Power Shell(PS) Azure CLI directory to '..\GitHub\ais-taskapi-aks\Auto-Scaling' and execute the below command to **scale Up** to 2 node:
> az aks scale --name taskapi-k8s --resource-group itesh-aks-demo-rg --node-count 2

 - Running ..

1) Check  that there are two nodes created  from Dashboard by using the following command:
> az aks browse --resource-group aisazdevopsrg  --name ais-taskapi-aks


3) Execute the below script to **scale down** to 1 node:
> az aks scale --name taskapi-k8s --resource-group itesh-aks-demo-rg --node-count 1

 - Running ..

See in the Dashboard that there are two nodes showing up but one is being torn without affecting the Application. While it was tearing it down I kept browsing the Application without any issue.

   



