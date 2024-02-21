using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class StaticFuncs 
{
    // just shuffle...
    public static string ShuffleString(string _characters)
    {
        System.Random rand = new System.Random();

        StringBuilder result = new StringBuilder(_characters);

        for (int i = result.Length - 1; i > 0; i--)
        {
            int index = rand.Next(i + 1);

            char temp = result[index];
            result[index] = result[i];
            result[i] = temp;
        }

        return result.ToString();
    }

    public static string GeneratePlayerCode(string _commoncharacters)
    {
        string code;
        string randomCharacters = "0123456789ABCDEFGHIJYZ";

        randomCharacters = ShuffleString(randomCharacters).Substring(0, 2);          // �������� �� ����
        code = _commoncharacters + randomCharacters;

        return ShuffleString(code);
    }

    // sprite renderer setting
    public static void SpriteRendering(GameObject _gameObject)
    {
        for (int i = NetworkManager.Instance.PlaySceneManager.LayerGrass.Length; i > 0; i--)
        {
            Vector3Int cellPosition = NetworkManager.Instance.PlaySceneManager.LayerGrass[i - 1].WorldToCell(_gameObject.transform.position);
            TileBase tile = NetworkManager.Instance.PlaySceneManager.LayerGrass[i - 1].GetTile(cellPosition);

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
        for (int i = 1; i <= NetworkManager.Instance.PlaySceneManager.LayerWall.Length; i++)
        {
            Vector3Int cellPosition = NetworkManager.Instance.PlaySceneManager.LayerWall[i - 1].WorldToCell(_pos);
            TileBase tile = NetworkManager.Instance.PlaySceneManager.LayerWall[i - 1].GetTile(cellPosition);

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
        effect.GetComponent<SpriteRenderer>().sortingLayerID = effect.transform.parent.GetComponentInParent<SpriteRenderer>().sortingLayerID;

        if (effect != NetworkManager.Instance.PlaySceneManager.LocalRPC.InfectEffect)
        {
            yield return WaitForSeconds(StaticVars.DELAY_TIME);
            StopEffect(effect);
        }
    }

    public static void StopEffect(GameObject effect)
    {
        effect.SetActive(false);
    }

    public static string GetColorName(int _colorCode)
    {
        return _colorCode switch
        {
            0b000000001 => "Brown",
            0b000000010 => "Gray",
            0b000000100 => "Pink",
            0b000001000 => "Red",
            0b000010000 => "Orange",
            0b000100000 => "Yellow",
            0b001000000 => "Green",
            0b010000000 => "Blue",
            0b100000000 => "Purple",
            _ => "",
        };
    }

    public static string WrapNameWithColor(string _color, string _name)
    {
        string colorHex = _color switch {
            "Brown" => "#7B5220FF",
            "Gray" => "#928FB6FF",
            "Pink" => "#FC4780FF",
            "Red" => "#AE0C0CFF",
            "Orange" => "#F48859FF",
            "Yellow" => "#F5CC50FF",
            "Green" => "#28C175FF",
            "Blue" => "#498BD3FF",
            "Purple" => "#925CFFFF",
            _ => "",
        };

        StringBuilder sb = new StringBuilder("<color=");
        sb.Append(colorHex);
        sb.Append(">");
        sb.Append(_name);
        sb.Append("</color>");
        return sb.ToString();
    }

    public static void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
