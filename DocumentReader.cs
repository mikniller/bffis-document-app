using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;

public class DocumentReader
{
    public List<Hold> Read(string filePath)
    {
        List<Hold> hold = new List<Hold>();
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
            {

                do
                {
                    bool isFirst = true;
                    while (excelReader.Read())
                    {
                        if (excelReader.Name.ToLowerInvariant() == "hold")
                        {
                            if (isFirst) // get column index 
                            {
                                Console.WriteLine(excelReader.Name);
                                isFirst = false;
                            }
                            else
                            {

                                Hold h = new Hold();
                                h.Name = SafeGetString(excelReader, HoldIndex.Name);
                                h.WeekDay = SafeGetString(excelReader, HoldIndex.WeekDay);
                                h.Time = SafeGetString(excelReader, HoldIndex.Time);
                                h.Place = SafeGetString(excelReader, HoldIndex.Place);
                                h.Age = SafeGetString(excelReader, HoldIndex.Age);
                                h.Description = SafeGetString(excelReader, HoldIndex.Description, " Beskrivelse mangler");
                                h.Responsible = SafeGetString(excelReader, HoldIndex.Responsible);
                                h.Assistent = SafeGetString(excelReader, HoldIndex.Assistente);
                                h.HalfSeason = SafeGetString(excelReader, HoldIndex.HalfSeason);
                                h.StartDate = SafeGetString(excelReader, HoldIndex.StartDate);
                                h.Price = SafeGetString(excelReader, HoldIndex.Price, "Ikke angivet");
                                h.Comments = SafeGetString(excelReader, HoldIndex.Comments);

                                h.Number = SafeGetString(excelReader, HoldIndex.Number);
                                h.Min = SafeGetString(excelReader, HoldIndex.Min);
                                h.Max = SafeGetString(excelReader, HoldIndex.Max);
                                h.Waiting = SafeGetString(excelReader, HoldIndex.Waiting);
                                h.Image = SafeGetString(excelReader, HoldIndex.Image, "");
                                h.Status = SafeGetString(excelReader, HoldIndex.Status, "");
                                hold.Add(h);
                            }
                        }
                    }
                } while (excelReader.NextResult());
            }
        }
        return hold;
    }

    private string SafeGetString(IExcelDataReader reader, int ordinal, string def = "")
    {
        try
        {
            if (reader.IsDBNull(ordinal))
                return def;

            return (reader.GetValue(ordinal) ?? def).ToString();
        }
        catch (Exception ex)
        {
            return def;
        }
    }
}




public class Hold
{

    public string Name { get; set; }

    public string Number { get; set; }

    public string WeekDay { get; set; }

    public string Time { get; set; }

    public string Place { get; set; }

    public string Max { get; set; }

    public string Min { get; set; }

    public string Waiting { get; set; }

    public string Age { get; set; }


    public string Description { get; set; }
    public string Responsible { get; set; }
    public string Assistent { get; set; }

    public string HalfSeason { get; set; }

    public string StartDate { get; set; }
    public string Price { get; set; }

    public string Comments { get; set; }

    public string Status { get; set; }

    public string Image { get; set; }

    public bool Udskudt
    {
        get
        {
            return (Status ?? "").ToLowerInvariant().Equals("udskudt");
        }
    }


    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("Nummer {0}, Navn {1}, Ugedag {2}, Tid {3}, Sted {4}, Max {5}, Min {6}, Alder {7}, Ansvarlig {8}, Halvsæson {9}, start {10}\r\nBeskrivelse {11}\r\n\r\n", Number, Name, WeekDay, Time, Place, Max, Min, Age,
        Responsible, HalfSeason, StartDate, Description);

        return builder.ToString();



    }
}

public static class HoldIndex
{

    public static int Name { get; set; } = 1;

    public static int Number { get; set; } = 0;

    public static int WeekDay { get; set; } = 2;

    public static int Time { get; set; } = 3;

    public static int Place { get; set; } = 4;

    public static int Max { get; set; } = 5;

    public static int Min { get; set; } = 6;

    public static int Waiting { get; set; } = 7;

    public static int Age { get; set; } = 8;


    public static int Description { get; set; } = 9;
    public static int Responsible { get; set; } = 10;
    public static int Assistente { get; set; } = 11;

    public static int HalfSeason { get; set; } = 12;

    public static int StartDate { get; set; } = 13;
    public static int Price { get; set; } = 14;

    public static int Status { get; set; } = 15;

    public static int Comments { get; set; } = 16;


    public static int Image { get; set; } = 17;
}