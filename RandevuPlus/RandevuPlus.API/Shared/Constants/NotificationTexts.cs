namespace RandevuPlus.API.Shared.Constants
{
    public static class NotificationTexts
    {
        public static string PurchaseCompleteUser(string instructorName, string title, string courseName, DateTime date, int slotStartIndex, int slotEndIndex)
        {
            return $"{date} tarihinde {date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute}-{date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute} arası {title} {instructorName} tarafından verilecek olan {courseName} randevunuz başarıyla oluşturulmuştur. Görüşmek üzere!";
        }

        public static string PurchaseCompleteInstructor(string courseName, DateTime date, int slotStartIndex, int slotEndIndex)
        {
            return $"{date} tarihinde {date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute}-{date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute} arası tarafınızdan verilecek olan {courseName} randevunuz başarıyla oluşturulmuştur. Görüşmek üzere!";
        }

        public static string AppointmentReminderUser(string instructorName, string title, string courseName, DateTime date, int slotStartIndex, int slotEndIndex)
        {   
            return $"{date} tarihinde {date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute}-{date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute} arası {title} {instructorName} tarafından verilecek olan {courseName} randevunuz yaklaşıyor. Hazır olun!";
        }

        public static string AppointmentReminderInstructor(string courseName, DateTime date, int slotStartIndex, int slotEndIndex)
        {
            return $"{date} tarihinde {date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute}-{date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute} arası tarafınızdan verilecek olan {courseName} randevunuz yaklaşıyor. Hazır olun!";
        }
        public static string AppointmentCompleteUser(string instructorName, string title, string courseName, DateTime date, int slotStartIndex, int slotEndIndex)
        {
            return $"{date} tarihinde {date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute}-{date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute} arası {title} {instructorName} tarafından verilen {courseName} randevunuz tamamlandı. Bir sonraki randevunuzu sabırsızlıkla bekliyoruz!";
        }

        public static string AppointmentCompleteInstructor(string courseName, DateTime date, int slotStartIndex, int slotEndIndex)
        {
            return $"{date} tarihinde {date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute}-{date.Date.AddMinutes(slotEndIndex * 30).Hour}:{date.Date.AddMinutes(slotEndIndex * 30).Minute} arası tarafınızdan verilen {courseName} randevunuz tamamlandı. Bir sonraki randevunuzu sabırsızlıkla bekliyoruz!";
        }
    }
}
