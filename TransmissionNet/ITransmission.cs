using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransmissionNet {
	public interface ITransmission {
		TorrentAdded AddTorrent(Uri url);
		IList<TorrentStatus> CheckStatus();
		void RemoveTorrent(int id);
	}
}
