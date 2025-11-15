using System;
using System.Collections.Generic;
using Interfaces;
using Systems.BuffSystem;
using Systems.EffectSystem;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Inventory;
using Systems.StatSystem;

namespace Systems.InventorySystem
{
    public class InventoryManager : IDisposable, IInitializable, ISaveable<InventoryManagerSaveData>
    {
        private readonly Dictionary<SlotCollectionType, IItemSlotCollection> _inventories;

        private InventoryManager(Dictionary<SlotCollectionType, IItemSlotCollection> inventories)
        {
            _inventories = inventories;
        }

        public static InventoryManager Create(IInventoryOwner owner)
        {
            return new InventoryManager(new Dictionary<SlotCollectionType, IItemSlotCollection>());
        }

        public static InventoryManager Load(InventoryManagerSaveData saveData, IInventoryOwner owner)
        {
            var inventories = new Dictionary<SlotCollectionType, IItemSlotCollection>();
            foreach (var pair in saveData.SlotCollections)
            {
                var inv = InventoryLoadFactory.LoadInventory(owner, pair.Value);
                inventories.Add(pair.Key, inv);
            }
            return new InventoryManager(inventories);
        }
        
        public InventoryManagerSaveData ToSaveData()
        {
            var inventories = new Dictionary<SlotCollectionType, SlotCollectionSaveData>();
            foreach (var pair in _inventories)
            {
                inventories.Add(pair.Key, pair.Value.ToSaveData());
            }

            return new InventoryManagerSaveData
            {
                SlotCollections = inventories
            };
        }
        public void Initialize()
        {
            foreach (var itemSlot in _inventories.Values)
                if (itemSlot is IInitializable initializable)
                    initializable.Initialize();
        }
        public void Dispose()
        {
            foreach (var itemSlot in _inventories.Values)
                if (itemSlot is IDisposable disposable)
                    disposable.Dispose();
        }
        
        
        public T GetInventory<T>(SlotCollectionType type) where T : class, IItemSlotCollection
        {
            if (_inventories.TryGetValue(type, out var inv))
                return inv as T;
            return null;
        }
        
        public IItemSlotCollection GetInventory(SlotCollectionType type) 
            => _inventories.GetValueOrDefault(type, null);
        
        public void RegisterInventory(SlotCollectionType type, IItemSlotCollection inventory)
        {
            if (!_inventories.TryAdd(type, inventory))
                throw new InvalidOperationException($"{type} already registered!");
        }
        
        public void UnregisterInventory(SlotCollectionType type)
        {
            _inventories.Remove(type);
        }

        public bool TryGetInventory(SlotCollectionType type, out IItemSlotCollection inventory)
            => _inventories.TryGetValue(type, out inventory);

        public IEnumerable<IItemSlotCollection> GetAllInventories()
            => _inventories.Values;

        public void RegisterInventoryStats(StatCollection statCollection)
        {
            var providers = new List<IStatProvider>();
            foreach (var inventory in _inventories.Values)
            {
                if (inventory is IStatProvider statProvider)
                    providers.Add(statProvider);
                
            }
            statCollection.AddMultipleProviders(providers);
        }

        public void RegisterInventoryEffects(EffectHandler effectHandler)
        {
            var providers = new List<IEffectProvider>();
            foreach (var inventory in _inventories.Values)
            {
                if (inventory is IEffectProvider effectProvider)
                    providers.Add(effectProvider);
                
            }
            effectHandler.AddProviders(providers);
        }
        
    }
}