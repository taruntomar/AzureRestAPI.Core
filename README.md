# AzureRESTAPI
It provides a wrapper on variour Azure REST APIs

## Getting Started
### Management API with client authentication
```c# 
using AzureRestAPI.ManagementAPI
AzureManagementAPI managementAPI = new AzureManagementAPI();
//configuring authentication parameters
managementAPI.AuthenticationMode = AuthenticationModes.ClientCredential;
managementAPI.AuthenticationConfig.Client.Id= ""; // client id registered in AzureAD info
managementAPI.AuthenticationConfig.Client.Secret= ""; 
managementAPI.AuthenticationConfig.BaseURL= ""; // url of the authority which provide access. ex: https://management.core.windows.net/
managementAPI.AuthenticationConfig.TanentID= ""; // id of Tanent in AD
//make a request
Subscription[] subscriptions =  managementAPI.Subscriptions(); // return list of all subscriptions to which client is having role reader
```



