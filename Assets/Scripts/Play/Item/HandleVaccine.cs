using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.Tilemaps;

// vaccine function script
public class HandleVaccine : PoolAble
{
    private void Start()
    {
        VaccineSpriteRender();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ReleaseObject();
    }

    // sprite renderer setting
    private void VaccineSpriteRender()
    {
        for (int i = 1; i <= PlayManager.Instance.layer.Length; i++)
        {
            Vector3Int cellPosition = PlayManager.Instance.layer[i - 1].WorldToCell(gameObject.transform.position);
            TileBase tile = PlayManager.Instance.layer[i - 1].GetTile(cellPosition);

            if (tile != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer " + i;
            }
        }
    }
}
