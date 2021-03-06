﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Tools;

namespace mbus
{
    internal class AnsiConsole
    {
        public static readonly AnsiTextWriter _out = new AnsiTextWriter(Console.Out);

        public static void WriteLine(string text)
            => _out.WriteLine(text);
    }
}
