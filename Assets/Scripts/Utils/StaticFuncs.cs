using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    // sprite renderer setting
    public static void SpriteRendering(GameObject gameObject)
    {
        for (int i = 1; i <= PlayManager.Instance.LayerGrass.Length; i++)
        {
            Vector3Int cellPosition = PlayManager.Instance.LayerGrass[i - 1].WorldToCell(gameObject.transform.position);
            TileBase tile = PlayManager.Instance.LayerGrass[i - 1].GetTile(cellPosition);

            if (tile != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer " + i;
            }
        }
    }

   // check position on wall
   public static bool CheckOnWall(Vector3 pos)
    {
        for (int i = 1; i <= PlayManager.Instance.LayerWall.Length; i++)
        {
            Vector3Int cellPosition = PlayManager.Instance.LayerWall[i - 1].WorldToCell(pos);
            TileBase tile = PlayManager.Instance.LayerWall[i - 1].GetTile(cellPosition);

            if (tile != null)
            {
                return true;
            }
        }
        return false;
    }
}
