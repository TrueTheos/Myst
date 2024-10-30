package myst.source;

import myst.source.objects.BlockData;
import myst.source.objects.EntityData;
import net.minecraft.client.MinecraftClient;
import net.minecraft.util.math.BlockPos;
import net.minecraft.util.math.Box;
import net.minecraft.block.BlockState;
import net.minecraft.entity.Entity;
import com.google.gson.Gson;
import java.util.List;
import java.util.stream.Collectors;

public class CommandProcessor {
    private final Gson gson;

    public CommandProcessor() {
        this.gson = new Gson();
    }

    private MinecraftClient getClient() {
        return MinecraftClient.getInstance();
    }

    public String processCommand(Command command) {
        try {
            if (getClient().world == null) {
                return gson.toJson(new Response(false, "Not in a world"));
            }

            switch (command.getType()) {
                case "GetBlocksInRange":
                    return handleGetBlocksInRange(command.getArguments());
                case "GetBlockAtPos":
                    return handleGetBlockAtPos(command.getArguments());
                case "GetEntitiesInRange":
                    return handleGetEntitiesInRange(command.getArguments());
                default:
                    return gson.toJson(new Response(false, "Unknown command type"));
            }
        } catch (Exception e) {
            return gson.toJson(new Response(false, "Error processing command: " + e.getMessage()));
        }
    }

    private String handleGetBlocksInRange(Object[] args) {
        int x = ((Number) args[0]).intValue();
        int y = ((Number) args[1]).intValue();
        int z = ((Number) args[2]).intValue();
        int range = ((Number) args[3]).intValue();

        List<BlockData> blocks = new java.util.ArrayList<>();

        BlockPos center = new BlockPos(x, y, z);
        for (int dx = -range; dx <= range; dx++) {
            for (int dy = -range; dy <= range; dy++) {
                for (int dz = -range; dz <= range; dz++) {
                    BlockPos pos = center.add(dx, dy, dz);
                    BlockState state = getClient().world.getBlockState(pos);
                    blocks.add(new BlockData(
                            pos.getX(), pos.getY(), pos.getZ(),
                            state.getBlock().toString(),
                            state.toString()
                    ));
                }
            }
        }

        return gson.toJson(new Response(true, blocks));
    }

    private String handleGetBlockAtPos(Object[] args) {
        int x = ((Number) args[0]).intValue();
        int y = ((Number) args[1]).intValue();
        int z = ((Number) args[2]).intValue();

        BlockPos pos = new BlockPos(x, y, z);
        BlockState state = getClient().world.getBlockState(pos);

        BlockData block = new BlockData(
                pos.getX(), pos.getY(), pos.getZ(),
                state.getBlock().toString(),
                state.toString()
        );

        return gson.toJson(new Response(true, block));
    }

    private String handleGetEntitiesInRange(Object[] args) {
        int x = ((Number) args[0]).intValue();
        int y = ((Number) args[1]).intValue();
        int z = ((Number) args[2]).intValue();
        int range = ((Number) args[3]).intValue();

        Box box = new Box(
                x - range, y - range, z - range,
                x + range, y + range, z + range
        );

        List<EntityData> entities = getClient().world.getEntitiesByClass(
                        Entity.class, box, e -> true)
                .stream()
                .map(e -> new EntityData(
                        e.getX(), e.getY(), e.getZ(),
                        e.getType().toString(),
                        e.getUuid().toString()
                ))
                .collect(Collectors.toList());

        return gson.toJson(new Response(true, entities));
    }
}