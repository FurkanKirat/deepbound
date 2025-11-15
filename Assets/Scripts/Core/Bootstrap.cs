using Core.Context;
using GameLoop;
using Systems.InputSystem;
using Systems;
using Visuals.CameraScripts;
using Visuals.ObjectVisuals;
using Visuals.Rendering;
using UnityEngine;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private EntityVisualizer entityVisualizer;
        [SerializeField] private BlockRenderer meshRenderer;
        [SerializeField] private SceneContextInjector sceneContextInjector;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private LogicGameLoop logicGameLoop;
        
        private void Awake()
        {
            var session = GameRoot.Instance.GameSession;
            var gameManager = session.GameManager;
            
            var worldContext = new ClientContext
            {
                World = gameManager.World,
                CameraManager = cameraManager
            };
            
            var initManager = new InitializeManager();
            
            cameraManager.Initialize(worldContext);
            meshRenderer.Init(gameManager.World);
            initManager.Register(gameManager);
            initManager.InitializeAll();

            
            var inputManager = new InputManager(worldContext);
            sceneContextInjector.Inject(worldContext);
            
            var tickContext = new TickContext
            {
                World = gameManager.World,
                GameStateManager = gameManager.GameStateManager,
            };
            
            session.RegisterDisposable(inputManager);
            session.RegisterDisposable(gameManager);
            
            logicGameLoop.Initialize(gameManager.TickManager, tickContext);
        }
    }

}