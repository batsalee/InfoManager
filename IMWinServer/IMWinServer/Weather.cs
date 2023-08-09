using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 날씨획득 xml
using System.Xml;

namespace IMClient
{
	class Weather
	{
		XmlNodeList weatherList;	// 날씨
		XmlNodeList tempList;		// 기온
		XmlNodeList popList;		// 강수확률
		XmlNodeList rehList;		// 습도
		
		public Weather() { }

		public void initWeather(UserInfo userInfo)
		{	
			// 지역에 따른 url문 구성
			string url = "http://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=" + userInfo.getLocationCode();
						
			// 기상청 제공 날씨 xml획득
			XmlDocument docX = new XmlDocument();
			docX.Load(url);

			// 문장구성
			weatherList = docX.GetElementsByTagName("wfKor");
			tempList = docX.GetElementsByTagName("temp");
			popList = docX.GetElementsByTagName("pop");
			rehList = docX.GetElementsByTagName("reh");
		}	

		public XmlNodeList getWeatherList() { return weatherList; }
		public XmlNodeList getTempList() { return tempList; }
		public XmlNodeList getPopList() { return popList; }
		public XmlNodeList getRehList() { return rehList; }
		public void setWeatherInfo(XmlNodeList weatherList, XmlNodeList tempList, XmlNodeList popList, XmlNodeList rehList)
		{ this.weatherList = weatherList; this.tempList = tempList; this.popList = popList; this.rehList = rehList; }
	}
}