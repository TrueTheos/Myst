package myst.source;

public class Command {
    private String Type; // Change to match JSON exactly
    private Object[] Arguments; // Change to match JSON exactly

    public String getType() {
        return Type;
    }

    public Object[] getArguments() {
        return Arguments;
    }
}
