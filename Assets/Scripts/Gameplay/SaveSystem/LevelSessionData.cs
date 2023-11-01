using System;

namespace Pelki.Gameplay.SaveSystem
{
    //sttrox: подумать название что бы отличать явно что это сохранения
    public class LevelSessionData
    {
        private IGameProgressSaver gameProgressSaver;
        private Action<object> _savingDataDelegat;

        public string SavePointId { get; set; }

        //public event Action<object> Saved;

        public void Save()
        {
            //sttox: вариант через событие
            //Saved?.Invoke(this);
            //sttrox: вариант через прямой вызов сохранения
            gameProgressSaver.SaveObject(this);
            //sttrox: вариант через делегат
            //_savingDataDelegat?.Invoke(this);
        }

        public void Initialize(IGameProgressSaver savesStorage)
        {
            gameProgressSaver = savesStorage;
        }

        /*public void Initialize(Action<object> savingDataDelegat)
        {
            _savingDataDelegat = savingDataDelegat;
        }*/
    }
}