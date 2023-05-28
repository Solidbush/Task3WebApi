using Microsoft.AspNetCore.Mvc;
using FileReaderLib;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordCounterController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }

        [HttpPost]
        public IActionResult PostCountWords([FromBody] string text)
        {
            WordCounter wordCounter = new WordCounter();
            return Ok(wordCounter.CountWordsThread(text));
        }
    }
}