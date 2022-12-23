var lines = await File.ReadAllLinesAsync("Input/input.txt");

var inputs = lines
    .Select(l => new Number
    {
        Value = int.Parse(l)// * 811_589_153L
    })
    .ToList();

var isUnique = inputs.Distinct().Count() == inputs.Count;


var sequence = new List<Number>(inputs);

//for (int i = 0; i < 10; i++)
//{
    Mix();
//}
var n1 = GetNthNumberAfterZero(1000);
var n2 = GetNthNumberAfterZero(2000);
var n3 = GetNthNumberAfterZero(3000);
var resultP1 = n1 + n2 + n3;
Console.WriteLine("P1: " + resultP1);

long GetNthNumberAfterZero(int n)
{
    var zero = inputs.Single(i => i.Value == 0);
    var zeroIndex = sequence.IndexOf(zero);
    return sequence[(zeroIndex + n) % sequence.Count].Value;
}

void Mix()
{
    for (int i = 0; i < inputs.Count; i++)
    {
        var input = inputs[i];
        var sequenceIndex = sequence.IndexOf(input);
        var newIndex = (sequenceIndex + input.Value) % (inputs.Count - 1);
        if (newIndex < 0)
            newIndex = inputs.Count - 1 + newIndex;

        sequence.Remove(input);
        sequence.Insert((int)newIndex, input);
    }
}

class Number
{
    public long Value { get; set; }
}