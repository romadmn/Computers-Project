using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputersApp.CosmoDb.Models;
using ComputersApp.CosmoDb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputersApp.CosmoDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly ICosmosDbService<Computer, string> _cosmoDbService;
        public ComputerController(ICosmosDbService<Computer, string> cosmoDbService)
        {
            _cosmoDbService = cosmoDbService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Computer>>> Get()
        {
            try
            {
                var response = await _cosmoDbService.GetAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> Get(string id)
        {
            try
            {
                var response = await _cosmoDbService.GetAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Computer>> Post(Computer computer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _cosmoDbService.CreateAsync(computer);
                    return Ok(response);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
