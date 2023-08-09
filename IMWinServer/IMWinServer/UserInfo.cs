using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMClient
{
	class UserInfo
	{
		string userName;
		string startLocationName;
		string arriveLocationName;
		string locationCode;	// arriveLocation의 지역코드
		string startLocationEName;
		string arriveLocationEName;

		public UserInfo() : this(null, null, null, null){ }
		public UserInfo(string userName, string startLocationName, string arriveLocationName, string locationCode)
		{
			this.userName = userName;
			this.startLocationName = startLocationName;
			this.arriveLocationName = arriveLocationName;
			this.locationCode = locationCode;
		}

		public string getUserName() { return userName; }
		public string getStartLocationName() { return startLocationName; }
		public string getArriveLocationName() { return arriveLocationName; }
		public string getLocationCode() { return locationCode; }
		public string getStartLocationEName() { return startLocationEName; }
		public string getArriveLocationEName() { return arriveLocationEName; }

		public void setUserInfo(string resCommunicate)
		{
			string[] userInfo = resCommunicate.Split('\0');
			userName = userInfo[0];
			startLocationName = userInfo[1];
			arriveLocationName = userInfo[2];
			locationCode = userInfo[3];
			startLocationEName = userInfo[4];
			arriveLocationEName = userInfo[5];
		}
	}
}
