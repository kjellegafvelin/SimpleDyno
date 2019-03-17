Imports System.Collections.Generic
Imports System.IO

Public Class DataInputFileReader
    Public OverlayFileCount As Integer = 0
    Public OverlayFiles() As String
    Public AnalyzedData(MAXDATAFILES, Main.LAST, Main.MAXDATAPOINTS) As Double

    Private Const MAXDATAFILES As Integer = 5

    Public Sub ReadDataFile(fileName As String)
        Dim temp As String, line() As String
        Dim PointCount As Integer
        Dim CopyFileName As String
        Dim DataCopyfile As StreamWriter
        Dim dataFileReader As StreamReader

        ReDim OverlayFiles(MAXDATAFILES)

        If fileName <> "" Then
            dataFileReader = New System.IO.StreamReader(fileName)
            With dataFileReader
                temp = .ReadLine
                Select Case temp
                    Case Is = Main.PowerRunVersion, "POWER_RUN_6_3", "POWER_RUN_6_4" 'This is a valid current version file
                        OverlayFileCount += 1
                        If OverlayFileCount = MAXDATAFILES Then
                            REM btnAddOverlayFile.Enabled = False
                            Main.frmFit.chkAddOrNew.Checked = False
                            Main.frmFit.chkAddOrNew.Enabled = False
                        End If
                        OverlayFiles(OverlayFileCount) = fileName.Substring(fileName.LastIndexOf("\") + 1)
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
                        MsgBox("A copy of " & fileName & " will be saved as a new version .sdp file.", vbOKOnly)
                        'Convert old data to new data
                        'open the copy file
                        CopyFileName = fileName.Substring(0, fileName.Length - 4) & ".sdp"
                        DataCopyfile = New System.IO.StreamWriter(CopyFileName)
                        DataCopyfile.WriteLine(Main.PowerRunVersion)
                        DataCopyfile.WriteLine(CopyFileName)
                        'load it up as if it were a version 6 file
                        OverlayFileCount += 1
                        If OverlayFileCount = MAXDATAFILES Then
                            REM btnAddOverlayFile.Enabled = False
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
                    Case Is = "POWER_CURVE" 'We are assuming that this is a SD 5.5 Power Run File.
                        MsgBox("A copy of " & fileName & " will be saved as a new version .sdp file.", vbOKOnly)
                        'Convert old data to new data
                        'open the copy file
                        CopyFileName = fileName.Substring(0, fileName.Length - 4) & ".sdp"
                        DataCopyfile = New System.IO.StreamWriter(CopyFileName)
                        DataCopyfile.WriteLine(Main.PowerRunVersion)
                        DataCopyfile.WriteLine(CopyFileName)
                        'load it up as if it were a version 6 file
                        OverlayFileCount += 1
                        If OverlayFileCount = MAXDATAFILES Then
                            REM btnAddOverlayFile.Enabled = False
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
                    Case Else
                        MsgBox("Could not open file.  If this is a Power Run created by an older SimpleDyno version please email it to damorc1@hotmail.com so that a fix can be made available", vbOKOnly)
                End Select
            End With
            dataFileReader.Close()
        End If
    End Sub



    Public Function ReadDataFile2(fileName As String) As List(Of DataRecord)

        Dim temp As String
        Dim PointCount As Integer

        If fileName = "" Then
            Throw New InvalidOperationException("Filename is missing")
        End If
        Using dataFileReader As New System.IO.StreamReader(fileName)


            With dataFileReader
                temp = .ReadLine
                Select Case temp
                    Case Is = Main.PowerRunVersion, "POWER_RUN_6_3", "POWER_RUN_6_4" 'This is a valid current version file

                        Do
                            temp = .ReadLine
                        Loop Until temp.StartsWith("NUMBER_OF_POINTS_FIT")

                        Dim noOfRecords As Integer = CInt(temp.Substring(temp.LastIndexOf(" "))) 'used the empty holder to remember the number of fit points

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

                        Dim dataRecords As New List(Of DataRecord)(noOfRecords)

                        For PointCount = 1 To noOfRecords
                            DataLine = Split(.ReadLine, " ") 'reads all the values on this line into a string array
                            Dim dataRecord As New DataRecord

                            For ParamCount = 0 To Main.LAST - 1
                                'This is how the titles are created in the fitting code except we do not add the space
                                UnitName = Split(Main.DataUnitTags(ParamCount), " ")
                                SearchString = Main.DataTags(ParamCount).Replace(" ", "_") & "_(" & UnitName(0) & ")"
                                ParamPosition = Array.IndexOf(TitlesSplit, SearchString)
                                If ParamPosition <> -1 Then
                                    Dim value As Double = CDbl(DataLine(ParamPosition))


                                    Select Case SearchString
                                        Case "Time_(Sec)"
                                            dataRecord.Time = value
                                        Case "RPM1_Roller_(rad/s)"
                                            dataRecord.RPM1_Roller = value
                                        Case "RPM1_Wheel_(rad/s)"
                                            dataRecord.RPM1_Wheel = value
                                        Case "RPM1_Motor_(rad/s)"
                                            dataRecord.RPM1_Motor = value
                                        Case "Ch1_Frequency_(Hz)"
                                            dataRecord.Ch1_Frequency = value
                                        Case "Ch1_Pulse_Width_(MS)"
                                            dataRecord.Ch1_Pulse_Width = value
                                        Case "Ch1_Duty_Cycle_(%)"
                                            dataRecord.Ch1_Duty_Cycle = value
                                        Case "Speed_(m/s)"
                                            dataRecord.Speed = value
                                        Case "RPM2_(rad/s)"
                                            dataRecord.RPM2 = value
                                        Case "Ratio_(M/W)"
                                            dataRecord.Ratio = value
                                        Case "Rollout_(mm)"
                                            dataRecord.Rollout = value
                                        Case "Ch2_Frequency_(Hz)"
                                            dataRecord.Ch2_Frequency = value
                                        Case "Ch2_Pulse_Width_(MS)"
                                            dataRecord.Ch2_Pulse_Width = value
                                        Case "Ch2_Duty_Cycle_(%)"
                                            dataRecord.Ch2_Duty_Cycle = value
                                        Case "Roller_Torque_(N.m)"
                                            dataRecord.Roller_Torque = value
                                        Case "Wheel_Torque_(N.m)"
                                            dataRecord.Wheel_Torque = value
                                        Case "Motor_Torque_(N.m)"
                                            dataRecord.Motor_Torque = value
                                        Case "Coast_Down_Torque_(N.m)"
                                            dataRecord.Coast_Down_Torque = value
                                        Case "Corr._Roller_Torque_(N.m)"
                                            dataRecord.Corr_Roller_Torque = value
                                        Case "Corr._Wheel_Torque_(N.m)"
                                            dataRecord.Corr_Wheel_Torque = value
                                        Case "Corr._Motor_Torque_(N.m)"
                                            dataRecord.Corr_Motor_Torque = value
                                        Case "Power_(W)"
                                            dataRecord.Power = value
                                        Case "Coast_Down_Power_(W)"
                                            dataRecord.Coast_Down_Power = value
                                        Case "Corr._Roller_Power_(W)"
                                            dataRecord.Corr_Roller_Power = value
                                        Case "Corr._Wheel_Power_(W)"
                                            dataRecord.Corr_Wheel_Power = value
                                        Case "Corr._Motor_Power_(W)"
                                            dataRecord.Corr_Motor_Power = value
                                        Case "Drag_(W)"
                                            dataRecord.Drag = value
                                        Case "Voltage_(V)"
                                            dataRecord.Voltage = value
                                        Case "Current_(A)"
                                            dataRecord.Current = value
                                        Case "Watts_In_(W)"
                                            dataRecord.Watts_In = value
                                        Case "Efficiency_(%)"
                                            dataRecord.Efficiency = value
                                        Case "Corr._Efficiency_(%)"
                                            dataRecord.Corr_Efficiency = value
                                        Case "Temperature1_(°C)"
                                            dataRecord.Temperature1 = value
                                        Case "Temperature2_(°C)"
                                            dataRecord.Temperature2 = value
                                        Case "Pin_4_Value_(Units)"
                                            dataRecord.Pin_4_Value = value
                                        Case "Pin_5_Value_(Units)"
                                            dataRecord.Pin_5_Value = value
                                    End Select

                                End If

                            Next
                            dataRecords.Add(dataRecord)
                        Next
                        Return dataRecords

                    Case Else
                        Throw New InvalidOperationException("Invalid data file type.")

                End Select

            End With
        End Using

    End Function

End Class

Public Structure DataRecord
    Public Time As Double
    Public RPM1_Roller As Double
    Public RPM1_Wheel As Double
    Public RPM1_Motor As Double
    Public Ch1_Frequency As Double
    Public Ch1_Pulse_Width As Double
    Public Ch1_Duty_Cycle As Double
    Public Speed As Double
    Public RPM2 As Double
    Public Ratio As Double
    Public Rollout As Double
    Public Ch2_Frequency As Double
    Public Ch2_Pulse_Width As Double
    Public Ch2_Duty_Cycle As Double
    Public Roller_Torque As Double
    Public Wheel_Torque As Double
    Public Motor_Torque As Double
    Public Coast_Down_Torque As Double
    Public Corr_Roller_Torque As Double
    Public Corr_Wheel_Torque As Double
    Public Corr_Motor_Torque As Double
    Public Power As Double
    Public Coast_Down_Power As Double
    Public Corr_Roller_Power As Double
    Public Corr_Wheel_Power As Double
    Public Corr_Motor_Power As Double
    Public Drag As Double
    Public Voltage As Double
    Public Current As Double
    Public Watts_In As Double
    Public Efficiency As Double
    Public Corr_Efficiency As Double
    Public Temperature1 As Double
    Public Temperature2 As Double
    Public Pin_4_Value As Double
    Public Pin_5_Value As Double
End Structure
