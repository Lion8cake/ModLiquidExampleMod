using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModLiquidExampleMod.Content.Dusts;
using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils.Structs;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Liquids
{
	//An example of a liquid with custom merging with other liquids
	//This is useful for other liquids wanting to create a new liquid when they collide 
	public class ExampleCustomMergeLiquid : ModLiquid
	{
		public override void SetStaticDefaults()
		{
			VisualViscosity = 100;
			LiquidFallLength = 12;
			DefaultOpacity = 0.5f;
			SlopeOpacity = 0.5f;
			AddMapEntry(new Color(200, 0, 0));
		}

		//PreLiquidMerge is the hook used for custom liquid merging code.
		//This hook/method runs just after the other liquid type is determined (just after the LiquidMerge hook/method) and returning false completely stops the liquid from doing anything when merging.
		//Here we make this liquid merge with all liquids to produce a new liquid, ExampleCustomMergeLiquid2.
		public override bool PreLiquidMerge(int liquidX, int liquidY, int tileX, int tileY, int otherLiquid)
		{
			if (otherLiquid == LiquidLoader.LiquidType<ExampleCustomMergeLiquid2>()) //check if the other liquid is of the type we can merhe
			{
				return false;
			}
			//tile variables, these help us edit the liquid at certain tile positions
			Tile leftTile = Main.tile[tileX - 1, tileY];
			Tile rightTile = Main.tile[tileX + 1, tileY];
			Tile upTile = Main.tile[tileX, tileY - 1];
			Tile tile = Main.tile[tileX, tileY];
			Tile liquidTile = Main.tile[liquidX, liquidY];

			//Checks the type of merging
			//
			//For more context:
			//Liquid to Liquid merging is split up into 2 types, 
			// * Top/Side merging
			// * Down merging
			//
			//Here we get which type the merging is based on the liquidY relitive to the tileY.
			//This is because liquidY and tileY are different in the down merging, but the same in the up/side merging
			if (liquidY == tileY)
			{
				//This is up/side merging for the liquid

				//Here we remove the liquid when merging
				//Majority of this code determines whether a liquid merge is spawned or not by checking the surrounding liquid amounts
				int liquidCount = 0;
				if (leftTile.LiquidType != Type)
				{
					liquidCount += leftTile.LiquidAmount;
					leftTile.LiquidAmount = 0;
				}
				if (rightTile.LiquidType != Type)
				{
					liquidCount += rightTile.LiquidAmount;
					rightTile.LiquidAmount = 0;
				}
				if (upTile.LiquidType != Type)
				{
					liquidCount += upTile.LiquidAmount;
					upTile.LiquidAmount = 0;
				}

				//check is the nearby amount is more than 24, and the other liquid is not this liquid
				if (liquidCount < 24 || otherLiquid == Type)
				{
					return false;
				}

				//change the liquid type and amount of liquid at the liquidtile
				liquidTile.LiquidType = LiquidLoader.LiquidType<ExampleCustomMergeLiquid2>();
				liquidTile.LiquidAmount = byte.MaxValue;
				//play the liquid merge sound
				if (!WorldGen.gen)
				{
					ModLiquidLib.Hooks.LiquidHooks.PlayLiquidChangeSound(tileX, tileY, Type, otherLiquid);
				}
				if (Main.netMode == NetmodeID.Server)
				{
					ModPacket packet = ModContent.GetInstance<ModLiquidLib.ModLiquidLib>().GetPacket();
					packet.Write((byte)ModLiquidLib.ModLiquidLib.MessageType.SyncCollisionSounds);
					packet.Write(tileX);
					packet.Write(tileY);
					packet.Write(Type);
					packet.Write(otherLiquid);
					packet.Send();
				}
				//frame the tile/update the tile/s nearby
				WorldGen.SquareTileFrame(liquidX, liquidY);
				//sync changes in multiplayer
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendTileSquare(-1, tileX - 1, tileY - 1, 3);
				}
			}
			else
			{
				//This is down merging for the liquid

				//remove the liquid amount 
				tile.LiquidAmount = 0;
				tile.LiquidType = 0;

				//spawn the new liquid at the liquid tile
				liquidTile.LiquidType = LiquidLoader.LiquidType<ExampleCustomMergeLiquid2>();
				liquidTile.LiquidAmount = byte.MaxValue;
				//play liquid merge sound
				if (!WorldGen.gen)
				{
					ModLiquidLib.Hooks.LiquidHooks.PlayLiquidChangeSound(tileX, tileY, Type, otherLiquid);
				}
				if (Main.netMode == NetmodeID.Server)
				{
					ModPacket packet = ModContent.GetInstance<ModLiquidLib.ModLiquidLib>().GetPacket();
					packet.Write((byte)ModLiquidLib.ModLiquidLib.MessageType.SyncCollisionSounds);
					packet.Write(tileX);
					packet.Write(tileY);
					packet.Write(Type);
					packet.Write(otherLiquid);
					packet.Send();
				}
				//frame the tile/s around the tile
				WorldGen.SquareTileFrame(liquidX, liquidY);
				//sync tile changs around
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendTileSquare(-1, tileX - 1, tileY, 3);
				}
			}
			return false;
		}

		public override int ChooseWaterfallStyle(int i, int j)
		{
			return ModContent.GetInstance<BloodLiquidFall>().Slot;
		}

		public override void RetroDrawEffects(int i, int j, SpriteBatch spriteBatch, ref RetroLiquidDrawInfo drawData, float liquidAmountModified, int liquidGFXQuality)
		{
			drawData.liquidAlphaMultiplier *= 1.5f;
			if (drawData.liquidAlphaMultiplier > 1f)
			{
				drawData.liquidAlphaMultiplier = 1f;
			}
		}

		#region Splash Effects
		public override void OnPlayerSplash(Player player, bool isEnter)
		{
			int splashDustType = ModContent.DustType<BloodSplash>();
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(player.position.X - 6f, player.position.Y + (player.height / 2) - 8f), player.width + 12, 24, splashDustType);
				Main.dust[dust].velocity.Y -= 1f;
				Main.dust[dust].velocity.X *= 2.5f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
		}

		public override void OnNPCSplash(NPC npc, bool isEnter)
		{
			int splashDustType = ModContent.DustType<BloodSplash>();
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(new Vector2(npc.position.X - 6f, npc.position.Y + (npc.height / 2) - 8f), npc.width + 12, 24, splashDustType);
				Main.dust[dust].velocity.Y -= 1f;
				Main.dust[dust].velocity.X *= 2.5f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].noGravity = true;
			}
			if (npc.aiStyle != NPCAIStyleID.Slime &&
					npc.type != NPCID.BlueSlime && npc.type != NPCID.MotherSlime && npc.type != NPCID.IceSlime && npc.type != NPCID.LavaSlime &&
					npc.type != NPCID.Mouse &&
					npc.aiStyle != NPCAIStyleID.GiantTortoise &&
					!npc.noGravity)
			{
				SoundEngine.PlaySound(SoundID.SplashWeak, npc.position);
			}
		}

		public override void OnProjectileSplash(Projectile proj, bool isEnter)
		{
			int splashDustType = ModContent.DustType<BloodSplash>();
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(new Vector2(proj.position.X - 6f, proj.position.Y + (proj.height / 2) - 8f), proj.width + 12, 24, splashDustType);
				Main.dust[dust].velocity.Y -= 1f;
				Main.dust[dust].velocity.X *= 2.5f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.SplashWeak, proj.position);
		}

		public override void OnItemSplash(Item item, bool isEnter)
		{
			int splashDustType = ModContent.DustType<BloodSplash>();
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(new Vector2(item.position.X - 6f, item.position.Y + (item.height / 2) - 8f), item.width + 12, 24, splashDustType);
				Main.dust[dust].velocity.Y -= 1f;
				Main.dust[dust].velocity.X *= 2.5f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.SplashWeak, item.position);
		}
		#endregion
	}
}
