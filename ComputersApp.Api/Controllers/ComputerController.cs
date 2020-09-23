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
    public class ComputerController : ControllerBase
    {
        private readonly IComputerService _computerService;
        public ComputerController(IComputerService computerService)
        {
            _computerService = computerService;
        }

        // GET: api/Computer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerDto>> Get([FromRoute] int id)
        {
            var computer = await _computerService.GetById(id);
            if (computer == null)
            {
                return NotFound();
            }
            return Ok(computer);
        }

        // GET: api/Computer
        [HttpGet]
        public async Task<ActionResult<List<ComputerDto>>> GetAll()
        {
            var computers = await _computerService.GetAll();
            if(computers == null)
            {
                return NotFound();
            }
            return computers;
        }

        // PUT: api/Computer
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ComputerDto computerDto)
        {
            var updated = await _computerService.Update(computerDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Computer
        [HttpPost]
        public async Task<ActionResult<ComputerDto>> Post([FromBody] ComputerDto computerDto)
        {
            var computer = await _computerService.Add(computerDto);
            return CreatedAtAction("Get", new { id = computer.Id }, computer);
        }

        // DELETE: api/Computer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleted = await _computerService.Remove(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
