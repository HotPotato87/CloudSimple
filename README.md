Cloud.Simple
===========

The simplest way to log messages, exceptions & send alerts.
Currently supports Azure Table Storage to store the data.


####Step 1. Sign up for an Azure account and provision a table storage container

Guide : http://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/

Make sure to copy down the account name & key. You can retrieve this from the Azure Management portal by clicking 'Manage Keys'.


####Step 2. Configure your Azure container

Using Visual Studio pull down the nuget package Cloud.Simple.Azure. 

```
Install-Package Cloud.Simple.Azure
```

In the initialization of your program, simply pass in your storage account name and key.

```
AzureSimpleContainer.Configure("[AccountName]", "[AccountKey]");
```

Now you can use the container. All operations are via a singleton instance.

####Step 3. Log messages/exceptions

###### Logging

**Simple Logging**

```
AzureSimpleContainer.Instance.LogMessage("test message");
```

**Categorized Logging (partitioned by category)**

```
AzureSimpleContainer.Instance.LogMessage("test message", category:"sports");
```

**Storing additional data with the log messages**


```
public class User
{
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}
AzureSimpleContainer.Instance.LogMessage(message, meta:new User() { Email = emailAddress});
```

Note : user in this case will be stored as a json string in an additional field

###### Exception Handling

**Simple exception handling**
```
try
{
    throw new DivideByZeroException();
}
catch (Exception eX)
{
    CloudSimpleContainer.Instance.HandleException(eX);
}
```

####Step 3. Configuring the container

To ensure performance, the simple container will not sync the data to Azure every log/exception. By default, it will sync every 60 seconds, or if the number of locally stored objects reaches over 20.

Of course, these values can be configured via a fluent syntax in the configuration of the container (where you passed in the storage keys)

**Commit changes every exception**

```
AzureSimpleContainer.Configure("[AccountName]","[AccountKey]")
    .ConfigureExceptionHandlers()
        .WithFlushThreshold(1);
```

**Commit changes every 5 log messages**

```
AzureSimpleContainer.Configure("[AccountName]","[AccountKey]")
    .ConfigureLogHandlers()
        .WithFlushThreshold(5);
```

**Disable the flush timer **
```
AzureSimpleContainer.Configure("[AccountName]","[AccountKey]")
    .ConfigureLogHandlers()
        .DisableFlushTimer();
```

**Change the flush timer to every 2 minutes **
```
AzureSimpleContainer.Configure("[AccountName]","[AccountKey]")
    .ConfigureLogHandlers()
        .WithFlushTimer(TimeSpan.FromMinutes(2));
```
