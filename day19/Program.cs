using System.Text.RegularExpressions;

var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2023.Day19.input.txt")!).ReadToEnd().Trim();
var part2 = Environment.GetEnvironmentVariable("part") == "part2";

var paragraphs = input.Split("\n\n");
var workflows = paragraphs[0].Split('\n').Select(l => l.Split('{')).ToDictionary(p => p[0], p => ParseWorkflow(p[1][..^1]));
var parts = paragraphs[1].Split('\n').Select(l => Regex.Match(l, @"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}"))
	.Select(m => m.Groups.Skip<Group>(1).Select(g => long.Parse(g.Value)).ToArray()).Select(v => new Part(v[0], v[1], v[2], v[3]));

Console.WriteLine(part2 ? Part2(new(new(1, 1, 1, 1), new(4000, 4000, 4000, 4000)), "in") : parts.Sum(p => Part1(p, "in")));

long Part1(Part part, string flow) => workflows[flow].Process(part) switch {
	"R" => 0,
	"A" => part.Total,
	var f => Part1(part, f),
};

long Part2(Range range, string flow) => flow switch {
	"R" => 0,
	"A" => range.Count,
	_ => workflows[flow].Branches.Sum(branch => {
		if (range is null) return 0;
		(range, var nextRange) = branch switch {
			LessThan.X(var value, _) => (range with { Min = range.Min with { X = value } }, range with { Max = range.Max with { X = value - 1 } }),
			LessThan.M(var value, _) => (range with { Min = range.Min with { M = value } }, range with { Max = range.Max with { M = value - 1 } }),
			LessThan.A(var value, _) => (range with { Min = range.Min with { A = value } }, range with { Max = range.Max with { A = value - 1 } }),
			LessThan.S(var value, _) => (range with { Min = range.Min with { S = value } }, range with { Max = range.Max with { S = value - 1 } }),
			GreaterThan.X(var value, _) => (range with { Max = range.Max with { X = value } }, range with { Min = range.Min with { X = value + 1 } }),
			GreaterThan.M(var value, _) => (range with { Max = range.Max with { M = value } }, range with { Min = range.Min with { M = value + 1 } }),
			GreaterThan.A(var value, _) => (range with { Max = range.Max with { A = value } }, range with { Min = range.Min with { A = value + 1 } }),
			GreaterThan.S(var value, _) => (range with { Max = range.Max with { S = value } }, range with { Min = range.Min with { S = value + 1 } }),
			_ => (null, range),
		};
		return Part2(nextRange, branch.Outcome);
	}),
};

static Workflow ParseWorkflow(string flow) =>
	new(flow.Split(',')
		.Select<string, Branch>(b => b.Split(':') switch {
			[string outcome] => new Branch(outcome),
			[string condition, string outcome] => (condition[0], condition[1], long.Parse(condition[2..])) switch {
				('x', '<', long value) => new LessThan.X(value, outcome),
				('m', '<', long value) => new LessThan.M(value, outcome),
				('a', '<', long value) => new LessThan.A(value, outcome),
				('s', '<', long value) => new LessThan.S(value, outcome),
				('x', '>', long value) => new GreaterThan.X(value, outcome),
				('m', '>', long value) => new GreaterThan.M(value, outcome),
				('a', '>', long value) => new GreaterThan.A(value, outcome),
				('s', '>', long value) => new GreaterThan.S(value, outcome),
				_ => throw new Exception(),
			},
			_ => throw new Exception(),
		})
		.ToArray());

record Workflow(Branch[] Branches) {
	public string Process(Part part) {
		foreach (var branch in Branches)
			if (branch.Test(part))
				return branch.Outcome;
		throw new Exception();
	}
}

record Branch(string Outcome) {
	public virtual bool Test(Part part) => true;
}
abstract record LessThan(long Value, string Outcome) : Branch(Outcome) {
	public record X(long Value, string Outcome) : LessThan(Value, Outcome) {
		public override bool Test(Part part) => part.X < Value;
	}
	public record M(long Value, string Outcome) : LessThan(Value, Outcome) {
		public override bool Test(Part part) => part.M < Value;
	}
	public record A(long Value, string Outcome) : LessThan(Value, Outcome) {
		public override bool Test(Part part) => part.A < Value;
	}
	public record S(long Value, string Outcome) : LessThan(Value, Outcome) {
		public override bool Test(Part part) => part.S < Value;
	}
}
abstract record GreaterThan(long Value, string Outcome) : Branch(Outcome) {
	public record X(long Value, string Outcome) : GreaterThan(Value, Outcome) {
		public override bool Test(Part part) => part.X > Value;
	}
	public record M(long Value, string Outcome) : GreaterThan(Value, Outcome) {
		public override bool Test(Part part) => part.M > Value;
	}
	public record A(long Value, string Outcome) : GreaterThan(Value, Outcome) {
		public override bool Test(Part part) => part.A > Value;
	}
	public record S(long Value, string Outcome) : GreaterThan(Value, Outcome) {
		public override bool Test(Part part) => part.S > Value;
	}
}

record Part(long X, long M, long A, long S) {
	public long Total => X + M + A + S;
}

record Range(Part Min, Part Max) {
	public long Count => (Max.X - Min.X + 1) * (Max.M - Min.M + 1) * (Max.A - Min.A + 1) * (Max.S - Min.S + 1);
}
