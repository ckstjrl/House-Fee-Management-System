# House-Fee-Management-System
## 관리비 기록 분석 GUI
주거 공간의 관리비를 체계적으로 기록하고 시각적으로 분석하기 위한 데스크톱 기반 관리비 관리 애플리케이션입니다.

월별 관리비 입력부터 항목별 분석, 연도별 비교까지
사용자가 소비 패턴을 직관적으로 파악할 수 있도록 설계했습니다.

## 프로젝트 개요
- 제작 이유

    기존에 관리비를 엑셀로 기록하며 관리했지만,  
    데이터가 누적될수록 월별·연도별 비교가 어렵고  
    소비 패턴을 직관적으로 파악하기 힘들다는 문제를 느꼈습니다.

    이러한 불편함을 해결하기 위해  
    관리비 데이터를 체계적으로 저장하고 시각화할 수 있는  
    데스크톱 기반 관리비 분석 프로그램을 직접 기획·제작했습니다.

- 사용자

    - 과거 관리비 내역을 한눈에 확인하고 싶은 사용자
    - 관리비를 간편하게 기록하고 비교하고 싶은 사용자
    - 하나의 프로그램으로 관리비 흐름을 관리하고 싶은 사용자

## 주요 기능
- 최근 6개월간 관리비 추이 확인
- 관리비 월별 입력 / 실시간 그래프로 항목별 비율 확인
- 관리비 과거 기록 확인
- 관리비 항목 CRUD
- 연도별 / 월별 관리비 비교 시각화
- 항목별 지난달, 작년 동월대비 관리비 증감 확인

## 기능별 화면
- 메인 (최근 6개월간 관리비 추이 확인)

    사용자의 편의에 맞춰 6개월간 관리비 추이를 꺾은선 그래프, 막대그래프로 선호에 맞게 변경 가능

    - 메인화면 꺾은선 그래프 version
    ![메인화면_꺾은선](/IMG/MainPage.jpg)
    
    - 메인화면 막대 그래프 version
    ![메인화면_막대](/IMG/MainPageStick.jpg)

- 관리비 입력

    관리비 입력, 실시간 항목별 비율 확인 및 관리비 항목 CRUD 가능

    - 관리비 입력
    ![내역추가](/IMG/AddRecordPage.jpg)

    - 항목별 비율
    ![내역추가_파이차트](/IMG/AddRecordPage_analysis.jpg)

    - 관리비 항목 CRUD
    ![항목추가](/IMG/AddItem.jpg)

- 관리비 과거 기록 확인

    - 이전 관리비 내역 확인
    ![과거내역](/IMG/HistoryPage.jpg)

- 연도별 / 월별 관리비 비교 시각화

    연평균 비교, 월별 비교 + 마우스 호버링시 항목 비율 출력

    - 연도별 관리비 비교 시각화
    ![연도별비교](/IMG/YearRecordAverageCompareBasic.jpg)
    ![연도별비교항목](/IMG/YearRecordAverageCompare.jpg)

    - 월별 관리비 비교 시각화
    ![월별비교](/IMG/MonthRecordAverageCompareBasic.jpg)
    ![월별비교항목](/IMG/MonthRecordAverageCompare.jpg)


- 항목별 지난달, 작년 동월대비 관리비 증감 확인

    ![항목별비교](/IMG/ItemCompare.jpg)

## 기술 스택
- Framework: .NET 8.0 / WPF

- Pattern: MVVM (Model-View-ViewModel)

- Libraries:

    - CommunityToolkit.Mvvm: 데이터 바인딩 및 커맨드 처리

    - LiveCharts.Wpf: 반응형 차트 및 툴팁 시각화

- Storage: JSON 직렬화 (로컬 데이터 저장)

## 프로젝트 구조
```
House-Fee-Management-System
├── IMG/                # README에서 사용하는 이미지
│
├── Models/             # 관리비 데이터 모델
│                       # (전기세, 수도세, 가스세 등 도메인 데이터 정의)
│
├── Services/           # 비즈니스 로직 처리
│                       # (데이터 저장, 조회, 계산 로직)
│
├── ViewModels/         # MVVM 패턴의 ViewModel
│                       # View와 Model 간 데이터 바인딩 및 상태 관리
│
├── Views/              # 화면 단위 UI(View)
│                       # 관리비 입력, 내역 조회, 분석 화면 등
│
├── App.xaml            # 애플리케이션 전역 리소스 및 스타일 정의
├── App.xaml.cs         # 애플리케이션 시작 및 설정 로직
│
├── MainWindow.xaml     # 메인 윈도우 UI
├── MainWindow.xaml.cs  # 메인 윈도우 코드 비하인드
│
├── AssemblyInfo.cs     # 어셈블리 정보 설정
├── app_icon.ico        # 애플리케이션 아이콘
├── .gitignore          # Git 버전 관리 제외 설정
└── README.md           # 프로젝트 설명 문서
```

## 개발 목적 & 배운점
- 개발 목적

    LIG Nex1 The SSEN 임베디드 스쿨 교육 과정중 C#, WPF, MVVM 수업을 복습하고 실제로 활용해보기 위해 진행한 프로젝트입니다.

    단순 기능 구현이 아닌,  
    실제 사용 시 불편함을 해결하는 것을 목표로  
    기획부터 구조 설계, UI 구현까지 직접 진행했습니다.

- 배운점
    
    - WPF 기반 데스크톱 애플리케이션 구조 이해
    - MVVM 패턴을 적용하여 View와 비즈니스 로직을 분리하는 설계 경험
    - 데이터 바인딩 및 Command 패턴을 활용한 UI 상태 관리
    - JSON 직렬화를 이용한 로컬 데이터 저장 및 관리
    - LiveCharts를 활용한 실시간 데이터 시각화 및 사용자 경험 개선

## 개선 예정 사항
- 관리비 내역 Excle / PDF 불러오기+내보내기 기능
- 클라우드 데이터 백업 연동

## 요약
House-Fee-Management-System은  
주거 관리비 데이터를 월별로 기록하고, 시각화된 그래프를 통해  
소비 패턴을 직관적으로 분석할 수 있도록 만든 WPF 기반 데스크톱 애플리케이션입니다.

MVVM 패턴을 적용하여 UI와 비즈니스 로직을 분리하였으며,  
JSON 기반 로컬 저장 방식을 통해 간편한 데이터 관리가 가능하도록 구현했습니다.

실제 생활에서 느낀 불편함을 바탕으로 기획부터 설계, 구현까지 진행한 프로젝트로,  
C#, WPF, MVVM 구조에 대한 이해와  
데이터 시각화 및 사용자 중심 UI 설계 경험을 쌓을 수 있었습니다.