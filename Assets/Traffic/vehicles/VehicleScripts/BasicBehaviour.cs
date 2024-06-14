using UnityEngine;

//public class BasicBehaviour : MonoBehaviour
//{
//    public ActorPathFinding pad;
//    private bool isWaiting;
//    private bool CanUpdate;
//    private bool IsBus;
//    private bool IsGreenLight;

//    public void SetUp()
//    {
//        pad.watch.OnCanGoChanged += Watch_OnCanGoChanged;

//    }

//    private void Watch_OnCanGoChanged(bool cango, bool hasBus)
//    {
//        if (isWaiting)
//        {
//            if (hasBus)
//            {
//                if (IsBus)
//                {
//                    this.IsGreenLight = cango;
//                    if (cango == true)
//                    {
//                        this.isWaiting = false;
//                        this.CanUpdate = cango;
//                    }
//                    return;
//                }

//                this.IsGreenLight = false;
//            }
//            else
//            {
//                this.IsGreenLight = cango;
//                this.isWaiting = false;
//                this.CanUpdate = cango;
//            }
//        }

//    }
//}

public class BasicBehaviour : MonoBehaviour
{
    //public ActorPathFinding pad;
    //private bool isWaiting;
    //private bool canUpdate;
    //private bool isBus;
    //private bool isGreenLight;

    //private LaneBridge bridge;

    ////this is an rework for the ai basics
    //public void SetUp()
    //{
    //    bridge.HasGreenLight += Bridge_HasGreenLight;
    //}

    //private void Bridge_HasGreenLight(bool canGo, bool hasBus)
    //{
    //    if (isWaiting)
    //    {
    //        if (hasBus)
    //        {
    //            if (isBus)
    //            {
    //                isGreenLight = canGo;
    //                if (canGo)
    //                {
    //                    isWaiting = false;
    //                    canUpdate = canGo;
    //                }
    //                return;
    //            }

    //            isGreenLight = false;
    //        }
    //        else
    //        {
    //            isGreenLight = canGo;
    //            isWaiting = canGo;
    //            canUpdate = canGo;
    //        }
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (bridge != null)
    //    {
    //        bridge.HasGreenLight -= Bridge_HasGreenLight;
    //    }
    //}
}