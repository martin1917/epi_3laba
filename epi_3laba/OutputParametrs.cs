namespace epi_3laba
{
    class OutputParametrs
    {
        public double CostsDevelopProject { get; set; } = default;
        public double DeveloperSalary { get; set; } = default;
        public double AdditionalSalary { get; set; } = default;
        public double SocialContributions { get; set; } = default;
        public double Overheads { get; set; } = default;
        public double OperatingCosts { get; set; } = default;
        public double ProjectPrice { get; set; } = default;
        public double LowerPriceLimit { get; set; } = default;
        public double ContractPrice { get; set; } = default;

        public override string ToString()
        {
            return $"затраты на разработку: {CostsDevelopProject} руб\n" + 
                $"З/П разработчиков: {DeveloperSalary} руб\n" + 
                $"дополнительная З/П: {AdditionalSalary} руб\n" + 
                $"отчисления на социальные нужды: {SocialContributions} руб\n" + 
                $"накладные расходы: {Overheads} руб\n" + 
                $"эксплуатационные расходы: {OperatingCosts} руб\n" + 
                $"цена разработанного ПО: {ProjectPrice} руб\n" + 
                $"нижний предел цены: {LowerPriceLimit} руб\n" + 
                $"договорная цена: {ContractPrice} руб\n";
        }
    }
}