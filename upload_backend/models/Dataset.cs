using System;

namespace WebApplication.models
{
    public enum AnswersTypeEnum
    {
        WithoutAnswers=0,
        AnswersFile=1,
    }

    public class Dataset
    {
        public Guid Id { get; init; }
        public string FileName { get; init; }
        public DateTime CreatedAt { get; init; }
        public int ImagesCount { get; init; }
        public bool ContainCyrillic { get; init; }
        public bool ContainsNumbers { get; init; }
        public bool ContainSpecialCharacters { get; init; }
        public bool CaseSensitivity { get; init; }
        
        public AnswersTypeEnum AnswersType { get; init; }

    }
}