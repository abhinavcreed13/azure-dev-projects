## Azure Blob File Uploader App

### **Initialize the Blob storage object model**

In the Azure Storage SDK for .NET Core, the standard pattern for using Blob storage consists of the following steps:

1. To get a `CloudStorageAccount`, call `CloudStorageAccount.Parse` (or `TryParse`) with your connection string.
2. To get a `CloudBlobClient`, call `CreateCloudBlobClient` on the `CloudStorageAccount`.
3. To get a `CloudBlobContainer`, call `GetContainerReference` on the `CloudBlobClient`.
4. To get a list of blobs and/or get references to individual blobs to upload and download data, use methods on the container.

In code, steps 1–3 look like this.

```csharp
CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString); // or TryParse()
CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
CloudBlobContainer container = blobClient.GetContainerReference(containerName);

```

None of this initialization code makes calls over the network. This means that some exceptions that occur because of incorrect information won't be thrown until later. For example, the call to `CloudStorageAccount.Parse` will throw an exception immediately if the connection string is formatted incorrectly, but no exception will be thrown if the storage account that a connection string points to doesn't exist.

To create a container when your app starts or when it first tries to use it, call `CreateIfNotExistsAsync` on a `CloudBlobContainer`.

`CreateIfNotExistsAsync` won't throw an exception if the container already exists, but it does make a network call to Azure Storage. Call it once during initialization, not every time you try to use a container.

### **Getting blobs by name**

To get an `ICloudBlob` by name, call one of the `GetXXXReference` methods on a `CloudBlobContainer`. If you know the type of the blob you are retrieving, to get an object that includes methods and properties tailored for that blob type, use one of the specific methods (`GetBlockBlobReference`, `GetAppendBlobReference`, or `GetPageBlobReference`).

None of these methods make network calls, nor do they confirm whether or not the targeted blob actually exists. They only create a blob reference object locally, which can then be used to call methods that *do* operate over the network and interact with blobs in storage. A separate method, `GetBlobReferenceFromServerAsync`, does call the Blob storage API, and will throw an exception if the blob doesn't already exist.

### **Listing blobs in a container**

You can get a list of the blobs in a container using `CloudBlobContainer`'s `ListBlobsSegmentedAsync` method. *Segmented* refers to the separate pages of results returned — a single call to `ListBlobsSegmentedAsync` is never guaranteed to return all the results in a single page. You may need to call it repeatedly using the `ContinuationToken` it returns to work our way through the pages. This makes the code for listing blobs a little more complex than the code for uploading or downloading, but there's a standard pattern you can use to get every blob in a container.

```csharp
BlobContinuationToken continuationToken = null;
BlobResultSegment resultSegment = null;

do
{
    resultSegment = await container.ListBlobsSegmentedAsync(continuationToken);

    // Do work here on resultSegment.Results

    continuationToken = resultSegment.ContinuationToken;
} while (continuationToken != null);

```

This will call `ListBlobsSegmentedAsync` repeatedly until `continuationToken` is `null`, which signals the end of the results.

**Important:** Never assume that `ListBlobsSegmentedAsync` results will arrive in a single page. Always check for a continuation token and use it if it's present.

### **Processing list results**

The object you'll get back from `ListBlobsSegmentedAsync` contains a `Results` property of type `IEnumerable<IListBlobItem>`. The `IListBlobItem` interface includes only a handful of properties about the blob's container and URL, and isn't very useful by itself.

To get useful blob objects out of `Results`, and to filter and cast the results to more specific blob object types, use the `OfType<>` method. Here are a few examples.

```csharp
// Get all blobs
var allBlobs = resultSegment.Results.OfType<ICloudBlob>();

// Get only block blobs
var blockBlobs = resultSegment.Results.OfType<CloudBlockBlo
```

Tip: Using OfType<> requires a reference to the System.Linq namespace (using System.Linq;).

### Blob uploads and downloads

After you have a reference to a blob, you can upload and download data. ICloudBlob objects have Upload and Download methods that support byte arrays, streams, and files as sources and targets. Specific types have additional methods for convenience — for example, CloudBlockBlob supports uploading and downloading strings with UploadTextAsync and DownloadTextAsync.

### **Create new blobs**

To create a new blob, call one of the `Upload` methods on a reference to a blob that doesn't exist in storage. This does two things: creates the blob in storage, and uploads the data.

### **Move data to and from blobs**

Moving data to and from a blob is a network operation that takes time. In the Azure Storage SDK for .NET Core, all methods that require network activity return `Task`s, so make sure you use `await` in your controller methods appropriately.

A common recommendation when working with large data objects is to use streams instead of in-memory structures like byte arrays or strings. This avoids buffering the full content in memory before sending it to the target. ASP.NET Core supports reading and writing streams from requests and responses.

### **Concurrent access**

Other processes may be adding, changing, or deleting blobs as your app is using them. Always code defensively and think about problems caused by concurrency, such as blobs that are deleted right as you try to download from them, or blobs whose contents change when you don't expect them to. For information about using AccessConditions and blob leases to manage concurrent blob access, at the end of this module, see the *Further Reading* section.

### **Deploy and run in Azure**

Your app is finished — let's deploy it and see it work. Create an App Service app and configure it with app settings for your storage account connection string and container name. Get the storage account's connection string with `az storage account show-connection-string`, and set the name of the container to be `files`.

The app name needs to be globally unique, so you'll need to choose your own name to fill in `<your-unique-app-name>`.

```
az appservice plan create \
--name blob-exercise-plan \
--resource-group learn-63507221-f793-443e-9ade-b6c86f129700 \
--sku FREE --location centralus
```

```
az webapp create \
--name <your-unique-app-name> \
--plan blob-exercise-plan \
--resource-group learn-63507221-f793-443e-9ade-b6c86f129700
```

```
CONNECTIONSTRING=$(az storage account show-connection-string \
--name <your-unique-storage-account-name> \
--output tsv)
```

```
az webapp config appsettings set \
--name <your-unique-app-name> --resource-group learn-63507221-f793-443e-9ade-b6c86f129700 \
--settings AzureStorageConfig:ConnectionString=$CONNECTIONSTRING AzureStorageConfig:FileContainerName=files
```

Now, you'll deploy your app. The following commands will publish the site to the `pub` folder, zip it up into `site.zip`, and deploy the zip to App Service.

```
dotnet publish -o pub
cd pub
zip -r ../site.zip *
```

```
az webapp deployment source config-zip \
--src ../site.zip \
--name <your-unique-app-name> \
--resource-group learn-63507221-f793-443e-9ade-b6c86f129700
```

To see the running app, in a browser, open `https://<your-unique-app-name>.azurewebsites.net`. It should look like the following image.

![https://docs.microsoft.com/en-gb/learn/modules/store-app-data-with-azure-blob-storage/media/7-fileuploader-empty.png](https://docs.microsoft.com/en-gb/learn/modules/store-app-data-with-azure-blob-storage/media/7-fileuploader-empty.png)

Try uploading and downloading some files to test the app. After you've uploaded a few files, to see the blobs that have been uploaded to the container, run the following code in the shell.

```
az storage blob list --account-name <your-unique-storage-account-name> --container-name files --query [].{Name:name} --output table
```