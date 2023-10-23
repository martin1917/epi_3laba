namespace epi_3laba
{
    class InputParametrs
    {
        public int TotalDevelopDays { get; set; } = default;
        public double SalaryPerDay { get; set; } = default;
        public double CoefficientAdditionalSalary { get; set; } = default;
        public double OverheadPercent { get; set; } = default;
        public double Power { get; set; } = default;
        public double PriceElectricityPerHour { get; set; } = default;
        public double ComputerManagerSalary { get; set; } = default;
        public int CountManagedComputers { get; set; } = default;
        public double BalanceComputerPrice { get; set; } = default;
        public int CodingDays { get; set; } = default;
        public int DebugDays { get; set; } = default;
        public double ProfitRate { get; set; } = default;
        public double VAT { get; set; } = default;
        public int CountReplic { get; set; } = default;
        public double AdditionalProfitPercent { get; set; } = default;

        public override string ToString()
        {
            return $"длительность разработки: {TotalDevelopDays} дн.\n" +
                $"з/п разработчика за день: {SalaryPerDay} руб/день\n" +
                $"коофициент доп. з/п: {CoefficientAdditionalSalary}\n" +
                $"процент накладных расходов: {OverheadPercent} %\n" +
                $"потребляемая мощность ЭВМ: {Power} кВт\n" +
                $"цена 1 кВт-ч электроэнергии: {PriceElectricityPerHour} кВт*ч\n" +
                $"заработная плата в месяц персонала: {ComputerManagerSalary} руб/мес\n" +
                $"кол-во обслуживаемых компьютеров: {CountManagedComputers} шт.\n" +
                $"балансовая стоимость компьютера: {BalanceComputerPrice} руб\n" +
                $"длительность кодирования: {CodingDays} дн.\n" +
                $"длительность отладки: {DebugDays} дн.\n" +
                $"норматив рентабельности: {ProfitRate} %\n" +
                $"НДС: {VAT} %\n" +
                $"тиражирование: {CountReplic} шт.\n" +
                $"дополнительная прибыль (%): {AdditionalProfitPercent} %\n";
        }
    }
}