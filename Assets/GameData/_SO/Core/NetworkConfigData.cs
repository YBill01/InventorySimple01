using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Core/NetworkConfigData", fileName = "NetworkConfig", order = 7)]

	public class NetworkConfigData : ScriptableObject
	{
		public string uri = "";
		public string tokenKey = "";
	}
}