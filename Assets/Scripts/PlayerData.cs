using System;
using UnityEngine;

namespace aggrathon.ld36
{
	[Serializable]
	public class PlayerData
	{
		static PlayerData[] players;
		public static PlayerData[] Players
		{
			get {
				if (players == null)
				{
					players = new PlayerData[] {
						new PlayerData("Player 1", Controller.player),
						new PlayerData("Bot 1", Controller.ai),
						new PlayerData("Bot 2", Controller.ai),
						new PlayerData("Bot 3", Controller.ai),
						new PlayerData("Bot 4", Controller.ai),
						new PlayerData("Bot 5", Controller.ai),
						new PlayerData("Bot 6", Controller.ai),
						new PlayerData("Bot 7", Controller.ai)
					};
				}
				return players;
			}
			set { players = value; }
		}

		public enum Controller
		{
			ai,
			player,
			player1,
			player2,
			dummy
		}

		public string name;
		public Controller controller;
		public Upgrades.Upgrade[] upgrades;
		public Color color;

		public PlayerData(string name, Controller cont, params Upgrades.Upgrade[] upgrades)
		{
			this.name = name;
			this.controller = cont;
			this.upgrades = upgrades;
			color = UnityEngine.Random.ColorHSV();
			color.a = 1f;
		}

		public PlayerData(string name, Controller cont, Color color, params Upgrades.Upgrade[] upgrades)
		{
			this.name = name;
			this.controller = cont;
			this.upgrades = upgrades;
			this.color = color;
		}

		public PlayerData(string name, Controller cont, Color color, int[] upgrades)
		{
			this.name = name;
			this.controller = cont;
			this.color = color;
			this.upgrades = new Upgrades.Upgrade[upgrades.Length];
			for (int i = 0; i < upgrades.Length; i++)
			{
				if(Enum.IsDefined(typeof(Upgrades.Upgrade), upgrades[i]))
					this.upgrades[i] = (Upgrades.Upgrade)upgrades[i];
				else
					this.upgrades[i] = Upgrades.Upgrade.None;
			}
		}

		public PlayerData() { }
	}
}
