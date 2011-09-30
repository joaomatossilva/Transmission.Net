using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TransmissionNet;
using Argotic.Syndication;
using Argotic.Extensions;

namespace TransmissionNet.Dev {
	class Program {
		static void Main(string[] args) {

			
			TransmissionRPC rpc = new TransmissionRPC(new Uri("http://127.0.0.1:9091/transmission/rpc"));
			/*
			var request = new { method = "torrent-get", arguments = new { ids = "\"recently-active\"", fields = new string[] {"id", "downloadedEver", "error", "errorString", "eta", "haveUnchecked", "haveValid", "leftUntilDone", "metadataPercentComplete", "rateDownload", "rateUpload", "recheckProgress", "status", "downloadDir", "isFinished"} } };
			string requestStr = JsonConvert.SerializeObject(request);

			string responseStr = rpc.Invoke(requestStr);
			*/

			//var returnObj = rpc.AddTorrent(new Uri("http://redirect.karmorra.info/6d7f809d36a2cfacdcd4b450c6239212.torrent"));


			//rpc.RemoveTorrent(2);

			var returnObj2 = rpc.CheckStatus();



			//JsonConvert.DeserializeAnonymousType(responseStr, )

			
			int i = 0;
			 
			/*
			RssFeed feed = RssFeed.Create(new Uri("http://showrss.karmorra.info/rss.php?user_id=69251&hd=0&proper=0&namespaces=true"), new Argotic.Common.SyndicationResourceLoadSettings { 
				RetrievalLimit = 10, 
				AutoDetectExtensions = true, 
			});
			feed.AddExtension(new ShowRssSyndicationExtension());

			foreach (var item in feed.Channel.Items) {
				int i = 0;
			}
			*/
		}
	}

	public class ShowRssSyndicationExtension : SyndicationExtension {

		public ShowRssContext Context { get; private set; }

		public ShowRssSyndicationExtension()
			: base("showrss", "http://showrss.karmorra.info/", new Version(1, 0), new Uri("http://showrss.karmorra.info/"), "Show RSS Extension", "Show RSS Extension") {
			this.Context = new ShowRssContext();
		}

		public override bool Load(System.Xml.XmlReader reader) {
			return true;
		}

		public override bool Load(System.Xml.XPath.IXPathNavigable source) {
			return true;
		}

		public override void WriteTo(System.Xml.XmlWriter writer) {
			throw new NotImplementedException();
		}
	}

	public class ShowRssContext{
		public int ShowId { get; set; }
		public string ShowName { get; set; }
		public string Episode { get; set; }
	}
}
