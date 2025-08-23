using Microsoft.Xna.Framework;
using ModLiquidExampleMod.Content.Liquids;
using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils;
using ModLiquidLib.Utils.LiquidContent;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Common.Players
{
	public class ExampleLiquidPlayer : ModPlayer
	{ 
		//Here we add fishing for example liquid and shimmer
		//GlobalLiquid allows us to toggle whether fishing rods can fish in shimmer or not, and we implement loot here.
		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			Player player = Player;
			//unfortunately, due to fisher being impossible to edit directly with getting what pool type is being fished in, we have to see what liquid the bobber's coords are located at.
			if (Main.tile[attempt.X, attempt.Y].LiquidType == LiquidLoader.LiquidType<ExampleLiquid>())
			{
				if (attempt.crate && Main.rand.NextBool(6))
				{
					if (Main.hardMode)
					{
						itemDrop = ItemID.LavaCrateHard;
					}
					else
					{
						itemDrop = ItemID.LavaCrate;
					}
				}
				else if (attempt.veryrare)
				{
					itemDrop = ItemID.GoldenCarp;
				}
				else if (attempt.rare)
				{
					itemDrop = ItemID.Goldfish;
				}
				else
				{
					itemDrop = 0; //This makes it so that most of the time fish are not caught when fishing (making catching fish rarer similarly to lava)
				}
			}
			else if (Main.tile[attempt.X, attempt.Y].LiquidType == LiquidID.Shimmer) //See ExampleGlobalLiquid.AllowShimmerFishing for enabling fishing in shimmer
			{
				if (attempt.veryrare)
				{
					itemDrop = ItemID.CrystalSerpent;
				}
				else if (attempt.rare)
				{
					itemDrop = ItemID.PrincessFish;
				}
				else if (attempt.uncommon)
				{
					itemDrop = ItemID.ChaosFish;
				}
				else
				{
					itemDrop = ItemID.Prismite;
				}
			}
		}

		public override void PostUpdateMiscEffects()
		{
			//ModLiquidLib already contains bools on whether the player is in a certain liquid or not.
			//Here we use it to give the player the plentiful satisfied buff for 30 seconds
			if (Player.GetModdedWetArray()[LiquidLoader.LiquidType<ExampleLiquid>() - LiquidID.Count])
			{
				Player.AddBuff(BuffID.WellFed2, 60 * 30, false, false);
				Player.buffImmune[BuffID.OnFire] = true;
				Player.buffImmune[BuffID.OnFire3] = true;
			}
		}

		//Please see ExampleWader to see how to make a custom liquid walking accessory
		public override void PreUpdateMovement()
		{
			//Here we change how walking on liquids works for vanilla liquids
			//Lava now is walk-able no matter if Lava waders or Water walking boots are equipped
			//Shimmer now requires Lava waders to be walked on
			if (Player.waterWalk2)
			{
				Player.GetModPlayer<ModLiquidPlayer>().canLiquidBeWalkedOn[LiquidID.Lava] = true;
				Player.GetModPlayer<ModLiquidPlayer>().canLiquidBeWalkedOn[LiquidID.Shimmer] = Player.waterWalk;
			}
		}
	}
}
