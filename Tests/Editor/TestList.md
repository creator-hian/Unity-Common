# Unity-Common Test List

## File 관련 테스트

### FileExceptionsTest.cs

- **DirectoryCreationException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FilePathException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FileReadException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FileWriteException**: 생성자 테스트 (기본, 메시지, 내부 예외)
- **FileTypeResolveException**: 생성자 테스트 (기본, 메시지, 내부 예외), 직렬화/역직렬화 테스트, 스택 트레이스 보존 테스트
- **FileOperationException**: 생성자 테스트 (기본, 메시지, 내부 예외)

### FileExtensionsTest.cs

- **파일 쓰기**: 유효/잘못된 경로, null 경로, 취소, 대용량 파일, 다양한 버퍼 크기 테스트
- **파일 잠금**: 읽기/쓰기 잠금 상태 테스트
- **파일 복사**: 취소, 덮어쓰기 테스트
- **파일 비교**: 동일/다른 파일, 취소 테스트
- **메모리 사용량**: 대용량 파일 쓰기 시 메모리 사용량 테스트
- **예외 처리**: 잘못된 버퍼 크기 테스트

### FileType 관련 ��스트

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
