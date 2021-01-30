public class MinMax<T>
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public float Range { get { return Max - Min; } }
    public float Count { get { return Range + 1; } }

    public MinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    /// <summary>
    /// Defines bounds along the new value
    /// </summary>
    /// <param name="value">The new value</param>

    public void Evaluate(float value)
    {
        if (value < Min) Min = value;
        if (value > Max) Max = value;
    }
}

public class MinMaxInt
{
    public int Min { get; private set; }
    public int Max { get; private set; }

    public int Range { get { return Max - Min; } }
    public int Count { get { return Range + 1; } }

    public MinMaxInt()
    {
        Min = int.MaxValue;
        Max = int.MinValue;
    }

    /// <summary>
    /// Defines bounds along the new value
    /// </summary>
    /// <param name="value">The new value</param>
    public void Evaluate(int value)
    {
        if (value < Min) Min = value;
        if (value > Max) Max = value;
    }
}
