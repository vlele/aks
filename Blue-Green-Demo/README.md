# Blue/Gree Deployment
This is the readme document of the Blue/Green Deployment. The same Task API solution that is used for Azure DevOps is used for this demo as well. The demonstration is going to be done in the same AKS Cluster that was built by of Azure DevOps module("aisazdevops-taskapi"). A namespace ('prod') will be created and both Blue & Green Deployment will be done in that namespace. This is used for most of the  proof of concepts on AKS ans Azure DevOps series.

**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and running 

**Steps:**

**Blue Deployment:**

0) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"
1) Open the VS 2017 Solution and make sure that the 'appsettings.json' has following JSON code as shown below:
       "Swagger": {
       "Version": "v1",
       "Title": "Simple Task API 1.0",
       "Description": "A sample task API to create and manage task list and tasks for a user. This is Version 1.0"}
Note: Please make sure not to check in the code into Git Hub. This changes in code is being made just for the Blue/Green Demo.

2) Build the solution in VS 2017 Solution and make sure there are no errors
3) Change Power Shell(PS) Azure CLI directory to '..\GitHub\ais-taskapi-aks\aisazdevops-taskapi' and execute the below command to build TaskApi V1.0(Blue) Image:

> docker images     <-- View the Images that already exists

> docker build -t taskapi-aspnetcore-v1.0.0 .        <-- Build the Images 

    Successfully built a3965b1bbff9

    Successfully tagged taskapi-aspnetcore-v1.0.0:latest

> docker images     <-- View the new Images created 
		
> docker tag taskapi-aspnetcore-v1.0.0 aisazdevopsacr.azurecr.io/taskapi-aspnetcore:v1

> docker push aisazdevopsacr.azurecr.io/taskapi-aspnetcore:v1

Note: If we get 'unauthorized: authentication required' error please login using command, 'az acr login --name aisazdevopsacr'

> az acr repository list --name aisazdevopsacr --output table

4) Create a secret that contains the credentials to connect to ACR as shown below:
> kubectl create secret docker-registry taskapiacrsecret --docker-server aisazdevopsacr.azurecr.io --docker-email Itesh.Simlai@appliedis.com --docker-username=aisazdevopsacr --docker-password  TsxMM8AmiLHUiALPsU6X+qJya2h2ZnbJ
    secret "taskapiacrsecret" created

5) Change PS directory to '..\GitHub\ais-taskapi-aks\Blue-Green-Demo' and execute the below command to get the Service Account details:
> kubectl get serviceaccounts default -o yaml > ./serviceaccount.yml

6) Edit the above 'serviceaccount.yml' file and add the following lines at the end:

imagePullSecrets:
- name: taskapiacrsecret

7) Make sure the Blue Deployment file ('taskapi-aspnetcore-config-blue.yml') contains the 'imagePullSecrets' as shown below:
apiVersion: apps/v1beta1

kind: Deployment
metadata:
  name: demo-taskapi-aspnetcore-deployment-v1
spec:
  replicas: 2
  template:
    metadata:
      labels:
        app: demo-taskapi
        version: v1.0.0
    spec:
      imagePullSecrets:
        - name: taskapiacrsecret

8) Make sure there is no existing Deployment that contains the same labels and selectors as shown below:
> kubectl delete deployment,services,configmap -l app=demo-taskapi

No resources found

9) Create the Blue Deployment 
> kubectl apply -f 'taskapi-aspnetcore-aks-blue.yml'

10) Run the following command to make check that  the TaskApi Application is accessible from Internet. The command should return a public IP(40.121.199.28) for 'demo-taskapi-aspnetcore-service'. Construct the application URL as http://40.121.199.28/swagger  :
> kubectl get all --namespace prod

....
|NAME|                                  |TYPE|           |CLUSTER-IP|    |EXTERNAL-IP|     |PORT(S)|        |AGE|
|svc/demo-taskapi-aspnetcore-service|   |LoadBalancer|   |10.0.32.160|   |40.117.133.34|   |80:32536/TCP|   |58m|

11) Browse the Blue Version of the TaskApi and show content of http://40.117.133.34/swagger  that has been published to end clients. It shows it is serving the version v1 of the application.


**Green Deployment:**
1) Open the VS 2017 Solution and change the 'appsettings.json' code as shown below:
       "Swagger": {
       "Version": "v2",
       "Title": "Simple Task API 2.0",
       "Description": "A sample task API to create and manage task list and tasks for a user. This is Version 2.0"}

Note: Please make sure not to check in the code into Git Hub. This change in code is being made just for the Blue/Green Deployment.

2) Build the solution in VS 2017 Solution and make sure there are no errors
3) Change PS directory to '..\GitHub\ais-taskapi-aks\aisazdevops-taskapi' and execute the below command to build TaskApi V2.0(Green) Image:
> docker images     <-- View the Images that already exists

> docker build -t taskapi-aspnetcore-v2.0.0  .

Successfully built 5ba69051081a

Successfully tagged taskapi-aspnetcore-v2.0.0:latest

> docker images     <-- View the new Images created 

> docker tag taskapi-aspnetcore-v2.0.0 aisazdevopsacr.azurecr.io/taskapi-aspnetcore:v2

> docker push aisazdevopsacr.azurecr.io/taskapi-aspnetcore:v2

Note: If we get 'unauthorized: authentication required' error please login using command, 'az acr login --name aisazdevopsacr'

> az acr repository list --name aisazdevopsacr --output table

4) Change PS directory to '..\GitHub\ais-taskapi-aks\Blue-Green-Demo' and execute the below script to deploy latest TaskApi V2.0(Green) Image:
> kubectl apply -f  'taskapi-aspnetcore-aks-green.yml'

5) The AKS Cluster Dashboard should show the below picture:
6) Browse the Blue Version of the TaskApi and show that the http://40.117.133.34/swagger   that has been published to end clients still serving the old content.
7) Run the following command to return a public IP(40.114.118.186) for 'demo-taskapi-aspnetcore-service-v2'. Construct the application URL as http://40.114.118.186/swagger  :
> kubectl get all --namespace prod

....
NAME                                     TYPE           CLUSTER-IP     EXTERNAL-IP      PORT(S)        AGE
svc/demo-taskapi-aspnetcore-service      LoadBalancer   10.0.32.160    40.117.133.34    80:32536/TCP   1h
svc/demo-taskapi-aspnetcore-service-v2   LoadBalancer   10.0.138.101   40.114.118.186   80:31875/TCP   2m

8) Browse the Green Version of the Application at URL(http://40.114.118.186/swagger) and do the smoke test on V2. If satisfied with results then get ready for switching the end clients to new version.
  
**Error Scenarios and fix Steps:**

i) Please make sure the above UI shows Version 2.0.  If it does not then the reason is that the Step1 is incorrectly built with old 'appsettings.json'. The 'appsettings.json'. file should be updated with correct version before building the DLLs.

ii) If above mistake has happened already then UI will show it. Please fix it as shown below. 
- Start again from Step1 with correct version in 'appsettings.json' as shown in step 1.
- Before doing Step 3 run the below commands:

> kubectl delete deployment,services,configmap -l version=v2.0.0

> docker rmi e5277cf63b28   <-- Image Id of the Docker Image having wrong version.
   
9) Execute the below script to switch end clients to the latest TaskApi V2.0(Green) Version:
> kubectl apply -f 'taskapi-aspnetcore-aks-switch.yml'

   service "demo-taskapi-aspnetcore-service-blue" configured	
   
10) Browse the TaskApi Application and show that the URL (http://40.117.133.34/swagger) that has been published to end clients is now serving the new version as shown below:
   
Important Note on Switching: 

i) After switching from Blue to Green or vice versa we should wait 1-3 mins to complete 

ii) If you want to see the change before view the page in another Browser e.g. IE
   
11) Execute the below script to rollback to the Blue deployment of TaskApi V1.0  Application:
> kubectl apply -f 'taskapi-aspnetcore-aks-roll-back.yml'

   service "demo-taskapi-aspnetcore-service-blue" configured
   
12) Browse the TaskApi Application and show that the URL (http://40.117.133.34/swagger) is now back to the old version

   



