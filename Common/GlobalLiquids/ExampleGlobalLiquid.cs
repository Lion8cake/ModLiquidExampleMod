using Microsoft.Xna.Framework;
using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace ModLiquidExampleMod.Common.GlobalLiquids
{
	//The GlobalLiquid class can do almost everything ModLiquid can do, and more.
	//Allowing the editing of both vanilla and other modded liquids.
	//Here we do a range of different edits to vanilla liquids to show how to use this global class.
	public class ExampleGlobalLiquid : GlobalLiquid
	{
		//AllowFishingInShimmer does exactly as it's name suggests. 
		//As this mod adds loot to the shimmer fishing pool, we need to make sure that we can actually fish in shimmer, and not get the shimmer debuff.
		public override bool AllowFishingInShimmer()
		{
			return true;
		}

		//When fishing in water, extra water dusts are produced when the bobber splashes.
		//This hook/method is for doing for other liquids, providing extra flare when fishing in a liquid
		//Here we make shimmer emit the npc shimmer particle when fished in
		public override bool OnFishingBobberSplash(Projectile proj, int type)
		{
			if (type == LiquidID.Shimmer)
			{
				ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
				{
					PositionInWorld = proj.position
				});
			}
			return true;
		}

		//ChooseWaterfallStyle allows for the editing of waterfall styles for other liquids.
		//Here we make honey use the underground desert waterfall (yellow waterfall)
		public override int? ChooseWaterfallStyle(int i, int j, int type)
		{
			if (type == LiquidID.Honey)
			{
				return ModLiquidLib.ID.WaterfallID.UndergroundDesert;
			}
			return null;
		}

		//EvaporatesInHell allows us to specify which liquids evaportate and don't evaporate in the underworld.
		//This allows us to do things such as prevent water from evaporating and allowing other liquids to evaporate, like shimmer as seen here.
		public override bool? EvaporatesInHell(int i, int j, int type)
		{
			if (type == LiquidID.Shimmer)
			{
				return true;
			}
			return null;
		}

		//The LiquidFallDelay hook/method is used for changing the falldelay of a liquid.
		//Here we change the delay of water to be 10 (maximum delay time)
		public override int? LiquidFallDelay(int type)
		{
			if (type == LiquidID.Water)
			{
				return 10;
			}
			return null;
		}

		//Like modded liquids, we can also change the merge type of a vanilla liquid.
		//Here we set the merge between lava and shimmer to be orichalcum.
		public override int? LiquidMerge(int i, int j, int type, int otherLiquid)
		{
			if (type == LiquidID.Lava)
			{
				if (type == LiquidID.Shimmer)
				{
					return TileID.Orichalcum;
				}
			}
			return null;
		}

		//BlocksTilePlacement allows us to specify which liquids block placing tiles over them.
		//Normally Lava is true, but here we set it to false, allowing the player to be able to place tiles over the liquid.
		public override bool? BlocksTilePlacement(Player player, int i, int j, int type)
		{
			if (type == LiquidID.Lava)
			{
				return false;
			}
			return null;
		}

		public override bool OnPlayerSplash(Player player, int type, bool isEnter)
		{
			if (type == LiquidID.Shimmer)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
				{
					PositionInWorld = player.position + new Vector2(player.width / 2, player.height / 2),
				}, player.whoAmI);
			}
			return true;
		}

		//ChecksForDrowning allows which liquid the player can drown in.
		//Normally water and honey will only let you drown, but here we make shimmer allow the player to drown in, and make water breathable.
		public override bool? ChecksForDrowning(int type)
		{
			if (type == LiquidID.Water)
			{
				return false;
			}
			if (type == LiquidID.Shimmer)
			{
				return true;
			}
			return null;
		}

		public override bool? PlayersEmitBreathBubbles(int type)
		{
			if (type == LiquidID.Shimmer)
			{
				return false;
			}
			return null;
		}
		
		//GlobalLiquid also contains a hook/method for pumping liquids
		//Here we make shimmer un-pump-able, returning false, skipping the liquid movement logic
		public override bool OnPump(int inLiquidType, int inX, int inY, int outX, int outY)
		{
			if (inLiquidType == LiquidID.Shimmer)
			{
				return false;
			}
			return true;
		}

		//Here, we make lava have no liquid movement at all
		//Normally, returning false has no velocity applied to the player at all as liquid movement is responsible for adding velocity to position.
		//To fix this, we copy the default movement when outside of liquids and have it run when in lava.
		public override bool PlayerLiquidMovement(Player player, int type, bool fallThrough, bool ignorePlats)
		{
			if (type == LiquidID.Lava)
			{
				player.DryCollision(fallThrough, ignorePlats);
				if (player.mount.Active && player.mount.IsConsideredASlimeMount && player.velocity.Y != 0f && !player.SlimeDontHyperJump)
				{
					Vector2 vel = player.velocity;
					player.velocity.X = 0f;
					player.DryCollision(fallThrough, ignorePlats);
					player.velocity.X = vel.X;
				}
				if (player.mount.Active && player.mount.Type == 43 && player.velocity.Y != 0f)
				{
					Vector2 vel = player.velocity;
					player.velocity.X = 0f;
					player.DryCollision(fallThrough, ignorePlats);
					player.velocity.X = vel.X;
				}
				return false;
			}
			return true;
		}

		//related to the PlayerLiquidMovement hook/method, we fix lava displaying the incorrect MPH number when moving through it
		//Since lava does not slow us down, we set its multiplier to 1x
		public override void StopWatchMPHMultiplier(int type, ref float multiplier)
		{
			if (type == LiquidID.Lava)
			{
				multiplier = 1f;
			}
		}
	}
}
