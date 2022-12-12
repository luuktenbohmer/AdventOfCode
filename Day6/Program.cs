// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var input = await File.ReadAllTextAsync("Input/Packet.txt");
var resultP1 = GetIndexOfDistinctCharacters(4);
var resultP2 = GetIndexOfDistinctCharacters(14);
Console.WriteLine("P1: " + resultP1);
Console.WriteLine("P2: " + resultP2);

Console.ReadKey();

int GetIndexOfDistinctCharacters(int numberOfDistinctCharacters)
{
    for (int i = 0; i < input.Length - (numberOfDistinctCharacters - 1); i++)
    {
        var distinctCharacters = input[i..(i + numberOfDistinctCharacters)].Distinct().Count() == numberOfDistinctCharacters;
        if (distinctCharacters)
        {
            return i + numberOfDistinctCharacters;
        }
    }

    throw new UnreachableException("There should always be a solution");
}