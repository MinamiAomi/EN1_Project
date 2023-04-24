using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject boxPrefab;

    int[,] map;
    GameObject[,] field;

    // Start is called before the first frame update
    void Start() {

        map = new int[,] {
           { 0, 0, 0, 0, 0 },
           { 0, 2, 1, 2, 0 },
           { 0, 0, 0, 2, 0 } };
        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)];

        Vector3 mapCenterOffset = new Vector3(field.GetLength(0) / 2, field.GetLength(1) / 2, 0);
       
        for (int y = 0; y < field.GetLength(0); y++) {
            for (int x = 0; x < field.GetLength(1); x++) {
                if (map[y, x] == 1) {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, field.GetLength(0) - y, 0) - mapCenterOffset,
                        Quaternion.identity);
                }
                if (map[y, x] == 2) {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, field.GetLength(0) - y, 0) - mapCenterOffset,
                        Quaternion.identity);
                }
            }
        }
    }

   // Update is called once per frame
   void Update() {

        // 右移動処理
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            var playerIndex = GetPlayerIndex();
            // 右のマスを1にし元のマスを0にする
            MoveNumber("Player", playerIndex, playerIndex + Vector2Int.right);
        }

        // 左移動処理
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            var playerIndex = GetPlayerIndex();
            // 右のマスを1にし元のマスを0にする
            MoveNumber("Player", playerIndex, playerIndex + Vector2Int.left);
        }

        // 右移動処理
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            var playerIndex = GetPlayerIndex();
            // 右のマスを1にし元のマスを0にする
            MoveNumber("Player", playerIndex, playerIndex + Vector2Int.down);
        }

        // 左移動処理
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            var playerIndex = GetPlayerIndex();
            // 右のマスを1にし元のマスを0にする
            MoveNumber("Player", playerIndex, playerIndex + Vector2Int.up);
        }
    }
    Vector2Int GetPlayerIndex() {
        // プレイヤ―の位置を探す 見つからなかったら-1
        for (int y = 0; y < field.GetLength(0); y++) {
            for (int x = 0; x < field.GetLength(1); x++) {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo) {
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1) ||
            moveTo.y < 0 || moveTo.y >= field.GetLength(0)) {
            return false;
        }

        if(field[moveTo.y,moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box") {
            Debug.Log("Box!!!");
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        Vector3 mapCenterOffset = new Vector3(field.GetLength(0) / 2, field.GetLength(1) / 2, 0);
        field[moveFrom.y, moveFrom.x].transform.position = 
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0) - mapCenterOffset;

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }
}
