Imports System.Drawing.Drawing2D

Public Class ChartControl
    Public PicOverlayHeight As Integer
    Public PicOverlayWidth As Integer
    Public XOverlayStartFraction As Double
    Public XOverlayEndFraction As Double
    Public YOverlayStartFraction As Double
    Public YOverlayEndFraction As Double
    Public OverlayPlotMax As Boolean
    Public OverlayFileCount As Integer = 0
    Public AnalyzedData(MAXDATAFILES, Main.LAST, Main.MAXDATAPOINTS) As Double
    Private Const MAXDATAFILES As Integer = 5
    Public xAxis As Double

    Public Sub DrawOverlay(OverlayBMP As Graphics, FileCount As Integer, AxisPen As Pen, AxisFont As Font, AxisBrush As SolidBrush, HeadingsFont As Font, Y4Font As Font, Y4Brush As SolidBrush, Y4Pen As Pen, ResultsFont As Font,
                            y4axis As Double, Y4Column As Integer, y4Max As Double(), TitleLine As Integer, UnitsLine As Integer, ResultsLine As Integer(), y4MaxAtX As Double(), y4MaxAtSelectedX As Double(),
                            OverlayDashes As DashStyle(), EqualSpacingCount As Integer, EqualSpacingPointers As Double(,), cmbOverlayDataY4SelectedIndex As Integer, cmbOverlayUnitsY4SelectedIndex As Integer,
                           TickLength As Integer, cmbOverlayDataXSelectedIndex As Integer, cmbOverlayUnitsXSelectedIndex As Integer)
        Dim TickInterval As Double, TempString As String

        With OverlayBMP

            TickInterval = PicOverlayHeight * (YOverlayEndFraction - YOverlayStartFraction) * 1 / 5
            Dim Counter As Integer
            For Counter = 0 To 4
                TempString = Main.NewCustomFormat((((y4axis) * Main.DataUnits(cmbOverlayDataY4SelectedIndex, cmbOverlayUnitsY4SelectedIndex)) / 5 * (5 - Counter)))
                .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)), CInt(PicOverlayWidth * XOverlayEndFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)))
                .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayEndFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter) - .MeasureString(TempString, AxisFont).Height / 2))
            Next
            TempString = Main.DataTags(cmbOverlayDataY4SelectedIndex) & vbCrLf & "(" & Split(Main.DataUnitTags(cmbOverlayDataY4SelectedIndex), " ")(cmbOverlayUnitsY4SelectedIndex) & ")"
            .DrawString(TempString, Y4Font, Y4Brush, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction - 5 - .MeasureString(TempString, Y4Font).Height)) ' * 1.5))
            'If OverlayPlotMax Then
            'TempString = "Max " & Main.DataTags(cmbOverlayDataY4SelectedIndex)
            '.DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
            'Else
            TempString = Main.DataTags(cmbOverlayDataY4SelectedIndex)
            .DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, TitleLine)
            'End If
            If OverlayPlotMax Then
                TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataY4SelectedIndex), " ")(cmbOverlayUnitsY4SelectedIndex) & ")"
            Else
                TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataY4SelectedIndex), " ")(cmbOverlayUnitsY4SelectedIndex) & ")"
            End If

            .DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
            For FileCount = 1 To OverlayFileCount
                If OverlayPlotMax Then
                    TempString = Main.NewCustomFormat(y4Max(FileCount) * Main.DataUnits(cmbOverlayDataY4SelectedIndex, cmbOverlayUnitsY4SelectedIndex)) & " @ " & Main.NewCustomFormat(y4MaxAtX(FileCount) * Main.DataUnits(cmbOverlayDataXSelectedIndex, cmbOverlayUnitsXSelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataXSelectedIndex), " ")(cmbOverlayUnitsXSelectedIndex)
                    .DrawString(TempString, ResultsFont, AxisBrush, Y4Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                Else
                    TempString = Main.NewCustomFormat(y4MaxAtSelectedX(FileCount) * Main.DataUnits(cmbOverlayDataY4SelectedIndex, cmbOverlayUnitsY4SelectedIndex)) ' & " @ " & Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataXSelectedIndex, cmbOverlayUnitsXSelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataXSelectedIndex), " ")(cmbOverlayUnitsXSelectedIndex)
                    .DrawString(TempString, ResultsFont, AxisBrush, Y4Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                End If

                Y4Pen.DashStyle = OverlayDashes(FileCount)
                For Counter = 2 To EqualSpacingCount - 1
                    .DrawLine(Y4Pen, CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataXSelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY4SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y4axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataXSelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY4SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y4axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight))
                Next
            Next
        End With

    End Sub

End Class
