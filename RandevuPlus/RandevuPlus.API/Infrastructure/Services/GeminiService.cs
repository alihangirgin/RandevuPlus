using Microsoft.Extensions.Options;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using RandevuPlus.API.Shared.Models;
using System.Text;
using OpenAI.Chat;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RandevuPlus.API.Infrastructure.Services
{
    public class GeminiService : IAiService
    {
        private readonly AiSettings _aiSettings;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public GeminiService(IOptions<AiSettings> aiSettings, IServiceScopeFactory serviceScopeFactory)
        {
            _aiSettings = aiSettings.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<string> AskQuestion(string question)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var query = _unitOfWork.Instructors.GetQueryable()
                .Include(i => i.User)
                .Include(i => i.Reviews)
                .Include(i => i.Skills)
                .Include(i => i.Availabilities)
                .Include(i => i.Experiences)
                .Include(i => i.Courses)
            .AsQueryable();


            var slotPattern = new string('1', 2);

            var instructors = await query.Where(i =>
                i.Availabilities.Any(a =>
                    (a.Date.Date == DateTime.UtcNow.AddHours(3).Date || a.Date.Date == DateTime.UtcNow.AddHours(3).AddDays(1).Date) &&
                    a.SlotString.Substring(0, 49).Contains(slotPattern))
            ).ToListAsync();


            StringBuilder sb = new StringBuilder();
            sb.Append($"Merhaba, şimdi sana {_aiSettings.Instructor}leri özellikleri ile birlikte tanımlayacağım ve sonrasında bir soru soracağım.\n" +
                $"Bilgisi verilen {_aiSettings.Instructor}ler arasından varsa en uygun 3 {_aiSettings.Instructor}i seçmeni istiyorum.\n" +
                $"Soru {_aiSettings.Topic} ile alakasız bir soru ise Lütfen içereğe uygun soru sorunuz yanıtını vermeni istiyorum, yani burada kontrol edeceğim parametre Soru parametresi\n" +
                $"Sorulan Soru {_aiSettings.Topic} ile alakalı bir soruysa yeterli bilgi olmasa bile 3 tane veya kaç tane varsa {_aiSettings.Instructor} seç\n" +
                $"Yanıtı verirken **Eğitmen Adı (EğitmenId:eğitmentId):** formatını kullanabilirsin" +
                $" Soru: {question}");
            foreach (var instructor in instructors)
            {
                sb.Append($"Eğitmen Id:{instructor.Id}  Adı:{instructor.User.FullName}\n");
                sb.Append($"Eğitmen Ünvanı {instructor.Title}\n");
                sb.Append($"Eğitmen Biyografisi {instructor.Bio}\n");
                foreach (var instructorExperience in instructor.Experiences)
                {
                    sb.Append($"Eğitmen Tecrübesi {instructorExperience.Description}\n");
                }
                foreach (var instructorSkill in instructor.Skills)
                {
                    sb.Append($"Eğitmen Yeteneği {instructorSkill.SkillName}\n");
                }
                foreach (var instructorCourse in instructor.Courses)
                {
                    sb.Append($"Eğitmen Kursları {instructorCourse.Name}\n");
                }
                foreach (var instructorReview in instructor.Reviews.OrderByDescending(x => x.CreatedAt).Take(3))
                {
                    sb.Append($"Eğitmen Yorumu {instructorReview.Comment}, 5 üzerinden Puanı: {instructorReview.Rating}\n");
                }
                sb.Append($"\n\n");
            }

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_aiSettings.ApiKey}";

            var jsonObject = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = sb.ToString(),

                            }
                        }
                    }
                }
            };
            string jsonData = JsonSerializer.Serialize(jsonObject);
            string responseBody = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("API Yanıtı: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Hata: " + response.StatusCode);
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                    return string.Empty;
                }
            }

            string responseText = string.Empty;
            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                var candidates = doc.RootElement.GetProperty("candidates");
                responseText = candidates[0].GetProperty("content")
                                          .GetProperty("parts")[0]
                                          .GetProperty("text")
                                          .GetString() ?? string.Empty;

                Console.WriteLine(responseText);
            }

            return responseText;
        }
    }
}
