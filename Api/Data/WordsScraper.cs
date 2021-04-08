using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Configuration;

namespace Api.Data
{
  public class WordsScraper : IWordsScraper
  {
    private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
    public WordsScraper(Microsoft.Extensions.Configuration.IConfiguration config)
    {
      _config = config;
    }

    //Constructs the Url to scrape from according to the desired word
    //takes as a parameter a word to rhyme with
    //Returns a string with the Url in the right query structure
    private string ConstructUrl(string wordToRhymeWith) {
        string url = _config.GetValue<string>("Settings:Url"); //Gets the url from the appsettings.json
        return url.Replace("wordGoesHere",wordToRhymeWith); //Inserts the new query into the url and returns it
    }
    private async Task<IHtmlDocument> ExtractWebsiteHtml(string Url)
    {
      var config = new Configuration()
               .WithDefaultLoader() //Instatiate a config obj
               .WithJs(); //Allows JavaScript loading 
      var document = await BrowsingContext.New(config)
      .OpenAsync(Url).WhenStable(); //Returns the DOM of the website
      HtmlParser parser = new HtmlParser(); //Instantiate the parser
      var htmlDoc = document.ToHtml(); //Converts the IDocument to string
      return parser.ParseDocument(htmlDoc); //Converts the string to IHtmlDocument
    }

    private Task<IEnumerable<IElement>> ExtractWords(IHtmlDocument htmlDocument)
    {
      IEnumerable<IElement> words = htmlDocument
        .QuerySelectorAll(".wordpanel"); //Extracts Dom Elements with the specified class
      return Task.FromResult<IEnumerable<IElement>>(words);
    }

    public async Task<IEnumerable<Rhyme>> MapRhymes(string wordToRhymeWith)
    {
      string url = ConstructUrl(wordToRhymeWith); //Instantiates the Url
      var siteHtml = await ExtractWebsiteHtml(url); //Extract the DOM from the website
      var words = await ExtractWords(siteHtml); //Extracts words from the DOM
      List<Rhyme> rhymes= new List<Rhyme>();
      foreach (var word in words)
      {
          rhymes.Add(CreateRhyme(word)); //Map word to Rhyme objects
      }
      IEnumerable<Rhyme> RhymesEnum = rhymes;
      return RhymesEnum;
    }

    private Rhyme CreateRhyme(IElement wordElement) {
        string word = wordElement.InnerHtml; //Gets the content of the element
        return new Rhyme {
            Word = word
        };
    }

  }
}