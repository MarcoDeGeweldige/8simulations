using System.Collections.Generic;
using UnityEngine;

public class LampostManager : MonoBehaviour
{
    public bool IstandAlon = false;
    public bool BuildOnStart = false;
    public bool Isgreen = false;

    //the internal behaviour for logic related stuff
    public ColourLogic ColourLogic;

    public LampWatch watch;

    public List<Lighting> lightings = new List<Lighting>();

    private void Start()
    {
        if(IstandAlon)
        {
            return;
        }
        LaneCommunicator communicator = GetComponentInParent<LaneCommunicator>();
        LaneId tempid = communicator.GetLaneId();
        int templanenum = communicator.GetComlaneNr();
        NodeType type = communicator.type;
        DirectLampLink.AssignTomanager(this, tempid, type, templanenum);
        if(BuildOnStart)
        {
            SetUpLights();
        }
    }

    public void SetUpLights()
    {
        lightings.AddRange(this.GetComponentsInChildren<Lighting>());

        foreach(Lighting lighting in lightings)
        {
            lighting.setup();
        }
        ConfigureLogic();

        AddLightsToLogic(lightings);
        ColourLogic.setLampToRed();
        ColourLogic.setLampToGreen();
    }

    //called on setup
    public void ConfigureLogic()
    {
    
        ColourLogic = this.gameObject.AddComponent<ColourLogic>();

        ColourLogic.setup(lightings.Count - 1);
    }

    public void AddLightsToLogic(List<Lighting> lightings)
    {
        foreach(Lighting lighting in lightings)
        {
            ColourLogic.AddItemToLogicM(lighting);
        }
    }

    public void SetToGreen()
    {
        ColourLogic.setLampToGreen();
    }

    public void SetToOrange()
    {
        ColourLogic.setLampToOrange();
    }

    public void SetToRed()
    {
        ColourLogic.setLampToRed();
    }

    public void SetLight(int index)
    {
        ColourLogic.UpdateState(index);
    }

    public int GetLightState()
    {
        return ColourLogic.CurrentState;
    }

}