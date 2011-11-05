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
		public Trackers[] trackers;
		public string hashString;
	}

	public class TorrentFiles {
		public string name;
		public long bytesCompleted;
		public long lenght;
	}

	public class Trackers {
		public string announce;
		public int id;
		public string scrape;
		public int tier;
	}
}
