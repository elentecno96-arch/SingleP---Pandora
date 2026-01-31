# SingleP---Pandora
Single Project - Pandora

게임 이름 : Pandora Reborn ( 판도라 리본 )

게임 장르 : Action Roguelike + Hack and Slash ( 액션 로그라이크 + 핵 앤 슬래시 )

게임 플랫폼 : PC

사용 엔진 : Unity 2022.3.62f2

개발 기간 : 26.01.29 ~ 26.02.27

게임 컨셉
- 마법을 조합 하여 스킬의 자유도를 올린 시스템
  
- 던전 또는 탑을 탐험하여 반복 플레이를 통한 성장
 
- 핵 앤 슬래쉬 장르의 다수 적 처치 재미

- Update -

- 26.01.29 =========================

[추가] 필수 싱글 톤 매니저 추가

      - GameManager.cs
      
      - UiManager.cs
      
      - AudioManager.cs
      
      - EffectManager.cs
      
      - SpawnManager.cs

      - PlayerManager.cs
      
[추가] 제네릭 싱글 톤 추가

      - SingletonT.cs

[추가] 씬 Main 추가

- 26.01.30 =========================

[내용] 플레이어 기초 이동 시스템 및 스탯 초기화 구조 구현

      - Player.cs : Stat, Movement, Combat, Condition 을 가지고 있습니다
      
      - InitAll() : Player가 각 부품의 Init()을 정해진 순서대로 초기화

[내용] Player 이동 구현

      - 물리 기반 이동 방식 ( 확장 매서드 이용 ) RigidbodyExtensions.cs
      
      - WASD 이동 방식, 이동 시 DOTween 기울기 연출 추가
      
      - Rigidbody의 중력 제거

[내용] Player, Enemy 공용 스탯 구조체

     - 플레이어, 적, 보스가 공용으로 사용할 수 있는 Stat 구조체 추가

     ( 상황에 따라 속성은 추가 될 예정 )

[내용] Skill 기초 작업

     - ISkillable.cs : 모든 스킬이 가져야 할 Execute() 인터페이스
     
     - SkillBase.cs : 쿨타임 연산 등 모든 스킬이 공통으로 사용할 로직을 추상 클래스로 분리

[내용] 전투 ( Combat ) 공격 방식 구현

     - 스킬을 가질 수 있는 최대 갯수, 스킬 사용 가능 여부 체크, SkillBase의 Execute() 실행

- 26.01.31 =========================

[내용] Game, Player Manager 하위 관리자 System 추가

     - Player를 관리하는 PlayerManager.cs (Singleton) 추가
     
     - GameManager 하위 관리자 Stat,Rune,Skill System 추가
     
     - PlayerManager 하위 관리자 State System 추가

[삭제] Player Condition, Stat 제거

     - 스탯과 상태는 하위 관리자인 Player State System이 관리 합니다
