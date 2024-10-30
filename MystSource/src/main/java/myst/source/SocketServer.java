package myst.source;

import com.google.gson.Gson;
import com.google.gson.JsonParseException;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class SocketServer {
    private static final int PORT = 5000; // Port number for the server
    private final Gson gson;
    private final CommandProcessor processor;
    private volatile boolean running;

    public SocketServer() {
        this.gson = new Gson();
        this.processor = new CommandProcessor();
    }

    public void startListening() {
        running = true;
        Thread serverThread = new Thread(this::listen);
        serverThread.setName("Minecraft-IPC-SocketServer");
        serverThread.setDaemon(true);
        serverThread.start();
    }

    public void stopListening() {
        running = false;
    }

    private void listen() {
        try (ServerSocket serverSocket = new ServerSocket(PORT)) {
            System.out.println("Server started on port " + PORT);

            while (running) {
                try {
                    // Accept incoming connections
                    Socket clientSocket = serverSocket.accept();
                    handleClient(clientSocket);
                } catch (IOException e) {
                    System.err.println("Error handling client connection: " + e.getMessage());
                }
            }
        } catch (IOException e) {
            System.err.println("Error starting server: " + e.getMessage());
        }
    }

    private void handleClient(Socket clientSocket) {
        try (BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
             BufferedWriter out = new BufferedWriter(new OutputStreamWriter(clientSocket.getOutputStream()))) {

            // Read command from client
            String commandJson = in.readLine();
            Command command = gson.fromJson(commandJson, Command.class);

            // Process the command and get a response
            String responseJson = processCommand(command);

            // Send the response to the client
            out.write(responseJson);
            out.newLine();
            out.flush();
        } catch (IOException | JsonParseException e) {
            System.err.println("Error handling client: " + e.getMessage());
        } finally {
            try {
                clientSocket.close();
            } catch (IOException e) {
                System.err.println("Error closing client socket: " + e.getMessage());
            }
        }
    }

    private String processCommand(Command command) {
        if (command == null) {
            return gson.toJson(new Response<String>(false, "Invalid command received")); // Specify String as type
        }

        // Use the existing CommandProcessor to process the command
        return processor.processCommand(command);
    }
}
