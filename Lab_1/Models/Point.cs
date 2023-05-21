namespace Lab_1.Models;

public class Point
{
    public int X_MaxAllowed;
    public int Y_MaxAllowed;
    public int X_ByteRepresentationLength;
    public int Y_ByteRepresentationLength;

    public float X_RangeStart;
    public float X_RangeEnd;
    public float Y_RangeStart;
    public float Y_RangeEnd;
    public float Step;
    public float F;

    public string X_Bytes;
    public string Y_Bytes;

    public Point(float xRangeStart, float xRangeEnd, float yRangeStart, float yRangeEnd, float step,
                 int xByteRepresentationLength, int yByteRepresentationLength)
    {
        X_RangeStart = xRangeStart;
        X_RangeEnd = xRangeEnd;
        Y_RangeStart = yRangeStart;
        Y_RangeEnd = yRangeEnd;
        Step = step;
        X_ByteRepresentationLength = xByteRepresentationLength;
        Y_ByteRepresentationLength = yByteRepresentationLength;
        X_MaxAllowed = (int)((xRangeEnd - xRangeStart) / step);
        Y_MaxAllowed = (int)((yRangeEnd - yRangeStart) / step);
    }

    public Point(Point point)
    {
        X_RangeStart = point.X_RangeStart;
        X_RangeEnd = point.X_RangeEnd;
        Y_RangeStart = point.Y_RangeStart;
        Y_RangeEnd = point.Y_RangeEnd;
        Step = point.Step;
        X_ByteRepresentationLength = point.X_ByteRepresentationLength;
        Y_ByteRepresentationLength = point.Y_ByteRepresentationLength;
        X_MaxAllowed = (int)((X_RangeEnd - X_RangeStart) / Step);
        Y_MaxAllowed = (int)((Y_RangeEnd - Y_RangeStart) / Step);
    }

    public void Set_X_Bytes(string s, string previousValue)
    {
        var num = X_RangeStart + (Step * Convert.ToInt32(s, 2));
        if (num > X_RangeEnd || num < X_RangeStart)
        {
            X_Bytes = previousValue;
            return;
        }
        X_Bytes = s;
    }

    public void Set_Y_Bytes(string s, string previousValue)
    {
        var num = Y_RangeStart + (Step * Convert.ToInt32(s, 2));
        if (num > Y_RangeEnd || num < Y_RangeStart)
        {
            Y_Bytes = previousValue;
            return;
        }
        Y_Bytes = s;
    }

    public float Get_X()
    {
        int xInt = Convert.ToInt32(X_Bytes, 2);
        return X_RangeStart + (Step * xInt);
    }

    public float Get_Y()
    {
        int yInt = Convert.ToInt32(Y_Bytes, 2);
        return Y_RangeStart + (Step * yInt);
    }

    public void Set_X(float x)
    {
        if (x > X_RangeEnd || x < X_RangeStart)
        {
            throw new ArgumentException();
        }
        int xInt = (int)((x - X_RangeStart) / Step);
        string s = Convert.ToString(xInt, 2);
        X_Bytes = s.PadLeft(X_ByteRepresentationLength, '0');
    }

    public void Set_Y(float y)
    {
        if (y > Y_RangeEnd || y < Y_RangeStart)
        {
            throw new ArgumentException();
        }
        int yInt = (int)((y - Y_RangeStart) / Step);
        string s = Convert.ToString(yInt, 2);
        Y_Bytes = s.PadLeft(X_ByteRepresentationLength, '0');
    }
}
