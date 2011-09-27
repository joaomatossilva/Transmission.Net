using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TransmissionNet {

	public class TorrentAddedArgument : TransmissionArguments {
		public TorrentAdded torrentadded;
	}

	public class TorrentAdded {
		public string hashString;
		public int id;
		public string name;
	}
}
