using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransmissionNet {

	public class TorrentStatusArgument : TransmissionArguments {
		public IList<TorrentStatus> torrents;
	}

	public class TorrentStatus {
		public int id;
		public int status;
		public string downloadDir;
		public string name;
		public bool isFinished;
		public bool isStalled;
	}
}
