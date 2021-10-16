using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoComplete.Common.Interfaces;
using AutoComplete.Common.Models;
using AutoComplete.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoComplete.BFF.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private const QueryOperation SearchOperation = QueryOperation.Prefix;
        private const string TableName = "city";
        private readonly IMySqlDal _dal;
        private readonly ILogger<CityController> _logger;
        private readonly IQueryService _queryService;

        public CityController(
            ILogger<CityController> logger,
            IMySqlDal dal,
            IQueryService queryService)
        {
            _logger = logger;
            _dal = dal;
            _queryService = queryService;
        }
        

        [HttpGet("autocomplete")]
        public async Task<ActionResult<IEnumerable<DbRef>>> AutoCompleteSearch([FromQuery] string substring, [FromQuery] int limit,
            CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrEmpty(substring))
                    substring = "";
                var query = _queryService.CreateAutoCompleteQuery(TableName, SearchOperation, substring, limit);
                var results = await _dal.FindAutoCompleteAsync<City>(query, ct);
                return Ok(results);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetById([FromRoute] string id, CancellationToken ct)
        {
            try
            {
                var queryFilter = City.GetIdQueryFilter(id);
                var query = _queryService.CreateQueryFromFilter(TableName, queryFilter);
                var result = await _dal.FindByIdAsync<City>(query, ct);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}