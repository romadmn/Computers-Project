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
        public ComputerController(IComputerService computerService)
        {
            _computerService = computerService;
        }

        // GET: api/Computer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerDto>> GetAsync([FromRoute] int id)
        {
            var computer = await _computerService.GetByIdAsync(id);
            if (computer == null)
            {
                return NotFound();
            }
            return Ok(computer);
        }

        // GET: api/Computer
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<ComputerDto>>> GetAllAsync()
        {
            var computers = await _computerService.GetAllAsync();
            if(computers == null)
            {
                return NotFound();
            }
            return Ok(computers);
        }

        // PUT: api/Computer
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] ComputerDto computerDto)
        {
            var updated = await _computerService.UpdateAsync(computerDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
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
            var deleted = await _computerService.RemoveAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
