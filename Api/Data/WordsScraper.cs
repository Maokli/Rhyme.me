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
    /// <summary>
    ///WebScraper class.
    ///Contains all the functionality to scrape the web and get data.
    /// </summary>
  public class WordsScraper : IWordsScraper
  {
    private readonly Microsoft.Extensions.Configuration.IConfiguration _config;


    public WordsScraper(Microsoft.Extensions.Configuration.IConfiguration config)
    {
      _config = config;
    }

    
    /// <summary>
    ///Scrapes the web and gets data.
    ///Takes as a parameter the key word to find rhymes for.
    ///Returns a collection of Rhyme objects holding the extracted data.
    /// </summary>
    public async Task<IEnumerable<Rhyme>> MapRhymes (string wordToRhymeWith)
    {
      string url = ConstructUrl(wordToRhymeWith); //Instantiates the Url
      var siteHtml = await ExtractWebsiteHtml(url); //Extract the DOM from the website
      var words = ExtractWords(siteHtml); //Extracts words from the DOM
      
      return words.Select(w => CreateRhyme(w));
    }

    /// <summary>
    ///Constructs the Url to scrape from according to the desired word.
    ///Takes as a parameter a word to rhyme with.
    ///Returns a string with the Url in the right query structure.
    /// </summary>
    private string ConstructUrl(string wordToRhymeWith) 
    {
        string url = _config.GetValue<string>("Settings:Url"); //Gets the url from the appsettings.json
        
        return url.Replace("wordGoesHere",wordToRhymeWith); //Inserts the new query into the url and returns it
    }

    /// <summary>
    ///Scrapes the website based on the passed URL string.
    ///Takes as a parameter the Url.
    ///Returns an IHtmlDocument object holding the DOM of the website.
    /// </summary>
    private async Task<IHtmlDocument> ExtractWebsiteHtml(string Url)
    {
      var httpClient = new HttpClient(); //Initialize the HttpClient
      var request = await httpClient.GetAsync(Url); //Fires a Get request to the Url
      var response = await request.Content.ReadAsStreamAsync(); //Serialize the request
      var parser = new HtmlParser(); //Initialize the parser
      
      return parser.ParseDocument(response); //Converts the IDocumen to IHtmlDocument
    }

    /// <summary>
    ///Extracts the DOM Elements from the entire website DOM.
    ///Takes as a parameter the Website's DOM.
    ///Returns a collection of IElement object holding the extracted elements.
    /// </summary>
    private IEnumerable<IElement> ExtractWords(IHtmlDocument htmlDocument)
    {
      return htmlDocument
      .All
      .Where(c => c.ClassName == "wordpanel") //Extracts Dom Elements with the specified class
      .Take(15); //Limits the number to 15 element
    }


    /// <summary>
    ///Parses the IElement to a Rhyme object.
    ///Takes as a parameter the IElement to parse.
    ///Returns a Rhyme Object.
    /// </summary>
    private Rhyme CreateRhyme(IElement wordElement) 
    {
        string word = wordElement.InnerHtml; //Gets the content of the element
        return new Rhyme {
            Word = word //assigns that word to the Word property in the Rhyme object
        };
    }

  }
}