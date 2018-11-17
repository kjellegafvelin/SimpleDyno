Imports System.Drawing.Drawing2D
Public Class SimpleDynoSubMultiYTimeGraph
    Inherits SimpleDynoSubForm

    Private myGraphSurface As Rectangle
    Private myDataRectangle As Rectangle

    Private MajorXTickOuter(4) As Point
    Private MajorXTickInner(4) As Point
    Private MinorXTickOuter(4) As Point
    Private MinorXTickInner(4) As Point

    Private MajorYTickOuter(,) As Point
    Private MajorYTickInner(,) As Point
    Private MinorYTickOuter(,) As Point
    Private MinorYTickInner(,) As Point

    Private XTickLabelPositions(4) As Point
    Private YTickLabelPositions(,) As Point
    Private XOptionPosition As Point
    Private XUnitPosition As Point
    Private YOptionPosition(4) As Point
    Private YUnitPosition(4) As Point

    Private NumberOfMajorTicks As Integer
    Private NumberOfMinorTicks As Integer

    Private OldX As Integer, OldY(4) As Integer, NewX As Integer, NewY(4) As Integer
    Private X_Time As Double

    Private XTickLabels() As String
    Private YTickLabels(,) As String
    Private XAxisLabel As String
    Private YAxisLabel(4) As String

    Private NewSessionTime As Double
    Private OldSessionTime As Double
    Public Overrides Sub ControlSpecificInitialization()

        Dim Counter As Integer

        myType = "MultiYTimeGraph"
        Y_Number_Allowed = 4
        myConfiguration = "Lines"

        For Counter = 1 To Y_Number_Allowed
            IsThisYSelected(Counter) = False
        Next



        IsThisYSelected(1) = True

        X_PrimaryPointer = 0 'UBound(CopyOfDataNames) 'always points to the session timer
        X_MinCurMaxPointer = 1 'always points to the "current" time
        X_PrimaryLabel = "Time"
        X_UnitsLabel = "Seconds"
        NumberOfMajorTicks = 5
        NumberOfMinorTicks = 21
        X_Maximum = 10
        X_Minimum = 0

        ReDim MajorXTickOuter(NumberOfMajorTicks)
        ReDim MajorXTickInner(NumberOfMajorTicks)
        ReDim MinorXTickOuter(NumberOfMinorTicks)
        ReDim MinorXTickInner(NumberOfMinorTicks)
        ReDim MajorYTickOuter(4, NumberOfMajorTicks)
        ReDim MajorYTickInner(4, NumberOfMajorTicks)
        ReDim MinorYTickOuter(4, NumberOfMinorTicks)
        ReDim MinorYTickInner(4, NumberOfMinorTicks)
        ReDim XTickLabelPositions(NumberOfMajorTicks)
        ReDim YTickLabelPositions(4, NumberOfMajorTicks)
        ReDim XTickLabels(NumberOfMajorTicks)
        ReDim YTickLabels(4, NumberOfMajorTicks)

    End Sub
    Public Overrides Sub ControlSpecificResize()

        Dim Width As Double, Height As Double, Count As Integer
        Dim MajorTickLength As Double, MinorTickLength As Double
        Dim Increment As Double

        Increment = 0.1

        With myGraphSurface
            .Width = Me.ClientSize.Width - 10 'padding 5 each side
            .Height = Me.ClientSize.Height - 10 'padding 5 each side
            .X = 5
            .Y = 5
            MajorTickLength = .Height / 25
            MinorTickLength = MajorTickLength / 2
        End With

        For Count = 1 To 4
            Y_DataPen(Count).Width = CInt(MinorTickLength / 3)
        Next


        With myDataRectangle
            .Height = CInt(myGraphSurface.Height * 0.7)
            .Width = CInt(myGraphSurface.Width * 0.7)
            .X = CInt(Me.ClientSize.Width / 2 - .Width / 2)
            .Y = CInt(Me.ClientSize.Height / 2 - .Height / 2)

            For Count = 1 To NumberOfMajorTicks
                MajorXTickOuter(Count).X = CInt(.X + .Width / (NumberOfMajorTicks - 1) * (Count - 1))
                MajorXTickOuter(Count).Y = CInt(.Y + .Height + MajorTickLength)
                MajorXTickInner(Count).X = CInt(MajorXTickOuter(Count).X)
                MajorXTickInner(Count).Y = .Y + .Height
                XTickLabels(Count) = (X_Minimum + (X_Maximum - X_Minimum) / (NumberOfMajorTicks - 1) * (Count - 1)).ToString

                'Y1 - left side - outside
                MajorYTickOuter(1, Count).X = CInt(.X - MajorTickLength)
                MajorYTickOuter(1, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                MajorYTickInner(1, Count).X = .X
                MajorYTickInner(1, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                YTickLabels(1, Count) = (Y_Minimum(1) + (Y_Maximum(1) - Y_Minimum(1)) / (NumberOfMajorTicks - 1) * (Count - 1)).ToString

                'Y2 - left side - inside
                MajorYTickOuter(2, Count).X = (CInt(.X + MajorTickLength))
                MajorYTickOuter(2, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                MajorYTickInner(2, Count).X = .X
                MajorYTickInner(2, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                YTickLabels(2, Count) = (Y_Minimum(2) + (Y_Maximum(2) - Y_Minimum(2)) / (NumberOfMajorTicks - 1) * (Count - 1)).ToString

                'Y3 - right side - inside
                MajorYTickOuter(3, Count).X = CInt(.X + .Width - MajorTickLength)
                MajorYTickOuter(3, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                MajorYTickInner(3, Count).X = .X + .Width
                MajorYTickInner(3, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                YTickLabels(3, Count) = (Y_Minimum(3) + (Y_Maximum(3) - Y_Minimum(3)) / (NumberOfMajorTicks - 1) * (Count - 1)).ToString

                'Y4 - right side - outside
                MajorYTickOuter(4, Count).X = CInt(.X + .Width + MajorTickLength)
                MajorYTickOuter(4, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                MajorYTickInner(4, Count).X = .X + .Width
                MajorYTickInner(4, Count).Y = CInt(.Bottom - .Height / (NumberOfMajorTicks - 1) * (Count - 1))
                YTickLabels(4, Count) = (Y_Minimum(4) + (Y_Maximum(4) - Y_Minimum(4)) / (NumberOfMajorTicks - 1) * (Count - 1)).ToString


            Next
            For Count = 1 To NumberOfMinorTicks
                MinorXTickOuter(Count).X = CInt(.X + .Width / (NumberOfMinorTicks - 1) * (Count - 1))
                MinorXTickOuter(Count).Y = CInt(.Y + .Height + MinorTickLength)
                MinorXTickInner(Count).X = MinorXTickOuter(Count).X
                MinorXTickInner(Count).Y = .Y + .Height

                'Y1
                MinorYTickOuter(1, Count).X = CInt(.X - MinorTickLength)
                MinorYTickOuter(1, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))
                MinorYTickInner(1, Count).X = .X
                MinorYTickInner(1, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))

                'Y2
                MinorYTickOuter(2, Count).X = CInt(.X + MinorTickLength)
                MinorYTickOuter(2, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))
                MinorYTickInner(2, Count).X = .X
                MinorYTickInner(2, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))

                'Y3
                MinorYTickOuter(3, Count).X = CInt(.X + .Width - MinorTickLength)
                MinorYTickOuter(3, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))
                MinorYTickInner(3, Count).X = .X + .Width
                MinorYTickInner(3, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))

                'Y4
                MinorYTickOuter(4, Count).X = CInt(.X + .Width + MinorTickLength)
                MinorYTickOuter(4, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))
                MinorYTickInner(4, Count).X = .X + .Width
                MinorYTickInner(4, Count).Y = CInt(.Bottom - .Height / (NumberOfMinorTicks - 1) * (Count - 1))
            Next
        End With

        Width = Math.Abs(MajorXTickInner(1).X - MajorXTickInner(2).X)
        Height = (myGraphSurface.Bottom - MajorXTickOuter(1).Y) / 2
        X_AxisFont = New Font(X_AxisFont.Name, CSng(Increment))
        Do Until Grafx.Graphics.MeasureString("99999", X_AxisFont).Width > Width Or Grafx.Graphics.MeasureString("99999", X_AxisFont).Height > Height
            Increment += 0.1
            X_AxisFont = New Font(X_AxisFont.Name, CSng(Increment))
        Loop
        Increment = 0.1

        Dim MaxLength As Integer = 1
        For Count = 1 To 4
            YAxisLabel(Count) = Y_PrimaryLabel(Count) & vbCrLf & "(" & myMinCurMaxAbb(Y_MinCurMaxPointer(Count)) & " " & Y_UnitsLabel(Count) & ")"
            If Y_PrimaryLabel(Count).ToString.Length > Y_PrimaryLabel(MaxLength).ToString.Length Then
                MaxLength = Count
            End If
        Next
        'CHECK - Remove the following prior to release
        'For Count = 1 To 4
        '    If Y_primarylabel(Count).ToString.Length > Y_primarylabel(MaxLength).ToString.Length Then
        '        MaxLength = Count
        '    End If
        'Next

        Width = myDataRectangle.Left
        Height = myDataRectangle.Top / 2
        Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
        'CHECK - Remove the following prior to release
        ' Do Until Grafx.Graphics.MeasureString(Y_PrimaryLabel(MaxLength), Y_AxisFont).Width > Width Or Grafx.Graphics.MeasureString(Y_PrimaryLabel(MaxLength), Y_AxisFont).Height > Height
        Do Until Grafx.Graphics.MeasureString(YAxisLabel(MaxLength), Y_AxisFont).Width > Width Or Grafx.Graphics.MeasureString(YAxisLabel(MaxLength), Y_AxisFont).Height > Height
            Increment += 0.1
            Y_AxisFont = New Font(Y_AxisFont.Name, CSng(Increment))
        Loop

        For Count = 1 To NumberOfMajorTicks
            XTickLabelPositions(Count).X = CInt(MajorXTickOuter(Count).X - Grafx.Graphics.MeasureString(XTickLabels(Count), X_AxisFont).Width / 2)
            XTickLabelPositions(Count).Y = MajorXTickOuter(Count).Y
            'Y1
            YTickLabelPositions(1, Count).Y = CInt(MajorYTickOuter(1, Count).Y - Grafx.Graphics.MeasureString(YTickLabels(1, Count), Y_AxisFont).Height / 2)
            YTickLabelPositions(1, Count).X = CInt(MajorYTickOuter(1, Count).X - Grafx.Graphics.MeasureString(YTickLabels(1, Count), Y_AxisFont).Width)
            'Y2
            YTickLabelPositions(2, Count).Y = CInt(MajorYTickOuter(2, Count).Y - Grafx.Graphics.MeasureString(YTickLabels(2, Count), Y_AxisFont).Height / 2)
            YTickLabelPositions(2, Count).X = MajorYTickOuter(2, Count).X '- grafx.Graphics.MeasureString(YTickLabels(2, Count), Y_axisfont).Width
            'Y3
            YTickLabelPositions(3, Count).Y = CInt(MajorYTickOuter(3, Count).Y - Grafx.Graphics.MeasureString(YTickLabels(3, Count), Y_AxisFont).Height / 2)
            YTickLabelPositions(3, Count).X = CInt(MajorYTickOuter(3, Count).X - Grafx.Graphics.MeasureString(YTickLabels(3, Count), Y_AxisFont).Width)
            'Y4
            YTickLabelPositions(4, Count).Y = CInt(MajorYTickOuter(4, Count).Y - Grafx.Graphics.MeasureString(YTickLabels(4, Count), Y_AxisFont).Height / 2)
            YTickLabelPositions(4, Count).X = MajorYTickOuter(4, Count).X '- grafx.Graphics.MeasureString(YTickLabels(4, Count), Y_axisfont).Width
        Next

        XAxisLabel = "Time (seconds)"
        'CHECK - Remove the following prior to release
        'For Count = 1 To 4
        '    YAxisLabel(Count) = Y_PrimaryLabel(Count) & vbCrLf & "(" & myMinCurMaxAbb(Y_MinCurMaxPointer(Count)) & " " & Y_UnitsLabel(Count) & ")"
        'Next
        XOptionPosition.X = CInt(myDataRectangle.Width / 2 + MajorXTickOuter(1).X - Grafx.Graphics.MeasureString(XAxisLabel, X_AxisFont).Width / 2)
        XOptionPosition.Y = CInt(MajorXTickOuter(1).Y + Grafx.Graphics.MeasureString(XAxisLabel, X_AxisFont).Height)
        'Y1
        YOptionPosition(1).X = myGraphSurface.Left
        YOptionPosition(1).Y = CInt(YTickLabelPositions(1, NumberOfMajorTicks).Y - Grafx.Graphics.MeasureString(YAxisLabel(1), Y_AxisFont).Height)
        'Y2
        YOptionPosition(2).X = myDataRectangle.Left 'myGraphSurface.Left 
        YOptionPosition(2).Y = CInt(YTickLabelPositions(2, NumberOfMajorTicks).Y - Grafx.Graphics.MeasureString(YAxisLabel(2), Y_AxisFont).Height)
        'Y3
        YOptionPosition(3).X = CInt(myDataRectangle.Right - Grafx.Graphics.MeasureString(YAxisLabel(3), Y_AxisFont).Width) ' myGraphSurface.Left
        YOptionPosition(3).Y = CInt(YTickLabelPositions(3, NumberOfMajorTicks).Y - Grafx.Graphics.MeasureString(YAxisLabel(3), Y_AxisFont).Height)
        'Y4
        YOptionPosition(4).X = myDataRectangle.Right
        YOptionPosition(4).Y = CInt(YTickLabelPositions(4, NumberOfMajorTicks).Y - Grafx.Graphics.MeasureString(YAxisLabel(4), Y_AxisFont).Height)

        ResetSDForm()
    End Sub
    Public Overrides Sub ResetSDForm()
        X_Result = 0
        X_Time = 0
        OldSessionTime = 0
        Redraw()
        For count As Integer = 1 To Y_Number_Allowed
            OldY(count) = CInt(myDataRectangle.Bottom - (((0 - Y_Minimum(count)) / (Y_Maximum(count) - Y_Minimum(count))) * myDataRectangle.Height))
        Next
    End Sub

    Overrides Sub DrawToBuffer(ByVal g As Graphics) 'WHY SEND A G AS AN ARGUMENT? - REMOVE
        'Calc the result and labels
        Dim Count As Integer

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.PixelOffsetMode = PixelOffsetMode.HighQuality


        If TimerTiggered = False Then
            Redraw()
        Else
            With Grafx.Graphics
                If X_Time >= X_Maximum Then
                    X_Time = 0
                    Redraw()
                End If

                X_Time = X_Time + (X_Result - OldSessionTime)
                OldSessionTime = X_Result
                If X_Time >= X_Maximum Then
                    NewX = myDataRectangle.Width + myDataRectangle.Left
                Else
                    NewX = CInt(((X_Time - X_Minimum) / (X_Maximum - X_Minimum)) * myDataRectangle.Width + myDataRectangle.Left)
                End If
                For Count = 1 To 4
                    If IsThisYSelected(Count) = True Then
                        If Y_Result(Count) > Y_Maximum(Count) Then Y_Result(Count) = Y_Maximum(Count)
                        If Y_Result(Count) < Y_Minimum(Count) Then Y_Result(Count) = Y_Minimum(Count)
                        NewY(Count) = CInt(myDataRectangle.Bottom - (((Y_Result(Count) - Y_Minimum(Count)) / (Y_Maximum(Count) - Y_Minimum(Count))) * myDataRectangle.Height))
                        Select Case myConfiguration
                            Case Is = "Lines"
                                .DrawLine(Y_DataPen(Count), OldX, OldY(Count), NewX, NewY(Count))
                                OldY(Count) = NewY(Count)
                            Case Is = "Points"
                                .DrawEllipse(Y_DataPen(Count), NewX, NewY(Count), Y_DataPen(Count).Width, Y_DataPen(Count).Width)
                        End Select
                    End If
                Next
                OldX = NewX

            End With
        End If

    End Sub
    Private Sub Redraw()
        Dim Tickcount As Integer, PlotYCount As Integer
        With Grafx.Graphics
            .Clear(BackClr)

            For Tickcount = 1 To NumberOfMajorTicks
                .DrawLine(AxisPen, MajorXTickOuter(Tickcount), MajorXTickInner(Tickcount))
                .DrawString(XTickLabels(Tickcount), X_AxisFont, AxisBrush, XTickLabelPositions(Tickcount))
                For PlotYCount = 1 To 4
                    If IsThisYSelected(PlotYCount) = True Then
                        .DrawLine(AxisPen, MajorYTickOuter(PlotYCount, Tickcount), MajorYTickInner(PlotYCount, Tickcount))
                        .DrawString(YTickLabels(PlotYCount, Tickcount), Y_AxisFont, AxisBrush, YTickLabelPositions(PlotYCount, Tickcount))
                    End If
                Next
            Next

            .DrawLine(AxisPen, MajorXTickInner(1), MajorXTickInner(NumberOfMajorTicks))
            .DrawLine(AxisPen, MajorYTickInner(1, 1), MajorYTickInner(1, NumberOfMajorTicks))
            If IsThisYSelected(3) = True Or IsThisYSelected(4) = True Then .DrawLine(AxisPen, MajorYTickInner(3, 1), MajorYTickInner(3, NumberOfMajorTicks))
            For Tickcount = 1 To NumberOfMinorTicks
                .DrawLine(AxisPen, MinorXTickOuter(Tickcount), MinorXTickInner(Tickcount))
                For PlotYCount = 1 To 4
                    If IsThisYSelected(PlotYCount) = True Then
                        .DrawLine(AxisPen, MinorYTickOuter(PlotYCount, Tickcount), MinorYTickInner(PlotYCount, Tickcount))
                    End If
                Next
            Next

            .DrawString(XAxisLabel, X_AxisFont, AxisBrush, XOptionPosition)

            For PlotYCount = 1 To 4
                If IsThisYSelected(PlotYCount) = True Then
                    .DrawString(YAxisLabel(PlotYCount), Y_AxisFont, Y_DataBrush(PlotYCount), YOptionPosition(PlotYCount))
                End If
            Next

            NewX = CInt(((0 - X_Minimum) / (X_Maximum - X_Minimum)) * myDataRectangle.Width + myDataRectangle.Left)
            OldX = NewX

            ' For PlotYCount = 1 To 4
            ' NewY(PlotYCount) = myDataRectangle.Bottom - (((0 - Y_Minimum(PlotYCount)) / (Y_Maximum(PlotYCount) - Y_Minimum(PlotYCount))) * myDataRectangle.Height)
            ' OldY(PlotYCount) = NewY(PlotYCount)
            ' Next

        End With
    End Sub
    Overrides Sub AddControlSpecificOptionItems()
        Dim TestStrip As ToolStripMenuItem
        Dim str1 As String
        Dim str2 As String()
        Dim str3 As String()

        str1 = "Configuration"
        str2 = {"Lines", "Points"}
        str3 = {}

        TestStrip = CreateAToolStripMenuItem("O", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

        str1 = "Y Range"
        str2 = {"Minimum", "Maximum"}
        str3 = {"TXT"}

        TestStrip = CreateAToolStripMenuItem("Y", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

        str1 = "Time Range"
        str2 = {"Maximum"}
        str3 = {"TXT"}

        TestStrip = CreateAToolStripMenuItem("T", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

        str1 = "Remove Plot"
        str2 = {}
        str3 = {}

        TestStrip = CreateAToolStripMenuItem("X", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)

    End Sub
  
    Public Overrides Sub ControlSpecificOptionSelection(ByVal Sent As String)

        Select Case Sent
            Case Is = "O_0"
                myConfiguration = "Lines"
            Case Is = "O_1"
                myConfiguration = "Points"
            Case Is = "X"
                IsThisYSelected(XY_Selected) = False
                Y_PrimaryLabel(XY_Selected) = "Parameter"
                Y_UnitsLabel(XY_Selected) = "Units"
            Case Else
                Dim Temp As String()
                Temp = Split(Sent, " ")
                ' If Temp(0) = "T_0_0" Then X_Minimum = CDbl(Temp(1)) REMOVING Time Minimum - Will need to be put back in for Y vs X plot
                If Temp(0) = "T_0_0" Then X_Maximum = CDbl(Temp(1))
                If Temp(0) = "Y_0_0" Then Y_Minimum(XY_Selected) = CDbl(Temp(1))
                If Temp(0) = "Y_1_0" Then Y_Maximum(XY_Selected) = CDbl(Temp(1))
        End Select
    End Sub
    Public Overrides Function ControlSpecificSerializationData() As String


    End Function
    Public Overrides Sub ControlSpecficCreateFromSerializedData(ByVal Sent As String())

    End Sub
    Overrides Sub ShowTheMenu()
        'This needs to be overidden from the parent as there are two options for data, x and y
        'and which one we select is based on where the mouseclick happened
        Dim Where As New System.Drawing.Point
        Where = Me.PointToClient(MousePosition)

        With Where

            .X = Control.MousePosition.X
            .Y = Control.MousePosition.Y
            'First check that we are above the X axis
            If Me.PointToClient(MousePosition).Y < myDataRectangle.Bottom AndAlso Me.PointToClient(MousePosition).Y > myDataRectangle.Top Then
                Select Case Me.PointToClient(MousePosition).X
                    Case Is < myDataRectangle.Left
                        XY_Selected = 1
                        IsThisYSelected(1) = True
                    Case Is > myDataRectangle.Right
                        XY_Selected = 4
                        IsThisYSelected(4) = True
                    Case Else
                        If Me.PointToClient(MousePosition).X < myDataRectangle.Left + myDataRectangle.Width / 2 Then
                            XY_Selected = 2
                            IsThisYSelected(2) = True
                        Else
                            XY_Selected = 3
                            IsThisYSelected(3) = True
                        End If
                End Select
                Contextmnu.Show(Where)
            End If
        End With

    End Sub
    
End Class
