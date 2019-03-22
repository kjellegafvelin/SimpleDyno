Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Collections.Generic
Imports LiveCharts
Imports LiveCharts.Wpf
Imports OxyPlot.Axes
Imports OxyPlot

Public Class AnalysisForm
    'Overlay Specific
    Private OverlayBMP As Graphics
    Private OverlayBitMap As Bitmap
    Private PicOverlayHeight As Integer
    Private PicOverlayWidth As Integer
    Private OverlayFileCount As Integer = 0

    Private dataFileReader As StreamReader
    Private OverlayFiles() As String
    'CHECK - THIS IS A LOCAL VALUE OF TICKLENGTH - MAY NEED TO RESCALE WITH RESIZE
    Private TickLength As Integer = 10
    Private PlotDrag As Boolean
    Private XOverlayStartFraction As Double
    Private XOverlayEndFraction As Double
    Private YOverlayStartFraction As Double
    Private YOverlayEndFraction As Double
    Dim xAxis As Double
    Private Const MAXDATAFILES As Integer = 5
    Private AnalyzedData(MAXDATAFILES, Main.LAST, Main.MAXDATAPOINTS) As Double
    Private chartControl As ChartControl
    Private OverlayXSelected As Double
    Private OverlayPlotMax As Boolean = True
    Private plotModel As OxyPlot.PlotModel
    Private dataRecordsList As List(Of List(Of DataRecord)) = New List(Of List(Of DataRecord))()

    Friend Sub Analysis_Setup()
        ReDim OverlayFiles(MAXDATAFILES)

        chartControl = New ChartControl()

        Me.Size = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size

        With pnlDataOverlay
            .Width = Me.ClientSize.Width - 165
            .Height = Me.ClientSize.Height - 5
            PicOverlayWidth = .Width
            PicOverlayHeight = .Height
        End With

        OverlayBitMap = New Bitmap(PicOverlayWidth, PicOverlayHeight)
        OverlayBMP = Graphics.FromImage(OverlayBitMap)

        Dim tempstring As String = ""
        Dim tempsplit2 As String()
        Dim paramcount As Integer
        For paramcount = 0 To Main.LAST - 1
            tempstring = tempstring & Main.DataTags(paramcount) & "_"
            'tempstring = tempstring & Main.DataTags(paramcount).Replace(" ", "_") & " "
        Next
        tempsplit2 = Split(tempstring, "_")

        ReDim Preserve tempsplit2(UBound(tempsplit2) - 1)
        'For paramcount = 0 To UBound(tempsplit2)
        ' tempsplit2(paramcount) = tempsplit2(paramcount).Replace("_", " ")
        ' Next
        cmbOverlayDataX.Items.AddRange(tempsplit2)
        tempsplit2 = Split(tempstring, "_")
        cmbOverlayDataY1.Items.AddRange(tempsplit2)
        cmbOverlayDataY2.Items.AddRange(tempsplit2)
        cmbOverlayDataY3.Items.AddRange(tempsplit2)
        cmbOverlayDataY4.Items.AddRange(tempsplit2)
        tempstring = ""
        tempsplit2 = Split(Main.DataUnitTags(Main.SPEED), " ")
        cmbOverlayCorrectedSpeedUnits.Items.AddRange(tempsplit2)
        cmbOverlayDataX.SelectedIndex = 0
        cmbOverlayDataY1.SelectedIndex = 0
        cmbOverlayDataY2.SelectedIndex = 0
        cmbOverlayDataY3.SelectedIndex = 0
        cmbOverlayDataY4.SelectedIndex = 0
        cmbOverlayCorrectedSpeedUnits.SelectedIndex = 0

        'pnlOverlaySetup()

        With chartControl
            .PicOverlayHeight = PicOverlayHeight
            .PicOverlayWidth = PicOverlayWidth
            .XOverlayStartFraction = XOverlayStartFraction
            .XOverlayEndFraction = XOverlayEndFraction
            .YOverlayStartFraction = YOverlayStartFraction
            .YOverlayEndFraction = YOverlayEndFraction
            .OverlayPlotMax = OverlayPlotMax
            .OverlayFileCount = OverlayFileCount
            .AnalyzedData = AnalyzedData
            .xAxis = xAxis
        End With


    End Sub
    Private Sub Analysis_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason <> CloseReason.FormOwnerClosing Then
            Me.Hide()
            e.Cancel = True
            Main.btnShow_Click(Me, System.EventArgs.Empty)
        End If
    End Sub
    Public Sub pnlOverlaySetup()
        'CHECK - THIS WHOLE SUB COULD DO WITH REWORKING 
        If Main.Formloaded Then
            Dim FileCount As Integer
            Dim Counter As Integer
            Dim TickInterval As Double, TempString As String

            'Define the "equal spacing" values - these allow the data to be plotted evenly across so that the line types can be seen clearly
            Dim EqualSpacingCount As Integer = 50
            Dim EqualSpacingInterval(MAXDATAFILES) As Double
            Dim EqualSpacingPointers(MAXDATAFILES, EqualSpacingCount) As Double

            'Find the correct font width for the columns
            Dim ColumnWidth As Integer = CInt(PicOverlayWidth / 8) '0.5 for legend, 1.5 for file, and 6 columns of data
            Dim Increment As Single = 0.1 'Rate at which we will increase the font size until we have one that fits
            Dim TempFont As New System.Drawing.Font("Arial", Increment) 'New temporary font for sizing
            Do Until OverlayBMP.MeasureString("Corr. Wheel Torque A", TempFont).Width > ColumnWidth 'test string is a little longer than the longest column header expected
                TempFont = New System.Drawing.Font("Arial", TempFont.Size + Increment) 'increase the size of the font
            Loop
            Dim ResultsFont As New Font("Arial", TempFont.Size) 'Font for results
            Dim HeadingsFont As New Font("Arial", TempFont.Size, FontStyle.Bold) 'Font for column headings
            Dim DashStyleFont As New Font("Arial", TempFont.Size, FontStyle.Italic) 'Font for legend
            Dim LineTypeColumn As Integer = 5 'sets small margin before legend text
            Dim FileColumn As Integer = LineTypeColumn + CInt(ColumnWidth / 2) '1/2 column width for legend
            Dim XColumn As Integer = FileColumn + CInt(ColumnWidth * 1.75) '1 1/2 column widths for filename 
            Dim Y1Column As Integer = XColumn + ColumnWidth
            Dim Y2Column As Integer = Y1Column + ColumnWidth
            Dim Y3Column As Integer = Y2Column + ColumnWidth
            Dim Y4Column As Integer = Y3Column + ColumnWidth
            Dim YDragColumn As Integer = Y4Column + ColumnWidth

            'Using the defined fonts find the correct positions for each line
            Dim Titleline As Integer = 5 'sets small top margin
            Dim UnitsLine As Integer = Titleline + CInt(TempFont.Height)
            Dim ResultsLine(MAXDATAFILES) As Integer
            ResultsLine(1) = UnitsLine + CInt(TempFont.Height)
            ResultsLine(2) = ResultsLine(1) + CInt(TempFont.Height)
            ResultsLine(3) = ResultsLine(2) + CInt(TempFont.Height)
            ResultsLine(4) = ResultsLine(3) + CInt(TempFont.Height)
            ResultsLine(5) = ResultsLine(4) + CInt(TempFont.Height)


            'CHECK - Ideally we should calculate the window size based on these new fonts and line and column definitions
            'However, seems to work fine for 900x600 and 1366x768 as of 01JUL13

            Dim xSet(Main.MAXDATAPOINTS) As Double, y1Set(Main.MAXDATAPOINTS) As Double, y2Set(Main.MAXDATAPOINTS) As Double, y3Set(Main.MAXDATAPOINTS) As Double, y4Set(Main.MAXDATAPOINTS) As Double
            Dim xMax(MAXDATAFILES) As Double, y1Max(MAXDATAFILES) As Double, y2Max(MAXDATAFILES) As Double, y3Max(MAXDATAFILES) As Double, y4Max(MAXDATAFILES) As Double, DragMax(MAXDATAFILES) As Double
            Dim y1MaxAtX(MAXDATAFILES) As Double, y2MaxAtX(MAXDATAFILES) As Double, y3MaxAtX(MAXDATAFILES) As Double, y4MaxAtX(MAXDATAFILES) As Double, DragMaxAtX(MAXDATAFILES) As Double
            Dim y1MaxAtSelectedX(MAXDATAFILES) As Double, y2MaxAtSelectedX(MAXDATAFILES) As Double, y3MaxAtSelectedX(MAXDATAFILES) As Double, y4MaxAtSelectedX(MAXDATAFILES) As Double, DragMaxAtSelectedX(MAXDATAFILES) As Double
            Dim XMaxDifferencePointer(MAXDATAFILES) As Long
            xAxis = 0
            Dim y1Axis As Double, y2Axis As Double, y3Axis As Double, y4Axis As Double

            Dim DragCompare As Double

            Dim AxisFont As New Font("Arial", 8)
            Dim AxisPen As New Pen(Color.Black, 1)
            Dim AxisBrush As New SolidBrush(AxisPen.Color)

            Dim XFont As New Font("Arial", 8)
            Dim XPen As New Pen(Color.Black, 2)
            Dim XBrush As New SolidBrush(XPen.Color)

            Dim Y1Font As New Font("Arial", 9)
            Dim Y1Pen As New Pen(Color.Blue, 2)
            Dim Y1Brush As New SolidBrush(Y1Pen.Color)

            Dim Y2Font As New Font("Arial", 9)
            Dim Y2Pen As New Pen(Color.Red, 2)
            Dim Y2Brush As New SolidBrush(Y2Pen.Color)

            Dim Y3Font As New Font("Arial", 9)
            Dim Y3Pen As New Pen(Color.Green, 2)
            Dim Y3Brush As New SolidBrush(Y3Pen.Color)

            Dim Y4Font As New Font("Arial", 9)
            Dim Y4Pen As New Pen(Color.Orange, 2)
            Dim Y4Brush As New SolidBrush(Y4Pen.Color)

            Dim DragFont As New Font("Arial", 8)
            Dim DragPen As New Pen(Color.Purple, 2)
            Dim DragBrush As New SolidBrush(DragPen.Color)

            Dim OverlayDashes(MAXDATAFILES) As DashStyle

            OverlayDashes(1) = DashStyle.Solid
            OverlayDashes(2) = DashStyle.Dash
            OverlayDashes(3) = DashStyle.Dot
            OverlayDashes(4) = DashStyle.DashDot
            OverlayDashes(5) = DashStyle.DashDotDot

            YOverlayStartFraction = 0.1 + OverlayFileCount * 0.0275
            YOverlayEndFraction = 0.925
            XOverlayStartFraction = 0.125
            XOverlayEndFraction = 0.875

            Dim XMaxDifference As Double


            For FileCount = 1 To OverlayFileCount
                XMaxDifference = 100000
                For Counter = 1 To CInt(AnalyzedData(FileCount, Main.SESSIONTIME, 0)) 'OvPoints(FileCount)
                    Dim currentX As Double = AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter)
                    Dim currentY1 As Double = AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, Counter)
                    Dim currentY2 As Double = AnalyzedData(FileCount, cmbOverlayDataY2.SelectedIndex, Counter)
                    Dim currentY3 As Double = AnalyzedData(FileCount, cmbOverlayDataY3.SelectedIndex, Counter)
                    Dim currentY4 As Double = AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, Counter)

                    If currentX > xMax(FileCount) Then
                        xMax(FileCount) = currentX
                        If xMax(FileCount) > xAxis Then
                            xAxis = xMax(FileCount)
                        End If
                    End If
                    'Check to see if we have passed the selected X value - this needs more work?
                    If Math.Abs(currentX - OverlayXSelected) < XMaxDifference Then
                        XMaxDifference = Math.Abs(currentX - OverlayXSelected)
                        XMaxDifferencePointer(FileCount) = Counter
                    End If

                    If currentY1 > y1Max(FileCount) Then
                        y1Max(FileCount) = currentY1
                        y1MaxAtX(FileCount) = currentX
                        If y1Max(FileCount) > y1Axis Then
                            y1Axis = y1Max(FileCount)
                        End If
                    End If

                    If currentY2 > y2Max(FileCount) Then
                        y2Max(FileCount) = currentY2
                        y2MaxAtX(FileCount) = currentX
                        If y2Max(FileCount) > y2Axis Then
                            y2Axis = y2Max(FileCount)
                        End If
                    End If

                    If currentY3 > y3Max(FileCount) Then
                        y3Max(FileCount) = currentY3
                        y3MaxAtX(FileCount) = currentX
                        If y3Max(FileCount) > y3Axis Then
                            y3Axis = y3Max(FileCount)
                        End If
                    End If

                    If currentY4 > y4Max(FileCount) Then
                        y4Max(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, Counter)
                        y4MaxAtX(FileCount) = currentX
                        If y4Max(FileCount) > y4Axis Then
                            y4Axis = y4Max(FileCount)
                        End If
                    End If
                Next
                y1MaxAtSelectedX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, CInt(XMaxDifferencePointer(FileCount)))
                y2MaxAtSelectedX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY2.SelectedIndex, CInt(XMaxDifferencePointer(FileCount)))
                y3MaxAtSelectedX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY3.SelectedIndex, CInt(XMaxDifferencePointer(FileCount)))
                y4MaxAtSelectedX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, CInt(XMaxDifferencePointer(FileCount)))
            Next

            Dim Interval As Integer
            'calculate the equal spacing pointer values
            'CHECK IF THIS SHOULD BE BEFORE OR AFTER THE CUSTOMROUND
            'CHECK  - this should have the last point and the first point
            For FileCount = 1 To OverlayFileCount
                EqualSpacingInterval(FileCount) = xMax(FileCount) / EqualSpacingCount
                Interval = 0
                For Counter = 1 To CInt(AnalyzedData(FileCount, Main.SESSIONTIME, 0))
                    If AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter) > Interval * EqualSpacingInterval(FileCount) Then
                        Interval += 1
                        If Interval <= EqualSpacingCount Then
                            EqualSpacingPointers(FileCount, Interval) = Counter
                        End If
                    End If
                Next
            Next

            'Set Axes to 1 if there are no data (i.e. the axis would be set to zero)
            If xAxis = 0 Then xAxis = 1
            If y1Axis = 0 Then y1Axis = 1
            If y2Axis = 0 Then y2Axis = 1
            If y3Axis = 0 Then y3Axis = 1
            If y4Axis = 0 Then y4Axis = 1

            'CHECK - THESE Might be better off using new custom round or format
            xAxis = Main.CustomRound(xAxis)
            y1Axis = Main.CustomRound(y1Axis)
            y2Axis = Main.CustomRound(y2Axis)
            y3Axis = Main.CustomRound(y3Axis)
            y4Axis = Main.CustomRound(y4Axis)

            'Okay - attempt to deal with the "Drag" questions
            'If drag is a selected Y value then if power is also a selected Y value, the Yaxes should be the same and the units should be the same
            'This is not going to work without arrays, so simply calculate the corrected speed and stick it in the corner if drag has been selected
            'is it worth setting up an array for this?

            With OverlayBMP
                .Clear(Color.White)
                'Draw Three Axes (Left, Right and Bottom)
                .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayStartFraction), CInt(PicOverlayHeight * YOverlayEndFraction), CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayEndFraction))
                .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayStartFraction), CInt(PicOverlayHeight * YOverlayStartFraction), CInt(PicOverlayWidth * XOverlayStartFraction), CInt(PicOverlayHeight * YOverlayEndFraction))
                .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction), CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayEndFraction))
                'X-Axis Details
                'Calculate the space between ticks
                TickInterval = PicOverlayWidth * (XOverlayEndFraction - XOverlayStartFraction) * 1 / 5
                For Counter = 0 To 4
                    'generate the tick label
                    TempString = Main.NewCustomFormat(((xAxis) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) / 5 * (Counter + 1))
                    'draw the tick
                    .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayStartFraction + (TickInterval * (Counter + 1))), CInt(PicOverlayHeight * YOverlayEndFraction), CInt(PicOverlayWidth * XOverlayStartFraction + (TickInterval * (Counter + 1))), CInt(PicOverlayHeight * YOverlayEndFraction + TickLength))
                    'draw the label
                    .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayStartFraction + (TickInterval * (Counter + 1)) - .MeasureString(TempString, AxisFont).Width / 2), CInt(PicOverlayHeight * YOverlayEndFraction + TickLength))
                Next
                'generate the axis title string...
                TempString = Main.DataTags(cmbOverlayDataX.SelectedIndex) & " (" & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex) & ")"
                '...and draw.
                .DrawString(TempString, HeadingsFont, AxisBrush, CInt(PicOverlayWidth / 2 - .MeasureString(TempString, HeadingsFont).Width / 2), CInt(PicOverlayHeight * YOverlayEndFraction) + TickLength + .MeasureString("9", AxisFont).Height)
                'prefix the string with "Max" for the results at the top of the page...
                'Version 6.4 moving the Max prefix to the units line to avoid overcrowding
                'If OverlayPlotMax Then
                'TempString = "Max " & Main.DataTags(cmbOverlayDataX.SelectedIndex)
                'Else
                TempString = Main.DataTags(cmbOverlayDataX.SelectedIndex)
                'CHECK FOR REMOVAL Debug.Print(OverlayXSelected & " " & xAxis)
                .DrawLine(Pens.Gray, CInt(XOverlayStartFraction * PicOverlayWidth + (OverlayXSelected / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayStartFraction * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + (OverlayXSelected / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight))
                'End If
                'and draw the column heading
                .DrawString(TempString, HeadingsFont, AxisBrush, XColumn - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                'generate the units string
                If OverlayPlotMax Then
                    TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex) & ")"
                Else
                    TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex) & ")"
                End If
                'TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex) & ")"
                'and draw the units heading
                .DrawString(TempString, HeadingsFont, AxisBrush, XColumn - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                'Write the filenames and the X max results
                For FileCount = 1 To OverlayFileCount
                    .DrawString(OverlayDashes(FileCount).ToString, DashStyleFont, AxisBrush, 0, ResultsLine(FileCount))
                    '.DrawString(OverlayFiles(FileCount), ResultsFont, AxisBrush, FileColumn - .MeasureString(OverlayFiles(FileCount), ResultsFont).Width, ResultsLine(FileCount))
                    'This should draw left justified string for the file name title
                    .DrawString(OverlayFiles(FileCount), ResultsFont, AxisBrush, FileColumn, ResultsLine(FileCount))
                    If OverlayPlotMax Then
                        TempString = Main.NewCustomFormat(xMax(FileCount) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex))
                        .DrawString(TempString, ResultsFont, AxisBrush, XColumn - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                    Else
                        TempString = Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex))
                        .DrawString(TempString, ResultsFont, AxisBrush, XColumn - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                    End If
                Next

                'Same pattern used for X ticks for each of the Y axes / Ticks / Results provided the specific Y column has been selected
                If cmbOverlayDataY1.SelectedIndex <> Main.LAST Then
                    TickInterval = PicOverlayHeight * (YOverlayEndFraction - YOverlayStartFraction) * 1 / 5
                    For Counter = 0 To 4
                        TempString = Main.NewCustomFormat((((y1Axis) * Main.DataUnits(cmbOverlayDataY1.SelectedIndex, cmbOverlayUnitsY1.SelectedIndex)) / 5 * (5 - Counter)))
                        .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayStartFraction - TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)), CInt(PicOverlayWidth * XOverlayStartFraction), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)))
                        .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayStartFraction - TickLength - .MeasureString(TempString, AxisFont).Width), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter) - .MeasureString(TempString, AxisFont).Height / 2))
                    Next
                    TempString = Main.DataTags(cmbOverlayDataY1.SelectedIndex) & vbCrLf & "(" & Split(Main.DataUnitTags(cmbOverlayDataY1.SelectedIndex), " ")(cmbOverlayUnitsY1.SelectedIndex) & ")"
                    .DrawString(TempString, Y1Font, Y1Brush, CInt(PicOverlayWidth * XOverlayStartFraction - .MeasureString(TempString, Y1Font).Width), CInt(PicOverlayHeight * YOverlayStartFraction - 5 - .MeasureString(TempString, Y1Font).Height)) ' * 1.5))
                    'If OverlayPlotMax Then
                    'TempString = "Max " & Main.DataTags(cmbOverlayDataY1.SelectedIndex)
                    '.DrawString(TempString, HeadingsFont, AxisBrush, Y1Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'Else
                    TempString = Main.DataTags(cmbOverlayDataY1.SelectedIndex)
                    .DrawString(TempString, HeadingsFont, AxisBrush, Y1Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'End If
                    If OverlayPlotMax Then
                        TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataY1.SelectedIndex), " ")(cmbOverlayUnitsY1.SelectedIndex) & ")"
                    Else
                        TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataY1.SelectedIndex), " ")(cmbOverlayUnitsY1.SelectedIndex) & ")"
                    End If
                    .DrawString(TempString, HeadingsFont, AxisBrush, Y1Column - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                    For FileCount = 1 To OverlayFileCount
                        If OverlayPlotMax Then
                            TempString = Main.NewCustomFormat(y1Max(FileCount) * Main.DataUnits(cmbOverlayDataY1.SelectedIndex, cmbOverlayUnitsY1.SelectedIndex)) & " @ " & Main.NewCustomFormat(y1MaxAtX(FileCount) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y1Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        Else
                            TempString = Main.NewCustomFormat(y1MaxAtSelectedX(FileCount) * Main.DataUnits(cmbOverlayDataY1.SelectedIndex, cmbOverlayUnitsY1.SelectedIndex)) ' & " @ " & Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y1Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        End If

                        Y1Pen.DashStyle = OverlayDashes(FileCount)
                        For Counter = 2 To EqualSpacingCount - 1
                            Dim x1 As Integer = CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth)
                            Dim x2 As Integer = CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth)

                            Dim y1 As Integer = CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y1Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight)
                            Dim y2 As Integer = CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y1Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight)

                            .DrawLine(Y1Pen, x1, y1, x2, y2)
                        Next
                    Next
                End If

                If cmbOverlayDataY2.SelectedIndex <> Main.LAST Then
                    TickInterval = PicOverlayHeight * (YOverlayEndFraction - YOverlayStartFraction) * 1 / 5
                    For Counter = 0 To 4
                        TempString = Main.NewCustomFormat((((y2Axis) * Main.DataUnits(cmbOverlayDataY2.SelectedIndex, cmbOverlayUnitsY2.SelectedIndex)) / 5 * (5 - Counter)))
                        .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayStartFraction), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)), CInt(PicOverlayWidth * XOverlayStartFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)))
                        .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayStartFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter) - .MeasureString(TempString, AxisFont).Height / 2))
                    Next
                    TempString = Main.DataTags(cmbOverlayDataY2.SelectedIndex) & vbCrLf & "(" & Split(Main.DataUnitTags(cmbOverlayDataY2.SelectedIndex), " ")(cmbOverlayUnitsY2.SelectedIndex) & ")"
                    .DrawString(TempString, Y2Font, Y2Brush, CInt(PicOverlayWidth * XOverlayStartFraction), CInt(PicOverlayHeight * YOverlayStartFraction - 5 - .MeasureString(TempString, Y2Font).Height)) ' * 1.5))
                    'If OverlayPlotMax Then
                    'TempString = "Max " & Main.DataTags(cmbOverlayDataY2.SelectedIndex)
                    '.DrawString(TempString, HeadingsFont, AxisBrush, Y2Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'Else
                    TempString = Main.DataTags(cmbOverlayDataY2.SelectedIndex)
                    .DrawString(TempString, HeadingsFont, AxisBrush, Y2Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'End If
                    If OverlayPlotMax Then
                        TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataY2.SelectedIndex), " ")(cmbOverlayUnitsY2.SelectedIndex) & ")"
                    Else
                        TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataY2.SelectedIndex), " ")(cmbOverlayUnitsY2.SelectedIndex) & ")"
                    End If

                    .DrawString(TempString, HeadingsFont, AxisBrush, Y2Column - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                    For FileCount = 1 To OverlayFileCount
                        If OverlayPlotMax Then
                            TempString = Main.NewCustomFormat(y2Max(FileCount) * Main.DataUnits(cmbOverlayDataY2.SelectedIndex, cmbOverlayUnitsY2.SelectedIndex)) & " @ " & Main.NewCustomFormat(y2MaxAtX(FileCount) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y2Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        Else
                            TempString = Main.NewCustomFormat(y2MaxAtSelectedX(FileCount) * Main.DataUnits(cmbOverlayDataY2.SelectedIndex, cmbOverlayUnitsY2.SelectedIndex)) ' & " @ " & Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y2Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        End If
                        Y2Pen.DashStyle = OverlayDashes(FileCount)
                        For Counter = 2 To EqualSpacingCount - 1
                            .DrawLine(Y2Pen, CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY2.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y2Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY2.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y2Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight))
                        Next
                    Next
                End If

                If cmbOverlayDataY3.SelectedIndex <> Main.LAST Then
                    TickInterval = PicOverlayHeight * (YOverlayEndFraction - YOverlayStartFraction) * 1 / 5
                    For Counter = 0 To 4
                        TempString = Main.NewCustomFormat((((y3Axis) * Main.DataUnits(cmbOverlayDataY3.SelectedIndex, cmbOverlayUnitsY3.SelectedIndex)) / 5 * (5 - Counter)))
                        .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayEndFraction - TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)), CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)))
                        .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayEndFraction - TickLength - .MeasureString(TempString, AxisFont).Width), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter) - .MeasureString(TempString, AxisFont).Height / 2))
                    Next
                    TempString = Main.DataTags(cmbOverlayDataY3.SelectedIndex) & vbCrLf & "(" & Split(Main.DataUnitTags(cmbOverlayDataY3.SelectedIndex), " ")(cmbOverlayUnitsY3.SelectedIndex) & ")"
                    .DrawString(TempString, Y3Font, Y3Brush, CInt(PicOverlayWidth * XOverlayEndFraction - .MeasureString(TempString, Y3Font).Width), CInt(PicOverlayHeight * YOverlayStartFraction - 5 - .MeasureString(TempString, Y3Font).Height)) '* 1.5))
                    'If OverlayPlotMax Then
                    'TempString = "Max " & Main.DataTags(cmbOverlayDataY3.SelectedIndex)
                    '.DrawString(TempString, HeadingsFont, AxisBrush, Y3Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'Else
                    TempString = Main.DataTags(cmbOverlayDataY3.SelectedIndex)
                    .DrawString(TempString, HeadingsFont, AxisBrush, Y3Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'End If
                    If OverlayPlotMax Then
                        TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataY3.SelectedIndex), " ")(cmbOverlayUnitsY3.SelectedIndex) & ")"
                    Else
                        TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataY3.SelectedIndex), " ")(cmbOverlayUnitsY3.SelectedIndex) & ")"
                    End If

                    .DrawString(TempString, HeadingsFont, AxisBrush, Y3Column - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                    For FileCount = 1 To OverlayFileCount
                        If OverlayPlotMax Then
                            TempString = Main.NewCustomFormat(y3Max(FileCount) * Main.DataUnits(cmbOverlayDataY3.SelectedIndex, cmbOverlayUnitsY3.SelectedIndex)) & " @ " & Main.NewCustomFormat(y3MaxAtX(FileCount) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y3Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        Else
                            TempString = Main.NewCustomFormat(y3MaxAtSelectedX(FileCount) * Main.DataUnits(cmbOverlayDataY3.SelectedIndex, cmbOverlayUnitsY3.SelectedIndex)) ' & " @ " & Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y3Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        End If

                        Y3Pen.DashStyle = OverlayDashes(FileCount)
                        For Counter = 2 To EqualSpacingCount - 1
                            .DrawLine(Y3Pen, CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY3.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y3Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY3.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y3Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight))
                        Next
                    Next
                End If

                If cmbOverlayDataY4.SelectedIndex <> Main.LAST Then

                    chartControl.DrawOverlay(OverlayBMP, FileCount, AxisPen, AxisFont, AxisBrush, HeadingsFont, Y4Font, Y4Brush, Y4Pen, ResultsFont, y4Axis, Y4Column, y4Max, Titleline, UnitsLine,
                                ResultsLine, y4MaxAtX, y4MaxAtSelectedX, OverlayDashes, EqualSpacingCount, EqualSpacingPointers, cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex,
                                TickLength, cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)


                    'TickInterval = PicOverlayHeight * (YOverlayEndFraction - YOverlayStartFraction) * 1 / 5
                    'For Counter = 0 To 4
                    '    TempString = Main.NewCustomFormat((((y4Axis) * Main.DataUnits(cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex)) / 5 * (5 - Counter)))
                    '    .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)), CInt(PicOverlayWidth * XOverlayEndFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)))
                    '    .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayEndFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter) - .MeasureString(TempString, AxisFont).Height / 2))
                    'Next
                    'TempString = Main.DataTags(cmbOverlayDataY4.SelectedIndex) & vbCrLf & "(" & Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex), " ")(cmbOverlayUnitsY4.SelectedIndex) & ")"
                    '.DrawString(TempString, Y4Font, Y4Brush, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction - 5 - .MeasureString(TempString, Y4Font).Height)) ' * 1.5))
                    ''If OverlayPlotMax Then
                    ''TempString = "Max " & Main.DataTags(cmbOverlayDataY4.SelectedIndex)
                    ''.DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    ''Else
                    'TempString = Main.DataTags(cmbOverlayDataY4.SelectedIndex)
                    '.DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    ''End If
                    'If OverlayPlotMax Then
                    '    TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex), " ")(cmbOverlayUnitsY4.SelectedIndex) & ")"
                    'Else
                    '    TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex), " ")(cmbOverlayUnitsY4.SelectedIndex) & ")"
                    'End If

                    '.DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                    'For FileCount = 1 To OverlayFileCount
                    '    If OverlayPlotMax Then
                    '        TempString = Main.NewCustomFormat(y4Max(FileCount) * Main.DataUnits(cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex)) & " @ " & Main.NewCustomFormat(y4MaxAtX(FileCount) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                    '        .DrawString(TempString, ResultsFont, AxisBrush, Y4Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                    '    Else
                    '        TempString = Main.NewCustomFormat(y4MaxAtSelectedX(FileCount) * Main.DataUnits(cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex)) ' & " @ " & Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                    '        .DrawString(TempString, ResultsFont, AxisBrush, Y4Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                    '    End If

                    '    Y4Pen.DashStyle = OverlayDashes(FileCount)
                    '    For Counter = 2 To EqualSpacingCount - 1
                    '        .DrawLine(Y4Pen, CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y4Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y4Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight))
                    '    Next
                    'Next
                End If

                'Hack Job for corrected speed
                TempString = "Max Corr. Speed" ' & DataTags(DRAG)
                .DrawString(TempString, HeadingsFont, AxisBrush, YDragColumn - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                TempString = "(" & Split(Main.DataUnitTags(Main.SPEED), " ")(cmbOverlayCorrectedSpeedUnits.SelectedIndex) & ")"
                .DrawString(TempString, HeadingsFont, AxisBrush, YDragColumn - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                For FileCount = 1 To OverlayFileCount
                    DragCompare = Double.MaxValue
                    Counter = 2
                    Do
                        Counter += 1
                    Loop Until AnalyzedData(FileCount, Main.POWER, Counter) - AnalyzedData(FileCount, Main.DRAG, Counter) < 0 Or Counter = AnalyzedData(FileCount, Main.SESSIONTIME, 0)
                    TempString = Main.NewCustomFormat(AnalyzedData(FileCount, Main.SPEED, Counter) * Main.DataUnits(Main.SPEED, cmbOverlayCorrectedSpeedUnits.SelectedIndex))  '& " @ " & NewCustomFormat(y4MaxAtX(FileCount) * DataUnits(cmbOverlayX.SelectedIndex, cmbOverlayXUnits.SelectedIndex)) & " " & Split(DataUnitTags(cmbOverlayX.SelectedIndex), " ")(cmbOverlayXUnits.SelectedIndex)
                    .DrawString(TempString, ResultsFont, AxisBrush, YDragColumn - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                Next

            End With

            pnlDataOverlay.BackgroundImage = OverlayBitMap
            pnlDataOverlay.Invalidate()

        End If
    End Sub


    Friend Sub btnAddOverlayFile_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddOverlayFile.Click

        ' Dirty trick here to open file from the Fit form
        If Not e.Equals(System.EventArgs.Empty) Then
            With OpenFileDialog1
                .Reset()
                .Filter = "Power Run files v6.3+ (*.sdp)|*.sdp|Power Run Files v5.5+ (*.txt)|*.txt"
                .Multiselect = True
                If .ShowDialog() <> DialogResult.OK Then
                    Return
                End If
            End With
        End If

        Dim dataInputFileReader As DataInputFileReader = New DataInputFileReader()

        Dim index As Integer
        For index = 0 To OpenFileDialog1.FileNames.Length - 1

            Dim fileName As String = OpenFileDialog1.FileNames.GetValue(index).ToString()

            Main.SetMouseBusy_ThreadSafe(Me)

            Dim dataRecords As List(Of DataRecord)

            Try
                With dataInputFileReader
                    '.ReadDataFile(fileName)
                    dataRecords = .ReadDataFile2(fileName)
                    OverlayFileCount = .OverlayFileCount
                    OverlayFiles = .OverlayFiles
                    AnalyzedData = .AnalyzedData
                End With
            Catch ex As FileFormatException
                MessageBox.Show("The file '" & fileName & "' has an invalid file format. Please try another file!", "File error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Main.SetMouseNormal_ThreadSafe(Me)
                Return
            Catch
                MessageBox.Show("Could not open the file '" & fileName & "'!", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Main.SetMouseNormal_ThreadSafe(Me)
                Return
            End Try

            clbFiles.Items.Add(Path.GetFileNameWithoutExtension(fileName), clbFiles.CheckedItems.Count < 5)
            Me.dataRecordsList.Add(dataRecords)
        Next

        SetupDiagram()

        'btnAddOverlayFile.Enabled = dataRecordsList.Count < 5

        Main.SetMouseNormal_ThreadSafe(Me)

        'pnlOverlaySetup()
    End Sub

    Private Sub SetupDiagram()

        Me.plotModel = New OxyPlot.PlotModel() With {
            .Background = OxyColors.White,
            .AxisTierDistance = 0,
            .PlotMargins = New OxyThickness(100, 0, 100, 50),
            .IsLegendVisible = True
        }

        Me.PlotView1.Model = Me.plotModel

        Dim i As Integer
        Dim xIndex As Integer = cmbOverlayDataX.SelectedIndex
        Dim xUnitsIndex As Integer = cmbOverlayUnitsX.SelectedIndex

        Dim xAxisUnit As String = Main.DataUnitTags(xIndex).Split(CType(" ", Char()))(xUnitsIndex)
        Dim xAxisTitle As String = Main.DataTags(xIndex)
        lblXTitle.Text = xAxisTitle
        lblXUnit.Text = "Max (" & xAxisUnit & ")"

        Dim y1Index As Integer
        Dim y1UnitsIndex As Integer
        Dim y1AxisUnit As String
        Dim y1AxisTitle As String

        Dim y2Index As Integer
        Dim y2UnitsIndex As Integer
        Dim y2AxisUnit As String
        Dim y2AxisTitle As String

        Dim y3Index As Integer
        Dim y3UnitsIndex As Integer
        Dim y3AxisUnit As String
        Dim y3AxisTitle As String

        Dim y4Index As Integer
        Dim y4UnitsIndex As Integer
        Dim y4AxisUnit As String
        Dim y4AxisTitle As String

        Me.SuspendLayout()

        y1Index = Math.Max(cmbOverlayDataY1.SelectedIndex, 0)
        y2Index = Math.Max(cmbOverlayDataY2.SelectedIndex, 0)
        y3Index = Math.Max(cmbOverlayDataY3.SelectedIndex, 0)
        y4Index = Math.Max(cmbOverlayDataY4.SelectedIndex, 0)

        lblFile1.Visible = clbFiles.CheckedItems.Count > 0
        lblFile2.Visible = clbFiles.CheckedItems.Count > 1
        lblFile3.Visible = clbFiles.CheckedItems.Count > 2
        lblFile4.Visible = clbFiles.CheckedItems.Count > 3
        lblFile5.Visible = clbFiles.CheckedItems.Count > 4
        lblXMax1.Visible = clbFiles.CheckedItems.Count > 0
        lblXMax2.Visible = clbFiles.CheckedItems.Count > 1
        lblXMax3.Visible = clbFiles.CheckedItems.Count > 2
        lblXMax4.Visible = clbFiles.CheckedItems.Count > 3
        lblXMax5.Visible = clbFiles.CheckedItems.Count > 4

        lblY1Title.Visible = y1Index < Main.LAST
        lblY1Unit.Visible = y1Index < Main.LAST
        lblY1Max1.Visible = clbFiles.CheckedItems.Count > 0 And y1Index < Main.LAST
        lblY1Max2.Visible = clbFiles.CheckedItems.Count > 1 And y1Index < Main.LAST
        lblY1Max3.Visible = clbFiles.CheckedItems.Count > 2 And y1Index < Main.LAST
        lblY1Max4.Visible = clbFiles.CheckedItems.Count > 3 And y1Index < Main.LAST
        lblY1Max5.Visible = clbFiles.CheckedItems.Count > 4 And y1Index < Main.LAST

        lblY2Title.Visible = y2Index < Main.LAST
        lblY2Unit.Visible = y2Index < Main.LAST
        lblY2Max1.Visible = clbFiles.CheckedItems.Count > 0 And y2Index < Main.LAST
        lblY2Max2.Visible = clbFiles.CheckedItems.Count > 1 And y2Index < Main.LAST
        lblY2Max3.Visible = clbFiles.CheckedItems.Count > 2 And y2Index < Main.LAST
        lblY2Max4.Visible = clbFiles.CheckedItems.Count > 3 And y2Index < Main.LAST
        lblY2Max5.Visible = clbFiles.CheckedItems.Count > 4 And y2Index < Main.LAST

        lblY3Title.Visible = y3Index < Main.LAST
        lblY3Unit.Visible = y3Index < Main.LAST
        lblY3Max1.Visible = clbFiles.CheckedItems.Count > 0 And y3Index < Main.LAST
        lblY3Max2.Visible = clbFiles.CheckedItems.Count > 1 And y3Index < Main.LAST
        lblY3Max3.Visible = clbFiles.CheckedItems.Count > 2 And y3Index < Main.LAST
        lblY3Max4.Visible = clbFiles.CheckedItems.Count > 3 And y3Index < Main.LAST
        lblY3Max5.Visible = clbFiles.CheckedItems.Count > 4 And y3Index < Main.LAST

        lblY4Title.Visible = y4Index < Main.LAST
        lblY4Unit.Visible = y4Index < Main.LAST
        lblY4Max1.Visible = clbFiles.CheckedItems.Count > 0 And y4Index < Main.LAST
        lblY4Max2.Visible = clbFiles.CheckedItems.Count > 1 And y4Index < Main.LAST
        lblY4Max3.Visible = clbFiles.CheckedItems.Count > 2 And y4Index < Main.LAST
        lblY4Max4.Visible = clbFiles.CheckedItems.Count > 3 And y4Index < Main.LAST
        lblY4Max5.Visible = clbFiles.CheckedItems.Count > 4 And y4Index < Main.LAST

        y1UnitsIndex = Math.Max(cmbOverlayUnitsY1.SelectedIndex, 0)
        If (y1Index < Main.LAST) Then
            y1AxisUnit = Main.DataUnitTags(y1Index).Split(CType(" ", Char()))(y1UnitsIndex)
            y1AxisTitle = Main.DataTags(y1Index)
            lblY1Title.Text = y1AxisTitle
            lblY1Unit.Text = "Max (" & y1AxisUnit & ")"
        End If

        y2UnitsIndex = Math.Max(cmbOverlayUnitsY2.SelectedIndex, 0)
        If (y2Index < Main.LAST) Then
            y2AxisUnit = Main.DataUnitTags(y2Index).Split(CType(" ", Char()))(y2UnitsIndex)
            y2AxisTitle = Main.DataTags(y2Index)
            lblY2Title.Text = y2AxisTitle
            lblY2Unit.Text = "Max (" & y2AxisUnit & ")"
        End If

        y3UnitsIndex = Math.Max(cmbOverlayUnitsY3.SelectedIndex, 0)
        If (y3Index < Main.LAST) Then
            y3AxisUnit = Main.DataUnitTags(y3Index).Split(CType(" ", Char()))(y3UnitsIndex)
            y3AxisTitle = Main.DataTags(y3Index)
            lblY3Title.Text = y3AxisTitle
            lblY3Unit.Text = "Max (" & y3AxisUnit & ")"
        End If

        y4UnitsIndex = Math.Max(cmbOverlayUnitsY4.SelectedIndex, 0)
        If (y4Index < Main.LAST) Then
            y4AxisUnit = Main.DataUnitTags(y4Index).Split(CType(" ", Char()))(y4UnitsIndex)
            y4AxisTitle = Main.DataTags(y4Index)
            lblY4Title.Text = y4AxisTitle
            lblY4Unit.Text = "Max (" & y4AxisUnit & ")"
        End If

        plotModel.Axes.Add(New LinearAxis() With {
                           .Key = "x",
                           .Position = OxyPlot.Axes.AxisPosition.Bottom,
                           .Title = xAxisTitle,
                           .MajorGridlineStyle = LineStyle.Solid,
                           .Unit = xAxisUnit
                           })

        If (y1Index < Main.LAST) Then
            plotModel.Axes.Add(New LinearAxis() With {
                                .Key = "y1",
                                .Position = OxyPlot.Axes.AxisPosition.Left,
                                .Title = y1AxisTitle,
                                .MajorGridlineStyle = LineStyle.Solid,
                                .MajorStep = 1, .Unit = y1AxisUnit
                                })
        End If

        If (y2Index < Main.LAST) Then
            plotModel.Axes.Add(New LinearAxis() With {
                               .Key = "y2",
                               .Position = OxyPlot.Axes.AxisPosition.Right,
                               .Title = y2AxisTitle,
                               .MajorGridlineStyle = LineStyle.Dash,
                               .MajorStep = 1, .Unit = y2AxisUnit
                               })
        End If

        If (y3Index < Main.LAST) Then
            plotModel.Axes.Add(New LinearAxis() With {
                           .Key = "y3",
                           .AxisDistance = 50,
                           .AxisTitleDistance = -46,
                           .Position = OxyPlot.Axes.AxisPosition.Left,
                           .Title = y3AxisTitle,
                           .MajorGridlineStyle = LineStyle.Dash,
                           .MajorStep = 1, .Unit = y3AxisUnit
                           })
        End If

        If (y4Index < Main.LAST) Then
            plotModel.Axes.Add(New LinearAxis() With {
                           .Key = "y4",
                           .AxisDistance = 50,
                           .AxisTitleDistance = -46,
                           .Position = OxyPlot.Axes.AxisPosition.Right,
                           .Title = y4AxisTitle,
                           .MajorGridlineStyle = LineStyle.Dash,
                           .MajorStep = 1, .Unit = y4AxisUnit
                           })
        End If

        Dim lineStyles As LineStyle() = {LineStyle.Solid, LineStyle.Dash, LineStyle.LongDash, LineStyle.DashDot, LineStyle.LongDashDot}


        Dim row As Integer = 0
        For i = 0 To dataRecordsList.Count - 1

            If (clbFiles.CheckedIndices.Contains(i) = False) Then
                Continue For
            End If

            Dim lineSeries1 As OxyPlot.Series.LineSeries
            If (y1Index < Main.LAST) Then

                lineSeries1 = New OxyPlot.Series.LineSeries With {
                    .YAxisKey = "y1",
                    .LineStyle = lineStyles(row),
                    .Color = OxyColors.Blue,
                    .Title = y1AxisTitle
                }
                plotModel.Series.Add(lineSeries1)
            End If

            Dim lineSeries2 As OxyPlot.Series.LineSeries
            If (y2Index < Main.LAST) Then
                lineSeries2 = New OxyPlot.Series.LineSeries With {
                    .YAxisKey = "y2",
                    .LineStyle = lineStyles(row),
                    .Color = OxyColors.Red,
                    .Title = y2AxisTitle
                }
                plotModel.Series.Add(lineSeries2)
            End If

            Dim lineSeries3 As OxyPlot.Series.LineSeries
            If (y3Index < Main.LAST) Then

                lineSeries3 = New OxyPlot.Series.LineSeries With {
                    .YAxisKey = "y3",
                    .LineStyle = lineStyles(row),
                    .Color = OxyColors.Green,
                    .Title = y3AxisTitle
                }

                plotModel.Series.Add(lineSeries3)
            End If

            Dim lineSeries4 As OxyPlot.Series.LineSeries
            If (y4Index < Main.LAST) Then
                lineSeries4 = New OxyPlot.Series.LineSeries With {
                    .YAxisKey = "y4",
                    .LineStyle = lineStyles(row),
                    .Color = OxyColors.Yellow,
                    .Title = y4AxisTitle
                }

                plotModel.Series.Add(lineSeries4)
            End If

            Dim x1Max As Double = 0
            Dim y1Max As Double = 0
            Dim y1MaxX As Double = 0
            Dim y2Max As Double = 0
            Dim y2MaxX As Double = 0
            Dim y3Max As Double = 0
            Dim y3MaxX As Double = 0
            Dim y4Max As Double = 0
            Dim y4MaxX As Double = 0

            For Each dataRecord As DataRecord In dataRecordsList(i)
                Dim xValue As Double = Main.DataActions(xIndex)(dataRecord) * Main.DataUnits(xIndex, xUnitsIndex)
                Dim y1Value As Double = 0
                Dim y2Value As Double = 0
                Dim y3Value As Double = 0
                Dim y4Value As Double = 0


                x1Max = Math.Max(x1Max, xValue)

                If (y1Index < Main.LAST) Then
                    y1Value = Main.DataActions(y1Index)(dataRecord) * Main.DataUnits(y1Index, y1UnitsIndex)
                    lineSeries1.Points.Add(New OxyPlot.DataPoint(xValue, y1Value))
                    If (y1Value > y1Max) Then
                        y1Max = y1Value
                        y1MaxX = xValue
                    End If
                End If

                If (y2Index < Main.LAST) Then
                    y2Value = Main.DataActions(y2Index)(dataRecord) * Main.DataUnits(y2Index, y2UnitsIndex)
                    lineSeries2.Points.Add(New OxyPlot.DataPoint(xValue, y2Value))
                    If (y2Value > y2Max) Then
                        y2Max = y2Value
                        y2MaxX = xValue
                    End If
                End If

                If (y3Index < Main.LAST) Then
                    y3Value = Main.DataActions(y3Index)(dataRecord) * Main.DataUnits(y3Index, y3UnitsIndex)
                    lineSeries3.Points.Add(New OxyPlot.DataPoint(xValue, y3Value))
                    If (y3Value > y3Max) Then
                        y3Max = y3Value
                        y3MaxX = xValue
                    End If
                End If

                If (y4Index < Main.LAST) Then
                    y4Value = Main.DataActions(y4Index)(dataRecord) * Main.DataUnits(y4Index, y4UnitsIndex)
                    lineSeries4.Points.Add(New OxyPlot.DataPoint(xValue, y4Value))
                    If (y4Value > y4Max) Then
                        y4Max = y4Value
                        y4MaxX = xValue
                    End If
                End If

            Next

            Select Case row
                Case 0
                    lblFile1.Text = clbFiles.Items.Item(i).ToString()
                    lblXMax1.Text = Main.NewCustomFormat(x1Max)
                    lblY1Max1.Text = Main.NewCustomFormat(y1Max) & " @ " & Main.NewCustomFormat(y1MaxX) & " " & xAxisUnit
                    lblY2Max1.Text = Main.NewCustomFormat(y2Max) & " @ " & Main.NewCustomFormat(y2MaxX) & " " & xAxisUnit
                    lblY3Max1.Text = Main.NewCustomFormat(y3Max) & " @ " & Main.NewCustomFormat(y3MaxX) & " " & xAxisUnit
                    lblY4Max1.Text = Main.NewCustomFormat(y4Max) & " @ " & Main.NewCustomFormat(y4MaxX) & " " & xAxisUnit
                Case 1
                    lblFile2.Text = clbFiles.Items.Item(i).ToString()
                    lblXMax2.Text = Main.NewCustomFormat(x1Max)
                    lblY1Max2.Text = Main.NewCustomFormat(y1Max) & " @ " & Main.NewCustomFormat(y1MaxX) & " " & xAxisUnit
                    lblY2Max2.Text = Main.NewCustomFormat(y2Max) & " @ " & Main.NewCustomFormat(y2MaxX) & " " & xAxisUnit
                    lblY3Max2.Text = Main.NewCustomFormat(y3Max) & " @ " & Main.NewCustomFormat(y3MaxX) & " " & xAxisUnit
                    lblY4Max2.Text = Main.NewCustomFormat(y4Max) & " @ " & Main.NewCustomFormat(y4MaxX) & " " & xAxisUnit
                Case 2
                    lblFile3.Text = clbFiles.Items.Item(i).ToString()
                    lblXMax3.Text = Main.NewCustomFormat(x1Max)
                    lblY1Max3.Text = Main.NewCustomFormat(y1Max) & " @ " & Main.NewCustomFormat(y1MaxX) & " " & xAxisUnit
                    lblY2Max3.Text = Main.NewCustomFormat(y2Max) & " @ " & Main.NewCustomFormat(y2MaxX) & " " & xAxisUnit
                    lblY3Max3.Text = Main.NewCustomFormat(y3Max) & " @ " & Main.NewCustomFormat(y3MaxX) & " " & xAxisUnit
                    lblY4Max3.Text = Main.NewCustomFormat(y4Max) & " @ " & Main.NewCustomFormat(y4MaxX) & " " & xAxisUnit
                Case 3
                    lblFile4.Text = clbFiles.Items.Item(i).ToString()
                    lblXMax4.Text = Main.NewCustomFormat(x1Max)
                    lblY1Max4.Text = Main.NewCustomFormat(y1Max) & " @ " & Main.NewCustomFormat(y1MaxX) & " " & xAxisUnit
                    lblY2Max4.Text = Main.NewCustomFormat(y2Max) & " @ " & Main.NewCustomFormat(y2MaxX) & " " & xAxisUnit
                    lblY3Max4.Text = Main.NewCustomFormat(y3Max) & " @ " & Main.NewCustomFormat(y3MaxX) & " " & xAxisUnit
                    lblY4Max4.Text = Main.NewCustomFormat(y4Max) & " @ " & Main.NewCustomFormat(y4MaxX) & " " & xAxisUnit
                Case 4
                    lblFile5.Text = clbFiles.Items.Item(i).ToString()
                    lblXMax5.Text = Main.NewCustomFormat(x1Max)
                    lblY1Max5.Text = Main.NewCustomFormat(y1Max) & " @ " & Main.NewCustomFormat(y1MaxX) & " " & xAxisUnit
                    lblY2Max5.Text = Main.NewCustomFormat(y2Max) & " @ " & Main.NewCustomFormat(y2MaxX) & " " & xAxisUnit
                    lblY3Max5.Text = Main.NewCustomFormat(y3Max) & " @ " & Main.NewCustomFormat(y3MaxX) & " " & xAxisUnit
                    lblY4Max5.Text = Main.NewCustomFormat(y4Max) & " @ " & Main.NewCustomFormat(y4MaxX) & " " & xAxisUnit
            End Select

            row = row + 1
        Next

        Me.ResumeLayout()
    End Sub

    Friend Sub btnClearOverlay_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearOverlay.Click
        ReDim AnalyzedData(MAXDATAFILES, Main.LAST, Main.MAXDATAPOINTS)
        OverlayFileCount = 0
        btnAddOverlayFile.Enabled = True
        Main.frmFit.chkAddOrNew.Enabled = True

        dataRecordsList.Clear()
        clbFiles.Items.Clear()
        SetupDiagram()
        'pnlOverlaySetup()
        btnAddOverlayFile.Enabled = True
    End Sub
    Private Sub btnSaveOverlay_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveOverlay.Click
        With SaveFileDialog1
            .Reset()
            .Filter = "Bitmap files (*.bmp)|*.bmp"
            .ShowDialog()
        End With
        If SaveFileDialog1.FileName <> "" Then
            OverlayBitMap.Save(SaveFileDialog1.FileName, Imaging.ImageFormat.Bmp)
        End If
    End Sub
    Private Sub cmbOverlayX_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayDataX.SelectedIndexChanged
        cmbOverlayUnitsX.Items.Clear()
        If cmbOverlayDataX.SelectedIndex <> Main.LAST Then
            cmbOverlayUnitsX.Items.AddRange(Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex)))
            cmbOverlayUnitsX.SelectedIndex = 0
        Else
            cmbOverlayUnitsX.Items.Add("--")
            cmbOverlayUnitsX.SelectedIndex = 0
        End If

        SetupDiagram()
        'pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayDataY1.SelectedIndexChanged
        cmbOverlayUnitsY1.Items.Clear()
        If cmbOverlayDataY1.SelectedIndex <> Main.LAST Then
            cmbOverlayUnitsY1.Items.AddRange(Split(Main.DataUnitTags(cmbOverlayDataY1.SelectedIndex)))
            cmbOverlayUnitsY1.SelectedIndex = 0
        Else
            cmbOverlayUnitsY1.Items.Add("--")
            cmbOverlayUnitsY1.SelectedIndex = 0
        End If

        SetupDiagram()
        'pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayDataY2.SelectedIndexChanged
        cmbOverlayUnitsY2.Items.Clear()
        If cmbOverlayDataY2.SelectedIndex <> Main.LAST Then
            cmbOverlayUnitsY2.Items.AddRange(Split(Main.DataUnitTags(cmbOverlayDataY2.SelectedIndex)))
            cmbOverlayUnitsY2.SelectedIndex = 0
        Else
            cmbOverlayUnitsY2.Items.Add("--")
            cmbOverlayUnitsY2.SelectedIndex = 0
        End If

        SetupDiagram()
        'pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayDataY3.SelectedIndexChanged
        cmbOverlayUnitsY3.Items.Clear()
        If cmbOverlayDataY3.SelectedIndex <> Main.LAST Then
            cmbOverlayUnitsY3.Items.AddRange(Split(Main.DataUnitTags(cmbOverlayDataY3.SelectedIndex)))
            cmbOverlayUnitsY3.SelectedIndex = 0
        Else
            cmbOverlayUnitsY3.Items.Add("--")
            cmbOverlayUnitsY3.SelectedIndex = 0
        End If

        SetupDiagram()
        'pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayDataY4.SelectedIndexChanged
        cmbOverlayUnitsY4.Items.Clear()
        If cmbOverlayDataY4.SelectedIndex <> Main.LAST Then
            cmbOverlayUnitsY4.Items.AddRange(Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex)))
            cmbOverlayUnitsY4.SelectedIndex = 0
        Else
            cmbOverlayUnitsY4.Items.Add("--")
            cmbOverlayUnitsY4.SelectedIndex = 0
        End If

        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub cmbOverlayXUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsX.SelectedIndexChanged
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub cmbOverlayY1Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY1.SelectedIndexChanged
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub cmbOverlayY2Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY2.SelectedIndexChanged
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub cmbOverlayY3Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY3.SelectedIndexChanged
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub cmbOverlayY4Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY4.SelectedIndexChanged
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub cmbOverlayCorrectedSpeedUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayCorrectedSpeedUnits.SelectedIndexChanged
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub

    Private Sub pnlDataOverlay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlDataOverlay.Click
        SetupDiagram()
        'pnlOverlaySetup()
    End Sub
    Private Sub pnlDataOverlay_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlDataOverlay.MouseMove
        'Display the X values based on the drawing window coordinates
        Dim MouseX As Integer, MouseY As Integer
        Dim LeftLimit As Integer, RightLimit As Integer, TopLimit As Integer, BottomLimit As Integer

        MouseX = pnlDataOverlay.PointToClient(Control.MousePosition).X
        MouseY = pnlDataOverlay.PointToClient(Control.MousePosition).Y

        LeftLimit = CInt(XOverlayStartFraction * PicOverlayWidth)
        RightLimit = CInt(XOverlayEndFraction * PicOverlayWidth)
        TopLimit = CInt(YOverlayStartFraction * PicOverlayHeight)
        BottomLimit = CInt(YOverlayEndFraction * PicOverlayHeight)

        If MouseX < LeftLimit Or MouseX > RightLimit Then
            OverlayXSelected = 0
            OverlayPlotMax = True
        Else
            If MouseY < TopLimit Or MouseY > BottomLimit Then
                OverlayXSelected = 0
                OverlayPlotMax = True
            Else
                'OverlayXSelected is the X value in the primary units being plotted
                OverlayXSelected = (MouseX - LeftLimit) / (RightLimit - LeftLimit) * xAxis
                OverlayPlotMax = False
                lblCurrentXValue.Text = Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & cmbOverlayUnitsX.SelectedItem.ToString
            End If
        End If
    End Sub

    Private Sub clbFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles clbFiles.SelectedIndexChanged
    End Sub

    Private Sub clbFiles_SelectedValueChanged(sender As Object, e As EventArgs) Handles clbFiles.SelectedValueChanged
        If clbFiles.CheckedItems.Count > 5 Then
            MessageBox.Show("Only the first 5 selected files can be displayed. Please deselect one or more files!", "Too many files selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SetupDiagram()

    End Sub
End Class