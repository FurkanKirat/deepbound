using Data.Models.Items;
using TMPro;
using UnityEngine;

namespace Visuals.UI.InventorySystem
{
    public class ItemPropertiesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text propertiesTxt;

        public void UpdateText(ItemInstance item, Vector2 position)
        {
            propertiesTxt.text = item.GetItemProperties();
            transform.position = position;
        }
    }
}