using UnityEngine;

namespace Game.Code.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameInstaller _gameInstaller;
        
        private void Awake()
        {
            _gameInstaller = new GameInstaller();
            _gameInstaller.InitLevel();
            
            DontDestroyOnLoad(this);
        }
    }
}
