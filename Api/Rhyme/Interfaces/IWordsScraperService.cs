using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Rhyme
{
    public interface IWordsScraperService
    {
        Task<IEnumerable<Rhyme>> MapRhymes(string wordToRhymeWith);
    }
}