using System.Collections.Generic;

public interface IGloballaneManager
{
    blockmsgBus AssignblockmsgBusData(List<CarSensormsg> carSensormsgs, List<SingleDetector> bikers, List<SingleDetector> walks, int nr);
    blockmsg AssignblockmsgData(List<CarSensormsg> carSensormsgs, List<SingleDetector> bikers, List<SingleDetector> walks);
    blockmsgCarOnly AssignblocksmsgCarOnlyData(List<CarSensormsg> carSensormsgs);
}