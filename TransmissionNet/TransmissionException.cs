using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransmissionNet {
	public class TransmissionException : Exception {
		public TransmissionException(string message)
			: base(message) {
		}

		public TransmissionException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
