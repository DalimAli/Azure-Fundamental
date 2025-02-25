
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Azure_Blob_Storage.Services;

public class ContainerService(BlobServiceClient _blobClient) : IContainerService
{
    public async Task CreateContainer(string containerName)
    {
        var blobContainerClientResponse = _blobClient.GetBlobContainerClient(containerName);
        await blobContainerClientResponse.CreateIfNotExistsAsync();
    }

    public Task DeleteContainer(string containerName)
    {
        var blobContainerClientResponse = _blobClient.GetBlobContainerClient(containerName);
        return blobContainerClientResponse.DeleteIfExistsAsync();
    }

    public async Task<List<string>> GetAllContainer()
    {
        List<string> containerName = new();

        await foreach (var containers in _blobClient.GetBlobContainersAsync())
        {
            containerName.Add(containers.Name);
        }

        return containerName;
    }

    public async Task<List<string>> GetAllContainerAndBlobs()
    {
        List<string> containerAndBlobNames = new();
        containerAndBlobNames.Add("Account Name : " + _blobClient.AccountName);
        containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
        await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
        {
            containerAndBlobNames.Add("--" + blobContainerItem.Name);
            BlobContainerClient _blobContainer =
                  _blobClient.GetBlobContainerClient(blobContainerItem.Name);
            await foreach (BlobItem blobItem in _blobContainer.GetBlobsAsync())
            {
                //get metadata
                var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                string blobToAdd = blobItem.Name;
                if (blobProperties.Metadata.ContainsKey("title"))
                {
                    blobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                }

                containerAndBlobNames.Add("------" + blobToAdd);
            }
            containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");

        }
        return containerAndBlobNames;
    }
}
