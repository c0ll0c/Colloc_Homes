using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StaticFuncs 
{
    // 3���� ���ڿ� 2���� ����� �� �÷��̾� �ڵ� ��ȯ
    public static string GeneratePlayerCode()
    {
        string characters = "0123456789ABCDEF";
        StringBuilder result = new StringBuilder();

        // 5�ڸ��� ���� ���ڿ� ����
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, characters.Length);
            result.Append(characters[randomIndex]);
        }

        return result.ToString();
    }
}
