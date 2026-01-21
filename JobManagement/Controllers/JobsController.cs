using JobManagement.Models;
using JobManagement.Models.ViewModels;
using JobManagement.Services;
using JobManagement.Services.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobManagement.Controllers
{
    public class JobsController : Controller
    {
        private readonly JobService _jobService;
        private readonly UserService _userService;

        public JobsController(JobService jobService, UserService userService)
        {
            _jobService = jobService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(string? fieldOrder)
        {
            if (string.IsNullOrEmpty(fieldOrder))
            {
                fieldOrder = "Id";
            }

            if (HttpContext.Session.GetString("fieldOrder") == "")
            {
                HttpContext.Session.SetString("fieldOrder", "Name");
            }

            if (HttpContext.Session.GetString("fieldOrder") == fieldOrder)
            {
                HttpContext.Session.SetString("fieldOrder", fieldOrder + "_desc");
            }
            else
            {
                HttpContext.Session.SetString("fieldOrder", fieldOrder);
            }

            // Limpa a url da tela de pesquisa, pois foi chamado pelo cadastro de tarefas
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("urlResearch")))
                HttpContext.Session.SetString("urlResearch", "");

            var list = await _jobService.FindAllAsync(HttpContext.Session.GetString("fieldOrder"));
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userService.FindAllAsync("Name");
            var viewModel = new JobFormViewModel { Users = users };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            // Remove a validação da key "Job.User", pois já é feita pela key "Job.UserId"
            if (ModelState.ContainsKey("Job.User"))
            {
                ModelState.Remove("Job.User");
            }
            if (!ModelState.IsValid)
            {
                var users = await _userService.FindAllAsync("Name");
                var viewModel = new JobFormViewModel { Job = job, Users = users };
                return View(viewModel);
            }
            await _jobService.InsertAsync(job);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" });
            }

            var obj = await _jobService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não foi encontrado" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var urlResearch = HttpContext.Session.GetString("urlResearch");
            // Limpa a var da Session que guarda o link da página de pesquisa            
            HttpContext.Session.SetString("urlResearch", "");

            try
            {
                await _jobService.RemoveAsync(id);

                if (!string.IsNullOrEmpty(urlResearch))
                {
                    return Redirect(urlResearch);
                }
                else
                    return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" });
            }

            var obj = await _jobService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" });
            }

            var obj = await _jobService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            List<User> users = await _userService.FindAllAsync("Name");
            JobFormViewModel viewModel = new JobFormViewModel { Job = obj, Users = users };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job)
        {
            var urlResearch = HttpContext.Session.GetString("urlResearch");
            // Limpa a var da Session que guarda o link da página de pesquisa            
            HttpContext.Session.SetString("urlResearch", "");

            // Remove a validação da key "Job.User", pois já é feita pela key "Job.UserId"
            if (ModelState.ContainsKey("Job.User"))
            {
                ModelState.Remove("Job.User");
            }

            if (!ModelState.IsValid)
            {
                var users = await _userService.FindAllAsync("Name");
                var viewModel = new JobFormViewModel { Job = job, Users = users };
                return View(viewModel);
            }
            if (id != job.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id incompatível" });
            }
            try
            {
                await _jobService.UpdateAsync(job);

                if (!string.IsNullOrEmpty(urlResearch))
                    return Redirect(urlResearch);
                else
                    return RedirectToAction(nameof(Index));

            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
