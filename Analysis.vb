Imports System.IO
Imports System.Drawing.Drawing2D
Public Class Analysis
    'Overlay Specific
    Private OverlayBMP As Graphics
    Private OverlayBitMap As Bitmap
    Private PicOverlayHeight As Integer
    Private PicOverlayWidth As Integer
    Private OverlayFileCount As Integer = 0

    Private DataInputFile As StreamReader
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
    Friend Sub Analysis_Setup()
        ReDim OverlayFiles(MAXDATAFILES)

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
        tempstring = tempstring & "None"
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

        pnlOverlaySetup()
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
                    If AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter) > xMax(FileCount) Then
                        xMax(FileCount) = AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter)
                        If xMax(FileCount) > xAxis Then
                            xAxis = xMax(FileCount)
                        End If
                    End If
                    'Check to see if we have passed the selected X value - this needs more work?
                    If Math.Abs(AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter) - OverlayXSelected) < XMaxDifference Then
                        XMaxDifference = Math.Abs(AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter) - OverlayXSelected)
                        XMaxDifferencePointer(FileCount) = Counter
                    End If
                    If AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, Counter) > y1Max(FileCount) Then
                        y1Max(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, Counter)
                        y1MaxAtX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter)
                        If y1Max(FileCount) > y1Axis Then
                            y1Axis = y1Max(FileCount)
                        End If
                    End If
                    If AnalyzedData(FileCount, cmbOverlayDataY2.SelectedIndex, Counter) > y2Max(FileCount) Then
                        y2Max(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY2.SelectedIndex, Counter)
                        y2MaxAtX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter)
                        If y2Max(FileCount) > y2Axis Then
                            y2Axis = y2Max(FileCount)
                        End If
                    End If
                    If AnalyzedData(FileCount, cmbOverlayDataY3.SelectedIndex, Counter) > y3Max(FileCount) Then
                        y3Max(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY3.SelectedIndex, Counter)
                        y3MaxAtX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter)
                        If y3Max(FileCount) > y3Axis Then
                            y3Axis = y3Max(FileCount)
                        End If
                    End If
                    If AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, Counter) > y4Max(FileCount) Then
                        y4Max(FileCount) = AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, Counter)
                        y4MaxAtX(FileCount) = AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, Counter)
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
                            .DrawLine(Y1Pen, CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y1Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY1.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y1Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight))
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
                        TickInterval = PicOverlayHeight * (YOverlayEndFraction - YOverlayStartFraction) * 1 / 5
                        For Counter = 0 To 4
                            TempString = Main.NewCustomFormat((((y4Axis) * Main.DataUnits(cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex)) / 5 * (5 - Counter)))
                            .DrawLine(AxisPen, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)), CInt(PicOverlayWidth * XOverlayEndFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter)))
                            .DrawString(TempString, AxisFont, AxisBrush, CInt(PicOverlayWidth * XOverlayEndFraction + TickLength), CInt(PicOverlayHeight * YOverlayStartFraction + (TickInterval * Counter) - .MeasureString(TempString, AxisFont).Height / 2))
                        Next
                        TempString = Main.DataTags(cmbOverlayDataY4.SelectedIndex) & vbCrLf & "(" & Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex), " ")(cmbOverlayUnitsY4.SelectedIndex) & ")"
                        .DrawString(TempString, Y4Font, Y4Brush, CInt(PicOverlayWidth * XOverlayEndFraction), CInt(PicOverlayHeight * YOverlayStartFraction - 5 - .MeasureString(TempString, Y4Font).Height)) ' * 1.5))
                    'If OverlayPlotMax Then
                    'TempString = "Max " & Main.DataTags(cmbOverlayDataY4.SelectedIndex)
                    '.DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'Else
                    TempString = Main.DataTags(cmbOverlayDataY4.SelectedIndex)
                    .DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, Titleline)
                    'End If
                    If OverlayPlotMax Then
                        TempString = "Max (" & Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex), " ")(cmbOverlayUnitsY4.SelectedIndex) & ")"
                    Else
                        TempString = "(" & Split(Main.DataUnitTags(cmbOverlayDataY4.SelectedIndex), " ")(cmbOverlayUnitsY4.SelectedIndex) & ")"
                    End If

                    .DrawString(TempString, HeadingsFont, AxisBrush, Y4Column - .MeasureString(TempString, HeadingsFont).Width / 2, UnitsLine)
                    For FileCount = 1 To OverlayFileCount
                        If OverlayPlotMax Then
                            TempString = Main.NewCustomFormat(y4Max(FileCount) * Main.DataUnits(cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex)) & " @ " & Main.NewCustomFormat(y4MaxAtX(FileCount) * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y4Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        Else
                            TempString = Main.NewCustomFormat(y4MaxAtSelectedX(FileCount) * Main.DataUnits(cmbOverlayDataY4.SelectedIndex, cmbOverlayUnitsY4.SelectedIndex)) ' & " @ " & Main.NewCustomFormat(OverlayXSelected * Main.DataUnits(cmbOverlayDataX.SelectedIndex, cmbOverlayUnitsX.SelectedIndex)) & " " & Split(Main.DataUnitTags(cmbOverlayDataX.SelectedIndex), " ")(cmbOverlayUnitsX.SelectedIndex)
                            .DrawString(TempString, ResultsFont, AxisBrush, Y4Column - .MeasureString(TempString, ResultsFont).Width / 2, ResultsLine(FileCount))
                        End If

                        Y4Pen.DashStyle = OverlayDashes(FileCount)
                        For Counter = 2 To EqualSpacingCount - 1
                            .DrawLine(Y4Pen, CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter))) / y4Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight), CInt(XOverlayStartFraction * PicOverlayWidth + ((AnalyzedData(FileCount, cmbOverlayDataX.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1)))) / xAxis) * (XOverlayEndFraction - XOverlayStartFraction) * PicOverlayWidth), CInt(YOverlayEndFraction * PicOverlayHeight - (AnalyzedData(FileCount, cmbOverlayDataY4.SelectedIndex, CInt(EqualSpacingPointers(FileCount, Counter + 1))) / y4Axis) * (YOverlayEndFraction - YOverlayStartFraction) * PicOverlayHeight))
                        Next
                    Next
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

        Dim temp As String, line() As String
        Dim PointCount As Integer
        Dim CopyFileName As String
        Dim DataCopyfile As StreamWriter

        If Not e.Equals(System.EventArgs.Empty) Then
            With OpenFileDialog1
                .Reset()
                .Filter = "Power Run files v6.3+ (*.sdp)|*.sdp|Power Run Files v5.5+ (*.txt)|*.txt"
                .ShowDialog()
            End With
        End If
        If OpenFileDialog1.FileName <> "" Then
            DataInputFile = New System.IO.StreamReader(OpenFileDialog1.FileName)
            With DataInputFile
                temp = .ReadLine
                Select Case temp
                    Case Is = Main.PowerRunVersion, "POWER_RUN_6_3", "POWER_RUN_6_4" 'This is a valid current version file
                        OverlayFileCount += 1
                        If OverlayFileCount = MAXDATAFILES Then
                            btnAddOverlayFile.Enabled = False
                            Main.frmFit.chkAddOrNew.Checked = False
                            Main.frmFit.chkAddOrNew.Enabled = False
                        End If
                        OverlayFiles(OverlayFileCount) = OpenFileDialog1.FileName.Substring(OpenFileDialog1.FileName.LastIndexOf("\") + 1)
                        Do
                            temp = .ReadLine
                        Loop Until temp.StartsWith("NUMBER_OF_POINTS_FIT")
                        AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0) = CDbl(temp.Substring(temp.LastIndexOf(" "))) 'used the empty holder to remember the number of fit points
                        temp = .ReadLine 'starting

                        Dim ColumnTitles As String
                        Dim TitlesSplit As String()
                        Dim SearchString As String
                        Dim DataLine As String()
                        Dim UnitName As String()
                        Dim ParamCount As Integer
                        Dim ParamPosition As Integer

                        ColumnTitles = .ReadLine 'titles
                        TitlesSplit = Split(ColumnTitles, " ")

                        For PointCount = 1 To CInt(AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0))
                            DataLine = Split(.ReadLine, " ") 'reads all the values on this line into a string array
                            For ParamCount = 0 To Main.LAST - 1
                                'This is how the titles are created in the fitting code except we do not add the space
                                UnitName = Split(Main.DataUnitTags(ParamCount), " ")
                                SearchString = Main.DataTags(ParamCount).Replace(" ", "_") & "_(" & UnitName(0) & ")"
                                ParamPosition = Array.IndexOf(TitlesSplit, SearchString)
                                If ParamPosition <> -1 Then
                                    AnalyzedData(OverlayFileCount, ParamCount, PointCount) = CDbl(DataLine(ParamPosition))
                                End If
                            Next
                        Next
                    Case Is = "POWER_CURVE_6_0" ' These were the earlier beta testing versions for uno
                        'There are a number of different versions carrying this heading
                        'Main differences between these and 6.3+ versions are no "_" between parameter and unit and Watts in was called Watts_(e)
                        MsgBox("A copy of " & OpenFileDialog1.FileName & " will be saved as a new version .sdp file.", vbOKOnly)
                        'Convert old data to new data
                        'open the copy file
                        Main.SetMouseBusy_ThreadSafe(Me)
                        CopyFileName = OpenFileDialog1.FileName.Substring(0, OpenFileDialog1.FileName.Length - 4) & ".sdp"
                        DataCopyfile = New System.IO.StreamWriter(CopyFileName)
                        DataCopyfile.WriteLine(Main.PowerRunVersion)
                        DataCopyfile.WriteLine(CopyFileName)
                        'load it up as if it were a version 6 file
                        OverlayFileCount += 1
                        If OverlayFileCount = MAXDATAFILES Then
                            btnAddOverlayFile.Enabled = False
                            Main.frmFit.chkAddOrNew.Checked = False
                            Main.frmFit.chkAddOrNew.Enabled = False
                        End If
                        OverlayFiles(OverlayFileCount) = CopyFileName.Substring(CopyFileName.LastIndexOf("\") + 1)
                        'Now read through the lines and copy them to the new file
                        Dim temprollerdiameter As Double
                        Dim tempwheeldiameter As Double
                        Dim tempgearratio As Double
                        temp = .ReadLine 'original file name line
                        Do
                            temp = .ReadLine
                            DataCopyfile.WriteLine(temp)
                            'while we are at it - look for roller dia, wheel dia and gear ratio
                            If temp.LastIndexOf(" ") <> -1 Then
                                'Debug.Print(temp & temp.LastIndexOf(" "))
                                If temp.Split(CChar(" "))(1) = "Gear_Ratio:" Then tempgearratio = CDbl(temp.Split(CChar(" "))(2))
                                If temp.Split(CChar(" "))(1) = "Wheel_Diameter:" Then tempwheeldiameter = CDbl(temp.Split(CChar(" "))(2))
                                If temp.Split(CChar(" "))(1) = "Roller_Diameter:" Then temprollerdiameter = CDbl(temp.Split(CChar(" "))(2))
                            End If
                            'Loop Until temp.LastIndexOf("Target_Roller_Mass") <> -1 'this takes us to the end of the old headings
                        Loop Until temp = "PRIMARY_CHANNEL_CURVE_FIT_DATA"
                        'line that holds the number of datapoints
                        temp = .ReadLine : DataCopyfile.WriteLine(temp)
                        AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0) = CDbl(temp.Substring(temp.LastIndexOf(" ")))
                        'line that holds the starting point
                        temp = .ReadLine : DataCopyfile.WriteLine(temp)

                        Dim ColumnTitles As String
                        Dim TitlesSplit As String()
                        Dim SearchString As String
                        Dim DataLine As String()
                        Dim UnitName As String()
                        Dim ParamCount As Integer
                        Dim ParamPosition As Integer

                        ColumnTitles = .ReadLine 'titles
                        'now depending on the title line, difference approaches are required.
                        'First substitute the "Watts_(e)" string to the current "Watts In" string including the splitter "_"
                        If ColumnTitles.Contains("Point") AndAlso ColumnTitles.Contains("SystemTime") Then 'this is an early beta version.
                            For PointCount = 1 To CInt(AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0))
                                temp = .ReadLine
                                line = temp.Split(CChar(" "))
                                AnalyzedData(OverlayFileCount, Main.SESSIONTIME, PointCount) = CDbl(line(1))
                                AnalyzedData(OverlayFileCount, Main.RPM1_ROLLER, PointCount) = CDbl(line(6)) ' / Main.DataUnits(Main.RPM1_ROLLER, 1) 'convert old RPM to new rad/s
                                AnalyzedData(OverlayFileCount, Main.RPM1_WHEEL, PointCount) = CDbl(line(7)) '/ Main.DataUnits(Main.RPM1_ROLLER, 1) 'convert old RPM to new rad/s
                                AnalyzedData(OverlayFileCount, Main.RPM1_MOTOR, PointCount) = CDbl(line(8)) ' / Main.DataUnits(Main.RPM1_ROLLER, 1) 'convert old RPM to new rad/s
                                AnalyzedData(OverlayFileCount, Main.RPM2, PointCount) = CDbl(line(10))
                                AnalyzedData(OverlayFileCount, Main.RPM2_RATIO, PointCount) = CDbl(line(11))
                                AnalyzedData(OverlayFileCount, Main.RPM2_ROLLOUT, PointCount) = CDbl(line(12))
                                AnalyzedData(OverlayFileCount, Main.SPEED, PointCount) = CDbl(line(15)) '/ Main.DataUnits(Main.SPEED, 1) 'convert old MPH to new m/s
                                AnalyzedData(OverlayFileCount, Main.TORQUE_ROLLER, PointCount) = CDbl(line(18)) 'already in N.m
                                AnalyzedData(OverlayFileCount, Main.TORQUE_WHEEL, PointCount) = CDbl(line(19)) 'AnalyzedData(OverlayFileCount, Main.POWER, PointCount) * (tempwheeldiameter / temprollerdiameter)
                                AnalyzedData(OverlayFileCount, Main.TORQUE_MOTOR, PointCount) = CDbl(line(20)) 'AnalyzedData(OverlayFileCount, Main.TORQUE_WHEEL, PointCount) / tempgearratio
                                AnalyzedData(OverlayFileCount, Main.POWER, PointCount) = CDbl(line(30))
                                AnalyzedData(OverlayFileCount, Main.DRAG, PointCount) = CDbl(line(33))
                                AnalyzedData(OverlayFileCount, Main.VOLTS, PointCount) = CDbl(line(36))
                                AnalyzedData(OverlayFileCount, Main.AMPS, PointCount) = CDbl(line(39))
                                AnalyzedData(OverlayFileCount, Main.WATTS_IN, PointCount) = CDbl(line(40))
                                AnalyzedData(OverlayFileCount, Main.EFFICIENCY, PointCount) = CDbl(line(42))
                                AnalyzedData(OverlayFileCount, Main.TEMPERATURE1, PointCount) = CDbl(line(43))
                                'Everything else is going to be '0'
                            Next
                        Else 'It looks more like the current version, but not quite.
                            ColumnTitles = ColumnTitles.Replace("Watts_(e)", "Watts_In")
                            'No replace all "(" at the beginning of the units with "_("
                            ColumnTitles = ColumnTitles.Replace("(", "_(")

                            TitlesSplit = Split(ColumnTitles, " ")

                            For PointCount = 1 To CInt(AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0))
                                DataLine = Split(.ReadLine, " ") 'reads all the values on this line into a string array
                                For ParamCount = 0 To Main.LAST - 1
                                    'This is how the titles are created in the fitting code except we do not add the space
                                    UnitName = Split(Main.DataUnitTags(ParamCount), " ")
                                    SearchString = Main.DataTags(ParamCount).Replace(" ", "_") & "_(" & UnitName(0) & ")"
                                    ParamPosition = Array.IndexOf(TitlesSplit, SearchString)
                                    If ParamPosition <> -1 Then
                                        AnalyzedData(OverlayFileCount, ParamCount, PointCount) = CDbl(DataLine(ParamPosition))
                                    End If
                                Next
                            Next
                        End If
                        'Now write all of the analyzed data to the datacopy file as if it were a power run
                        'write the new heading line
                        Dim tempstring As String = ""
                        Dim tempsplit As String()
                        For ParamCount = 0 To Main.LAST - 1
                            tempsplit = Split(Main.DataUnitTags(ParamCount), " ")
                            tempstring = tempstring & Main.DataTags(ParamCount).Replace(" ", "_") & "_(" & tempsplit(0) & ") "
                        Next
                        'Write the column headings
                        DataCopyfile.WriteLine(tempstring)
                        'now write out the new file format
                        For PointCount = 1 To CInt(AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0))
                            tempstring = "" 'count.ToString & " "
                            For ParamCount = 0 To Main.LAST - 1 'CHECK - time is now the last column which will mess up the overlay routine .
                                tempsplit = Split(Main.DataUnitTags(ParamCount), " ") ' How many units are there
                                tempstring = tempstring & AnalyzedData(OverlayFileCount, ParamCount, PointCount) * Main.DataUnits(ParamCount, 0) & " " 'DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(unitcount) & ") "
                            Next
                            '...and write it
                            DataCopyfile.WriteLine(tempstring)
                        Next
                        DataCopyfile.WriteLine(vbCrLf)
                        Do Until .EndOfStream
                            temp = .ReadLine
                            DataCopyfile.WriteLine(temp)
                        Loop
                        DataCopyfile.Close()
                        Main.SetMouseNormal_ThreadSafe(Me)
                    Case Is = "POWER_CURVE" 'We are assuming that this is a SD 5.5 Power Run File.
                        MsgBox("A copy of " & OpenFileDialog1.FileName & " will be saved as a new version .sdp file.", vbOKOnly)
                        'Convert old data to new data
                        'open the copy file
                        Main.SetMouseBusy_ThreadSafe(Me)
                        CopyFileName = OpenFileDialog1.FileName.Substring(0, OpenFileDialog1.FileName.Length - 4) & ".sdp"
                        DataCopyfile = New System.IO.StreamWriter(CopyFileName)
                        DataCopyfile.WriteLine(Main.PowerRunVersion)
                        DataCopyfile.WriteLine(CopyFileName)
                        'load it up as if it were a version 6 file
                        OverlayFileCount += 1
                        If OverlayFileCount = MAXDATAFILES Then
                            btnAddOverlayFile.Enabled = False
                            Main.frmFit.chkAddOrNew.Checked = False
                            Main.frmFit.chkAddOrNew.Enabled = False
                        End If
                        OverlayFiles(OverlayFileCount) = CopyFileName.Substring(CopyFileName.LastIndexOf("\") + 1)
                        'Now read through the lines and copy them to the new file
                        Dim temprollerdiameter As Double
                        Dim tempwheeldiameter As Double
                        Dim tempgearratio As Double
                        temp = .ReadLine 'original file name line
                        Do
                            temp = .ReadLine
                            DataCopyfile.WriteLine(temp)
                            'while we are at it - look for roller dia, wheel dia and gear ratio
                            If temp.LastIndexOf(" ") <> -1 Then
                                'Debug.Print(temp & temp.LastIndexOf(" "))
                                If temp.Split(CChar(" "))(1) = "Gear_Ratio:" Then tempgearratio = CDbl(temp.Split(CChar(" "))(2))
                                If temp.Split(CChar(" "))(1) = "Wheel_Diameter:" Then tempwheeldiameter = CDbl(temp.Split(CChar(" "))(2))
                                If temp.Split(CChar(" "))(1) = "Roller_Diameter:" Then temprollerdiameter = CDbl(temp.Split(CChar(" "))(2))
                            End If
                            'Loop Until temp.LastIndexOf("Target_Roller_Mass") <> -1 'this takes us to the end of the old headings
                        Loop Until temp = "PRIMARY_CHANNEL_CURVE_FIT_DATA"
                        'line that holds the number of datapoints
                        temp = .ReadLine : DataCopyfile.WriteLine(temp)
                        AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0) = CDbl(temp.Substring(temp.LastIndexOf(" ")))
                        'line that holds the starting point
                        temp = .ReadLine : DataCopyfile.WriteLine(temp)
                        'next is the original heading line which we will discard
                        temp = .ReadLine
                        'write the new heading line
                        Dim tempstring As String = ""
                        Dim tempsplit As String()
                        Dim paramcount As Integer
                        For paramcount = 0 To Main.LAST - 1
                            tempsplit = Split(Main.DataUnitTags(paramcount), " ")
                            tempstring = tempstring & Main.DataTags(paramcount).Replace(" ", "_") & "_(" & tempsplit(0) & ") "
                        Next
                        'Write the column headings
                        DataCopyfile.WriteLine(tempstring)
                        'now read in all of the fit data 
                        For PointCount = 1 To CInt(AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0))
                            temp = .ReadLine
                            line = temp.Split(CChar(" "))
                            'This is the old line format
                            'Point Time RollerRPM WheelRPM MotorRPM SpeedMPH SpeedKPH PowerWatts PowerHP TorqueNm Torqueinoz Torquecmg DragWatts DragHP
                            '   1    2     3         4         5        6       7         8         9       10          11       12       13        14
                            'This is the new line format
                            'Time(Sec) RPM1_Roller(rad/s) RPM1_Wheel(rad/s) RPM1_Motor(rad/s) Speed(m/s) RPM2(rad/s) Ratio(M/W) Rollout(mm) Roller_Torque(N.m) Wheel_Torque(N.m) Motor_Torque(N.m) Power(W) Drag(W) Voltage(V) Current(A) Watts_(e)(W) Efficiency(%) Temperature(°C) 
                            'So...
                            AnalyzedData(OverlayFileCount, Main.SESSIONTIME, PointCount) = CDbl(line(1))
                            AnalyzedData(OverlayFileCount, Main.RPM1_ROLLER, PointCount) = CDbl(line(2)) / Main.DataUnits(Main.RPM1_ROLLER, 1) 'convert old RPM to new rad/s
                            AnalyzedData(OverlayFileCount, Main.RPM1_WHEEL, PointCount) = CDbl(line(3)) / Main.DataUnits(Main.RPM1_ROLLER, 1) 'convert old RPM to new rad/s
                            AnalyzedData(OverlayFileCount, Main.RPM1_MOTOR, PointCount) = CDbl(line(4)) / Main.DataUnits(Main.RPM1_ROLLER, 1) 'convert old RPM to new rad/s
                            AnalyzedData(OverlayFileCount, Main.SPEED, PointCount) = CDbl(line(5)) / Main.DataUnits(Main.SPEED, 1) 'convert old MPH to new m/s
                            AnalyzedData(OverlayFileCount, Main.TORQUE_ROLLER, PointCount) = CDbl(line(9)) 'already in N.m
                            AnalyzedData(OverlayFileCount, Main.POWER, PointCount) = CDbl(line(7))
                            AnalyzedData(OverlayFileCount, Main.DRAG, PointCount) = CDbl(line(12))
                            'recalc the motor and wheel torques
                            AnalyzedData(OverlayFileCount, Main.TORQUE_WHEEL, PointCount) = AnalyzedData(OverlayFileCount, Main.POWER, PointCount) * (tempwheeldiameter / temprollerdiameter)
                            AnalyzedData(OverlayFileCount, Main.TORQUE_MOTOR, PointCount) = AnalyzedData(OverlayFileCount, Main.TORQUE_WHEEL, PointCount) / tempgearratio
                            'Everything else is going to be '0'
                        Next
                        'now write out the new file format
                        For PointCount = 1 To CInt(AnalyzedData(OverlayFileCount, Main.SESSIONTIME, 0))
                            tempstring = "" 'count.ToString & " "
                            For paramcount = 0 To Main.LAST - 1 'CHECK - time is now the last column which will mess up the overlay routine .
                                tempsplit = Split(Main.DataUnitTags(paramcount), " ") ' How many units are there
                                tempstring = tempstring & AnalyzedData(OverlayFileCount, paramcount, PointCount) * Main.DataUnits(paramcount, 0) & " " 'DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(unitcount) & ") "
                            Next
                            '...and write it
                            DataCopyfile.WriteLine(tempstring)
                        Next
                        DataCopyfile.WriteLine(vbCrLf)
                        Do Until .EndOfStream
                            temp = .ReadLine
                            DataCopyfile.WriteLine(temp)
                        Loop
                        DataCopyfile.Close()
                        Main.SetMouseNormal_ThreadSafe(Me)
                    Case Else
                        MsgBox("Could not open file.  If this is a Power Run created by an older SimpleDyno version please email it to damorc1@hotmail.com so that a fix can be made available", vbOKOnly)
                End Select
            End With
            DataInputFile.Close()
        End If
        pnlOverlaySetup()
    End Sub
    Friend Sub btnClearOverlay_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearOverlay.Click
        ReDim AnalyzedData(MAXDATAFILES, Main.LAST, Main.MAXDATAPOINTS)
        OverlayFileCount = 0
        btnAddOverlayFile.Enabled = True
        Main.frmFit.chkAddOrNew.Enabled = True
        pnlOverlaySetup()
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
        pnlOverlaySetup()
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
        pnlOverlaySetup()
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
        pnlOverlaySetup()
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
        pnlOverlaySetup()
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
        pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayXUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsX.SelectedIndexChanged
        pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY1Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY1.SelectedIndexChanged
        pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY2Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY2.SelectedIndexChanged
        pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY3Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY3.SelectedIndexChanged
        pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayY4Units_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayUnitsY4.SelectedIndexChanged
        pnlOverlaySetup()
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnlOverlaySetup()
    End Sub
    Private Sub cmbOverlayCorrectedSpeedUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOverlayCorrectedSpeedUnits.SelectedIndexChanged
        pnlOverlaySetup()
    End Sub
    'CHECK IF YOU WANT TO PULL ALL DECLARATIONS TO THE TOP
    Private OverlayXSelected As Double
    Private OverlayPlotMax As Boolean = True
    Private Sub pnlDataOverlay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlDataOverlay.Click
        pnlOverlaySetup()
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
End Class