using System.Diagnostics;

namespace Day13;

public class Packet
{
    public Packet(string rawPacket)
    {
        Parse(rawPacket);
        RawPacket = rawPacket;
    }

    private void Parse(string rawPacket)
    {
        Items = new();
        ParseInternal(rawPacket[1..], Items);
    }

    private void ParseInternal(string rawPacket, ListItem? currentList)
    {
        if (currentList == null)
        {
            if (rawPacket.Length != 0)
                throw new UnreachableException();

            return;
        }
        var listOpenIndex = rawPacket.IndexOf('[');
        var listCloseIndex = rawPacket.IndexOf(']');

        // Both -1 -> End of string
        if (listOpenIndex == listCloseIndex)
            return;

        if ((listOpenIndex < listCloseIndex && listOpenIndex != -1) || listCloseIndex == -1)
        {
            var integers = rawPacket[..listOpenIndex]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Select(i => new IntegerItem(i));

            currentList.Items.AddRange(integers);

            var newList = new ListItem();
            currentList.Items.Add(newList);
            newList.Parent = currentList;
            ParseInternal(rawPacket[(listOpenIndex + 1)..], newList);
        }
        else if ((listCloseIndex < listOpenIndex && listCloseIndex != -1) || listOpenIndex == -1)
        {
            var integers = rawPacket[..listCloseIndex]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Select(i => new IntegerItem(i));

            currentList.Items.AddRange(integers);

            ParseInternal(rawPacket[(listCloseIndex + 1)..], currentList.Parent);
        }
        else
        {
            throw new UnreachableException($"Should not happen. Open: {listOpenIndex}, Close: {listCloseIndex}");
        }
    }

    public bool GreaterThan(Packet other) =>
        Items.GreaterThan(other.Items);

    public ListItem Items { get; set; } = new();
    public string RawPacket { get; }
}

public abstract class Item
{
    public ListItem? Parent { get; set; }
    public abstract bool GreaterThan(Item other);
    public abstract bool SimilarTo(Item other);
}

public class ListItem : Item
{
    public List<Item> Items { get; set; } = new();

    public override bool GreaterThan(Item other)
    {
        if (other is ListItem listOther)
        {
            for (int i = 0; i < Math.Min(Items.Count, listOther.Items.Count); i++)
            {
                if (Items[i].SimilarTo(listOther.Items[i]))
                    continue;

                return Items[i].GreaterThan(listOther.Items[i]);
            }

            return Items.Count > listOther.Items.Count;
        }
        else if (other is IntegerItem)
        {
            return GreaterThan(new ListItem { Items = new() { other } });
        }

        throw new UnreachableException();
    }

    public override bool SimilarTo(Item other)
    {
        if (other is ListItem listOther)
        {
            for (int i = 0; i < Math.Min(Items.Count, listOther.Items.Count); i++)
            {
                if (Items[i].SimilarTo(listOther.Items[i]))
                    continue;

                return false;
            }

            return Items.Count == listOther.Items.Count;
        }
        else if (other is IntegerItem)
        {
            return SimilarTo(new ListItem { Items = new() { other } });
        }

        throw new UnreachableException();
    }
}

public class IntegerItem : Item
{
    public IntegerItem(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public override bool GreaterThan(Item other)
    {
        if (other is IntegerItem integerOther)
        {
            return Value > integerOther.Value;
        }
        else if (other is ListItem listOther)
        {
            return new ListItem
            {
                Items = new() { new IntegerItem(Value) }
            }.GreaterThan(listOther);
        }

        throw new UnreachableException();
    }

    public override bool SimilarTo(Item other)
    {
        if (other is IntegerItem integerOther)
        {
            return Value == integerOther.Value;
        }
        else if (other is ListItem listOther)
        {
            return new ListItem
            {
                Items = new() { new IntegerItem(Value) }
            }.SimilarTo(listOther);
        }

        throw new UnreachableException();
    }
}