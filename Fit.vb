Imports System.IO
Public Class Fit
    'CHECK - This needs to be reset to 0 for release versions
#Const LoadOldPowerRunData = 1

    Private AvailableFits As String() = {"Four Parameter", "2nd Order Poly", "3rd Order Poly", "4th Order Poly", "5th Order Poly", "MA Smooth"} ' "Test"} ', "Simple Smoothing"}
    Private FitStartPoint As Integer = 1
    Private CurrentSmooth As Double
    Private VoltageSmooth As Double
    Private RPM1Smooth As Double
    Private CoastDownSmooth As Double
    Private MaxPosition As Integer
    Private MinAllowableDataPoints As Integer = 10
    Public Shared PowerRunSpikeLevel As Double
    Private TempDouble As Double
    Private MaxRunDownRPM As Double
    'Dim CopyRPM1Data(Main.MAXDATAPOINTS) As Double
    Private x(0) As Double 'Main.MAXDATAPOINTS) As Double
    Private y(0) As Double ' (Main.MAXDATAPOINTS) As Double
    Private fy(0) As Double '(Main.MAXDATAPOINTS) As Double

    Private Vx(0) As Double 'Main.MAXDATAPOINTS) As Double
    Private Vy(0) As Double ' (Main.MAXDATAPOINTS) As Double
    Private Vfy(0) As Double '(Main.MAXDATAPOINTS) As Double

    Private Ix(0) As Double 'Main.MAXDATAPOINTS) As Double
    Private Iy(0) As Double ' (Main.MAXDATAPOINTS) As Double
    Private Ify(0) As Double '(Main.MAXDATAPOINTS) As Double


    Private CoastDownX(0) As Double
    Private CoastDownY(0) As Double
    Private CoastDownFY(0) As Double
    Private CoastDownP(0) As Double
    Private CoastDownT(0) As Double
    ' Dim fx(Main.MAXDATAPOINTS) As Double
    Private c() As Double
    Public Shared blnfit As Boolean = False
    Private blnRPMFit As Boolean = False
    Private blnCoastDownDownFit As Boolean = False
    Private blnVoltageFit As Boolean = False
    Private blnCurrentFit As Boolean = False
    Private LabelFont As New Font("Arial", 10)
    Private WhichFitData As Integer = 0
    Private RPM As Integer = 0
    Private RUNDOWN As Integer = 1
    Private CURRENT As Integer = 2
    Private VOLTAGE As Integer = 3
    Private FitData(Main.LAST, Main.MAXDATAPOINTS) As Double 'CHECK adding one additional primary dimension to hold residuals
    Private inputfile As StreamReader
    Private inputdialog As New OpenFileDialog
    Friend Sub Fit_Setup()
        cmbWhichFit.Items.AddRange(AvailableFits)
        cmbWhichFit.SelectedIndex = 1 'Second Order - fastest initial
        cmbWhichRDFit.Items.AddRange(AvailableFits)
        cmbWhichRDFit.SelectedIndex = 1 'Second Order - fastest initial
        VoltageSmooth = (scrlVoltageSmooth.Maximum + 1 - scrlVoltageSmooth.Value) / 2
        CurrentSmooth = (scrlCurrentSmooth.Maximum + 1 - scrlCurrentSmooth.Value) / 2
        RPM1Smooth = (scrlRPM1Smooth.Maximum + 1 - scrlRPM1Smooth.Value) / 2
        CoastDownSmooth = (scrlCoastDownSmooth.Maximum + 1 - scrlCoastDownSmooth.Value) / 2
    End Sub
    Private Sub Fit_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason <> CloseReason.FormOwnerClosing Then
            Me.Hide()
            e.Cancel = True
            Main.RestartForms()
            Main.btnShow_Click(Me, System.EventArgs.Empty)
        End If
    End Sub
    Private Sub scrlStartFit_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles scrlStartFit.Scroll
        If e.Type = ScrollEventType.EndScroll Then
            FitStartPoint = scrlStartFit.Maximum + 1 - scrlStartFit.Value
            txtFitStart.Text = FitStartPoint.ToString
            rdoRPM1.Checked = True
            ProcessData()
        End If
    End Sub
    Private Sub scrlCurrentSmooth_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles scrlCurrentSmooth.Scroll
        If e.Type = ScrollEventType.EndScroll Then
            CurrentSmooth = (scrlCurrentSmooth.Maximum + 1 - scrlCurrentSmooth.Value) / 2
            txtCurrentSmooth.Text = CurrentSmooth.ToString
            rdoCurrent.Checked = True
            FitCurrent()
        End If
    End Sub
    Private Sub scrlVoltageSmooth_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles scrlVoltageSmooth.Scroll
        If e.Type = ScrollEventType.EndScroll Then
            VoltageSmooth = (scrlVoltageSmooth.Maximum + 1 - scrlVoltageSmooth.Value) / 2
            txtVoltageSmooth.Text = VoltageSmooth.ToString
            rdoVoltage.Checked = True
            FitVoltage()
        End If
    End Sub
    Private Sub scrlRPM1Smooth_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles scrlRPM1Smooth.Scroll
        If e.Type = ScrollEventType.EndScroll Then
            RPM1Smooth = (scrlRPM1Smooth.Maximum + 1 - scrlRPM1Smooth.Value) / 2
            txtRPM1Smooth.Text = RPM1Smooth.ToString
            FitRPMData()
        End If
    End Sub
    Private Sub scrlCoastDownSmooth_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles scrlCoastDownSmooth.Scroll
        If e.Type = ScrollEventType.EndScroll Then
            CoastDownSmooth = (scrlCoastDownSmooth.Maximum + 1 - scrlCoastDownSmooth.Value) / 2
            txtCoastDownSmooth.Text = CoastDownSmooth.ToString
            FitRPMRunDownData()
        End If
    End Sub
    Private Sub txtPowerRunSpikeLevel_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPowerRunSpikeLevel.Leave
        If Main.Formloaded Then
            Dim LocalMin As Double = 1
            Dim LocalMax As Double = 999999
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
                PowerRunSpikeLevel = TempDouble
                rdoRPM1.Checked = True
                ProcessData()
            Else
                MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                With CType(sender, TextBox)
                    .Text = PowerRunSpikeLevel.ToString
                    .Focus()
                End With
            End If
        End If
    End Sub
#Region "Plotting Section"
    Private Sub pnlDataWindowSetup()
        'Need to resize the graphics panel to suit the screen size
        Me.Size = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size
        Dim PicDataWidth As Integer
        Dim PicDataHeight As Integer

        With pnlDataWindow
            .Width = Me.ClientSize.Width - .Left - 10
            .Height = Me.ClientSize.Height - .Top - 10
            PicDataHeight = .Height
            PicDataWidth = .Width
        End With

        Dim DataBitMap As Bitmap = New Bitmap(PicDataWidth, PicDataHeight)
        Dim DataWindowBMP As System.Drawing.Graphics = Graphics.FromImage(DataBitMap)

        Dim XLeft As Double = 0.1 * PicDataWidth
        Dim XRight As Double = 0.9 * PicDataWidth
        Dim Ytop As Double = 0.1 * PicDataHeight
        Dim YBottom As Double = 0.9 * PicDataHeight

        Dim AxesPen As New Pen(Color.Black)
        Dim AxisBrush As New SolidBrush(AxesPen.Color)
        Dim RawDataPen As New Pen(Color.Green, 2)
        Dim CopyDataPen As New Pen(Color.Black, 2)
        Dim RawDataBrush As New SolidBrush(RawDataPen.Color)
        Dim FitDataPen As New Pen(Color.Red, 2)
        Dim FitDataBrush As New SolidBrush(FitDataPen.Color)
        Dim TempTorqueDataPen As New Pen(Color.LightBlue, 2)
        Dim TempPowerDataPen As New Pen(Color.LightGray, 2)
        Dim TempTorqueDataBrush As New SolidBrush(TempTorqueDataPen.Color)
        Dim TempPowerDataBrush As New SolidBrush(TempPowerDataPen.Color)


        Dim RawDataMax As Double = 0
        Dim RawDataMin As Double = 999999
        Dim RawPDataMax As Double = 0
        Dim RawPDataMin As Double = 999999
        Dim RawTDataMax As Double = 0
        Dim RawTDataMin As Double = 999999
        Dim FitDataMax As Double = 0
        Dim FitDataMin As Double = 999999
        Dim WhichDimension As Integer
        Dim YAxisLabel As String = ""

        Dim XTimeMax As Double '= x(UBound(x)) 'FitData(Main.SESSIONTIME, UBound(FitData, 2))
        Dim XTimeMin As Double '= x(1)

        Select Case WhichFitData
            Case Is = RPM
                XTimeMax = x(UBound(x))
                XTimeMin = x(1)
                'Debug.Print("xmax " & XTimeMax.ToString & "  xmin " & XTimeMin.ToString)
                WhichDimension = Main.RPM1_ROLLER
                YAxisLabel = "RPM"
                For Count As Integer = 1 To UBound(y)
                    If y(Count) > RawDataMax Then RawDataMax = y(Count)
                    If y(Count) < RawDataMin Then RawDataMin = y(Count)
                    If FitData(Main.TORQUE_ROLLER, Count) > RawTDataMax Then RawTDataMax = FitData(Main.TORQUE_ROLLER, Count)
                    If FitData(Main.TORQUE_ROLLER, Count) < RawTDataMin Then RawTDataMin = FitData(Main.TORQUE_ROLLER, Count)
                    If FitData(Main.POWER, Count) > RawPDataMax Then RawPDataMax = FitData(Main.POWER, Count)
                    If FitData(Main.POWER, Count) < RawPDataMin Then RawPDataMin = FitData(Main.POWER, Count)
                Next
                'Debug.Print("Tdatmax " & RawTDataMax.ToString & " : Tdatamin " & RawTDataMin.ToString)
            Case Is = RUNDOWN
                XTimeMax = CoastDownX(UBound(CoastDownX))
                XTimeMin = CoastDownX(1)
                WhichDimension = Main.RPM1_ROLLER
                YAxisLabel = "RPM"
                For Count As Integer = 1 To UBound(CoastDownY)
                    If CoastDownY(Count) > RawDataMax Then RawDataMax = CoastDownY(Count)
                    If CoastDownY(Count) < RawDataMin Then RawDataMin = CoastDownY(Count)
                    If CoastDownT(Count) > RawTDataMax Then RawTDataMax = CoastDownT(Count)
                    If CoastDownT(Count) < RawTDataMin Then RawTDataMin = CoastDownT(Count)
                    If CoastDownP(Count) > RawPDataMax Then RawPDataMax = CoastDownP(Count)
                    If CoastDownP(Count) < RawPDataMin Then RawPDataMin = CoastDownP(Count)
                Next
            Case Is = CURRENT
                XTimeMax = x(UBound(x))
                XTimeMin = x(1)
                'XTimeMax = Ix(UBound(Ix))
                'XTimeMin = Ix(1)
                WhichDimension = Main.AMPS
                YAxisLabel = "Amps"
                For Count As Integer = 1 To UBound(FitData, 2)
                    If Main.CollectedData(WhichDimension, Count) > RawDataMax Then RawDataMax = Main.CollectedData(WhichDimension, Count)
                    If Main.CollectedData(WhichDimension, Count) < RawDataMin Then RawDataMin = Main.CollectedData(WhichDimension, Count)
                Next
            Case Is = VOLTAGE
                XTimeMax = x(UBound(x))
                XTimeMin = x(1)
                'XTimeMax = Vx(UBound(Vx))
                'XTimeMin = Vx(1)
                WhichDimension = Main.VOLTS
                YAxisLabel = "Volts"
                For Count As Integer = 1 To UBound(FitData, 2)
                    If Main.CollectedData(WhichDimension, Count) > RawDataMax Then RawDataMax = Main.CollectedData(WhichDimension, Count)
                    If Main.CollectedData(WhichDimension, Count) < RawDataMin Then RawDataMin = Main.CollectedData(WhichDimension, Count)
                Next
        End Select


        Dim TickLength As Integer = 5
        Dim TickString As String = ""
        Dim GraphicsFont As New Font("Arial", 10)
        Dim XTickInterval As Double = 0
        Dim YTickInterval As Double = 0

        With DataWindowBMP
            .Clear(Color.White)
            .DrawLine(AxesPen, CInt(XLeft), CInt(Ytop), CInt(XLeft), CInt(YBottom)) 'Draw Left Y Axis
            .DrawLine(AxesPen, CInt(XLeft), CInt(YBottom), CInt(XRight), CInt(YBottom)) 'Draw Bottom X Axis  
            XTickInterval = (XRight - XLeft) / 5
            YTickInterval = (YBottom - Ytop) / 5
            For Count As Integer = 0 To 4
                'Left Y ticks 
                .DrawLine(AxesPen, CInt(XLeft - TickLength), CInt(Ytop + (YTickInterval * Count)), CInt(XLeft), CInt(Ytop + (YTickInterval * Count)))
                TickString = Main.NewCustomFormat((((RawDataMax - RawDataMin) / 5) * (5 - Count) + RawDataMin) * Main.DataUnits(Main.RPM1_ROLLER, 1)) '.ToString(Main.CustomFormat((YRollerRPMAxis / 5) * (5 - Count)))
                .DrawString(TickString, GraphicsFont, AxisBrush, CInt(XLeft - .MeasureString(TickString, GraphicsFont).Width - TickLength), CInt(Ytop + (YTickInterval * Count) - .MeasureString(TickString, GraphicsFont).Height / 2))
                'Bottom X Ticks
                .DrawLine(AxesPen, CInt(XLeft + (XTickInterval * (Count + 1))), CInt(YBottom), CInt(XLeft + (XTickInterval * (Count + 1))), CInt(YBottom + TickLength))
                TickString = Main.NewCustomFormat((((XTimeMax - XTimeMin) / 5) * (Count + 1)) + XTimeMin) '.ToString(Main.CustomFormat(XTimeAxis / 5 * (Count + 1)))
                .DrawString(TickString, GraphicsFont, AxisBrush, CInt(XLeft + (XTickInterval * (Count + 1))) - .MeasureString(TickString, GraphicsFont).Width / 2, CInt(YBottom) + TickLength)
            Next
            'X-Axis Title
            .DrawString("Seconds", GraphicsFont, AxisBrush, CInt(PicDataWidth / 2) - .MeasureString("Seconds", GraphicsFont).Width / 2, CInt(YBottom) + TickLength * 2)
            'y-axis Title
            .DrawString(YAxisLabel, GraphicsFont, AxisBrush, CInt(XLeft) - .MeasureString(YAxisLabel, GraphicsFont).Width / 2, CInt(Ytop) - CInt(.MeasureString(YAxisLabel, GraphicsFont).Height * 1.5))

            .DrawString("RAW Data", GraphicsFont, Brushes.Green, 10, 5)
            .DrawString("FIT Data", GraphicsFont, Brushes.Red, 90, 5)
            .DrawString("Torque Curve (Max = " & Main.NewCustomFormat(RawTDataMax) & ")", GraphicsFont, TempTorqueDataBrush, 170, 5)
            .DrawString("Power Curve (Max = " & Main.NewCustomFormat(RawPDataMax) & ")", GraphicsFont, TempPowerDataBrush, 350, 5)


            'Draw Raw data
            If Main.DataPoints > 1 Then 'And blnfit Then

                Select Case WhichFitData
                    Case Is = RPM
                        For Count As Integer = 1 To UBound(y) - 1
                            .DrawLine(RawDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (y(Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (y(Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                            If blnRPMFit Then
                                .DrawLine(FitDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (fy(Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (fy(Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                                'CHECK - get rid of this try catch block when done
                                Try
                                    .DrawLine(TempTorqueDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.TORQUE_ROLLER, Count) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.TORQUE_ROLLER, Count + 1) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)))
                                Catch ex As Exception
                                    Stop
                                End Try
                                .DrawLine(TempPowerDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.POWER, Count) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.POWER, Count + 1) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)))
                            End If
                        Next
                    Case Is = RUNDOWN
                        For Count As Integer = 1 To UBound(CoastDownY) - 1
                            .DrawLine(RawDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownY(Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownY(Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                            If blnCoastDownDownFit Then
                                .DrawLine(FitDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownFY(Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownFY(Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                                .DrawLine(TempTorqueDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownT(Count) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownT(Count + 1) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)))
                                .DrawLine(TempPowerDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownP(Count) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownP(Count + 1) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)))
                            End If
                        Next
                    Case Is = VOLTAGE
                        For Count As Integer = 1 To UBound(y) - 1
                            .DrawLine(RawDataPen, CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (Main.CollectedData(WhichDimension, Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count + 1)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (Main.CollectedData(WhichDimension, Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                            If blnVoltageFit Then
                                .DrawLine(FitDataPen, CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (FitData(WhichDimension, Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count + 1)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (FitData(WhichDimension, Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                            End If
                        Next
                    Case Is = CURRENT
                        For Count As Integer = 1 To UBound(y) - 1
                            .DrawLine(RawDataPen, CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (Main.CollectedData(WhichDimension, Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count + 1)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (Main.CollectedData(WhichDimension, Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                            If blnCurrentFit Then
                                .DrawLine(FitDataPen, CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (FitData(WhichDimension, Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count + 1)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (FitData(WhichDimension, Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
                            End If
                        Next
                End Select

            End If

            'If Main.DataPoints > 1 And blnfit Then
            '    'For Count As Integer = 1 To UBound(fy) - 1 'UBound(FitData, 2) - 1
            '    Select Case WhichFitData
            '        Case Is = RPM
            '            For Count As Integer = 1 To UBound(y) - 1
            '                .DrawLine(FitDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (fy(Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (fy(Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
            '                .DrawLine(TempTorqueDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.TORQUE_ROLLER, Count) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.TORQUE_ROLLER, Count + 1) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)))
            '                .DrawLine(TempPowerDataPen, CInt(XLeft + ((x(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.POWER, Count) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)), CInt(XLeft + ((x(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (FitData(Main.POWER, Count + 1) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)))
            '            Next
            '        Case Is = RUNDOWN
            '            For Count As Integer = 1 To UBound(CoastDownY) - 1
            '                .DrawLine(FitDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownFY(Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownFY(Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
            '                .DrawLine(TempTorqueDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownT(Count) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownT(Count + 1) - RawTDataMin) / (RawTDataMax - RawTDataMin) * (YBottom - Ytop)))
            '                .DrawLine(TempPowerDataPen, CInt(XLeft + ((CoastDownX(Count) - XTimeMin) / (XTimeMax - XTimeMin)) * (XRight - XLeft)), CInt(YBottom - (CoastDownP(Count) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)), CInt(XLeft + ((CoastDownX(Count + 1) - XTimeMin)) / (XTimeMax - XTimeMin) * (XRight - XLeft)), CInt(YBottom - (CoastDownP(Count + 1) - RawPDataMin) / (RawPDataMax - RawPDataMin) * (YBottom - Ytop)))
            '            Next
            '        Case Is = VOLTAGE, CURRENT
            '            For Count As Integer = 1 To UBound(y) - 1
            '                .DrawLine(FitDataPen, CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (FitData(WhichDimension, Count) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)), CInt(XLeft + ((Main.CollectedData(Main.SESSIONTIME, Count + 1)) / XTimeMax) * (XRight - XLeft)), CInt(YBottom - (FitData(WhichDimension, Count + 1) * Main.DataUnits(WhichDimension, 0) - RawDataMin) / (RawDataMax - RawDataMin) * (YBottom - Ytop)))
            '            Next
            '    End Select
            'End If

        End With

        pnlDataWindow.BackgroundImage = DataBitMap
        pnlDataWindow.Invalidate()

    End Sub
#End Region
#Region "Mathematical Curve Fitting Section"
    Friend Sub ProcessData()
        Try
            Do 'Loop until the RPM data falls below the Power Run Threshold at which time WhichDataMode will go to LIVE
                Application.DoEvents()
                'Debug.Print(Main.WhichDataMode.ToString)
            Loop Until Main.WhichDataMode = Main.LIVE

            If Main.StopFitting = False Then

                blnfit = False
                blnRPMFit = False
                blnCoastDownDownFit = False
                blnVoltageFit = False
                blnCurrentFit = False

                Main.btnHide_Click(Me, System.EventArgs.Empty)
                Me.Show()


                '//////////////////////////////////////////////
                ' This section allows you to run a dummy fit if
                '   if LoadPowerRunData Const is 1
                '//////////////////////////////////////////////
#If LoadOldPowerRunData = 1 Then
                'If Main.GearRatio = 999.0 AndAlso inputdialog.FileName = "" Then
                setxyc() 'reads an existing data file for the raw RPM numbers
                'End If
#End If
                'Copy all collected data to fit data
                ReDim FitData(Main.LAST, Main.DataPoints)
                Dim t As Integer, count As Integer
                For t = 0 To Main.LAST - 1
                    For count = 1 To Main.DataPoints  '- FitStartPoint + 1
                        FitData(t, count) = Main.CollectedData(t, count + FitStartPoint - 1)
                    Next
                Next

                'Always Fit the RPM1 Data
                FitRPMData()

                If Main.frmCorrection.chkUseRunDown.Checked = True Then
                    FitRPMRunDownData()
                Else
                    blnCoastDownDownFit = True
                End If

                If rdoVoltage.Enabled = True Then
                    FitVoltage()
                Else
                    blnVoltageFit = True
                End If

                If rdoCurrent.Enabled = True Then
                    FitCurrent()
                Else
                    blnCurrentFit = True
                End If

                rdoRPM1.Checked = True
                rdoRPM1_CheckedChanged(Me, EventArgs.Empty)

            End If
            Main.RestartForms()
        Catch e As Exception
            MsgBox("ProcessData Error: " & e.ToString, MsgBoxStyle.Exclamation)
            End
        End Try
    End Sub
    Sub FitRPMData()

        Dim yoffset As Double, xoffset As Double, count As Integer, count2 As Integer, RawRPM1Max As Double

        ReDim y(Main.DataPoints)
        ReDim x(Main.DataPoints)

        'Flag to the code and to the user that we are fitting the data
        Main.ProcessingData = True
        Main.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        'x(0) = Main.CollectedData(Main.SESSIONTIME, FitStartPoint - 1)
        'y(0) = Main.CollectedData(Main.RPM1_ROLLER, FitStartPoint - 1)

        PowerRunSpikeLevel = CDbl(txtPowerRunSpikeLevel.Text)
        lblProgress.Text = "RPM1 Spike removal..."
        prgFit.Maximum = Main.DataPoints
        For count = FitStartPoint To Main.DataPoints
            prgFit.Value = count
            count2 += 1
            y(count2) = Main.CollectedData(Main.RPM1_ROLLER, count)
            x(count2) = Main.CollectedData(Main.SESSIONTIME, count)
            If Math.Abs(y(count2) - y(count2 - 1)) * Main.DataUnits(Main.RPM1_ROLLER, 1) > PowerRunSpikeLevel Then
                y(count2) = y(count2 - 1) ' Main.CollectedData(Main.RPM1_ROLLER, count - 1)
            Else
                If y(count2) > RawRPM1Max Then
                    MaxPosition = count2
                    RawRPM1Max = y(count2)
                End If
            End If
        Next
        prgFit.Value = prgFit.Maximum
        lblProgress.Text = "Done"
        'Then reset FitData to that size and copy all of the Collected data to that array.

        ReDim Preserve FitData(Main.LAST, MaxPosition) '- FitStartPoint + 1)
        ReDim Preserve y(MaxPosition) ' - FitStartPoint + 1)
        ReDim Preserve x(MaxPosition)
        ReDim fy(MaxPosition)
        'ReDim fx(MaxPosition)

        'the x and y value arrays sent to NonlinFit are working copies of the raw data
        'To fit using NonLin, x and y arrays need to start at the origin (0,0) -CHECK THIS DOES NOT SEEM TO BE TRUE FOR RUNDOWN
        'This means subtracting the first x and y values from all pairs
        'we need to remember the yoffset to add it back later.  We can apply this to all fits for simplicity
        yoffset = y(1) 'FitData(Main.RPM1_ROLLER, 1)
        'xoffset = x(1) 'the first time point should now always be zero so no need for offset removal'FitData(Main.SESSIONTIME, 1)

        For count = 1 To UBound(y) 'UBound(FitData, 2)
            'x(count) = x(count) - xoffset 'FitData(Main.SESSIONTIME, Count) - xoffset 'FitData(Main.SESSIONTIME, 1)
            y(count) = y(count) - yoffset 'CopyRPM1Data(Count) - yoffset 'FitData(Main.RPM1_ROLLER, count) - yoffset
        Next

        blnfit = False
        blnRPMFit = False
        WhichFitData = RPM
        pnlDataWindowSetup() 'Shows the user the data we are fitting.
        lblProgress.Text = "Fitting RPM1..."
        'DC 21JAN16 - Old code based on VBA source from Optimiz.xla
        'Note that the old code returns the function values for the fitted Y parameter
        'Call NonLin, sending the local copies of the data by ref
        'NonLin_fitting(x, y, fy, cmbWhichFit.SelectedIndex + 1, blnRPMFit)

        'DC 21JAN16 - New code based on the work of Rod Stephens - Used with permission of the Author
        'BestCoeffs is a generic list of type double and holds the coefficients of the polynomial solution
        'Points is a generic list of System.Drawing.PointF and is the raw X and Y data to be fit
        'Degree is an integer and represents the degree of the fit.
        'Original line is:
        ' BestCoeffs = FindPolynomialLeastSquaresFit(Points, degree)
        blnRPMFit = FindPolynomialLeastSquaresFit_NEW(x, y, fy, cmbWhichFit.SelectedIndex + 1)
        If blnRPMFit Then

            'fy() now contains the fit data. Copy it to FitData adding back the offsets
            'FitData(Main.RPM1_ROLLER, 1) = fy(1) + yoffset
            'FitData(Main.SESSIONTIME, 1) = x(1) + xoffset

            For count = 1 To UBound(y) 'UBound(FitData, 2)
                'x(count) = x(count) + xoffset
                y(count) = y(count) + yoffset
                'fx(count) = fx(count) + xoffset
                fy(count) = fy(count) + yoffset
                FitData(Main.SESSIONTIME, count) = x(count)
                FitData(Main.RPM1_ROLLER, count) = fy(count)
                'Setup power and torque temporarily
                FitData(Main.TORQUE_ROLLER, count) = (FitData(Main.RPM1_ROLLER, count) - FitData(Main.RPM1_ROLLER, count - 1)) / (FitData(Main.SESSIONTIME, count) - FitData(Main.SESSIONTIME, count - 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                FitData(Main.POWER, count) = FitData(Main.TORQUE_ROLLER, count) * FitData(Main.RPM1_ROLLER, count) ' + FitData(Main.RPM1_ROLLER, count - 1)) / 2)
            Next

            FitData(Main.TORQUE_ROLLER, 1) = 0 'fy(1) + yoffset
            FitData(Main.POWER, 1) = 0 'x(1) + xoffset

            If Main.frmCorrection.chkUseRunDown.Checked AndAlso blnCoastDownDownFit Then CreateCoastDownData()
            prgFit.Value = prgFit.Maximum
            lblProgress.Text = "Done"
            Me.Cursor = Cursors.Default
            Main.ProcessingData = False
            Main.Cursor = Cursors.Default
            blnfit = True
            pnlDataWindowSetup()
        Else
            'What are we going to do if the fit was not completed?
        End If
    End Sub
    Sub FitRPMRunDownData()

        Dim Count As Integer, Count2 As Integer, RawRPM1Max As Double

        If Main.frmCorrection.blnUsingLoadedRunDownFile = False Then

            lblUsingRunDownFile.Text = "No Coast Down File Loaded"

            Main.ProcessingData = True
            Main.Cursor = Cursors.WaitCursor
            Me.Cursor = Cursors.WaitCursor

            ReDim CoastDownX(Main.DataPoints)
            ReDim CoastDownY(Main.DataPoints)

            'CoastDownY(0) = Main.CollectedData(Main.RPM1_ROLLER, Main.DataPoints)
            'CoastDownX(0) = Main.CollectedData(Main.SESSIONTIME, Main.DataPoints)
            PowerRunSpikeLevel = CDbl(txtPowerRunSpikeLevel.Text)
            Count2 = 0
            RawRPM1Max = 0
            lblProgress.Text = "Coast Down Spike removal..."
            prgFit.Maximum = Main.DataPoints
            For Count = Main.DataPoints To 1 Step -1
                prgFit.Value = Count2 '
                Count2 += 1
                CoastDownY(Count2) = Main.CollectedData(Main.RPM1_ROLLER, Count)
                CoastDownX(Count2) = Main.CollectedData(Main.SESSIONTIME, Count)
                If Math.Abs(CoastDownY(Count2) - CoastDownY(Count2 - 1)) * Main.DataUnits(Main.RPM1_ROLLER, 1) > PowerRunSpikeLevel Then
                    CoastDownY(Count2) = CoastDownY(Count2 - 1)
                Else
                    If CoastDownY(Count2) > RawRPM1Max Then
                        MaxPosition = Count2
                        RawRPM1Max = CoastDownY(Count2)
                    End If
                End If
            Next
            prgFit.Value = prgFit.Maximum
            lblProgress.Text = "Done"
            'Then reset FitData to that size and copy all of the Collected data to that array.
            'ReDim FitData(Main.LAST, MaxPosition) '- FitStartPoint + 1) 'CHECK - DON'T MODIFY THIS AGAIN
            ReDim Preserve CoastDownY(MaxPosition + 1) ' - FitStartPoint + 1)
            ReDim Preserve CoastDownX(MaxPosition + 1)
            CoastDownY(MaxPosition + 1) = 0 'ensures that the 0 position is 0 when reverse
            CoastDownX(MaxPosition + 1) = 0
            ReDim CoastDownFY(MaxPosition)
            ReDim CoastDownP(MaxPosition)
            ReDim CoastDownT(MaxPosition)
            'ReDim fx(MaxPosition)

            'CHECK - This routine does not seem to need the offsets
            'Dim yoffset As Double, xoffset As Double, tempcopy(UBound(y)) As Double

            'Flag to the code and to the user that we are fitting the data

            lblProgress.Text = "Fitting Coast Down..."

            'x and y need to have their order reversed as they were pouplated from the end of the run to the top of the run down
            Array.Reverse(CoastDownX) ', ', 1, UBound(CoastDownX))
            Array.Reverse(CoastDownY) ', 1, UBound(CoastDownY))

            ReDim Preserve CoastDownY(MaxPosition) ' - FitStartPoint + 1)
            ReDim Preserve CoastDownX(MaxPosition)

            blnfit = False
            blnCoastDownDownFit = False
            WhichFitData = RUNDOWN
            pnlDataWindowSetup() 'Shows the user the data we are fitting.

            'Call NonLin, sending the local copies of the data by ref
            NonLin_fitting(CoastDownX, CoastDownY, CoastDownFY, cmbWhichRDFit.SelectedIndex + 1, blnCoastDownDownFit)
            If blnCoastDownDownFit Then
                'Calc the rundown torque and power
                'Note these are based on RPM and not time which means we have to find the closest RPM in the power up section
                'This is going to be a pain.
                prgFit.Value = prgFit.Maximum
                lblProgress.Text = "Done"
                Me.Cursor = Cursors.Default
                Main.ProcessingData = False
                Main.Cursor = Cursors.Default
                FitData(Main.RPM1_ROLLER, 0) = FitData(Main.RPM1_ROLLER, 1)
                CoastDownFY(0) = CoastDownFY(1)
                'Dim Difference As Double, MinDifference As Double, MinDifferenceIndex As Integer
                'Need to find the closest value
                'First need to find the highestt run down fit poin
                'Dim MaxRunDownRPM As Double
                For Count = 1 To UBound(CoastDownFY)
                    If CoastDownFY(Count) > MaxRunDownRPM Then MaxRunDownRPM = CoastDownFY(Count)
                    CoastDownT(Count) = -1 * (CoastDownFY(Count) - CoastDownFY(Count - 1)) / (CoastDownX(Count) - CoastDownX(Count - 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                    CoastDownP(Count) = CoastDownT(Count) * CoastDownFY(Count) ' + CoastDownFY(Count - 1)) / 2)
                    'Debug.Print(CoastDownX(Count) & " " & CoastDownFY(Count) & " " & CoastDownT(Count) & " " & CoastDownP(Count))
                Next
                CreateCoastDownData()
                'MsgBox(MaxRunDownRPM.ToString, vbOKOnly)
                'This section should be in its own routine as it needs to be called if the RPM1 data is refit.
                'Also, if the spike threshold is changed, both RPM1 and Coast Down need to be refit.
                'For Count = 1 To UBound(FitData, 2)
                '    If FitData(Main.RPM1_ROLLER, Count) >= MaxRunDownRPM Then
                '        FitData(Main.TORQUE_COASTDOWN, Count) = -1 * (CoastDownFY(2) - CoastDownFY(1)) / (CoastDownX(2) - CoastDownX(1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                '    Else
                '        MinDifference = 999999
                '        For Count2 = 1 To UBound(CoastDownFY)
                '            Difference = Math.Abs(CoastDownFY(Count2) - FitData(Main.RPM1_ROLLER, Count))
                '            If Difference < MinDifference Then
                '                MinDifference = Difference
                '                MinDifferenceIndex = Count2
                '            End If
                '        Next
                '        FitData(Main.TORQUE_COASTDOWN, Count) = -1 * (CoastDownFY(MinDifferenceIndex) - CoastDownFY(MinDifferenceIndex + 1)) / (CoastDownX(MinDifferenceIndex) - CoastDownX(MinDifferenceIndex + 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                '    End If
                '    FitData(Main.POWER_COASTDOWN, Count) = FitData(Main.TORQUE_COASTDOWN, Count) * ((FitData(Main.RPM1_ROLLER, Count) + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
                'Next
                blnfit = True
                pnlDataWindowSetup()
            Else
                'what are we doing if the fit didn;t complete
                MsgBox("Skipped Fitdata", vbOKOnly)
            End If

        Else
            'read in the selected coast down file
            Dim CoastDownInputStream As StreamReader, temp As String, tempsplit() As String, NumberOfPoints As Integer
            CoastDownInputStream = New StreamReader(Main.frmCorrection.RunDownOpenFileDialog.FileName)
            lblUsingRunDownFile.Text = Main.frmCorrection.RunDownOpenFileDialog.FileName.Substring(Main.frmCorrection.RunDownOpenFileDialog.FileName.LastIndexOf("\") + 1)
            With CoastDownInputStream
                Do
                    temp = .ReadLine
                Loop Until temp.StartsWith("NUMBER_OF_COAST_DOWN_POINTS_FIT")
                NumberOfPoints = CInt(temp.Substring(temp.LastIndexOf(" ")))
                ReDim CoastDownX(NumberOfPoints), CoastDownFY(NumberOfPoints)
                temp = .ReadLine 'titles
                For Count = 1 To NumberOfPoints
                    temp = .ReadLine
                    tempsplit = Split(temp, " ")
                    CoastDownX(Count) = CDbl(tempsplit(0))
                    CoastDownFY(Count) = CDbl(tempsplit(1))
                Next
            End With
            FitData(Main.RPM1_ROLLER, 0) = FitData(Main.RPM1_ROLLER, 1)
            'Dim Difference As Double, MinDifference As Double, MinDifferenceIndex As Integer
            'Need to find the closest value
            'First need to find the highestt run down fit poin
            'Dim MaxRunDownRPM As Double
            For Count = 1 To UBound(CoastDownFY)
                If CoastDownFY(Count) > MaxRunDownRPM Then MaxRunDownRPM = CoastDownFY(Count)
            Next
            CreateCoastDownData()
            'For Count = 1 To UBound(FitData, 2)
            '    If FitData(Main.RPM1_ROLLER, Count) >= MaxRunDownRPM Then
            '        FitData(Main.TORQUE_COASTDOWN, Count) = -1 * (CoastDownFY(2) - CoastDownFY(1)) / (CoastDownX(2) - CoastDownX(1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
            '    Else
            '        MinDifference = 999999
            '        For Count2 = 1 To UBound(CoastDownFY)
            '            Difference = Math.Abs(CoastDownFY(Count2) - FitData(Main.RPM1_ROLLER, Count))
            '            If Difference < MinDifference Then
            '                MinDifference = Difference
            '                MinDifferenceIndex = Count2
            '            End If
            '        Next
            '        FitData(Main.TORQUE_COASTDOWN, Count) = -1 * (CoastDownFY(MinDifferenceIndex) - CoastDownFY(MinDifferenceIndex + 1)) / (CoastDownX(MinDifferenceIndex) - CoastDownX(MinDifferenceIndex + 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
            '    End If
            '    FitData(Main.POWER_COASTDOWN, Count) = FitData(Main.TORQUE_COASTDOWN, Count) * ((FitData(Main.RPM1_ROLLER, Count) + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
            'Next
            blnCoastDownDownFit = True
        End If



    End Sub
    Sub CreateCoastDownData()
        'this section needs to be independent as it changes if either RPM1 or Coast Down fits change
        Dim Count As Integer, Count2 As Integer, MinDifference As Double, difference As Double, MinDifferenceIndex As Integer
        For Count = 1 To UBound(FitData, 2)
            If FitData(Main.RPM1_ROLLER, Count) >= MaxRunDownRPM Then
                FitData(Main.TORQUE_COASTDOWN, Count) = -1 * (CoastDownFY(2) - CoastDownFY(1)) / (CoastDownX(2) - CoastDownX(1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
            Else
                MinDifference = 999999
                For Count2 = 1 To UBound(CoastDownFY) - 1
                    difference = Math.Abs(CoastDownFY(Count2) - FitData(Main.RPM1_ROLLER, Count))
                    If difference < MinDifference Then
                        MinDifference = difference
                        MinDifferenceIndex = Count2
                    End If
                Next
                Try
                    FitData(Main.TORQUE_COASTDOWN, Count) = -1 * (CoastDownFY(MinDifferenceIndex) - CoastDownFY(MinDifferenceIndex + 1)) / (CoastDownX(MinDifferenceIndex) - CoastDownX(MinDifferenceIndex + 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                Catch ex As Exception
                    Stop
                End Try

            End If
            FitData(Main.POWER_COASTDOWN, Count) = FitData(Main.TORQUE_COASTDOWN, Count) * FitData(Main.RPM1_ROLLER, Count) ' + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
        Next
    End Sub
    Sub FitVoltage()
        Main.ProcessingData = True
        Main.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor
        lblProgress.Text = "Smoothing voltage..."
        VoltageSmooth = (scrlVoltageSmooth.Maximum + 1 - scrlVoltageSmooth.Value) / 2
        Dim Count As Integer
        ReDim Vx(UBound(FitData, 2)), Vy(UBound(FitData, 2)), Vfy(UBound(FitData, 2))
        'Voltage
        For Count = 1 To UBound(FitData, 2)
            Vy(Count) = Main.CollectedData(Main.VOLTS, Count + FitStartPoint - 1)
        Next
        blnVoltageFit = False
        MovingAverageSmooth(Vy, Vfy, VoltageSmooth) 'Last Number is the smoothing window in %
        blnVoltageFit = True

        For Count = 1 To UBound(FitData, 2)
            FitData(Main.VOLTS, Count) = Vfy(Count)
        Next

        'WritePowerFile()

        Me.Cursor = Cursors.Default
        Main.ProcessingData = False
        Main.Cursor = Cursors.Default
        lblProgress.Text = "Done"
        blnfit = True
        pnlDataWindowSetup()
    End Sub
    Sub FitCurrent()
        Main.ProcessingData = True
        Main.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor
        lblProgress.Text = "Smoothing current..."
        CurrentSmooth = (scrlCurrentSmooth.Maximum + 1 - scrlCurrentSmooth.Value) / 2
        Dim Count As Integer
        ReDim Ix(UBound(FitData, 2)), Iy(UBound(FitData, 2)), Ify(UBound(FitData, 2))
        'Current
        For Count = 1 To UBound(FitData, 2)
            Iy(Count) = Main.CollectedData(Main.AMPS, Count + FitStartPoint - 1)
        Next
        blnCurrentFit = False
        MovingAverageSmooth(Iy, Ify, CurrentSmooth) 'Last Number is the smoothing window in %
        blnCurrentFit = True
        For Count = 1 To UBound(FitData, 2)
            FitData(Main.AMPS, Count) = Ify(Count)
        Next

        'WritePowerFile()

        Me.Cursor = Cursors.Default
        Main.ProcessingData = False
        Main.Cursor = Cursors.Default
        lblProgress.Text = "Done"
        blnfit = True
        pnlDataWindowSetup()
    End Sub
    Sub WritePowerFile()
        Dim DataOutputFile As New System.IO.StreamWriter(Main.LogPowerRunDataFileName)
        Dim Count As Integer

        With DataOutputFile
            'NOTE: The data files are space delimited
            'Write out the header information - CHECK - this needs to align with the V5 / V6 conversion and read in by overlay section
            'CHECK - this needs to be updated per version
            .WriteLine(Main.PowerRunVersion) 'Confirms power curve and version
            .WriteLine(Main.LogPowerRunDataFileName & vbCrLf & DateAndTime.Today & vbCrLf)
            .WriteLine("Acquisition: " & Main.cmbAcquisition.SelectedItem.ToString)
            .WriteLine("Number_of_Channels: " & Main.NUMBER_OF_CHANNELS.ToString)
            .WriteLine("Sampling_Rate " & Main.SAMPLE_RATE.ToString)
            If Main.cmbCOMPorts.SelectedItem IsNot Nothing Then
                .WriteLine("COM_Port: " & Main.cmbCOMPorts.SelectedItem.ToString)
            Else
                .WriteLine("No_COM_Port_Selected")
            End If
            If Main.cmbBaudRate.SelectedItem IsNot Nothing Then
                .WriteLine("Baud_Rate: " & Main.cmbBaudRate.SelectedItem.ToString)
            Else
                .WriteLine("No_Baud_Rate_Selected")
            End If
            .WriteLine("Car_Mass: " & Main.frmDyno.CarMass.ToString & " grams")
            .WriteLine("Frontal_Area: " & Main.frmDyno.FrontalArea.ToString & " mm2")
            .WriteLine("Drag_Coefficient: " & Main.frmDyno.DragCoefficient.ToString)
            .WriteLine("Gear_Ratio: " & Main.GearRatio.ToString)
            .WriteLine("Wheel_Diameter: " & Main.frmDyno.WheelDiameter.ToString & " mm")
            .WriteLine("Roller_Diameter: " & Main.frmDyno.RollerDiameter.ToString & " mm")
            .WriteLine("Roller_Wall_Thickness: " & Main.frmDyno.RollerWallThickness.ToString & " mm")
            .WriteLine("Roller_Mass: " & Main.frmDyno.RollerMass.ToString & " grams")
            .WriteLine("Axle_Diameter: " & Main.frmDyno.AxleDiameter.ToString & " mm")
            .WriteLine("Axle_Mass: " & Main.frmDyno.AxleMass.ToString & " grams")
            .WriteLine("End_Cap_Mass: " & Main.frmDyno.EndCapMass.ToString & " grams")
            .WriteLine("Extra_Diameter: " & Main.frmDyno.ExtraDiameter.ToString & " mm")
            .WriteLine("Extra_Wall_Thickness: " & Main.frmDyno.ExtraWallThickness.ToString & " mm")
            .WriteLine("Extra_Mass: " & Main.frmDyno.ExtraMass.ToString & " grams")
            .WriteLine("Target_MOI: " & Main.IdealMomentOfInertia.ToString & " kg/m2")
            .WriteLine("Actual_MOI: " & Main.DynoMomentOfInertia.ToString & " kg/m2")
            .WriteLine("Target_Roller_Mass: " & Main.IdealRollerMass.ToString & " grams")
            .WriteLine("Signals_Per_RPM1: " & Main.frmDyno.SignalsPerRPM.ToString)
            .WriteLine("Signals_Per_RPM2: " & Main.frmDyno.SignalsPerRPM2.ToString)
            .WriteLine("Channel_1_Threshold: " & Main.HighSignalThreshold.ToString)
            .WriteLine("Channel_2_Threshold: " & Main.HighSignalThreshold2.ToString)
            .WriteLine("Run_RPM_Threshold: " & Main.PowerRunThreshold.ToString)
            .WriteLine("Run_Spike_Removal_Threshold: " & PowerRunSpikeLevel.ToString)
            .WriteLine("Curve_Fit_Model: " & cmbWhichFit.SelectedItem.ToString)
            .WriteLine("Coast_Down?_Roller?_Wheel?_Drivetrain?: " & rdoRunDown.Enabled.ToString & " " & Main.frmCorrection.rdoFreeRoller.Checked.ToString & " " & Main.frmCorrection.rdoRollerAndWheel.Checked.ToString & " " & Main.frmCorrection.rdoRollerAndDrivetrain.Checked.ToString)
            If Main.frmCorrection.blnUsingLoadedRunDownFile = True Then
                .WriteLine("Coast_Down_File_Loaded: " & Main.frmCorrection.RunDownOpenFileDialog.FileName)
            Else
                .WriteLine("Coast_Down_Fit_Model: " & cmbWhichRDFit.SelectedItem.ToString)
            End If

            .WriteLine("Voltage_Smoothing_%: " & txtVoltageSmooth.Text)
            .WriteLine("Current_Smoothing_%: " & txtCurrentSmooth.Text)
            .WriteLine(vbCrLf)
            .WriteLine("PRIMARY_CHANNEL_CURVE_FIT_DATA")
            .WriteLine("NUMBER_OF_POINTS_FIT" & " " & UBound(FitData, 2).ToString)
            .WriteLine("STARTING_POINT" & " " & FitStartPoint.ToString)

            'Create the column headings string based on the Data structure 
            'Only Primary SI units of the values are written
            Dim tempstring As String = ""
            Dim tempsplit As String()
            Dim paramcount As Integer
            For paramcount = 0 To Main.LAST - 1
                tempsplit = Split(Main.DataUnitTags(paramcount), " ")
                tempstring = tempstring & Main.DataTags(paramcount).Replace(" ", "_") & "_(" & tempsplit(0) & ") "
            Next
            'Write the column headings
            .WriteLine(tempstring)
            'Need to set the zeroth value to support using the count and count-1 approach to torque and power calculations
            FitData(Main.RPM1_ROLLER, 0) = FitData(Main.RPM1_ROLLER, 1)
            'Reset Maxima
            'YMax = 0 ': Pmax = 0 : Tmax = 0

            'Process the results for the fit data
            For Count = 1 To UBound(FitData, 2)
                'Keep track of the max value for plotting purposes
                FitData(Main.LAST, Count) = FitData(Main.RPM1_ROLLER, Count) - Main.CollectedData(Main.RPM1_ROLLER, Count)
                'If FitData(Main.RPM1_ROLLER, count) > YMax Then YMax = FitData(Main.RPM1_ROLLER, count) 'for scaling the axis when we show the fit
                'update wheel and motor RPM
                FitData(Main.RPM1_WHEEL, Count) = FitData(Main.RPM1_ROLLER, Count) * Main.RollerRPMtoWheelRPM
                FitData(Main.RPM1_MOTOR, Count) = FitData(Main.RPM1_ROLLER, Count) * Main.RollerRPMtoMotorRPM
                'update speed and drag
                FitData(Main.SPEED, Count) = FitData(Main.RPM1_ROLLER, Count) * Main.RollerRadsPerSecToMetersPerSec
                FitData(Main.DRAG, Count) = FitData(Main.SPEED, Count) ^ 3 * Main.ForceAir
                're-calc roller torque and power using the Fit data
                ' FitData(Main.TORQUE_ROLLER, Count) = (FitData(Main.RPM1_ROLLER, Count) - FitData(Main.RPM1_ROLLER, Count - 1)) / (FitData(Main.SESSIONTIME, Count) - FitData(Main.SESSIONTIME, Count - 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                'NOTE - new power calculation uses (new-old) / 2  - REMOVED 06DEC13
                'FitData(Main.POWER, Count) = FitData(Main.TORQUE_ROLLER, Count) * ((FitData(Main.RPM1_ROLLER, Count) + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
                FitData(Main.TORQUE_WHEEL, Count) = FitData(Main.POWER, Count) / FitData(Main.RPM1_WHEEL, Count)
                FitData(Main.TORQUE_MOTOR, Count) = FitData(Main.POWER, Count) / FitData(Main.RPM1_MOTOR, Count)
                'Calculated Corrected values for power and torque if use rundown is selected
                If Main.frmCorrection.chkUseRunDown.Checked Then
                    'Select how the coast down values are to be applied
                    'If its a freeroller rundown, just add torque to the roller torque and continue as usual.
                    If Main.frmCorrection.rdoFreeRoller.Checked Then
                        FitData(Main.CORRECTED_TORQUE_ROLLER, Count) = FitData(Main.TORQUE_ROLLER, Count) + FitData(Main.TORQUE_COASTDOWN, Count)
                        FitData(Main.CORRECTED_POWER_ROLLER, Count) = FitData(Main.CORRECTED_TORQUE_ROLLER, Count) * ((FitData(Main.RPM1_ROLLER, Count) + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
                        FitData(Main.CORRECTED_POWER_WHEEL, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count)
                        FitData(Main.CORRECTED_POWER_MOTOR, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count)
                        FitData(Main.CORRECTED_TORQUE_WHEEL, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count) / FitData(Main.RPM1_WHEEL, Count)
                        FitData(Main.CORRECTED_TORQUE_MOTOR, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count) / FitData(Main.RPM1_MOTOR, Count)
                    End If
                    'if its a roller plus wheel leave roller alone, adjust wheel and back calc to motor
                    If Main.frmCorrection.rdoRollerAndWheel.Checked Then
                        'Add corrected torque to wheel
                        FitData(Main.CORRECTED_TORQUE_WHEEL, Count) = FitData(Main.TORQUE_ROLLER, Count) + FitData(Main.TORQUE_COASTDOWN, Count)
                        FitData(Main.CORRECTED_POWER_WHEEL, Count) = FitData(Main.CORRECTED_TORQUE_WHEEL, Count) * ((FitData(Main.RPM1_ROLLER, Count) + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
                        'Now set the motor torque and power
                        FitData(Main.CORRECTED_POWER_MOTOR, Count) = FitData(Main.CORRECTED_POWER_WHEEL, Count)
                        FitData(Main.CORRECTED_TORQUE_MOTOR, Count) = FitData(Main.CORRECTED_POWER_WHEEL, Count) / FitData(Main.RPM1_MOTOR, Count)
                        'Now reset the wheel and roller torques to uncorrected
                        FitData(Main.CORRECTED_TORQUE_ROLLER, Count) = FitData(Main.TORQUE_ROLLER, Count)
                        FitData(Main.CORRECTED_POWER_ROLLER, Count) = FitData(Main.POWER, Count)
                    End If
                    'if its a roller plus drive train leave roller and wheel alone and apply only to motor
                    If Main.frmCorrection.rdoRollerAndDrivetrain.Checked Then
                        'temporarily add torque to roller
                        FitData(Main.CORRECTED_TORQUE_ROLLER, Count) = FitData(Main.TORQUE_ROLLER, Count) + FitData(Main.TORQUE_COASTDOWN, Count)
                        FitData(Main.CORRECTED_POWER_ROLLER, Count) = FitData(Main.CORRECTED_TORQUE_ROLLER, Count) * ((FitData(Main.RPM1_ROLLER, Count) + FitData(Main.RPM1_ROLLER, Count - 1)) / 2)
                        'Now set the motor torque and power
                        FitData(Main.CORRECTED_POWER_MOTOR, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count)
                        FitData(Main.CORRECTED_TORQUE_MOTOR, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count) / FitData(Main.RPM1_MOTOR, Count)
                        'Now reset the wheel and roller torques to uncorrected
                        FitData(Main.CORRECTED_TORQUE_ROLLER, Count) = FitData(Main.TORQUE_ROLLER, Count)
                        FitData(Main.CORRECTED_POWER_ROLLER, Count) = FitData(Main.POWER, Count)
                        FitData(Main.CORRECTED_POWER_WHEEL, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count)
                        FitData(Main.CORRECTED_TORQUE_WHEEL, Count) = FitData(Main.CORRECTED_POWER_ROLLER, Count) / FitData(Main.RPM1_WHEEL, Count)
                    End If
                End If

                'Update other parameters requiring calculations
                'RPM2 will be already there but the ratio and rollout need to be calculated
                If FitData(Main.RPM2, Count) <> 0 Then
                    FitData(Main.RPM2_RATIO, Count) = FitData(Main.RPM2, Count) / FitData(Main.RPM1_WHEEL, Count)
                    FitData(Main.RPM2_ROLLOUT, Count) = Main.WheelCircumference / FitData(Main.RPM2_RATIO, Count)
                Else
                    FitData(Main.RPM2_RATIO, Count) = 0
                    FitData(Main.RPM2_ROLLOUT, Count) = 0
                End If
                'Volts and Amps will already be there but watts in and efficiency need to be added
                FitData(Main.WATTS_IN, Count) = FitData(Main.VOLTS, Count) * FitData(Main.AMPS, Count)
                If FitData(Main.WATTS_IN, Count) <> 0 Then
                    FitData(Main.EFFICIENCY, Count) = FitData(Main.POWER, Count) / FitData(Main.WATTS_IN, Count) * 100
                    FitData(Main.CORRECTED_EFFICIENCY, Count) = FitData(Main.CORRECTED_POWER_MOTOR, Count) / FitData(Main.WATTS_IN, Count) * 100
                Else
                    FitData(Main.EFFICIENCY, Count) = 0
                    FitData(Main.CORRECTED_EFFICIENCY, Count) = 0
                End If
                'Write the data file based on the FitData structure.
                'Build the results string...
                tempstring = "" 'count.ToString & " "
                For paramcount = 0 To Main.LAST - 1
                    tempsplit = Split(Main.DataUnitTags(paramcount), " ") ' How many units are there
                    tempstring = tempstring & FitData(paramcount, Count) * Main.DataUnits(paramcount, 0) & " " 'DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(unitcount) & ") "
                Next
                '...and write it
                .WriteLine(tempstring)

            Next
            'Add the coast down data if it was recorded
            .WriteLine(vbCrLf & "FULL_SET_COAST_DOWN_FIT_DATA")
            .WriteLine("NUMBER_OF_COAST_DOWN_POINTS_FIT " & UBound(CoastDownFY))
            .WriteLine("Time_(Sec) RPM1_Roller_(rad/s)")
            If UBound(CoastDownFY) > 1 Then
                For Count = 1 To UBound(CoastDownFY)
                    .WriteLine(CoastDownX(Count) & " " & CoastDownFY(Count))
                Next
            End If

            'Add the raw data.  In V6 we are also calculating the raw torques, powers etc. This makes the file larger but will make Excel work easier
            .WriteLine(vbCrLf & "PRIMARY_CHANNEL_RAW_DATA")
            .WriteLine("NUMBER_OF_POINTS_COLLECTED" & " " & Main.DataPoints.ToString)
            'Again, create the header row
            tempstring = ""
            For paramcount = 0 To Main.LAST - 1 'CHECK - time is now the last column which will mess up the overlay routine .
                tempsplit = Split(Main.DataUnitTags(paramcount), " ")
                tempstring = tempstring & Main.DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(0) & ") "
            Next
            'Write the column headings
            .WriteLine(tempstring)
            'Need to set the zeroth value to support using the count and count-1 approach to torque and power calculations
            Main.CollectedData(Main.RPM1_ROLLER, 0) = Main.CollectedData(Main.RPM1_ROLLER, 1)
            For Count = 1 To Main.DataPoints
                're-calc speed, wheel and motor RPMs based on collected data
                Main.CollectedData(Main.SPEED, Count) = Main.CollectedData(Main.RPM1_ROLLER, Count) * Main.RollerRadsPerSecToMetersPerSec
                Main.CollectedData(Main.RPM1_WHEEL, Count) = Main.CollectedData(Main.RPM1_ROLLER, Count) * Main.RollerRPMtoWheelRPM
                Main.CollectedData(Main.RPM1_MOTOR, Count) = Main.CollectedData(Main.RPM1_ROLLER, Count) * Main.RollerRPMtoMotorRPM
                're-calc roller torque and power useing the collected data
                Main.CollectedData(Main.TORQUE_ROLLER, Count) = (Main.CollectedData(Main.RPM1_ROLLER, Count) - Main.CollectedData(Main.RPM1_ROLLER, Count - 1)) / (Main.CollectedData(Main.SESSIONTIME, Count) - Main.CollectedData(Main.SESSIONTIME, Count - 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                'NOTE - new power calculation uses (new-old) / 2
                Main.CollectedData(Main.POWER, Count) = Main.CollectedData(Main.TORQUE_ROLLER, Count) * ((Main.CollectedData(Main.RPM1_ROLLER, Count) + Main.CollectedData(Main.RPM1_ROLLER, Count - 1)) / 2)
                'now re-calc wheel and motor torque based on Power
                Main.CollectedData(Main.TORQUE_WHEEL, Count) = Main.CollectedData(Main.POWER, Count) / Main.CollectedData(Main.RPM1_WHEEL, Count)
                Main.CollectedData(Main.TORQUE_MOTOR, Count) = Main.CollectedData(Main.POWER, Count) / Main.CollectedData(Main.RPM1_MOTOR, Count)
                'recalc Drag and set a max speed based on it
                Main.CollectedData(Main.DRAG, Count) = Main.CollectedData(Main.SPEED, Count) ^ 3 * Main.ForceAir
                'Update other parameters requiring calculations
                'Main.RPM2 will be already there but the ratio and rollout need to be calculated
                If Main.CollectedData(Main.RPM2, Count) <> 0 Then
                    Main.CollectedData(Main.RPM2_RATIO, Count) = Main.CollectedData(Main.RPM2, Count) / Main.CollectedData(Main.RPM1_WHEEL, Count)
                    Main.CollectedData(Main.RPM2_ROLLOUT, Count) = Main.WheelCircumference / Main.CollectedData(Main.RPM2_RATIO, Count)
                Else
                    Main.CollectedData(Main.RPM2_RATIO, Count) = 0
                    Main.CollectedData(Main.RPM2_ROLLOUT, Count) = 0
                End If
                'Volts and Amps will already be there but watts in and efficiency need to be added
                Main.CollectedData(Main.WATTS_IN, Count) = Main.CollectedData(Main.VOLTS, Count) * Main.CollectedData(Main.AMPS, Count)
                If Main.CollectedData(Main.WATTS_IN, Count) <> 0 Then
                    Main.CollectedData(Main.EFFICIENCY, Count) = Main.CollectedData(Main.POWER, Count) / Main.CollectedData(Main.WATTS_IN, Count) * 100
                Else
                    Main.CollectedData(Main.EFFICIENCY, Count) = 0
                End If
                'Build the results string...
                tempstring = ""
                For paramcount = 0 To Main.LAST - 1 'CHECK - time is now the last column which will mess up the overlay routine .
                    tempsplit = Split(Main.DataUnitTags(paramcount), " ") ' How many units are there
                    tempstring = tempstring & Main.CollectedData(paramcount, Count) * Main.DataUnits(paramcount, 0) & " " 'DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(unitcount) & ") "
                Next
                '...and write it
                .WriteLine(tempstring)

            Next

        End With
        'Save the file
        DataOutputFile.Close()
    End Sub
    Sub MovingAverageSmooth(ByRef SentY() As Double, ByRef SentFY() As Double, ByVal WindowPercent As Double)
        Dim n As Long, w As Integer, t As Long, s As Long
        Dim temp As Double
        n = 0 'reset window width
        w = CInt(Int(UBound(SentY) / 100 * WindowPercent))
        For t = 1 To UBound(SentY)  'top to bottom of data
            prgFit.Value = CInt(prgFit.Maximum / UBound(SentY) * t)
            For s = t - n To t + n
                temp = temp + SentY(CInt(s))
            Next s
            SentFY(CInt(t)) = temp / (n * 2 + 1)
            temp = 0
            If n <> w And UBound(SentY) - t > w Then
                n = n + 1
            ElseIf UBound(SentY) - t <= w Then
                n = n - 1
            End If
        Next t
    End Sub
    Sub NonLin_fitting(ByRef SentX() As Double, ByRef SentY() As Double, ByRef SentFY() As Double, ByVal SentCurveChoice As Integer, ByRef blnFitFinished As Boolean)

        Dim ParameterSetSize As Integer, NewC As Double

        Select Case SentCurveChoice
            Case Is = 1 'Four parameter fit
                'Set the number of parameters to be fit 
                ParameterSetSize = 4
                ReDim c(ParameterSetSize)
                NewC = 1
                'Resize and initialize the equation parameters

                c(1) = 1 : c(2) = NewC : c(3) = SentY(UBound(SentY)) : c(4) = NewC
                Do
                    Application.DoEvents()
                    'Perform the NL-Rregressio with LM algorithm
                    Call LMNoLinearFit(SentX, SentY, SentFY, c, 1)
                    'Check that we have valid results.  
                    'If not, change initial conditions and re-try
                    If Double.IsNaN(c(1)) = True Then
                        blnfit = False
                        If NewC > 10000 Then 'CHECK - why is this here, if NewC > 10000 then this simply resets all parameters and starts again?
                            NewC = 1
                            c(1) = 1
                            c(2) = NewC
                            c(3) = SentY(UBound(SentY))
                            c(4) = NewC
                        Else
                            NewC = NewC * 5
                            c(1) = 1
                            c(2) = NewC
                            c(3) = SentY(UBound(SentY))
                            c(4) = NewC
                        End If
                    Else
                        'Flag that the fit has worked
                        blnfit = True
                        blnFitFinished = True
                    End If
                Loop Until blnfit Or blnFitFinished Or Main.StopFitting
            Case Is = 2, 3, 4, 5
                'Resize and initialize the equation parameters based on sentcurve choice
                ReDim c(SentCurveChoice + 1)
                Application.DoEvents()
                'Perform the NL-Rregressio with LM algorithm
                Call LMNoLinearFit(SentX, SentY, SentFY, c, SentCurveChoice + 1)
                'Check that we have valid results.  
                blnfit = True
                blnFitFinished = True
            Case Is = 6 'Test algorithm(s)
                If rdoRPM1.Checked Then
                    lblProgress.Text = "Smoothing RPM1..."
                    RPM1Smooth = (scrlRPM1Smooth.Maximum + 1 - scrlRPM1Smooth.Value) / 2
                    MovingAverageSmooth(SentY, SentFY, RPM1Smooth)
                    blnfit = True
                    blnFitFinished = True
                End If
                If rdoRunDown.Checked Then
                    lblProgress.Text = "Smoothing Coast Down..."
                    CoastDownSmooth = (scrlCoastDownSmooth.Maximum + 1 - scrlCoastDownSmooth.Value) / 2
                    MovingAverageSmooth(SentY, SentFY, CoastDownSmooth)
                    blnfit = True
                    blnFitFinished = True
                End If
        End Select
    End Sub
    Sub LMNoLinearFit(ByRef x() As Double, ByRef y() As Double, ByRef fy() As Double, ByRef c() As Double, ByVal WhichCurve As Integer)
        ' ver. 14.04.2006 by Luis Isaac Ramos Garcia and Foxes Team

        Dim i As Integer, k As Integer, kmax As Integer
        Dim Mcoef(,) As Double, det As Double, tpc() As Double
        Dim Diffc() As Double       ' Vector de diferecias entre antiguos c y nuevos
        Dim delta As Double         ' Valor delta de algoritmo Levenberg-Marquardt
        Dim tdelta As Double        ' relative increment
        Dim mu As Double            ' reduction/amplification quadratic error
        Dim ch20 As Double          ' quadratic error previous step
        Dim delta0 As Double        ' increment previous step
        Dim tol As Double           ' machine tolerance
        Dim fact As Double          ' step factor
        Dim itmax As Integer
        Dim niter As Integer
        Dim ch2 As Double

        ReDim Diffc(UBound(c)), tpc(UBound(c)) ' was tpc(LBound(c) To UBound(c))
        delta = 0.05  '0.1%
        k = 0
        niter = 0
        itmax = 1000
        fact = 10
        tol = 2 * 10 ^ -16
        kmax = CInt(itmax / 4)
        ch20 = chi2(x, y, c, WhichCurve)

        'Dim temp As Integer, tempstring As String

        prgFit.Maximum = CInt(kmax + itmax)
        Do While k < kmax And niter < itmax And Main.StopFitting = False 'VL. 30/10/2004
            prgFit.Value = k + niter
            Application.DoEvents()
            Call CalculateMcoef(x, y, c, Mcoef, delta, WhichCurve)
            Call SolveLS(Mcoef, Diffc, det)
            For i = 1 To UBound(c)
                tpc(i) = c(i) + Diffc(i)
                ' If tpc(i) < 0 Then tpc(i) = 1
            Next
            ch2 = chi2(x, y, tpc, WhichCurve)

            If ch2 > tol Then mu = ch2 / ch20

            tdelta = (ch2 - ch20)
            If ch2 > tol Then tdelta = tdelta / ch2 'VL. 30/10/2004

            If mu > 10 Then
                delta = delta * fact
            Else
                If Math.Abs(tdelta) < 0.001 Then k = k + 1
                delta = delta / fact
                For i = 1 To UBound(c)
                    c(i) = tpc(i)
                Next i
                ch20 = ch2
            End If

            'check delta oscillation
            If niter Mod 4 = 0 Then
                If delta0 = delta Then
                    fact = 1 '2 relaxed step
                Else         ' Both of these were set to 1 (redundant really) because XP and Vista were giving different results
                    fact = 1 '10 fast step
                End If
                delta0 = delta
            End If

            niter = niter + 1
        Loop

        ch2 = chi2(x, y, c, WhichCurve)

        FunParm(fy, c, x, WhichCurve)

    End Sub
    Sub FunParm(ByRef f() As Double, ByVal c() As Double, ByVal x() As Double, ByVal WhichCurve As Integer)
        Dim i As Integer, n As Integer, t As Integer 'CHECK i and n were long
        n = UBound(x)
        Select Case WhichCurve
            Case Is = 1
                For i = 1 To n
                    f(i) = -c(3) / (1 + (x(i) / c(2)) ^ c(1)) ^ c(4) + c(3) 'this is the four parameter logistic fit
                Next i
            Case Is >= 2
                For i = 1 To n
                    f(i) = c(1)
                    For t = 1 To WhichCurve - 1
                        f(i) = f(i) + x(i) ^ t * c(t + 1)
                    Next t
                Next i
        End Select
    End Sub
    Sub DFunParm(ByRef df(,) As Double, ByVal c() As Double, ByVal x() As Double, ByVal WhichCurve As Integer)
        Dim ctemp() As Double, n As Integer, m As Integer, i As Integer, j As Integer, f0() As Double, f1() As Double, h As Double
        Dim t As Integer
        n = UBound(x)
        m = UBound(c)
        ReDim ctemp(m)
        'approximate derivative with finite differences
        h = 10 ^ -4
        For j = 1 To m
            'step forward
            For i = 1 To m
                If i = j Then
                    ctemp(i) = c(i) + h / 2
                Else
                    ctemp(i) = c(i)
                End If
                'If ctemp(i) < 0 Then ctemp(i) = 1
            Next i
            ReDim f1(n)
            Select Case WhichCurve
                Case Is = 1
                    For i = 1 To n
                        f1(i) = -ctemp(3) / (1 + (x(i) / ctemp(2)) ^ ctemp(1)) ^ ctemp(4) + ctemp(3) 'four parameter logistic fit
                    Next i
                Case Is >= 2
                    For i = 1 To n
                        f1(i) = ctemp(1)
                        For t = 1 To WhichCurve - 1
                            f1(i) = f1(i) + x(i) ^ t * ctemp(t + 1) 'Linear Fit
                        Next t
                    Next i
            End Select
            'step back
            For i = 1 To m
                If i = j Then
                    ctemp(i) = c(i) - h / 2
                Else
                    ctemp(i) = c(i)
                End If
                'If ctemp(i) < 0 Then ctemp(i) = 1
            Next i
            ReDim f0(n)
            Select Case WhichCurve
                Case Is = 1
                    For i = 1 To n
                        f0(i) = -ctemp(3) / (1 + (x(i) / ctemp(2)) ^ ctemp(1)) ^ ctemp(4) + ctemp(3) 'four parameter logistic fit
                    Next i
                Case Is >= 2
                    For i = 1 To n
                        f0(i) = ctemp(1)
                        For t = 1 To WhichCurve - 1
                            f0(i) = f0(i) + x(i) ^ t * ctemp(t + 1)
                        Next t
                    Next i
            End Select
            'finite difference
            For i = 1 To n
                df(i, j) = (f1(i) - f0(i)) / h
            Next i
        Next j
    End Sub
    Private Function chi2(ByRef x() As Double, ByRef y() As Double, ByRef c() As Double, ByVal WhichCurve As Integer) As Double

        Dim i As Integer, f(UBound(x)) As Double

        FunParm(f, c, x, WhichCurve)
        chi2 = 0
        For i = 1 To UBound(x) 'was LBound(x) To UBound(x)
            chi2 = chi2 + (y(i) - f(i)) ^ 2
        Next

    End Function
    Sub CalculateMcoef(ByRef x() As Double, ByRef y() As Double, ByRef c() As Double, ByRef Mcoef(,) As Double, ByVal delta As Double, ByVal WhichCurve As Integer)

        Dim i As Integer, l As Integer, k As Integer
        Dim dv(UBound(x), UBound(c) + 1) As Double
        'Dim temp As Double
        Dim f(UBound(x)) As Double
        ReDim Mcoef(UBound(c), UBound(c) + 1)
        FunParm(f, c, x, WhichCurve)
        DFunParm(dv, c, x, WhichCurve)
        For i = 1 To UBound(x) ' - LBound(x) + 1
            For k = 1 To UBound(c) ' - LBound(c) + 1
                Mcoef(k, UBound(c) + 1) = (y(i) - f(i)) * dv(i, k) + Mcoef(k, UBound(c) + 1)
                For l = k To UBound(c) ' - LBound(c) + 1
                    Mcoef(k, l) = dv(i, k) * dv(i, l) + Mcoef(k, l)
                Next
            Next
        Next
        For k = 2 To UBound(c) '- LBound(c) + 1
            For l = 1 To k - 1
                Mcoef(k, l) = Mcoef(l, k)
            Next
        Next
        For i = 1 To UBound(c) '- LBound(c) + 1              'Algoritmo Marquart
            Mcoef(i, i) = Mcoef(i, i) * (1 + delta)
        Next

    End Sub
    '------------------------------------------------------------------------------------------
    '=====================================================================================
    ' Linear System routine by Luis Isaac Ramos Garcia
    ' v. 13.10.2004
    ' Sub SolveLS  = Solving Linear System with scaled pivot
    ' Sub TM       = Triangularize a square matrix with scaled pivot
    ' sub solveTS  = Solving a triangular system with backsubstitution
    '                                                   Last mod. 2.11.2004 by Foxes Team
    '=====================================================================================
    Sub SolveLS(ByRef Mc(,) As Double, ByVal sol() As Double, ByVal det As Double)
        ' Rutina para resolver un sistema de ecucacines lineales de la forma:
        '    MC(1,1)*sol(1)+ MC(1,2)*sol(2)+MC(1,3)*sol(3)+...+MC(1,N)*sol(N) = MC(1,N+1)
        '    MC(2,1)*sol(1)+ MC(2,2)*sol(2)+MC(2,3)*sol(3)+...+MC(2,N)*sol(N) = MC(2,N+1)
        '    MC(3,1)*sol(1)+ MC(2,2)*sol(2)+MC(3,3)*sol(3)+...+MC(3,N)*sol(N) = MC(3,N+1)
        '    .............................................................................
        '    MC(N,1)*sol(1)+ MC(N,2)*sol(2)+MC(N,3)*sol(3)+...+MC(N,N)*sol(N) = MC(N,N+1)
        '   n numero de incognitas
        '   MC matriz de coeficientes extendida
        '               MC(1,1), MC(1,2),MC(1,3),...,MC(1,N) |MC(1,N+1)
        '               MC(2,1), MC(2,2),MC(2,3),...,MC(2,N) |MC(2,N+1)
        '               MC(3,1), MC(3,2),MC(3,3),...,MC(3,N) |MC(3,N+1)
        '               ..............................................
        '               MC(N,1), MC(N,2),MC(N,3),...,MC(N,N) |MC(N,N+1)
        '   Sol vector solucion
        '   det derminante de la matriz de coeficientes

        ' Bibliografia
        ' Numerical Recipies in Fortran77; W.H. Press, et al.; Cambridge U. Press
        ' Metodos numericos con Matlab; J. M Mathewss et al.; Prentice Hall
        Dim n As Integer
        n = UBound(sol) ' - LBound(sol) + 1

        Call tm(n, Mc)                ' Trigonalizamos la matriz con pivoteo escalado para eviar errores
        Call solveTS(Mc, sol, det)      ' Solucion del sistema trigonal
    End Sub
    Sub tm(ByVal n As Integer, ByRef Mc(,) As Double)
        ' Rutina para tragonalizar una matriz extendida de un sistema
        ' de ecuaciones lineales en la forma:
        '
        '               MC(1,1), MC(1,2),MC(1,3),...,MC(1,N) |MC(1,N+1)
        '               MC(2,1), MC(2,2),MC(2,3),...,MC(2,N) |MC(2,N+1)
        '               MC(3,1), MC(3,2),MC(3,3),...,MC(3,N) |MC(3,N+1)
        '               ..............................................
        '               MC(N,1), MC(N,2),MC(N,3),...,MC(N,N) |MC(N,N+1)
        '
        '   n numero de incognitas
        '   MC matriz de coeficientes extendida
        Dim q As Integer, r As Integer, c As Integer
        Dim m(,) As Double
        ReDim m(n, n)
        For q = 1 To n
            Call Pivot(q, n, Mc)                     'Pivotamos la matriz para evitar errores de redondeo
            For r = q + 1 To n
                If Mc(q, q) = 0 Then Exit Sub
                m(r, q) = Mc(r, q) / Mc(q, q)
                Mc(r, q) = 0
                For c = q + 1 To n + 1
                    Mc(r, c) = Mc(r, c) - m(r, q) * Mc(q, c)
                Next
            Next
        Next
    End Sub
    Private Sub Pivot(ByVal rin As Integer, ByVal n As Integer, ByRef Mc(,) As Double)
        'Rutina para hacer un pivoteo paracial ecalado de una matriz extendida
        'para un sistema de ecuaciones lineales en la forma:
        '               MC(1,1), MC(1,2),MC(1,3),...,MC(1,N) |MC(1,N+1)
        '               MC(2,1), MC(2,2),MC(2,3),...,MC(2,N) |MC(2,N+1)
        '               MC(3,1), MC(3,2),MC(3,3),...,MC(3,N) |MC(3,N+1)
        '               ..............................................
        '               MC(N,1), MC(N,2),MC(N,3),...,MC(N,N) |MC(N,N+1)
        '
        '   rin la fila dede la que comenzamos el pivoteo
        '   n numero de incognitas
        '   MC matriz de coeficientes extendida
        '
        'mod. ver. 2-11-04 VL
        '
        'Dim q As Integer '///This removed - flagged by VB Express as unused
        Dim r As Integer, c As Integer
        Dim m() As Double                               ' m() guarda el maximo falor de cada fila
        ReDim m(n) 'was ReDim m(rin To n)
        Dim max As Double
        'Dim temp() As Double
        'ReDim temp(rin To N + 1)
        Dim temp As Double
        For r = rin To n
            m(r) = Math.Abs(Mc(r, rin))
            'For c = rin + 1 To N + 1
            For c = rin + 1 To n
                If Math.Abs(Mc(r, c)) > m(r) Then
                    m(r) = Math.Abs(Mc(r, c))
                End If
            Next
        Next

        For r = rin To n
            If m(r) <> 0 Then  'VL 25-10-2004
                m(r) = Math.Abs(Mc(r, rin)) / m(r)
            End If
        Next

        max = m(rin)
        If max = 0 Then Exit Sub 'VL 25-10-2004
        'rows swap routine 'VL 2-11-2004
        For r = rin + 1 To n
            If max < m(r) Then
                For c = rin To n + 1
                    temp = Mc(rin, c)
                    Mc(rin, c) = Mc(r, c)
                    Mc(r, c) = temp
                Next
                max = m(r)
            End If
        Next
    End Sub
    Private Sub solveTS(ByVal Mc(,) As Double, ByVal sol() As Double, ByVal det As Double)
        ' Rutina para resolver un sisitema trigonal de ecucacines lineales de la forma:
        '    MC(1,1)*sol(1)+ MC(1,2)*sol(2)+MC(1,3)*sol(3)+...+MC(1,N)*sol(N) = MC(1,N+1)
        '                    MC(2,2)*sol(2)+MC(2,3)*sol(3)+...+MC(2,N)*sol(N) = MC(2,N+1)
        '                                   MC(3,3)*sol(3)+...+MC(3,N)*sol(N) = MC(3,N+1)
        '    .............................................................................
        '                                                       MC(N,N)*sol(N) = MC(N,N+1)
        '   n numero de incognitas
        '   MC matriz de coeficientes extendida
        '               MC(1,1), MC(1,2),MC(1,3),...,MC(1,N) |MC(1,N+1)
        '               MC(2,1), MC(2,2),MC(2,3),...,MC(2,N) |MC(2,N+1)
        '               MC(3,1), MC(3,2),MC(3,3),...,MC(3,N) |MC(3,N+1)
        '               ..............................................
        '               MC(N,1), MC(N,2),MC(N,3),...,MC(N,N) |MC(N,N+1)
        '   Sol vector solucion
        Dim n As Integer
        Dim k As Integer, i As Integer

        n = UBound(sol) ' - LBound(sol) + 1
        ' Aseguramos que el sistema tiene solucion
        det = 1
        For k = 1 To n
            det = Mc(k, k) * det
        Next k

        For k = n To 1 Step -1
            sol(k) = Mc(k, n + 1) / Mc(k, k)
        Next k
        For k = n - 1 To 1 Step -1
            For i = k + 1 To n
                sol(k) = sol(k) - (Mc(k, i) / Mc(k, k)) * sol(i)
            Next i
        Next k
    End Sub
    Sub setxyc()

        'Reads raw RPM data from an existing data file.  This is used to test the curve fit procedure
        'and to re-fit data sent by other folks

        With inputdialog
            .Reset()
            .Filter = "All files (*.*)|*.*"
            .ShowDialog()
        End With

        If inputdialog.FileName <> "" Then
            inputfile = New StreamReader(inputdialog.FileName)
            Select Case inputfile.ReadLine
                Case Is = Main.PowerRunVersion, "POWER_RUN_6_3"
                    Dim temp As String = "", tempsplit() As String
                    Do Until temp.StartsWith("Actual_MOI:")
                        temp = inputfile.ReadLine
                    Loop
                    tempsplit = Split(temp, " ")
                    Main.DynoMomentOfInertia = CDbl(tempsplit(1))
                    Do Until inputfile.ReadLine = "PRIMARY_CHANNEL_RAW_DATA"
                    Loop
                    Main.DataPoints = CInt(Split(inputfile.ReadLine, " ")(1)) 'Number of points
                    inputfile.ReadLine() 'Blank line with headings
                    Dim Counter As Integer
                    Dim DataLine As String
                    For Counter = 1 To Main.DataPoints
                        DataLine = inputfile.ReadLine
                        'CHECK A LOOP SIMILAR TO THE ONE USED TO WRITE THE FILE WOULD WORK WELL HERE
                        Main.CollectedData(Main.SESSIONTIME, Counter) = CDbl(Split(DataLine, " ")(0)) 'Time
                        Main.CollectedData(Main.RPM1_ROLLER, Counter) = CDbl(Split(DataLine, " ")(1)) 'rads/s
                    Next
                Case Is = "POWER_CURVE" ' the string for version 5.5 
                    Do Until inputfile.ReadLine = "PRIMARY_CHANNEL_RAW_DATA"
                    Loop
                    Main.DataPoints = CInt(Split(inputfile.ReadLine, " ")(1)) 'Number of points
                    inputfile.ReadLine() 'Blank line with headings
                    Dim Counter As Integer
                    Dim DataLine As String
                    For Counter = 1 To Main.DataPoints
                        DataLine = inputfile.ReadLine
                        'CHECK A LOOP SIMILAR TO THE ONE USED TO WRITE THE FILE WOULD WORK WELL HERE
                        Main.CollectedData(Main.SESSIONTIME, Counter) = CDbl(Split(DataLine, " ")(1)) 'Time
                        Main.CollectedData(Main.RPM1_ROLLER, Counter) = CDbl(Split(DataLine, " ")(2)) / Main.DataUnits(Main.RPM1_ROLLER, 1) 'rads/s
                    Next
            End Select

        End If

    End Sub

#End Region
    Private Sub rdoRPM1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRPM1.CheckedChanged
        If Me.Visible = True Then
            If rdoRPM1.Checked Then
                cmbWhichFit.Enabled = True
                scrlRPM1Smooth.Enabled = True
                scrlStartFit.Enabled = True
                txtPowerRunSpikeLevel.Enabled = True
                WhichFitData = RPM
                pnlDataWindowSetup()
            Else
                cmbWhichFit.Enabled = False
                scrlRPM1Smooth.Enabled = False
                scrlStartFit.Enabled = False
                txtPowerRunSpikeLevel.Enabled = False
            End If
        End If
    End Sub
    Private Sub rdoRunDown_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRunDown.CheckedChanged
        If rdoRunDown.Checked Then
            cmbWhichRDFit.Enabled = True
            If cmbWhichRDFit.SelectedItem.ToString = "MA Smooth" Then
                scrlCoastDownSmooth.Enabled = True
            Else
                scrlCoastDownSmooth.Enabled = False
            End If
            WhichFitData = RUNDOWN
            pnlDataWindowSetup()
        Else
            cmbWhichRDFit.Enabled = False
        End If
    End Sub
    Private Sub rdoCurrent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoCurrent.CheckedChanged
        If rdoCurrent.Checked Then
            scrlCurrentSmooth.Enabled = True
            WhichFitData = CURRENT
            pnlDataWindowSetup()
        Else
            scrlCurrentSmooth.Enabled = False
        End If
    End Sub
    Private Sub rdoVoltage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoVoltage.CheckedChanged
        If rdoVoltage.Checked Then
            scrlVoltageSmooth.Enabled = True
            WhichFitData = VOLTAGE
            pnlDataWindowSetup()
        Else
            scrlVoltageSmooth.Enabled = False
        End If
    End Sub
    Private Sub cmbWhichFit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWhichFit.SelectedIndexChanged
        If Me.Visible Then
            'CHECK - THIS WILL CHANEG THE INDEX CANT CHANGE ULNESS THE RDO IS CHECKED
            If rdoRPM1.Checked = True Then
                WhichFitData = RPM
                Main.StopFitting = False
                If cmbWhichFit.SelectedItem.ToString = "MA Smooth" Then
                    scrlRPM1Smooth.Enabled = True
                Else
                    scrlRPM1Smooth.Enabled = False
                End If
                FitRPMData()
            Else
                rdoRPM1.Checked = True
            End If
        End If
    End Sub
    Private Sub cmbWhichRDFit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWhichRDFit.SelectedIndexChanged
        If Me.Visible Then
            If rdoRunDown.Checked = True Then
                WhichFitData = RUNDOWN
                Main.StopFitting = False
                If cmbWhichRDFit.SelectedItem.ToString = "MA Smooth" Then
                    scrlCoastDownSmooth.Enabled = True
                Else
                    scrlCoastDownSmooth.Enabled = False
                End If
                FitRPMRunDownData()
            Else
                rdoRunDown.Checked = True
            End If
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAnalysis.Click
        inputdialog.FileName = "" 'reset for the setxyc hack
        Me.Hide()
        Main.btnStartLoggingRaw.Enabled = True
        'If blnfit Then
        If blnRPMFit AndAlso blnCoastDownDownFit AndAlso blnVoltageFit AndAlso blnCurrentFit Then
            WritePowerFile()
            If chkAddOrNew.Checked = False Then
                Main.frmAnalysis.btnClearOverlay_Click_1(Me, System.EventArgs.Empty)
            End If
            Main.frmAnalysis.WindowState = FormWindowState.Normal
            Main.frmAnalysis.OpenFileDialog1.FileName = Main.LogPowerRunDataFileName
            Main.frmAnalysis.btnAddOverlayFile_Click_1(Me, System.EventArgs.Empty)
            Main.frmAnalysis.ShowDialog()
        Else
            'Main.StopFitting = True
            MsgBox("All of the expected curve fits were not completed. Please make sure all data are fit appropriately", vbOKOnly)
            Me.Show() 'Main.btnShow_Click(Me, System.EventArgs.Empty)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopFitting.Click
        Main.StopFitting = True
        blnfit = False
        Me.Cursor = Cursors.Default
        ' Main.ProcessingData = False
        Main.Cursor = Cursors.Default
    End Sub

End Class