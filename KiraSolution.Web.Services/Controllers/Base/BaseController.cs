using KiraSolution.Web.Services.Areas.Authentication.Resources;
using KiraSolution.Web.Services.Other.Transaction.Exceptions;
using KiraSolution.Web.Services.Other.Transaction.Executor;
using KiraSolution.Web.Services.Other.Transaction.Model;
using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.ViewModelBase;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KiraSolution.Web.Services.Controllers.Base
{

    [Route("[controller]")]
    [ApiController]
    public class BaseController<TViewModel> : ControllerBase, IBaseController<TViewModel>
        where TViewModel : class, IBaseViewModel<int>
    {
        internal readonly IApplicationServiceBase<int, TViewModel> _service;
        internal string AcceptedFileTypes = string.Empty;

        public BaseController(IApplicationServiceBase<int, TViewModel> service) =>
            _service = service;

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            Func<Task<IEnumerable<TViewModel>>> predicate = async () => { return await _service.GetAllAsync(); };
            var response = await SafeExecutor<IEnumerable<TViewModel>>.ExecAsync(predicate);
            return ProcessResponse(response);
        }

        [HttpGet("{id}", Name = "Get[controller]")]
        public virtual async Task<IActionResult> Get(int id) 
        {
            var response = await GetModelAsync(id);
            return ProcessResponse(response);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TViewModel viewModel)
        {
            SetServiceUser();

            Func<Task<TViewModel>> predicate = async () => { return await _service.AddWithResponseAsync(viewModel); };
            var response = await SafeExecutor<TViewModel>.ExecAsync(predicate);
            return ProcessResponse(response, true);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] TViewModel viewModel)
        {
            SetServiceUser();

            if (viewModel.Id != id) return BadRequest();

            Func<Task> predicate = async () =>  await _service.ModifyAsync(viewModel); 
            var response = await SafeExecutor.ExecAsync(predicate);
            return ProcessResponse(response);
        }

        [HttpPatch("{Id}")]
        public virtual async Task<IActionResult> Patch(int Id, [FromBody] JsonPatchDocument<TViewModel> pathDocument)
        {
            SetServiceUser();
            Func<Task> predicate = async () => await PathData(Id, pathDocument);
            var response = await SafeExecutor.ExecAsync(predicate);
            return ProcessResponse(response);
        }

        private async Task<TViewModel> PathData(int id, JsonPatchDocument<TViewModel> pathDocument)
        {
            if (pathDocument is null) throw new Exception(AuthenticationResource.BadPathDocument);

            var response = await _service.GetAsync(id);
            if (response == null) throw new NotFoundException();

            pathDocument.ApplyTo(response, ModelState);
            if (!TryValidateModel(response)) throw new ModelStateException(ModelState);

            return await _service.ModifyWithResponseAsync(response);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            SetServiceUser();
            var response = await GetModelAsync(id);

            if(response.Data == null) return NotFound(response);
            var response1 = SafeExecutor.Exec(() => _service.Remove(response.Data));
            return ProcessResponse(response1); ;
        }

        private string GetControllerName() =>
            ControllerContext.RouteData.Values["controller"].ToString();

        private void SetServiceUser()
            => _service.SetUser(GetUser());

        protected string GetUser()
        {
            if (HttpContext.User is null || !HttpContext.User.Claims.Any()) return string.Empty;
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        protected IActionResult ProcessResponse<T>(IAnswerBase<T> response, bool isPost = false) where T : class
        {
            if (response.Success)
            {
                if (isPost) 
                    return new CreatedAtRouteResult($"Get{GetControllerName()}", 
                        new { id = ((IBaseViewModel<int>)response.Data).Id }, response);
                else return Ok(response);
            }
            else return BadRequest(response);
        }

        private async Task<IAnswerBase<TViewModel>> GetModelAsync(int id) 
        {
            Func<Task<TViewModel>> predicate = async () => { return await _service.GetAsync(id); };
            return  await SafeExecutor<TViewModel>.ExecAsync(predicate);
        }
    }
}
