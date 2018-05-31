# Resource Quota in Kubennetes
This is the readme document for Resource Quota demo in Kubennetes . This demo shows how to set quotas for the total amount memory and CPU that can be used by all Containers running in a namespace in an AKS Cluster. The demonstration is going to be done in the same AKS Cluster that was built by of Azure DevOps module("aisazdevops-taskapi").

**Implementation:** 
We use an already created AKS Cluster and create a LimitRange object(cpu-limit-range/mem-min-max-demo-lr). Then create a namespace and create Pods asking 100 CPUs which is beyond quota. Similarly, first create a pod within the defined memory limit. Then create a pod beyond the defined memory limit. 


**Pre-requisite:**
- Azure CLI installed in the machine where the Kubectl Client will run. It could be Laptop, Desktop, Portal. Azure CLI can be installed from here, https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- Use the AzPowerShell in Admin mode and execute the below steps 
- Set the execution policy in Power Shell 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned'
- The AKS Cluster is already created and running 

**CPU Limit Demo**

**Steps:**

0) Clone the GIT Repo from the "https://github.com/icsimlai/ais-taskapi-aks.git"

1) Change Power Shell(PS) Azure CLI directory to '..\GitHub\ais-taskapi-aks\Resource-Quota' and execute the below command to create a namespace and after that create a Pod with 100 CPUs:
> kubectl create namespace cpu-example  <-- Create Namespace

> kubectl create -f cpu-defaults.yaml --namespace=cpu-example  <-- Create CPU constraints of 1-CPU and 0.5-CPU default

> kubectl create -f cpu-defaults-demo.yaml --namespace=cpu-example   <--Create Pod w/o asking CPU requests/limits and see it's ok

> kubectl get pod default-cpu-demo --output=yaml --namespace=cpu-example  <--Check Pod got defaults w/o asking CPU requests/limits

> kubectl create -f cpu-request-limit-2.yaml --namespace=cpu-example   <--Create Pod w/o asking CPU requests/limits and see it's ok

2) Run the following command to check the Pod creation:

> kubectl get pods --namespace=cpu-example

| NAME |         | READY |    | STATUS |        | RESTARTS |    | AGE |

cpu-demo-2         0/1       **Pending**   0          40s

default-cpu-demo   1/1       Running   0          15m

3. Run the following command to check the Pod creation details:
> kubectl describe pod cpu-demo-2 --namespace=cpu-example

…

Events:

  | Type |     | Reason  |           | Age |               | From  |              | Message |

  | Warning |     | FailedScheduling  | | 15s (x14 over 2m) |   | default-scheduler  | | No nodes are available that match all of the predicates: Insufficient cpu (1). |

As you can see the error message says “**PodScheduled   False**” and the reason is “**Insufficient cpu,..**”

**Clean Up**
> kubectl delete namespace cpu-example

> kubectl delete pod cpu-demo-2 --namespace=cpu-example


**Memory Limit Demo-1**
Create a pod within the defined memory limit. 

**Steps:**
1) Execute the below command to create a namespace and a LimitRange:
> kubectl create namespace constraints-mem-example

> kubectl create -f memory-constraints.yaml --namespace=constraints-mem-example

2) Run the following command to view detailed information about the LimitRange::
> kubectl get limitrange mem-min-max-demo-lr --namespace=constraints-mem-example --output=yaml

The output shows the minimum and maximum memory constraints as expected. But notice that even though we didn’t specify default values in the configuration file for the LimitRange, they were created automatically.

3) Run the following command to create a Pod that has one Container. The Container manifest specifies a memory request of 600 MiB and a memory limit of 800 MiB. These satisfy the minimum and maximum memory constraints imposed by the LimitRange.:
> kubectl create -f memory-constraints-pod.yaml --namespace=constraints-mem-example

4) Verify that the Pod’s Container status is running:
> kubectl get pod constraints-mem-demo --namespace=constraints-mem-example

5) View detailed information about the Pod:
> kubectl get pod constraints-mem-demo --output=yaml --namespace=constraints-mem-example

The output shows that the Container has a memory request of 600 MiB and a memory limit of 800 MiB. These satisfy the constraints imposed by the LimitRange, so pod creation is successful.

resources:

limits:

  memory: 800Mi

requests:

  memory: 600Mi


6) Delete the Pod:
> kubectl delete pod constraints-mem-demo --namespace=constraints-mem-example

**Memory Limit Demo-2**
Create a pod beyond the defined memory limit. Here’s the configuration file for a Pod that has one Container. The Container specifies a memory request of 800 MiB and a memory limit of 1.5 GiB.

**Steps:**

1) Attempt to create the Pod:

> kubectl create -f memory-constraints-pod-2.yaml --namespace=constraints-mem-example

The output shows that the Pod does not get created, because the Container specifies a memory limit that is too large:
Error from server (Forbidden): error when creating "docs/tasks/administer-cluster/memory-constraints-pod-2.yaml":
pods "constraints-mem-demo-2" is forbidden: maximum memory usage per Container is 1Gi, but limit is 1536Mi.

**Clean Up**
> kubectl delete namespace constraints-mem-example
