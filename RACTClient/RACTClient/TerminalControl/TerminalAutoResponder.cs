using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RACTClient
{
    /// <summary>
    /// 규칙 기반 자동 응답기 구현체입니다.
    /// </summary>
    public class TerminalAutoResponder : ITerminalAutoResponder
    {
        private readonly List<AutoResponseRule> _rules = new List<AutoResponseRule>();
        
        public bool IsEnabled { get; set; } = true;

        public void AddRule(AutoResponseRule rule)
        {
            if (rule == null) return;
            _rules.Add(rule);
        }

        public void ClearRules()
        {
            _rules.Clear();
        }

        public bool HasRule(string pattern, string response)
        {
            return _rules.Exists(r => r.Pattern == pattern && r.Response == response);
        }

        public string Process(string receivedText)
        {
            if (!IsEnabled || string.IsNullOrEmpty(receivedText))
                return null;

            foreach (var rule in _rules)
            {
                if (string.IsNullOrEmpty(rule.Pattern)) continue;

                if (rule.IsRegex)
                {
                    if (Regex.IsMatch(receivedText, rule.Pattern, RegexOptions.IgnoreCase))
                    {
                        return rule.Response;
                    }
                }
                else
                {
                    if (receivedText.IndexOf(rule.Pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return rule.Response;
                    }
                }
            }

            return null;
        }
    }
}
