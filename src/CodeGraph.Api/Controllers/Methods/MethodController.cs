﻿using CodeGraph.Domain.Graph.Database.Repositories;
using CodeGraph.Domain.Graph.QueryModels.Results;
using Microsoft.AspNetCore.Mvc;

namespace CodeGraph.Api.Controllers.Methods
{
    [Route("api/[controller]")]
    [ApiController]
    public class MethodController : ControllerBase
    {
        private readonly IMethodRepository _methodRepository;
        private readonly ILogger<MethodController> _logger;

        public MethodController(IMethodRepository methodRepository, ILogger<MethodController> logger)
        {
            _methodRepository = methodRepository;
            _logger = logger;
        }

        [HttpGet("pk/{pk}", Name = nameof(GetMethodsByPk))]
        public async Task<ActionResult> GetMethodsByPk(int pk)
        {
            MethodQueryResult? queryResult = await _methodRepository.LookupMethodByPk(pk.ToString());

            if (queryResult == null)
            {
                return NotFound();
            }
            
            return Ok(queryResult);
        }
        
        [HttpGet("fullName/{fullName}", Name = nameof(GetMethodsByFullName))]
        public async Task<ActionResult> GetMethodsByFullName(string fullName)
        {
            MethodQueryResult? queryResult = await _methodRepository.LookupMethodByFullName(fullName);

            if (queryResult == null)
            {
                return NotFound();
            }
            
            return Ok(queryResult);
        }
    }
}