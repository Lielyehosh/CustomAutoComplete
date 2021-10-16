using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoComplete.Common;
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
        private readonly ILogger<CityController> _logger;
        private readonly IMySqlDal _dal;
        private const QueryOperation SearchOperation = QueryOperation.Prefix;
        private const string TableName = "city";
        private int AutoCompleteLimit { get; set; } = 10;

        public CityController(
            ILogger<CityController> logger,
            IMySqlDal dal)
        {
            _logger = logger;
            _dal = dal;
        }

        
        [HttpGet("autocomplete")]
        public async Task<IEnumerable<DbRef>> AutoCompleteSearch([FromQuery] string substring, [FromQuery] int limit, CancellationToken ct)
        {
            try
            {
                var query = Query.CreateAutoCompleteQuery(substring,SearchOperation,TableName, limit);
                var results = await _dal.SearchAutoComplete<City>(query, ct);
                return results.Select(c => c.ToFieldChoice());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<DbRef>> GetById([FromRoute] string id, CancellationToken ct)
        {
            try
            {
                var query = Query.CreateSearchByIdQuery(id, TableName);
                // await Task.Delay(5000, ct);
                var results = await _dal.SearchAutoComplete<City>(query, ct);
                return results.Select(c => new DbRef()
                {
                    Id = c.Id,
                    Label = c.ToLabel()
                });
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}