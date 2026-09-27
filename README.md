# 둘기의 도시 생존기

Unity C#으로 만드는 2D 모바일 서바이벌 액션 아케이드 게임입니다. 화가 난 주인공 비둘기가 음식을 먹어 성장하고, 동료 비둘기를 모으며, 눈빛 공격과 응가 폭격으로 인간을 굴복시켜 빵을 받는 게임입니다.

## 개발 환경

- Unity **6000.3.25f1 (Unity 6.3 LTS)** 고정
- Git으로 프로젝트 전체를 공유합니다. `Library/`, 빌드 결과, 사용자별 에디터 설정은 저장소에 올리지 않습니다.
- 기본 에셋은 Unity 내장 기능만 사용합니다. 새 패키지를 추가할 때는 팀에 공유하고 `Packages/manifest.json`과 `Packages/packages-lock.json`을 함께 커밋합니다.

## 처음 실행하기

1. Unity Hub에서 Unity **6000.3.25f1**을 설치하고 Android Build Support(필요한 경우 Android SDK & NDK Tools, OpenJDK 포함)를 선택합니다.
2. 저장소를 내려받은 뒤 Unity Hub의 **Add → Add project from disk**에서 저장소 폴더를 선택합니다.
3. 같은 에디터 버전으로 프로젝트를 열고 패키지 가져오기가 끝날 때까지 기다립니다.
4. Android 빌드는 `File → Build Profiles`에서 Android 프로필을 추가한 뒤 기기에서 확인합니다. iOS 빌드는 macOS와 Xcode가 필요합니다.

에디터 버전은 `ProjectSettings/ProjectVersion.txt`에 고정되어 있습니다. 팀원마다 다른 Unity 버전으로 프로젝트를 저장하지 마세요.

## 프로젝트 폴더

`Assets/_Project/` 아래 파일을 기능별로 나눕니다.

| 폴더 | 용도 |
| --- | --- |
| `Art/Characters` | 플레이어, 비둘기, 인간 스프라이트 |
| `Art/Environment` | 음식, 오브젝트, 배경과 맵 타일 |
| `Art/UI` | UI 이미지와 아이콘 |
| `Art/VFX` | 눈빛 공격과 응가 폭격 효과 |
| `Audio/SFX`, `Audio/BGM` | 효과음과 배경음 |
| `Data` | 음식, 스테이지, 적 스펙 ScriptableObject |
| `Prefabs`, `Scenes`, `UI` | 재사용 프리팹, 씬, UI |
| `Scripts/Core`, `Player`, `AI`, `Combat`, `UI` | 공통, 플레이어, AI, 전투, UI 코드 |

현재는 프로젝트 골격과 협업 기준만 준비되어 있습니다. Unity 에디터에서 첫 씬과 플랫폼별 Player Settings를 설정한 뒤 씬을 Build Profiles에 추가하세요. 화면은 16:9 및 19:20 비율에 대응하고 UI에는 안전 영역을 적용합니다.

## 팀별 작업 경계

| 파트 | 담당 | 소유 영역 |
| --- | --- | --- |
| 1 | 박지윤 | 플레이어 이동·성장, 음식 획득, 눈빛 공격 |
| 2 | 남유리 | 동료 군단, 인간 AI/FSM, 상납 시스템 |
| 3 | 진자연 | 응가 폭격, 전투 규칙, 스테이지 맵 |
| 4 | 이우진 | UI/UX, 미니맵, 사운드 및 이펙트 통합 |

공통 데이터와 시스템 연결 규칙은 [팀 협업 가이드](docs/TEAM_WORKFLOW.md), 리소스 이름·라이선스 관리는 [에셋 가이드](docs/ASSET_GUIDE.md)를 따릅니다.

## 기본 흐름

작업은 `feat/part-1-player-growth` 또는 `fix/issue-summary`처럼 짧은 브랜치에서 진행하고, `main`에 직접 커밋하지 말고 Pull Request로 합칩니다. PR에는 변경 목적, 에디터에서 확인한 내용, 씬·에셋 변경 사항을 적습니다. Unity의 `.meta` 파일은 에셋과 함께 커밋하고, 삭제하거나 재생성하지 마세요.
