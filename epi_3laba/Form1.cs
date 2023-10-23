using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Linq;

namespace epi_3laba
{
    public partial class Form1 : Form
    {
        private const string POSITIVE_DOUBLE_PATTERN = @"^\d+((,|\.)\d{1,3})?$";
        private const string POSITIVE_INT_PATTERN    = @"^\d+$";

        private InputParametrs inputParametrs;
        private OutputParametrs outputParametrs;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSolve_Click(object sender, System.EventArgs e)
        {
            if (Solve())
            {
                DrawCharts();
            }
        }
        
        private bool Solve()
        {
            // get values from Form
            (string label, string value) totalDevelopDays            = (lblTotalDevelopDays.Text           , tbTotalDevelopDays.Text.Trim().Replace('.', ','));
            (string label, string value) salaryPerDay                = (lblSalaryPerDay.Text               , tbSalaryPerDay.Text.Trim().Replace('.', ','));
            (string label, string value) coefficientAdditionalSalary = (lblCoefficientAdditionalSalary.Text, tbCoefficientAdditionalSalary.Text.Trim().Replace('.', ','));
            (string label, string value) overheadPercent             = (lblOverheadPercent.Text            , tbOverheadPercent.Text.Trim().Replace('.', ','));
            (string label, string value) power                       = (lblPower.Text                      , tbPower.Text.Trim().Replace('.', ','));
            (string label, string value) priceElectricityPerHour     = (lblPriceElectricityPerHour.Text    , tbPriceElectricityPerHour.Text.Trim().Replace('.', ','));
            (string label, string value) computerManagerSalary       = (lblComputerManagerSalary.Text      , tbComputerManagerSalary.Text.Trim().Replace('.', ','));
            (string label, string value) countManagedComputers       = (lblCountManagedComputers.Text      , tbCountManagedComputers.Text.Trim().Replace('.', ','));
            (string label, string value) balanceComputerPrice        = (lblBalanceComputerPrice.Text       , tbBalanceComputerPrice.Text.Trim().Replace('.', ','));
            (string label, string value) codingDays                  = (lblCodingDays.Text                 , tbCodingDays.Text.Trim().Replace('.', ','));
            (string label, string value) debugDays                   = (lblDebugDays.Text                  , tbDebugDays.Text.Trim().Replace('.', ','));
            (string label, string value) profitRate                  = (lblProfitRate.Text                 , tbProfitRate.Text.Trim().Replace('.', ','));
            (string label, string value) VAT                         = (lblVAT.Text                        , tbVAT.Text.Trim().Replace('.', ','));
            (string label, string value) сountReplic                 = (lblCountReplic.Text                , tbCountReplic.Text.Trim().Replace('.', ','));
            (string label, string value) additionalProfitPercent     = (lblAdditionalProfitPercent.Text    , tbAdditionalProfitPercent.Text.Trim().Replace('.', ','));

            var listOfIntedgers = new List<(string label, string value)>()
            {
                totalDevelopDays,
                countManagedComputers,
                codingDays,
                debugDays,
                сountReplic
            };
            
            var listOfDoubles = new List<(string label, string value)>()
            {
                salaryPerDay,
                coefficientAdditionalSalary,
                overheadPercent,
                power,
                priceElectricityPerHour,
                computerManagerSalary,
                balanceComputerPrice,
                profitRate,
                VAT,
                additionalProfitPercent
            };

            // validate fields
            var sb = new StringBuilder();
            CheckWordsByPattern(sb, listOfIntedgers, POSITIVE_INT_PATTERN);
            CheckWordsByPattern(sb, listOfDoubles, POSITIVE_DOUBLE_PATTERN);

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // convert strings to specified types
            inputParametrs = new InputParametrs()
            {
                TotalDevelopDays            = int.Parse(totalDevelopDays.value),
                SalaryPerDay                = double.Parse(salaryPerDay.value),
                CoefficientAdditionalSalary = double.Parse(coefficientAdditionalSalary.value),
                OverheadPercent             = double.Parse(overheadPercent.value),
                Power                       = double.Parse(power.value),
                PriceElectricityPerHour     = double.Parse(priceElectricityPerHour.value),
                ComputerManagerSalary       = double.Parse(computerManagerSalary.value),
                CountManagedComputers       = int.Parse(countManagedComputers.value),
                BalanceComputerPrice        = double.Parse(balanceComputerPrice.value),
                CodingDays                  = int.Parse(codingDays.value),
                DebugDays                   = int.Parse(debugDays.value),
                ProfitRate                  = double.Parse(profitRate.value),
                VAT                         = double.Parse(VAT.value),
                CountReplic                 = int.Parse(сountReplic.value),
                AdditionalProfitPercent     = double.Parse(additionalProfitPercent.value)
            };

            outputParametrs = Calculate(inputParametrs);

            // setup results into text boxes
            tbCostsDevelopProject.Text = outputParametrs.CostsDevelopProject.ToString("0.00");
            tbDeveloperSalary.Text     = outputParametrs.DeveloperSalary.ToString("0.00");
            tbAdditionalSalary.Text    = outputParametrs.AdditionalSalary.ToString("0.00");
            tbSocialContributions.Text = outputParametrs.SocialContributions.ToString("0.00");
            tbOverheads.Text           = outputParametrs.Overheads.ToString("0.00");
            tbOperatingCosts.Text      = outputParametrs.OperatingCosts.ToString("0.00");
            tbProjectPrice.Text        = outputParametrs.ProjectPrice.ToString("0.00");
            tbLowerPriceLimit.Text     = outputParametrs.LowerPriceLimit.ToString("0.00");
            tbContractPrice.Text       = outputParametrs.ContractPrice.ToString("0.00");

            return true;
        }

        private void CheckWordsByPattern(StringBuilder sb, List<(string label, string value)> items, string pattern)
        {
            foreach (var (label, value) in items)
            {
                if (string.IsNullOrEmpty(value))
                {
                    sb.AppendLine($"Ничего не введено: '{label}'");
                }
                else if (!Regex.IsMatch(value, pattern))
                {
                    sb.AppendLine($"Ошибка ввода: '{label}'");
                }
            }
        }

        private void DrawPieChart()
        {
            pieChart.Titles.Add("Затраты на разработку ПП");

            string[] pieLegends =
            {
                lblDeveloperSalary.Text,
                lblAdditionalSalary.Text,
                lblSocialContributions.Text,
                lblOverheads.Text,
                lblOperatingCosts.Text
            };

            foreach (var legend in pieLegends)
            {
                pieChart.Legends.Add(new Legend(legend) { Docking = Docking.Bottom });
            }

            double[] pieValues =
            {
                outputParametrs.DeveloperSalary,
                outputParametrs.AdditionalSalary,
                outputParametrs.SocialContributions,
                outputParametrs.Overheads,
                outputParametrs.OperatingCosts
            };

            var pieSeries = new Series("Pie series") { ChartType = SeriesChartType.Pie };
            pieSeries.Points.DataBindXY(pieLegends, pieValues);
            pieChart.Series.Add(pieSeries);

            foreach (var point in pieSeries.Points)
            {
                point.Label = point.YValues[0].ToString("0.00") + "\n" + (point.YValues[0] / outputParametrs.CostsDevelopProject).ToString("P");
                point.LegendText = point.AxisLabel;
            }
        }
        
        private void DrawColumnChart()
        {
            histogram.Titles.Add("Затраты на разработку ПП");
            histogram.ChartAreas[0].RecalculateAxesScale();

            string[] histLegends =
            {
                lblCostsDevelopProject.Text,
                lblProjectPrice.Text,
                lblLowerPriceLimit.Text,
                lblContractPrice.Text
            };

            double[] histValues =
            {
                outputParametrs.CostsDevelopProject,
                outputParametrs.ProjectPrice,
                outputParametrs.LowerPriceLimit,
                outputParametrs.ContractPrice
            };

            var histSeries = new Series("Histogram series") { ChartType = SeriesChartType.Column };
            histSeries.Points.DataBindXY(histLegends, histValues);
            histogram.Series.Add(histSeries);

            foreach (var point in histSeries.Points)
            {
                point.Label = point.YValues[0].ToString("0.00");
            }
        }

        private void DrawCharts()
        {
            ClearCharts();
            DrawPieChart();
            DrawColumnChart();
        }

        private OutputParametrs Calculate(InputParametrs inputParametrs)
        {
            const int WORKTIME_COMPUTER_PER_YEAR_IN_HOURS = 8 * 288;
            const int NORM_AMORTIZATION_PERCENT = 50;

            var outputParametrs = new OutputParametrs();
            outputParametrs.DeveloperSalary = inputParametrs.TotalDevelopDays * inputParametrs.SalaryPerDay;
            outputParametrs.AdditionalSalary = inputParametrs.CoefficientAdditionalSalary * outputParametrs.DeveloperSalary;
            outputParametrs.SocialContributions = (outputParametrs.DeveloperSalary + outputParametrs.AdditionalSalary) * 0.356;
            outputParametrs.Overheads = outputParametrs.DeveloperSalary * inputParametrs.OverheadPercent / 100;

            double electricityCostsPerYear = inputParametrs.Power * WORKTIME_COMPUTER_PER_YEAR_IN_HOURS * inputParametrs.PriceElectricityPerHour;
            double computerManageCostsPerYear = inputParametrs.ComputerManagerSalary * 12 / inputParametrs.CountManagedComputers;
            double amortization = NORM_AMORTIZATION_PERCENT * inputParametrs.BalanceComputerPrice / 100;
            double repairCosts = 0.05 * inputParametrs.BalanceComputerPrice;
            double operatingCostsPerHour = (electricityCostsPerYear + computerManageCostsPerYear + amortization + repairCosts) / WORKTIME_COMPUTER_PER_YEAR_IN_HOURS;

            outputParametrs.OperatingCosts = (inputParametrs.CodingDays + inputParametrs.DebugDays) * 8 * operatingCostsPerHour;
            outputParametrs.CostsDevelopProject = outputParametrs.DeveloperSalary + outputParametrs.AdditionalSalary + outputParametrs.SocialContributions + outputParametrs.Overheads + outputParametrs.OperatingCosts;
            outputParametrs.ProjectPrice = outputParametrs.CostsDevelopProject * (1 + inputParametrs.ProfitRate / 100);
            outputParametrs.LowerPriceLimit = outputParametrs.ProjectPrice * (1 + inputParametrs.VAT / 100) / inputParametrs.CountReplic;
            outputParametrs.ContractPrice = outputParametrs.LowerPriceLimit * (1 + inputParametrs.AdditionalProfitPercent / 100);

            return outputParametrs;
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Epi file (*.epi)|*.epi",
                ValidateNames = false,
                CheckFileExists = true,
                CheckPathExists = true,
            };
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false))
            {
                writer.Write(inputParametrs.ToString());
                writer.Write($"{new string('-', 30)}\n");
                writer.Write(outputParametrs.ToString());
            }
        }

        private void loadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
            {
                return;
            }

            using (StreamReader reader = new StreamReader(openFileDialog.FileName))
            {
                tbTotalDevelopDays.Text            = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbSalaryPerDay.Text                = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbCoefficientAdditionalSalary.Text = reader.ReadLine().Split(' ').Last();
                tbOverheadPercent.Text             = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbPower.Text                       = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbPriceElectricityPerHour.Text     = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbComputerManagerSalary.Text       = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbCountManagedComputers.Text       = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbBalanceComputerPrice.Text        = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbCodingDays.Text                  = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbDebugDays.Text                   = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbProfitRate.Text                  = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbVAT.Text                         = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbCountReplic.Text                 = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
                tbAdditionalProfitPercent.Text     = reader.ReadLine().Split(' ').Reverse().Skip(1).First();
            }

            if (Solve())
            {
                DrawCharts();
            }
        }

        private void clearResultToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ClearCharts();
            ClearInputTextBoxes();
            ClearOutputTextBoxes();
        }

        private void ClearCharts()
        {
            pieChart.Legends.Clear();
            pieChart.Series.Clear();
            pieChart.Titles.Clear();
            histogram.Legends.Clear();
            histogram.Series.Clear();
            histogram.Titles.Clear();
        }

        private void ClearInputTextBoxes()
        {
            tbTotalDevelopDays.Text = "";
            tbSalaryPerDay.Text = "";
            tbCoefficientAdditionalSalary.Text = "";
            tbOverheadPercent.Text = "";
            tbPower.Text = "";
            tbPriceElectricityPerHour.Text = "";
            tbComputerManagerSalary.Text = "";
            tbCountManagedComputers.Text = "";
            tbBalanceComputerPrice.Text = "";
            tbCodingDays.Text = "";
            tbDebugDays.Text = "";
            tbProfitRate.Text = "";
            tbVAT.Text = "";
            tbCountReplic.Text = "";
            tbAdditionalProfitPercent.Text = "";
        }
        
        private void ClearOutputTextBoxes()
        {
            tbCostsDevelopProject.Text = "";
            tbDeveloperSalary.Text = "";
            tbAdditionalSalary.Text = "";
            tbSocialContributions.Text = "";
            tbOverheads.Text = "";
            tbOperatingCosts.Text = "";
            tbProjectPrice.Text = "";
            tbLowerPriceLimit.Text = "";
            tbContractPrice.Text = "";
        }
    }
}