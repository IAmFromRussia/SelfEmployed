using Microsoft.AspNetCore.Mvc;
using SelfEmployedWeb.Models;

namespace SelfEmployedWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class DialogsClientsController : ControllerBase
{
	private readonly ILogger<DialogsClientsController> _logger;

	public DialogsClientsController(ILogger<DialogsClientsController> logger)
	{
		_logger = logger;
	}

	[HttpGet("getDialogGuidByClients/{clients}")]
	public Guid GetDialogGuidByClients(string clients)
	{
		if (string.IsNullOrEmpty(clients)) return Guid.Empty;

		var clientsList = clients
			.Trim()
			.Split(" ")
			.Select(Guid.Parse)
			.ToList();

		var dialogGuid = new RGDialogsClients().Init()
			.GroupBy(d => d.IDRGDialog)
			.Where(gr => clientsList.All(c => gr.Any(dial => dial.IDClient == c)))
			.Select(gr => gr.Key)
			.FirstOrDefault();
		
		return 
			string.IsNullOrEmpty(dialogGuid.ToString())
				? Guid.Empty 
				: dialogGuid;
	}
}