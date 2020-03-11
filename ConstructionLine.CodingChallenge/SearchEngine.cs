using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly Dictionary<Color, IEnumerable<Shirt>> _colorDict;
        private readonly Dictionary<Size, IEnumerable<Shirt>> _sizeDict;

        public SearchEngine(List<Shirt> shirts)
        {
            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            _colorDict = shirts.GroupBy(s => s.Color).ToDictionary(g => g.Key, g => g.Select(i => i));
            _sizeDict = shirts.GroupBy(s => s.Size).ToDictionary(g => g.Key, g => g.Select(i => i));
        }


        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            var set = new HashSet<Shirt>();
            var colorCounts = new List<ColorCount>();
            var sizeCount = new List<SizeCount>();

            foreach (var size in options.Sizes)
            {
                var matchingItems = _sizeDict[size].AsQueryable();
                sizeCount.Add(new SizeCount {Size = size, Count = matchingItems.Count()});
                foreach (var matchingItem in matchingItems)
                {
                    set.Add(matchingItem);
                }
            }

            foreach (var color in options.Colors)
            {
                var matchingItems = _colorDict[color].AsQueryable();
                colorCounts.Add(new ColorCount { Color = color, Count = matchingItems.Count() });
                set = set.Intersect(matchingItems).ToHashSet();
            }

            return new SearchResults
            {
                Shirts = set.ToList(),
                ColorCounts = colorCounts,
                SizeCounts = sizeCount
            };
        }
    }
}