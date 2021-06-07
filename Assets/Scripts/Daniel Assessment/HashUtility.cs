using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HashUtility
{
    /// <summary>
    /// Converts a passed string into an integer hash and returns it.
    /// </summary>
    /// <param name="text">The string to be converted into a hash.</param>
    /// <returns>The hashed string as an integer.</returns>
    public static int HashString(string text)
    {
        unchecked
        {
            int hash = 23;
            foreach (char c in text)
            {
                hash = hash * 31 + c;
            }
            return hash;
        }
    }
}
