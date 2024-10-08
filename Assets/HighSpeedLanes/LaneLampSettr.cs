using Newtonsoft.Json;
using System;
using UnityEngine;

public class LaneLampSettr
{
    private bool SetupDone = false;

    public GloballaneManager worldspwanerManager;

    // Start is called before the first frame update

    public void SetupSettr(GloballaneManager globallaneManager)
    {
        this.worldspwanerManager = globallaneManager;
        SetupDone = true;
    }

    public void DecodeJappie(string jappei)
    {
        recieverpakket.SignalGroup gr;

        try
        {
            gr = JsonConvert.DeserializeObject<recieverpakket.SignalGroup>(jappei);
        }
        catch(Exception)
        {
            return;
            throw;
        }

        Debug.Log(jappei);
        if(SetupDone)
        {
            updateLamps(gr);
        }
    }

    public void updateLamps(recieverpakket.SignalGroup lightCar)
    {
        SetBlockLampsCarLaneA(lightCar.blocksMsg);
        SetBlockLampsCarLaneB(lightCar.blocksMsg2);
        SetAllBikePedLamps(lightCar);
    }

    public void SetAllBikePedLamps(recieverpakket.SignalGroup message)
    {
        recieverpakket.blocksMsg msg = message.blocksMsg;
        worldspwanerManager.PrebuiltLaneA.GetComponent<PrebuildBlockInfo>().Setlamps(msg.A.bikers, msg.A.Walkers);
        worldspwanerManager.PrebuiltLaneB.GetComponent<PrebuildBlockInfo>().Setlamps(msg.B.bikers, msg.B.Walkers);
        recieverpakket.blocksMsg2 msgn = message.blocksMsg2;
        worldspwanerManager.PrebuiltLaneE.GetComponent<PrebuildBlockInfo>().Setlamps(msgn.E.bikers, msgn.E.Walkers);
        worldspwanerManager.PrebuiltLaneF.GetComponent<PrebuildBlockInfo>().Setlamps(msgn.F.bikers, msgn.F.Walkers);
    }

    public void SetBlockLampsCarLaneA(recieverpakket.blocksMsg msg)
    {
        for(int i = 0; i < 4; i++)
        {

            worldspwanerManager.Carlanes[i].GetComponentInChildren<CarLanebehaviour>().SetLampLight(msg.A.Cars[i]);

        }

        for(int i = 4; i < 4 + 4; i++)
        {
            worldspwanerManager.Carlanes[i].GetComponentInChildren<CarLanebehaviour>().SetLampLight(msg.B.Cars[i - 4]);
        }
        for(int i = 4 + 4; i < 4 + 4 + 4; i++)
        {
            worldspwanerManager.Carlanes[i].GetComponentInChildren<CarLanebehaviour>().SetLampLight(msg.C.Cars[i - (4 + 4)]);
        }
    }

    public void SetBlockLampsCarLaneB(recieverpakket.blocksMsg2 msg)
    {
        for(int i = 0; i < 4; i++)
        {
            worldspwanerManager.CarlanesB[i].GetComponentInChildren<CarLanebehaviour>().SetLampLight(msg.D.Cars[i]);
        }

        for(int i = 4; i < 4 + 3; i++)
        {
            worldspwanerManager.CarlanesB[i].GetComponentInChildren<CarLanebehaviour>().SetLampLight(msg.E.Cars[i - 4]);
        }
        for(int i = 4 + 3; i < 4 + 3 + 4; i++)
        {
            worldspwanerManager.CarlanesB[i].GetComponentInChildren<CarLanebehaviour>().SetLampLight(msg.F.Cars[i - (3 + 4)]);
        }
    }
}