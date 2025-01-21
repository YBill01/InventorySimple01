using GameName.Gameplay.Features.Stock;
using UnityEngine;
using UnityEngine.UI;

namespace GameName.UI
{
	public class UIPlayerStockSlotDraggedItem : MonoBehaviour
	{
		public StockItem Item { get; private set; }

		public int SlotIndex { get; private set; }
		public int ItemIndex { get; private set; }

		public void Init(StockItem item, int slotIndex, int itemIndex, Sprite sprite)
		{
			Item = item;

			SlotIndex = slotIndex;
			ItemIndex = itemIndex;

			GetComponent<Image>().sprite = sprite;
		}
	}
}