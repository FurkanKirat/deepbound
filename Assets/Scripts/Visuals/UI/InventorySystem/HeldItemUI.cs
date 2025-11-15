using Core;
using Core.Events;
using Data.Models.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Visuals.Interfaces;

namespace Visuals.UI.InventorySystem
{
    public class HeldItemUI : MonoBehaviour, IClientTickable
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemAmountText;

        private bool _isActive = false;
        
        private void OnEnable()
        {
            ClientGameLoop.Instance.Register(this);
            GameEventBus.Subscribe<HeldItemChangedEvent>(OnHeldItemChanged);
        }

        private void OnDisable()
        {
            ClientGameLoop.Instance?.Unregister(this);
            GameEventBus.Unsubscribe<HeldItemChangedEvent>(OnHeldItemChanged);
        }

        private void OnHeldItemChanged(HeldItemChangedEvent e)
        {
            var item = e.NewItem;

            if (item.IsEmpty)
            {
                if (_isActive)
                    root.SetActive(false);
                
                _isActive = false;
            }
            else
            {
                if (!_isActive)
                    root.SetActive(true);
                
                itemImage.sprite = item.GetSprite();
                itemAmountText.text = item.FormatItemCount();
                _isActive = true;
            }
                
        }

        public void ClientTick(float deltaTime)
        {
            root.transform.position = Input.mousePosition;
        }
        
    }

}