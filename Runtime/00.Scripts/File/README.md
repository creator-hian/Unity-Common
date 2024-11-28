## 파일 시스템

파일 시스템 작업을 쉽고 안전하게 처리할 수 있는 기능을 제공합니다.

### 주요 기능

#### 1. 파일 작업
- 파일 쓰기 (동기/비동기)
- 파일 복사 (비동기)
- 파일 크기 확인
- 파일 비교
- 파일 잠금 상태 확인

#### 2. 파일 타입 관리
- 파일 타입 정의 및 해석
- MIME 타입 지원
- 카테고리 기반 파일 분류
- 확장자 기반 파일 타입 식별

### 사용 예시

1. 파일 쓰기:
    ```csharp
    // 동기식 파일 쓰기
    FileExtensions.WriteFileToPath("path/to/file.txt", byteData);
    
    // 비동기식 파일 쓰기
    await FileExtensions.WriteFileToPathAsync("path/to/file.txt", byteData);
    ```

2. 파일 타입 확인:
    ```csharp
    var resolver = FileTypeResolver.Instance;
    var fileType = resolver.GetFileType("example.png");
    
    Console.WriteLine($"확장자: {fileType.Extension}");
    Console.WriteLine($"설명: {fileType.Description}");
    Console.WriteLine($"카테고리: {fileType.Category}");
    ```

3. 파일 비교:
    ```csharp
    bool areEqual = await FileExtensions.CompareFilesAsync("file1.txt", "file2.txt");
    ```

### 지원하는 파일 타입

#### 일반 파일
- 텍스트 파일 (.txt)
- JSON/XML/CSV (.json, .xml, .csv)
- HTML 파일 (.html)
- 이미지 파일 (.jpg, .png, .gif)
- PDF 문서 (.pdf)
- ZIP 아카이브 (.zip)

#### Unity 관련 파일
- 씬 파일 (.unity)
- 프리팹/에셋 (.prefab, .asset)
- 머티리얼/셰이더 (.mat, .shader)
- 애니메이션 관련 (.anim, .controller)
- 메타 파일 (.meta)

### 예외 처리

라이브러리는 다음과 같은 구체적인 예외들을 제공합니다:
- `DirectoryCreationException`: 디렉토리 생성 실패
- `FilePathException`: 잘못된 파일 경로
- `FileWriteException`: 파일 쓰기 실패
- `FileOperationException`: 일반적인 파일 작업 실패
- `FileTypeResolveException`: 파일 타입 해석 실패

### 확장 가능성

커스텀 파일 타입 추가:
```csharp
FileTypes.RegisterTypeProvider(() => new[]
{
    new FileTypeDefinition(
        ".custom",
        "Custom File",
        FileCategory.Common.Data,
        "application/x-custom"
    )
});
```

### 성능 최적화
- 대용량 파일 처리를 위한 청크 단위 쓰기
- 파일 크기에 따른 최적화된 버퍼 크기 자동 계산
- 비동기 작업 지원으로 UI 응답성 보장

## 원작성자
- [Hian](https://github.com/creator-hian)

## 기여자

## 라이센스
[라이센스 정보 추가 필요]
