# InfoManager
원하는 장소에 이 시스템을 설치 후 사용자가 접근시 해당 사용자를 자동으로 식별해 스케쥴에 맞는 날씨 및 교통정보를 음성으로 제공하는 시스템</br>

예를들어 아침에 처음 가는 곳이 냉장고라면 냉장고 옆에 이 시스템을 설치 후 매일 아침 사용자가 접근시 자동으로 해당 사용자를 식별하여 스케쥴에 맞는 목적지까지의 교통 정보 및 날씨 정보를 음성으로 알려주는 시스템</br>

---

# 작품 소개
- RFID 기반 자동 인식</br>
- 사용자가 원하는 장소에 시스템을 설치</br>
- 시스템이 설치된 장소에 접근 시 사용자를 자동 식별</br>
- 해당 사용자에게 날씨 및 교통 정보를 음성으로 제공</br>
- 시스템이 설치된 곳에선 어디서나 동일한 서비스 제공</br>

---

# 참고 사항
이 프로젝트는 제 대학교 졸업작품을 위해 제작된 프로젝트입니다.</br>

기획단계에서 졸업작품인 만큼 학교에서 배운 모든것을 써보자! 라는 각오로 기획했습니다.</br>
그래서 프로그래밍언어 뿐 아니라 네트워크나 데이터베이스, 그 외 장비사용까지 최대한 많은것을 사용해보고자 노력했고</br>
그 외에도 실제 프로젝트 진행이라면 모든것을 알고 진행할 수는 없기에 할줄 모르는 내용도 추가해야겠다고 생각해서 
당시 전혀 사용할 줄 몰랐던 C#을 공부해가면서 제작하게 되었습니다.</br>
그 외에도 사용해본적 없던 외부API나 당시 신규서비스에 가까웠던 TTS등을 사용해보게 되었습니다.</br>

그 결과 학교에서 배웠던 C언어, 데이터베이스, 프로젝트진행방법 등에 더해</br>
개인적으로 공부했던 네트워크 소켓 프로그래밍, Linux, C# 기초수준 등을 더해 제작했습니다.</br>

또한 프로그램 소스코드 작성에도 당시 알던 수준의 기법은 최대한 사용해보고자 했습니다.</br>
예를들어 여러 값을 리턴받아야 하는 경우 구조체를 만들어 리턴하거나, 레퍼런스로 값을 전달하거나, 벡터같은 선형자료구조로 리턴 받는 등 다양한 방법이 있는데 이런것들도 이것저것 사용해보고자 했습니다.</br>
그 결과 코드가 조금 지저분하거나 난잡해보일 수 있으나 졸업작품인 만큼 모든것을 사용해보고자 했던 의지로 봐주시면 좋을 것 같습니다.</br>

---

# 개발 내용
Client, Server, DB Server 3단 구성</br>

※ 특이 사항</br>
개발 당시 TTS서비스가 초창기시절이라 일부 제약이 있었기 때문에 현재 push되어 있는 프로젝트에는 아래 내용 중 Client의 역할 일부를 서버에서 처리 후 클라이언트로 전송합니다.</br>
당시 Linux기반 TTS이던 espeak를 사용하면 아래의 내용과 동일하게 프로젝트 구성을 할 수 있었으나 당시의 espeak는 한국어 TTS의 음질이나 Quality가 굉장히 떨어졌고 그로인해 어쩔 수 없이 Microsoft Heami TTS를 채택해야 했습니다.</br>
그를 위해 Client측을 Windows기반인 Windows 10 iot로 변경해야 했으며 쾌적한 처리를 위해 서버단에서 정보만 전달하는것이 아닌 정보를 파싱하여 음성파일까지 만든 후 해당 파일을 전송하는 식으로 처리할 수 밖에 없었습니다.</br>
Linux기반의 espeak를 사용해서 구현했던 코드를 이 README 파일 가장 아래에 첨부해두었습니다.</br>

## 1. Client
1\) Client의 구성</br>
- RFID 리더기</br>
- 라즈베리파이</br>
  - Windows 10 iot 운영체제
  - C#기반 프로그램
- 스피커</br>

2\) Client의 역할</br>
- 사용자가 원하는 장소에 Client 시스템 설치</br>
- RFID 리더기를 통한 태그 입력</br>
- 클라이언트 프로그램을 통해 서버와 네트워크 통신</br>
- 서버로부터 날씨/교통정보를 획득하여 음성으로 출력</br>

3\) Client 프로그램 구성</br>
- 사용자 접근 시 RFID 태그 값 입력</br>
- 소켓기반 네트워크 통신으로 서버프로그램에 태그 값 전송</br>
- 서버프로그램으로부터 각종 사용자 정보를 반환받음</br>
- 사용자 정보를 기반으로 기상청 홈페이지에서 날씨 조회</br>
- 날씨 정보를 XML형태로 반환받음</br>
- XML을 문장으로 구성 후 음성으로 출력</br>

4\) Client 프로그램 주요 내용</br>
- 네트워크 통신</br>
  - TCP기반으로 연결 후 전송</br>
- 기상청 홈페이지에서 날씨 조회</br>
  - RSS 기반으로 해당 지역의 날씨 정보 XML형태로 반환</br>
  - 도메인 주소 기반으로 정보 요청</br>
  - XMLDocument를 이용해 문장 구성</br>
- 음성으로 출력</br>
  - Microsoft에서 제공하는 무료 TTS 서비스 이용

## 2. Server
1\) Server의 역할
- Client로부터 사용자의 태그 값을 전송 받음
- 해당 태그 값으로 데이터베이스 서버에 질의
- DB로부터 사용자이름, 출발지, 도착지, 도착지역코드 등을 반환받음
- 클라이언트에게 사용자 정보를 전송

2\) Server의 구성
- C#기반 Windows Server 프로그램
  
3\) Server 프로그램의 구성
- 네트워크 개방 및 접속 허용
- 소켓 기반 네트워크 통신으로 Client로부터 태그 값 전송 받음
- 전송받은 태그 값으로 DB Server에 사용자 정보 질의
- 획득한 사용자 정보를 클라이언트로 전송

4\) Server 프로그램 주요 내용
- 네트워크 통신
  - TCP 기반 멀티쓰레드 방식으로 연결 후 통신
  - 다중 접속 지원 및 균등한 정보 반환 시간 제공
- 데이터베이스에 질의
  - 태그 값, 현재 요일, 현재 시간을 기반으로 질의
  - 주중/주말 또는 시간에 따라 행선지가 다를 수 있음
  - 세 개의 테이블을 조인하여 한번에 정보를 반환 받음

## 3. DB Server
1\) DB Server의 역할
- Server프로그램측에서 RFID 태그 값으로 사용자 정보 요청시 사용자의 정보 반환
- 사용자이름, RFID태그 값, 출발지, 도착지, 지역코드 등이 저장되어 있음
- 유비쿼터스 서비스 제공을 위해 Database서버 사용

2\) DB Server의 구성
- Ubuntu 기반 Linux 운영체제
- C언어 기반 소켓프로그래밍
- MySQL기반 DataBase

3\) DataBase 구성
- CLIENT 테이블
  - 사용자 이름과 RFID태그 값 저장
- LOCATION 테이블
  - 사용자의 출발지, 행선지, 별도의 정보 저장
- LOCATION_CODE 테이블
  - 200여개의 시/군/구 단위 지역명과 지역코드 저장
<img width="80%" src="https://github.com/batsalee/HelloGitHub/assets/109213754/99474b1d-b9c9-41e2-8ddb-82059c0bc76a"/>

## 4. 프로그램 구성도
<img width="80%" src="https://github.com/batsalee/HelloGitHub/assets/109213754/98d521c1-ffaa-40b3-a7d3-f04270629777"/>
<img width="80%" src="https://github.com/batsalee/HelloGitHub/assets/109213754/462c1b09-314c-4939-9346-881a54aaba5c"/>

---

# 날씨 및 교통정보
1\) 날씨정보
- 사용자정보의 도착지 사용
- 도착지의 지역코드 기반 기상청에 RSS로 Xml획득
- Xml 파싱을 통해 날씨정보 획득
- 교통정보의 통행소요시간만큼의 이후의 날씨정보 반환

2\) 교통정보
- 사용자정보의 출발지와 도착지 사용
- GeoCoding을 이용해 출발지, 도착지의 위도/경도 획득
- 위도/경도 기반 Google maps에 RSS로 Xml획득
- Xml 파싱을 통해 통행소요시간 획득

---

# 적용 기술
1\) RSS(Really Simple Syndication)
- 웹에 필요한 정보 요청 시 xml로 결과를 반환
- 날씨정보(기상청), 교통정보(구글 maps) 획득 시 사용

2\) TTS(Text to Speech)
- 날씨 및 교통정보를 parsing하여 음성으로 출력
- 별도의 확인없이 자동으로 정보 획득 가능

3\) GeoCoding
- DB의 사용자 정보에는 지역명만 저장
- 교통정보 획득에는 위도/경도 정보가 필요
- 지역명을 입력하면 위도/경도를 반환하는 기능을 구현
  
4\) RFID
- 별도의 입력 없이 사용자의 접근을 인식
  
5\) 라즈베리파이
- 소형 컴퓨터로써 Client 프로그램이 탑재됨

6\) MySQL
- 사용자정보 및 지역정보에 대한 데이터베이스 구현

7\) 소켓프로그래밍
- Server와 DB Server와 Client간에 TCP기반 소켓통신
- 시스템만 설치하면 어디에서든 사용할 수 있게 하기 위해 Server와 Client를 분리

8\) C/C# 프로그래밍
- DB Server는 C언어 기반 프로그램
- Client와 정보획득을 위한 Server는 C#기반 프로그램

9\) 운영체제
- DB Server는 Ubuntu기반의 Linux
- Server는 Windows 7
- Clinet는 Windows 10 iot

---

# 고난의 흔적
- 음성파일 전달이 아닌 문자열 전달 후 클라이언트에서 TTS로 출력하는 방식으로 구현했던 코드입니다.
- Client를 Windows 10 iot가 아닌 Ubuntu로 구성 후 C언어 기반으로 TTS출력을 하고자 했으나 당시 TTS의 Quality문제로 현재 레포지토리의 내용처럼 변경되었습니다.

```C#
// 6. 날씨 정보 획득
Weather weather = new Weather();
weather.initWeather(userInfo);

// 7. 날씨 문장 구성
int index = durationHour / 3;
string weatherText = "오늘 " + userInfo.getArriveLocationName() + "의 날씨는 " + weather.getWeatherList()[index].InnerText +
" 기온은 " + weather.getTempList()[index].InnerText + "도 강수확률은 " + weather.getPopList()[index].InnerText +
"퍼센트 습도는 " + weather.getRehList()[index].InnerText + "퍼센트입니다";

// 8. 리눅스서버로 날씨, 교통정보 반환		
string defaultText = userInfo.getUserName() + "님 안녕하십니까 ";		
string resInfo = defaultText + trafficText + weatherText;
				
communicator.sendCommunicate(resInfo);		
```

```C
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sys/socket.h>
#include <arpa/inet.h>

void main()
{
	char ipAddr[20];
	int portNo;

	// ip, port 입력
	printf("Server IP(default 192.168.11.9) : ");
	scanf("%s", ipAddr);
	printf("port Number(default 1105) : ");
	scanf("%d", &portNo);

	while(1) {
		int inputTag;
		printf("태그인식 : ");
		scanf("%d", &inputTag);
		char RFIDTag[20];
		sprintf(RFIDTag, "%d", inputTag);
		
		// 1. socket()
		int clnt_sock = socket(PF_INET, SOCK_STREAM, 0);
		if(clnt_sock == -1) {
			fputs("socket() error", stderr);
			continue;
		}

		// connect 준비		
		struct sockaddr_in serv_addr;
		memset(&serv_addr, 0, sizeof(serv_addr));
		serv_addr.sin_family = AF_INET;
		serv_addr.sin_addr.s_addr = inet_addr(ipAddr);
		serv_addr.sin_port = htons(portNo);

		int serv_addr_size = sizeof(serv_addr);

		// 2. connect()
		if( connect(clnt_sock, (struct sockaddr*)&serv_addr, serv_addr_size) == -1 ) {
			fputs("connect() error", stderr);
			close(clnt_sock);
			continue;
		}
		
		// RFID Tag 전송
		send(clnt_sock, RFIDTag, sizeof(RFIDTag), 0);

		// 모든 정보를 문장화한 결과를 받음
		char recvRes[1024];
		recv(clnt_sock, recvRes, sizeof(recvRes), 0);

		// TTS로 내용을 음성으로 출력
		char ttsParam[1024+20] = "";
		sprintf(ttsParam, "espeak -v ko \"%s\"", recvRes);
		system(ttsParam);

		close(clnt_sock);
	}
}

