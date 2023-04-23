using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Organiza.API.Configurations.Filters;
using Organiza.Application.Services.UserServices;
using Organiza.Domain.Infra.BaseResponses;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Organiza.API.Controllers._BaseController
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    //[Authorize(Roles = "paymentcore.transactions.read")]
    [Produces("application/json", new string[] { "application/xml", "text/plain", "application/pdf", "application/x-www-form-urlencoded" })]
    [ExcludeFromCodeCoverage]
    [ServiceFilter(typeof(InterceptorHandlerFilter))]
    public class BaseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        private const string MESSAGE = "Operação realizada com sucesso.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userServiceor"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseController(IMediator mediator, IUserService userServiceor)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userService = userServiceor ?? throw new ArgumentNullException(nameof(userServiceor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequestModel"></typeparam>
        /// <typeparam name="TResponseModel"></typeparam>
        /// <param name="request"></param>
        /// <param name="isVoid"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        protected async Task<IActionResult> DoAction<TRequestModel, TResponseModel>(TRequestModel request, bool isVoid, HttpMethod httpMethod) where TRequestModel : IRequest<TResponseModel>
        {
            var response = new BaseResponse<TResponseModel>
            {
                Message = MESSAGE,
                CorrelationId = _userService.CorrelationId
            };

            if (isVoid)
                return await ProcessAndReturnVoid(request, response);


            response.Data = await _mediator.Send(request!);

            if (response.Data is null)
            {
                response.StatusCode = HttpStatusCode.NoContent;
                return StatusCode((int)HttpStatusCode.NoContent, response);
            }

            response.StatusCode = httpMethod == HttpMethod.Post
                         ? HttpStatusCode.Created : HttpStatusCode.OK;

            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequestModel"></typeparam>
        /// <param name="request"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected async Task<IActionResult> DoAction<TRequestModel>(TRequestModel request, string contentType, string fileName)
        {
            var ret = await _mediator.Send(request!);
            return ret is Stream stream ? File(stream!, contentType, fileName) : File(((byte[]?)ret)!, contentType, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequestModel"></typeparam>
        /// <typeparam name="TResponseModel"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<IActionResult> DoAction<TRequestModel, TResponseModel>(TRequestModel request) where TRequestModel : IRequest<PagedBaseResponse<TResponseModel>>
        {
            var response = await _mediator.Send(request!);
            response.Message = MESSAGE;
            response.CorrelationId = _userService.CorrelationId;

            response.StatusCode = HttpStatusCode.OK;
            if (response.Data is null || !response.Data.Any())
                response.StatusCode = HttpStatusCode.NoContent;


            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetErrorMessage(ModelError e) => e.ErrorMessage.Contains("' is not valid for ") || e.ErrorMessage.Contains("Error converting value") || e.ErrorMessage.Contains("Could not convert string to integer:")
                           ? "O Valor enviado não é válido! Verifique o tipo de propriedade e tente novamente."
                           : e.ErrorMessage;

        private async Task<IActionResult> ProcessAndReturnVoid<TRequestModel, TResponseModel>(TRequestModel request, BaseResponse<TResponseModel> response) where TRequestModel : IRequest<TResponseModel>
        {
            await _mediator.Send(request!);
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

    }
}
