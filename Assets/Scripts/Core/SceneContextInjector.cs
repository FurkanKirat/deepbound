using Core.Context;
using Interfaces;
using UnityEngine;
using Visuals.UI.InventorySystem.Panels;

namespace Core
{
    public class SceneContextInjector : MonoBehaviour
    {
        [SerializeField] private HotbarPanel hotbarPanel;
        public void Inject(ClientContext clientContext)
        {
            foreach (var mono in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (mono is IInitializable<ClientContext> playerContextInitializable)
                    playerContextInitializable.Initialize(clientContext);
            }
            
            hotbarPanel.Initialize(clientContext.Player);
        }
    }

}