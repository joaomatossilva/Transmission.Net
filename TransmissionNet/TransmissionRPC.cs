using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace TransmissionNet {
	public class TransmissionRPC :ITransmission {

		private Uri endpoint;
		private string session_id = string.Empty;

		public TransmissionRPC(string host, int port, string path) {
			endpoint = new Uri(string.Format("http://{0}:{1}/{2}", host, port, path));
		}

		public TransmissionRPC(Uri endpoint) {
			this.endpoint = endpoint;
		}

		public TorrentAdded AddTorrent(Uri torrentUrl) {
			var request = new { method = "torrent-add", arguments = new { filename = torrentUrl.ToString(), paused = true } };
			string requestStr = JsonConvert.SerializeObject(request);

			string responseStr = Invoke(requestStr);

			//huge Hack
			responseStr = responseStr.Replace("torrent-added", "torrentadded");
			var response = JsonConvert.DeserializeObject<TransmissionResponse<TorrentAddedArgument>>(responseStr);
			return response.arguments.torrentadded;
		}

		public IList<TorrentStatus> CheckStatus() {
			var request = new { method = "torrent-get", arguments = new { fields = new string[] { "id", "name", "status", "downloadDir", "isFinished", "isStalled" } } };
			string requestStr = JsonConvert.SerializeObject(request);

			string responseStr = Invoke(requestStr);
			var response = JsonConvert.DeserializeObject<TransmissionResponse<TorrentStatusArgument>>(responseStr);
			return response.arguments.torrents;
		}

		public void RemoveTorrent(int id) {
			var request = new { method = "torrent-remove", arguments = new { ids = new int[] { id } } };
			string requestStr = JsonConvert.SerializeObject(request);

			string responseStr = Invoke(requestStr);
			var response = JsonConvert.DeserializeObject<TransmissionResponse<TransmissionArguments>>(responseStr);
			int i = 0;
		}

		private string Invoke(string request) {

			WebClient client = new WebClient();
			client.Headers.Add("X-Transmission-Session-Id", session_id);
			byte[] data =  System.Text.Encoding.UTF8.GetBytes(request);
			byte[] response = null;
			try {
				response = client.UploadData(endpoint.ToString(), "POST", data);
			} catch (WebException ex) {
				var httpResponse = (HttpWebResponse) ex.Response;
				if (null != httpResponse && httpResponse.StatusCode == HttpStatusCode.Conflict) {
					session_id = httpResponse.Headers["X-Transmission-Session-Id"];
					client.Headers["X-Transmission-Session-Id"] = session_id;
					response = client.UploadData(endpoint.ToString(), "POST", data);
				} else {
					throw;
				}
			}

			return System.Text.Encoding.UTF8.GetString(response);
		}

	}
}
