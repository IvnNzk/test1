using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatasetController : ControllerBase
    {
        private readonly ILogger<DatasetController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UploadDBContext _dbContext;

        public DatasetController(ILogger<DatasetController> logger, IWebHostEnvironment appEnvironment,
            UploadDBContext dbContext)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _dbContext = dbContext;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(_dbContext.Datasets.ToArray());
        }
    }
}