﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeadgenFrontend.Models;
using LeadgenFrontend.Services;
using LeadgenWebApi;
using Microsoft.AspNetCore.Http;

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
                var medium = HttpContext.Session.GetString(model.Entry.CampaignId + "-Medium");
                var referralId = HttpContext.Session.GetInt32(model.Entry.CampaignId + "-ReferralId");

                var lead = await _apiService.Client.ApiLeadGenerationSignUpPostAsync(model.Entry.CampaignId, model.Entry.Email,
                model.Entry.Phone, model.Entry.Name, model.Entry.Surname, referralId, referralId != null ? "platform" : null, medium, model.Entry.Slug);

                HttpContext.Session.SetInt32(lead.CampaignId + "-LeadId", (int)lead.Id);

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
                var campaign = await _apiService.Client.ApiLeadGenerationGetCampaignGetAsync(null, slug);

                var leadId = HttpContext.Session.GetInt32(campaign.Id + "-LeadId");
                var lead = await _apiService.Client.ApiLeadGenerationGetLeadGetAsync(leadId);

                var model = new StatusPageViewModel() { Campaign = campaign, CampaignLead = lead };

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

        [Route("/r/{medium}/{referralId}")]
        public async Task<IActionResult> Referral(string medium, int referralId)
        {
            try
            {

                string mediumDestination;
                switch (medium.ToUpperInvariant())
                {
                    case "G":
                        mediumDestination = "G+";
                        break;
                    case "F":
                        mediumDestination = "Facebook";
                        break;
                    case "T":
                        mediumDestination = "Twitter";
                        break;
                    case "U":
                        mediumDestination = "UniqueLink";
                        break;
                    default:
                        mediumDestination = "";
                        break;
                }

                var lead = await _apiService.Client.ApiLeadGenerationGetLeadGetAsync(referralId);
                var campaign = await _apiService.Client.ApiLeadGenerationGetCampaignGetAsync(lead.CampaignId, null);

                HttpContext.Session.SetString(campaign.Id + "-Medium", mediumDestination);
                HttpContext.Session.SetInt32(campaign.Id + "-ReferralId", referralId);

                return RedirectToAction("Index", new { slug = campaign.Slug });
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

        public IActionResult SendReferral(int leadId, string slug, LeadGenReferralViewModel data)
        {
            try
            {
                _apiService.Client.ApiLeadGenerationSendReferralEmailPostAsync(data.Email, data.Name, data.Surname, leadId);

                return RedirectToAction("Status", new { slug = slug });
            }
            catch (SwaggerException<ErrorResponse> ex)
            {
                AlertDanger(ex.Result.Message);
                return RedirectToAction("Status", new
                {
                    slug = slug
                });
            }
            catch (Exception ex)
            {
                AlertDanger(ex.Message);
                return RedirectToAction("Status", new { slug = slug });
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
