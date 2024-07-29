using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinecraftConnection;
using System;

public class MinecraftBox : MonoBehaviour
{
    public GameObject cube; // アタッチされたCubeオブジェクトを参照
    private MinecraftCommands command;

    // Start is called before the first frame update
    void Start()
    {
        string address = "127.0.0.1";
        ushort port = 25575;
        string pass = "minecraft";
        command = new MinecraftCommands(address, port, pass);

        StartCoroutine(VoxelizeModel());
    }

    IEnumerator VoxelizeModel()
    {
        command.DisplayTitle("地形自動生成開始");

        string playerName = "applewindows";

        int posX = (int)command.GetPlayerData(playerName).Position.X;
        int posY = (int)command.GetPlayerData(playerName).Position.Y;
        int posZ = (int)command.GetPlayerData(playerName).Position.Z;

        Vector3 cubeSize = cube.transform.localScale;

        float cubeX = cube.transform.position.x;
        float cubeZ = cube.transform.position.z;
        float cubeY = cube.transform.position.y;

        for (float y = 0; y < 200 * cubeSize.y; y += cubeSize.y * 2)
        {
            for (float x = 0; x < 1700 * cubeSize.x; x += cubeSize.x * 2)
            {
                for (float z = 0; z < 2100 * cubeSize.z; z += cubeSize.z * 2)
                {
                    // アタッチされたCubeの位置を設定する
                    cube.transform.position = new Vector3(cubeX + x, cubeY + y, cubeZ + z);
                    // 衝突チェックを行う
                    Collider[] colliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2);
                    foreach (Collider collider in colliders)
                    {
                        if (collider.GetComponent<MeshCollider>() != null)
                        {
                            // 3x3x3のエリアにブロックを設置
                            int blockX = posX + (int)x;
                            int blockY = posY + (int)y * 1;
                            int blockZ = posZ + (int)z;

                            for (int dy = -1; dy <= 1; dy++)
                            {
                                for (int dx = -1; dx <= 1; dx++)
                                {
                                    for (int dz = -1; dz <= 1; dz++)
                                    {
                                        command.SetBlock(blockX + dx, blockY + dy, blockZ + dz, "stone");
                                    }
                                }
                            }
                        }
                    }


                    // 動きを見やすくするために待機時間を追加
                    yield return null;//new WaitForSeconds(0.0f);
                }
            }
        }
    }
}