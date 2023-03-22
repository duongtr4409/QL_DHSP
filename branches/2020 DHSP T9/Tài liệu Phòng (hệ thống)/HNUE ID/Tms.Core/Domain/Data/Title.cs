using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Title")]
    public class Title : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double Level1 { get; set; }
        public double Level2 { get; set; }
        public double Level3 { get; set; }
        public double Level4 { get; set; }
        public double Level5 { get; set; }
        public double Level6 { get; set; }
        public double Level7 { get; set; }
        public double Level8 { get; set; }
        public double Level9 { get; set; }
        public double Level10 { get; set; }
        public double Level11 { get; set; }
        public double Level12 { get; set; }
        public int TitleTypeId { get; set; }
        
        public double GetRatio(int salaryLevel)
        {
            switch (salaryLevel)
            {
                case 1:
                    return Level1;
                case 2:
                    return Level2;
                case 3:
                    return Level3;
                case 4:
                    return Level4;
                case 5:
                    return Level5;
                case 6:
                    return Level6;
                case 7:
                    return Level7;
                case 8:
                    return Level8;
                case 9:
                    return Level9;
                case 10:
                    return Level10;
                case 11:
                    return Level11;
                case 12:
                    return Level12;
            }
            return 0;
        }
    }
}