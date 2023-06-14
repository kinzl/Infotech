using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Questionnaire_Frontend.DownloadProjectReport
{
    public class CheckboxCellEvent : IPdfPCellEvent
    {
        private bool isChecked;

        public CheckboxCellEvent(bool isChecked)
        {
            this.isChecked = isChecked;
        }

        public void CellLayout(
            PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            // Positioniere die Checkbox in der Zelle
            float boxSize = 8;
            float x = position.Left;
            //float x = position.Left + 2;
            float y = position.Bottom + (position.Height - boxSize) - 5;
            //float y = position.Bottom + (position.Height - boxSize) / 2;

            // Zeichne das ausgefüllte Kästchen (Checkbox) nur für ausgewählte Boxen
            if (isChecked)
            {
                PdfContentByte canvas = canvases[PdfPTable.TEXTCANVAS];
                canvas.SetColorFill(BaseColor.BLACK);
                canvas.Rectangle(x, y, boxSize, boxSize);
                canvas.Fill();
            }

            // Zeichne den Rand der Checkbox
            PdfContentByte borderCanvas = canvases[PdfPTable.LINECANVAS];
            borderCanvas.SetColorStroke(BaseColor.BLACK);
            borderCanvas.SetLineWidth(1f);
            borderCanvas.Rectangle(x, y, boxSize, boxSize);
            borderCanvas.Stroke();
        }
    }
}
