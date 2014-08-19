Cloud.Simple
===========

The simplest way to log messages, exceptions & send alerts.
Currently supports Azure Table Storage to store the data.


##Step 1. Sign up for an Azure account and provision a table storage container##

Guide : http://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/

Make sure to copy down the Accountname & key. You can retrieve this from the Azure Management portal by clicking 'Manage Keys'


##Step 2. Configure your Azure container##

Now, using Visual Studio pull down the nuget package Cloud.Simple.Azure. 

Whether running within a website, or a desktop application it runs as a singleton, in which you can use a fluent syntax to configure.

The simplest configuration is to write 

```
AzureSimpleContainer.Configure("[AccountName]", "[AccountKey]");
```

in the startup of your program.

Step 3. Log messages/exceptions
======================================
