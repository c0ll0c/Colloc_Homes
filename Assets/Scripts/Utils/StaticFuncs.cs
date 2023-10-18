using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StaticFuncs 
{
    // 3개의 숫자와 2개의 영어로 된 플레이어 코드 반환
    public static string GeneratePlayerCode()
    {
        string characters = "0123456789ABCDEF";
        StringBuilder result = new StringBuilder();

        // 5자리의 랜덤 문자열 생성
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, characters.Length);
            result.Append(characters[randomIndex]);
        }

        return result.ToString();
    }
}
