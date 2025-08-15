using ModLiquidExampleMod.Content.Liquids;
using ModLiquidLib.Hooks;
using System.IO;
using Terraria.ModLoader;

namespace ModLiquidExampleMod
{
	public class ModLiquidExampleMod : Mod
	{
		//See ExampleLiquid and it's OnPump hook for creating visual effects in server sided hooks only (such as PreLiquidMerge, OnPump, LiquidUpdate)
		public enum MessageType : byte
		{
			PumpPlaySound
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType msgType = (MessageType)reader.ReadByte();

			switch (msgType)
			{
				case MessageType.PumpPlaySound:
					int intX = reader.ReadInt32();
					int intY = reader.ReadInt32();
					int outX = reader.ReadInt32();
					int outY = reader.ReadInt32();
					ExampleLiquid.PlayCustomPumpSound(intX, intY, outX, outY);
					break;
			}
		}
	}
}
