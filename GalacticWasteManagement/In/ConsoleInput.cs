﻿using System;

namespace GalacticWasteManagement.In
{
    public class ConsoleInput : IInput
    {
        private readonly bool optForDefaults;

        public ConsoleInput(bool optForDefaults = true)
        {
            this.optForDefaults = optForDefaults;
        }

        public string Name => "Console";

        public void TrySet<T>(Param<T> param)
        {
            while (true)
            {
                string candidate = null;
                if ((param.optional && !optForDefaults) || !param.optional)
                {
                    Console.WriteLine($"Please provide value for {(param.optional ? "(optional) " : "")}parameter '{param.inputParam.Name}'.{(param.optional ? $" (Leave empty for default '{param.defaultValue}')" : "")}");
                    candidate = Console.ReadLine();
                }
                try
                {
                    if (string.IsNullOrEmpty(candidate) && param.optional)
                    {
                        return;
                    }
                    param.SetValue(param.inputParam.Parse(candidate));
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"'{candidate}' is not a valid value for '{param.inputParam.Name}'. {e.Message}");
                }
            }
        }
    }
}
