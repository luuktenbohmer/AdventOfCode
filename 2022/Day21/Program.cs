using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync("Input/input.txt");

var monkeys = new List<Monkey>();
foreach (var line in lines)
{
    var firstSplit = line.Split(": ");
    if (int.TryParse(firstSplit[1], out var number))
    {
        monkeys.Add(new NumberMonkey(firstSplit[0], number));
        continue;
    }

    var regex = new Regex(@"([a-z]{4}) ([*/\-+]) ([a-z]{4})");
    var regexResult = regex.Match(firstSplit[1]);
    if (!regexResult.Success)
        throw new Exception();

    var op = regexResult.Groups[2].Value[0];

    monkeys.Add(new MathOperationMonkey(
        firstSplit[0],
        op,
        regexResult.Groups[1].Value,
        regexResult.Groups[3].Value
    ));
}

monkeys.ForEach(m => m.SetRequiredMonkeys(monkeys));
var rootMonkey = (MathOperationMonkey)monkeys.Single(m => m.Name == "root");

// P1
var resultP1 = rootMonkey.GetNumber();
Console.WriteLine("P1: " + resultP1);

// P2
rootMonkey.Operator = '=';
var human = (NumberMonkey)monkeys.Single(m => m.Name == "humn");
var leftBranches = rootMonkey.LeftMonkey.GetBranch();
var rightBranches = rootMonkey.RightMonkey.GetBranch();
long resultP2 = -1;
if (rootMonkey.RightMonkey.GetBranch().Contains(human) || rootMonkey.RightMonkey == human)
{
    var desiredAnswer = rootMonkey.LeftMonkey.GetNumber();
    resultP2 = rootMonkey.RightMonkey.GetInputForAnswer(desiredAnswer, human);

}
else if (rootMonkey.LeftMonkey.GetBranch().Contains(human) || rootMonkey.LeftMonkey == human)
{
    var desiredAnswer = rootMonkey.RightMonkey.GetNumber();
    resultP2 = rootMonkey.LeftMonkey.GetInputForAnswer(desiredAnswer, human);
}
else
{
    throw new Exception();
}


Console.WriteLine("P2: " + resultP2);

Console.ReadKey();

public abstract class Monkey
{
    public Monkey(string name)
    {
        Name = name;
    }
    public string Name { get; set; }

    public List<Monkey> RequiredMonkeys { get; set; } = new();

    public abstract long GetNumber();
    public virtual void SetRequiredMonkeys(List<Monkey> monkeys) { }
    public virtual long GetInputForAnswer(long desiredAnswer, Monkey known)
    {
        if (Name == "humn")
            return desiredAnswer;

        throw new Exception();
    }
    public List<Monkey> GetBranch()
    {
        return RequiredMonkeys.SelectMany(r => r.GetBranch()).Concat(RequiredMonkeys).ToList();
    }
}

public class NumberMonkey : Monkey
{
    public NumberMonkey(string name, int number) : base(name)
    {
        Number = number;
    }

    public long Number { get; set; }

    public override long GetNumber()
    {
        return Number;
    }
}

public class MathOperationMonkey : Monkey
{
    private readonly string leftMonkeyName;
    private readonly string rightMonkeyName;
    public Monkey LeftMonkey;
    public Monkey RightMonkey;

    public MathOperationMonkey(string name, char op, string leftMonkey, string rightMonkey) : base(name)
    {
        Operator = op;
        leftMonkeyName = leftMonkey;
        rightMonkeyName = rightMonkey;
    }

    public char Operator { get; set; }

    public override void SetRequiredMonkeys(List<Monkey> monkeys)
    {
        LeftMonkey = monkeys.Single(m => m.Name == leftMonkeyName);
        RightMonkey = monkeys.Single(m => m.Name == rightMonkeyName);
        RequiredMonkeys.Add(LeftMonkey);
        RequiredMonkeys.Add(RightMonkey);
    }

    public override long GetNumber()
    {
        return Operator switch
        {
            '*' => LeftMonkey.GetNumber() * RightMonkey.GetNumber(),
            '/' => LeftMonkey.GetNumber() / RightMonkey.GetNumber(),
            '-' => LeftMonkey.GetNumber() - RightMonkey.GetNumber(),
            '+' => LeftMonkey.GetNumber() + RightMonkey.GetNumber(),
            '=' => LeftMonkey.GetNumber() == RightMonkey.GetNumber() ? 1 : 0,
            _ => throw new UnreachableException()
        };
    }

    public override long GetInputForAnswer(long desiredAnswer, Monkey unknown)
    {
        var branches = LeftMonkey.GetBranch();
        var leftUnknown = branches.Contains(unknown) || LeftMonkey == unknown;

        if (Operator == '*')
        {
            if (leftUnknown)
            {
                var desiredNextAnswer = desiredAnswer / RightMonkey.GetNumber();
                return LeftMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
            else
            {
                var desiredNextAnswer = desiredAnswer / LeftMonkey.GetNumber();
                return RightMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
        }
        else if (Operator == '/')
        {
            if (leftUnknown)
            {
                var desiredNextAnswer = desiredAnswer * RightMonkey.GetNumber();
                return LeftMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
            else
            {
                var desiredNextAnswer = LeftMonkey.GetNumber() / desiredAnswer;
                return RightMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
        }
        else if (Operator == '+')
        {
            if (leftUnknown)
            {
                var desiredNextAnswer = desiredAnswer - RightMonkey.GetNumber();
                return LeftMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
            else
            {
                var desiredNextAnswer = desiredAnswer - LeftMonkey.GetNumber();
                return RightMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
        }
        else if (Operator == '-')
        {
            if (leftUnknown)
            {
                var desiredNextAnswer = desiredAnswer + RightMonkey.GetNumber();
                return LeftMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
            else
            {
                var desiredNextAnswer = LeftMonkey.GetNumber() - desiredAnswer;
                return RightMonkey.GetInputForAnswer(desiredNextAnswer, unknown);
            }
        }
        else
        {
            throw new Exception();
        }
    }
}