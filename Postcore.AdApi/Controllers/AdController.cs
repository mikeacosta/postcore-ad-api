using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Postcore.AdApi.Services;
using Postcore.AdApi.Shared.Messages;
using Postcore.AdApi.Shared.Models;

namespace Postcore.AdApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _service;
        private readonly IConfiguration _configuration;
        private readonly IAmazonSimpleNotificationService _sns;

        public AdController(IAdService service, IConfiguration configuration, IAmazonSimpleNotificationService sns)
        {
            _service = service;
            _configuration = configuration;
            _sns = sns;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await _service.GetAll());
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(404)]
        [ProducesResponseType(201, Type = typeof(CreateAdResponseDto))]
        public async Task<IActionResult> Create(AdDto dto)
        {
            string id;
            try
            {
                id = await _service.Add(dto);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }

            return StatusCode(201, new CreateAdResponseDto { Id = id });
        }

        [HttpPut]
        [Route("confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirm(ConfirmAdDto dto)
        {
            try
            {
                await _service.Confirm(dto);

                if (dto.Status == AdStatus.Active)
                    await SendAdConfirmMessage(dto);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }

            return new OkResult();
        }

        private async Task SendAdConfirmMessage(ConfirmAdDto dto)
        {
            var topicArn = _configuration.GetValue<string>("TopicArn");
            var ad = await _service.Get(dto.Id);
            var message = new ConfirmAdMessage { Id = dto.Id, Title = ad.Title };
            var msgJson = JsonConvert.SerializeObject(message);
            await _sns.PublishAsync(topicArn, msgJson);
        }
    }
}