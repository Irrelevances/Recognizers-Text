﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class PortugueseNumberParserConfiguration : BaseNumberParserConfiguration
    {
        public PortugueseNumberParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public PortugueseNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = NumbersDefinitions.LangMarker;
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = NumbersDefinitions.DecimalSeparatorChar;
            this.FractionMarkerToken = NumbersDefinitions.FractionMarkerToken;
            this.NonDecimalSeparatorChar = NumbersDefinitions.NonDecimalSeparatorChar;
            this.HalfADozenText = NumbersDefinitions.HalfADozenText;
            this.WordSeparatorToken = NumbersDefinitions.WordSeparatorToken;

            this.WrittenDecimalSeparatorTexts = NumbersDefinitions.WrittenDecimalSeparatorTexts;
            this.WrittenGroupSeparatorTexts = NumbersDefinitions.WrittenGroupSeparatorTexts;
            this.WrittenIntegerSeparatorTexts = NumbersDefinitions.WrittenIntegerSeparatorTexts;
            this.WrittenFractionSeparatorTexts = NumbersDefinitions.WrittenFractionSeparatorTexts;

            this.CardinalNumberMap = NumbersDefinitions.CardinalNumberMap.ToImmutableDictionary();
            this.OrdinalNumberMap = NumberMapGenerator.InitOrdinalNumberMap(NumbersDefinitions.OrdinalNumberMap, NumbersDefinitions.PrefixCardinalMap, NumbersDefinitions.SuffixOrdinalMap);
            this.RelativeReferenceMap = NumbersDefinitions.RelativeReferenceMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();

            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.Singleline);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.Singleline);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.Singleline);
        }

        public override ImmutableDictionary<string, long> CardinalNumberMap { get; set; }

        public override NumberOptions Options { get; set; }

        public override CultureInfo CultureInfo { get; set; }

        public override char DecimalSeparatorChar { get; set; }

        public override Regex DigitalNumberRegex { get; set; }

        public override Regex FractionPrepositionRegex { get; }

        public override Regex NegativeNumberSignRegex { get; set; }

        public override string FractionMarkerToken { get; set; }

        public override Regex HalfADozenRegex { get; set; }

        public override string HalfADozenText { get; set; }

        public override string LangMarker { get; set; }

        public override char NonDecimalSeparatorChar { get; set; }

        public string NonDecimalSeparatorText { get; private set; }

        public override ImmutableDictionary<string, long> OrdinalNumberMap { get; set; }

        public override ImmutableDictionary<string, string> RelativeReferenceMap { get; set; }

        public override ImmutableDictionary<string, long> RoundNumberMap { get; set; }

        public override string WordSeparatorToken { get; set; }

        public override IEnumerable<string> WrittenDecimalSeparatorTexts { get; set; }

        public override IEnumerable<string> WrittenGroupSeparatorTexts { get; set; }

        public override IEnumerable<string> WrittenIntegerSeparatorTexts { get; set; }

        public override IEnumerable<string> WrittenFractionSeparatorTexts { get; set; }

        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            var result = new List<string>();

            foreach (var token in tokens)
            {
                var tempWord = token.Trim(NumbersDefinitions.PluralSuffix);
                if (this.OrdinalNumberMap.ContainsKey(tempWord))
                {
                    result.Add(tempWord);
                    continue;
                }

                // ends with 'avo' or 'ava'
                if (NumbersDefinitions.WrittenFractionSuffix.Any(suffix => tempWord.EndsWith(suffix)))
                {
                    var origTempWord = tempWord;
                    var newLength = origTempWord.Length;
                    tempWord = origTempWord.Remove(newLength - 3);

                    if (string.IsNullOrWhiteSpace(tempWord))
                    {
                        // Ignore avos in fractions.
                        continue;
                    }
                    else if (this.CardinalNumberMap.ContainsKey(tempWord))
                    {
                        result.Add(tempWord);
                        continue;
                    }
                    else
                    {
                        tempWord = origTempWord.Remove(newLength - 2);
                        if (this.CardinalNumberMap.ContainsKey(tempWord))
                        {
                            result.Add(tempWord);
                            continue;
                        }
                    }
                }

                result.Add(token);
            }

            return result;
        }

        public override long ResolveCompositeNumber(string numberStr)
        {
            if (this.OrdinalNumberMap.ContainsKey(numberStr))
            {
                return this.OrdinalNumberMap[numberStr];
            }

            if (this.CardinalNumberMap.ContainsKey(numberStr))
            {
                return this.CardinalNumberMap[numberStr];
            }

            long value = 0;
            long finalValue = 0;
            var strBuilder = new StringBuilder();
            int lastGoodChar = 0;
            for (int i = 0; i < numberStr.Length; i++)
            {
                strBuilder.Append(numberStr[i]);
                if (this.CardinalNumberMap.ContainsKey(strBuilder.ToString()) && this.CardinalNumberMap[strBuilder.ToString()] > value)
                {
                    lastGoodChar = i;
                    value = this.CardinalNumberMap[strBuilder.ToString()];
                }

                if ((i + 1) == numberStr.Length)
                {
                    finalValue += value;
                    strBuilder.Clear();
                    i = lastGoodChar++;
                    value = 0;
                }
            }

            return finalValue;
        }
    }
}