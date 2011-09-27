using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransmissionNet {
	public class TransmissionResponse<T> where T : TransmissionArguments {
		public string result;
		public T arguments;
	}

	public class TransmissionArguments {
	}
}
