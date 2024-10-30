package myst.source;

import net.fabricmc.api.ClientModInitializer;

import net.fabricmc.fabric.api.client.event.lifecycle.v1.ClientLifecycleEvents;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MystSource implements ClientModInitializer {
	public static final String MOD_ID = "mystsource";

	public static final Logger LOGGER = LoggerFactory.getLogger(MOD_ID);

	private ExecutorService executorService;
	private SocketServer manager;

	@Override
	public void onInitializeClient() {
		LOGGER.debug("MystSource initialized");
		SocketServer server = new SocketServer();
		server.startListening();
	}
}