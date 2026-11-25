using Microsoft.Extensions.Options;
using UmbracoExam17.Domain;
using UmbracoExam17.Models;

namespace UmbracoExam17.Domain.Services
{
    public class ReadingTimeService(IOptionsMonitor<GlobalSetting> globalSetting) : IReadingTimeService
    {
        public int CalculateReadingTime(string? content)
        {
            if (string.IsNullOrWhiteSpace(content)) return 0;

            var wordCount = content
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries
                )
                .Length;

            return Math.Max(1, (int)Math.Ceiling(wordCount / (double)globalSetting.CurrentValue.WordsPerMinute));
        }
    }
}
