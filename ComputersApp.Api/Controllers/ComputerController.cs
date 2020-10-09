using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputersApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly IComputerService _computerService;
        private readonly IServiceBusTopicSender _serviceBusTopicSender;
        public ComputerController(IComputerService computerService, IServiceBusTopicSender serviceBusTopicSender)
        {
            _computerService = computerService;
            _serviceBusTopicSender = serviceBusTopicSender;
        }

        // GET: api/Computer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerDto>> GetAsync([FromRoute] int id)
        {
            var computer = await _computerService.GetByIdAsync(id);
            return Ok(computer);
        }

        // GET: api/Computer
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ComputerDto>>> GetAllAsync()
        {
            var computers = await _computerService.GetAllAsync();
            return Ok(computers);
        }

        // PUT: api/Computer
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] ComputerDto computerDto)
        {
            await _computerService.UpdateAsync(computerDto);
            return NoContent();
        }

        // POST: api/Computer
        [HttpPost("bus")]
        public async Task<ActionResult<ComputerDto>> PostToServiceBusAsync([FromBody] ComputerDto computerDto)
        {
            await _serviceBusTopicSender.SendMessage(computerDto);
            return Ok(computerDto);
        }

        // POST: api/Computer
        [HttpPost]
        public async Task<ActionResult<ComputerDto>> PostAsync([FromBody] ComputerDto computerDto)
        {
            var computer = await _computerService.AddAsync(computerDto);
            return CreatedAtAction("Get", new { id = computer.Id }, computer);
        }

        // DELETE: api/Computer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _computerService.RemoveAsync(id);
            return Ok();
        }
    }
}
