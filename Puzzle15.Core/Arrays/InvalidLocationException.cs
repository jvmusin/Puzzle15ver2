using System;

namespace Puzzle15.Core.Arrays
{
	public class InvalidLocationException : Exception
	{
		public InvalidLocationException() : this("")
		{
		}

		public InvalidLocationException(string message) : base(message)
		{
		}
	}
}