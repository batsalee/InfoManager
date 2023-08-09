using System;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace IMClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			while (true) {
				// 1. 태그 입력받음
				string inputTag;
				Console.Write ("태그인식 : ");
				inputTag = Console.ReadLine ();

				// 00001234 같은 경우 1234로 만들기 위해
				int temp = int.Parse (inputTag);
				inputTag = temp.ToString ();

				// 2. Socket()
				Socket clntSock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				//
				Console.WriteLine ("소켓생성 성공");

				// 3. Connect()
				clntSock.Connect ("192.168.11.1", 1105);

				//
				Console.WriteLine ("윈 서버와 연결 성공");

				// 4. RFID Tag 전송
				byte[] sendBuf = Encoding.ASCII.GetBytes(inputTag);
				clntSock.Send(sendBuf);

				// 5. 결과 wav파일 전송받기

				// 데이터를 받을 버퍼 생성 
				byte[] buffer = new byte[4];
				// 클라이언트로부터 파일 크기를 받음 
				clntSock.Receive (buffer);
				// 받은 데이터를 정수로 변환하고 변수에 저장 
				int fileLength = BitConverter.ToInt32 (buffer, 0);
				// 버퍼 크기 새로 지정 
				buffer = new byte[1024];
				// 현재까지 받은 파일 크기 변수 
				int totalLength = 0;

				// Console.WriteLine ("파일만들기 전");

				// 파일을 만듦 
				string fileName = inputTag + ".wav";
				FileStream fileStr = new FileStream (fileName, FileMode.Create, FileAccess.Write);
				// 받을 데이터를 파일에 쓰기 위해 BinaryWriter 객체 생성 
				BinaryWriter writer = new BinaryWriter (fileStr);

				// Console.WriteLine ("파일만든 후");


				// 파일 수신 작업 
				while (totalLength < fileLength) {
					// 클라이언트가 보낸 파일 데이터를 받음 
					int receiveLength = clntSock.Receive (buffer);
					// 받은 데이터를 파일에 씀 
					writer.Write (buffer, 0, receiveLength);
					// 현재까지 받은 파일 크기를 더함 
					totalLength += receiveLength;
				}

				//
				Console.WriteLine ("파일전송 성공");

				// 파일 실행
				ProcessStartInfo startInfo = new ProcessStartInfo ();
				startInfo.FileName = "aplay";
				startInfo.Arguments = fileName;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;

				Process process = new Process ();
				process.StartInfo = startInfo;
				process.Start ();
				process.WaitForExit ();


				// 종료 작업 
				process.Close ();
				writer.Close ();
				clntSock.Close ();
			}
		}
	}
}
