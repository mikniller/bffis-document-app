using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

public class DocumentBuilder
{
    public void Build(List<Hold> hold, string outputPath,string year)
    {

        var doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));
        doc.Open();
        AddHeader(year,doc);
        int cnt = 0;
        foreach (var team in hold)
        {
           cnt++;
           AddTeam(team, doc, cnt % 2 == 1);

            if (cnt % 2 == 0)
                doc.NewPage();

        }
        doc.Close();
    }

    private void AddHeader(string year,Document doc) {
       iTextSharp.text.Font header = FontFactory.GetFont("Arial", 22, iTextSharp.text.Font.BOLD, BaseColor.Black);
       iTextSharp.text.Font subHeader = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD, BaseColor.Black);
       Paragraph p = new Paragraph();
       p.Alignment = Element.ALIGN_CENTER;
       Chunk hdrChunk = new Chunk("Børnefritidsforeningen i Sydhavnen\n\n", header);
       Chunk subChunk = new Chunk("Program "+year+ "\n\n\n\n", subHeader);
       p.Add(hdrChunk);
       p.Add(subChunk);

        
            DottedLineSeparator dottedline = new DottedLineSeparator();
            dottedline.Offset = 0;
            dottedline.Gap = 2f;
            p.Add(dottedline);
            p.Add(new Chunk("\n\n\n"));
        


       doc.Add(p);
    }

    private void AddTeam(Hold team, Document doc, bool addSeparator)
    {
        iTextSharp.text.Font header = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.Black);
        iTextSharp.text.Font boldText = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.Black);
        iTextSharp.text.Font descFont = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.DarkGray);
        iTextSharp.text.Font text = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
        iTextSharp.text.Font textRed = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.Red);
        iTextSharp.text.Image img = null;

        if (string.IsNullOrEmpty(team.Image) == false)
        {
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "images/" + team.Image);
            if (File.Exists(imgPath))
            {
                img = iTextSharp.text.Image.GetInstance(imgPath);
                img.ScaleToFit(120f, 120f);
                img.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
                img.IndentationLeft = 9f;
                img.SpacingAfter = 9f;
                img.BorderWidthTop = 16f;
                img.BorderColorTop = iTextSharp.text.BaseColor.White;
            }
            else {
                Console.WriteLine("Kan ikke finde billedet " + team.Image);
            }
        }

        Paragraph p = new Paragraph();
        p.Alignment = Element.ALIGN_JUSTIFIED;

        Chunk nameChunk = new Chunk(team.Name, header);
        Chunk ageChunk = new Chunk(" for " + team.Age + "\n\n", text);

        p.Add(nameChunk);
        p.Add(ageChunk);

        iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
        list.SetListSymbol("\u2022");
        list.IndentationLeft = 20f;
        list.Add("Ansvarlig Instruktør : " + team.Responsible);
        list.Add("Yderligere Instruktør(er) : " + team.Assistent);
        list.Add("Tidspunkt : " + team.WeekDay + " " + team.Time + " i " + team.Place);
        list.Add("Deltagere : " + team.Min + " til " + team.Max);
        list.Add("Pris : " + team.Price);
        list.Add("Opstart : " + team.StartDate);
        p.Add(list);


        Paragraph p1 = new Paragraph();

        Chunk descChunk = new Chunk("\n\n" + team.Description + "\n\n", descFont);
        p1.Add(descChunk);

        if (string.IsNullOrWhiteSpace(team.HalfSeason) == false && team.HalfSeason.ToLowerInvariant() != "nej")
        {
            Chunk halfSeason = new Chunk("Bemærk", boldText);
            Chunk halfSeason2 = new Chunk(" Dette hold udbydes kun for en halv sæson og løber indtil "+team.HalfSeason, text);
            p1.Add(halfSeason);
            p1.Add(halfSeason2);
        }
        
        if (string.IsNullOrWhiteSpace(team.Status))
        {
            Chunk extraInfo = new Chunk("\nHoldet er endnu ikke helt på plads ( "+ team.Comments+")", textRed);
            
            p1.Add(extraInfo);
        }
        if (addSeparator)
        {
            p1.Add(new Chunk("\n\n"));
            DottedLineSeparator dottedline = new DottedLineSeparator();
            dottedline.Offset = 0;
            dottedline.Gap = 2f;
            p1.Add(dottedline);
            p1.Add(new Chunk("\n\n"));
        }

        if (img != null)
        {
            doc.Add(img);
        }

        doc.Add(p);
        doc.Add(p1);

    }



}