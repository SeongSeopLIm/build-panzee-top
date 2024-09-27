# build-panzee-top

**우왁굳** 팬 게임입니당. \>o</

## Tool

- **Unity** 2022.3.45

## Requirements

- [UniRX](https://github.com/neuecc/UniRx)
- [UniTask](https://github.com/Cysharp/UniTask)
- [DoTween]

## Spawn Prefab List 추가 가이드

이 문서는 **스폰 리스트**에 새로운 프리팹을 추가하는 방법을 설명합니다. 아래 단계를 따라 진행해주세요.

### 1. Prefab 폴더로 이동
`SpawnBundleSettings`의 `PrefabFolderPath`에 기록된 폴더로 이동합니다.  
기본 경로: `Assets/Resources/Prefabs/Actors/SpawnObjects`

### 2. 기존 프리팹 복사 및 편집
해당 폴더에서 기존 프리팹 중 하나를 복사합니다. 복사한 프리팹을 선택하여 편집 모드로 이동합니다.

### 3. 프리팹의 이미지 교체
편집 모드에서 원하는 이미지로 교체합니다.  
이때, 새로운 스폰 객체를 쉽게 구분할 수 있도록 시각적 요소를 신경 써주세요.

### 4. 콜리전 리셋
이미지를 교체한 후 **콜리전 리셋**을 진행합니다.  
(기존 콜리전이 남아있을 수 있으므로 새로고침이 필요합니다.)

### 5. 모든 프리팹 추가 후 Prefab List 업데이트
원하는 프리팹을 모두 추가한 후, 다시 **SpawnBundleSettings**로 돌아갑니다.  
하단의 **Update Prefab List** 버튼을 클릭하여 프리팹 목록을 갱신합니다.

### 6. ProbabilityCount 입력
새로 추가된 프리팹들이 목록에 추가된 것을 확인한 후, 각 프리팹의 `ProbabilityCount` 값을 입력합니다.

### 확률 계산 방법
각 프리팹의 출현 확률은 리스트 내 모든 `ProbabilityCount`의 합계를 기준으로 계산됩니다.  
각 프리팹의 출현 확률은 해당 프리팹의 `ProbabilityCount`를 전체 `ProbabilityCount`의 합으로 나눈 비율로 결정됩니다.

#### 예시
5개의 프리팹이 있고, `ProbabilityCount` 값이 각각 6, 1, 1, 1, 1이라면:  
- 첫 번째 프리팹은 전체 확률의 60%를 가집니다.
- 나머지 네 개의 프리팹은 각각 10%의 확률을 갖게 됩니다.
