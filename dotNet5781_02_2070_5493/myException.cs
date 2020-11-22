using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2070_5493
{
    /// <summary>
    /// Exception class for our program.
    /// </summary>
    [Serializable]
    class myException : Exception
    {
        public myException() : base() { } // Default constructor.
        public myException(string message) : base(message) { } // Constructor with a message.
        public myException(string message, Exception inner) : base(message, inner) { } 
        // Constructor with a message and an exception.
        
        override public string ToString()
        {
            return "ERROR: " + Message + "\n";
        }

    }
}
