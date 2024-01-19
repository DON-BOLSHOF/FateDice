using System;
using UnityEngine;

namespace BKA.System.Exceptions
{
    public class SynchronizationException : Exception
    {
        public SynchronizationException(string message = default)
        {
            Debug.LogError($"Synchronization exception of data: {message}");
        }
    }
}