using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Api.Models;

namespace Api.Interfaces
{
    public interface IWordsScraper
    {
        Task<IEnumerable<Rhyme>> MapRhymes(string wordToRhymeWith);
    }
}