// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using mbus.CommandLine;
using Microsoft.EntityFrameworkCore.Tools.Commands;

namespace mbus.Commands
{
    internal class HelpCommandBase : EFCommandBase
    {
        private CommandLineApplication _command;

        public override void Configure(CommandLineApplication command)
        {
            _command = command;

            base.Configure(command);
        }

        protected override int Execute()
        {
            _command.ShowHelp();

            return base.Execute();
        }
    }
}
