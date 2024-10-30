using MystClient;
using MystClient.MinecraftObjects;
using static MystClient.MinecraftAPI;

class Client
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Myst initialized");

        using (var api = new MinecraftAPI())
        {
            try
            {
                // Example: Get entities around spawn point
                var entities = await api.GetEntitiesInRange(0, 64, 0, 10);
                foreach (var entity in entities)
                {
                    Console.WriteLine($"Found {entity.Type} at ({entity.X}, {entity.Y}, {entity.Z})");
                }

                // Example: Get block at specific position
                var block = await api.GetBlockAtPos(0, 65, 0);
                if (block != null) Console.WriteLine($"Block at (0,65,0) is {block.BlockType}");


                // Example: Get blocks in 5 block radius
                var blocks = await api.GetBlocksInRange(0, 65, 0, 5);
                if(blocks != null && blocks.Length > 0) Console.WriteLine($"Found {blocks.Length} blocks in range");
            }
            catch (MinecraftAPIException ex)
            {
                Console.WriteLine($"Minecraft API error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Keep console window open
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}