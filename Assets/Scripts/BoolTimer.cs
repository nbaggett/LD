using UnityEngine;
/// <summary>
/// Sets a bool to true for a certain amount of time, then resets
/// </summary>
public struct BoolTimer
{
    float resetTime;

    /// <summary>
    /// Set the value to true for time seconds
    /// </summary>
    public void Set(float time)
    {
        resetTime = Mathf.Max(resetTime, Time.time + time);
    }

    /// <summary>
    /// Set the value to true for time seconds
    /// </summary>
    public void Set(float time, bool overwrite)
    {
        if (overwrite)
            resetTime = Time.time + time;
        else
            resetTime = Mathf.Max(resetTime, Time.time + time);
    }

    /// <summary>
    /// Set the value to false and reset timer
    /// </summary>
    public void Reset()
    {
        resetTime = Time.time - 1;
    }

    public bool Value { get { return Time.time <= resetTime; } }

    public static implicit operator bool(BoolTimer bt)
    {
        return bt.Value;
    }
}