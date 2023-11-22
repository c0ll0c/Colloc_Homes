using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Tilemaps;

public static class StaticFuncs 
{
    public static string GeneratePlayerCode(string _commoncharacters)
    {
        string code;
        string randomCharacters = "0123456789ABCDEFGHIJYZ";

        randomCharacters = NetworkManager.Instance.ShuffleCharacter(randomCharacters).Substring(0, 2);          // 랜덤으로 세 글자
        code = _commoncharacters + randomCharacters;

        return NetworkManager.Instance.ShuffleCharacter(code);
    }

    // sprite renderer setting
    public static void SpriteRendering(GameObject _gameObject)
    {
        for (int i = PlayManager.Instance.LayerGrass.Length; i > 0; i--)
        {
            Vector3Int cellPosition = PlayManager.Instance.LayerGrass[i - 1].WorldToCell(_gameObject.transform.position);
            TileBase tile = PlayManager.Instance.LayerGrass[i - 1].GetTile(cellPosition);

            if (tile != null)
            {
                _gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer " + i;
                return;
            }
        }
    }

   // check position on wall
   public static bool CheckOnWall(Vector3 _pos)
    {
        for (int i = 1; i <= PlayManager.Instance.LayerWall.Length; i++)
        {
            Vector3Int cellPosition = PlayManager.Instance.LayerWall[i - 1].WorldToCell(_pos);
            TileBase tile = PlayManager.Instance.LayerWall[i - 1].GetTile(cellPosition);

            if (tile != null)
            {
                return true;
            }
        }
        return false;
    }

    // compare float
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    // reuse WaitForSeconds
    private static readonly Dictionary<float, WaitForSeconds> _WaitForSeconds = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    public static WaitForSeconds WaitForSeconds(float _seconds)
    {
        WaitForSeconds wfs;
        if (!_WaitForSeconds.TryGetValue(_seconds, out wfs))
        {
            _WaitForSeconds.Add(_seconds, wfs = new WaitForSeconds(_seconds));
        }
        return wfs;
    }

    // player effect
    public static IEnumerator SetEffect(GameObject effect)
    {
        effect.SetActive(true);
        yield return StaticFuncs.WaitForSeconds(3.0f);
        effect.SetActive(false);
    }
}
