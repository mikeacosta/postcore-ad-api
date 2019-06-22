using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postcore.AdApi.Services;
using Postcore.AdApi.Shared.Models;

namespace Postcore.AdApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _service;

        public AdController(IAdService service)
        {
            _service = service;
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
    }
}