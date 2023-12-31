﻿using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;

namespace SimpleInventorySort
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class Core : MySessionComponentBase
	{
		// Declarations
		private static readonly string version = "v0.1.0.21";
		private static bool m_debug = false;

		private bool m_initialized = false;
		private readonly List<CommandHandlerBase> m_chatHandlers = new List<CommandHandlerBase>();
		private readonly List<SimulationProcessorBase> m_simHandlers = new List<SimulationProcessorBase>();

		// Properties
		public static bool Debug {
			get { return m_debug; }

			set { m_debug = value; }
		}

		// Initializers
		private void Initialize()
		{
			// Load Settings
			Settings.Instance.Load();

			// Chat Line Event
			AddMessageHandler();

			// Chat Handlers
			m_chatHandlers.Add(new CommandToggle());
			m_chatHandlers.Add(new CommandDebug());
			m_chatHandlers.Add(new CommandFaction());
			m_chatHandlers.Add(new CommandManual());
			m_chatHandlers.Add(new CommandInterval());
			m_chatHandlers.Add(new CommandSettings());

			// Simulation Handlers
			m_simHandlers.Add(new SimulationSort());

			Logging.Instance.WriteLine(string.Format("Script Initialization: {0}", version));
		}

		// Utility
		public void HandleMessageEntered(string messageText, ref bool sendToOthers)
		{
			try {
				if (messageText[0] != '/') {
					return;
				}

				string[] commandParts = messageText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

				if (commandParts[0].ToLower() != "/sort") {
					return;
				}

				sendToOthers = false;
				int paramCount = commandParts.Length - 1;

				if (paramCount < 1 || (paramCount == 1 && commandParts[1].ToLower() == "help")) {
					List<string> commands = new List<string>();

					foreach (CommandHandlerBase chatHandler in m_chatHandlers) {
						string commandBase = chatHandler.GetCommandText().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).First();

						if (!commands.Contains(commandBase)) {
							commands.Add(commandBase);
						}
					}

					string commandList = string.Join(", ", commands);
					string info = string.Format("Simple Inventory Sort {0}.  Available Commands: {1}", version, commandList);

					Communication.Message(info);

					return;
				}

				foreach (CommandHandlerBase chatHandler in m_chatHandlers) {
					int commandCount = 0;

					if (chatHandler.CanHandle(commandParts.Skip(1).ToArray(), ref commandCount)) {
						chatHandler.HandleCommand(commandParts.Skip(commandCount + 1).ToArray());
						return;
					}
				}
			}
			catch (Exception ex) {
				Logging.Instance.WriteLine(string.Format("HandleMessageEntered(): {0}", ex.ToString()));
			}
		}

		public void AddMessageHandler()
		{
			MyAPIGateway.Utilities.MessageEntered += HandleMessageEntered;
		}

		public void RemoveMessageHandler()
		{
			MyAPIGateway.Utilities.MessageEntered -= HandleMessageEntered;
		}

		// Overrides
		public override void UpdateBeforeSimulation()
		{
			try {
				if (MyAPIGateway.Utilities == null) {
					return;
				}

				if (!m_initialized) {
					m_initialized = true;
					Initialize();
				}

				foreach (SimulationProcessorBase simHandler in m_simHandlers) {
					simHandler.Handle();
				}
			}
			catch (Exception ex) {
				Logging.Instance.WriteLine(string.Format("UpdateBeforeSimulation(): {0}", ex.ToString()));
			}
		}

		protected override void UnloadData()
		{
			try {
				RemoveMessageHandler();

				if (Logging.Instance != null) {
					Logging.Instance.Close();
				}
			}
			catch {
			}

			base.UnloadData();
		}
	}
}
