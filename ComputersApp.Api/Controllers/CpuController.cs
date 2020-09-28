using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputersApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuController : ControllerBase
    {
        private readonly ICpuService _cpuService;
        public CpuController(ICpuService cpuService)
        {
            _cpuService = cpuService;
        }

        // GET: api/Computer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CpuDto>> GetAsync([FromRoute] int id)
        {
            var computer = await _cpuService.GetByIdAsync(id);
            if (computer == null)
            {
                return NotFound();
            }
            return Ok(computer);
        }

        // GET: api/Computer
        [HttpGet]
        public async Task<ActionResult<List<CpuDto>>> GetAllAsync()
        {
            var computers = await _cpuService.GetAllAsync();
            if (computers == null)
            {
                return NotFound();
            }
            return Ok(computers);
        }

        // PUT: api/Computer
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] CpuDto computerDto)
        {
            var updated = await _cpuService.UpdateAsync(computerDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Computer
        [HttpPost]
        public async Task<ActionResult<CpuDto>> PostAsync([FromBody] CpuDto computerDto)
        {
            var computer = await _cpuService.AddAsync(computerDto);
            return CreatedAtAction("Get", new { id = computer.Id }, computer);
        }

        // DELETE: api/Computer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _cpuService.RemoveAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
