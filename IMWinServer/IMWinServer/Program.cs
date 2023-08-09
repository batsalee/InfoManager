// C# 프로젝트 namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 날씨획득 XmlNodeList 타입
using System.Xml;
// IP와 PORT를 app.config로 지정하기 위해
using System.Configuration;
// 네트워크 통신 namespace
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace IMClient
{
	class Program
	{
		static Thread th;

		static void Main(string[] args)
		{
			///////////// 서버오픈 ////////////////// 			
			// 1. 서버 socket 생성
			Socket servSock = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			// 2. binding
			servSock.Bind(new IPEndPoint(IPAddress.Any,1105));
			// 3. listen
			servSock.Listen(10);
			/////////////////////////////////////////
//
			Console.WriteLine("서버오픈");

			while(true)
			{
				// 4. accept : 클라이언트(라즈베리파이)와 연결
				Socket clntSock = servSock.Accept();
//
				Console.WriteLine("클라이언트와 연결");

				th = new Thread(delegate () { Func(clntSock); });
				th.Start();
			}

			// 10. 클라이언트와 통신 종료
			//communicator.endCommunicate();
		}

		public static void Func(Socket clntSock)
		{			
			byte[] recvBytes = new byte[1024];
			int nRecv;
		
			// 5. 클라이언트에게서 RFID태그값 받음
			nRecv = clntSock.Receive(recvBytes);
			string RFIDTag = Encoding.UTF8.GetString(recvBytes,0,nRecv);

			// 리눅스서버(DB)에 연결
			Socket linuxSock = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp); // 리눅스 서버와 통신소켓
			linuxSock.Connect("192.168.11.9",1105); // 리눅스 서버와 연결 

			//
			Console.WriteLine("리눅스소켓 생성");

			// 리눅스서버에게 RFID태그 전송
			byte[] sendBuf = Encoding.ASCII.GetBytes(RFIDTag);
			int nSend = linuxSock.Send(sendBuf);

			// 리눅스(DB)에게 유저정보 받기
			nRecv = linuxSock.Receive(recvBytes);
			string recvUserInfo = Encoding.UTF8.GetString(recvBytes,0,nRecv);

			//
			Console.WriteLine("사용자정보 받앗음");

			// 3. 사용자정보가공
			UserInfo userInfo = new UserInfo();
			userInfo.setUserInfo(recvUserInfo);
			if(userInfo.getLocationCode() == "0000000000") th.Abort();    // DB에 값이 없거나 DB오류일 때

			// 4. 교통 정보 획득
			Geocode startGeocoder = new Geocode();  // 시작지역의 위도/경도 구하기
			startGeocoder.initGeocode(userInfo.getStartLocationEName());
			string startCoordinates = startGeocoder.getCoordinates();
			Geocode endGeocoder = new Geocode();    // 도착지역의 위도/경도 구하기
			endGeocoder.initGeocode(userInfo.getArriveLocationEName());
			string endCoordinates = endGeocoder.getCoordinates();

			Traffic traffic = new Traffic();
			traffic.initTraffic(startCoordinates,endCoordinates); // 위도/경도를 이용해 이동시간 계산

			// 5. 교통 문장 구성
			XmlNodeList duration = traffic.getDurationList();
			int tagCount = duration.Count;
			int durationValue = int.Parse(traffic.getDurationList()[tagCount - 1].SelectNodes("value")[0].InnerText);
			int durationHour = durationValue / 3600;
			int durationMin = durationValue % 3600 / 60;

			string trafficText = userInfo.getStartLocationName() + "에서 " + userInfo.getArriveLocationName() + "까지 소요시간은 " + durationHour + "시간 " + durationMin + "분이며 ";

			// 6. 날씨 정보 획득
			Weather weather = new Weather();
			weather.initWeather(userInfo);

			// 7. 날씨 문장 구성
			int index = durationHour / 3;
			string weatherText = "오늘 " + userInfo.getArriveLocationName() + "의 날씨는 " + weather.getWeatherList()[index].InnerText +
				" 기온은 " + weather.getTempList()[index].InnerText + "도 강수확률은 " + weather.getPopList()[index].InnerText +
				"퍼센트 습도는 " + weather.getRehList()[index].InnerText + "퍼센트입니다";

			// 8. tts 파일 생성	
			TTS tts = new TTS();
			tts.initTTS(RFIDTag);
			string defaultText = userInfo.getUserName() + "님 안녕하십니까 ";
			tts.attachTTS(defaultText);
			tts.attachTTS(trafficText);
			tts.attachTTS(weatherText);
			tts.disposeTTS();

			//
			Console.Write(defaultText + trafficText + weatherText);

			// 9. 완성된 tts파일 전송
			string path = @"C:\Users\이동현\Desktop\졸작발표\" + RFIDTag + ".wav";
			// 파일을 엶 
			FileStream fileStr = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			// 파일 크기를 가져옴 
			int fileLength = (int)fileStr.Length;
			// 파일 크기를 서버에 전송하기 위해 바이트 배열로 변환 
			byte[] buffer = BitConverter.GetBytes(fileLength);
			// 파일 크기 전송 
			clntSock.Send(buffer);
			// 파일을 보낼 횟수 
			int count = fileLength / 1024 + 1;
			// 파일을 읽기 위해 BinaryReader 객체 생성 
			BinaryReader reader = new BinaryReader(fileStr);

			// 파일 송신 작업 
			for(int i = 0;i < count;i++)
			{
				// 파일을 읽음 
				buffer = reader.ReadBytes(1024);
				// 읽은 파일을 서버로 전송 
				clntSock.Send(buffer);
			}

			fileStr.Close();
			reader.Close();
			linuxSock.Close();
			clntSock.Close();
		}
	}
}