using System;

namespace Dzaba.PhoneCleaner
{
	[Serializable]
	internal class ExitCodeException : Exception
	{
		public ExitCode ExitCode { get; }

		public ExitCodeException(ExitCode exitCode)
		{
			ExitCode = exitCode;
        }

		public ExitCodeException(string message, ExitCode exitCode)
			: base(message)
        {
            ExitCode = exitCode;
        }

        public ExitCodeException(string message, ExitCode exitCode, Exception inner)
			: base(message, inner)
        {
            ExitCode = exitCode;
        }
	}
}
