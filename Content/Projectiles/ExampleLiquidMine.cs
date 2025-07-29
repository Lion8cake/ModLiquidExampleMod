using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Projectiles
{
	public class ExampleLiquidMine : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(801);
			AIType = 801;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			ExampleLiquidRocket.ExampleLiquidExplosiveKill(Projectile);
		}
	}
}
