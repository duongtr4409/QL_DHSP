using System;

namespace Ums.Core.Entities.Statistic
{
    public class FacultyAverage
    {
        private double _totalOvertime;
        private double _totalTeachWork;
        private double _totalMoney;
        public string Name { get; set; }
        public int Stave { get; set; }
        public double TotalOvertime
        {
            get => _totalOvertime;
            set => _totalOvertime = Math.Round(value, 2);
        }
        public double TotalTeachWork
        {
            get => _totalTeachWork;
            set => _totalTeachWork = Math.Round(value, 2);
        }
        public double TotalMoney
        {
            get => _totalMoney;
            set => _totalMoney = Math.Round(value, 2);
        }
        public double OverTimeAverage => Math.Round(TotalOvertime / Stave, 2);
        public double WorkAverage => Math.Round(TotalTeachWork / Stave, 2);
        public double MoneyAverage => Math.Round(TotalMoney / Stave, 2);
    }
}