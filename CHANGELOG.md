# 변경 로그

## 버전 관리 정책

이 프로젝트는 Semantic Versioning을 따릅니다:

- **Major.Minor.Patch** 형식
  - **Major**: 호환성이 깨지는 변경
  - **Minor**: 하위 호환성 있는 기능 추가
  - **Patch**: 하위 호환성 있는 버그 수정
- **최신 버전이 상단에, 이전 버전이 하단에 기록됩니다.**

### Unity 버전 호환성

- Unity 2021.3 이상 지원
- 각 Unity LTS 버전에 대한 호환성 보장

## [0.2.0] - 2024-12-06

### 추가

- Unity 2023.2+ 버전을 위한 Awaitable 파일 작업 지원
  - WriteFileToPathAwaitable
  - CopyFileAwaitable
  - CompareFilesAwaitable
- 파일 작업의 세 가지 방식 지원
  - 동기(Sync)
  - 비동기(Async)
  - Awaitable (Unity 2023.2+)

### 변경

- 파일 작업 테스트 구조 개선
  - Sync, Async, Awaitable 테스트 분리
  - 테스트 안정성 향상

### 호환성

- Awaitable 기능은 Unity 2023.2 이상 버전에서만 사용 가능
- 기존 동기/비동기 기능은 Unity 2021.3 이상에서 계속 지원

## [0.1.1] - 2024-12-05

<!-- markdownlint-disable MD024 -->
### 추가
<!-- markdownlint-enable MD024 -->

- GitHub Release를 통한 자동 패키지 퍼블리싱 기능 추가
  - release 브랜치의 릴리스가 publish될 때 자동으로 GitHub Packages에 배포
  - GitHub Release UI를 통한 버전 관리 지원

<!-- markdownlint-disable MD024 -->
### 변경
<!-- markdownlint-enable MD024 -->

- 패키지 배포 프로세스 개선

## [0.1.0] - 2024-12-05

<!-- markdownlint-disable MD024 -->
### 추가
<!-- markdownlint-enable MD024 -->

- 파일 시스템 기능 개선 및 안정화
  - 파일 작업 기능 강화
  - 타입 관리 시스템 개선
  - MIME 타입 지원 확장
  - 파일 카테고리 분류 체계화

### 예정된 변경사항

- 네트워크 기능 추가
- 데이터 직렬화 지원

### 업그레이드 가이드

- 성능 최적화 권장사항
- API 변경사항 문서 (릴리즈 시 제공 예정)

## [0.0.1] - 2024-11-30

<!-- markdownlint-disable MD024 -->
### 추가
<!-- markdownlint-enable MD024 -->

- 초기 패키지 구조 설정
- 기본 파일 시스템 기능 구현
  - 파일 작업 (쓰기, 복사, 비교)
  - 파일 타입 관리
  - MIME 타입 지원
  - 파일 카테고리 분류
