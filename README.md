# Azure DevOps and AKS Features Demonstration
This is the readme document for Azure DevOps and AKS Features Demonstration. There are a couple of modules under this series of demo  which will show how to create a Azure DevOps Project and explore various features in the AKS Cluster. The demonstration is going to be done in the same AKS Cluster that is built by of Azure DevOps Project. Use a Azure DevOps Project to provision all the Azure resources, a Git code repository, Application Insights integration and a CI/CD pipeline setup for deployment to AKS. Use the DevOps Project dashboard to monitor code commits, builds and deployments from a single view in the Azure portal. The demo uses a C#/.NET Core application in Azure DevOps Project to target AKS and show how the DevOps project creates an AKS cluster along with an Azure Container Registry (ACR) and configures a trust relationship between the two. This enables AKS cluster to pull down container images from ACR. The Build and Release pipeline in VSTS builds the Docker container image, pushes the image to ACR then packaged the Helm chart and deploys it to the newly created AKS cluster.

**Implementation**: 
We create a DevOps Project(aisazdevops) in Azure and choose type of application as .Net. Then we choose the application framework as ASP.NET Core and azure service to deploy the application to as AKS. Finally, we choose VSTS and Azure settings to create everything needed for this demo as mentioned below,
AKS Custer(aisazdevops)

**Pre-requisites:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- Admin Access to Azure Portal
- The AKS Cluster is already created and Sample “Hello World” application running under 'prod' namespace

**Steps**:

**Create Azure DevOps Project Using Git Hub Code:**
1) Login to Azure Portal and click “+Create a resource” and Click “Create” Button on the UI that shows up
2) Choose “Bring your own code” and click “Next”  on the UI that shows up
3) On the "Bring your own code" UI provide name of the "Code Repository" as Git Hub, "Repository" and "Branch" names  and click “Next”  
4) Choose “Asp.Net Core” for the web application framework and click “Next”  on the UI that shows up
5) Choose “Kubernetes Service” and click “Next”  on the UI that shows up
6) Provide the VSTS and Azure information asked on the UI that shows up and click “Done”  
7) After Deployment was successful the Dashboard is shown in the portal. A project is set up in a repository in our VSTS account, a build executes, and our application deploys to AKS. This dashboard provides visibility into our code repository, VSTS CI/CD pipeline, and our application in AKS.
8) After Deployment to AKS cluster is completed a deployment is going to be made in "dev" namespace in the worked node. To see it, browse the Dashboard in AKS by doing the following steps:
Open a new Azure CLI command Window and execute the following commands to open the AKS Dashboard,

> az login

> Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned

> az account list   <-- From the list note down the Subscription Id where the Cluster is running

> azure account set <SubscriptionId>

> az provider register -n Microsoft.ContainerService
 
> az aks get-credentials --resource-group azdevopsdemo9110 --name azdevopsdemo
 
> az aks browse --resource-group azdevopsdemo9110 --name azdevopsdemo

9) In the DevOps Project Dashboard there is a link(AKS deployed application link) on the top right below the EXTERNAL ENDPOINT header. Click the link to browse the Application URL deployed in AKS cluster. The page should show the Task API Application Page.

**Mongo DB Installation:**

1) Open another Powershell window in admin mode and run the commands of 8 step except the last one 
2) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
3) Change Power Shell(PS) Azure CLI directory to '..\GitHub\ais-taskapi-aks\mongodb' and Proceed with the Mongo DB installation steps as mentioned by the Readme file in "..\GitHub\ais-taskapi-aks\mongodb" 
4) Test the Task API Application in browser and verify that it works with backend DB as expected by running some manual tests i.g., create an user , task  ... 

**CI/CD Pipeline:**
1) Launch the "aisazdevops-taskapi.sln" in VS 2017 and make some code change e.g., version no. under "swagger:" in "appsettings.json" and build locally using VS 2017 to ensure zero build error and check in. 
2) Open the Azure Portal DevOps Project Dashboard and see that the build has kicked in and is under progress. Wait for some time till the Release shows green tick mark.
3) Test the Task API Application in browser and verify that it shows the changes as expected 

**AKS Features Demo:**

1) Proceed with the AKS Features Demo as per the steps mentioned by the Readme file in respective folders
2) It is adviced to use a seperate name space and deploy the artifacts while demoing the AKS Features in the cluster
3) It is adviced not to checkin any code to Git Hub for any AKS Features Demo activity. Delete any resources created in the seperate name space while demoing the AKS Features in the cluster

Note: There may be some documentation bug found while trying out the stuff in this series. Please send me input and it will be resolved asap.