# Create Node With Specific VM
This is the readme document for the Create Node With Specific VM demo. Azure provides options to create AKS cluster with a particular VM Size depending on the regions. Different regions have different availability of VM Sizes. Check what VMs are supported in a region and choose a supported VM and create a AKS Cluster. 

**Implementation:** 
We create a AKS Cluster by performing the steps mentioned below and use a specific argument specifying the VM Size as mentioned in steps. Before executing the same we must check for the availability of the VM in that region.

**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'

**Steps:**

0) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
1) Change Power Shell(PS) Azure CLI directory to '..\GitHub\ais-taskapi-aks\Create-Node-With-Specific-VM' 
2) Log in to PowerShell using “az login”
3) Run “az account list” to note down the SubscriptionId being used
4) Run “az account set --subscription <subscriptionid>”   <-- change the place holder with correct id
5) Run the below command to use the “ContainerService”. 
> az provider register -n Microsoft.ContainerService

Do the same for the other packages using the following commands:
> az provider register -n Microsoft.Network

> az provider register -n Microsoft.Storage

> az provider register -n Microsoft.Compute

> az configure --defaults location=eastus
 
6)	Run the below command  to create the Resource Group
> az group create --name myRG --location eastus

The resource group created above is to hold the resources for Master Node and we do not own this. Azure manages this and does not allow to change anything in it. There is another resource group that will be created when we execute step 8 which is meant for the resources of worker nodes and azure will put everything under this resource group. 

7)	Execute the below command to check the supported VM Sizes:
> az vm list-skus -l eastus -o table

The output should show something like "Supported VM Sizes.txt" given in the same folder.

8)	Run the below command to create the cluster:
> az aks create --resource-group myRG --name taskapi-k8s --node-count 1 --node-vm-size Standard_D2_v2 --generate-ssh-keys
 
Check the “myRG” Resource Group in the portal and see that “taskapi-k8s” cluster is created , it takes 30 mins approx...
Once the cluster is created check the VM size that has been allocated by Azure.
 
9)	 Check the cluster availability with the following commands:
> az aks list

10)	Run the below command to start using the cluster:
> az aks get-credentials --resource-group myRG --name taskapi-k8s

Merged "taskapi-k8s" as current context in C:\Users\itesh\.kube\config

11)	Browse the Dashboard as shown in the below UI:
> az aks browse --resource-group myRG --name taskapi-k8s

