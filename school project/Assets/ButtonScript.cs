using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject PrepHud;

    public void BeginFirst_toggle()
    {
        CameraMovement.CurrentPlayer = 1 + (-CameraMovement.CurrentPlayer);
    }

    public void EndGame_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SelectBuilding_ind1()
    {
        CameraMovement.selectedStructure = 1;
        CameraMovement.UpdateBuildingGhost = true;
    }

    public void SelectBuilding_ind2()
    {
        CameraMovement.selectedStructure = 2;
        CameraMovement.UpdateBuildingGhost = true;
    }

    public void SelectBuilding_ind3()
    {
        CameraMovement.selectedStructure = 3;
        CameraMovement.UpdateBuildingGhost = true;
    }

    public void SelectBuilding_ind4()
    {
        CameraMovement.selectedStructure = 4;
        CameraMovement.UpdateBuildingGhost = true;
    }

    public void SelectBuilding_ind5()
    {
        CameraMovement.selectedStructure = 5;
        CameraMovement.UpdateBuildingGhost = true;
    }


    public void SelectBullet_ind1()
    {
        CameraMovement.selectedHitArea = 0;
    }

    public void SelectBullet_ind2()
    {
        CameraMovement.selectedHitArea = 1;
    }

    public void SelectBullet_ind3()
    {
        CameraMovement.selectedHitArea = 2;
    }


    public void ReadyForBattle()
    {
        CameraMovement.GameState = "Battle";
        Destroy(CameraMovement.BuildingGhost);
        Destroy(PrepHud);
    }
}
