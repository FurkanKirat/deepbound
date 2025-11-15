using System;
using Systems.InventorySystem;
using Visuals.UI.InventorySystem.Panels;
using System.Collections.Generic;
using Core;
using Core.Events;
using Data.Database;
using Data.Models.Crafting;
using UnityEngine;
using Visuals.UI.CraftingSystem;

namespace Visuals.UI.InventorySystem.UIControllers
{
    public class InventoryRootPanelController : 
        MonoBehaviour,
        IUIPanelController
    {
        [Header("References")]
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private RectTransform background;
        
        [Header("Inventory")]
        [SerializeField] private PlayerInventoryPanel playerInventoryPanel;
        [SerializeField] private ChestInventoryPanel chestInventoryPanel;
        
        [Header("Crafting")]
        [SerializeField] private FurnacePanel furnacePanel;
        [SerializeField] private HandCraftingPanel handCraftingPanel;
        [SerializeField] private WorkbenchPanel workbenchPanel;
        [SerializeField] private AlchemyTablePanel alchemyTablePanel;
        
        [Header("Equipment & Accessories")]
        [SerializeField] private GameObject equipmentAccessoryRoot;
        [SerializeField] private AccessoryPanel accessoryPanel;
        [SerializeField] private EquipmentPanel playerEquipmentPanel;
        [SerializeField] private InventoryPlayerUI inventoryPlayerUI;
        
        [Header("UI")]
        [SerializeField] private ItemPropertiesUI itemPropertiesUI;

        private readonly Dictionary<PanelType, IUIPanel> _activePanels = new();
        private IInventoryPanel[] _shiftPriority;

        public bool IsOpen { get; private set; }
        public bool BlocksWorldInteraction => true;
        public bool PausesGame => true;
        public UIPanelType PanelType => UIPanelType.Inventory;

        protected void Start()
        {
            UIPanelManager.Instance.Register(this);
            _shiftPriority = new IInventoryPanel[]
                { chestInventoryPanel, playerEquipmentPanel, accessoryPanel, playerInventoryPanel };
        }

        protected void OnDestroy()
        {
            UIPanelManager.Instance?.Unregister(this);
        }
        
        private void OnEnable()
        {
            GameEventBus.Subscribe<InventoryOpenRequestEvent>(OnInventoryOpenRequest);
            GameEventBus.Subscribe<ItemSlotHoveredEvent>(OnItemSlotHovered);
            GameEventBus.Subscribe<ItemSlotHoverEndedEvent>(OnItemSlotHoverEnded);
        }
        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<InventoryOpenRequestEvent>(OnInventoryOpenRequest);
            GameEventBus.Unsubscribe<ItemSlotHoveredEvent>(OnItemSlotHovered);
            GameEventBus.Unsubscribe<ItemSlotHoverEndedEvent>(OnItemSlotHoverEnded);
        }

        private void OnInventoryOpenRequest(InventoryOpenRequestEvent evt)
        {
            OpenInventories(evt);
        }

        private void OpenInventories(InventoryOpenRequestEvent evt)
        {
            Close();

            _activePanels.Clear();

            if (evt.CraftingStation != CraftingStation.None)
            {
                var panel = GetCraftingPanel(evt.CraftingStation);
                panel.SetItems(Databases.Recipes.GetAll(evt.CraftingStation));
                panel.gameObject.SetActive(true);
                _activePanels.Add(panel.PanelType, panel);
            }
            
            foreach (var config in evt.Configs)
            {
                var owner = config.Owner;
                foreach (var inventoryType in config.InventoryTypes)
                {
                    var panel = GetPanelForOwner(owner, inventoryType);
                    panel.Initialize(owner);
                    var root = panel.Root;
                    
                    _activePanels.TryAdd(panel.PanelType, panel);
                    if (root.activeSelf)
                    {
                        continue;
                    }
                    root.SetActive(true);
                    
                }
            }
            
            Open();
        }

        private IInventoryPanel GetPanelForOwner(IInventoryOwner owner, SlotCollectionType type)
        {
            return owner.InventoryOwnerType switch
            {
                InventoryOwnerType.Player => type switch
                {
                    SlotCollectionType.Inventory => playerInventoryPanel,
                    SlotCollectionType.Accessory => accessoryPanel,
                    SlotCollectionType.Equipment => playerEquipmentPanel,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                },
                InventoryOwnerType.Chest => chestInventoryPanel,
                _ => null
            };
        }
        
        private BaseCraftingPanel GetCraftingPanel(CraftingStation craftingStation)
        {
            return craftingStation switch
            {
                CraftingStation.Hand => handCraftingPanel,
                CraftingStation.Furnace => furnacePanel,
                CraftingStation.Workbench => workbenchPanel,
                CraftingStation.AlchemyTable => alchemyTablePanel,
                _ => throw new ArgumentOutOfRangeException(nameof(craftingStation), craftingStation, null)
            };
        }


        public void Close()
        {
            foreach (var (_, panel) in _activePanels)
                panel.Root.SetActive(false);

            PlayCloseSound();
            itemPropertiesUI.gameObject.SetActive(false);
            
            _activePanels.Clear();
            panelRoot.SetActive(false);
            IsOpen = false;
            UIPanelManager.Instance?.NotifyClosed(this);
            GameEventBus.Publish(new UIPanelClosedEvent(PanelType, BlocksWorldInteraction, PausesGame));
        }

        public void Open()
        {
            panelRoot.SetActive(true);
            IsOpen = true;
            UIPanelManager.Instance?.NotifyOpened(this);
            
            PlayOpenSound();
            GameEventBus.Publish(new UIPanelOpenedEvent(PanelType, BlocksWorldInteraction, PausesGame));
        }
        
        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        private void PlayOpenSound()
        {
            var sounds = new List<AudioClip>();
            foreach (var (_,panel) in _activePanels)
            {
                if (ResourceDatabases.Sounds.TryGet(panel.OpenSound, out var sound))
                    sounds.Add(sound);
            }
            GameEventBus.Publish(new SfxPlayRequest(sounds));
        }

        private void PlayCloseSound()
        {
            var sounds = new List<AudioClip>();
            foreach (var (_,panel) in _activePanels)
            {
                if (ResourceDatabases.Sounds.TryGet(panel.CloseSound, out var sound))
                    sounds.Add(sound);
            }
            GameEventBus.Publish(new SfxPlayRequest(sounds));
        }

        public IEnumerable<IInventoryPanel> GetShiftPriority()
        {
            foreach (var panel in _shiftPriority)
            {
                if (!_activePanels.ContainsKey(panel.PanelType))
                    continue;
                yield return panel;
            }
        }
        
        private void OnItemSlotHoverEnded(ItemSlotHoverEndedEvent e)
        {
            itemPropertiesUI.gameObject.SetActive(false);
        }

        private void OnItemSlotHovered(ItemSlotHoveredEvent e)
        {
            itemPropertiesUI.UpdateText(e.Item, e.Position);
            itemPropertiesUI.gameObject.SetActive(true);
        }

    }
}


