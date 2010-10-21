﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenWrap.Commands;
using OpenWrap.Services;

namespace OpenWrap.Commands.Core
{
    [Command(Verb="get", Noun="help")]
    public class GetHelpCommand : ICommand
    {
        [CommandInput(Position=0)]
        public string CommandName { get; set; }

        protected ICommandRepository CommandRepository
        {
            get { return Services.Services.GetService<ICommandRepository>(); }
        }

        public IEnumerable<ICommandOutput> Execute()
        {
            return string.IsNullOrEmpty(CommandName) ? ListAllCommands(CommandRepository) : ListCommand();

        }

        IEnumerable<ICommandOutput> ListAllCommands(IEnumerable<ICommandDescriptor> commandRepository)
        {
            yield return new Result("\r\nList of available commands\r\n--------------------------\r\n");
            foreach (var command in commandRepository.OrderBy(d => d.Noun).ThenBy(d => d.Verb))
            {
                yield return new CommandListResult(command);
            }
        }

        IEnumerable<ICommandOutput> ListCommand()
        {
            var matchingCommands = CommandRepository.Where(x => (x.Verb + "-" + x.Noun).ContainsNoCase(CommandName)).OrderBy(d => d.Noun).ThenBy(d => d.Verb).ToList();
            if (matchingCommands.Count == 0)
                return CommandNotFound();
            if (matchingCommands.Count > 1)
                return MultipleCommands(matchingCommands);

            return CommandDescription(matchingCommands[0]);
        }

        IEnumerable<ICommandOutput> CommandDescription(ICommandDescriptor matchingCommand)
        {
            yield return new CommandDescriptionOutput(matchingCommand);
        }

        IEnumerable<ICommandOutput> MultipleCommands(List<ICommandDescriptor> matchingCommands)
        {
            return ListAllCommands(matchingCommands);
        }

        IEnumerable<ICommandOutput> CommandNotFound()
        {
            yield return new Error("Command '{0}' not found.", CommandName);
        }
    }
}
