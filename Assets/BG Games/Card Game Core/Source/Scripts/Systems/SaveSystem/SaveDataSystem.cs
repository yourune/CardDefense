using BG_Games.Card_Game_Core.Systems.PlayerProfile.Statistics;

namespace BG_Games.Card_Game_Core.Systems.SaveSystem
{
    public abstract class SaveDataSystem<T> where T : class, new()
    {
        protected abstract string Key { get; }
        private T _playerData;
            
        public virtual T Load()
        {
            if (_playerData == null)
            {
                _playerData = SaveService.Load(Key, new T());
            }
                
            return _playerData;
        }
        
        public virtual void Save(T data)
        {
            _playerData = data;
            SaveService.Save(_playerData, Key);
        }

        public void Save()
        {
            Save(Load());
        }

        public virtual void Delete()
        {
            _playerData = null;
            SaveService.Delete(Key);
        }
    }
}
