using System.Collections.Generic;

namespace MoodleParser
{
    public class Question
    {
        public string Id { get; set; }
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; set; }
        public bool IsCorrect { get; set; }
    }
}
