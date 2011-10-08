using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.IO;

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
			var filename = torrentUrl.ToString();
			if (torrentUrl.Scheme.Equals("file")) {
				filename = torrentUrl.OriginalString;
				return AddTorrentInternalFromFile(filename);
			}  else {
				return AddTorrentInternalFromWeb(filename);
			}
			
		}

		private TorrentAdded AddTorrentInternalFromWeb(string url) {
			var request = new { method = "torrent-add", arguments = new { filename = url } };
			var response = Invoke<TorrentAddedArgument>(request);
			return response.torrentadded;
		}

		private TorrentAdded AddTorrentInternalFromFile(string filePath) {

			byte[] data = File.ReadAllBytes(filePath);
			var metaInfo = Base64Encode(data);

			var request = new { method = "torrent-add", arguments = new { metainfo = metaInfo } };
			var response = Invoke<TorrentAddedArgument>(request);
			return response.torrentadded;
		}

		public IList<TorrentStatus> CheckStatus() {
			var request = new { method = "torrent-get", arguments = new { fields = new string[] { "id", "name", "status", "downloadDir", "isFinished", "isStalled", "files", "percentDone", "torrentFile", "trackers", "hashString" } } };
			var response = Invoke<TorrentStatusArgument>(request);
			return response.torrents;
		}

		public void RemoveTorrent(int id) {
			var request = new { method = "torrent-remove", arguments = new { ids = new int[] { id } } };
			var response = Invoke<TransmissionArguments>(request);
		}

		public void StopTorrent(int id) {
			var request = new { method = "torrent-stop", arguments = new { ids = new int[] { id } } };
			var response = Invoke<TransmissionArguments>(request);
		}

		public void StartTorrent(int id) {
			var request = new { method = "torrent-start", arguments = new { ids = new int[] { id } } };
			var response = Invoke<TransmissionArguments>(request);
		}

		public void AddTracker(int id, string tracker) {
			var request = new {method = "torrent-set", arguments = new {ids = new int[] {id}, trackerAdd = new string[] { tracker } }};
			Invoke<TransmissionArguments>(request);
		}

		public void RemoveTracker(int id, int trackerId) {
			var request = new { method = "torrent-set", arguments = new { ids = new int[] { id }, trackerRemove = new int[] { trackerId } } };
			Invoke<TransmissionArguments>(request);
		}

		private T Invoke<T>(object requestObject) where T: TransmissionArguments {
			string request = JsonConvert.SerializeObject(requestObject);
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
					throw new TransmissionException("Unable to connet to server.", ex);
				}
			}

			var responseStr = System.Text.Encoding.UTF8.GetString(response);
			
			//huge Hack
			if (typeof(T) == typeof(TorrentAddedArgument)) {
				responseStr = responseStr.Replace("torrent-added", "torrentadded");
			}
			//end hack

			var responseObject = JsonConvert.DeserializeObject<TransmissionResponse<T>>(responseStr);
			CheckResultSuccess(responseObject.result);
			return responseObject.arguments;
		}

		private void CheckResultSuccess(string resultMessage) {
			if (!resultMessage.Equals("success", StringComparison.InvariantCultureIgnoreCase)) {
				throw new TransmissionException(resultMessage);
			}
		}

		private static string Base64Encode(byte[] data) {
			try {
				string encodedData = Convert.ToBase64String(data);
				return encodedData;
			} catch (Exception e) {
				throw new Exception("Error in base64Encode" + e.Message);
			}
		}
	}
}
