# Ingress-Controller Deployment
This is the readme document of the Ingress-Controller Deployment. The demonstration is going to be done in the same AKS Cluster that was built by Azure DevOps module("aisazdevops-taskapi"). This demo walks through a sample deployment of the NGINX ingress controller in an AKS cluster.  Two applications are run in the AKS cluster, each of which is accessible over a single address using the mechanism of “Ingress Controller”. A namespace ('prod') will be created and both application deployment will be done in that namespace.Then the KUBE-LEGO project is used to automatically generate and configure Let's Encrypt certificates. 

**Implementation**: 

An ingress controller is a piece of software that provides reverse proxy, configurable traffic routing, and TLS termination for Kubernetes services. Kubernetes ingress resources are used to configure the ingress rules and routes for individual Kubernetes services. Using an ingress controller and ingress rules, a single external address can be used to route traffic to multiple services in a Kubernetes cluster. Please follow the below Steps in an Azure CLI for the demo,


**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and running 
- Helm is installed and working. It can be installed from https://github.com/kubernetes/helm/releases 

**Steps:**

1) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
2) Create a new Resource Group(ais-aks-demo-rg) and a AKS cluster(ais-aks-demo) by performing the steps 1-16 in section 3. Please use the following command while executing step 7 to create a single node AKS cluster:
> az aks create --resource-group ais-aks-demo-rg --name ais-aks-demo --node-count 1 --generate-ssh-keys

2. Run the below command to merge the Kubectl Client config file,
> az aks get-credentials --resource-group ais-aks-demo-rg --name ais-aks-demo

3. Per MS documentation in https://docs.microsoft.com/en-in/azure/aks/ingress now we will use Helm to install the NGINX ingress controller. Run the below commands to initialize Helm and update the chart repository :
> helm init

$HELM_HOME has been configured at C:\Users\itesh\.helm.

Tiller (the Helm server-side component) has been installed into your Kubernetes Cluster.
Please note that by default, Tiller is deployed with an insecure 'allow unauthenticated users' policy.
For more information on securing your installation see: https://docs.helm.sh/using_helm/#securing-
your-helm-installation
Happy Helming!

> helm repo update

Hang tight while we grab the latest from your chart repositories...
...Skip local chart repository
...Successfully got an update from the "brigade" chart repository
...Successfully got an update from the "stable" chart repository
Update Complete. ⎈ Happy Helming!⎈ 

4. Install the **NGINX ingress controller** in the kube-system namespace using the below command, 
> helm install stable/nginx-ingress --namespace kube-system
  
5. Get the NGINX ingress controller info in the kube-system namespace using the below command, 
> kubectl get service -l app=nginx-ingress --namespace kube-system

|NAME|                                      |TYPE|        |CLUSTER-IP|     |EXTERNAL-IP|   |PORT(S)|                  |AGE|

mottled-gecko-nginx-ingress-controller    LoadBalancer    10.0.239.164  **40.114.106.146** 80:30800/TCP,443:32658/TCP   6m

mottled-gecko-nginx-ingress-default-backend ClusterIP      10.0.17.179    <none>           80/TCP                       6m


Note: Browsing the **http://40.114.106.146/** will give **default backend - 404**as expected. It means ok.

6. Configure the DNS name of NGINX ingress controller in the kube-system namespace executing the below shell script in command prompt. Please replace the **IP address** below that you received(EXTERNAL-IP) in step 5 and give a **DNS Name** as you like,

6a. Get Public IP Name corresponding to the IP Address ('40.114.106.146')
> az network public-ip list --query "[?ipAddress!=null]|[?contains(ipAddress, '40.114.106.146')].[resourceGroup]" --output tsv

MC_ais-aks-demo-rg_ais-aks-demo_eastus	

6b. Get Resource Group Name for the AKS Cluster corresponding to the IP Address ('40.114.106.146')
> az network public-ip list --query "[?ipAddress!=null]|[?contains(ipAddress, '40.114.106.146')].[name]" --output tsv

kubernetes-a8babd27252b711e885679ec9943c35f

6c. Update **Public IP Address** with **DNS Name(ais-aks-demo)** the AKS Cluster 

> az network public-ip update --resource-group MC_ais-aks-demo-rg_ais-aks-demo_eastus --name  kubernetes-a8babd27252b711e885679ec9943c35f --dns-name ais-aks-demo

Note: Because HTTPS certificates are going to be used, we need to configure an FQDN name for the ingress controllers IP address. Browsing the **http://ais-aks-demo.eastus.cloudapp.azure.com** will give **default backend - 404** as expected

7. Install the **KUBE-LEGO** using the following Helm install command. Update the email address with one from our organization.  in the kube-system namespace using the below command,
> helm install stable/kube-lego --set config.LEGO_EMAIL=Itesh.Simlai@appliedis.com --set config.LEGO_URL=https://acme-v01.api.letsencrypt.org/directory
 

8. Now an **Ingress Controller** and a Certificate Management Solution have been configured. We can run a few applications in our AKS cluster. For the demo we will use Helm to run multiple instances of a Simple **Hello World** application. Add the Azure samples Helm repository on our development system using the below command,
> helm repo add azure-samples https://azure-samples.github.io/helm-charts/

"azure-samples" has been added to your repositories

9. Run the AKS hello world chart to install **an instance** of “aks-helloworld” with the following command,
> helm install azure-samples/aks-helloworld

10. Install **another instance** of the AKS hello world chart with the following command, specify a new title and a unique service name so that the two applications are visually distinct.
> helm install azure-samples/aks-helloworld --set title="AKS Ingress Demo" --set serviceName="ingress-demo"

11. Now create an **Ingress Route** by the using kubectl apply command with the below yaml file , 
>  kubectl apply -f hello-world-ingress.yaml

ingress "hello-world-ingress" created

12. Check the Dashboard for both deployments

13. **Test** the ingress configuration
Both applications are now running on our Kubernetes cluster, however have been configured with a service of type ClusterIP. As such, the applications are not accessible from the internet as it is with this type. In order to make the services available to internet traffic, we created a Kubernetes ingress resource. The ingress resource configures the rules that route traffic to one of the two applications. Take note that the traffic to the address https://ais-aks-demo.eastus.cloudapp.azure.com/  is routed to the service named aks-helloworld and traffic to the address https://ais-aks-demo.eastus.cloudapp.azure.com/hello-world-two  is routed to the ingress-demo service.

12a. Browse to the FQDN of our Kubernetes ingress controller and see the hello world application
 
12b. Now browse to the FQDN of the ingress controller with the /hello-world-two path and see the hello world application with the custom title
 
12c. Also notice that the connection is encrypted and that a certificate issued by Let's Encrypt is used in both the cases.
