using Azure_Blob_Storage.Models;
using Azure_Blob_Storage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Azure_Blob_Storage.Controllers;

public class ContainerController(IContainerService containerService) : Controller
{

    public async Task<IActionResult> Index()
    {
        var containerNames = await containerService.GetAllContainer();

        
        return View(containerNames);
    }

    public async Task<IActionResult> Create()
    {
        return View(new Container());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Container container) 
    {
        await containerService.CreateContainer(container.Name);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string containerName)
    {
        await containerService.DeleteContainer(containerName);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContainerAndBlobs()
    {
        return View(await containerService.GetAllContainerAndBlobs());
    }

}
