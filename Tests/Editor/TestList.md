# Unity-Common Test List

## File 관련 테스트

### FileExceptionsTest.cs

- **DirectoryCreationException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FilePathException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FileReadException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FileWriteException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FileTypeResolveException**: 생성자 테스트 (기본, 메시지, 내부 예외), 직렬화/역직렬화 테스트, 스택 트레이스 보존 테스트
- **FileOperationException**: 생성자 테스트 (기본, 메시지, 내부 예외)

### FileExtensionsSync

#### FileExtensionsWriteTest.cs

- **WriteFileToPath**: 유효/잘못된 경로, null 경로, 중첩 디렉토리, 대용량 파일 테스트

#### FileExtensionsLockTest.cs

- **IsFileLocked**: 읽기/쓰기 잠금 상태 테스트

#### FileExtensionsCompareTest.cs

- **CompareFiles**: 동일/다른 파일, 내용 비교 옵션 테스트

#### FileExtensionsCopyTest.cs

- **CopyFile**: 기본 복사, 덮어쓰기, 존재하지 않는 원본, 대상 파일 존재 테스트

### FileExtensionsAsync

#### FileExtensionsTaskWriteTest.cs

- **WriteFileToPathAsync**: 유효/잘못된 경로, null 경로, 취소, 중첩 디렉토리, 대용량 파일 테스트

#### FileExtensionsTaskCompareTest.cs

- **CompareFilesAsync**: 동일/다른 파일, 취소 테스트

#### FileExtensionsTaskCopyTest.cs

- **CopyFileAsync**: 기본 복사, 덮어쓰기, 취소 테스트

### FileExtensionsAwaitable (Unity 2023.2+)

#### FileExtensionsAwaitableWriteTest.cs

- **WriteFileToPathAwaitable**: 유효 경로 테스트

#### FileExtensionsAwaitableCompareTest.cs

- **CompareFilesAwaitable**: 동일 내용 파일 비교 테스트

#### FileExtensionsAwaitableCopyTest.cs

- **CopyFileAwaitable**: 기본 복사 동작 테스트

### FileType 관련 테스트

#### CustomFileTypesTest.cs

- **커스텀 타입**: 등록 및 해석 테스트 (.md, .py)

#### FileCategoryTest.cs

- **카테고리**: 등록된/등록되지 않은 이름으로 검색 테스트
- **카테고리 속성**: Unity 카테고리 속성 테스트
- **ToString**: 카테고리 이름 반환 테스트

#### FileTypeDefinitionTest.cs

- **생성자**: 유효/잘못된 매개변수 테스트 (확장자, 설명, 카테고리, MIME 타입)
- **속성**: 확장자 소문자 저장, MIME 타입 대소문자 구분 없는 비교 테스트
- **동등성**: 동일 속성 인스턴스 동등성 테스트
- **MIME 타입**: null, 빈 배열, 중복 MIME 타입 처리 테스트

#### FileTypeEdgeCaseTest.cs

- **특수 문자**: 파일 경로 처리 테스트 (한글, 공백, 특수문자)
- **다중 확장자**: 마지막 확장자 사용 테스트
- **대소문자 구분**: 확장자 대소문자 구분 없는 처리 테스트

#### FileTypeIntegrationTest.cs

- **실제 파일**: 생성 및 타입 확인 테스트
- **경로**: 중첩, 상대, 절대 경로 처리 테스트

#### FileTypeRegistryTest.cs

- **파일 타입 등록**: 유효/중복 타입 등록 테스트
- **파일 타입 검색**: 확장자, MIME 타입별 검색 테스트
- **대소문자 구분**: 확장자 대소문자 구분 없는 검색 테스트

#### FileTypeResolverPerformanceTest.cs

- **성능**: GetFileType, GetTypesByCategory 메서드 성능 테스트

#### FileTypeResolverTest.cs

- **파일 타입 해석**: 일반/알 수 없는 타입 해석 테스트
- **카테고리별 타입**: 이미지, 애니메이션 카테고리 타입 반환 테스트
- **IsTypeOf**: 파일 경로와 카테고리 비교 테스트
- **MIME 타입**: 대소문자 구분 없는 MIME 타입 검색 테스트, null/빈 MIME 타입 처리 테스트
