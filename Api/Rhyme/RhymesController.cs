using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rhyme
{
  public class RhymesController : BaseApiController
  {
    private readonly IWordsScraperService _scraper;
    public RhymesController(IWordsScraperService scraper)
    {
      _scraper = scraper; //Initializes a WordsScraper object
    }

    [HttpGet("{word}")]
    public async Task<IEnumerable<Rhyme>> GetRhymes(string word) {
        var rhymes = await _scraper.MapRhymes(word); //Scrapes the web and returns Data
        return rhymes;
    }
  }
}