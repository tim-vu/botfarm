using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using FluentValidation;

namespace FORFarm.Application.Proxies
{
    public static class ProxyPropertyValidators
    {
        private const int MinimumPort = 1;
        private const int MaximumPort = 65535;
        private static readonly Regex IpAddress = new Regex("^(?=.*[^\\.]$)((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.?){4}$");

        public static IRuleBuilderOptions<T, string> ValidIpAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(ip => IpAddress.IsMatch(ip));
        }

        public static IRuleBuilderOptions<T, int> ValidPort<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.Must(p => p >= MinimumPort && p <= MaximumPort);
        }
    }
}
