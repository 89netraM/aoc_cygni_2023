var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2023.Day06.input.txt")!).ReadToEnd().Trim();
var part2 = Environment.GetEnvironmentVariable("part") == "part2";

var lines = input.Split('\n');
var races = part2
	? [(long.Parse(string.Concat(lines[0][5..].Split(' ', StringSplitOptions.RemoveEmptyEntries))), long.Parse(string.Concat(lines[1][9..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse))))]
	: lines[0][5..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).Zip(lines[1][9..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));

Console.WriteLine(races.Select(p => Enumerable.Range(0, (int)p.Item1).LongCount(t => (p.Item1 - t) * t >= p.Item2)).Aggregate(1L, (a, b) => a * b));
