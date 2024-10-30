package myst.source.objects;

public class BlockData {
    private final int x, y, z;
    private final String blockType;
    private final String blockState;

    public BlockData(int x, int y, int z, String blockType, String blockState) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.blockType = blockType;
        this.blockState = blockState;
    }
}
