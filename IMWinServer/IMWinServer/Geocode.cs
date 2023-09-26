using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 위도경도 정보획득 xml
using System.Xml;

namespace IMClient
{
	class Geocode
	{
		string coordinates;
		XmlNodeList location;
		const string googleKey = ""; // GitGuardian Mail 전송 후 삭제조치

		public Geocode() { }

		public void initGeocode(string locationName)
		{
			string url = "https://maps.googleapis.com/maps/api/geocode/xml?address=" + locationName + "&key=" + googleKey;
			XmlDocument docX = new XmlDocument();
			docX.Load(url);

			location =  docX.GetElementsByTagName("location");
			this.coordinates = location[0].SelectNodes("lat")[0].InnerText + "," + location[0].SelectNodes("lng")[0].InnerText;
		}
		
		public string getCoordinates() { return coordinates; }
	}
}
