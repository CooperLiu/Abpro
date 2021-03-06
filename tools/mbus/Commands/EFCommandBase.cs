﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using mbus.CommandLine;
using Microsoft.EntityFrameworkCore.Tools.Commands;

namespace mbus.Commands
{
    internal abstract class EFCommandBase : CommandBase
    {
        public override void Configure(CommandLineApplication command)
        {
            command.HelpOption("-h|--help");

            base.Configure(command);
        }
    }
}
