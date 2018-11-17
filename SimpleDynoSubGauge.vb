Imports System.Drawing.Drawing2D

Public Class SimpleDynoSubGauge
    Inherits SimpleDynoSubForm

    Private myGaugeSurface As Rectangle
    Private myDialRectangle As Rectangle

    'Dim BackgroundImage As Bitmap
    'Dim BackgroundGraphics As Graphics
    Dim TempTemp As Double

    Private MajorTickOuter() As Point
    Private MajorTickInner() As Point
    Private MinorTickOuter() As Point
    Private MinorTickInner() As Point
    Private TickLabelPositions() As Point
    Private ParameterPosition As Point
    Private UnitPosition As Point
    Private EndOfNeedle As Point
    Private Center As Point
    Private SweepClockwise As Integer
    Private NumberOfMajorTicks As Integer
    Private NumberOfMinorTicks As Integer
    Private LongestLabel As Integer
    Private Angle As Single
    Private PointAngle As Single
    Private StartAngle As Single
    Private TickLabels() As String

    'CHECK - PULL TIMER FROM RELEASE
    'Private TestTimer As Timer
    'CHECK - PULL TIMER FROM RELEASE
    Private Sub testtick(ByVal sender As Object, ByVal e As EventArgs)
        Angle += 5
        If Angle = 360 Then
            Angle = 10
            StartAngle += 5
        End If
        Me.ControlSpecificResize()
    End Sub
    Public Overrides Sub ControlSpecificInitialization()


        myType = "Gauge"
        Y_Number_Allowed = 1
        XY_Selected = 1

        '///////
        ''CHECK - PULL TIMER FROM RELEASE
        'TestTimer = New Timer
        'TestTimer.Interval = 20
        'TestTimer.Stop()
        ''//////
        'AddHandler Me.TestTimer.Tick, AddressOf testtick

        myConfiguration = "270 270 1"
        Angle = 270
        PointAngle = 270 'CHECK - not been used now - forward compatability item
        StartAngle = PointAngle - Angle / 2
        SweepClockwise = 1
        NumberOfMajorTicks = 5
        NumberOfMinorTicks = 21

        ReDim MajorTickOuter(NumberOfMajorTicks)
        ReDim MajorTickInner(NumberOfMajorTicks)
        ReDim MinorTickOuter(NumberOfMinorTicks)
        ReDim MinorTickInner(NumberOfMinorTicks)
        ReDim TickLabelPositions(NumberOfMajorTicks)
        ReDim TickLabels(NumberOfMajorTicks)

    End Sub
    Public Overrides Sub ControlSpecificResize()

        Dim Count As Integer
        Dim MajorTickLength As Double, MinorTickLength As Double
        Dim Increment As Double

        With myGaugeSurface
            .Width = CInt(Me.ClientSize.Width * 0.9) 'padding 1% each side
            .Height = CInt(Me.ClientSize.Height * 0.9) 'padding 1% each side
            .X = CInt(Me.ClientSize.Width * 0.05) 'Puts the drawing surface top corner
            .Y = CInt(Me.ClientSize.Height * 0.05) ' in a posisition to pad 5 all around
        End With

        Dim MinX As Double = 1, MinY As Double = 1, MaxX As Double = -1, MaxY As Double = -1, TempX As Double, TempY As Double
        Dim TempWidth As Double, TempHeight As Double, TempCenterX As Double, TempCenterY As Double

        For Arc As Integer = CInt(StartAngle) To CInt(StartAngle + Angle)
            TempX = Math.Cos(ConvertedToRadians(360 - Arc))
            TempY = Math.Sin(ConvertedToRadians(360 - Arc))
            If TempX < MinX Then MinX = TempX
            If TempX > MaxX Then MaxX = TempX
            If TempY < MinY Then MinY = TempY
            If TempY > MaxY Then MaxY = TempY
        Next

        MaxX = (CInt(MaxX * 1000) / 1000)
        MaxY = (CInt(MaxY * 1000) / 1000)
        MinX = (CInt(MinX * 1000) / 1000)
        MinY = (CInt(MinY * 1000) / 1000)

        If MinX >= 0 Then
            TempWidth = MaxX
            TempCenterX = 0
        Else
            If MaxX > 0 Then
                TempWidth = Math.Abs(MaxX - MinX)
                TempCenterX = TempWidth / Math.Abs(TempWidth / MinX)
            Else
                TempWidth = Math.Abs(MinX)
                TempCenterX = TempWidth '1
            End If
        End If

        If MinY >= 0 Then
            TempHeight = MaxY
            TempCenterY = TempHeight
        Else
            If MaxY > 0 Then
                TempHeight = Math.Abs(MaxY - MinY)
                TempCenterY = TempHeight / Math.Abs(TempHeight / MaxY)
            Else
                TempHeight = Math.Abs(MinY)
                TempCenterY = 0
            End If
        End If

        Dim FoldWidth As Double, FoldHeight As Double
        FoldWidth = myGaugeSurface.Width / TempWidth
        FoldHeight = myGaugeSurface.Height / TempHeight

        If FoldWidth >= FoldHeight Then
            myDialRectangle.Height = CInt(2 * FoldHeight)
            myDialRectangle.Width = CInt(2 * FoldHeight)
            MajorTickLength = myDialRectangle.Height * 0.15
            MinorTickLength = MajorTickLength / 2
            Center.X = CInt(myGaugeSurface.X + myGaugeSurface.Width / 2 - TempWidth * FoldHeight / 2 + TempWidth * FoldHeight * TempCenterX / TempWidth)
            Center.Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height * (TempCenterY / TempHeight))
        Else
            myDialRectangle.Height = CInt(2 * FoldWidth)
            myDialRectangle.Width = CInt(2 * FoldWidth)
            MajorTickLength = myDialRectangle.Width * 0.15
            MinorTickLength = MajorTickLength / 2
            Center.X = CInt(myGaugeSurface.X + myGaugeSurface.Width * (TempCenterX / TempWidth))
            Center.Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 - TempHeight * FoldWidth / 2 + TempHeight * FoldWidth * TempCenterY / TempHeight)
        End If

        myDialRectangle.X = Center.X - CInt(myDialRectangle.Width / 2)
        myDialRectangle.Y = Center.Y - CInt(myDialRectangle.Height / 2)

        With myDialRectangle
            For Count = 1 To NumberOfMajorTicks
                MajorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(StartAngle + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                MajorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(StartAngle + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                MajorTickInner(Count).X = CInt(Center.X + (.Width - MajorTickLength) / 2 * Math.Cos(ConvertedToRadians(StartAngle + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                MajorTickInner(Count).Y = CInt(Center.Y + (.Height - MajorTickLength) / 2 * Math.Sin(ConvertedToRadians(StartAngle + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                If SweepClockwise = 1 Then
                    TickLabels(Count) = NewCustomFormat(Y_Minimum(Y_Number_Allowed) + (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) / (NumberOfMajorTicks - 1) * (Count - 1))
                Else
                    TickLabels(Count) = NewCustomFormat(Y_Maximum(Y_Number_Allowed) - (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) / (NumberOfMajorTicks - 1) * (Count - 1))
                End If
Next
            For Count = 1 To NumberOfMinorTicks
                MinorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(StartAngle + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                MinorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(StartAngle + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                MinorTickInner(Count).X = CInt(Center.X + (.Width - MinorTickLength) / 2 * Math.Cos(ConvertedToRadians(StartAngle + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                MinorTickInner(Count).Y = CInt(Center.Y + (.Height - MinorTickLength) / 2 * Math.Sin(ConvertedToRadians(StartAngle + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
            Next
        End With

        Dim TickLabelWidths(NumberOfMajorTicks) As Double
        Dim TickLabelHeights(NumberOfMajorTicks) As Double
        With myDialRectangle
            Dim l As Double, Score As Integer

            Increment = 0
            Do
                Increment += 0.1
                Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                Score = 0
                For Count = 1 To NumberOfMajorTicks
                    TickLabelWidths(Count) = Grafx.Graphics.MeasureString(TickLabels(Count), Y_AxisFont).Width
                    TickLabelHeights(Count) = Grafx.Graphics.MeasureString(TickLabels(Count), Y_AxisFont).Height
                    l = ((TickLabelWidths(Count) / 2) ^ 2 + (TickLabelHeights(Count)) ^ 2) ^ 0.5
                    TickLabelPositions(Count).X = CInt(Center.X + (.Width - MajorTickLength - l) / 2 * Math.Cos(ConvertedToRadians(StartAngle + (Angle / (NumberOfMajorTicks - 1) * (Count - 1))))) - CInt(Grafx.Graphics.MeasureString(TickLabels(Count), Y_AxisFont).Width / 2)
                    TickLabelPositions(Count).Y = CInt(Center.Y + (.Height - MajorTickLength - l) / 2 * Math.Sin(ConvertedToRadians(StartAngle + (Angle / (NumberOfMajorTicks - 1) * (Count - 1))))) - CInt(Grafx.Graphics.MeasureString(TickLabels(Count), Y_AxisFont).Height / 2)
                Next
                TickLabelHeights(0) = Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Height * 2 'To cover primary lavel and units
                TickLabelWidths(0) = Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width
                'This option based on centering in the gaugesurface
                'TickLabelPositions(0).Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 - TickLabelHeights(0) / 4) '(Center.Y + (MajorTickInner(3).Y - Center.Y) / 2 - TickLabelHeights(0) / 2)
                'TickLabelPositions(0).X = CInt(myGaugeSurface.X + myGaugeSurface.Width / 2 - TickLabelWidths(0) / 2) 'CInt(Center.X + (MajorTickInner(3).X - Center.X) / 2 - TickLabelWidths(0) / 2)
                'This option based on centering between needle centre and tick 3
                TickLabelPositions(0).Y = CInt(Center.Y + (MajorTickInner(3).Y - Center.Y) / 2 - TickLabelHeights(0) / 2)
                TickLabelPositions(0).X = CInt(Center.X + (MajorTickInner(3).X - Center.X) / 2 - TickLabelWidths(0) / 2)
              
                For o As Integer = 0 To NumberOfMajorTicks
                    For i As Integer = 0 To NumberOfMajorTicks
                        If TickLabelPositions(o).X < TickLabelPositions(i).X + TickLabelWidths(i) AndAlso _
                            TickLabelPositions(o).X + TickLabelWidths(o) > TickLabelPositions(i).X AndAlso _
                            TickLabelPositions(o).Y < TickLabelPositions(i).Y + TickLabelHeights(i) AndAlso _
                            TickLabelPositions(o).Y + TickLabelHeights(o) > TickLabelPositions(i).Y Then
                            'No overlap
                            Score += 1
                        Else

                        End If
                    Next
                Next
                
            Loop Until Score > NumberOfMajorTicks + 1
            'Need to check that the end ticks (1 and 5) are not outside the Gaugesurface area
            If TickLabelPositions(1).X < Me.ClientRectangle.X Then TickLabelPositions(1).X = Me.ClientRectangle.X
            If TickLabelPositions(5).X < Me.ClientRectangle.X Then TickLabelPositions(5).X = Me.ClientRectangle.X
            If TickLabelPositions(1).Y < Me.ClientRectangle.Y Then TickLabelPositions(1).Y = Me.ClientRectangle.Y
            If TickLabelPositions(5).Y < Me.ClientRectangle.Y Then TickLabelPositions(5).Y = Me.ClientRectangle.Y

        End With

        ParameterPosition.Y = TickLabelPositions(0).Y
        ParameterPosition.X = TickLabelPositions(0).X

        UnitPosition.X = CInt(ParameterPosition.X + TickLabelWidths(0) / 2 - Grafx.Graphics.MeasureString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)
        UnitPosition.Y = ParameterPosition.Y + Y_AxisFont.Height

        Y_DataPen(XY_Selected).Width = 3

    End Sub
    Overrides Sub DrawToBuffer(ByVal g As Graphics) 'WHY SEND A G AS AN ARGUMENT? - REMOVE
        
        Dim TickCount As Integer
        If Y_Result(XY_Selected) > Y_Maximum(Y_Number_Allowed) Then Y_Result(XY_Selected) = Y_Maximum(Y_Number_Allowed)
        If Y_Result(XY_Selected) < Y_Minimum(Y_Number_Allowed) Then Y_Result(XY_Selected) = Y_Minimum(Y_Number_Allowed)
        Grafx.Graphics.Clear(BackClr)

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.PixelOffsetMode = PixelOffsetMode.HighQuality

        With Grafx.Graphics
            '///////
            '/ NEXT SECTION NEW APPROACH 6.5 TRYING TO AVOID SELECT CASE
            .DrawArc(AxisPen, myDialRectangle, StartAngle, Angle)
            For TickCount = 1 To NumberOfMajorTicks
                .DrawLine(AxisPen, MajorTickOuter(TickCount), MajorTickInner(TickCount))
                .DrawString(TickLabels(TickCount), Y_AxisFont, AxisBrush, TickLabelPositions(TickCount))
            Next
            For TickCount = 1 To NumberOfMinorTicks
                .DrawLine(AxisPen, MinorTickOuter(TickCount), MinorTickInner(TickCount))
            Next
            .DrawString(Y_PrimaryLabel(XY_Selected), Y_AxisFont, AxisBrush, ParameterPosition)
            .DrawString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(XY_Selected), Y_AxisFont, AxisBrush, UnitPosition)


            With myDialRectangle
                If SweepClockwise = 1 Then
                    EndOfNeedle.X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(StartAngle + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                    EndOfNeedle.Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(StartAngle + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                Else
                    EndOfNeedle.X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(StartAngle + Angle - ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                    EndOfNeedle.Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(StartAngle + Angle - ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                End If
            End With
            .DrawLine(Y_DataPen(XY_Selected), Center, EndOfNeedle)

            '/ END OF NEW SECTION
            '//////////////

        End With

    End Sub
    Overrides Sub AddControlSpecificOptionItems()

        Dim TestStrip As ToolStripMenuItem
        Dim str1 As String
        Dim str2 As String()
        Dim str3 As String()
       

        'str1 = "Configuration"
        'str2 = {"90 deg", "180 deg", "270 deg"}
        'str3 = {"Up", "Right", "Down", "Left"} 'Note - Only Up supported now - this will allow forward compatability later

        'TestStrip = CreateAToolStripMenuItem("O", str1, str2, str3)
        'contextmnu.Items.Add(TestStrip)

        str1 = "Configuration"
        str2 = {"Arc width (degrees)", "Direction (degrees)"}
        str3 = {"TXT"}

        TestStrip = CreateAToolStripMenuItem("F", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

        str1 = "Sweep Direction"
        str2 = {"Clockwise", "Anticlockwise"}
        str3 = {}

        TestStrip = CreateAToolStripMenuItem("O", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

        str1 = "Range"
        str2 = {"Minimum", "Maximum"}
        str3 = {"TXT"}

        TestStrip = CreateAToolStripMenuItem("M", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

    End Sub
    Public Overrides Sub ControlSpecificOptionSelection(ByVal Sent As String)
        Select Case Sent
            Case Is = "O_0"
                SweepClockwise = 1
                myConfiguration = Angle.ToString & " " & PointAngle.ToString & " " & SweepClockwise
            Case Is = "O_1"
                SweepClockwise = 0
                myConfiguration = Angle.ToString & " " & PointAngle.ToString & " " & SweepClockwise
            Case Else
                Dim Temp As String()
                Temp = Split(Sent, " ")
                If Temp(0) = "M_0_0" Then Y_Minimum(Y_Number_Allowed) = CDbl(Temp(1))
                If Temp(0) = "M_1_0" Then Y_Maximum(Y_Number_Allowed) = CDbl(Temp(1))
                If Temp(0) = "F_0_0" Then
                    Angle = CSng(Temp(1))
                    'PointAngle = 180 'CHECK - not used at the moment
                    StartAngle = PointAngle - Angle / 2
                    myConfiguration = Angle.ToString & " " & PointAngle.ToString & " " & SweepClockwise
                End If
                If Temp(0) = "F_1_0" Then
                    'Angle = CSng(Temp(1))
                    PointAngle = CSng(Temp(1)) '180 'CHECK - not used at the moment
                    StartAngle = PointAngle - Angle / 2
                    myConfiguration = Angle.ToString & " " & PointAngle.ToString & " " & SweepClockwise
                End If
        End Select
    End Sub
    Public Overrides Function ControlSpecificSerializationData() As String

    End Function
    Public Overrides Sub ControlSpecficCreateFromSerializedData(ByVal Sent As String())
        'Original version was the following line, but misses the point direction.  Need to check for pre-6.5 configuration string
        'Angle = CSng(Split(myConfiguration, " ")(0))
        Dim TempString() As String
        TempString = Split(myConfiguration, " ")
        Angle = CSng(TempString(0))
        If UBound(TempString) = 1 Then 'This is an older gauge version
            PointAngle = 270 'Up 'CHECK IT MAY MAKE MORE SENSE TO STORE THE ARC STARTING ANGLE
            SweepClockwise = 1
            myConfiguration = Angle.ToString & " " & PointAngle.ToString & " " & SweepClockwise.ToString
        Else
            PointAngle = CSng(TempString(1))
            SweepClockwise = CInt(TempString(2))
        End If
        StartAngle = PointAngle - Angle / 2
    End Sub
End Class
