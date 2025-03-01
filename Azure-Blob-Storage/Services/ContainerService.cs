
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Azure_Blob_Storage.Services;

public class ContainerService(BlobServiceClient _blobServiceClient) : IContainerService
{
    public async Task CreateContainer(string containerName)
    {
        var blobContainerClientResponse = _blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClientResponse.CreateIfNotExistsAsync();
    }

    public Task DeleteContainer(string containerName)
    {
        var blobContainerClientResponse = _blobServiceClient.GetBlobContainerClient(containerName);
        return blobContainerClientResponse.DeleteIfExistsAsync();
    }

    public async Task<List<string>> GetAllContainer()
    {
        List<string> containerName = new();

        await foreach (var containers in _blobServiceClient.GetBlobContainersAsync())
        {
            containerName.Add(containers.Name);
        }

        return containerName;
    }

    public async Task<List<string>> GetAllContainerAndBlobs()
    {
        List<string> containerAndBlobNames = new();
        containerAndBlobNames.Add("Account Name : " + _blobServiceClient.AccountName);
        containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
        await foreach (BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
        {
            containerAndBlobNames.Add("--" + blobContainerItem.Name);
            BlobContainerClient _blobContainer =
                  _blobServiceClient.GetBlobContainerClient(blobContainerItem.Name);
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
