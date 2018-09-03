using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeadgenFrontend.Models;
using LeadgenFrontend.Services;
using LeadgenWebApi;

namespace LeadgenFrontend.Controllers
{
    public class HomeController : BaseController
    {
        private ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [Route("/e/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            try
            {
                var campaign = _apiService.Client.ApiLeadGenerationGetCampaignGetAsync(null, slug);

                var model = new EntryPageViewModel() { Campaign = await campaign };

                return View(model);
            }
            catch (SwaggerException<ErrorResponse> ex)
            {
                AlertDanger(ex.Result.Message);
                return Redirect("https://ucook.co.za");
            }
            catch (Exception ex)
            {
                AlertDanger(ex.Message);
                return Redirect("https://ucook.co.za");
            }

        }

        public async Task<IActionResult> SignUp(EntryPageViewModel model)
        {
            try
            {
                var lead = await _apiService.Client.ApiLeadGenerationSignUpPostAsync(model.Entry.CampaignId, model.Entry.Email,
                model.Entry.Phone, model.Entry.Name, model.Entry.Surname, null, null, null, null);

                return RedirectToAction("Status", new { slug = model.Entry.Slug });
            }
            catch (SwaggerException<ErrorResponse> ex)
            {
                AlertDanger(ex.Result.Message);
                return RedirectToAction("Index", new { slug = model.Entry.Slug });
            }
            catch (Exception ex)
            {
                AlertDanger(ex.Message);
                return RedirectToAction("Index", new { slug = model.Entry.Slug });
            }
        }

        [Route("/s/{slug}")]
        public async Task<IActionResult> Status(string slug)
        {
            try
            {
                var campaign = _apiService.Client.ApiLeadGenerationGetCampaignGetAsync(null, slug);
                var lead = _apiService.Client.ApiLeadGenerationGetLeadGetAsync(null);

                var model = new StatusPageViewModel() { Campaign = await campaign, CampaignLead = await lead };

                return View(model);
            }
            catch (SwaggerException<ErrorResponse> ex)
            {
                AlertDanger(ex.Result.Message);
                return RedirectToAction("Index", new { slug = slug });
            }
            catch (Exception ex)
            {
                AlertDanger(ex.Message);
                return RedirectToAction("Index", new { slug = slug });
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
