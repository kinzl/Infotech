using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;

namespace CreatePDFReport;
public class PDFReport
{
    private iTextSharp.text.Document document;
    private PdfWriter writer;
    private int maxPageCount = 0;
    private SecurityCheckContext _db;
    private List<QuestionDto> AllQuestionsAndAnswers;
    public PDFReport(SecurityCheckContext db, List<QuestionDto> allQuestionsAndAnswers)
    {
        _db = db;
        AllQuestionsAndAnswers = allQuestionsAndAnswers;
    }
    public void CreatePDF()
    {
        FirstPage();

        AddNewEmptyPage();

        SecondPage();
        //AddTable();

        AddNewEmptyPage();

        ManagementSummaryPage();

        CloseDocument();
    }

    private void ManagementSummaryPage()
    {
        AddImgRightCorner();

        AddTextAbouveTable();

        AddMMSTable();

        AddStandardFooter();
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
        string[] themenbereiche = { "Organisation", "Nutzungsrichtlinie", "Geheimhaltung und Datenschutz", "Asset- und Risikomanagement", "Notfallmanagement", "Awareness", "Systembetrieb", "Netzwerk und Kommunikation", "Zutritts- und Zugriffsberechtigung" };
        Random random = new Random();
        foreach (string themenbereich in themenbereiche)
        {
            table.AddCell(themenbereich);
            table.AddCell(random.Next(1, 10).ToString()); // Fragen
            table.AddCell(random.Next(50, 100).ToString()); // Max. Punkte
            table.AddCell(random.Next(1, 50).ToString()); // Erreichte Punkte
            table.AddCell(random.Next(1, 100).ToString() + "%"); // Erreichte Punkte (%)
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

    private void AddTextAbouveTable()
    {
        Phrase general = new Phrase();
        Chunk header = new Chunk("MANAGEMENT SUMMARY", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD, BaseColor.RED));

        general.Add(header);
        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, general, 50, 720, 0);

        string managementSummaryText = @"Am 28.03.2023 wurde der Ist-Stand hinsichtlich Informationssicherheit und Datenschutz durch die Beantwortung von 98 Fragen erhoben. Die Erhebung der Informationen wurde im Zuge einer Befragung von Max Mustermann durch Hr. Martin Mallinger (Infotech) durchgeführt.";

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

    private void SecondPage()
    {
        AddImgRightCorner();

        AddSecondPageMiddleText();

        AddStandardFooter();
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
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(fullPath);
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

    private void AddSecondPageMiddleText()
    {
        string firstText = @"Dieses Dokument enthält vertrauliche Informationen und ist lediglich für die Geschäftsführung sowie die IT-Abteilung der Firma 'Musterfirma' vorgesehen. Eine Weitergabe an Dritte ist nicht gestattet und kann schwerwiegende Schäden für Musterfirma zur Folge haben.";

        string secondText = @"Der in diesem Dokument enthaltene Fragenkatalog wurde von Infotech - basierend auf branchenüblichen Standards - erstellt. Die Veröffentlichung, jede Art gewerblicher Nutzung sowie eine Weitergabe an Dritte ist nicht gestattet.";

        string thirdText = @"Die Fragen wurden im Zuge eines Interviews durch Musterfirma beantwortet. Die von Infotech empfohlenen Maßnahmen setzen die Richtigkeit und Vollständigkeit der Antworten voraus, da keine über ein Interview hinausgehenden Audits durchgeführt wurden.";

        string fourthText = @"Bei jeder Frage können 0 bis 3 Punkte erreicht werden, wobei 3 das Maximum ist. Wird eine Frage mit 0 Punkten beurteilt, bedeutet dies, dass das Thema im Unternehmen nicht behandelt wurde. Eine Beurteilung mit 1 Punkt bedeutet, dass bereits erste Maßnahmen umgesetzt bzw. die Anforderungen rudimentär erfüllt sind. Damit eine Frage mit 2 Punkten beurteilt wird, müssen die Anforderungen weitestgehend erfüllt sein, technische Maßnahmen weitestgehend dem Stand der Technik entsprechen und Prozesse grundlegend definiert und etabliert sein. Damit eine Frage mit 3 Punkten beurteilt wird, müssen technische Maßnahmen vollumfänglich umgesetzt und Prozesse definiert sein. Darüber hinaus muss die Wirksamkeit der technischen und organisatorischen Maßnahmen regelmäßig nachvollziehbar auditiert und verbessert werden.";

        string fithText = @"Es liegt in der Verantwortung von Musterfirma zu entscheiden, welche Handlungsempfehlungen in welcher Form umgesetzt werden.";

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
        int count = 9;//reader.NumberOfPages;
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
        Chunk chunk3 = new Chunk("Schärdinger Straße 35 | 4910 Ried i. I. | ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk4 = new Chunk("Telefon", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD));
        Chunk chunk5 = new Chunk(" 07752 81711 0 | ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk6 = new Chunk("E-Mail ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk7 = new Chunk("office@infotech.at", new Font(Font.FontFamily.HELVETICA, 10, Font.UNDERLINE, BaseColor.RED));
        chunk7.SetAnchor("mailto:office@infotech.at");
        Chunk chunk8 = new Chunk(" | ", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL));
        Chunk chunk9 = new Chunk("www.infotech.at", new Font(Font.FontFamily.HELVETICA, 10, Font.UNDERLINE, BaseColor.RED));
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
        leftCell1.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell1);

        PdfPCell rightCell1 = new PdfPCell(new Phrase("Interview / Selbstauskunft", normalFont));
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
        leftCell3.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
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
        leftCell5.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell5);
        PdfPCell rightCell3 = new PdfPCell(new Phrase("Max Mayr (IT-Verantwortlicher) Martin Mallinger (Infotech, Auditor)", normalFont));
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
        leftCell7.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell7);
        PdfPCell rightCell4 = new PdfPCell(new Phrase("Betrachtet wird die Informationssicherheit der Unternehmensgruppe. Es gibt keine Außenstandorte. Ca. 500 Mitarbeiter, davon 450 IT-Arbeitsplätze und ca. 300 bis 350 IT-Nutzer.", normalFont));
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
        leftCell9.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER; table.AddCell(leftCell9);

        PdfPCell rightCell5 = new PdfPCell(new Phrase("1.5", normalFont));
        table.AddCell(rightCell5);

        PdfPCell leftCell10 = new PdfPCell(new Phrase("Klassifizierung", normalFont));
        leftCell10.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell10.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell10.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell10.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell10);

        PdfPCell rightCell6 = new PdfPCell(new Phrase("Vertraulich", normalFont));
        table.AddCell(rightCell6);

        PdfPCell leftCell11 = new PdfPCell(new Phrase("Dokumentenverteiler", normalFont));
        leftCell11.BackgroundColor = new BaseColor(200, 200, 200);
        leftCell11.HorizontalAlignment = Element.ALIGN_LEFT;
        leftCell11.VerticalAlignment = Element.ALIGN_MIDDLE;
        leftCell11.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
        table.AddCell(leftCell11);

        PdfPCell rightCell7 = new PdfPCell(new Phrase("Max Mayr (IT-Verantwortlicher) Martin Mallinger (Infotech, Auditor)", normalFont));
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
        Chunk chunk1 = new Chunk("Firma", boldFont);
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
        iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance("C:\\Schule\\2022-23KoglerD190073\\SYP\\PDFReport\\CreatePDFReport\\CreatePDFReport\\Pictures\\infotech-logo-farbe.png");
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
}