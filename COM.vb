Public Class COM
    'Global Temporary Double for Checking the numeric input of the textboxes
    Private TempDouble As Double
    'Calibration timer specific
    Private CalibrationButton As Button
    Private Smalltmr As New Timer
    Friend Calibrating As Boolean = False
    Friend CalibrationValues(6) As Double
    Friend NumberOfCalibrationValues As Long
    Friend Sub COM_Setup()
        SetupCalibrationButtons()
        Main.COMPortCalibration()
    End Sub
    Private Sub COM_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Prevents form from actually closing, rather it hides
        If e.CloseReason <> CloseReason.FormOwnerClosing Then
            Me.Hide()
            e.Cancel = True
            Main.btnShow_Click(Me, System.EventArgs.Empty)
        End If
    End Sub
    Friend Sub SetupCalibrationButtons()
        'Adds handler to all of the calibration buttons
        Smalltmr.Stop()
        AddHandler Smalltmr.Tick, AddressOf CalibrationCounter
        Smalltmr.Interval = 1000
        For Each c As Control In Me.Controls
            If TypeOf c Is Button And InStr(c.Name, "Calibrate") <> 0 Then
                AddHandler c.Click, AddressOf btnCalibrate_Click
            End If
        Next
    End Sub

    Private Sub btnCalibrate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles btnCalibrateV1.Click
        'User enters known value into the "Input" Box.  Sample those readings for 1 sec and enter the average value into the Voltage at Ax box
        'This sub just sets the id of the button and starts the timer
        CalibrationButton = CType(sender, Button)
        ReDim CalibrationValues(6)
        Smalltmr.Start()
        Calibrating = True
    End Sub
    Private Sub CalibrationCounter(ByVal sender As Object, ByVal e As System.EventArgs)
        Calibrating = False
        Smalltmr.Stop()
        Dim tempeventargs As New System.EventArgs
        For temp As Integer = 0 To 5
            CalibrationValues(temp) = CalibrationValues(temp) / NumberOfCalibrationValues / Main.BitsToVoltage
        Next
        With Main
            Select Case CalibrationButton.Name.ToString
                Case Is = "btnCalibrateV1"
                    .SetControlText_Threadsafe(txtA0Voltage1, .NewCustomFormat(CalibrationValues(0)))
                    txtA0Voltage1_Leave(txtA0Voltage1, tempeventargs)
                Case Is = "btnCalibrateV2"
                    .SetControlText_Threadsafe(txtA0Voltage2, .NewCustomFormat(CalibrationValues(0)))
                    txtA0Voltage2_Leave(txtA0Voltage2, tempeventargs)
                    'Note the current calibration needs to use the resistance
                Case Is = "btnCalibrateI1"
                    'use resistance and voltage to calibrate
                    If .Resistance1 <> 0 Then
                        .SetControlText_Threadsafe(txtA1Voltage1, .NewCustomFormat(CalibrationValues(1)))
                        .SetControlText_Threadsafe(txtInputCurrent1, .NewCustomFormat(CalibrationValues(0) / .Resistance1))

                        txtA1Voltage1_Leave(txtA1Voltage1, tempeventargs)
                        txtInputCurrent1_Leave(txtInputCurrent1, tempeventargs)
                    Else
                        .SetControlText_Threadsafe(txtA1Voltage1, .NewCustomFormat(CalibrationValues(1)))
                        txtA1Voltage1_Leave(txtA1Voltage1, tempeventargs)
                    End If
                Case Is = "btnCalibrateI2"
                    If .Resistance2 <> 0 Then
                        .SetControlText_Threadsafe(txtA1Voltage2, .NewCustomFormat(CalibrationValues(1)))
                        .SetControlText_Threadsafe(txtInputCurrent2, .NewCustomFormat(.VoltageIntercept + .VoltageSlope * CalibrationValues(0) * .BitsToVoltage / .Resistance2))

                        txtA1Voltage2_Leave(txtA1Voltage2, tempeventargs)
                        txtInputCurrent2_Leave(txtInputCurrent2, tempeventargs)

                    Else
                        .SetControlText_Threadsafe(txtA1Voltage2, .NewCustomFormat(CalibrationValues(1)))
                        txtA1Voltage2_Leave(txtA1Voltage2, tempeventargs)
                    End If
                Case Is = "btnCalibrateTemp1T1"
                    .SetControlText_Threadsafe(txtA2Voltage1, .NewCustomFormat(CalibrationValues(2)))
                    txtA2Voltage1_Leave(txtA2Voltage1, tempeventargs)
                Case Is = "btnCalibrateTemp1T2"
                    .SetControlText_Threadsafe(txtA2Voltage2, .NewCustomFormat(CalibrationValues(2)))
                    txtA2Voltage2_Leave(txtA2Voltage2, tempeventargs)
                Case Is = "btnCalibrateTemp2T1"
                    .SetControlText_Threadsafe(txtA3Voltage1, .NewCustomFormat(CalibrationValues(3)))
                    txtA3Voltage1_Leave(txtA3Voltage1, tempeventargs)
                Case Is = "btnCalibrateTemp2T2"
                    .SetControlText_Threadsafe(txtA3Voltage2, .NewCustomFormat(CalibrationValues(3)))
                    txtA3Voltage2_Leave(txtA3Voltage2, tempeventargs)
                Case Is = "btnCalibratePinA4V1"
                    .SetControlText_Threadsafe(txtA4Voltage1, .NewCustomFormat(CalibrationValues(4)))
                    txtA4Voltage1_Leave(txtA4Voltage1, tempeventargs)
                Case Is = "btnCalibratePinA4V2"
                    .SetControlText_Threadsafe(txtA4Voltage2, .NewCustomFormat(CalibrationValues(4)))
                    txtA4Voltage1_Leave(txtA4Voltage2, tempeventargs)
                Case Is = "btnCalibratePinA5V1"
                    .SetControlText_Threadsafe(txtA5Voltage1, .NewCustomFormat(CalibrationValues(5)))
                    txtA4Voltage1_Leave(txtA5Voltage1, tempeventargs)
                Case Is = "btnCalibratePinA5V2"
                    .SetControlText_Threadsafe(txtA5Voltage2, .NewCustomFormat(CalibrationValues(5)))
                    txtA4Voltage1_Leave(txtA5Voltage2, tempeventargs)
            End Select
        End With
        NumberOfCalibrationValues = 0
    End Sub

    Private Sub txtA0Voltage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA0Voltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A0Voltage2 Then
                'need to check that it is not the same as A1Voltage
                Main.A0Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A0Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A0Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A0Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputVoltage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputVoltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Voltage2 Then
                Main.Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA0Voltage2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA0Voltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A0Voltage1 Then
                Main.A0Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A0Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A0Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A0Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputVoltage2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputVoltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Voltage1 Then
                Main.Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA1Voltage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA1Voltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A1Voltage2 Then
                Main.A1Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A1Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A1Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A1Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputCurrent1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputCurrent1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Current2 Then
                Main.Current1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Current2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Current1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Current1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA1Voltage2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA1Voltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A1Voltage1 Then
                Main.A1Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A1Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A1Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A1Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputCurrent2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputCurrent2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Current1 Then
                Main.Current2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Current1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Current2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Current2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA2Voltage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA2Voltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A2Voltage2 Then
                Main.A2Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A2Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A2Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A2Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputTemp1Temperature1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputTemp1Temperature1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Temp1Temperature2 Then
                Main.Temp1Temperature1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Temp1Temperature2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Temp1Temperature1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Temp1Temperature1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA2Voltage2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA2Voltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A2Voltage1 Then
                Main.A2Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A2Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A2Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A2Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputTemp1Temperature2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputTemp1Temperature2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Temp1Temperature1 Then
                Main.Temp1Temperature2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Temp1Temperature1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Temp1Temperature2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Temp1Temperature2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA3Voltage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA3Voltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A3Voltage2 Then
                Main.A3Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A3Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A3Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A3Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputTemp2Temperature1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputTemp2Temperature1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Temp2Temperature2 Then
                Main.Temp2Temperature1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Temp2Temperature2 Then
                    MsgBox(CType(sender, TextBox).Name & "Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Temp2Temperature1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Temp2Temperature1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA3Voltage2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA3Voltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A3Voltage1 Then
                Main.A3Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A3Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A3Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A3Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtInputTemp2Temperature2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInputTemp2Temperature2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.Temp2Temperature1 Then
                Main.Temp2Temperature2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.Temp2Temperature1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.Temp2Temperature2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.Temp2Temperature2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtResistance1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtResistance1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            Main.Resistance1 = TempDouble
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = Main.Resistance1.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtResistance2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtResistance2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            Main.Resistance2 = TempDouble
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = Main.Resistance2.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtA4Voltage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtA4Voltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A4Voltage2 Then
                Main.A4Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A4Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A4Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A4Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA4Voltage2_Leave(sender As Object, e As System.EventArgs) Handles txtA4Voltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A4Voltage1 Then
                Main.A4Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A4Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A4Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A4Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtPin4Value1_Leave(sender As Object, e As System.EventArgs) Handles txtPin4Value1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A4Value2 Then
                Main.A4Value1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A4Value2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A4Value1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A4Value1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtPin4Value2_Leave(sender As Object, e As System.EventArgs) Handles txtPin4Value2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A4Value1 Then
                Main.A4Value2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A4Value1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A4Value2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A4Value2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA5Voltage1_Leave(sender As Object, e As System.EventArgs) Handles txtA5Voltage1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A5Voltage2 Then
                Main.A5Voltage1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A5Voltage2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A5Voltage1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A5Voltage1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtA5Voltage2_Leave(sender As Object, e As System.EventArgs) Handles txtA5Voltage2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 5
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A5Voltage1 Then
                Main.A5Voltage2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A5Voltage1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A5Voltage2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A5Voltage2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtPin5Value1_Leave(sender As Object, e As System.EventArgs) Handles txtPin5Value1.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A5Value2 Then
                Main.A5Value1 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A5Value2 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A5Value1.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A5Value1 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
    Private Sub txtPin5Value2_Leave(sender As Object, e As System.EventArgs) Handles txtPin5Value2.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        'if mainform loaded, need to validate the entry and check that values 1 and 2 in the calibration curve are different
        If Main.Formloaded Then
            If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) AndAlso TempDouble <> Main.A5Value1 Then
                Main.A5Value2 = TempDouble
                Main.COMPortCalibration()
            Else
                If TempDouble = Main.A5Value1 Then
                    MsgBox(CType(sender, TextBox).Name & " Values 1 and 2 must be different for calibration", MsgBoxStyle.Exclamation)
                Else
                    MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
                End If
                With CType(sender, TextBox)
                    .Text = Main.A5Value2.ToString
                    .Focus()
                End With
            End If
        Else
            Double.TryParse(CType(sender, TextBox).Text, TempDouble)
            Main.A5Value2 = TempDouble
            Main.COMPortCalibration()
        End If
    End Sub
End Class