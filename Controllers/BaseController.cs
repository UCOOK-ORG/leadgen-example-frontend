using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeadgenFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LeadgenFrontend.Controllers
{
    /// <summary>
    /// Class BaseController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class BaseController : Controller
    {
        /// <summary>
        /// Alerts the success.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="dismissable">if set to <c>true</c> [dismissable].</param>
        public void AlertSuccess(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        /// <summary>
        /// Alerts the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="dismissable">if set to <c>true</c> [dismissable].</param>
        public void AlertInformation(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        /// <summary>
        /// Alerts the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="dismissable">if set to <c>true</c> [dismissable].</param>
        public void AlertWarning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        /// <summary>
        /// Alerts the danger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="dismissable">if set to <c>true</c> [dismissable].</param>
        public void AlertDanger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        /// <summary>
        /// Adds the alert.
        /// </summary>
        /// <param name="alertStyle">The alert style.</param>
        /// <param name="message">The message.</param>
        /// <param name="dismissable">if set to <c>true</c> [dismissable].</param>
        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? JsonConvert.DeserializeObject<List<Alert>>((string)TempData[Alert.TempDataKey])
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = JsonConvert.SerializeObject(alerts);
        }
    }
}
