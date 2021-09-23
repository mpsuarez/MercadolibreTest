using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proxy.Domain;
using Proxy.Service.EventHandlers.Commands;
using Proxy.Service.Queries.DataTransferObjects;
using Proxy.Service.Queries.QueryServiceContracts;
using Proxy.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Web.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestQueryService _requestQueryService;
        private readonly IGeneralSettingsQueryService _generalSettingsQueryService;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IRequestQueryService requestQueryService, IGeneralSettingsQueryService generalSettingsQueryService, IMediator mediator)
        {
            _logger = logger;
            _requestQueryService = requestQueryService;
            _generalSettingsQueryService = generalSettingsQueryService;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<RequestDataTransfer> requests = await _requestQueryService.GetAllAsync();
            return View(requests);
        }

        public async Task<IActionResult> GeneralSettingsEdit()
        {
            GeneralSettingsDataTransfer generalSettings = await _generalSettingsQueryService.GetAsync();
            return View(generalSettings);
        }

        [HttpPost]
        public async Task<IActionResult> GeneralSettingsEdit(GeneralSettingsDataTransfer generalSettingsDataTransfer)
        {
            if (ModelState.IsValid)
            {
                GeneralSettingsEditCommand generalSettingsEditCommand = new GeneralSettingsEditCommand()
                {
                    MaxRequestsByIP = generalSettingsDataTransfer.MaxRequestsByIP,
                    MaxRequestsByEndpoint = generalSettingsDataTransfer.MaxRequestsByEndpoint
                };
                IdentityResult result = await _mediator.Send(generalSettingsEditCommand);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View(generalSettingsDataTransfer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
