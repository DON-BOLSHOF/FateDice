using System;
using UnityEngine;

namespace BKA.System.Exceptions
{
    public class BootsTrapException : Exception
    {
        public BootsTrapException()
        {
            Debug.LogError("BootsTrap is not uploaded. Data is not ready to use!");
        }
    }
}