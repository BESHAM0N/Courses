public  struct Coordinate
{
    public readonly int X;
    public readonly int Y;

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    public override string ToString()
    {
        return $"{X}, {Y}";
    }
    
    public string ToHumanString()
    {
        return $"{X + 1} {Y + 1}";
    }
}
