using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  public class RhymesController : BaseApiController
  {
    private readonly IWordsScraper _scraper;
    public RhymesController(IWordsScraper scraper)
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