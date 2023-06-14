using iTextSharp.text;
using iTextSharp.text.pdf;
using Questionnaire_Frontend.DownloadProjectReport;
using Rectangle = iTextSharp.text.Rectangle;

namespace CreatePDFReport;

public class PDFReport
{
    private iTextSharp.text.Document document;
    private PdfWriter writer;
    private int maxPageCount = 0;
    private SecurityCheckContext _db;
    private List<QuestionDto> _allQuestionsAndAnswers;
    private IFormFile _chart;
    private PdfReportDto _pdfReportDto;

    public PDFReport(SecurityCheckContext db, List<QuestionDto> allQuestionsAndAnswers, IFormFile chart, PdfReportDto pdfReportDto)
    {
        _db = db;
        _allQuestionsAndAnswers = allQuestionsAndAnswers;
        _chart = chart;
        _pdfReportDto = pdfReportDto;
    }

    public void CreatePDF()
    {
        FirstPage();

        AddNewEmptyPage();

        SecondPage();
        //AddTable();

        AddNewEmptyPage();

        ManagementSummaryPage();

        AddNewEmptyPage();

        DiagramPage();

        AddNewEmptyPage();

        DetailsPages();

        CloseDocument();
    }

    private void ManagementSummaryPage()
    {
        AddImgRightCorner();

        AddTextAbouveTable();

        AddMMSTable();

        AddStandardFooter();
    }

    private int calculateReachedPointsForCategory(string category)
    {
        //int notAnswered = _allQuestionsAndAnswers.Where(x => x.Category == category).Select(x => x.Answer.NotAnswered.Selected).Count();
        //int answerZero = _allQuestionsAndAnswers.Where(x => x.Category == category).Select(x => x.Answer.AnswerZero.Selected).Count();
        int answerOne = (_allQuestionsAndAnswers.Where(x => x.Category == category).Where(x => x.Answer.AnswerOne.Selected).Count() * 1);
        int answerTwo = (_allQuestionsAndAnswers.Where(x => x.Category == category).Where(x => x.Answer.AnswerTwo.Selected).Count() * 2);
        int answerThree = (_allQuestionsAndAnswers.Where(x => x.Category == category).Where(x => x.Answer.AnswerThree.Selected).Count() * 3);

        return (answerOne + answerTwo + answerThree);
    }

    private void SecondPage()
    {
        AddImgRightCorner();

        AddSecondPageMiddleText();

        AddStandardFooter();
    }

    private void AddSecondPageMiddleText()
    {
        string firstText =
            @"Dieses Dokument enthält vertrauliche Informationen und ist lediglich für die Geschäftsführung sowie die IT-Abteilung der Firma " + _pdfReportDto.CompanyName + " vorgesehen. Eine Weitergabe an Dritte ist nicht gestattet und kann schwerwiegende Schäden für " + _pdfReportDto.CompanyName + " zur Folge haben.";

        string secondText =
            @"Der in diesem Dokument enthaltene Fragenkatalog wurde von Infotech - basierend auf branchenüblichen Standards - erstellt. Die Veröffentlichung, jede Art gewerblicher Nutzung sowie eine Weitergabe an Dritte ist nicht gestattet.";

        string thirdText =
            @"Die Fragen wurden im Zuge eines Interviews durch " + _pdfReportDto.CompanyName + " beantwortet. Die von Infotech empfohlenen Maßnahmen setzen die Richtigkeit und Vollständigkeit der Antworten voraus, da keine über ein Interview hinausgehenden Audits durchgeführt wurden.";

        string fourthText =
            @"Bei jeder Frage können 0 bis 3 Punkte erreicht werden, wobei 3 das Maximum ist. Wird eine Frage mit 0 Punkten beurteilt, bedeutet dies, dass das Thema im Unternehmen nicht behandelt wurde. Eine Beurteilung mit 1 Punkt bedeutet, dass bereits erste Maßnahmen umgesetzt bzw. die Anforderungen rudimentär erfüllt sind. Damit eine Frage mit 2 Punkten beurteilt wird, müssen die Anforderungen weitestgehend erfüllt sein, technische Maßnahmen weitestgehend dem Stand der Technik entsprechen und Prozesse grundlegend definiert und etabliert sein. Damit eine Frage mit 3 Punkten beurteilt wird, müssen technische Maßnahmen vollumfänglich umgesetzt und Prozesse definiert sein. Darüber hinaus muss die Wirksamkeit der technischen und organisatorischen Maßnahmen regelmäßig nachvollziehbar auditiert und verbessert werden.";

        string fithText =
            @"Es liegt in der Verantwortung von " + _pdfReportDto.CompanyName + " zu entscheiden, welche Handlungsempfehlungen in welcher Form umgesetzt werden.";

        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));
        Paragraph firstP = new Paragraph(firstText);
        firstP.Alignment = Element.ALIGN_CENTER;
        document.Add(firstP);
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));

        Paragraph secondP = new Paragraph(secondText);
        secondP.Alignment = Element.ALIGN_CENTER;
        document.Add(secondP);
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));

        Paragraph thirdP = new Paragraph(thirdText);
        thirdP.Alignment = Element.ALIGN_CENTER;
        document.Add(thirdP);
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));

        Paragraph fourthP = new Paragraph(fourthText);
        fourthP.Alignment = Element.ALIGN_CENTER;
        document.Add(fourthP);
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));

        Paragraph fithP = new Paragraph(fithText);
        fithP.Alignment = Element.ALIGN_CENTER;
        document.Add(fithP);
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));
    }
    //private void AddSecondPageMiddleText()
    //{
    //    Phrase firstText = new Phrase();
    //    Chunk chunk1 = new Chunk("Dieses Dokument enthält vertrauliche Informationen und ist lediglich für die Geschäftsführung sowie die IT-Abteilung der Firma 'Musterfirma' vorgesehen. Eine Weitergabe an Dritte ist nicht gestattet und kann schwerwiegende Schäden für Musterfirma zur Folge haben.");
    //    firstText.Add(chunk1);
    //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, firstText, 50, 100, 0);

    //    Phrase secondText = new Phrase();
    //    Chunk chunk2 = new Chunk("Der in diesem Dokument enthaltene Fragenkatalog wurde von Infotech - basierend auf branchenüblichen Standards - erstellt. Die Veröffentlichung, jede Art gewerblicher Nutzung sowie eine Weitergabe an Dritte ist nicht gestattet.");
    //    secondText.Add(chunk2);
    //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, secondText, 50, 200, 0);

    //    Phrase thirdText = new Phrase();
    //    Chunk chunk3 = new Chunk("Die Fragen wurden im Zuge eines Interviews durch Musterfirma beantwortet. Die von Infotech empfohlenen Maßnahmen setzen die Richtigkeit und Vollständigkeit der Antworten voraus, da keine über ein Interview hinausgehenden Audits durchgeführt wurden.");
    //    thirdText.Add(chunk3);
    //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, thirdText, 50, 300, 0);

    //    Phrase fourthText = new Phrase();
    //    Chunk chunk4 = new Chunk("Bei jeder Frage können 0 bis 3 Punkte erreicht werden, wobei 3 das Maximum ist. Wird eine Frage mit 0 Punkten beurteilt, bedeutet dies, dass das Thema im Unternehmen nicht behandelt wurde. Eine Beurteilung mit 1 Punkt bedeutet, dass bereits erste Maßnahmen umgesetzt bzw. die Anforderungen rudimentär erfüllt sind. Damit eine Frage mit 2 Punkten beurteilt wird, müssen die Anforderungen weitestgehend erfüllt sein, technische Maßnahmen weitestgehend dem Stand der Technik entsprechen und Prozesse grundlegend definiert und etabliert sein. Damit eine Frage mit 3 Punkten beurteilt wird, müssen technische Maßnahmen vollumfänglich umgesetzt und Prozesse definiert sein. Darüber hinaus muss die Wirksamkeit der technischen und organisatorischen Maßnahmen regelmäßig nachvollziehbar auditiert und verbessert werden.");
    //    fourthText.Add(chunk4);
    //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, fourthText, 50, 400, 0);

    //    Phrase fithText = new Phrase();
    //    Chunk chunk5 = new Chunk("Es liegt in der Verantwortung von Musterfirma zu entscheiden, welche Handlungsempfehlungen in welcher Form umgesetzt werden.");
    //    fithText.Add(chunk5);
    //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, fithText, 50, 500, 0);
    //}

    private void FirstPage()
    {
        FirstStepsCreatePDF();

        AddImgRightCorner();

        AddImgLeftMid();

        AddCompAddLoc();

        AddTablePOne();

        AddFirstPageFooter();
        //AddLinesToPDF();
    }

    private int GetMaxPageCount()
    {
        string relativePath = "PDFs/document.pdf";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        //PdfReader reader = new PdfReader(fullPath);
        int count = 9; //reader.NumberOfPages;
        return count;
    }

    private void AddNewEmptyPage()
    {
        document.NewPage();
    }

    private void AddFirstPageFooter()
    {
        // Erstellen der Phrase für die Fußzeile
        Phrase footerText = new Phrase();

        // Erste Zeile der Fußzeile
        Chunk chunk1 = new Chunk("INFOTECH ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.RED));
        Chunk chunk2 = new Chunk("EDV-Systeme GmbH", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
        footerText.Add(chunk1);
        footerText.Add(chunk2);
        float footerX = document.LeftMargin;
        float footerY = document.BottomMargin;
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, footerText, footerX, footerY + 20, 0);
        // Neue Zeile
        //footerText.Add(Chunk.NEWLINE);
        footerText = new Phrase();
        // Zweite Zeile der Fußzeile
        Chunk chunk3 = new Chunk("Schärdinger Straße 35 | 4910 Ried i. I. | ",
            new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk4 = new Chunk("Telefon", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD));
        Chunk chunk5 = new Chunk(" 07752 81711 0 | ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk6 = new Chunk("E-Mail ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk7 = new Chunk("office@infotech.at",
            new Font(Font.FontFamily.HELVETICA, 10, Font.UNDERLINE, BaseColor.RED));
        chunk7.SetAnchor("mailto:office@infotech.at");
        Chunk chunk8 = new Chunk(" | ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk9 = new Chunk("www.infotech.at",
            new Font(Font.FontFamily.HELVETICA, 10, Font.UNDERLINE, BaseColor.RED));
        chunk9.SetAnchor("http://www.infotech.at");
        footerText.Add(chunk3);
        footerText.Add(chunk4);
        footerText.Add(chunk5);
        footerText.Add(chunk6);
        footerText.Add(chunk7);
        footerText.Add(chunk8);
        footerText.Add(chunk9);

        // Positionieren der Fußzeile am unteren Rand der Seite
        //float footerX = document.LeftMargin;
        //float footerY = document.BottomMargin;

        // Schreiben der Fußzeile auf die Seite
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, footerText, footerX, footerY, 0);
    }

    private void AddTablePOne()
    {
        PdfPTable table = new PdfPTable(2);
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        float[] widths = { 2f, 3f };
        table.SetWidths(widths);
        Font normalFont = new Font(Font.FontFamily.HELVETICA, 12);
        Font boldFont = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
        // Erstellen der linken Spalte
        PdfPCell leftCell1 = new PdfPCell(new Phrase("Art der Durchführung", normalFont));
        leftCell1.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell1.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell1.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                           Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell1);

        PdfPCell rightCell1 = new PdfPCell(new Phrase(_pdfReportDto.TypeOfExecution, normalFont));
        table.AddCell(rightCell1);
        //PdfPCell leftCell2 = new PdfPCell(new Phrase("Interview / Selbstauskunft", normalFont));
        //leftCell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //leftCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
        //leftCell2.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        //table.AddCell(leftCell2);

        PdfPCell leftCell3 = new PdfPCell(new Phrase("Datum der Durchführung", normalFont));
        leftCell3.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell3.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell3.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                           Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell3);
        PdfPCell rightCell2 = new PdfPCell(new Phrase(DateTime.Now.ToString("dd.MM.yyyy"), normalFont));
        table.AddCell(rightCell2);
        //PdfPCell leftCell4 = new PdfPCell(new Phrase("02.05.2023", normalFont));
        //leftCell4.HorizontalAlignment = Element.ALIGN_LEFT;
        //leftCell4.VerticalAlignment = Element.ALIGN_MIDDLE;
        //leftCell4.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        //table.AddCell(leftCell4);

        PdfPCell leftCell5 = new PdfPCell(new Phrase("Teilnehmer", normalFont));
        leftCell5.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell5.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell5.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                           Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell5);
        PdfPCell rightCell3 =
            new PdfPCell(new Phrase(_pdfReportDto.Participants, normalFont));
        table.AddCell(rightCell3);
        //PdfPCell leftCell6 = new PdfPCell(new Phrase("Max Mayr(IT-Verantwortlicher) Martin Mallinger (Infotech, Auditor)", normalFont));
        //leftCell6.HorizontalAlignment = Element.ALIGN_LEFT;
        //leftCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
        //leftCell6.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        //table.AddCell(leftCell6);

        PdfPCell leftCell7 = new PdfPCell(new Phrase("Scope", normalFont));
        leftCell7.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell7.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell7.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell7.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                           Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell7);
        PdfPCell rightCell4 =
            new PdfPCell(new Phrase(
                _pdfReportDto.Scope, normalFont));
        table.AddCell(rightCell4);
        //PdfPCell leftCell8 = new PdfPCell(new Phrase("Betrachtet wird die Informationssicherheit der Unternehmensgruppe. Es gibt keine Außenstandorte. Ca. 500 Mitarbeiter, davon 4505 IT-Arbeitsplätze und ca. 300 bis 350 IT-Nutzer.", normalFont));
        //leftCell8.HorizontalAlignment = Element.ALIGN_LEFT;
        //leftCell8.VerticalAlignment = Element.ALIGN_MIDDLE;
        //leftCell8.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        //table.AddCell(leftCell8);

        PdfPCell leftCell9 = new PdfPCell(new Phrase("Version Security Check", normalFont));
        leftCell9.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell9.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell9.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell9.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                           Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell9);

        PdfPCell rightCell5 = new PdfPCell(new Phrase("1.5", normalFont));
        table.AddCell(rightCell5);

        PdfPCell leftCell10 = new PdfPCell(new Phrase("Klassifizierung", normalFont));
        leftCell10.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell10.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell10.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell10.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                            Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell10);

        PdfPCell rightCell6 = new PdfPCell(new Phrase(_pdfReportDto.Classification, normalFont));
        table.AddCell(rightCell6);

        PdfPCell leftCell11 = new PdfPCell(new Phrase("Dokumentenverteiler", normalFont));
        leftCell11.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell11.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell11.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell11.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER |
                            Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell11);

        PdfPCell rightCell7 =
            new PdfPCell(new Phrase(_pdfReportDto.DocumentDistributor, normalFont));
        table.AddCell(rightCell7);
        // Rechte Spalte
        //PdfPCell rightCell1 = new PdfPCell(new Phrase("Interview / Selbstauskunft", normalFont));
        //PdfPCell rightCell2 = new PdfPCell(new Phrase(DateTime.Now.ToString("dd.MM.yyyy"), normalFont));
        //PdfPCell rightCell3 = new PdfPCell(new Phrase("Max Mayr (IT-Verantwortlicher) Martin Mallinger (Infotech, Auditor)", normalFont));
        //PdfPCell rightCell4 = new PdfPCell(new Phrase("Betrachtet wird die Informationssicherheit der Unternehmensgruppe. Es gibt keine Außenstandorte. Ca. 500 Mitarbeiter, davon 450 IT-Arbeitsplätze und ca. 300 bis 350 IT-Nutzer.", normalFont));
        //PdfPCell rightCell5 = new PdfPCell(new Phrase("1.5", normalFont));
        //PdfPCell rightCell6 = new PdfPCell(new Phrase("Vertraulich", normalFont));
        //PdfPCell rightCell7 = new PdfPCell(new Phrase("Max Mayr (IT-Verantwortlicher) Martin Mallinger (Infotech, Auditor)", normalFont));

        //table.AddCell(leftCell1);
        //table.AddCell(rightCell1);
        //table.AddCell(leftCell2);
        //table.AddCell(rightCell2);
        //table.AddCell(leftCell3);
        //table.AddCell(rightCell3);
        //table.AddCell(leftCell4);
        //table.AddCell(rightCell4);
        //table.AddCell(leftCell5);
        //table.AddCell(rightCell5);
        //table.AddCell(leftCell6);
        //table.AddCell(rightCell6);
        //table.AddCell(leftCell7);
        //table.AddCell(rightCell7);

        leftCell1.BorderWidth = 1f;
        leftCell1.BorderColor = BaseColor.LIGHT_GRAY;
        rightCell1.BorderWidth = 1f;
        rightCell1.BorderColor = BaseColor.LIGHT_GRAY;

        //leftCell2.BorderWidth = 1f;
        //leftCell2.BorderColor = BaseColor.BLACK;
        rightCell2.BorderWidth = 1f;
        rightCell2.BorderColor = BaseColor.LIGHT_GRAY;

        leftCell3.BorderWidth = 1f;
        leftCell3.BorderColor = BaseColor.LIGHT_GRAY;
        rightCell3.BorderWidth = 1f;
        rightCell3.BorderColor = BaseColor.LIGHT_GRAY;

        //leftCell4.BorderWidth = 1f;
        //leftCell4.BorderColor = BaseColor.BLACK;
        rightCell4.BorderWidth = 1f;
        rightCell4.BorderColor = BaseColor.LIGHT_GRAY;

        leftCell5.BorderWidth = 1f;
        leftCell5.BorderColor = BaseColor.LIGHT_GRAY;
        rightCell5.BorderWidth = 1f;
        rightCell5.BorderColor = BaseColor.LIGHT_GRAY;

        //leftCell6.BorderWidth = 1f;
        //leftCell6.BorderColor = BaseColor.BLACK;
        rightCell6.BorderWidth = 1f;
        rightCell6.BorderColor = BaseColor.LIGHT_GRAY;

        leftCell7.BorderWidth = 1f;
        leftCell7.BorderColor = BaseColor.LIGHT_GRAY;
        rightCell7.BorderWidth = 1f;
        rightCell7.BorderColor = BaseColor.LIGHT_GRAY;

        leftCell9.BorderWidth = 1f;
        leftCell9.BorderColor = BaseColor.LIGHT_GRAY;

        leftCell10.BorderWidth = 1f;
        leftCell10.BorderColor = BaseColor.LIGHT_GRAY;

        leftCell11.BorderWidth = 1f;
        leftCell11.BorderColor = BaseColor.LIGHT_GRAY;

        table.WriteSelectedRows(0, -1, 50, 450, writer.DirectContent);
        //document.Add(table);
        //Phrase footer = new Phrase("Wien, am " + DateTime.Now.ToString("dd.MM.yyyy"), normalFont);
        //footer.SetLeading(0, 2);

        // Erstellen Sie eine neue Tabelle mit einer Spalte
        PdfPTable table2 = new PdfPTable(1);

        // Erstellen Sie eine Zelle für den Footer-Text
        PdfPCell cell = new PdfPCell(new Phrase("Wien, am " + DateTime.Now.ToString("dd.MM.yyyy"), normalFont));

        // Positionieren Sie die Zelle am unteren Rand der Seite
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        cell.Border = Rectangle.NO_BORDER;
        cell.PaddingBottom = 10f;

        // Fügen Sie die Zelle zur Tabelle hinzu
        table2.AddCell(cell);

        // Positionieren Sie die Tabelle am unteren Rand der Seite
        table2.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table2.WriteSelectedRows(0, -1, -140, 280, writer.DirectContent);


        //document.Add(footer);
    }

    private void AddCompAddLoc()
    {
        Font boldFont = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
        Chunk chunk1 = new Chunk(_pdfReportDto.CompanyName, boldFont);
        Chunk chunk2 = new Chunk("Anschrift", boldFont);
        Chunk chunk3 = new Chunk("Ort", boldFont);

        // Erstellen einer Phrase mit den drei Chunks
        Phrase phrase = new Phrase();
        phrase.Add(chunk1);
        phrase.Add(Chunk.NEWLINE); // Hinzufügen einer Zeilenumbruchsequenz
        phrase.Add(chunk2);
        phrase.Add(Chunk.NEWLINE); // Hinzufügen einer Zeilenumbruchsequenz
        phrase.Add(chunk3);

        PdfPTable table = new PdfPTable(1);
        table.TotalWidth = 300;

        // Hinzufügen der Phrase als Zelle in die Tabelle
        PdfPCell cell = new PdfPCell();
        cell.AddElement(phrase);
        cell.Border = Rectangle.NO_BORDER;
        table.AddCell(cell);

        // Schreiben der Tabelle an die Position (50, 500)
        table.WriteSelectedRows(0, -1, 50, 520, writer.DirectContent);
    }

    private void AddImgLeftMid()
    {
        string relativePath = "Pictures/infotechSecurCheck.PNG";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(fullPath);
        image.SetAbsolutePosition(50, 610);

        // Vergrößern des Abstands nach oben um 50 Punkte
        //image.SetAbsolutePosition(image.AbsoluteX, image.AbsoluteY + 50);

        image.ScaleToFit(250f, 100f);
        //image.SpacingBefore = 10f;
        ////image.ScaleToFit(document.PageSize);
        //image.Alignment = iTextSharp.text.Image.ALIGN_LEFT;

        //image.setFixedPosition(100, 250);
        document.Add(image);
    }

    private void AddLinesToPDF()
    {
        //document.Add(new Paragraph("Hello, World!"));
    }

    private void FirstStepsCreatePDF()
    {
        Console.WriteLine("Creating PDF Document");

        document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 25, 25);

        string filePath = "C:\\Users\\kinzl\\Downloads\\pdfreport.pdf";

        writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

        document.Open();
    }

    private void CloseDocument()
    {
        document.Close();
        writer.Close();
    }

    private void AddTable()
    {
        PdfPTable table = new PdfPTable(3);
        table.HorizontalAlignment = Element.ALIGN_CENTER;
        table.WidthPercentage = 100;
        table.TotalWidth = 698.5f;
        table.LockedWidth = true;
        table.SetWidths(new float[] { 1, 1, 1 });
        iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(
            "C:\\Schule\\2022-23KoglerD190073\\SYP\\PDFReport\\CreatePDFReport\\CreatePDFReport\\Pictures\\infotech-logo-farbe.png");
        img1.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        img1.ScaleToFit(120f, 155.25f);

        iTextSharp.text.pdf.PdfPCell imgCell1 = new iTextSharp.text.pdf.PdfPCell(img1);
        imgCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        imgCell1.BackgroundColor = new BaseColor(255, 255, 255);
        imgCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        table.AddCell(imgCell1);

        document.Add(table);
    }

    public void AddImgRightCorner()
    {
        string relativePath = "Pictures/infotechLogoNeu.jpg";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(fullPath);
        image.ScaleToFit(160f, 80f);
        image.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
        //image.IndentationLeft = 9f;
        //image.SpacingAfter = 120f;
        document.Add(image);
    }

    private void AddTextAbouveTable()
    {
        Phrase general = new Phrase();
        Chunk header = new Chunk("MANAGEMENT SUMMARY", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD, BaseColor.RED));

        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 720, 0);

        //string managementSummaryText = @"Am 28.03.2023 wurde der Ist-Stand hinsichtlich Informationssicherheit und Datenschutz durch die Beantwortung von 98 Fragen erhoben. Die Erhebung der Informationen wurde im Zuge einer Befragung von Max Mustermann durch Hr. Martin Mallinger (Infotech) durchgeführt.";

        string managementSummaryText = _pdfReportDto.ManagementSummary;
        // Erstellen eines neuen Paragraphs mit dem Text
        Paragraph managementSummaryParagraph = new Paragraph(managementSummaryText);

        // Festlegen der gewünschten Position
        float x = 50; // X-Koordinate
        float y = 700; // Y-Koordinate

        // Erstellen einer ColumnText-Instanz und Festlegen der Position
        ColumnText columnText = new ColumnText(writer.DirectContent);
        columnText.SetSimpleColumn(x, y, PageSize.A4.Width - 50, y - 200); // 200 ist die maximale Höhe für den Absatz

        // Hinzufügen des Paragraphs zum ColumnText
        columnText.AddElement(managementSummaryParagraph);

        // Ausrichtung des Textes
        columnText.Alignment = Element.ALIGN_LEFT;

        // Schleife zum Hinzufügen von Absätzen bei Bedarf
        while (ColumnText.HasMoreText(columnText.Go()))
        {
            // Erstellen eines neuen Absatzes
            Paragraph newParagraph = new Paragraph();

            // Hinzufügen des Absatzes zum ColumnText
            columnText.AddElement(newParagraph);
        }

        // Schließen des ColumnText
        columnText.Go();

        string tableHeaderText = @"Im Zuge des IT Security Checks wurde der Ist-Stand zu folgenden Themenbereichen erhoben: ";
        Paragraph tableHeaderParagraph = new Paragraph(tableHeaderText);

        // Festlegen der gewünschten Position
        x = 50; // X-Koordinate
        y = 620; // Y-Koordinate

        columnText = new ColumnText(writer.DirectContent);
        columnText.SetSimpleColumn(x, y, PageSize.A4.Width - 50, y - 200); // 200 ist die maximale Höhe für den Absatz

        // Hinzufügen des Paragraphs zum ColumnText
        columnText.AddElement(tableHeaderParagraph);

        // Ausrichtung des Textes
        columnText.Alignment = Element.ALIGN_LEFT;

        // Schleife zum Hinzufügen von Absätzen bei Bedarf
        while (ColumnText.HasMoreText(columnText.Go()))
        {
            // Erstellen eines neuen Absatzes
            Paragraph newParagraph = new Paragraph();

            // Hinzufügen des Absatzes zum ColumnText
            columnText.AddElement(newParagraph);
        }

        // Schließen des ColumnText
        columnText.Go();
    }

    private void AddMMSTable()
    {
        PdfPTable table = new PdfPTable(5);

        // Festlegen der Breitenverhältnisse der Spalten
        float[] columnWidths = { 4f, 1f, 1f, 1f, 1f };
        table.SetWidths(columnWidths);

        // Hinzufügen des Headers
        string[] headers = { "Themenbereich", "Fragen", "Max. Punkte", "Erreichte Punkte", "Erreichte Punkte (%)" };
        foreach (string headerText in headers)
        {
            PdfPCell headerCell = new PdfPCell(new Phrase(headerText, new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.RED)));
            headerCell.BackgroundColor = new BaseColor(210, 210, 210); // Hellgraue Farbe für den Hintergrund
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(headerCell);
        }

        // Befüllen des Body mit den Themenbereichen und Zufallswerten
        //string[] themenbereiche = { "Organisation", "Nutzungsrichtlinie", "Geheimhaltung und Datenschutz", "Asset- und Risikomanagement", "Notfallmanagement", "Awareness", "Systembetrieb", "Netzwerk und Kommunikation", "Zutritts- und Zugriffsberechtigung" };
        List<string> themenbereiche = _allQuestionsAndAnswers.Select(x => x.Category).Distinct().ToList();
        Random random = new Random();
        foreach (string themenbereich in themenbereiche)
        {
            table.AddCell(themenbereich);
            int nrQuestion = _allQuestionsAndAnswers.Where(x => x.Category == themenbereich).Select(x => x.Question).Count();
            table.AddCell(nrQuestion.ToString());
            //table.AddCell(random.Next(1, 10).ToString()); // Fragen
            double maxPoints = nrQuestion * 4;
            table.AddCell(maxPoints.ToString()); // Max. Punkte
            double reachedPoints = calculateReachedPointsForCategory(themenbereich);
            table.AddCell(reachedPoints.ToString()); // Erreichte Punkte
            double percantage = 0;
            try
            {
                percantage = ((reachedPoints / maxPoints) * 100);
            }
            catch
            {
                percantage = 0;
            }
            table.AddCell(percantage.ToString("#,###") + " %"); // Erreichte Punkte (%)
        }

        // Festlegen der Trennungslinien
        //table.DefaultCell.Border = Rectangle.NO_BORDER;
        //table.DefaultCell.BorderColor = BaseColor.GRAY;
        //table.DefaultCell.BorderWidthBottom = 0.5f;

        // Hinzufügen der Tabelle zum Dokument
        float xPosition = 50;
        float yPosition = 580;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);
        // Tabelle auf die gewünschte Position zeichnen
        //table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);
        //document.Add(table);

    }

    private void AddThirdPageLastText()
    {
        string firstParagraph = @"Gesamt wurden 26 Abweichungen bzw. Punkte mit Verbesserungspotential identifiziert. Für diese Abweichungen werden 14 Handlungsempfehlungen mit der Kritikalität ´hoch` eingestuft. Weitere 10 Handlungsempfehlungen wurden mit der Kritikalität ´mittel` beurteilt. Den restlichen 2 Hand- lungsempfehlungen wurde die Kritikalität ´niedrig` zugewiesen.";

        for (int i = 0; i < 19; i++)
        {
            document.Add(new Paragraph("\n"));
        }

        Paragraph firstP = new Paragraph(firstParagraph);
        firstP.Alignment = Element.ALIGN_CENTER;
        document.Add(firstP);
        string secondParagraph = @"In den Bereichen der organisatorischen Informationssicherheit ´Organisation`, ´Nutzungsrichtlinie`, ´Geheimhaltung und Datenschutz`, ´Asset- und Risikomanagement`, ´Notfallmanagement` und ´Awareness` wurde folgende Hauptabweichungen identifiziert:";
        document.Add(new Paragraph("\n"));
        firstP = new Paragraph(secondParagraph);
        firstP.Alignment = Element.ALIGN_CENTER;
        document.Add(firstP);
        string thirdParagraph = @"             • Es gibt keine Regelung für die korrekte Entsorgung von Medien";
        firstP = new Paragraph(thirdParagraph);
        document.Add(firstP);
        //document.Add(new Paragraph("\n"));

        string fourthParagraph = @"             • Es gibt keine Notfallpläne";
        firstP = new Paragraph(fourthParagraph);
        document.Add(firstP);
        document.Add(new Paragraph("\n"));

        string fithParagraph = @"Im Bereich der technischen Informationssicherheit, ´Systembetrieb`, ´Netzwerk und Kommunikation` und ´Zutritts- und Zugriffsberechtigungen` wurden folgende Hauptabweichungen festgestellt:";
        firstP = new Paragraph(fithParagraph);
        firstP.Alignment = Element.ALIGN_CENTER;
        document.Add(firstP);
        document.Add(new Paragraph("\n"));
    }

    private void AddStandardFooter()
    {
        // Rechteck hinzufügen
        PdfContentByte canvas = writer.DirectContent;
        Rectangle rect = new Rectangle(0, 0, PageSize.A4.Width, 100);
        rect.BackgroundColor = new BaseColor(82, 82, 82); // Graue Farbe setzen
        canvas.Rectangle(rect);

        Phrase general = new Phrase();

        Chunk checkCompanyDate = new Chunk("IT Security Check, Musterfirma, " + DateTime.Now.ToString("MMM yyyy"));
        general.Add(checkCompanyDate);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 110, 0);

        general = new Phrase();

        Chunk pageCount = new Chunk("Seite " + document.PageNumber + " / ");
        Chunk maxPageCount = new Chunk(GetMaxPageCount() + "", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));

        general.Add(pageCount);
        general.Add(maxPageCount);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 420, 110, 0);



        //ColumnText columnText = new ColumnText(writer.DirectContent);
        //columnText.SetSimpleColumn(40, 80, 200, 60); // Koordinaten und Größe des Textbereichs anpassen

        //BaseColor backgroundColor = new BaseColor(50, 50, 50); // Dunkelgraue Hintergrundfarbe

        //// Rechteck für den Hintergrund zeichnen
        //PdfContentByte contentByte = writer.DirectContent;
        //contentByte.SetColorFill(backgroundColor);
        //contentByte.Fill();



        // Text schreiben
        general = new Phrase();

        Chunk redLine = new Chunk("| ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.RED));
        Chunk firstGrayLine = new Chunk("Haus des Internets ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));
        Chunk secondGrayLine = new Chunk("Haus der Sicherheit ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));
        Chunk thirdGrayLine = new Chunk("Haus der Digitalisierung ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));

        general.Add(redLine);
        general.Add(firstGrayLine);
        general.Add(redLine);
        general.Add(secondGrayLine);
        general.Add(redLine);
        general.Add(thirdGrayLine);
        ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, general, 40, 70, 0);

        general = new Phrase();

        Chunk fourthGrayLine = new Chunk("Haus der Arbeitsplätze ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));
        Chunk fithGrayLine = new Chunk("Haus der Daten ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));
        Chunk sixthGrayLine = new Chunk("Haus der Netzwerke ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));

        general.Add(redLine);
        general.Add(fourthGrayLine);
        general.Add(redLine);
        general.Add(fithGrayLine);
        general.Add(redLine);
        general.Add(sixthGrayLine);
        ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, general, 40, 50, 0);

        general = new Phrase();

        Chunk seventhGrayLine = new Chunk("INFOTECH EDV-Systeme GmbH | Schärdinger Straße 35 | 4910 Ried i.I. | Tel +43 7752 81711 | E-Mail office@infotech.at | www.infotech.at", new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE));
        general.Add(seventhGrayLine);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 40, 20, 0);

        string relativePath = "Pictures/FooterRightBottom.png";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        Image image = Image.GetInstance(fullPath);
        image.ScaleToFit(210, 90);
        image.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
        image.Border = 20;
        image.BorderColor = BaseColor.WHITE;

        float x = 500;
        float y = 30;

        canvas.AddImage(image, image.ScaledWidth, 0f, 0f, image.ScaledHeight, x, y);

        //Das mit den Max Seitenanzahl geht nicht
        //Infotech fragen, ob sie report schicken können!
    }

    private void AddPoints()
    {
        //Paragraph firstP = new Paragraph();
        //document.Add(new Paragraph("\n"));
        //string p = @"             • Mobile Datenträger werden nicht verschlüsselt";
        //firstP = new Paragraph(p);
        //document.Add(firstP);


        //p = @"             • Kryptografische Schlüssel werden nicht zentral verwaltet";
        //firstP = new Paragraph(p);
        //document.Add(firstP);

        //p = @"             • Es werden nicht mehrere Backupstände extern vorgehalten";
        //firstP = new Paragraph(p);
        //document.Add(firstP);

        //p = @"             • Die Funktionalität der Datensicherung wird nicht regelmäßig geprüft";
        //firstP = new Paragraph(p);
        //document.Add(firstP);

        //p = @"             • Extern erreichbare Zugänge werden nicht durch eine Multi-Faktor-Authentifizierung                  geschützt";
        //firstP = new Paragraph(p);
        //document.Add(firstP);

        //p = @"             • Einige Benutzer arbeiten permanent mit privilegierten Konten";
        //firstP = new Paragraph(p);
        //document.Add(firstP);

        //string firstParagraph = @"Unter Berücksichtigung aller Fragen ergibt sich ein durchschnittlicher Security Score von 0,9 von maximal 3 möglichen Punkten. Wird pro Kategorie ein Security Score von >= 2,0 Punkten erreicht, kann davon ausgegangen werden, dass die Anforderungen weitestgehend erfüllt sind und einem gutem Sicherheitsniveau entsprechen. Um einen Score von 3 Punkten zu erreichen, müssen alle Anforderungen gängiger Normen erfüllt sein. Dies erfordert einerseits umfangreiche technische Maßnahmen sowie definierte und gelebte Prozesse samt regelmäßigen Audits und Verbesserungen.";

        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //document.Add(new Paragraph("\n"));
        //firstP = new Paragraph(firstParagraph);
        //document.Add(firstP);

        string relativePath = "Pictures/ReportGraphic.png";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(fullPath);
        image.SetAbsolutePosition(50, 150);

        image.ScaleToFit(475f, 420f);
        document.Add(image);

    }
    private void AddDetails(string category)
    {
        Phrase general = new Phrase();
        Chunk header = new Chunk("DETAILS", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD, BaseColor.RED));

        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 720, 0);

        general = new Phrase();
        header = new Chunk(category, new Font(Font.FontFamily.HELVETICA, 15, Font.BOLD, BaseColor.RED));
        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 680, 0);

    }

    private void AddFirstDetailQuestion(QuestionDto question)
    {
        var general = new Phrase();
        var header = new Chunk(question.Question, new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD, BaseColor.WHITE));
        header.SetBackground(new BaseColor(64, 64, 64));
        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 655, 0);

        // Erstellen einer Tabelle für die Checkboxen
        //for (int i = 0; i < 6; i++)
        //{
        //    document.Add(new Paragraph("\n"));
        //}


        PdfPTable table = new PdfPTable(3);
        table.DefaultCell.Border = Rectangle.NO_BORDER;

        float[] columnWidths = { 0.5f, 3f, 16f };
        table.SetWidths(columnWidths);



        // Checkbox 0: Ausgefüllt

        PdfPCell cell2 = new PdfPCell();
        cell2.Border = Rectangle.NO_BORDER;
        cell2.CellEvent = new CheckboxCellEvent(question.Answer.AnswerZero.Selected);
        table.AddCell(cell2);

        PdfPCell cell1 = new PdfPCell(new Phrase("Bewertung 0:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell1.Border = Rectangle.NO_BORDER;
        table.AddCell(cell1);

        PdfPCell cell9 = new PdfPCell(new Phrase(question.Answer.AnswerZero.Answertext));
        cell9.Border = Rectangle.NO_BORDER;
        table.AddCell(cell9);

        // Checkbox 1: Nicht ausgefüllt
        PdfPCell cell4 = new PdfPCell();
        cell4.Border = Rectangle.NO_BORDER;
        cell4.CellEvent = new CheckboxCellEvent(question.Answer.AnswerOne.Selected);
        table.AddCell(cell4);

        PdfPCell cell3 = new PdfPCell(new Phrase("Bewertung 1:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell3.Border = Rectangle.NO_BORDER;
        table.AddCell(cell3);

        PdfPCell cell10 = new PdfPCell(new Phrase(question.Answer.AnswerOne.Answertext));
        cell10.Border = Rectangle.NO_BORDER;
        table.AddCell(cell10);

        // Checkbox 2: Nicht ausgefüllt
        PdfPCell cell6 = new PdfPCell();
        cell6.Border = Rectangle.NO_BORDER;
        cell6.CellEvent = new CheckboxCellEvent(question.Answer.AnswerTwo.Selected);
        table.AddCell(cell6);

        PdfPCell cell5 = new PdfPCell(new Phrase("Bewertung 2:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell5.Border = Rectangle.NO_BORDER;
        table.AddCell(cell5);

        PdfPCell cell11 = new PdfPCell(new Phrase(question.Answer.AnswerTwo.Answertext));
        cell11.Border = Rectangle.NO_BORDER;
        table.AddCell(cell11);

        // Checkbox 3: Nicht ausgefüllt
        PdfPCell cell8 = new PdfPCell();
        cell8.Border = Rectangle.NO_BORDER;
        cell8.CellEvent = new CheckboxCellEvent(question.Answer.AnswerThree.Selected);
        table.AddCell(cell8);

        PdfPCell cell7 = new PdfPCell(new Phrase("Bewertung 3:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell7.Border = Rectangle.NO_BORDER;
        table.AddCell(cell7);

        PdfPCell cell12 = new PdfPCell(new Phrase(question.Answer.AnswerThree.Answertext));
        cell12.Border = Rectangle.NO_BORDER;
        table.AddCell(cell12);

        float xPosition = 50;
        float yPosition = 640;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

        //-------------------------- 2. Tabelle

        table = new PdfPTable(2);
        table.WidthPercentage = 100;
        table.DefaultCell.Border = Rectangle.NO_BORDER;
        float[] c = { 5f, 4f };
        table.SetWidths(c);
        // Erstellen des linken Headers
        PdfPCell header1 = new PdfPCell(new Phrase("BEGRÜNDUNG/UMGESETZTE MASSNAHMEN:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
        header1.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header1);

        // Erstellen des rechten Headers
        PdfPCell header2 = new PdfPCell(new Phrase("RISIKO/EMPFEHLUNG:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
        header2.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header2);

        // Einfügen des Textes in der linken Spalte
        cell1 = new PdfPCell(new Phrase(question.Reason, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell1.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund
        table.AddCell(cell1);

        // Einfügen des Textes in der rechten Spalte
        cell2 = new PdfPCell(new Phrase(question.Recommendation, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell2.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund


        table.AddCell(cell2);

        xPosition = 50;
        yPosition = 520;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);
    }

    private void AddSecondDetailQuestion(QuestionDto question)
    {
        var general = new Phrase();
        var header = new Chunk(question.Question, new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD, BaseColor.WHITE));
        header.SetBackground(new BaseColor(64, 64, 64));
        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 400, 0);

        // Erstellen einer Tabelle für die Checkboxen
        //for (int i = 0; i < 6; i++)
        //{
        //    document.Add(new Paragraph("\n"));
        //}


        PdfPTable table = new PdfPTable(3);
        table.DefaultCell.Border = Rectangle.NO_BORDER;

        float[] columnWidths = { 0.5f, 3f, 16f };
        table.SetWidths(columnWidths);



        // Checkbox 0: Ausgefüllt

        PdfPCell cell2 = new PdfPCell();
        cell2.Border = Rectangle.NO_BORDER;
        cell2.CellEvent = new CheckboxCellEvent(question.Answer.AnswerZero.Selected);
        table.AddCell(cell2);

        PdfPCell cell1 = new PdfPCell(new Phrase("Bewertung 0:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell1.Border = Rectangle.NO_BORDER;
        table.AddCell(cell1);

        PdfPCell cell9 = new PdfPCell(new Phrase(question.Answer.AnswerZero.Answertext));
        cell9.Border = Rectangle.NO_BORDER;
        table.AddCell(cell9);

        // Checkbox 1: Nicht ausgefüllt
        PdfPCell cell4 = new PdfPCell();
        cell4.Border = Rectangle.NO_BORDER;
        cell4.CellEvent = new CheckboxCellEvent(question.Answer.AnswerOne.Selected);
        table.AddCell(cell4);

        PdfPCell cell3 = new PdfPCell(new Phrase("Bewertung 1:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell3.Border = Rectangle.NO_BORDER;
        table.AddCell(cell3);

        PdfPCell cell10 = new PdfPCell(new Phrase(question.Answer.AnswerOne.Answertext));
        cell10.Border = Rectangle.NO_BORDER;
        table.AddCell(cell10);

        // Checkbox 2: Nicht ausgefüllt
        PdfPCell cell6 = new PdfPCell();
        cell6.Border = Rectangle.NO_BORDER;
        cell6.CellEvent = new CheckboxCellEvent(question.Answer.AnswerTwo.Selected);
        table.AddCell(cell6);

        PdfPCell cell5 = new PdfPCell(new Phrase("Bewertung 2:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell5.Border = Rectangle.NO_BORDER;
        table.AddCell(cell5);

        PdfPCell cell11 = new PdfPCell(new Phrase(question.Answer.AnswerTwo.Answertext));
        cell11.Border = Rectangle.NO_BORDER;
        table.AddCell(cell11);

        // Checkbox 3: Nicht ausgefüllt
        PdfPCell cell8 = new PdfPCell();
        cell8.Border = Rectangle.NO_BORDER;
        cell8.CellEvent = new CheckboxCellEvent(question.Answer.AnswerThree.Selected);
        table.AddCell(cell8);

        PdfPCell cell7 = new PdfPCell(new Phrase("Bewertung 3:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell7.Border = Rectangle.NO_BORDER;
        table.AddCell(cell7);

        PdfPCell cell12 = new PdfPCell(new Phrase(question.Answer.AnswerThree.Answertext));
        cell12.Border = Rectangle.NO_BORDER;
        table.AddCell(cell12);

        float xPosition = 50;
        float yPosition = 385;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

        //-------------------------- 2. Tabelle

        table = new PdfPTable(2);
        table.WidthPercentage = 100;
        table.DefaultCell.Border = Rectangle.NO_BORDER;

        // Erstellen des linken Headers
        PdfPCell header1 = new PdfPCell(new Phrase("BEGRÜNDUNG/UMGESETZTE MASSNAHMEN:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)));
        header1.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header1);

        // Erstellen des rechten Headers
        PdfPCell header2 = new PdfPCell(new Phrase("RISIKO/EMPFEHLUNG:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)));
        header2.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header2);

        // Einfügen des Textes in der linken Spalte
        cell1 = new PdfPCell(new Phrase(question.Reason, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell1.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund
        table.AddCell(cell1);

        // Einfügen des Textes in der rechten Spalte
        cell2 = new PdfPCell(new Phrase(question.Recommendation, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell2.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund


        table.AddCell(cell2);

        xPosition = 50;
        yPosition = 270;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

        //--------------SECOND

        //AddSecondDetailsTable();

    }

    private void DetailsPages()
    {
        //AddImgRightCorner();

        //AddDetails();


        //AddStandardFooter();
        int nrCategories = _allQuestionsAndAnswers.Select(x => x.Category).Distinct().Count();
        int nrQuestion = _allQuestionsAndAnswers.Select(x => x.Question).Count();
        bool isEven = false;
        if (nrQuestion % 2 == 1) isEven = true;


        for (int i = 0; i < nrCategories; i++)
        {
            //AddNewEmptyPage();
            var currentCategory = _allQuestionsAndAnswers.Select(x => x.Category).ToList()[i];
            AddDetails(currentCategory);
            AddImgRightCorner();
            int nrQuestions = (_allQuestionsAndAnswers.Where(x => x.Category == currentCategory).Select(x => x.Question).Count());
            for (int j = 0; j < nrQuestion; j++)
            {
                try
                {
                    AddFirstDetailQuestion(_allQuestionsAndAnswers.Where(x => x.Category == currentCategory).Select(x => x).ToList()[j]);
                    AddSecondDetailQuestion(_allQuestionsAndAnswers.Where(x => x.Category == currentCategory).Select(x => x).ToList()[++j]);
                    AddNewEmptyPage();
                    //j++;
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        //for (int i = 0; i < 6; i++)
        //{
        //    AddNewEmptyPage();
        //    AddImgRightCorner();
        //    AddSecondDetailsTable();

        //    AddStandardFooter();
        //}

    }

    private void AddSecondDetailsTable()
    {
        var general = new Phrase();
        var header = new Chunk("1. UNTERSTÜTZT DAS MANAGEMENT DAS THEMA INFORMATIONSSICHERHEIT?", new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD, BaseColor.WHITE));
        header.SetBackground(new BaseColor(64, 64, 64));
        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 720, 0);

        // Erstellen einer Tabelle für die Checkboxen
        //for (int i = 0; i < 6; i++)
        //{
        //    document.Add(new Paragraph("\n"));
        //}


        PdfPTable table = new PdfPTable(3);
        table.DefaultCell.Border = Rectangle.NO_BORDER;

        float[] columnWidths = { 0.5f, 3f, 16f };
        table.SetWidths(columnWidths);



        // Checkbox 0: Ausgefüllt

        PdfPCell cell2 = new PdfPCell();
        cell2.Border = Rectangle.NO_BORDER;
        cell2.CellEvent = new CheckboxCellEvent(true);
        table.AddCell(cell2);

        PdfPCell cell1 = new PdfPCell(new Phrase("Bewertung 0:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell1.Border = Rectangle.NO_BORDER;
        table.AddCell(cell1);

        PdfPCell cell9 = new PdfPCell(new Phrase("Das Thema Informationssicherheit wird vom Management nicht getragen."));
        cell9.Border = Rectangle.NO_BORDER;
        table.AddCell(cell9);

        // Checkbox 1: Nicht ausgefüllt
        PdfPCell cell4 = new PdfPCell();
        cell4.Border = Rectangle.NO_BORDER;
        cell4.CellEvent = new CheckboxCellEvent(false);
        table.AddCell(cell4);

        PdfPCell cell3 = new PdfPCell(new Phrase("Bewertung 1:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell3.Border = Rectangle.NO_BORDER;
        table.AddCell(cell3);

        PdfPCell cell10 = new PdfPCell(new Phrase("Das Thema Informationssicherheit ist dem Management bekannt, wird aber nur eingeschränkt unterstützt."));
        cell10.Border = Rectangle.NO_BORDER;
        table.AddCell(cell10);

        // Checkbox 2: Nicht ausgefüllt
        PdfPCell cell6 = new PdfPCell();
        cell6.Border = Rectangle.NO_BORDER;
        cell6.CellEvent = new CheckboxCellEvent(false);
        table.AddCell(cell6);

        PdfPCell cell5 = new PdfPCell(new Phrase("Bewertung 2:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell5.Border = Rectangle.NO_BORDER;
        table.AddCell(cell5);

        PdfPCell cell11 = new PdfPCell(new Phrase("Die Wichtigkeit ist dem Management bewusst, Infromationssicherheit wird allerdings nicht aktiv eingefordert."));
        cell11.Border = Rectangle.NO_BORDER;
        table.AddCell(cell11);

        // Checkbox 3: Nicht ausgefüllt
        PdfPCell cell8 = new PdfPCell();
        cell8.Border = Rectangle.NO_BORDER;
        cell8.CellEvent = new CheckboxCellEvent(false);
        table.AddCell(cell8);

        PdfPCell cell7 = new PdfPCell(new Phrase("Bewertung 3:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell7.Border = Rectangle.NO_BORDER;
        table.AddCell(cell7);

        PdfPCell cell12 = new PdfPCell(new Phrase("Das Thema Informationssicherheit wird vom Management eingefordert und uneingeschränkt unterstützt."));
        cell12.Border = Rectangle.NO_BORDER;
        table.AddCell(cell12);

        float xPosition = 50;
        float yPosition = 705;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

        //-------------------------- 2. Tabelle

        table = new PdfPTable(2);
        table.WidthPercentage = 100;
        table.DefaultCell.Border = Rectangle.NO_BORDER;
        float[] c = { 5f, 4f };
        table.SetWidths(c);
        // Erstellen des linken Headers
        PdfPCell header1 = new PdfPCell(new Phrase("BEGRÜNDUNG/UMGESETZTE MASSNAHMEN:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
        header1.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header1);

        // Erstellen des rechten Headers
        PdfPCell header2 = new PdfPCell(new Phrase("RISIKO/EMPFEHLUNG:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
        header2.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header2);

        // Einfügen des Textes in der linken Spalte
        cell1 = new PdfPCell(new Phrase("Das Management fordert Informationssicherheit nicht aktiv ein. Wird ein Bedarf gemeldet, werden Ressourcen jedoch freigegeben (z. B. Anschaffung von Hardware).", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell1.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund
        table.AddCell(cell1);

        // Einfügen des Textes in der rechten Spalte
        cell2 = new PdfPCell(new Phrase("Informationssicherheit sollte vom Management in einer für alle Mitarbeiter verbindlichen Richtlinie eingefordert werden.\n" +
            "Kritikalität: ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell2.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund


        table.AddCell(cell2);

        xPosition = 50;
        yPosition = 585;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

        AddSecondSecondDetailsTable();
    }

    private void AddSecondSecondDetailsTable()
    {
        var general = new Phrase();
        var header = new Chunk("1. UNTERSTÜTZT DAS MANAGEMENT DAS THEMA INFORMATIONSSICHERHEIT?", new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD, BaseColor.WHITE));
        header.SetBackground(new BaseColor(64, 64, 64));
        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 450, 0);

        // Erstellen einer Tabelle für die Checkboxen
        //for (int i = 0; i < 6; i++)
        //{
        //    document.Add(new Paragraph("\n"));
        //}


        PdfPTable table = new PdfPTable(3);
        table.DefaultCell.Border = Rectangle.NO_BORDER;

        float[] columnWidths = { 0.5f, 3f, 16f };
        table.SetWidths(columnWidths);



        // Checkbox 0: Ausgefüllt

        PdfPCell cell2 = new PdfPCell();
        cell2.Border = Rectangle.NO_BORDER;
        cell2.CellEvent = new CheckboxCellEvent(true);
        table.AddCell(cell2);

        PdfPCell cell1 = new PdfPCell(new Phrase("Bewertung 0:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell1.Border = Rectangle.NO_BORDER;
        table.AddCell(cell1);

        PdfPCell cell9 = new PdfPCell(new Phrase("Das Thema Informationssicherheit wird vom Management nicht getragen."));
        cell9.Border = Rectangle.NO_BORDER;
        table.AddCell(cell9);

        // Checkbox 1: Nicht ausgefüllt
        PdfPCell cell4 = new PdfPCell();
        cell4.Border = Rectangle.NO_BORDER;
        cell4.CellEvent = new CheckboxCellEvent(false);
        table.AddCell(cell4);

        PdfPCell cell3 = new PdfPCell(new Phrase("Bewertung 1:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell3.Border = Rectangle.NO_BORDER;
        table.AddCell(cell3);

        PdfPCell cell10 = new PdfPCell(new Phrase("Das Thema Informationssicherheit ist dem Management bekannt, wird aber nur eingeschränkt unterstützt."));
        cell10.Border = Rectangle.NO_BORDER;
        table.AddCell(cell10);

        // Checkbox 2: Nicht ausgefüllt
        PdfPCell cell6 = new PdfPCell();
        cell6.Border = Rectangle.NO_BORDER;
        cell6.CellEvent = new CheckboxCellEvent(false);
        table.AddCell(cell6);

        PdfPCell cell5 = new PdfPCell(new Phrase("Bewertung 2:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell5.Border = Rectangle.NO_BORDER;
        table.AddCell(cell5);

        PdfPCell cell11 = new PdfPCell(new Phrase("Die Wichtigkeit ist dem Management bewusst, Infromationssicherheit wird allerdings nicht aktiv eingefordert."));
        cell11.Border = Rectangle.NO_BORDER;
        table.AddCell(cell11);

        // Checkbox 3: Nicht ausgefüllt
        PdfPCell cell8 = new PdfPCell();
        cell8.Border = Rectangle.NO_BORDER;
        cell8.CellEvent = new CheckboxCellEvent(false);
        table.AddCell(cell8);

        PdfPCell cell7 = new PdfPCell(new Phrase("Bewertung 3:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
        cell7.Border = Rectangle.NO_BORDER;
        table.AddCell(cell7);

        PdfPCell cell12 = new PdfPCell(new Phrase("Das Thema Informationssicherheit wird vom Management eingefordert und uneingeschränkt unterstützt."));
        cell12.Border = Rectangle.NO_BORDER;
        table.AddCell(cell12);

        float xPosition = 50;
        float yPosition = 435;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

        //-------------------------- 2. Tabelle

        table = new PdfPTable(2);
        table.WidthPercentage = 100;
        table.DefaultCell.Border = Rectangle.NO_BORDER;
        float[] c = { 5f, 4f };
        table.SetWidths(c);

        // Erstellen des linken Headers
        PdfPCell header1 = new PdfPCell(new Phrase("BEGRÜNDUNG/UMGESETZTE MASSNAHMEN:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
        header1.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header1);

        // Erstellen des rechten Headers
        PdfPCell header2 = new PdfPCell(new Phrase("RISIKO/EMPFEHLUNG:", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE)));
        header2.BackgroundColor = new BaseColor(169, 169, 169); // Grauer Hintergrund
        table.AddCell(header2);

        // Einfügen des Textes in der linken Spalte
        cell1 = new PdfPCell(new Phrase("Das Management fordert Informationssicherheit nicht aktiv ein. Wird ein Bedarf gemeldet, werden Ressourcen jedoch freigegeben (z. B. Anschaffung von Hardware).", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell1.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund
        table.AddCell(cell1);

        // Einfügen des Textes in der rechten Spalte
        cell2 = new PdfPCell(new Phrase("Informationssicherheit sollte vom Management in einer für alle Mitarbeiter verbindlichen Richtlinie eingefordert werden.\n" +
            "Kritikalität: ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
        cell2.BackgroundColor = new BaseColor(220, 220, 220); // Hellgrauer Hintergrund


        table.AddCell(cell2);

        xPosition = 50;
        yPosition = 315;

        table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        table.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);

    }

    private void DiagramPage()
    {
        AddImgRightCorner();


        AddPoints();



        AddStandardFooter();
    }

}
