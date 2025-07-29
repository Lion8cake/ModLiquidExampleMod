using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Projectiles
{
	public class ExampleLiquidSnowmanRocket : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(810);
			AIType = 810;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			ExampleLiquidRocket.ExampleLiquidExplosiveKill(Projectile);
		}
	}
}
