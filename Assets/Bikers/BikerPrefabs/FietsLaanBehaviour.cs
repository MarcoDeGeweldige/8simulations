using UnityEngine;

public class FietsLaanBehaviour : MonoBehaviour
{
    public LampostManager LampostManager;
    public void SetLampLight(int state)
    {
        LampostManager.SetLight(state);
    }
}