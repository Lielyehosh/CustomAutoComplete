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
        private QueryOperation _searchOperation = QueryOperation.Prefix;
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
        public async Task<IEnumerable<DbRef>> AutoCompleteSearch([FromQuery] string substring, CancellationToken ct)
        {
            try
            {
                var query = CreateAutoCompleteQuery(substring);
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

        private Query CreateAutoCompleteQuery(string substring)
        {
            try
            {
                return new Query()
                {
                    Name = "Name",
                    Operation = _searchOperation,
                    Value = substring,
                    Table = TableName,
                    Limit = AutoCompleteLimit
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to parse query");
                return null;
            }
        }
    }
}