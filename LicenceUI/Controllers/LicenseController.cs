using LicenseUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedTenant;

namespace LicenseUI.Controllers
{
    public class LicenseController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public LicenseController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        /// <summary>
        /// Action to get all licenses and load the view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            using var client = _http.CreateClient();
            var url = $"{_config["ApiGateway:BaseUrl"]}/license";
            var response = await client.GetStringAsync(url);
            var licenses = JsonConvert.DeserializeObject<List<LicenseViewModel>>(response);
            return View(licenses);
        }

        /// <summary>
        /// Action to create a new license
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(LicenseViewModel model)
        {
            using var client = _http.CreateClient();
            var url = $"{_config["ApiGateway:BaseUrl"]}/license";
            var json = JsonConvert.SerializeObject(new
            {
                model.LicenseNumber,
                model.LicenseType,
                model.Applicant,
                model.Status,
                model.IssueDate,
                model.ExpirationDate,
                TenantStore.TenantId
            });

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            await client.PostAsync(url, content);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action to renew an existing license
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Renew(LicenseViewModel model)
        {
            using var client = _http.CreateClient();
            var url = $"{_config["ApiGateway:BaseUrl"]}/license";
            var json = JsonConvert.SerializeObject(new
            {
                model.LicenseId,
                model.Applicant,
                model.ExpirationDate,
                TenantStore.TenantId
            });

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            await client.PatchAsync(url, content);

            return RedirectToAction("Index");
        }
    }
}
