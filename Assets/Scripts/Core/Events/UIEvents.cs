using System;
using System.Collections.Generic;
using Data.Models.Crafting;
using Systems.InventorySystem;
using Visuals.UI;

namespace Core.Events
{
    public readonly struct InventoryOpenRequestEvent : IEvent
    {
        public readonly IEnumerable<InventoryOpenConfig> Configs;
        public readonly CraftingStation CraftingStation;

        public InventoryOpenRequestEvent(IEnumerable<InventoryOpenConfig> configs, CraftingStation includeFurnacePanel = CraftingStation.None)
        {
            Configs = configs;
            CraftingStation = includeFurnacePanel;
        }

        public InventoryOpenRequestEvent(InventoryOpenConfig config, CraftingStation includeFurnacePanel = CraftingStation.None)
            : this(new[] { config }, includeFurnacePanel)
        { }
    }

    public readonly struct ChatToggleRequested : IEvent { }
    
    public readonly struct MinimapToggleRequested : IEvent { }
    
    public readonly struct SettingsToggleRequested : IEvent { }

    public readonly struct UIPanelOpenedEvent : IEvent
    {
        public readonly UIPanelType PanelType;
        public readonly bool BlocksWorldInteraction;
        public readonly bool PausesGame;

        public UIPanelOpenedEvent(UIPanelType panelType, bool blocksWorldInteraction, bool pausesGame)
        {
            PanelType = panelType;
            BlocksWorldInteraction = blocksWorldInteraction;
            PausesGame = pausesGame;
        }
    }

    public readonly struct UIPanelClosedEvent : IEvent
    {
        public readonly UIPanelType PanelType;
        public readonly bool BlocksWorldInteraction;
        public readonly bool PausesGame;

        public UIPanelClosedEvent(UIPanelType panelType, bool blocksWorldInteraction, bool pausesGame)
        {
            PanelType = panelType;
            BlocksWorldInteraction = blocksWorldInteraction;
            PausesGame = pausesGame;
        }
    }

}