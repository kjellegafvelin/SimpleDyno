Imports System.Drawing.Drawing2D

Public Class SimpleDynoSubGauge
    Inherits SimpleDynoSubForm

    Private myGaugeSurface As Rectangle
    Private myDialRectangle As Rectangle

    'Dim BackgroundImage As Bitmap
    'Dim BackgroundGraphics As Graphics

    Private MajorTickOuter() As Point
    Private MajorTickInner() As Point
    Private MinorTickOuter() As Point
    Private MinorTickInner() As Point
    Private TickLabelPositions() As Point
    Private ParameterPosition As Point
    Private UnitPosition As Point
    Private EndOfNeedle As Point
    Private Center As Point

    Private NumberOfMajorTicks As Integer
    Private NumberOfMinorTicks As Integer
    Private LongestLabel As Integer
    ' Private Maximum As Double
    'Private Minimum As Double
    Private Angle As Double
    Private PointAngle As Double
    Private TickLabels() As String
    Public Overrides Sub ControlSpecificInitialization()


        myType = "Gauge"
        Y_Number_Allowed = 1
        XY_Selected = 1

        myConfiguration = "270 deg"
        Angle = 270
        PointAngle = 0 'CHECK - not been used now - forward compatability item
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

        'Debug.Print("Gauge Resize Called W " & Me.ClientSize.Width & " H " & Me.ClientSize.Height)

        Dim Width As Double, Count As Integer
        Dim MajorTickLength As Double, MinorTickLength As Double
        Dim Increment As Double

        Increment = 0.1
        LongestLabel = 1

        With myGaugeSurface
            .Width = Me.ClientSize.Width - 10 'padding 5 each side
            .Height = Me.ClientSize.Height - 10 'padding 5 each side
            .X = 5 'Puts the drawing surface top corner
            .Y = 5 ' in a posisition to pad 5 all around
            If .Height > .Width Then
                MajorTickLength = .Width / 5
            Else
                MajorTickLength = .Height / 5
            End If
            MinorTickLength = MajorTickLength / 2
        End With

        Select Case myConfiguration
            Case Is = "90 deg"
                With myDialRectangle
                    If myGaugeSurface.Width / 0.71 > myGaugeSurface.Height / 0.5 Then 'height drives the gauge
                        .Height = CInt(myGaugeSurface.Height / 0.5)
                        .Width = .Height
                    Else
                        .Width = CInt(myGaugeSurface.Width / 0.71)
                        .Height = .Width
                    End If
                    .X = CInt(Me.ClientSize.Width / 2 - .Width / 2)
                    .Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 - .Height / 2)
                    Center.X = CInt(.X + .Width / 2)
                    Center.Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 + .Height / 4)
                    For Count = 1 To NumberOfMajorTicks
                        MajorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(225 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(225 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickInner(Count).X = CInt(Center.X + (.Width - MajorTickLength) / 2 * Math.Cos(ConvertedToRadians(225 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickInner(Count).Y = CInt(Center.Y + (.Height - MajorTickLength) / 2 * Math.Sin(ConvertedToRadians(225 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        TickLabels(Count) = NewCustomFormat(Y_Minimum(Y_Number_Allowed) + (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) / (NumberOfMajorTicks - 1) * (Count - 1))
                        If TickLabels(Count).Length > TickLabels(LongestLabel).Length Then
                            LongestLabel = Count
                        End If
                    Next
                    For Count = 1 To NumberOfMinorTicks
                        MinorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(225 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(225 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickInner(Count).X = CInt(Center.X + (.Width - MinorTickLength) / 2 * Math.Cos(ConvertedToRadians(225 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickInner(Count).Y = CInt(Center.Y + (.Height - MinorTickLength) / 2 * Math.Sin(ConvertedToRadians(225 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                    Next
                End With

                Width = myDialRectangle.Width * 0.6
                Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                'Do Until (Grafx.Graphics.MeasureString(Y_Maximum(Y_Number_Allowed), Y_AxisFont).Width * 2 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width) > Width
                Do Until (Grafx.Graphics.MeasureString(TickLabels(LongestLabel), Y_AxisFont).Width * 2 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width) > Width
                    Increment += 0.1
                    Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                Loop

                TickLabelPositions(1) = MajorTickInner(1)
                TickLabelPositions(2) = MajorTickInner(2)
                TickLabelPositions(2).X -= CInt(Grafx.Graphics.MeasureString(TickLabels(2), Y_AxisFont).Width * 2 / 5)
                TickLabelPositions(3) = MajorTickInner(3)
                TickLabelPositions(3).X -= CInt(Grafx.Graphics.MeasureString(TickLabels(3), Y_AxisFont).Width / 2)
                TickLabelPositions(4) = MajorTickInner(4)
                TickLabelPositions(4).X -= CInt(Grafx.Graphics.MeasureString(TickLabels(4), Y_AxisFont).Width * 3 / 5)
                TickLabelPositions(5) = MajorTickInner(5)
                TickLabelPositions(5).X -= CInt(Grafx.Graphics.MeasureString(TickLabels(5), Y_AxisFont).Width)

                ParameterPosition.Y = TickLabelPositions(5).Y + Y_AxisFont.Height
                ParameterPosition.X = CInt(MajorTickOuter(3).X - Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)
                UnitPosition.Y = ParameterPosition.Y + Y_AxisFont.Height
                UnitPosition.X = CInt(MajorTickOuter(3).X - Grafx.Graphics.MeasureString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)

                myDialRectangle.Y = CInt(Center.Y - myDialRectangle.Height / 2)

            Case Is = "180 deg"

                With myDialRectangle
                    If myGaugeSurface.Width <= myGaugeSurface.Height / 0.5 Then 'height drives the gauge
                        .Width = myGaugeSurface.Width
                        .Height = .Width
                    Else
                        .Height = CInt(myGaugeSurface.Height / 0.5)
                        .Width = .Height
                    End If
                    .X = CInt(Me.ClientSize.Width / 2 - .Width / 2)
                    .Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 - .Height / 2)
                    Center.X = CInt(.X + .Width / 2)

                    Center.Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 + .Height / 4)

                    For Count = 1 To NumberOfMajorTicks
                        MajorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(180 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(180 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickInner(Count).X = CInt(Center.X + (.Width - MajorTickLength) / 2 * Math.Cos(ConvertedToRadians(180 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickInner(Count).Y = CInt(Center.Y + (.Height - MajorTickLength) / 2 * Math.Sin(ConvertedToRadians(180 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        TickLabels(Count) = NewCustomFormat(Y_Minimum(Y_Number_Allowed) + (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) / (NumberOfMajorTicks - 1) * (Count - 1))
                        If TickLabels(Count).Length > TickLabels(LongestLabel).Length Then
                            LongestLabel = Count
                        End If
                    Next
                    For Count = 1 To NumberOfMinorTicks
                        MinorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(180 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(180 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickInner(Count).X = CInt(Center.X + (.Width - MinorTickLength) / 2 * Math.Cos(ConvertedToRadians(180 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickInner(Count).Y = CInt(Center.Y + (.Height - MinorTickLength) / 2 * Math.Sin(ConvertedToRadians(180 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                    Next
                End With

                Width = myDialRectangle.Width * 0.7
                Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                'Do Until Grafx.Graphics.MeasureString(Y_Maximum(Y_Number_Allowed), Y_AxisFont).Width * 2 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width > Width
                Do Until (Grafx.Graphics.MeasureString(TickLabels(LongestLabel), Y_AxisFont).Width * 2 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width) > Width
                    Increment += 0.1
                    Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                Loop

                TickLabelPositions(1) = MajorTickInner(1)
                TickLabelPositions(1).Y = CInt(TickLabelPositions(1).Y - (Grafx.Graphics.MeasureString(TickLabels(1), Y_AxisFont).Height / 1.25))
                TickLabelPositions(2) = MajorTickInner(2)
                TickLabelPositions(2).X = CInt(TickLabelPositions(2).X - (Grafx.Graphics.MeasureString(TickLabels(2), Y_AxisFont).Width * 1 / 5))
                TickLabelPositions(3) = MajorTickInner(3)
                TickLabelPositions(3).X = CInt(TickLabelPositions(3).X - (Grafx.Graphics.MeasureString(TickLabels(3), Y_AxisFont).Width / 2))
                TickLabelPositions(4) = MajorTickInner(4)
                TickLabelPositions(4).X = CInt(TickLabelPositions(4).X - (Grafx.Graphics.MeasureString(TickLabels(4), Y_AxisFont).Width * 4 / 5))
                TickLabelPositions(5) = MajorTickInner(5)
                TickLabelPositions(5).X = CInt(TickLabelPositions(5).X - Grafx.Graphics.MeasureString(TickLabels(5), Y_AxisFont).Width)
                TickLabelPositions(5).Y = CInt(TickLabelPositions(5).Y - (Grafx.Graphics.MeasureString(TickLabels(5), Y_AxisFont).Height / 1.25))


                ParameterPosition.Y = CInt(TickLabelPositions(2).Y + Y_AxisFont.Height / 1.5)
                ParameterPosition.X = CInt(MajorTickOuter(3).X - Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)
                UnitPosition.Y = ParameterPosition.Y + Y_AxisFont.Height 'myDialRectangle.Height / 3 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Height
                UnitPosition.X = CInt(MajorTickOuter(3).X - Grafx.Graphics.MeasureString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)

                myDialRectangle.Y = CInt(Center.Y - myDialRectangle.Height / 2)

            Case Is = "270 deg"
                With myDialRectangle
                    If myGaugeSurface.Width > myGaugeSurface.Height / 0.85 Then 'height drives the gauge
                        .Height = CInt(myGaugeSurface.Height / 0.85)
                        .Width = .Height
                    Else
                        .Width = myGaugeSurface.Width
                        .Height = .Width
                    End If
                    .X = CInt(Me.ClientSize.Width / 2 - .Width / 2)
                    .Y = myGaugeSurface.Y
                    Center.X = CInt(.X + .Width / 2)
                    'Center.Y = .Y + .Height / 2
                    Center.Y = CInt(myGaugeSurface.Y + myGaugeSurface.Height / 2 + .Height / 14)

                    For Count = 1 To NumberOfMajorTicks
                        MajorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(135 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(135 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickInner(Count).X = CInt(Center.X + (.Width - MajorTickLength) / 2 * Math.Cos(ConvertedToRadians(135 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        MajorTickInner(Count).Y = CInt(Center.Y + (.Height - MajorTickLength) / 2 * Math.Sin(ConvertedToRadians(135 + (Angle / (NumberOfMajorTicks - 1) * (Count - 1)))))
                        TickLabels(Count) = NewCustomFormat(Y_Minimum(Y_Number_Allowed) + (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) / (NumberOfMajorTicks - 1) * (Count - 1))
                        If TickLabels(Count).Length > TickLabels(LongestLabel).Length Then
                            LongestLabel = Count
                        End If
                    Next
                    For Count = 1 To NumberOfMinorTicks
                        MinorTickOuter(Count).X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(135 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickOuter(Count).Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(135 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickInner(Count).X = CInt(Center.X + (.Width - MinorTickLength) / 2 * Math.Cos(ConvertedToRadians(135 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                        MinorTickInner(Count).Y = CInt(Center.Y + (.Height - MinorTickLength) / 2 * Math.Sin(ConvertedToRadians(135 + (Angle / (NumberOfMinorTicks - 1) * (Count - 1)))))
                    Next
                End With

                Width = myDialRectangle.Width * 0.65
                Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                Do Until (Grafx.Graphics.MeasureString(TickLabels(LongestLabel), Y_AxisFont).Width * 2 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width) > Width
                    Increment += 0.1
                    Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
                Loop

                TickLabelPositions(1) = MajorTickInner(1)
                TickLabelPositions(1).Y = CInt(TickLabelPositions(1).Y - Grafx.Graphics.MeasureString(TickLabels(1), Y_AxisFont).Height)
                TickLabelPositions(2) = MajorTickInner(2)
                TickLabelPositions(2).X = CInt(TickLabelPositions(2).X - (Grafx.Graphics.MeasureString(TickLabels(2), Y_AxisFont).Width * 1 / 5))
                TickLabelPositions(3) = MajorTickInner(3)
                TickLabelPositions(3).X = CInt(TickLabelPositions(3).X - (Grafx.Graphics.MeasureString(TickLabels(3), Y_AxisFont).Width / 2))
                TickLabelPositions(4) = MajorTickInner(4)
                TickLabelPositions(4).X = CInt(TickLabelPositions(4).X - (Grafx.Graphics.MeasureString(TickLabels(4), Y_AxisFont).Width * 4 / 5))
                TickLabelPositions(5) = MajorTickInner(5)
                TickLabelPositions(5).X = CInt(TickLabelPositions(5).X - Grafx.Graphics.MeasureString(TickLabels(5), Y_AxisFont).Width)
                TickLabelPositions(5).Y = CInt(TickLabelPositions(5).Y - Grafx.Graphics.MeasureString(TickLabels(5), Y_AxisFont).Height)




                UnitPosition.X = CInt(MajorTickOuter(3).X - Grafx.Graphics.MeasureString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)
                UnitPosition.Y = TickLabelPositions(2).Y '.Height / 3 + Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Height
                ParameterPosition.Y = UnitPosition.Y - Y_AxisFont.Height
                ParameterPosition.X = CInt(MajorTickOuter(3).X - Grafx.Graphics.MeasureString(Y_PrimaryLabel(Y_Number_Allowed), Y_AxisFont).Width / 2)

                Y_DataPen(XY_Selected).Width = 3

                myDialRectangle.Y = CInt(Center.Y - myDialRectangle.Height / 2)

        End Select
    End Sub
    Overrides Sub DrawToBuffer(ByVal g As Graphics) 'WHY SEND A G AS AN ARGUMENT? - REMOVE
        
        Dim TickCount As Integer
        If Y_Result(XY_Selected) > Y_Maximum(Y_Number_Allowed) Then Y_Result(XY_Selected) = Y_Maximum(Y_Number_Allowed)
        If Y_Result(XY_Selected) < Y_Minimum(Y_Number_Allowed) Then Y_Result(XY_Selected) = Y_Minimum(Y_Number_Allowed)
        Grafx.Graphics.Clear(BackClr)

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.PixelOffsetMode = PixelOffsetMode.HighQuality

        With Grafx.Graphics
            Select Case myConfiguration
                Case Is = "90 deg"
                    .DrawArc(AxisPen, myDialRectangle, 225, 90)
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
                        EndOfNeedle.X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(225 + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                        EndOfNeedle.Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(225 + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                    End With
                    .DrawLine(Y_DataPen(XY_Selected), Center, EndOfNeedle)

                Case Is = "180 deg"
                    .DrawArc(AxisPen, myDialRectangle, 180, 180)
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
                        EndOfNeedle.X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(180 + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                        EndOfNeedle.Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(180 + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                    End With
                    .DrawLine(Y_DataPen(XY_Selected), Center, EndOfNeedle)
                Case Is = "270 deg"
                    .DrawArc(AxisPen, myDialRectangle, 135, 270)
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
                        EndOfNeedle.X = CInt(Center.X + .Width / 2 * Math.Cos(ConvertedToRadians(135 + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                        EndOfNeedle.Y = CInt(Center.Y + .Height / 2 * Math.Sin(ConvertedToRadians(135 + ((Y_Result(XY_Selected) - Y_Minimum(Y_Number_Allowed)) / (Y_Maximum(Y_Number_Allowed) - Y_Minimum(Y_Number_Allowed)) * Angle))))
                    End With
                    .DrawLine(Y_DataPen(XY_Selected), Center, EndOfNeedle)
            End Select

        End With

    End Sub
    Overrides Sub AddControlSpecificOptionItems()

        Dim TestStrip As ToolStripMenuItem
        Dim str1 As String
        Dim str2 As String()
        Dim str3 As String()
       

        str1 = "Configuration"
        str2 = {"90 deg", "180 deg", "270 deg"}
        str3 = {"Up"} 'Note - Only Up supported now - this will allow forward compatability later
      
        TestStrip = CreateAToolStripMenuItem("O", str1, str2, str3)
        contextmnu.Items.Add(TestStrip)

        str1 = "Range"
        str2 = {"Minimum", "Maximum"}
        str3 = {"TXT"}

        TestStrip = CreateAToolStripMenuItem("M", str1, str2, str3)
        contextmnu.Items.Add(TestStrip)
       

    End Sub
    Public Overrides Sub ControlSpecificOptionSelection(ByVal Sent As String)
        Select Case Sent
            Case Is = "O_0_0"
                myConfiguration = "90 deg"
                Angle = 90
                PointAngle = 0 'CHECK - not used at the moment
            Case Is = "O_1_0"
                myConfiguration = "180 deg"
                Angle = 180
                PointAngle = 0 'CHECK - not used at the moment
            Case Is = "O_2_0"
                myConfiguration = "270 deg"
                Angle = 270
                PointAngle = 0 'CHECK - not used at the moment
            Case Else
                Dim Temp As String()
                Temp = Split(Sent, " ")
                If Temp(0) = "M_0_0" Then Y_Minimum(Y_Number_Allowed) = CDbl(Temp(1))
                If Temp(0) = "M_1_0" Then Y_Maximum(Y_Number_Allowed) = CDbl(Temp(1))
        End Select
    End Sub
    Public Overrides Function ControlSpecificSerializationData() As String

    End Function
    Public Overrides Sub ControlSpecficCreateFromSerializedData(ByVal Sent As String())
        Angle = CDbl(Split(myConfiguration, " ")(0))
    End Sub
End Class
