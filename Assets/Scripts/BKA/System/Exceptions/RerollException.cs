using System;
using UnityEngine;

namespace BKA.System.Exceptions
{
    public class RerollException : Exception
    {
        public RerollException()
        {
            Debug.LogError("Dices is not ready!");
        }
    }
}