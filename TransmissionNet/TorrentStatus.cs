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

		public double percentDone;
		public TorrentFiles[] files;
		public string torrentfile;
	}

	public class TorrentFiles {
		public string name;
		public int bytesCompleted;
		public int lenght;
	}
}
