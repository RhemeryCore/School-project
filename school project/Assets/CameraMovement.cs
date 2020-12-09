using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    #region Init values
    public GameObject MyCamera;
    public GameObject GameController;
    public float sensitive;
    public GameObject[] BuildingArray;
    public GameObject[] HitAreaArray;
    public GameObject[] HitAreaRedArray;
    public static float[] HitAreaRadius = { 0.5f, 1, 2};
    public GameObject BulletObj;
    public GameObject CurrentPlayerText;
    public static bool SomeoneHit = false;

    public GameObject EndGameRestart;

    public static int[] BuildingMax = { 5, 5, 3, 2, 1 };
    public static int[] BuildingPlayerPlaced = { 0, 0, 0, 0, 0 };
    public static int[] BuildingEnemyPlaced = { 0, 0, 0, 0, 0 };

    public static bool UpdateBuildingGhost = true;
    public static bool UpdateHitAreaGhost = true;
    public static GameObject BuildingGhost;
    public static GameObject HitAreaGhost;
    public static GameObject HitArea;
    public static GameObject HitAreaRed;

    public static int selectedStructure = -1;
    public static int selectedHitArea = 1;

    public static string GameState = "Preaparation";
    public static int CurrentPlayer = 0;

    public static bool PlayerShot = false;

    public float CameraSizeZoom = 0f;

    private bool cameraInitVal = false;

    public static int CameraWaitMove = 0;

    public static int __currentEnemyBuildingIndex = 0;
    public static bool enemyBuilding = false;

    #endregion

    void Start()
    {
        GameState = "Preaparation";
        UpdateBuildingGhost = true;

        EndGameRestart.gameObject.SetActive(false);

        CoreSystem.system_log_write("Game Init");
        enemyBuilding = true;
        for (__currentEnemyBuildingIndex = 0; __currentEnemyBuildingIndex < BuildingArray.Length; __currentEnemyBuildingIndex++)
        {
            for (int j = 0; j < BuildingMax[__currentEnemyBuildingIndex]; j++)
            {
                var obj = CoreSystem.instance_create(Random.Range(85, 115), 0.5f, Random.Range(-10, -40), BuildingArray[__currentEnemyBuildingIndex]);
                obj.GetComponent<BuildingScript>().BuildingIndex = __currentEnemyBuildingIndex;
                obj.GetComponent<BuildingScript>().EnemyTag = true;
                obj.GetComponent<BuildingScript>().health = 100 + (25 * __currentEnemyBuildingIndex);
                BuildingEnemyPlaced[__currentEnemyBuildingIndex]++;
            }
        }

        /*enemyBuilding = true;
        for (__currentEnemyBuildingIndex = 0; __currentEnemyBuildingIndex < BuildingArray.Length; __currentEnemyBuildingIndex++)
        {
            for (int j = 0; j < BuildingMax[__currentEnemyBuildingIndex]; j++)
            {
                var obj = CoreSystem.instance_create(Random.Range(-85, -115), 0.5f, Random.Range(-10, -40), BuildingArray[__currentEnemyBuildingIndex]);
                obj.GetComponent<BuildingScript>().BuildingIndex = __currentEnemyBuildingIndex;
                obj.GetComponent<BuildingScript>().EnemyTag = false;
                obj.GetComponent<BuildingScript>().health = 100 + (25 * __currentEnemyBuildingIndex);
                BuildingPlayerPlaced[__currentEnemyBuildingIndex]++;
            }
        }*/
        BuildingGhost = CoreSystem.instance_create(0, 1, 0, BuildingArray[0]);

        sensitive = 0.1f;

        MyCamera.transform.position = new Vector3(-100, 6, -25);
        CoreSystem.system_log_write("Game end Init");
    }
    
    void Update()
    {
        if (GameState == "Preaparation")
        {
            CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text = "Build structures";
            if (UpdateBuildingGhost == true)
            {
                //Debug.Log("created");
                Destroy(BuildingGhost);
                if (selectedStructure != -1)
                {
                    BuildingGhost = CoreSystem.instance_create(0, 1, 0, BuildingArray[selectedStructure - 1]);
                }
                else
                {
                    BuildingGhost = CoreSystem.instance_create(0, 1, 0, BuildingArray[0]);
                }
                BuildingGhost.gameObject.tag = "BuildingGhost";
                UpdateBuildingGhost = false;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (UpdateBuildingGhost == false)
                {
                    Vector3 trans = hit.point;
                    BuildingGhost.transform.position = new Vector3(trans.x, 0.5f, trans.z);
                }
                if ((Input.GetMouseButtonDown(0)))
                {
                    if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    {
                        if ((hit.collider.gameObject.tag == "BuildingGhost") && (hit.collider.gameObject.tag != "Structure"))
                        {
                            Vector3 trans = hit.point;
                            if ((selectedStructure != -1) && (BuildingPlayerPlaced[selectedStructure - 1] < (BuildingMax[selectedStructure - 1])) && ((trans.x < -85) && (trans.x > -115) && (trans.z < -10) && (trans.z > -40)))
                            {
                                enemyBuilding = false;
                                var obj = CoreSystem.instance_create(trans.x, 0.5f, trans.z, BuildingArray[selectedStructure - 1]);
                                obj.GetComponent<BuildingScript>().BuildingIndex = selectedStructure - 1;
                                BuildingPlayerPlaced[selectedStructure - 1]++;
                            }
                        }
                    }
                }
            }
        }
        if (GameState == "Battle")
        {
            if (CurrentPlayer == 0)
            {
                if (UpdateHitAreaGhost == true)
                {
                    Destroy(HitAreaGhost);
                    if (selectedHitArea != -1)
                    {
                        HitAreaGhost = CoreSystem.instance_create(0, 1, 0, HitAreaArray[selectedHitArea]);
                    }
                    else
                    {
                        HitAreaGhost = CoreSystem.instance_create(0, 1, 0, HitAreaArray[0]);
                    }
                    HitAreaGhost.gameObject.tag = "HitAreaGhost";
                    UpdateHitAreaGhost = false;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if ((UpdateHitAreaGhost == false) && (HitAreaGhost != null))
                    {
                        Vector3 trans = hit.point;
                        HitAreaGhost.transform.position = new Vector3(trans.x, 0f, trans.z);
                    }
                }

                CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text = "Your turn";
                if ((Input.GetMouseButtonDown(0)))
                {
                    if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    {
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (GameState == "Battle")
                            {
                                if ((hit.collider.gameObject.tag == "Surface") || (hit.collider.gameObject.tag == "HitAreaGhost") || (hit.collider.gameObject.tag == "HitArea"))
                                {
                                    Vector3 trans = hit.point;
                                    if (trans.z > 0)
                                    {
                                        Destroy(CameraMovement.HitAreaGhost);
                                        CoreSystem.instance_create(trans.x, 100, trans.z, BulletObj);
                                        PlayerShot = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text = "Enemy turn";
                if ((PlayerShot == false) && (CameraWaitMove <= 0))
                {
                    EnemyTurn();
                    PlayerShot = true;
                }
            }

            if((selectedHitArea >= 0) && (selectedHitArea < HitAreaArray.Length))
            {
                HitArea = HitAreaArray[selectedHitArea];
                HitAreaRed = HitAreaRedArray[selectedHitArea];
            }

            int __buildingPlayerExistenceCheck = BuildingArray.Length;
            int __buildingEnemyExistenceCheck = BuildingArray.Length;
            for (int i = 0; i < BuildingArray.Length; i++)
            {
                if (BuildingPlayerPlaced[i] <= 0)
                {
                    __buildingPlayerExistenceCheck--;
                }
                if (BuildingEnemyPlaced[i] <= 0)
                {
                    __buildingEnemyExistenceCheck--;
                }
            }
            if (__buildingPlayerExistenceCheck <= 0)
            {
                CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text = "You lost";
                GameState = "End";
                EndGameRestart.gameObject.SetActive(true);
            }
            if (__buildingEnemyExistenceCheck <= 0)
            {
                CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text = "You win";
                GameState = "End";
                EndGameRestart.gameObject.SetActive(true);
            }
        }

        CameraWaitMove--;
        if (GameState == "Battle")
        {
            if (CameraWaitMove <= 0)
            {
                if (CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text == "Your turn")
                {
                    MyCamera.transform.position = new Vector3(-100, CoreSystem.smooth_approach(MyCamera.transform.position.y, 35, 0.1f), CoreSystem.smooth_approach(MyCamera.transform.position.z, 3, 0.1f));
                }
                if (CurrentPlayerText.GetComponent<UnityEngine.UI.Text>().text == "Enemy turn")
                {
                    MyCamera.transform.position = new Vector3(-100, CoreSystem.smooth_approach(MyCamera.transform.position.y, 30, 0.1f), CoreSystem.smooth_approach(MyCamera.transform.position.z, -50, 0.1f));
                }
            }
        }

        if (GameState == "Preaparation")
        {
            if (cameraInitVal == true)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    MyCamera.transform.Translate(Vector3.up * sensitive);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    MyCamera.transform.Translate(Vector3.down * sensitive);
                }
                /*if (Input.GetKey(KeyCode.D))
                {
                    MyCamera.transform.Translate(Vector3.right * sensitive);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    MyCamera.transform.Translate(Vector3.left * sensitive);
                }*/

                
                sensitive -= 0.1f * Mathf.Round(Input.mouseScrollDelta.y);
                sensitive = Mathf.Max(sensitive, 0.5f);
                sensitive = Mathf.Min(sensitive, 1);
                CameraSizeZoom += (Mathf.Round(Input.mouseScrollDelta.y) / 10) + (sensitive * Mathf.Round(Input.mouseScrollDelta.y));
                CameraSizeZoom = CoreSystem.smooth_approach(CameraSizeZoom, 0, 0.1f);

                if (MyCamera.transform.position.y > 2)
                {
                    MyCamera.transform.Translate(Vector3.forward * CameraSizeZoom);
                }
                else
                {
                    MyCamera.transform.Translate(Vector3.forward * -0.1f);
                    CameraSizeZoom = 0;
                }

                MyCamera.transform.position = new Vector3(MyCamera.transform.position.x, Mathf.Min(Mathf.Max(MyCamera.transform.position.y, 5), 40), MyCamera.transform.position.z);
            }
            else
            {
                CameraSizeZoom = 0;
                cameraInitVal = true;
            }
        }
    }

    public static float last_enemyx = 0;
    public static float last_enemyy = 0;

    public static float prev_enemyx = 0;
    public static float prev_enemyy = 0;

    public static bool prev_is_hit = false;

    private void EnemyTurn()
    {
        if(prev_is_hit == false)
        {
            last_enemyx = Random.Range(85f, 115f);
            last_enemyy = Random.Range(10f, 40f);
        }
        else
        {
            last_enemyx = prev_enemyx + Random.Range(-1f, 1f);
            last_enemyy = prev_enemyy + Random.Range(-1f, 1f);
        }

        var enemyTargetx = last_enemyx;
        var enemyTargety = last_enemyy;

        CoreSystem.instance_create(enemyTargetx, 220, enemyTargety, BulletObj);

        if (prev_is_hit == false)
        {
            prev_enemyx = enemyTargetx;
            prev_enemyy = enemyTargety;
        }
    }
}
