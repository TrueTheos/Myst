package myst.source.objects;

public class EntityData {
    private final double x, y, z;
    private final String type;
    private final String uuid;

    public EntityData(double x, double y, double z, String type, String uuid) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
        this.uuid = uuid;
    }
}
