using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Events;
using Data.Models.Crafting;
using Generated.Localization;
using Interfaces;
using Localization;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Visuals.UI.CraftingSystem
{
    public abstract class BaseCraftingPanel :
        UIGrid<CraftingSlotUI, CraftingRecipe>, 
        IInitializable<ClientContext>,
        IUIPanel,
        ILocalizable
    {
        [Header("Crafting Panel")]
        [SerializeField] private TextButton craftButton;
        [SerializeField] private CraftingSlotUI mainSlotUI;
        [SerializeField] private TMP_Text requirementsTxt, titleText;
        [SerializeField] private RequiredItemsUI requiredItemsUI;
        private IPlayer Player { get; set; }
        private Dictionary<CraftingRecipe, bool> _canCraftByRecipe;
        
        public GameObject Root => gameObject;
        public string OpenSound => null;
        public string CloseSound => null;
        public abstract PanelType PanelType { get; }
        public abstract CraftingStation Station { get; }
        
        public void Initialize(ClientContext data)
        {
            Player = data.Player;
            mainSlotUI.UpdateUser(Player);
        }
        
        private void Awake()
        {
            StartCorner = GridLayoutGroup.Corner.UpperLeft;
            Constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            ConstraintCount = 8;
            craftButton.Button.onClick.AddListener(OnCraftClicked);
            Localize();
        }
        
        private void OnEnable()
        {
            GameEventBus.Subscribe<CraftingSlotClickedEvent>(OnCraftingSlotClicked);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CraftingSlotClickedEvent>(OnCraftingSlotClicked);
        }

        protected override void SetupAction(CraftingSlotUI item, CraftingRecipe data)
        {
            item.UpdateUser(Player);
            item.UpdateSlot(data);
            item.CanCraft = _canCraftByRecipe.TryGetValue(data, out var canCraft) && canCraft;
        }

        public override void SetItems(IEnumerable<CraftingRecipe> items)
        {
            var craftingRecipes = items as CraftingRecipe[] ?? items.ToArray();
            _canCraftByRecipe = Player.InventoryManager.GetPlayerInventory().CanCraft(craftingRecipes);
            base.SetItems(craftingRecipes);
            
            if (craftingRecipes.Length > 0)
            {
                SetMainSlot(craftingRecipes[0]);
            }
            else
            {
                mainSlotUI.UpdateSlot(null);
                mainSlotUI.CanCraft = false;
            }
        }

        private void OnCraftClicked()
        {
            GameEventBus.Publish(new CraftingRequest(mainSlotUI.Recipe, Player));
            _canCraftByRecipe = Player.InventoryManager.GetPlayerInventory().CanCraft(_canCraftByRecipe.Keys);
            foreach (var child in SpawnedItems)
            {
                child.CanCraft = _canCraftByRecipe.TryGetValue(child.Recipe, out var canCraft) && canCraft;
            }
        }
        
        private void OnCraftingSlotClicked(CraftingSlotClickedEvent e)
        {
            SetMainSlot(e.Recipe);
        }
        
        private void SetMainSlot(CraftingRecipe recipe)
        {
            mainSlotUI.UpdateSlot(recipe);
            mainSlotUI.CanCraft = _canCraftByRecipe.TryGetValue(recipe, out var canCraft) && canCraft;
            requiredItemsUI.SetItems(recipe.Requires);
        }


        public void Localize()
        {
            craftButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiCraft);
            requirementsTxt.text = LocalizationDatabase.Get(LocalizationKeys.UiRequirements);
            titleText.text = LocalizationDatabase.Get($"crafting_station.{CaseUtils.ToSnakeCase(Station.ToString())}");
        }
    }
}