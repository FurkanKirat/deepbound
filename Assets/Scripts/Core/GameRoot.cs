using Session;
using Systems.AudioSystem;
using UnityEngine;
using Visuals.UI;

namespace Core
{
    public class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance { get; private set; }
        public GameSession GameSession { get; private set; }
        public SettingsManager SettingsManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        [SerializeField] private CursorManager cursorManager;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            SettingsManager = new SettingsManager();
            GameConfigLoader.Load(this);
            
            GameSession = new GameSession();
            AudioManager = new AudioManager(transform);
            cursorManager.Initialize();
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}