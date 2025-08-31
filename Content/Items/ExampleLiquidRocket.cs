using ModLiquidExampleMod.Content.Liquids;
using ModLiquidExampleMod.Content.Projectiles;
using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModLiquidExampleMod.Content.Items
{
	public class ExampleLiquidRocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			AmmoID.Sets.IsSpecialist[Type] = true; 

			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.RocketLauncher].Add(Type, ModContent.ProjectileType<Projectiles.ExampleLiquidRocket>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.GrenadeLauncher].Add(Type, ModContent.ProjectileType<ExampleLiquidGrenade>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.ProximityMineLauncher].Add(Type, ModContent.ProjectileType<ExampleLiquidMine>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.SnowmanCannon].Add(Type, ModContent.ProjectileType<ExampleLiquidSnowmanRocket>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.Celeb2].Add(Type, ProjectileID.Celeb2Rocket);

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.width = 20;
			Item.height = 14;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.ammo = AmmoID.Rocket;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 10);
			Item.DamageType = DamageClass.Ranged;
		}

		public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(ItemID.DryRocket);
			recipe.AddIngredient(Type);
			recipe.SortAfterFirstRecipesOf(ItemID.DryRocket);
			recipe.Register();

			Recipe recipe2 = Recipe.Create(Type);
			recipe2.AddIngredient(ItemID.DryRocket);
			recipe2.AddLiquid(null, "ExampleLiquid");
			recipe2.Register();
		}
	}
}
