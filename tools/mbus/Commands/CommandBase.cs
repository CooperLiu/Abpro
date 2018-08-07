// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using mbus;
using mbus.CommandLine;

namespace Microsoft.EntityFrameworkCore.Tools.Commands
{
    internal abstract class CommandBase
    {
        public virtual void Configure(CommandLineApplication command)
        {
            var verbose = command.Option("-v|--verbose", "verbose description");
            var noColor = command.Option("--no-color", "NoColorDescription");
            var prefixOutput = command.Option("--prefix-output", "Resources.PrefixDescription");

            command.HandleResponseFiles = true;

            command.OnExecute(
                () =>
                    {
                        Reporter.IsVerbose = verbose.HasValue();
                        Reporter.NoColor = noColor.HasValue();
                        Reporter.PrefixOutput = prefixOutput.HasValue();

                        Validate();

                        return Execute();
                    });
        }

        protected virtual void Validate()
        {
        }

        protected virtual int Execute()
            => 0;
    }
}
