# 리소스 및 에셋 가이드

## 시각·오디오 기준

- 2D 탑뷰/쿼터뷰, 툰 스타일의 캐주얼 파스텔 톤을 기준으로 합니다. 픽셀 아트는 사용하지 않습니다.
- 스프라이트는 투명 배경 PNG를 사용합니다. UI 아이콘은 128×128 또는 256×256px, 캐릭터/NPC는 높이 약 512px, 타일은 256×256px 또는 배경 스프라이트는 최대 2048×1536px 기준입니다.
- 화면은 16:9 및 19:20 비율에 대응합니다. HUD 배치는 안전 영역과 다양한 화면 크기를 고려합니다.
- SFX는 WAV/OGG, BGM은 OGG/MP3를 사용합니다.

## 이름 규칙

파일명은 영문 PascalCase 토큰과 밑줄을 사용합니다. 상태가 없는 종류는 상태 토큰을 생략합니다.

| 종류 | 형식 | 예시 |
| --- | --- | --- |
| UI | `UI_[유형]_[이름]_[상태]` | `UI_Bar_PlayerHp.png`, `UI_Gauge_EyeAttack.png` |
| 캐릭터/NPC | `Char_[타입]_[이름]_[동작]` | `Char_Player_RedEyePigeon_Idle.png`, `Char_NPC_Pigeon_Walk.png` |
| 환경/오브젝트 | `Env_[유형]_[이름]` | `Env_Food_TrashCan.png`, `Env_Bg_Alleyway.png` |
| 효과 | `VFX_[기술/원인]_[효과]` | `VFX_EyeBeam_PinkRay.png`, `VFX_PoopBomb_FlockDrop.png` |
| 효과음 | `SFX_[이름]` | `SFX_Pigeon_Coo.wav`, `SFX_EyeBeam_Ziririt.wav` |
| 배경음 | `BGM_[장소/상태]` | `BGM_Stage1_Alley.mp3` |

## 출처와 라이선스

아래 사이트는 탐색을 위한 후보입니다. 사이트 전체가 같은 라이선스인 것은 아니므로 **다운로드하는 개별 파일의 라이선스와 상업적 이용 조건을 확인한 뒤** 프로젝트에 추가하세요. 출처 표시가 필요한 경우 크레딧과 원본 링크를 기록합니다.

- 그래픽/UI: [Kenney](https://kenney.nl/assets), [itch.io 게임 에셋](https://itch.io/game-assets/free), [OpenGameArt](https://opengameart.org/)
- 효과음: [Freesound](https://freesound.org/), [Sonniss GDC Audio](https://sonniss.com/gameaudiogdc)
- 음악: [Incompetech](https://incompetech.com/music/), [YouTube Audio Library](https://www.youtube.com/audiolibrary)
- 한글 폰트: [눈누](https://noonnu.cc/)

## 에셋 등록 기록

새 외부 에셋을 추가하면 이 표에 **개별 에셋명, 원본 URL, 라이선스, 저작자/표시 문구, 사용 위치**를 기록합니다.

| 에셋 | 원본 URL | 라이선스 | 크레딧 문구 | 사용 위치 |
| --- | --- | --- | --- | --- |
| *(추가 시 기록)* |  |  |  |  |

기획서의 초기 수집 목록은 README의 파트별 소유권과 함께 사용하며, 에셋 수집 상태는 담당 파트가 PR에서 갱신합니다.
