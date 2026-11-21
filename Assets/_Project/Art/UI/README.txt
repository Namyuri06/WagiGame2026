둘기의 도시 생존기 - 최종 이미지 기반 Unity 리소스 폴더

폴더
- Logo
- Buttons
- HUD
- Skills
- Panels
- Dialogue
- Effects
- Pigeons
- Reference

*_reference.png
- 방금 만든 최종 목업 이미지에서 직접 잘라낸 디자인 참고/사용용 리소스입니다.

*_blank.png / *_frame.png / *_fill.png
- 실제 Unity 런타임에서 쓰기 위한 버전입니다.
- 텍스트/숫자는 TextMeshPro로 추가하세요.

체력/에너지
- hp_bar_frame.png 위에 hp_bar_fill.png 배치
- energy_bar_frame.png 위에 energy_bar_fill.png 배치
- Fill Image의 Type = Filled
- Fill Method = Horizontal
- Fill Origin = Left

코드 예:
hpFill.fillAmount = (float)currentHp / maxHp;

Unity Import
- Texture Type = Sprite (2D and UI)
- Sprite Mode = Single
- Mesh Type = Full Rect
- Filter Mode = Point (no filter) 권장: 픽셀 느낌 유지 시
