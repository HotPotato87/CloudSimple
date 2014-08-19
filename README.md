Cloud.Simple
===========

The simplest way to log messages, exceptions & send alerts.
Currently supports Azure Table Storage to store the data.


####Step 1. Sign up for an Azure account and provision a table storage container

Guide : http://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/

Make sure to copy down the account name & key. You can retrieve this from the Azure Management portal by clicking 'Manage Keys'.


####Step 2. Configure your Azure container

Using Visual Studio pull down the nuget package Cloud.Simple.Azure. 

Whether running within a website, or a desktop application it runs as a singleton, in which you can use a fluent syntax to configure.

The simplest configuration is to write 

```
AzureSimpleContainer.Configure("[AccountName]", "[AccountKey]");
```

in the startup of your program.

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

***Simple exception handling***
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


