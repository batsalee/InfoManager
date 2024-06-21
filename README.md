# \[학부 프로젝트 / 졸업작품\] InfoManager

## 목차
1. 개요
2. 참고사항
3. 프로젝트 개요
4. 개발내용  
    1\) Client  
    2) Server  
    3) DB Server  
    4) 프로그램 구성도  
5. 날씨 및 교통정보  
6. 적용기술  
7. 고충 및 해결방법
8. 링크
---
## 1. 개요

사용자 접근 시 해당 사용자를 자동으로 식별해 스케쥴에 맞는 날씨 및 교통정보를 음성으로 제공하는 시스템  
예를들어 아침에 처음 가는 곳이 냉장고라면 냉장고 옆에 이 시스템을 설치 후 매일 아침 사용자가 접근시 자동으로 해당 사용자를 식별하여 스케쥴에 맞는 목적지까지의 교통 정보 및 날씨 정보를 음성으로 알려주는 시스템  
</br>

## 2. 참고 사항

이 프로젝트는 제 대학교 졸업작품을 위해 제작된 프로젝트입니다.  

기획단계에서 졸업작품인 만큼 학교에서 배운 모든것을 써보자! 라는 각오로 기획했습니다.  
그래서 프로그래밍언어 뿐 아니라 네트워크, 데이터베이스, iot 장비사용 등 최대한 많은것을 사용해보고자 노력했습니다.  
그 외에도 실제 프로젝트 진행이라면 모든것을 알고 진행할 수는 없기에 당시 사용 경험이 없었던 외부API나 당시 신규서비스에 가까웠던 TTS등을 사용했고 C#을 공부해가면서 제작했습니다.   
그 결과 학교에서 배웠던 C언어, 데이터베이스, 프로젝트진행방법 등에 더해 개인적으로 공부했던 네트워크 소켓 프로그래밍, Linux, C# 기초수준 등을 더해 제작했습니다.  

또한 프로그램 소스코드 작성에도 당시 알던 수준의 기법은 최대한 사용해보고자 했습니다.  
예를들어 여러 값을 리턴받아야 하는 경우 구조체를 만들어 리턴하거나, 레퍼런스로 값을 전달하거나, 벡터같은 선형자료구조로 리턴 받는 등의 다양한 방법을 모두 사용해보고자 했습니다.  
그 결과 코드가 조금 지저분하거나 난잡해보일 수 있으나 졸업작품인 만큼 모든것을 사용해보고자 했던 의지로 봐주시면 좋을 것 같습니다.  
</br>

## 3. 프로젝트 개요

- 사용자가 원하는 장소에 시스템을 설치
- 시스템이 설치된 장소에 접근 시 RFID 기반으로 사용자를 자동 식별
- 해당 사용자의 일정에 맞는 날씨 및 교통 정보를 음성으로 제공
- 시스템이 설치된 곳에선 어디서나 동일한 서비스 제공
---
## 개발 내용

Client, Server, DB Server 3단 구성  

※ 참고사항  
개발 당시 TTS서비스가 초창기시절이라 라이센스 등의 제약이 있었기 때문에 선택할 수 있는 TTS서비스의 폭이 좁았습니다. 그로인해 준수한 TTS를 사용하기 위해 아래에 작성되어 있는 기획 중 Client의 역할 일부를 Server에서 처리 후 클라이언트로 전송하도록 구현하여 push해두었습니다.  
</br>

## 1. Client

#### 1) Client의 구성
- RFID 리더기
- 스피커
- 라즈베리파이  
    - Windows 10 iot 운영체제
    - C#기반 프로그램

#### 2) Client의 역할
- 사용자가 원하는 장소에 Client 시스템 설치
- RFID 리더기를 통한 태그 입력
- 클라이언트 프로그램을 통해 서버와 네트워크 통신
- 서버로부터 날씨/교통정보를 획득하여 음성으로 출력

#### 3) Client 프로그램 구성
- 사용자 접근 시 RFID 태그 값 입력
- 소켓기반 네트워크 통신으로 서버프로그램에 태그 값 전송
- 서버프로그램으로부터 각종 사용자 정보를 반환받음
- 사용자 정보를 기반으로 기상청 홈페이지에서 날씨 조회
- 날씨 정보를 XML형태로 반환받음
- XML을 문장으로 구성 후 음성으로 출력

#### 4) Client 프로그램 주요 내용
- 네트워크 통신  
    - TCP기반으로 연결 후 전송
- 기상청 홈페이지에서 날씨 조회  
    - RSS 기반으로 해당 지역의 날씨 정보 XML형태로 반환
    - 도메인 주소 기반으로 정보 요청
    - XMLDocument를 이용해 문장 구성
- 음성으로 출력  
    - Microsoft에서 제공하는 무료 TTS 서비스 이용
</br>

## 2. Server

#### 1) Server의 역할
- Client로부터 사용자의 태그 값을 전송 받음
- 해당 태그 값으로 데이터베이스 서버에 질의
- DB로부터 사용자이름, 출발지, 도착지, 도착지역코드 등을 반환받음
- 클라이언트에게 사용자 정보를 전송

#### 2) Server의 구성
- C#기반 Windows Server 프로그램

#### 3) Server 프로그램의 구성
- 네트워크 개방 및 접속 허용
- 소켓 기반 네트워크 통신으로 Client로부터 태그 값 전송 받음
- 전송받은 태그 값으로 DB Server에 사용자 정보 질의
- 획득한 사용자 정보를 클라이언트로 전송

#### 4) Server 프로그램 주요 내용
- 네트워크 통신
    - TCP 기반 멀티쓰레드 방식으로 연결 후 통신
    - 다중 접속 지원 및 균등한 정보 반환 시간 제공
- 데이터베이스에 질의
    - 태그 값, 현재 요일, 현재 시간을 기반으로 질의
    - 주중/주말 또는 시간에 따라 행선지가 다를 수 있음
    - 세 개의 테이블을 조인하여 한번에 정보를 반환 받음
</br>

## 3. DB Server

#### 1) DB Server의 역할
- Server프로그램측에서 RFID 태그 값으로 사용자 정보 요청시 사용자의 정보 반환
- 사용자이름, RFID태그 값, 출발지, 도착지, 지역코드 등이 저장되어 있음
- 유비쿼터스 서비스 제공을 위해 Database서버 사용

#### 2) DB Server의 구성
- Ubuntu 기반 Linux 운영체제
- C언어 기반 소켓프로그래밍
- MySQL기반 DataBase

#### 3) DataBase 구성
- CLIENT 테이블
    - 사용자 이름과 RFID태그 값 저장
- LOCATION 테이블
    - 사용자의 출발지, 행선지, 별도의 정보 저장
- LOCATION_CODE 테이블
    - 200여개의 시/군/구 단위 지역명과 지역코드 저장
![](https://blog.kakaocdn.net/dn/cYrjwf/btsH8FekzST/kZ40AkwJ8CjOykcww1kjq0/img.png)
</br>

## 4. 프로그램 구성도
![](https://blog.kakaocdn.net/dn/SxfXK/btsH8A5p9v0/xMXoMB3ASffxZitYMWygYk/img.png)

![](https://blog.kakaocdn.net/dn/BhMp2/btsH83stnUc/kDL1guYFT6btKdAAtfB2aK/img.png)

---
## 날씨 및 교통정보

#### 1) 날씨정보
- 사용자정보의 도착지 사용
- 도착지의 지역코드 기반 기상청에 RSS로 Xml획득
- Xml 파싱을 통해 날씨정보 획득
- 교통정보의 통행소요시간만큼의 이후의 날씨정보 반환

#### 2) 교통정보
- 사용자정보의 출발지와 도착지 사용
- GeoCoding을 이용해 출발지, 도착지의 위도/경도 획득
- 위도/경도 기반 Google maps에 RSS로 Xml획득
- Xml 파싱을 통해 통행소요시간 획득
---
## 적용 기술

#### 1) RSS(Really Simple Syndication)
- 웹에 필요한 정보 요청 시 xml로 결과를 반환
- 날씨정보(기상청), 교통정보(구글 maps) 획득 시 사용

#### 2) TTS(Text to Speech)
- 날씨 및 교통정보를 parsing하여 음성으로 출력
- 별도의 확인없이 자동으로 정보 획득 가능

#### 3) GeoCoding
- DB의 사용자 정보에는 지역명만 저장
- 교통정보 획득에는 위도/경도 정보가 필요
- 지역명을 입력하면 위도/경도를 반환하는 기능을 구현

#### 4) RFID
- 별도의 입력행위 없이 사용자의 접근을 인식

#### 5) 라즈베리파이
- 소형 컴퓨터로써 Client 프로그램이 탑재됨

#### 6) MySQL
- 사용자정보 및 지역정보에 대한 데이터베이스 구현

#### 7) 소켓프로그래밍
- Server와 DB Server와 Client간에 TCP기반 소켓통신
- 시스템만 설치하면 어디에서든 사용할 수 있게 하기 위해 Server와 Client를 분리

#### 8) C/C# 프로그래밍
- DB Server는 C언어 기반 프로그램
- Client와 정보획득을 위한 Server는 C#기반 프로그램

#### 9) 운영체제
- DB Server는 Ubuntu기반의 Linux
- Server는 Windows 7
- Client는 Windows 10 iot
---
## 고충 및 해결방법

#### 1) 처음 다뤄보는 하드웨어

Client를 RFID리더기, 스피커, 라즈베리파이로 구성했고 라즈베리파이에 소프트웨어를 빌드하는 방식으로 구현했습니다.  
그런데 아무리 방법을 바꿔봐도 음성이 출력되지 않았고 이 문제를 해결하는데만 3주에 가까운 시간이 소모됐습니다.  
처음 의심했던 사항은 라즈베리파이에 설치된 Window 10 iot 운영체제의 사운드 옵션이었지만 최대 음량으로 설정해도 스피커에서는 약간의 노이즈만 발생할 뿐이었고, 이때문에 라즈베리파이를 포맷 후 ubuntu를 설치해보는 등 소프트웨어적인 모든 방법을 고민해봤으나 해결되지 않았습니다.  
결국 밝혀진 원인은 라즈베리파이의 바이오스 설정때문이었습니다. 운영체제에서의 음량설정 외에도 바이오스에서 별도의 음량설정이 있었고 이 음량이 너무 낮게 설정되어있어서 운영체제의 음량을 높이면 노이즈만 커졌던 것이었죠.  
정말 사소한 문제임에도 처음 다뤄보는 하드웨어라서 긴 시간을 고민하고 낭비해본 경험이었습니다.

#### 2) RSS를 위한 DB구축

기상청 정보와 교통정보를 얻기 위해 기상청과 Google maps의 외부 api서비스로 xml을 제공해주는 RSS를 사용했습니다.  
다만 이 정보를 사용하기 위해선 출발지와 도착지의 한글명과 각 지역의 코드를 DB에 저장해줘야 했고 이 과정을 그저 복사-붙여넣기로 반복하느라 이틀정도의 시간이 소비됐습니다.  
지금이라면 혹시나 정규표현식으로 문장을 만들 수 있지 않을까라는 발상을 하겠지만 당시에는 관련 지식이 없었기에 SQL을 만들 프로그램을 작성할 시간에 직접 복사-붙여넣기를 하기로 선택하여 비효율적인 작업이 발생했었습니다.  

#### 3) 초창기 서비스였던 TTS의 Quality

개발 당시 TTS서비스가 초창기시절이라 라이센스 등의 제약이 있었기 때문에 선택할 수 있는 TTS서비스의 폭이 좁았습니다. 당시 선택할 수 있었던 TTS서비스는 Windows에서 사용가능한 Microsoft Heami TTS와 Linux에서 사용가능한 espeak가 있었습니다.  

Linux기반 TTS인 espeak를 사용하면 위에서 작성한 최초의 기획과 동일하게 프로젝트 구성을 할 수 있었으나 당시의 espeak는 한국어 TTS의 음질이나 Quality가 굉장히 떨어졌고 그로인해 어쩔 수 없이 Microsoft Heami TTS를 채택해야 했습니다. 그로 인해 Client측을 Windows기반인 Windows 10 iot로 변경해야 했으며 쾌적한 처리를 위해 서버단에서 정보만 전달하는것이 아닌 정보를 파싱하여 음성파일까지 만든 후 해당 파일을 전송하는 식으로 처리할 수 밖에 없었습니다.  

Linux기반의 espeak를 사용해서 구현했던 고난의 흔적인 코드를 아래에 첨부해두었습니다.   
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

```C++
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
```
---
## 링크

#### 1) GitHub
[https://github.com/batsalee/InfoManager](https://github.com/batsalee/InfoManager)

#### 2) Blog
[https://smallpants.tistory.com/269](https://smallpants.tistory.com/269)
