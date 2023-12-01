namespace Day2;

public static class RPCFactory
{
    public static RPC GetRPC(string rpc)
    {
        return rpc switch
        {
            "A" or "X" => RPC.Rock,
            "B" or "Y" => RPC.Paper,
            "C" or "Z" => RPC.Scissors,
            _ => throw new Exception($"Invalid rpc input: {rpc}")
        };
    }

    public static Result GetResult(string result)
    {
        return result switch
        {
            "X" => Result.Lose,
            "Y" => Result.Draw,
            "Z" => Result.Win,
            _ => throw new Exception($"Invalid result input: {result}")
        };
    }
}
