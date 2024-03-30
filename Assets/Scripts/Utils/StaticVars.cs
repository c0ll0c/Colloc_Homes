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
    public static float NPC_TIME = 120.0f;  // can talk to npc at 3m
    public static float Hidden_TIME = 20.0f;
    public static float Ending_TIME = 6.0f;
    public static float VACCINE_TIME = 50.0f; // 50s
    public static int COLLOC_WIN_TIME = 30; //30s
    public static float EVENT_OCCUR_TIME = 295f; // 200f // event happens at 1m 40s

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
    };

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
}
