using System;
using UnityEngine;

namespace GameName.Gameplay.Objects
{
	[Serializable]
	public struct InteractionCondition
	{
		public bool touchable;
		public bool clickable;

		[Space]
		public bool stockable;
		public bool stackable;
		public bool droppable;

		[Space]
		public bool removable;

		//public bool collectable;
	}
}