# Monitoring Solution With Log Analytics
This is the readme document of the Monitoring Solution With Log Analytics. This demo walks through deployment of the Container Monitoring Solution with Log Analytics workspace for a sample “Hello World” application running in an AKS cluster. The OMS agent is deployed as a DaemonSet in the cluster. The OMS agent pod is scheduled on each node to send the logs to the analytics service. Once configured, this integration provides right insights into the Kubernetes cluster and containerized workloads. The demonstration is going to be done in the same AKS Cluster that was built by of Azure DevOps module("aisazdevops-taskapi"). A namespace ('prod') will be created and the Deployment will be done in that namespace. This is used for most of the  proof of concepts on AKS ans Azure DevOps series.

**Implementation:** 
“Hello World” application is already deployed as part of "Ingress-Controller" demo. Now, we will enable container health and collect metrics from cluster are automatically using a containerized version of the OMS Agent for Linux and store in our Log Analytics workspace. Please refer the following link for more info,   https://docs.microsoft.com/en-us/azure/monitoring/monitoring-container-health.   


**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and Sample “Hello World” application running under 'prod' namespace

**Imp. Notes:**
1.	Enabling monitoring of your AKS container already deployed cannot be accomplished from the portal, it can only be performed using the provided Azure Resource Manager template

2.	We cannot use kubectl to upgrade, delete, re-deploy, or deploy the agent.


**Steps:**

1) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
2) Run the below command to deploy a Log Analytics workspace with the Container Monitoring Solution using an ARM template,
> az group deployment create --resource-group ais-aks-demo-rg --template-uri https://raw.githubusercontent.com/neumanndaniel/armtemplates/master/operationsmanagement/containerMonitoringSolution.json --parameters workspaceName=ais-aks-demo-ws --verbose
  
2) Retrieve the **OMS Workspace ID** and **Primary Key** using the below ,

2a) Go to Azure Portal --> Home --> Resource groups -->  ais-aks-demo-rg --> Containers(ais-aks-demo-ws) -->  ”OMS Workspace” and from right panel copy the **“Workspace Id”** 

2adb4bd1-dcd3-4370-a95e-1b9dbc86d4ea

2b) From inside “OMS Workspace” navigation pane on the left side select Advanced settings and then Select Connected Sources  -->   “Windows Servers”  and make a note of the **WORKSPACE ID** and **PRIMARY KEY**. We will need them to configure the **DaemonSet** in Kubernetes which deploys the Microsoft/OMS docker image to all of the nodes in our AKS cluster: 

**WORKSPACE ID**:
 2adb4bd1-dcd3-4370-a95e-1b9dbc86d4ea

**PRIMARY KEY**:   l5Qutidm0jjhZEj0xKfF5CElG7dg7Ww6KKtzkUzY/S0Uge1KEZYl/4cMIbd51NABCN9ZLJnNYIaNpBYF1K8lZA==

2c) Now, deploy a **DaemonSet** with the **microsoft/oms** image. Copy the **yaml** from the following **URL**:
https://raw.githubusercontent.com/Microsoft/OMS-docker/master/Kubernetes/omsagent.yaml and replace the following variables ,

    agentVersion: 1.6.0-42

    dockerProviderVersion: 1.0.0-33

    env:

       - name: **WSID**
         value: 2adb4bd1-dcd3-4370-a95e-1b9dbc86d4ea
       - name: **KEY** 
         value:   l5Qutidm0jjhZEj0xKfF5CElG7dg7Ww6KKtzkUzY/S0Uge1KEZYl/4cMIbd51NABCN9ZLJnNYIaNpBYF1K8lZA==

The updated file(**kubemonitor.yaml**) that was used for this demo is given in the folder('..\GitHub\ais-taskapi-aks\Monitoring-Solution-With-Log-Analytics'). 

2d) Change Power Shell(PS) Azure CLI directory to '..\GitHub\ais-taskapi-aks\Monitoring-Solution-With-Log-Analytics' and execute the below command and check the result:
> kubectl create -f kubemonitor.yml

daemonset "omsagent" created

 2e) Verify DaemonSet deployment and that containers are running by using the following command:
> kubectl get daemonset

Also check the same from Dashboard by using the following command:
> az aks browse --resource-group aisazdevopsrg  --name ais-taskapi-aks

2f) Verify the logs in our Azure Container Monitoring Solution. Go to Azure Portal --> Home --> Resource groups --> ais-aks-demo-rg --> Containers(ais-aks-demo-ws) --> 
”Overview” and from right panel see the summary. Click the blue ring under summary and see the below details.

2g) There's lot of summarized boxes on the right where details of the entire AKS Cluster is given. We can get info of our specific interest by running some search query. Go to the extreme right box and click on a sample query as shown below "See all the commands in past 24 hours.". By clicking the  link we will see all the commands that was run in past 24 hrs. on the left hand side under "COMMAND"


2h) Get more in-depth Log Search with Log Analytics using the Analytics Link as shown below:
Go to Azure Portal --> Home --> Resource groups --> ais-aks-demo-rg --> Containers(ais-aks-demo-ws) --> ”Overview” and from panel  click the **"Analytics"**. Click the  link and this will redirect us to https://portal.loganalytics.io URL, specific for our monitoring solution, and this is where Azure Log Analytics really comes into play. See a new window opens in a new tab.As seen in the new tab, there is a handy section with **"Common Queries"** where we can easily just click "Run" on any one of them to execute and see the results.
