using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UebungWPFBars_Chart_FileReader
{
    public class Patient
    {
        public String? Firstname { get; set; }
        public String? Lastname { get; set; }
        public int ID { get; set; }
        public List<double>? Pointers { get; set; }




        public override string ToString()
        {
            return $"Id + {ID} + Firstname + {Firstname}, Lastname + {Lastname}";
        }

        public static bool TryParse(String line, out Patient temp)
        {
            temp = null;
            String[] linecontent = line.Split(';');
            if (linecontent.Length > 3)
            {
                temp = new Patient();
                temp.Firstname = linecontent[0];
                temp.Lastname = linecontent[1];
                if (int.TryParse(linecontent[2], out int value))
                {
                    temp.ID = value;
                }
                else { return false; }
                temp.Pointers = new List<double>();
                for (int i = 3; i < linecontent.Length; i++)
                {
                    if (double.TryParse(linecontent[i], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double value2))
                    {
                        temp.Pointers.Add(value2);
                    }
                   
                }

                return true;
            }
            else return false;


        }

        public String ToCSVString()
        {
            String pointcollection = "";
            foreach (double temp in Pointers)
            {
                pointcollection += temp;
                pointcollection += ";";
            }
            return Firstname + ";" + Lastname + ";" + ID + ";" + pointcollection;
        }
    }
}
