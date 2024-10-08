using System.Collections.Generic;
using UnityEngine;

public class ColourLogic : MonoBehaviour
{
    public int CurrentState;

    //this is a bit of the max
    public int Options;

    //list of lamos
    public List<Lighting> LampLights;

    public LampWatch watch;

    //index starts at zero
    public void setup(int options)
    {

        Options = options;
        LampLights = new List<Lighting>();
        //default
        CurrentState = 0;
        watch = GetComponentInParent<LampWatch>();

    }

    //insert in order of priority
    //green yellow orange purple red
    public void AddItemToLogicM(Lighting lighting)
    {
        LampLights.Add(lighting);
    }

    public void UpdateState(int Statenum)
    {
        StatusUpdate(Statenum);
    }

    //tpggle it over
    private void StatusUpdate(int Status)
    {
        switch(Status)
        {
            case 0:
                //setLampToGreen();
                setLampToRed();
                break;

            case 2:
                //setLampToRed();
                setLampToGreen();
                break;

            case 1:
                setLampToOrange();
                break;

            default:
                setLampToOrange();
                break;
        }
    }

    public void setLampToGreen()
    {
        LampLights[1].Status = false;
        LampLights[2].Status = false;

        watch.CanGo = true;
        //0
        CurrentState = 0;
        LampLights[0].Status = true;
    }

    public void setLampToOrange()
    {
        LampLights[0].Status = false;
        LampLights[2].Status = false;
        watch.CanGo = false;
        //1
        CurrentState = 1;
        LampLights[1].Status = true;
    }

    public void setLampToRed()
    {
        watch.CanGo = false;
        LampLights[0].Status = false;
        LampLights[1].Status = false;
        //2
        CurrentState = 2;
        LampLights[2].Status = true;
    }

}