using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransmissionNet {
	public interface ITransmission {
		TorrentAdded AddTorrent(Uri url);
		IList<TorrentStatus> CheckStatus();
		void RemoveTorrent(int id);
		void StopTorrent(int id);
		void StartTorrent(int id);
		void AddTracker(int id, string tracker);
		void RemoveTracker(int id, int trackerId);
	}
}
