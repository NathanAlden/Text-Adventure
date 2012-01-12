using System;
using System.Collections.Generic;

using Junior.Common;

using TextAdventure.Engine.Game.Commands;
using TextAdventure.Engine.Game.Events;
using TextAdventure.Engine.Game.Messages;
using TextAdventure.Engine.Game.World;
using TextAdventure.Engine.Objects;

namespace TextAdventure.Engine.Game
{
	public abstract class Context
	{
		private readonly CommandQueue _commandQueue;
		private readonly WorldInstance _worldInstance;

		protected Context(WorldInstance worldInstance, CommandQueue commandQueue)
		{
			worldInstance.ThrowIfNull("worldInstance");
			commandQueue.ThrowIfNull("commandQueue");

			_worldInstance = worldInstance;
			_commandQueue = commandQueue;
		}

		public IEnumerable<Actor> Actors
		{
			get
			{
				return _worldInstance.World.Actors;
			}
		}

		public IEnumerable<Message> Messages
		{
			get
			{
				return _worldInstance.World.Messages;
			}
		}

		public IEnumerable<Timer> Timers
		{
			get
			{
				return _worldInstance.World.Timers;
			}
		}

		public Board CurrentBoard
		{
			get
			{
				return _worldInstance.CurrentBoard;
			}
		}

		public Player Player
		{
			get
			{
				return _worldInstance.Player;
			}
		}

		public PlayerInput PlayerInput
		{
			get
			{
				return _worldInstance.PlayerInput;
			}
		}

		public IWorldTime WorldTime
		{
			get
			{
				return _worldInstance.WorldTime;
			}
		}

		public EventResult RaiseEvent<TEvent>(IEventHandler<TEvent> eventHandler, TEvent @event)
			where TEvent : Event
		{
			@event.ThrowIfNull("event");

			return _worldInstance.RaiseEvent(eventHandler, @event);
		}

		public void EnqueueCommand(Command command, Action<CommandResult> commandExecutedDelegate = null)
		{
			command.ThrowIfNull("command");

			_commandQueue.EnqueueCommand(command, commandExecutedDelegate);
		}

		public void EnqueueCommandToExecuteAtTime(Command command, TimeSpan totalWorldTime, Action<CommandResult> commandExecutedDelegate = null)
		{
			command.ThrowIfNull("command");
			if (totalWorldTime < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("totalWorldTime");
			}

			_commandQueue.EnqueueCommandToExecuteAtTime(command, totalWorldTime, commandExecutedDelegate);
		}

		public void EnqueueCommandWithExecutionDelay(Command command, TimeSpan executionDelay, Action<CommandResult> commandExecutedDelegate = null)
		{
			command.ThrowIfNull("command");
			if (executionDelay < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("executionDelay");
			}

			_commandQueue.EnqueueCommandWithExecutionDelay(command, executionDelay, commandExecutedDelegate);
		}

		public void CancelCommand(Command command)
		{
			command.ThrowIfNull("command");

			_commandQueue.CancelCommand(command);
		}

		public void CancelCommand(Guid commandId)
		{
			_commandQueue.CancelCommand(commandId);
		}

		public void CancelCommands(IUnique context)
		{
			context.ThrowIfNull("context");

			_commandQueue.CancelCommands(context.Id);
		}

		public bool CommandQueued(Command command)
		{
			command.ThrowIfNull("command");

			return _commandQueue.CommandQueued(command);
		}

		public bool CommandQueued(Guid commandId)
		{
			return _commandQueue.CommandQueued(commandId);
		}

		public Board GetBoardById(Guid id)
		{
			return _worldInstance.World.GetBoardById(id);
		}

		public Actor GetActorById(Guid id)
		{
			return _worldInstance.World.GetActorById(id);
		}

		public ActorInstance GetActorInstanceById(Guid id)
		{
			return CurrentBoard.ActorInstanceLayer.GetActorInstanceById(id);
		}

		public IEnumerable<ActorInstance> GetActorInstancesByActorId(Guid actorId)
		{
			return CurrentBoard.ActorInstanceLayer.GetActorInstancesByActorId(actorId);
		}

		public Message GetMessageById(Guid id)
		{
			return _worldInstance.World.GetMessageById(id);
		}

		public Timer GetTimerById(Guid id)
		{
			return _worldInstance.World.GetTimerById(id);
		}

		protected internal void EnqueueMessage(Message message, MessageQueuePosition position)
		{
			message.ThrowIfNull("message");

			_worldInstance.MessageQueue.EnqueueMessage(message, position);
		}
	}
}