using UnityEngine;

public class RoleCounter : MonoBehaviour
{
    [SerializeField] private GameObject[] mafiaAlivePoints;
    [SerializeField] private GameObject[] mafiaDeadPoints;
    [SerializeField] private GameObject[] peacefulAlivePoints;
    [SerializeField] private GameObject[] peacefulDeadPoints;

    private void CleanRolePoints()
    {
        foreach (GameObject mafiaAlivePoint in mafiaAlivePoints)
        {
            mafiaAlivePoint.SetActive(false);
        }
        foreach (GameObject mafiaDeadPoint in mafiaDeadPoints)
        {
            mafiaDeadPoint.SetActive(false);
        }
        foreach (GameObject peacefulAlivePoint in peacefulAlivePoints)
        {
            peacefulAlivePoint.SetActive(false);
        }
        foreach (GameObject peacefulDeadPoint in peacefulDeadPoints)
        {
            peacefulDeadPoint.SetActive(false);
        }
    }
    public void DisplayRolePoints(int aliveMafia, int deadMafia, 
        int alivePeaceful, int deadPeaceful)
    {
        CleanRolePoints();
        for (int i = 1; i <= aliveMafia; i++)
        {
            mafiaAlivePoints[i].SetActive(true);
        }
        for (int i = 1; i <= deadMafia; i++)
        {
            mafiaDeadPoints[i].SetActive(true);
        }
        for (int i = 1; i <= alivePeaceful; i++)
        {
            peacefulAlivePoints[i].SetActive(true);
        }
        for (int i = 1; i <= deadPeaceful; i++)
        {
            peacefulDeadPoints[i].SetActive(true);
        }
    }
}
