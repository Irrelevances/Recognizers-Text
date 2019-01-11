﻿using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimeExtractorConfiguration : BaseOptionsConfiguration, IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.PrepositionRegex, RegexOptions.Singleline);
        public static readonly Regex NowRegex = new Regex(DateTimeDefinitions.NowRegex, RegexOptions.Singleline);
        public static readonly Regex SuffixRegex = new Regex(DateTimeDefinitions.SuffixRegex, RegexOptions.Singleline);

        // TODO: modify it according to the corresponding English regex
        public static readonly Regex TimeOfDayRegex = new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexOptions.Singleline);
        public static readonly Regex SpecificTimeOfDayRegex = new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexOptions.Singleline);
        public static readonly Regex TimeOfTodayAfterRegex = new Regex(DateTimeDefinitions.TimeOfTodayAfterRegex, RegexOptions.Singleline);
        public static readonly Regex TimeOfTodayBeforeRegex = new Regex(DateTimeDefinitions.TimeOfTodayBeforeRegex, RegexOptions.Singleline);
        public static readonly Regex SimpleTimeOfTodayAfterRegex = new Regex(DateTimeDefinitions.SimpleTimeOfTodayAfterRegex, RegexOptions.Singleline);
        public static readonly Regex SimpleTimeOfTodayBeforeRegex = new Regex(DateTimeDefinitions.SimpleTimeOfTodayBeforeRegex, RegexOptions.Singleline);
        public static readonly Regex SpecificEndOfRegex = new Regex(DateTimeDefinitions.SpecificEndOfRegex, RegexOptions.Singleline);
        public static readonly Regex UnspecificEndOfRegex = new Regex(DateTimeDefinitions.UnspecificEndOfRegex, RegexOptions.Singleline);

        // TODO: add this for Portuguese
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexOptions.Singleline);
        public static readonly Regex ConnectorRegex = new Regex(DateTimeDefinitions.ConnectorRegex, RegexOptions.Singleline);
        public static readonly Regex NumberAsTimeRegex = new Regex(DateTimeDefinitions.NumberAsTimeRegex, RegexOptions.Singleline);
        public static readonly Regex DateNumberConnectorRegex = new Regex(DateTimeDefinitions.DateNumberConnectorRegex, RegexOptions.Singleline);

        public PortugueseDateTimeExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            IntegerExtractor = Number.Portuguese.IntegerExtractor.GetInstance();
            DatePointExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration(this));
            TimePointExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this));
            UtilityConfiguration = new PortugueseDatetimeUtilityConfiguration();
        }

        public IExtractor IntegerExtractor { get; }

        public IDateExtractor DatePointExtractor { get; }

        public IDateTimeExtractor TimePointExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        Regex IDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex IDateTimeExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayAfterRegex => TimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex => SimpleTimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayBeforeRegex => TimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex => SimpleTimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex IDateTimeExtractorConfiguration.SpecificEndOfRegex => SpecificEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnspecificEndOfRegex => UnspecificEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        Regex IDateTimeExtractorConfiguration.NumberAsTimeRegex => NumberAsTimeRegex;

        Regex IDateTimeExtractorConfiguration.DateNumberConnectorRegex => DateNumberConnectorRegex;

        public bool IsConnector(string text)
        {
            text = text.Trim();
            return string.IsNullOrEmpty(text)
                    || PrepositionRegex.IsMatch(text)
                    || ConnectorRegex.IsMatch(text);
        }
    }
}