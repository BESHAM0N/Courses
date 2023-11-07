using System;

public static class Extensions
{
    public static Coordinate ToCoordinate(this (int x, int y) value)
    {
        return new Coordinate(value.x, value.y);
    }
    
    public static string ToSerializable(this string value, GameSide side, Command command, string destination = "")
    {
        var gameSide = side.CurrentSide == ColorType.Black ? "1" : "2";

        switch (command)
        {
            case Command.Click:
                return $"Player {gameSide} {command} to {value}";
        
            case Command.Move:
                return $"Player {gameSide} {command} from {value} to {destination}";

            case Command.Remove:
                return $"Player {gameSide} {command} checker at {value}";

            default:
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
        }
    }
}
