using System;
using System.Linq;

namespace aggrathon.ld36
{
	public class Upgrades
	{
		public enum Upgrade
		{
			Random,
			None,
			Better_Engine,
			Front_Plow,
			Side_Armor,
			Hyper_Booster,
			Cannon
		}

		public static void EnableUpgrade(Upgrade u, CarController car)
		{
			switch (u)
			{
				case Upgrade.Random:
					EnableUpgrade((Upgrade)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Upgrade)).Length) ,car);
					break;
				case Upgrade.Better_Engine:
					car.upgradeObjects.Where(o=> o.name.Equals("AdditionalPipes")).First().SetActive(true);
					car.maximumTorque *= 1.5f;
					car.reverseTorque *= 1.5f;
					break;
				case Upgrade.Front_Plow:
					car.upgradeObjects.Where(o => o.name.Equals("PloughDetailed")).First().SetActive(true);
					car.upgradeObjects.Where(o => o.name.Equals("PloughCollider")).First().SetActive(true);
					car.rigidbody.mass += 200f;
					car.frontArmor += 5f;
					break;
				case Upgrade.Side_Armor:
					car.upgradeObjects.Where(o => o.name.Equals("Side_Armor")).First().SetActive(true);
					car.rigidbody.mass += 300f;
					car.sideArmor += 5f;
					break;
				case Upgrade.Hyper_Booster:
					car.upgradeObjects.Where(o => o.name.Equals("BoostPipes")).First().SetActive(true);
					car.boostMultiplier *= 1.2f;
					car.boostDrain *= 0.75f;
					break;
				case Upgrade.Cannon:
					car.upgradeObjects.Where(o => o.name.Equals("Cannon")).First().SetActive(true);
					break;
			}
		}
	}
}
