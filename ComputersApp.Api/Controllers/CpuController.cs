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
        public async Task<ActionResult<CpuDto>> Get([FromRoute] int id)
        {
            var computer = await _cpuService.GetById(id);
            if (computer == null)
            {
                return NotFound();
            }
            return Ok(computer);
        }

        // GET: api/Computer
        [HttpGet]
        public async Task<ActionResult<List<CpuDto>>> GetAll()
        {
            var computers = await _cpuService.GetAll();
            if (computers == null)
            {
                return NotFound();
            }
            return computers;
        }

        // PUT: api/Computer
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CpuDto computerDto)
        {
            var updated = await _cpuService.Update(computerDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Computer
        [HttpPost]
        public async Task<ActionResult<CpuDto>> Post([FromBody] CpuDto computerDto)
        {
            var computer = await _cpuService.Add(computerDto);
            return CreatedAtAction("Get", new { id = computer.Id }, computer);
        }

        // DELETE: api/Computer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleted = await _cpuService.Remove(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
