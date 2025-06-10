using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollGround : MonoBehaviour
{
    [Header("スクロール速度")]
    [SerializeField] private float speed = 5.0f;

    [Header("タイルを削除するワールド座標の閾値（x）")]
    [SerializeField] private float endPosition;

    [Header("Ground 用 Tilemap")]
    [SerializeField] private Tilemap blockTilemap;

    // 最初にタイルマップのセル範囲をキャッシュしておく
    private BoundsInt tileBounds;

    private void Start()
    {

        tileBounds = blockTilemap.cellBounds;
    }

    private void Update()
    {
        // 1. 左方向にスクロール
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 2. 各セルをチェックして、指定位置より左に行ったタイルを削除
        foreach (Vector3Int cellPos in tileBounds.allPositionsWithin)
        {
            // タイルが無ければスキップ
            if (!blockTilemap.HasTile(cellPos))
                continue;

            // セル座標→ワールド座標
            Vector3 worldPos = blockTilemap.CellToWorld(cellPos);

            // endPosition より左（x <= endPosition）なら削除
            if (worldPos.x <= endPosition)
            {
                blockTilemap.SetTile(cellPos, null);
            }
        }
    }
}