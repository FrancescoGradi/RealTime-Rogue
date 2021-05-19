using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCellView : MonoBehaviour {
    
    public List<GameObject> cellSpawnPoints;
    public int envObjectsLayer = 12;
    public int playerLayer = 11;
    public int enemyLayer = 9;
    public int itemsLayer = 10;
    public float cellRadius = 0.9f;

    private Collider[] bufferColliders = new Collider[1];
    private Collider hit;

    public List<float> GetLocalCellView() {

        List<float> localCellView = new List<float>();

        foreach (GameObject cellSpawnPoint in cellSpawnPoints) {
            
            int n_hits = Physics.OverlapBoxNonAlloc(cellSpawnPoint.transform.position, new Vector3(cellRadius, cellRadius, cellRadius), bufferColliders);

            if (n_hits == 0) {
                DrawX(cellSpawnPoint.transform.position, Color.white);
                localCellView.Add(0);
            } else {
                hit = bufferColliders[0];
                if (hit != null) {
                    if (hit.transform.gameObject.layer == itemsLayer) {
                        // Qua dobbiamo adesso distinguere tra Health Potion, 
                        if (hit.gameObject.GetComponent<BastardSword>() != null) {
                            DrawX(cellSpawnPoint.transform.position, Color.magenta);
                            localCellView.Add(6);
                        } else if (hit.gameObject.GetComponent<GoldenShield>() != null) {
                            DrawX(cellSpawnPoint.transform.position, Color.green);
                            localCellView.Add(5);
                        } else if (hit.gameObject.GetComponent<HealthPotion>() != null) {
                            DrawX(cellSpawnPoint.transform.position, Color.blue);
                            localCellView.Add(4);
                        }
                    } else if (hit.transform.gameObject.layer == enemyLayer) {
                        DrawX(cellSpawnPoint.transform.position, Color.gray);
                        localCellView.Add(3);
                    } else if (hit.transform.gameObject.layer == playerLayer) {
                        DrawX(cellSpawnPoint.transform.position, Color.red);
                        localCellView.Add(2);
                    } else if (hit.transform.gameObject.layer == envObjectsLayer) {
                        DrawX(cellSpawnPoint.transform.position, Color.yellow);
                        localCellView.Add(1);
                    }  else {
                        DrawX(cellSpawnPoint.transform.position, Color.white);
                        localCellView.Add(0);
                    } 
                } else {
                    DrawX(cellSpawnPoint.transform.position, Color.white);
                    localCellView.Add(0);
                } 
            }
        }

        return localCellView;
    }

    public void SetPosition(float x, float z) {
        this.gameObject.transform.position = new Vector3(x, this.gameObject.transform.position.y, z);
    }

    private void DrawX(Vector3 pos, Color color) {
        if (playerLayer == 11) {
            Debug.DrawLine(new Vector3(pos.x + 0.5f, pos.y, pos.z), new Vector3(pos.x - 0.5f, pos.y, pos.z), color, Time.fixedDeltaTime * 5);
            Debug.DrawLine(new Vector3(pos.x, pos.y, pos.z + 0.5f), new Vector3(pos.x, pos.y, pos.z - 0.5f), color, Time.fixedDeltaTime * 5);
        }
    }   
}
