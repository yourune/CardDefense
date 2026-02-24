using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace BG_Games.Card_Game_Core.Systems
{
    public class InputLockSystem
    {
        private List<ISupportLockInput> _gameplayInputReaders;
        private EventSystem _eventSystem;

        public InputLockSystem(EventSystem eventSystem)
        {
            _eventSystem = eventSystem;
            _gameplayInputReaders = new List<ISupportLockInput>();
        }

        public void AddInputReader(ISupportLockInput input)
        {
            _gameplayInputReaders.Add(input);
        }

        public void RemoveInputReader(ISupportLockInput input)
        {
            _gameplayInputReaders.Remove(input);
        }

        /// <summary>
        /// Disables input in gameplay modules such as cards or hero
        /// </summary>
        /// <param name="state">is enabled</param>
        public void SetEnabledGamplayInput(bool state)
        {
            foreach (var inputReader in _gameplayInputReaders)
            {
                inputReader.SetEnabledInputReading(state);
            }
        }

        /// <summary>
        /// Disables input reading in gameplay modules also UI
        /// </summary>
        /// <param name="state">is enabled</param>
        /// <param name="sender">sender</param>
        public void SetEnabledAllInput(bool state)
        {
            SetEnabledGamplayInput(state);
            _eventSystem.enabled = state;
        }
    }
}
