namespace Systems.SaveSystem.Interfaces
{
    public interface ISaveable<out TSaveData> where TSaveData : ISaveData
    {
        TSaveData ToSaveData();
    }
}