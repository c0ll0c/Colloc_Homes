# Colloc Homes Convention

<br>

## Branch Name
- (dev name)@(feature)

<br>

## Commit Message
- feat: 새로운 기능 추가
- fix: 버그 수정
- docs: 문서 관련
- refactor: 코드 리팩토링
- clean: 빌드 업무 수정, 패키지 매니저 수정 등
- add: 기능, 문서 외 애매한 내용 추가
- delete: 삭제
- update: 버전 업데이트
- rename: 이름 변경

<br>

## Asset
- Animation: (object name)\_anim
- Prefab(Pascal): (original prefab name)\_(new name) 
- Hierarchy (Camel): (prefab name)\_(new name)

<br>

## Script
- 00.cs: 구조, 모델
- 00Manager.cs: 00에 대한 전체적 관리
- 00Controller.cs: 플레이어가 직접적인 조종
- Handle00.cs: 플레이어와 상호작용
- 00Operator.cs: 플레이어와 상관없이 작동
- (UIObject name)(UI Type)UI.cs: UI Object (ex. LoadingTextUI.cs)
- (Prefab name)OnClick.cs: Button Object

### 변수 선언
public vars(Pascal)
enter
[serializable] private vars(Camel)
private vars(Camel)
- 함수 매개변수: \_camelCase (ex. \_state)
