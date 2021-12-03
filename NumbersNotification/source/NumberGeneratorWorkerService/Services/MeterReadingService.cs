namespace NumberGeneratorWorkerService.Services
{
    public class MeterReadingService
    {
        private static int value = 0;

        public static int GetCurrentReading()
        {
            value++;
            if (value > 5)
            {
                value = 1;
            }

            return value;
        }
    }
}
