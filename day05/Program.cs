var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2023.Day05.input.txt")!).ReadToEnd().Trim();
var part2 = Environment.GetEnvironmentVariable("part") == "part2";

var sections = input.Split("\n\n");
var seeds = sections[0][7..].Split(' ').Select(long.Parse).ToList();
var maps = sections.Skip(1).Select(m => m.Split('\n').Skip(1)
	.Select(l => System.Text.RegularExpressions.Regex.Match(l, @"(\d+) (\d+) (\d+)"))
	.Select(m => new Range(long.Parse(m.Groups[1].Value), long.Parse(m.Groups[2].Value), long.Parse(m.Groups[3].Value)))
	.OrderBy(r => r.Source)
	.ToList()).ToList();

if (part2) {
	var s = seeds.Chunk(2).Select(FromChunk);
	foreach (var map in maps)
		s = s.SelectMany(seed => Plant(map, seed));
	Console.WriteLine(s.Min(seed => seed.Min));
}
else {
	foreach (var map in maps)
		for (int i = 0; i < seeds.Count; i++)
			foreach (var range in map)
				if (range.IsInRange(seeds[i])) {
					seeds[i] = range.Move(seeds[i]);
					break;
				}
	Console.WriteLine(seeds.Min());
}

static IEnumerable<Seed> Plant(List<Range> map, Seed seed) {
	foreach (var range in map)
	{
		if (range.SourceMax < seed.Min) { continue; }
		if (seed.Max < range.Source) { break; }
		if (seed.Min < range.Source) {
			yield return seed with { Max = range.Source - 1 };
			seed = seed with { Min = range.Source };
		}
		if (seed.Max < range.SourceMax) {
			yield return new(range.Move(seed.Min), range.Move(seed.Max));
			break;
		}
		yield return new(range.Move(seed.Min), range.Move(range.SourceMax));
		seed = seed with { Min = range.SourceMax + 1 };
	}
	if (map[^1].SourceMax < seed.Min) yield return seed;
}

static Seed FromChunk(long[] chunk) => new(chunk[0], chunk[0] + chunk[1] - 1);

record Range(long Destination, long Source, long Length) {
	public long SourceMax => Source + Length - 1;

	public bool IsInRange(long seed) => Source <= seed && seed < Source + Length;

	public long Move(long seed) => seed + Destination - Source;
}

record Seed(long Min, long Max);
