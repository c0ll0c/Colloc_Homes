using UnityEngine;

public static class StaticVars
{
    // player tags
    public static string TAG_COLLOC = "T_C";
    public static string TAG_HOLMES = "T_H";
    public static string TAG_INFECT = "T_I";

    // network variables
    public static string PREFS_NICKNAE = "nickname";
    public static byte MAX_PLAYERS_PER_ROOM = 6;
    public static byte MIN_PLAYERS_PER_ROOM = 1;
    public static int PLAYER_SLOTS = 0b111111; // 6 max players
    public static int PLAYER_COLORS = 0b111111111; // 9 colors

    // time variables
    public static double GAME_TIME = 300.0f; // 5m
    public static int START_PANEL_TIME = 3; // 3s
    public static double VACCINE_DROP_INTERVAL = 60.0f; // 1m
    public static float DETOX_USE_TIME = 3.0f; // 3s
    public static float DETOX_DEACTIVE_TIME = 30.0f; // 30s
    public static float ATTACK_TIME = 15.0f; // 15s
    public static float DELAY_TIME = 3.0f; // 3s
    public static float INFECT_DELAY_TIME = 6.0f; // 6S
    public static readonly float ENUM_TIME = 0.25f; // 0.25s
    public static float NPC_TIME = 120.0f;  // 2m
    public static float Hidden_TIME = 20.0f;
    public static float Ending_TIME = 6.0f;
    public static float VACCINE_TIME = 50.0f; // 50s
    public static int COLLOC_WIN_TIME = 30; //30s

    // item variables
    public static float PLANE_SPEED = 5.0f; 

    // homes variables
    public static float HOMES_SPEED = 3.0f;

    public static Vector2[] SpawnPosition =
    {
        new Vector2(1.8f, 1.3f),
        new Vector2(3.0f, 2.0f),
        new Vector2(4.2f, 1.3f),
        new Vector2(4.2f, -0.3f),
        new Vector2(3.0f, -1.0f),
        new Vector2(1.8f, -0.3f),
    };

    public static Vector2[] CluePosition = {
        new Vector2(-3.4f, -3f),
        new Vector2(-3.4f, 1.9f),
        new Vector2(-5f, 1.9f),
        new Vector2(3.6f, -1.2f),
        new Vector2(3.8f, 4.2f),
        new Vector2(-2f, 14f),
        new Vector2(-0.3f, 16f),
        new Vector2(9.8f, 12.6f),
        new Vector2(6f, 14f),
        new Vector2(12f, -9f),
        new Vector2(-4f, -8f),
        new Vector2(-9f, -6.3f),
        new Vector2(14.7f, 4.4f),
        new Vector2(7.6f, -6f),
        new Vector2(6f, -8f),
        new Vector2(4f, -10.5f),
        new Vector2(12.7f, -7.3f),
        new Vector2(-0.5f, 13f),
        new Vector2(9.2f, -3.4f),
    };
    
    /*
    public static Vector2[] CluePosition = {
        new Vector2(4.0f, 4.0f),
        new Vector2(-0.2f, -3.0f),
        new Vector2(4.0f, -10.0f),
        new Vector2(15.0f, 4.0f),
        new Vector2(-0.4f, 16.5f),
        new Vector2(-3.4f, -3.0f),
        new Vector2(-2.8f, 2.5f),
        new Vector2(12.0f, -9.0f),
        new Vector2(-8.4f, -6.0f),
        new Vector2(7.0f, -1.1f),
        new Vector2(1.8f, 1.3f),
        new Vector2(-6.0f, 8.0f),
        new Vector2(-4.0f, 6.0f),
        new Vector2(-4.5f, -4.6f),
        new Vector2(9.3f, 7.2f),
        new Vector2(13.3f, 4.4f),
        new Vector2(6.2f, 18.4f),
        new Vector2(-7f, -4.5f),
        new Vector2(5.0f, 7.7f),
    };*/

    public static string[] Colors =
    {
        "Brown",
        "Blue",
        "Gray",
        "Green",
        "Orange",
        "Pink",
        "Purple",
        "Yellow",
    };

    public static string[] dialogBody = {
        ", 범인을 찾은 건가?",
        "싱겁긴.",
        "오, 빨리 말해 보게! 이들 중 누가 콜록인가?",
        "... 수사 중 ...",
        "자네는 틀렸어. 앞으로 볼 일 없을 걸세.",
        "오, 역시! 약속대로 현상금을 주지. 수고했네!",
        ", 아직 3분이 되지 않았어. 성급한 판단일세."
    };

    public static string[] tipMessage =
    {
        "홈즈와 콜록의 승률은 동일하게 16%예요",
        "다른 홈즈를 견제하세요!",
        "콜록은 공격과 감염이 모두 가능해요!",
        "단서를 획득하면 일정 시간 동안 숨겨져요.\n빠르게 얻어야겠죠?",
        "개발진 팀 이름은 열무예요!",
        "열무는 디자이너 둘, 개발자 셋으로 이루어져 있어요",
        "열무의 팀 이름이 어쩌면 영 래디쉬가 될 뻔했다는 소문이...",
        "비행기의 추락을 조심하세요",
        "경감님도 감염될 수 있다는 사실...",
        "곧 도입될 코인 시스템을 기대해 주세요!",
        "홈즈와 단둘이 있을 때 감염을 시키면 들키겠죠?",
        "백신은 사실 아이깨끗해예요. 손씻기의 중요성 ㄷㄷ",
        "개발진은 열심히 맵 공사 중...",
    };

    public static string[] hideClueMessage =
    {
        "메롱 :P",
        "내가 먼저 다녀감 ^^v",
        "나중에 오시길...",
        "풉킥",
        "당신은 늦었다 ㄱ-",
    };

    public static string[] tutorialMessage =
    {
        "반갑네, 여기는 콜록 바이러스가 퍼진 섬일세.",
        "현상금을 목적으로 이 섬에 모인 홈즈들 사이에, 콜록 하나가 잠입해 있을 것일세.",
        "섬의 곳곳에는 콜록이 위조한 홈즈 ID카드와 코드에 대한 단서가 숨겨져 있지.\n단서들을 잘 조합하여 콜록을 찾아 나에게 알려 주게나.",
        "콜록 바이러스에 전염되지 않게 조심해야 한다네!\n이제부터 게임 방법을 설명해 주겠네.\n먼저 자네가 취할 수 있는 행동은 공격과 감염, 두 가지야.",
        "콜록이거나 감염된 홈즈라면,\n다른 플레이어와 가까워지면 상대방을 감염시킬 수 있다네.\n한번 해 보시게나.",
        "잘했네. 자네가 감염된 홈즈라면,\n단서 보기와 콜록 지목을 못 하게 된다네.\n다른 플레이어는 자네가 콜록인지, 홈즈인지, 감염된 상태인지 모를 걸세.",
        "다음으로 공격일세.\n콜록과 홈즈 둘 다 공격을 할 수가 있어. 감염과 같은 방식이지.",
        "마찬가지로 다른 플레이어와 가까워지면, 상대를 공격할 수 있어.\n공격당한 플레이어는 3초 간 모든 행동이 마비가 되지.\n감염 또는 공격의 방어 수단으로 잘 활용하길 바라.",
        "승리와 패배 조건을 설명해 주겠네. 먼저 홈즈일세.",
        "이 섬은 개인전이야. 누구보다 빠르게 콜록을 찾아 나에게 고발해 주게.\n제일 먼저 고발하여 승리한 홈즈 한 명에게 포상금이 지급될 것이야.\n단서를 한번 직접 찾아보게나!",
        "잘했네.\n획득한 단서는 단서 수첩에서 확인해 볼 수 있다네.\n콜록의 전체 코드와, 다른 플레이어들의 코드를 잘 비교해 보길 바라.",
        "다른 홈즈가 먼저 콜록을 나에게 고발하거나,\n나에게 무고한 홈즈를 고발하였을 때, 자네는 패배할 걸세.\n그러니 신중해야 한다는 사실을 명심하게.",
        "보급 헬기가 세 개의 백신을 보급해 줄 것일세.\n백신을 먹어서 감염을 한번 예방하게나.",
        "감염이 된다면, 앞서 말했던 페널티가 일시적으로 주어지게 될 거야.\n단서를 볼 수도, 콜록을 고발할 수도 없지.\n해독 부스에 들어가 감염되지 않은 상태로 돌아가야 한다네. 해 보시게나.",
        "잘했네. 이제 콜록의 승리와 패배를 알려 주겠네.",
        "콜록은, 타임 오버가 되거나,\n모든 홈즈가 감염되어 있는 상태로 일정 시간을 유지하면 승리해.\n콜록들은 두 번째 조건을 만족시키기 위해 해독 부스를 파괴하곤 하더군.",
        "그리고 콜록을 찾은 홈즈가 나에게 고발하면 패배하게 되지.",
        "홈즈들은 단서를 토대로 콜록을 찾아내기에, 콜록은 단서들을 훼손시키더군.\n일시적으로 단서를 발견해도 읽을 수 없게 한다네.\n아주 지독하게도 말이야!",
        "그리고 모든 행동은 홈즈들의 행동과 구분이 되지 않아 아주 골치가 아프다네...",
        "자, 이제 모든 준비가 끝났어. 건투를 빌겠네. 자네에게 행운이 깃들길!",
    };
}
