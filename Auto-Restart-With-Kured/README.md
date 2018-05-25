# Auto Restart of AKS Cluster with Kured 
This is the readme document of the Deployment of an automated node restarting solution. By default, AKS Cluster won’t restart nodes after applying security patches. This demo walks through a sample deployment of the Restart Solution in an AKS cluster. We use  Kured, an open-source reboot daemon for Kubernetes to address the same. Kured runs as a DaemonSet and monitors each node for the presence of a file indicating that a reboot is required. It then orchestrates those reboots across the cluster, following the cordon and drain process. The demonstration is going to be done in the same AKS Cluster that was built by of Azure DevOps module("aisazdevops-taskapi"). A namespace ('prod') will be created and the Deployment will be done in that namespace. This is used for most of the  proof of concepts on AKS ans Azure DevOps series.

**Implementation:**
“Hello World” application is already deployed as part of the "Ingress-Controller" demo . Now, we will deploy the Kured (Kubernetes Reboot Daemon) as a Kubernetes Daemonset that performs safe automatic node reboots when the need to do so is indicated by the package management system of the underlying OS. It does the following:
- Watches for the presence of a reboot sentinel e.g. /var/run/reboot-required
- Utilises a lock in the API server to ensure only one node reboots at a time
- Cordons & drains worker nodes before reboot, uncordoning them after


**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and Sample “Hello World” application running under 'prod' namespace

**Imp. Notes:**
1.	The daemon image contains a 1.7.x k8s.io/client-go and kubectl binary for the purposes of maintaining the lock and draining worker nodes. Whilst it has only been tested on a 1.7.x cluster, Kubernetes typically has good forwards/backwards compatibility so there is a reasonable chance it will work on adjacent versions
2.	Additionally, the image contains a systemctl binary from Ubuntu 16.04 in order to command reboots. Again, although this has not been tested against other systemd distributions there is a good chance that it will work.



**Steps:**

0) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"

1) 1) Run the following commands to obtain the nodes and pods currently running in AKS cluster:
> kubectl get nodes

|NAME|                                       |STATUS|    |ROLES|     |AGE|       |VERSION|

aks-nodepool1-42704320-0   Ready      agent       1d           v1.9.6

> kubectl get pods

|NAME|                                              |READY|     |STATUS|        |RESTARTS|    |AGE|

acs-helloworld-hissing-grizzly-85fd499b8c-486fv     1/1         Running             0          1d

acs-helloworld-wrapping-octopus-89d9986f-bgmt7      1/1         Running             0          1d

knobby-salamander-kube-lego-7d8c8bd89-4g5pc         1/1         Running             0          1d

omsagent-7bg5w                                      1/1         Running             0          4h


2) Run the following command to obtain a default installation of Kured without Prometheus alerting interlock or Slack notifications:
> kubectl apply -f kured-ds.yaml

Note that the value of the reboot parameter(period=5m) is set to 5 minutes in the **“kured-ds.yaml”** file. This means the Cluster is going to reboot at every 5 minutes. While doing that it will log messages and do the “Cordons & drains worker nodes” before reboot. As a result it is terminating the pods as seen by the below command:
> kubectl get pods

|NAME|                                              |READY|     |STATUS|        |RESTARTS|    |AGE|
acs-helloworld-hissing-grizzly-85fd499b8c-h98pt     0/1         Pending             0          3s

acs-helloworld-hissing-grizzly-85fd499b8c-tk9tz     1/1         Terminating         0          1m

acs-helloworld-wrapping-octopus-89d9986f-cnhdw      0/1         Pending             0          3s

acs-helloworld-wrapping-octopus-89d9986f-zcbjw      1/1         Terminating         0          1m

knobby-salamander-kube-lego-7d8c8bd89-jq4s2         1/1         Terminating         0          1m

knobby-salamander-kube-lego-7d8c8bd89-m8fvw         0/1         Pending             0          3s

omsagent-4qlhv                                      1/1         Terminating         0          54s

