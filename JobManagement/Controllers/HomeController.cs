using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using JobManagement.Models;
using JobManagement.Models.Enums;
using JobManagement.Models.ViewModels;
using JobManagement.Services;
using Microsoft.AspNetCore.Http.Extensions;

namespace JobManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobService _jobService;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, JobService jobService, UserService userService)
        {
            _logger = logger;
            _jobService = jobService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Research));
        }

        public async Task<IActionResult> Research(bool filtered, string fieldOrder, DateTime? minDate, DateTime? maxDate,
            int selectedUser, EJobStatus selectedStatus, EJobPriority selectedPriority)
        {
            // Anula os controles de filtro quando realizada a ordenação
            if (!Request.GetEncodedUrl().Contains("fieldOrder"))
                filtered = true;

            // Parametro para aplicar a ordenação das colunas da grade
            if (string.IsNullOrEmpty(fieldOrder))
                fieldOrder = "Id";

            if (HttpContext.Session.GetString("urlResearch") == Request.GetEncodedUrl())
            {
                if (HttpContext.Session.GetString("fieldOrder") == fieldOrder)
                    HttpContext.Session.SetString("fieldOrder", fieldOrder + "_desc");
                else
                    HttpContext.Session.SetString("fieldOrder", fieldOrder);
            }
            else
            {
                if (HttpContext.Session.GetString("fieldOrder") == fieldOrder + "_desc")
                    HttpContext.Session.SetString("fieldOrder", fieldOrder + "_desc");
                else
                    HttpContext.Session.SetString("fieldOrder", fieldOrder);
            }

            // Mantem o filtro de Usuário com a opção selecionada
            if (selectedUser != 0)
                HttpContext.Session.SetString("selectedUser", selectedUser.ToString());

            if (!filtered && fieldOrder != "Id" && Convert.ToInt32(HttpContext.Session.GetString("selectedUser")) != 0)
                selectedUser = Convert.ToInt32(HttpContext.Session.GetString("selectedUser"));

            if (filtered && selectedUser == 0)
                HttpContext.Session.SetString("selectedUser", "0");

            var listUsers = await _userService.FindAllAsync("Name");
            ViewData["listUsers"] = new SelectList(listUsers, "Id", "Name", selectedUser);

            // Mantem o filtro de Status com a opção selecionada
            if (selectedStatus != 0)
                HttpContext.Session.SetString("selectedStatus", selectedStatus.ToString());

            if (HttpContext.Session.GetString("selectedStatus") == null)
                HttpContext.Session.SetString("selectedStatus", "0");

            if (!filtered && fieldOrder != "Id" && (EJobStatus)Enum.Parse(typeof(EJobStatus), HttpContext.Session.GetString("selectedStatus")) != 0)
                selectedStatus = (EJobStatus)Enum.Parse(typeof(EJobStatus), HttpContext.Session.GetString("selectedStatus"));

            if (filtered && selectedStatus == 0)
                HttpContext.Session.SetString("selectedStatus", "0");

            var listStatus = Enum.GetValues(typeof(EJobStatus))
               .Cast<EJobStatus>()
               .Select(e => new
               {
                   Value = e,
                   Name = e.GetDisplayName() // Chama o método de extensão
               })
               .ToList();
            ViewData["listStatus"] = new SelectList(listStatus, "Value", "Name", selectedStatus);

            // Mantem o filtro de Prioridade com a opção selecionada
            if (selectedPriority != 0)
                HttpContext.Session.SetString("selectedPriority", selectedPriority.ToString());

            if (HttpContext.Session.GetString("selectedPriority") == null)
                HttpContext.Session.SetString("selectedPriority", "0");

            if (!filtered && fieldOrder != "Id" && (EJobPriority)Enum.Parse(typeof(EJobPriority), HttpContext.Session.GetString("selectedPriority")) != 0)
                selectedPriority = (EJobPriority)Enum.Parse(typeof(EJobPriority), HttpContext.Session.GetString("selectedPriority"));

            if (filtered && selectedPriority == 0)
                HttpContext.Session.SetString("selectedPriority", "0");

            var listPriority = Enum.GetValues(typeof(EJobPriority))
               .Cast<EJobPriority>()
               .Select(e => new
               {
                   Value = e,
                   Name = e.GetDisplayName() // Chama o método de extensão
               })
               .ToList();
            ViewData["listPriority"] = new SelectList(listPriority, "Value", "Name", selectedPriority);

            // Controles para o filtro de Data Inicial e Data Final
            if (minDate.HasValue)
                HttpContext.Session.SetString("minDate", minDate.ToString());
            else if (!filtered && fieldOrder != "Id" && HttpContext.Session.GetString("minDate") != null)
                minDate = DateTime.Parse(HttpContext.Session.GetString("minDate"));
            if (minDate.HasValue)
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            if (filtered && !minDate.HasValue)
                HttpContext.Session.Remove("minDate");

            if (maxDate.HasValue)
                HttpContext.Session.SetString("maxDate", maxDate.ToString());
            else if (!filtered && fieldOrder != "Id" && HttpContext.Session.GetString("maxDate") != null)
                maxDate = DateTime.Parse(HttpContext.Session.GetString("maxDate"));
            if (maxDate.HasValue)
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            if (filtered && !maxDate.HasValue)
                HttpContext.Session.Remove("maxDate");

            // Guarda a url da pesquisa
            HttpContext.Session.SetString("urlResearch", Request.GetEncodedUrl());

            List<Job> list;
            if (selectedUser == 0 && selectedStatus == 0 && selectedPriority == 0 && !minDate.HasValue && !maxDate.HasValue)
                list = await _jobService.FindAllAsync(HttpContext.Session.GetString("fieldOrder"));
            else
                list = await _jobService.FilterAsync(HttpContext.Session.GetString("fieldOrder"), minDate, maxDate, selectedUser, selectedStatus, selectedPriority);

            return View(list);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Gerenciador de Tarefas 1.0";
            ViewData["Developer"] = "Davi Winter";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
