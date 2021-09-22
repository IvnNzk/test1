using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatasetUploaderController : ControllerBase
    {
        private readonly ILogger<DatasetUploaderController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UploadDBContext _dbContext;

        public DatasetUploaderController(ILogger<DatasetUploaderController> logger, IWebHostEnvironment appEnvironment,
            UploadDBContext dbContext)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _dbContext = dbContext;
        }

        // Download file
        [HttpGet]
        [Description("Загрузить файл по имени")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<IActionResult> Get(string fileName)
        {
            string pathToFile = _appEnvironment.WebRootPath + "/Files/" + fileName;

            if (!System.IO.File.Exists(pathToFile))
            {
                return BadRequest("File doesn't exist");
            }

            return File(await System.IO.File.ReadAllBytesAsync(pathToFile), "application/octet-stream", fileName);
        }

        [HttpPost]
        [Description("Загрузить файл с dataset")]
        [RequestSizeLimit(10000000000000)]
        public async Task<ActionResult<Dataset>> Post(IFormFile uploadedFile, bool containCyrillic,
            bool containsNumbers, bool containSpecialCharacters, bool caseSensitivity)
        {
            if (!Regex.IsMatch(uploadedFile.FileName, @"^[a-zA-Z.]+$"))
            {
                return BadRequest("Имя должно содержать только латинские буквы.");
            }

            if (uploadedFile.FileName.ToLower().Contains("capcha"))
            {
                return BadRequest("Имя файла не должно содержать capcha");
            }

            if (uploadedFile.FileName.Length <= 8 && uploadedFile.FileName.Length >= 4)
            {
                return BadRequest("Длинна файла не должна быть больше");
            }

            var fileUUID = Guid.NewGuid();
            string path = $"/Files/{fileUUID}_{uploadedFile.FileName}";

            await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            int imageCount = 0;

            using (var archive = ZipFile.OpenRead(_appEnvironment.WebRootPath + path))
            {
                var checkIfExsist = archive.Entries;

                var bar = archive.Entries.ToList();
                var entry = archive.Entries.Where((elem) => elem.FullName == "answers.txt");

                var maxCountFiles = 0;
                if (containCyrillic)
                {
                    maxCountFiles += 3000;
                }

                if (containsNumbers)
                {
                    maxCountFiles += 3000;
                }

                if (containSpecialCharacters)
                {
                    maxCountFiles += 3000;
                }

                if (caseSensitivity)
                {
                    maxCountFiles += 3000;
                }


                var countImgaes = 0;
                foreach (var fileInArchive in archive.Entries)
                {
                    if (Regex.IsMatch(fileInArchive.FullName, @"^.*\.(jpg|JPG)$"))
                    {
                        countImgaes++;
                    }
                }

                if (maxCountFiles < countImgaes || countImgaes == 0)
                {
                    return BadRequest("Количество картинок должно быть в диапозоне...");
                }
            }

            var dataset = new Dataset()
            {
                Id = fileUUID,
                FileName = uploadedFile.FileName,
                CreatedAt = DateTime.Now,
                ImagesCount = 2000,
                ContainCyrillic = true,
                ContainsNumbers = true,
                ContainSpecialCharacters = true,
                CaseSensitivity = true,
                AnswersType = AnswersTypeEnum.AnswersFile,
            };

            _dbContext.Datasets.Add(dataset);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = dataset.Id });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dataset))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            if (_dbContext.Datasets.Find(id) != null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}