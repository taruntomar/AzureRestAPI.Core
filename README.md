This is the library which can be inherited to implement any new AzureRest API like Graph api, Management API and Billing API

This LIbrary contain two classes:
1. RestAPI
2. RestResponse

You can make REST API Calls by inheriting RestAPI class.
It includes an Authenticator, which does authentication

for example,
```c#
class ManagementAPI:RestAPI
{

}



```

