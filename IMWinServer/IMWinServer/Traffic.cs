using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 교통정보획득 xml
using System.Xml;

namespace IMClient
{
	class Traffic
	{		
		// 시작지역, 도착지역, 걸리는 시간
		XmlNodeList duration;
		const string googleKey = "AIzaSyAkM6C1d9pbg6hYJHOFRo72vNpFzHNwGU8";
		// 사고정보, 실시간 정보 등
		//.....

		public Traffic() { }

		public void initTraffic(string start_location, string end_location)
		{
			// 출발지역과 도착지역까지의 교통정보를 얻기 위한 url문 구성
			string url = "https://maps.googleapis.com/maps/api/directions/xml?origin=" + start_location + "&destination=" + end_location + "&mode=transit&key=" + googleKey;

			XmlDocument docX = new XmlDocument();
			docX.Load(url);

			duration = docX.GetElementsByTagName("duration");
		}
		
		public XmlNodeList getDurationList() { return duration; }
	}
}