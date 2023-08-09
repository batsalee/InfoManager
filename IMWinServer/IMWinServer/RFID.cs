using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMClient
{
	class RFID
	{
		string RFIDTag;

		public RFID() : this(null) {}
		public RFID(string RFIDTag)	{ this.RFIDTag = RFIDTag; }
		
		public void readRFIDTag()
		{
			RFIDTag = Console.ReadLine();
			setRFIDTag(RFIDTag);
		}

		public string getRFIDTag()
		{
			return RFIDTag;
		}

		public void setRFIDTag(string RFIDTag)
		{
			// 태그값이 000123인 경우 123으로 변경하기 위해
			int temp = int.Parse(RFIDTag);
			this.RFIDTag = temp.ToString(); 
		}
	}
}
