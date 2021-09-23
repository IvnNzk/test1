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
    static class UploaderBadRequest
    {
        public static string FileDoesntExist = "файл не существует";
        public static string NameShouldContainOnlyLatin = "Имя должно содержать только латинские буквы.";
        public static string NameShouldntContainCapchaWord = "Имя файла не должно содержать capcha";
        public static string FileNameLengthShouldBeInRange4_8 = "Длинна имени файла должна быть в диапозоне  от 4 до 8";
        public static string ImagesCountShouldBeInRange = "Количество картинок должно быть в диапозоне ";
    }

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
                return BadRequest(UploaderBadRequest.FileDoesntExist);
            }

            return File(await System.IO.File.ReadAllBytesAsync(pathToFile), "application/octet-stream", fileName);
        }

        [HttpPost]
        [Description("Загрузить файл с dataset")]
        [RequestSizeLimit(10000000000000)]
        public async Task<IActionResult> Post(IFormFile uploadedFile, bool containCyrillic,
            bool containsNumbers, bool containSpecialCharacters, bool caseSensitivity)
        {
            if (!Regex.IsMatch(uploadedFile.FileName, @"^[a-zA-Z.]+$"))
            {
                return BadRequest(UploaderBadRequest.NameShouldContainOnlyLatin);
            }

            if (uploadedFile.FileName.ToLower().Contains("capcha"))
            {
                return BadRequest(UploaderBadRequest.NameShouldntContainCapchaWord);
            }

            if (uploadedFile.FileName.Length <= 8 && uploadedFile.FileName.Length >= 4)
            {
                return BadRequest(UploaderBadRequest.FileNameLengthShouldBeInRange4_8);
            }

            var fileUUID = Guid.NewGuid();
            string path = $"/Files/{fileUUID}_{uploadedFile.FileName}";

            await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            var countImagaes = 0;
            var maxCountFiles = SummarizeMaxCountImages(containCyrillic, containsNumbers, containSpecialCharacters,
                caseSensitivity);
            using (var archive = ZipFile.OpenRead(_appEnvironment.WebRootPath + path))
            {
                var checkIfExsist = archive.Entries;

                var bar = archive.Entries.ToList();
                var entry = archive.Entries.Where((elem) => elem.FullName == "answers.txt");

                foreach (var fileInArchive in archive.Entries)
                {
                    if (Regex.IsMatch(fileInArchive.FullName, @"^.*\.(jpg|JPG)$"))
                    {
                        countImagaes++;
                    }
                }

                if (maxCountFiles < countImagaes || countImagaes == 0)
                {
                    return BadRequest(UploaderBadRequest.ImagesCountShouldBeInRange + maxCountFiles.ToString());
                }
            }

            /*
             *bool containCyrillic,
            bool containsNumbers, bool containSpecialCharacters, bool caseSensitivity
             */
            var dataset = new Dataset()
            {
                Id = fileUUID,
                FileName = uploadedFile.FileName,
                CreatedAt = DateTime.Now,
                ImagesCount = countImagaes,
                ContainCyrillic = containCyrillic,
                ContainsNumbers = containsNumbers,
                ContainSpecialCharacters = containSpecialCharacters,
                CaseSensitivity = caseSensitivity,
                AnswersType = AnswersTypeEnum.AnswersFile,
            };

            _dbContext.Datasets.Add(dataset);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = dataset.Id }, dataset);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(string id)
        {
            if (_dbContext.Datasets.Find(id) != null)
            {
                return NotFound();
            }

            return Ok();
        }

        [NonAction]
        static int SummarizeMaxCountImages(bool containCyrillic, bool containsNumbers, bool containSpecialCharacters,
            bool caseSensitivity)
        {
            int maxCountFiles = 0;
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

            return maxCountFiles;
        }
    }
}