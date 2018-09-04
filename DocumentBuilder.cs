using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Linq;

public class DocumentBuilder
{
    public void Build(List<Hold> hold, string outputPath,string year)
    {

        var doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));
        doc.Open();
        AddHeader(year,doc,false);
        int cnt = 0;


        iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
        list.SetListSymbol("\u2022");
        list.IndentationLeft = 20f;

        foreach (var team in hold.Where(h => h.Udskudt==false))
        {
                cnt++;

                AddTeam(team, doc, cnt % 2 == 1);

                if (cnt % 2 == 0)
                    doc.NewPage();
        }

            addLargeFooter(doc);

        doc.Close();
    }


    public void BuildSimple(List<Hold> hold, string outputPath, string year)
    {

        var doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));
        doc.Open();
        AddHeader(year, doc, true);
        int cnt = 0;

        iTextSharp.text.Font subHeader2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
        Paragraph p = new Paragraph();
        p.Alignment = Element.ALIGN_LEFT;
        Chunk subChunk2 = new Chunk("Børnefritidsforeningen i sydhavnen udbyder fritidsaktiviteter for alle børn i og omkring sydhavnen. Sæsonen løber typisk fra medio september til ultimo april og tilmelding til foreningens aktiviteter åbnes start september\n\nI år udbydes følgende aktiviteter:\n\n", subHeader2);
        p.Add(subChunk2);


        iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
        list.SetListSymbol("\u2022");
        list.IndentationLeft = 20f;


        foreach (var team in hold.Where(h => h.Udskudt == false))
        {
            list.Add(team.Name + " for " + team.Age + " - " + team.WeekDay + " " + team.Time + " i " + team.Place);
        }
        p.Add(list);
        doc.Add(p);

        iTextSharp.text.Image img = null;

        string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "images/" + "banner.png");
        if (File.Exists(imgPath))
        {
            img = iTextSharp.text.Image.GetInstance(imgPath);
            img.ScaleToFit(500, 180f);
            img.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_CENTER;
            img.IndentationLeft = 1f;
            img.SpacingAfter = 1f;
            img.BorderWidthTop = 1f;
            img.BorderColorTop = iTextSharp.text.BaseColor.White;
            doc.Add(img);
        }
        else
        {
            Console.WriteLine("Kan ikke finde billedet banner.png");
        }



        addFooter(doc);

        doc.Close();
    }



    private void AddHeader(string year,Document doc, bool simple=false) {
       iTextSharp.text.Font header = FontFactory.GetFont("Arial", 22, iTextSharp.text.Font.BOLD, BaseColor.Black);
        iTextSharp.text.Font subHeader = FontFactory.GetFont("Arial",14, iTextSharp.text.Font.BOLD, BaseColor.Black);
        iTextSharp.text.Font subHeader2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
        Paragraph p = new Paragraph();
       p.Alignment = Element.ALIGN_CENTER;
       Chunk hdrChunk = new Chunk("Børnefritidsforeningen i Sydhavnen\n\n", header);
       Chunk subChunk = new Chunk("Program "+year+ "\n\n", subHeader);
       Chunk subChunk2 = new Chunk("Alle aktiviteter foregår på skolen i Sydhavnen, Støberigade 1, 2450 København SV\n\n", subHeader2);


        iTextSharp.text.Image img = null;
        string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "images/" + "play1.png");
        if (File.Exists(imgPath))
        {
            img = iTextSharp.text.Image.GetInstance(imgPath);
            img.ScaleToFit(50, 50f);
            img.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_LEFT;
            img.IndentationLeft = 1f;
            img.SpacingAfter = 1f;
            img.BorderWidthTop = 1f;
            img.BorderColorTop = iTextSharp.text.BaseColor.White;
            doc.Add(img);
        }
        else
        {
            Console.WriteLine("Kan ikke finde billedet banner.png");
        }



        p.Add(hdrChunk);
        p.Add(subChunk);

        p.Add(subChunk2);


        iTextSharp.text.Image img1 = null;
        string img1Path = Path.Combine(Directory.GetCurrentDirectory(), "images/" + "play2.png");
        if (File.Exists(img1Path))
        {
            img1 = iTextSharp.text.Image.GetInstance(img1Path);
            img1.ScaleToFit(50, 50f);
            img1.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_RIGHT;
            img1.IndentationLeft = 1f;
            img1.SpacingAfter = 1f;
            img1.BorderWidthTop = 1f;
            img1.BorderColorTop = iTextSharp.text.BaseColor.White;
            doc.Add(img1);
        }
        else
        {
            Console.WriteLine("Kan ikke finde billedet banner.png");
        }



        DottedLineSeparator dottedline = new DottedLineSeparator();
            dottedline.Offset = 0;
            dottedline.Gap = 2f;
            p.Add(dottedline);
            p.Add(new Chunk("\n\n\n"));


        doc.Add(p);
    }




    private void addTeamSimple1(Hold team, Document doc, iTextSharp.text.List list)
    {
        iTextSharp.text.Font text = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);

        if (string.IsNullOrWhiteSpace(team.Status))
            text = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.Red);

        Paragraph p = new Paragraph();
        p.Alignment = Element.ALIGN_LEFT;

        Chunk chunk = new Chunk(team.Name + " for " + team.Age + " " + team.WeekDay + " " + team.Time + " i " + team.Place + ", Pris " + team.Price + "\n\n", text);
        list.Add(chunk);

    }



    private void addFooter(Document doc)
    {
        iTextSharp.text.Font text = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
        iTextSharp.text.Font textLink = FontFactory.GetFont("Verdana", 14, iTextSharp.text.Font.UNDERLINE, BaseColor.Blue);

        Paragraph p = new Paragraph();
        p.Alignment = Element.ALIGN_LEFT;


        DottedLineSeparator dottedline = new DottedLineSeparator();
        dottedline.Offset = 0;
        dottedline.Gap = 2f;
        //p.Add(dottedline);


        Chunk chunk1 = new Chunk("\n\nDet detaljerede program og andre oplysninger på foreningens hjemmeside ", text);
        Chunk chunk2 = new Chunk("http://www.bffis.dk", textLink);

        p.Add(chunk1);
        p.Add(chunk2);
        doc.Add(p);

    }



    private void addLargeFooter(Document doc)
    {
        iTextSharp.text.Font text = FontFactory.GetFont("Verdana", 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);

        Paragraph p = new Paragraph();
        p.Alignment = Element.ALIGN_LEFT;


        DottedLineSeparator dottedline = new DottedLineSeparator();
        dottedline.Offset = 0;
        dottedline.Gap = 2f;
        //p.Add(dottedline);


        Chunk chunk1 = new Chunk("\n\nFind det detaljerede program og andre oplysninger på http://www.bffis.dk", text);

        Chunk chunk2 = new Chunk("\n\nTilmelding foretages på http://www.bffis.dk/Tilmelding.aspx", text);


        p.Add(chunk1);
        p.Add(chunk2);
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