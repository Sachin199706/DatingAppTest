namespace DatingApp.Extension
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int age=today.Year-date.Year;
            if (date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
