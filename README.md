# Unity Common

Unity 프로젝트에서 자주 사용되는 기본적인 기능들을 제공하는 패키지입니다.

## 요구사항

- Unity 2021.3 이상
- .NET Standard 2.1

## 기능 목록

### 1. [파일 시스템](Runtime/00.Scripts/File/README.md)

- 파일 작업 (쓰기, 복사, 비교)
- 동기(Sync) 작업
- 비동기(Async) 작업
- Unity 2023.2+ Awaitable 작업 지원
- 파일 타입 관리
- MIME 타입 지원
- 파일 카테고리 분류

### 2. 추가 예정

- 네트워크 기능
- 데이터 직렬화
- 로깅 시스템
- 등...

## 설치 방법

### UPM을 통한 설치 (Git URL 사용)

#### 선행 조건

- Git 클라이언트(최소 버전 2.14.0)가 설치되어 있어야 합니다.
- Windows 사용자의 경우 `PATH` 시스템 환경 변수에 Git 실행 파일 경로가 추가되어 있어야 합니다.

#### 설치 방법 1: Package Manager UI 사용

1. Unity 에디터에서 Window > Package Manager를 엽니다.
2. 좌측 상단의 + 버튼을 클릭하고 "Add package from git URL"을 선택합니다.

   ![Package Manager Add Git URL](https://i.imgur.com/1tCNo66.png)
3. 다음 URL을 입력합니다:

```text
https://github.com/creator-hian/Unity-Common.git
```

4. 'Add' 버튼을 클릭합니다.

   ![Package Manager Add Button](https://i.imgur.com/yIiD4tT.png)

#### 설치 방법 2: manifest.json 직접 수정

1. Unity 프로젝트의 `Packages/manifest.json` 파일을 열어 다음과 같이 dependencies 블록에 패키지를 추가하세요:

```json
{
  "dependencies": {
    "com.creator-hian.unity.common": "https://github.com/creator-hian/Unity-Common.git",
    ...
  }
}
```

#### 특정 버전 설치

특정 버전을 설치하려면 URL 끝에 #{version} 을 추가하세요:

```json
{
  "dependencies": {
    "com.creator-hian.unity.common": "https://github.com/creator-hian/Unity-Common.git#0.0.1",
    ...
  }
}
```

#### 참조 문서

- [Unity 공식 매뉴얼 - Git URL을 통한 패키지 설치](https://docs.unity3d.com/kr/2023.2/Manual/upm-ui-giturl.html)

## 문서

각 기능에 대한 자세한 설명은 해당 기능의 README를 참조하세요:

- [파일 시스템 문서](Runtime/00.Scripts/File/README.md)

## 원작성자

- [Hian](https://github.com/creator-hian)

## 기여자

## 라이센스

[라이센스 정보 추가 필요]
