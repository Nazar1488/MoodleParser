using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MoodleParser
{
    public static class Parser
    {
        public static readonly List<Question> QuestionsList = new List<Question>();

        public static void Parse(string htmlFilePath)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlFilePath, Encoding.UTF8);
            var form = htmlDocument.DocumentNode.SelectNodes("//form[@class='questionflagsaveform']");
            var questions = form.Descendants().Where(d => d.HasClass("que"));
            foreach (var htmlNode in questions)
            {
                var question = new Question();
                question.Answers = new List<Answer>();
                question.Id = htmlNode.Id;
                question.IsCorrect = htmlNode.HasClass("correct");
                var info = htmlNode.Descendants().Where(d => d.HasClass("qtext"));
                question.QuestionText = info.First().InnerText;
                var existing = QuestionsList.Find(q => q.QuestionText == question.QuestionText);
                if (existing != null)
                {
                    if (!existing.IsCorrect)
                    {
                        if (question.IsCorrect)
                        {
                            QuestionsList.RemoveAll(q => q.QuestionText == existing.QuestionText);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                var answers = htmlNode.Descendants().Where(d => d.HasClass("answer"));
                answers = answers.First().Descendants("div");
                foreach (var answer in answers)
                {
                    var answerResult = new Answer();
                    answerResult.IsCorrect = answer.HasClass("correct");
                    var text = answer.Descendants("label");
                    answerResult.Text = text.First().InnerText;
                    question.Answers.Add(answerResult);
                }

                QuestionsList.Add(question);
            }
        }

        public static void Save(string saveFilePath)
        {
            using (var streamWriter = new StreamWriter(saveFilePath, true))
            {
                var result = QuestionsList.OrderBy(q => q.Id).ToList();
                foreach (var question in result)
                {
                    streamWriter.WriteLine($"{question.Id}: {question.QuestionText}");
                    streamWriter.WriteLine("Віповіді:");
                    foreach (var questionAnswer in question.Answers)
                    {
                        streamWriter.WriteLine($"{questionAnswer.Text} {questionAnswer.IsCorrect}");
                    }
                    streamWriter.WriteLine();
                }
            }
        }
    }
}
