using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Projectiles
{
	public class ExampleLiquidBomb : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(904);
			Projectile.aiStyle = 16;
			AIType = 903;
			Projectile.timeLeft = 180;
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
