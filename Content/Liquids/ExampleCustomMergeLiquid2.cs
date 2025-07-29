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
	//The second merge example for a modded liquid
	//here instead we prevent any merging all together, and the way to do this is super simple
	public class ExampleCustomMergeLiquid2 : ModLiquid
	{
		public override void SetStaticDefaults()
		{
			VisualViscosity = 200;
			LiquidFallLength = 4;
			DefaultOpacity = 0.75f;
			SlopeOpacity = 0.75f;
			AddMapEntry(new Color(100, 0, 0));
		}

		//We prevent any merging just by returning false, no sounds are played, no tiles or liquids are created
		//As this liquid is the result of 2 liquids merging, we want this liquid to not merge at all
		//This is so that this liquid isn't immediately turned into a tile when touching another liquid that was used to create this liquid
		public override bool PreLiquidMerge(int liquidX, int liquidY, int tileX, int tileY, int otherLiquid)
		{
			return false;
		}

		public override int ChooseWaterfallStyle(int i, int j)
		{
			return ModContent.GetInstance<BloodClotLiquidFall>().Slot;
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
			int splashDustType = ModContent.DustType<BloodClotSplash>();
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
			int splashDustType = ModContent.DustType<BloodClotSplash>();
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
			int splashDustType = ModContent.DustType<BloodClotSplash>();
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
			int splashDustType = ModContent.DustType<BloodClotSplash>();
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
