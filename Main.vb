'SimpleDyno
'Damian Cunningham 2010 - 2014
Imports System.IO
Imports System.IO.Ports
Imports System.Management
Imports System.Drawing.Drawing2D
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Public Class Main
    Inherits System.Windows.Forms.Form
#Region "Compiler Constants"
    'These constants are used to control how the app is compiled
#Const QueryPerformance = 0 'Triggers performance monitoring
#If QueryPerformance Then
    Dim StartWatch As Long, StopWatch As Long, WatchTickConversion As Long
    Dim A As Boolean = QueryPerformanceFrequency(WatchTickConversion)
    Const P_FREQ As Integer = 0
    Const P_TIME As Integer = 1
    Dim PerformanceData(2, 200) As Double 'hold frequency value and performance values
    Dim PerfBufferCount As Integer
#End If


#End Region
#Region "API structures"
    'Structures Required by winmm.dll
    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Structure WAVEFORMATEX
        Dim wFormatTag As Int16
        Dim nchannels As Int16
        Dim nSamplesPerSec As Int32
        Dim nAvgBytesPerSec As Int32
        Dim nBlockAlign As Int16
        Dim wBitsPerSample As Int16
        Dim cbSize As Int16
    End Structure
    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Structure WAVEHDR
        Dim lpData As IntPtr
        Dim dwBufferLength As Int32
        Dim dwBytesRecorded As Int32
        Dim dwUser As IntPtr
        Dim dwFlags As Int32
        Dim dwLoops As Int32
        Dim lpNext As IntPtr
        Dim reserved As IntPtr
    End Structure
#End Region
#Region "API declarations"
    'Function declarations for winmm.dll and kernel32.dll
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInOpen(ByRef lphWaveIn As IntPtr, ByVal uDeviceID As IntPtr, ByRef lpFormat As WAVEFORMATEX, ByVal dwCallback As WaveCallBackProcedure, ByVal dwInstance As IntPtr, ByVal dwFlags As Int32) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInClose(ByVal hWaveIn As IntPtr) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInReset(ByVal hWaveIn As IntPtr) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInStart(ByVal hWaveIn As IntPtr) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInStop(ByVal hWaveIn As IntPtr) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInAddBuffer(ByVal hWaveIn As IntPtr, ByRef lpWaveInHdr As WAVEHDR, ByVal uSize As Int32) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInPrepareHeader(ByVal hWaveIn As IntPtr, ByRef lpWaveInHdr As WAVEHDR, ByVal uSize As Int32) As Int32
    End Function
    <DllImport("winmm.dll", SetLastError:=True)> _
    Shared Function waveInUnprepareHeader(ByVal hWaveIn As IntPtr, ByRef lpWaveInHdr As WAVEHDR, ByVal uSize As Int32) As Int32
    End Function
    'QueryPerformanceCounter and  QueryPerformanceFrequency are used for performance testing only
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Shared Function QueryPerformanceCounter(ByRef lpPerformanceCount As Long) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function QueryPerformanceFrequency(<Out()> ByRef lpFrequency As Long) As Boolean
    End Function
#End Region
#Region "Constants"
    'General constants
    Public Const SDVersion As String = "6_5" 'Version Information
    Private Const Phi As Double = 1.61803398875 'Golden Ratio
    Friend Const BitsToVoltage As Double = 1024 / 5   '10 bit voltage conversion 
    Friend Const MAXDATAPOINTS As Integer = 50000 'Maximum Power Run and Analysis Data Points
    Friend Const MAXDATAFILES As Integer = 3 'Maximum number of Analysis files

    'These are standard winmm and waveinput constants 
    Private Const WAVE_FORMAT_PCM As Int32 = 1
    Private Const WAVE_MAPPER As Int32 = -1
    Private Const CALLBACK_FUNCTION As Int32 = &H30000
    Private Const WIM_DATA As Int32 = &H3C0
    Private Const BITS_PER_SAMPLE As Integer = 8
    Private Const NUMBER_OF_BUFFERS As Int32 = 20

    'Constants for new data structure approach
    'Primary Dimensions for DATA
    Friend Const SESSIONTIME As Integer = 0
    Friend Const RPM1_ROLLER As Integer = 1
    Friend Const RPM1_WHEEL As Integer = 2
    Friend Const RPM1_MOTOR As Integer = 3
    Friend Const CHAN1_FREQUENCY As Integer = 4
    Friend Const CHAN1_PULSEWIDTH As Integer = 5
    Friend Const CHAN1_DUTYCYCLE As Integer = 6
    Friend Const SPEED As Integer = 7
    Friend Const RPM2 As Integer = 8
    Friend Const RPM2_RATIO As Integer = 9
    Friend Const RPM2_ROLLOUT As Integer = 10
    Friend Const CHAN2_FREQUENCY As Integer = 11
    Friend Const CHAN2_PULSEWIDTH As Integer = 12
    Friend Const CHAN2_DUTYCYCLE As Integer = 13
    Friend Const TORQUE_ROLLER As Integer = 14
    Friend Const TORQUE_WHEEL As Integer = 15
    Friend Const TORQUE_MOTOR As Integer = 16
    Friend Const TORQUE_COASTDOWN As Integer = 17
    Friend Const CORRECTED_TORQUE_ROLLER As Integer = 18
    Friend Const CORRECTED_TORQUE_WHEEL As Integer = 19
    Friend Const CORRECTED_TORQUE_MOTOR As Integer = 20
    Friend Const POWER As Integer = 21
    Friend Const POWER_COASTDOWN As Integer = 22
    Friend Const CORRECTED_POWER_ROLLER As Integer = 23
    Friend Const CORRECTED_POWER_WHEEL As Integer = 24
    Friend Const CORRECTED_POWER_MOTOR As Integer = 25
    Friend Const DRAG As Integer = 26
    Friend Const VOLTS As Integer = 27
    Friend Const AMPS As Integer = 28
    Friend Const WATTS_IN As Integer = 29
    Friend Const EFFICIENCY As Integer = 30
    Friend Const CORRECTED_EFFICIENCY As Integer = 31
    Friend Const TEMPERATURE1 As Integer = 32
    Friend Const TEMPERATURE2 As Integer = 33
    Friend Const PIN04VALUE As Integer = 34
    Friend Const PIN05VALUE As Integer = 35

    'CHECK INTEGERS FOR QUERY PERFORMANCE IF WE LEAVE RUNDOWN IN
#If QueryPerformance Then
    Friend Const PERFORMANCE As Integer = 36
    Friend Const LAST As Integer = 37
#Else
    Friend Const LAST As Integer = 36 ' CHECK - PRE RUNDOWN THIS WAS 27
#End If
    'Secondary Dimensions for DATA
    Private Const MINIMUM As Integer = 0
    Private Const ACTUAL As Integer = 1
    Private Const MAXIMUM As Integer = 2
    Private Const MINCURMAXPOINTER As Integer = 3
    Friend WithEvents Button1 As System.Windows.Forms.Button

#End Region
#Region "SimpleDyno Function Declarations"
    'Wave call back function declaration
    Delegate Function WaveCallBackProcedure(ByVal hwi As IntPtr, ByVal uMsg As Int32, ByVal dwInstance As Int32, ByVal dwParam1 As Int32, ByVal dwParam2 As Int32) As Int32
    Private myCallBackFunction As New WaveCallBackProcedure(AddressOf MyWaveCallBackProcedure) 'use with callback

    'Custom Rounding and Formatting Functions
    Friend Function CustomRound(ByVal Sent As Double) As Double
        'This is not particularly fast, but it is not used often?
        Dim TenCount As Double = 1
        If Sent > 0 Then
            Sent = Sent / 5
            Do Until Sent > 1
                Sent = Sent * 10
                TenCount = TenCount / 10
            Loop
            If Int(Sent) > Sent Then
                Sent = Int(Sent) * TenCount * 5
            Else
                Sent = Int((Sent + 1)) * TenCount * 5
            End If
        End If
        CustomRound = Sent
    End Function

    'Formatting function for numbers and significant digits presented
    Friend Function NewCustomFormat(ByVal sent As Double) As String
        Dim TempFormat As String
        Select Case sent
            Case Is >= 100
                TempFormat = "0"
            Case Is >= 10
                TempFormat = "0.0"
            Case Is >= 1
                TempFormat = "0.00"
            Case Is >= 0.1
                TempFormat = "0.000"
            Case Is >= 0.001 '0.01
                TempFormat = "0.0000"
            Case Else
                TempFormat = "0"
        End Select
        NewCustomFormat = sent.ToString(TempFormat)
    End Function
#End Region
#Region "SimpleDyno Definitions "
    'Version Strings
    Public Shared MainTitle As String = "SimpleDyno " & SDVersion & " by DamoRC"
    Public Shared PowerRunVersion As String = "POWER_RUN_" & SDVersion
    Public Shared LogRawVersion As String = "LOG_RAW_" & SDVersion
    Public Shared InterfaceVersion As String = "SimpleDyno_Interface_" & SDVersion

    'Sub Forms
    Public Shared frmDyno As Dyno
    Public Shared frmCOM As COM
    Public Shared frmAnalysis As Analysis
    Public Shared frmFit As Fit
    Public Shared frmCorrection As Correction

    Private TempDouble As Double 'Global String Temporary Variable for Double Conversion

    'Serial Port Coms and measures
    Private mySerialPort As New SerialPort
    Private COMPortsAvailable As Boolean = False
    Public Shared COMPortMessage() As String
    Public Shared Voltage1 As Double
    Public Shared Voltage2 As Double
    Public Shared A0Voltage1 As Double
    Public Shared A0Voltage2 As Double
    Public Shared VoltageSlope As Double
    Public Shared VoltageIntercept As Double
    Public Shared Current1 As Double
    Public Shared Current2 As Double
    Public Shared A1Voltage1 As Double
    Public Shared A1Voltage2 As Double
    Public Shared CurrentSlope As Double
    Public Shared CurrentIntercept As Double
    Public Shared Temp1Temperature1 As Double
    Public Shared Temp1Temperature2 As Double
    Public Shared A2Voltage1 As Double
    Public Shared A2Voltage2 As Double
    Public Shared Temperature1Slope As Double
    Public Shared Temperature1Intercept As Double
    Public Shared Temp2Temperature1 As Double
    Public Shared Temp2Temperature2 As Double
    Public Shared A3Voltage1 As Double
    Public Shared A3Voltage2 As Double
    Public Shared Temperature2Slope As Double
    Public Shared Temperature2Intercept As Double
    Public Shared A4Value1 As Double
    Public Shared A4Value2 As Double
    Public Shared A4Voltage1 As Double
    Public Shared A4Voltage2 As Double
    Public Shared A4ValueSlope As Double
    Public Shared A4ValueIntercept As Double
    Public Shared A5Value1 As Double
    Public Shared A5Value2 As Double
    Public Shared A5Voltage1 As Double
    Public Shared A5Voltage2 As Double
    Public Shared A5ValueSlope As Double
    Public Shared A5ValueIntercept As Double
    Public Shared Resistance1 As Double
    Public Shared Resistance2 As Double

    'For new graphical interface
    Public Shared WithEvents f As New List(Of SimpleDynoSubForm)

    'NOTE - The following delclarations use the largest primary and secondary dimensions
    'Friend" declarations are to allow passing the information to the new graphical interface classes
    Public Shared Data(LAST - 1, MINCURMAXPOINTER) As Double
    Public Shared DataTags(LAST - 1) As String
    Public Shared DataUnits(LAST - 1, 5) As Double 'allows for 5 different units for each Data value
    Public Shared DataUnitTags(LAST - 1) As String 'labels for the Various units
    Public Shared DataAreUsed(LAST - 1) As Boolean

    'Use the new Data Structure approach for the collected data from power runs
    Public Shared CollectedData(LAST - 1, MAXDATAPOINTS) As Double

    'General
    Private i As Integer, j As Integer, k As Integer, k2 As Integer
    Private AllowedCharacters As String = "0123456789" & Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

    Private AvailableChannels As String() = {"1 Channel", "2 Channels"}
    Private AvailableSampleRates As String() = {"11025 Hertz", "22050 Hertz", "44100 Hertz"}
    Private AvailableBufferSizes As String() = {"256 bytes", "512 bytes", "1024 bytes", "2048 bytes", "4096 bytes"}

    Private AcquisitionOptions As String() = {"Audio Only", "Audio & COM Port Sensing", "COM Port Only"}

    Public Shared Formloaded As Boolean = False

    'Wave Specific
    Public Shared SAMPLE_RATE As Integer
    Public Shared NUMBER_OF_CHANNELS As Int32
    Private BUFFER_SIZE As Int32
    Private BytesToSeconds As Double
    Private ElapsedTimeUnit As Double
    Private bufferpin0 As GCHandle
    Private bufferpin1 As GCHandle
    Private bufferpin2 As GCHandle
    Private bufferpin3 As GCHandle
    Private bufferpin4 As GCHandle
    Private bufferpin5 As GCHandle
    Private bufferpin6 As GCHandle
    Private bufferpin7 As GCHandle
    Private bufferpin8 As GCHandle
    Private bufferpin9 As GCHandle
    Private bufferpin10 As GCHandle
    Private bufferpin11 As GCHandle
    Private bufferpin12 As GCHandle
    Private bufferpin13 As GCHandle
    Private bufferpin14 As GCHandle
    Private bufferpin15 As GCHandle
    Private bufferpin16 As GCHandle
    Private bufferpin17 As GCHandle
    Private bufferpin18 As GCHandle
    Private bufferpin19 As GCHandle
    Private RawWaveData() As Byte
    Private RawWaveData0() As Byte
    Private RawWaveData1() As Byte
    Private RawWaveData2() As Byte
    Private RawWaveData3() As Byte
    Private RawWaveData4() As Byte
    Private RawWaveData5() As Byte
    Private RawWaveData6() As Byte
    Private RawWaveData7() As Byte
    Private RawWaveData8() As Byte
    Private RawWaveData9() As Byte
    Private RawWaveData10() As Byte
    Private RawWaveData11() As Byte
    Private RawWaveData12() As Byte
    Private RawWaveData13() As Byte
    Private RawWaveData14() As Byte
    Private RawWaveData15() As Byte
    Private RawWaveData16() As Byte
    Private RawWaveData17() As Byte
    Private RawWaveData18() As Byte
    Private RawWaveData19() As Byte
    Private waveFormat As WAVEFORMATEX
    Private WaveBufferHeaders() As WAVEHDR
    Private WaveInHandle As IntPtr
    Private BufferCount As Int32 = 0
    Private WavesStarted As Boolean = False
    Private WaitForNewSignal As Double
    Private InCallBackProcedure As Boolean = False
    Private UseAdvancedProcessing As Boolean = False


    'Signal Plotting Specific
    Private SignalPinned As Boolean = False
    Private SignalWindowBMP As Graphics
    Public SignalBitmap As Bitmap
    Private Channel1SignalPen As New Pen(Color.Red)
    Private Channel2SignalPen As New Pen(Color.Yellow)
    Private Channel1ThresholdPen As New Pen(Color.Green)
    Private Channel2ThresholdPen As New Pen(Color.Blue)
    Private LastYPosition As Integer
    Private NextYPosition As Integer
    Private LastYPosition2 As Integer
    Private NextYPosition2 As Integer
    Private PicSignalHeight As Integer
    Private PicSignalWidth As Integer
    Private SignalXConversion As Double
    Private SignalYConversion As Double
    Private SignalThresholdYConverted As Integer
    Private SignalThreshold2YConverted As Integer
    Private CurrentSignalXPosition As Double = 0

    'Data Collection Mode
    Public Shared WhichDataMode As Integer
    Public Const LIVE As Integer = 0
    Public Const LOGRAW As Integer = 1
    Public Const POWERRUN As Integer = 2

    'Dyno Specific Variables that need to be declared here for performance reasons
    Public Shared GearRatio As Double
    Public Shared WheelCircumference As Double

    'High and Low Signal Thresholds, Channels 1 and 2
    Public Shared HighSignalThreshold As Double
    Public Shared HighSignalThreshold2 As Double

    'Dyno Calculations Specific
    Public Shared DynoMomentOfInertia As Double
    Public Shared IdealMomentOfInertia As Double
    Public Shared IdealRollerMass As Double
    Public Shared ForceAir As Double
    Public Shared RollerRPMtoSpeed As Double
    Public Shared RollerRadsPerSecToMetersPerSec As Double
    Public Shared RollerRPMtoWheelRPM As Double
    Public Shared RollerRPMtoMotorRPM As Double
    Public Shared FoundHighSignal2 As Boolean = False
    Public Shared FoundHighSignal As Boolean = False
    Public Shared LastHighBufferPosition2 As Integer
    Public Shared LastHighBufferPosition As Integer
    Public Shared ElapsedTime2 As Double
    Public Shared TempTorqueMax As Double
    Public Shared OldAngularVelocity As Double
    Public Shared ElapsedTime As Double
    Public Shared LastSignal As Integer
    Public Shared NewElapsedTimeCorrection As Double
    Public Shared OldElapsedTimeCorrection As Double
    Public Shared LastSignal2 As Integer
    Public Shared NewElapsedTimeCorrection2 As Double
    Public Shared OldElapsedTimeCorrection2 As Double
    Public Shared ElapsedTimeToRadPerSec As Double
    Public Shared ElapsedTimeToRadPerSec2 As Double
    Public Shared TotalElapsedTime As Double
    Public Shared TotalElapsedTime2 As Double
    Private WhichSignal As Integer
    Private WhichSignal2 As Integer
    Private Const HIGHSIGNAL As Integer = 0
    Private Const LOWSIGNAL As Integer = 1
    Public Shared SpeedAir As Double

    'File Handling Specific
    Private SettingsDirectory As String = "C:\SimpleDyno"
    Private SettingsFile As String = "\SimpleDynoSettings.sds"
    Private DefaultViewFile As String = "\DefaultView.sdi"
    Private DataOutputFile As StreamWriter
    'Private DataInputFile As StreamReader 'In Main, this is used to load other peoples raw data when GearRatio is 999
    Private LogRawDataFileName As String
    Public Shared LogPowerRunDataFileName As String
    'Private ParameterInputFile As StreamReader
    'Private ParameterOutputFile As StreamWriter

    'Raw data and curve fitting specific
    Public Shared StopFitting As Boolean = False
    Public Shared ProcessingData As Boolean = False
    Public Shared DataPoints As Integer
    Public Shared PowerRunThreshold As Double ' = 1
    Private ActualPowerRunThreshold As Double
    Private MinimumPowerRunPoints As Double
    Private StopAddingBuffers As Boolean = False

#End Region
#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtPowerRunThreshold As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnStartLoggingRaw As System.Windows.Forms.Button
    Friend WithEvents btnResetMaxima As System.Windows.Forms.Button
    Public WithEvents btnStartPowerRun As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents txtZeroTimeDetect As System.Windows.Forms.TextBox
    Friend WithEvents lblZeroDetect As System.Windows.Forms.Label
    Friend WithEvents pnlSignalWindow As SimpleDyno.DoubleBufferPanel
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnMultiYTime As System.Windows.Forms.Button
    Friend WithEvents btnNewGauge As System.Windows.Forms.Button
    Friend WithEvents btnNewLabel As System.Windows.Forms.Button
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents btnHide As System.Windows.Forms.Button
    Friend WithEvents btnSaveAs As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnStartAcquisition As System.Windows.Forms.Button
    Friend WithEvents cmbAcquisition As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSampleRate As System.Windows.Forms.ComboBox
    Friend WithEvents cmbChannels As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBaudRate As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCOMPorts As System.Windows.Forms.ComboBox
    Friend WithEvents lblCOMActive As System.Windows.Forms.Label
    Friend WithEvents txtThreshold2 As System.Windows.Forms.TextBox
    Friend WithEvents txtThreshold1 As System.Windows.Forms.TextBox
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents btnAnalysis As System.Windows.Forms.Button
    Friend WithEvents btnDyno As System.Windows.Forms.Button
    Friend WithEvents lblInterface As System.Windows.Forms.Label
    Friend WithEvents txtInterface As System.Windows.Forms.TextBox
    Friend WithEvents chkAdvancedProcessing As System.Windows.Forms.CheckBox
    Friend WithEvents cmbBufferSize As System.Windows.Forms.ComboBox
    Friend WithEvents btnPerformanceTest As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.btnStartLoggingRaw = New System.Windows.Forms.Button()
        Me.btnResetMaxima = New System.Windows.Forms.Button()
        Me.btnStartPowerRun = New System.Windows.Forms.Button()
        Me.btnCOM = New System.Windows.Forms.Button()
        Me.btnDyno = New System.Windows.Forms.Button()
        Me.btnAnalysis = New System.Windows.Forms.Button()
        Me.txtThreshold2 = New System.Windows.Forms.TextBox()
        Me.txtThreshold1 = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnMultiYTime = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNewGauge = New System.Windows.Forms.Button()
        Me.btnSaveAs = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnNewLabel = New System.Windows.Forms.Button()
        Me.btnHide = New System.Windows.Forms.Button()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.txtPowerRunThreshold = New System.Windows.Forms.TextBox()
        Me.txtZeroTimeDetect = New System.Windows.Forms.TextBox()
        Me.lblZeroDetect = New System.Windows.Forms.Label()
        Me.btnStartAcquisition = New System.Windows.Forms.Button()
        Me.cmbAcquisition = New System.Windows.Forms.ComboBox()
        Me.cmbSampleRate = New System.Windows.Forms.ComboBox()
        Me.cmbChannels = New System.Windows.Forms.ComboBox()
        Me.cmbBaudRate = New System.Windows.Forms.ComboBox()
        Me.cmbCOMPorts = New System.Windows.Forms.ComboBox()
        Me.lblCOMActive = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.lblInterface = New System.Windows.Forms.Label()
        Me.txtInterface = New System.Windows.Forms.TextBox()
        Me.chkAdvancedProcessing = New System.Windows.Forms.CheckBox()
        Me.cmbBufferSize = New System.Windows.Forms.ComboBox()
        Me.btnPerformanceTest = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.pnlSignalWindow = New SimpleDyno.DoubleBufferPanel()
        Me.SuspendLayout()
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Text files (*.txt)|*.txt"
        '
        'btnStartLoggingRaw
        '
        Me.btnStartLoggingRaw.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStartLoggingRaw.Location = New System.Drawing.Point(274, 1)
        Me.btnStartLoggingRaw.Name = "btnStartLoggingRaw"
        Me.btnStartLoggingRaw.Size = New System.Drawing.Size(68, 21)
        Me.btnStartLoggingRaw.TabIndex = 42
        Me.btnStartLoggingRaw.Text = "Log Raw Data"
        '
        'btnResetMaxima
        '
        Me.btnResetMaxima.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResetMaxima.Location = New System.Drawing.Point(138, 1)
        Me.btnResetMaxima.Name = "btnResetMaxima"
        Me.btnResetMaxima.Size = New System.Drawing.Size(68, 21)
        Me.btnResetMaxima.TabIndex = 41
        Me.btnResetMaxima.Text = "Reset"
        '
        'btnStartPowerRun
        '
        Me.btnStartPowerRun.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStartPowerRun.Location = New System.Drawing.Point(342, 1)
        Me.btnStartPowerRun.Name = "btnStartPowerRun"
        Me.btnStartPowerRun.Size = New System.Drawing.Size(68, 21)
        Me.btnStartPowerRun.TabIndex = 43
        Me.btnStartPowerRun.Text = "Power Run"
        '
        'btnCOM
        '
        Me.btnCOM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCOM.Location = New System.Drawing.Point(70, 1)
        Me.btnCOM.Name = "btnCOM"
        Me.btnCOM.Size = New System.Drawing.Size(68, 21)
        Me.btnCOM.TabIndex = 172
        Me.btnCOM.Text = "COM"
        Me.btnCOM.UseVisualStyleBackColor = True
        '
        'btnDyno
        '
        Me.btnDyno.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDyno.Location = New System.Drawing.Point(2, 1)
        Me.btnDyno.Name = "btnDyno"
        Me.btnDyno.Size = New System.Drawing.Size(68, 21)
        Me.btnDyno.TabIndex = 170
        Me.btnDyno.Text = "Dyno"
        Me.btnDyno.UseVisualStyleBackColor = True
        '
        'btnAnalysis
        '
        Me.btnAnalysis.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAnalysis.Location = New System.Drawing.Point(206, 1)
        Me.btnAnalysis.Name = "btnAnalysis"
        Me.btnAnalysis.Size = New System.Drawing.Size(68, 21)
        Me.btnAnalysis.TabIndex = 171
        Me.btnAnalysis.Text = "Analysis"
        Me.btnAnalysis.UseVisualStyleBackColor = True
        '
        'txtThreshold2
        '
        Me.txtThreshold2.CausesValidation = False
        Me.txtThreshold2.Enabled = False
        Me.txtThreshold2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThreshold2.Location = New System.Drawing.Point(396, 68)
        Me.txtThreshold2.Name = "txtThreshold2"
        Me.txtThreshold2.Size = New System.Drawing.Size(23, 21)
        Me.txtThreshold2.TabIndex = 169
        Me.txtThreshold2.Tag = ""
        Me.txtThreshold2.Text = "113"
        Me.txtThreshold2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtThreshold2.Visible = False
        '
        'txtThreshold1
        '
        Me.txtThreshold1.CausesValidation = False
        Me.txtThreshold1.Enabled = False
        Me.txtThreshold1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThreshold1.Location = New System.Drawing.Point(367, 67)
        Me.txtThreshold1.Name = "txtThreshold1"
        Me.txtThreshold1.Size = New System.Drawing.Size(23, 21)
        Me.txtThreshold1.TabIndex = 168
        Me.txtThreshold1.Tag = ""
        Me.txtThreshold1.Text = "143"
        Me.txtThreshold1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtThreshold1.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Enabled = False
        Me.btnClose.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(206, 90)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(68, 21)
        Me.btnClose.TabIndex = 86
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnMultiYTime
        '
        Me.btnMultiYTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMultiYTime.Location = New System.Drawing.Point(138, 68)
        Me.btnMultiYTime.Name = "btnMultiYTime"
        Me.btnMultiYTime.Size = New System.Drawing.Size(68, 21)
        Me.btnMultiYTime.TabIndex = 85
        Me.btnMultiYTime.Text = "Y vs Time"
        Me.btnMultiYTime.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnLoad.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(2, 90)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(68, 21)
        Me.btnLoad.TabIndex = 77
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.Enabled = False
        Me.btnSave.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(70, 90)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(68, 21)
        Me.btnSave.TabIndex = 78
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNewGauge
        '
        Me.btnNewGauge.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewGauge.Location = New System.Drawing.Point(70, 68)
        Me.btnNewGauge.Name = "btnNewGauge"
        Me.btnNewGauge.Size = New System.Drawing.Size(68, 21)
        Me.btnNewGauge.TabIndex = 83
        Me.btnNewGauge.Text = "Gauge"
        Me.btnNewGauge.UseVisualStyleBackColor = True
        '
        'btnSaveAs
        '
        Me.btnSaveAs.Enabled = False
        Me.btnSaveAs.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveAs.Location = New System.Drawing.Point(138, 90)
        Me.btnSaveAs.Name = "btnSaveAs"
        Me.btnSaveAs.Size = New System.Drawing.Size(68, 21)
        Me.btnSaveAs.TabIndex = 79
        Me.btnSaveAs.Text = "Save As"
        Me.btnSaveAs.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(274, 27)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(66, 13)
        Me.Label17.TabIndex = 58
        Me.Label17.Text = "Run Start at"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNewLabel
        '
        Me.btnNewLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewLabel.Location = New System.Drawing.Point(2, 68)
        Me.btnNewLabel.Name = "btnNewLabel"
        Me.btnNewLabel.Size = New System.Drawing.Size(68, 21)
        Me.btnNewLabel.TabIndex = 82
        Me.btnNewLabel.Text = "Label"
        Me.btnNewLabel.UseVisualStyleBackColor = True
        '
        'btnHide
        '
        Me.btnHide.Enabled = False
        Me.btnHide.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHide.Location = New System.Drawing.Point(274, 90)
        Me.btnHide.Name = "btnHide"
        Me.btnHide.Size = New System.Drawing.Size(68, 21)
        Me.btnHide.TabIndex = 80
        Me.btnHide.Text = "Hide"
        Me.btnHide.UseVisualStyleBackColor = True
        '
        'btnShow
        '
        Me.btnShow.Enabled = False
        Me.btnShow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShow.Location = New System.Drawing.Point(342, 90)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(68, 21)
        Me.btnShow.TabIndex = 81
        Me.btnShow.Text = "Show"
        Me.btnShow.UseVisualStyleBackColor = True
        '
        'txtPowerRunThreshold
        '
        Me.txtPowerRunThreshold.CausesValidation = False
        Me.txtPowerRunThreshold.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPowerRunThreshold.Location = New System.Drawing.Point(342, 24)
        Me.txtPowerRunThreshold.Name = "txtPowerRunThreshold"
        Me.txtPowerRunThreshold.Size = New System.Drawing.Size(67, 21)
        Me.txtPowerRunThreshold.TabIndex = 44
        Me.txtPowerRunThreshold.Tag = "1\999999"
        Me.txtPowerRunThreshold.Text = "0"
        Me.txtPowerRunThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtZeroTimeDetect
        '
        Me.txtZeroTimeDetect.CausesValidation = False
        Me.txtZeroTimeDetect.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZeroTimeDetect.Location = New System.Drawing.Point(492, 68)
        Me.txtZeroTimeDetect.Name = "txtZeroTimeDetect"
        Me.txtZeroTimeDetect.Size = New System.Drawing.Size(38, 21)
        Me.txtZeroTimeDetect.TabIndex = 51
        Me.txtZeroTimeDetect.Tag = "0.1\2"
        Me.txtZeroTimeDetect.Text = "1"
        Me.txtZeroTimeDetect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblZeroDetect
        '
        Me.lblZeroDetect.AutoSize = True
        Me.lblZeroDetect.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZeroDetect.Location = New System.Drawing.Point(426, 72)
        Me.lblZeroDetect.Name = "lblZeroDetect"
        Me.lblZeroDetect.Size = New System.Drawing.Size(64, 13)
        Me.lblZeroDetect.TabIndex = 32
        Me.lblZeroDetect.Text = "Zero Detect"
        Me.lblZeroDetect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnStartAcquisition
        '
        Me.btnStartAcquisition.Location = New System.Drawing.Point(425, 90)
        Me.btnStartAcquisition.Name = "btnStartAcquisition"
        Me.btnStartAcquisition.Size = New System.Drawing.Size(105, 21)
        Me.btnStartAcquisition.TabIndex = 163
        Me.btnStartAcquisition.Text = "Start"
        Me.btnStartAcquisition.UseVisualStyleBackColor = True
        '
        'cmbAcquisition
        '
        Me.cmbAcquisition.FormattingEnabled = True
        Me.cmbAcquisition.Location = New System.Drawing.Point(426, 2)
        Me.cmbAcquisition.Name = "cmbAcquisition"
        Me.cmbAcquisition.Size = New System.Drawing.Size(171, 21)
        Me.cmbAcquisition.TabIndex = 162
        '
        'cmbSampleRate
        '
        Me.cmbSampleRate.FormattingEnabled = True
        Me.cmbSampleRate.Location = New System.Drawing.Point(508, 24)
        Me.cmbSampleRate.Name = "cmbSampleRate"
        Me.cmbSampleRate.Size = New System.Drawing.Size(89, 21)
        Me.cmbSampleRate.TabIndex = 161
        '
        'cmbChannels
        '
        Me.cmbChannels.FormattingEnabled = True
        Me.cmbChannels.Location = New System.Drawing.Point(426, 24)
        Me.cmbChannels.Name = "cmbChannels"
        Me.cmbChannels.Size = New System.Drawing.Size(81, 21)
        Me.cmbChannels.TabIndex = 160
        '
        'cmbBaudRate
        '
        Me.cmbBaudRate.FormattingEnabled = True
        Me.cmbBaudRate.Location = New System.Drawing.Point(531, 46)
        Me.cmbBaudRate.Name = "cmbBaudRate"
        Me.cmbBaudRate.Size = New System.Drawing.Size(66, 21)
        Me.cmbBaudRate.TabIndex = 159
        '
        'cmbCOMPorts
        '
        Me.cmbCOMPorts.DropDownWidth = 300
        Me.cmbCOMPorts.FormattingEnabled = True
        Me.cmbCOMPorts.Location = New System.Drawing.Point(426, 46)
        Me.cmbCOMPorts.Name = "cmbCOMPorts"
        Me.cmbCOMPorts.Size = New System.Drawing.Size(104, 21)
        Me.cmbCOMPorts.TabIndex = 158
        '
        'lblCOMActive
        '
        Me.lblCOMActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCOMActive.Location = New System.Drawing.Point(531, 91)
        Me.lblCOMActive.Name = "lblCOMActive"
        Me.lblCOMActive.Size = New System.Drawing.Size(66, 19)
        Me.lblCOMActive.TabIndex = 157
        Me.lblCOMActive.Text = "COM Active"
        Me.lblCOMActive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lblInterface
        '
        Me.lblInterface.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterface.Location = New System.Drawing.Point(1, 54)
        Me.lblInterface.Name = "lblInterface"
        Me.lblInterface.Size = New System.Drawing.Size(205, 13)
        Me.lblInterface.TabIndex = 174
        Me.lblInterface.Text = "Currently using:"
        Me.lblInterface.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtInterface
        '
        Me.txtInterface.CausesValidation = False
        Me.txtInterface.Enabled = False
        Me.txtInterface.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterface.Location = New System.Drawing.Point(342, 68)
        Me.txtInterface.Name = "txtInterface"
        Me.txtInterface.Size = New System.Drawing.Size(23, 21)
        Me.txtInterface.TabIndex = 175
        Me.txtInterface.Tag = ""
        Me.txtInterface.Text = "C:\SimpleDyno\DefaultView.sdi"
        Me.txtInterface.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtInterface.Visible = False
        '
        'chkAdvancedProcessing
        '
        Me.chkAdvancedProcessing.AutoSize = True
        Me.chkAdvancedProcessing.Location = New System.Drawing.Point(531, 71)
        Me.chkAdvancedProcessing.Name = "chkAdvancedProcessing"
        Me.chkAdvancedProcessing.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkAdvancedProcessing.Size = New System.Drawing.Size(45, 17)
        Me.chkAdvancedProcessing.TabIndex = 182
        Me.chkAdvancedProcessing.Text = "Adv"
        Me.chkAdvancedProcessing.UseVisualStyleBackColor = True
        '
        'cmbBufferSize
        '
        Me.cmbBufferSize.FormattingEnabled = True
        Me.cmbBufferSize.Location = New System.Drawing.Point(300, 68)
        Me.cmbBufferSize.Name = "cmbBufferSize"
        Me.cmbBufferSize.Size = New System.Drawing.Size(36, 21)
        Me.cmbBufferSize.TabIndex = 183
        Me.cmbBufferSize.Visible = False
        '
        'btnPerformanceTest
        '
        Me.btnPerformanceTest.Enabled = False
        Me.btnPerformanceTest.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPerformanceTest.Location = New System.Drawing.Point(214, 67)
        Me.btnPerformanceTest.Name = "btnPerformanceTest"
        Me.btnPerformanceTest.Size = New System.Drawing.Size(36, 22)
        Me.btnPerformanceTest.TabIndex = 184
        Me.btnPerformanceTest.Text = "Perf"
        Me.btnPerformanceTest.UseVisualStyleBackColor = True
        Me.btnPerformanceTest.Visible = False
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(2, 23)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(68, 21)
        Me.Button1.TabIndex = 185
        Me.Button1.Text = "Correction"
        '
        'pnlSignalWindow
        '
        Me.pnlSignalWindow.BackColor = System.Drawing.SystemColors.Control
        Me.pnlSignalWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlSignalWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSignalWindow.Location = New System.Drawing.Point(599, 2)
        Me.pnlSignalWindow.Name = "pnlSignalWindow"
        Me.pnlSignalWindow.Size = New System.Drawing.Size(25, 108)
        Me.pnlSignalWindow.TabIndex = 33
        '
        'Main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.AutoScroll = True
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(626, 112)
        Me.Controls.Add(Me.txtThreshold1)
        Me.Controls.Add(Me.pnlSignalWindow)
        Me.Controls.Add(Me.txtThreshold2)
        Me.Controls.Add(Me.btnStartAcquisition)
        Me.Controls.Add(Me.lblCOMActive)
        Me.Controls.Add(Me.cmbCOMPorts)
        Me.Controls.Add(Me.cmbBaudRate)
        Me.Controls.Add(Me.cmbChannels)
        Me.Controls.Add(Me.cmbSampleRate)
        Me.Controls.Add(Me.cmbAcquisition)
        Me.Controls.Add(Me.lblZeroDetect)
        Me.Controls.Add(Me.txtZeroTimeDetect)
        Me.Controls.Add(Me.txtInterface)
        Me.Controls.Add(Me.lblInterface)
        Me.Controls.Add(Me.btnCOM)
        Me.Controls.Add(Me.btnDyno)
        Me.Controls.Add(Me.btnAnalysis)
        Me.Controls.Add(Me.btnResetMaxima)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnMultiYTime)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.txtPowerRunThreshold)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.btnStartPowerRun)
        Me.Controls.Add(Me.btnStartLoggingRaw)
        Me.Controls.Add(Me.btnNewGauge)
        Me.Controls.Add(Me.btnHide)
        Me.Controls.Add(Me.btnSaveAs)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.btnNewLabel)
        Me.Controls.Add(Me.chkAdvancedProcessing)
        Me.Controls.Add(Me.btnPerformanceTest)
        Me.Controls.Add(Me.cmbBufferSize)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SimpleDyno 6_2_5 Beta by DamoRC"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
#Region "Form Load, WndProc, Button and Trackbar Events, Delgates and Close"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
#If QueryPerformance Then
        btnPerformanceTest.Visible = True
                cmbBufferSize.Visible = True
#Else
        btnPerformanceTest.Visible = False
        cmbBufferSize.Visible = False
#End If

        'Create Instances of the sub forms
        frmDyno = New Dyno
        frmCOM = New COM
        frmAnalysis = New Analysis
        frmFit = New Fit
        frmCorrection = New Correction

        'Populate combo boxes
        cmbChannels.Items.AddRange(AvailableChannels)
        cmbChannels.SelectedIndex = 0
        cmbSampleRate.Items.AddRange(AvailableSampleRates)
        cmbSampleRate.SelectedIndex = 0
        cmbBufferSize.Items.AddRange(AvailableBufferSizes)
        cmbBufferSize.SelectedIndex = 0

        'Setup the arrays for the data structure
        PrepareArrays()

        'Check and load available COM Ports
        GetAvailableCOMPorts()

        If COMPortsAvailable = True Then
            cmbAcquisition.Items.AddRange(AcquisitionOptions)
        Else
            cmbAcquisition.Items.Add(AcquisitionOptions(0))
        End If
        cmbAcquisition.SelectedIndex = 0

        frmAnalysis.Analysis_Setup()
        frmFit.Fit_Setup()

        'Load saved setting
        LoadParametersFromFile()

        frmDyno.Dyno_Setup()
        frmCOM.COM_Setup()

        'Setup graphics data
        PrepareGraphicsParameters()

        SetupTextBoxCharacterHandling()

        'Set Size and Title
        Me.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Me.Height
        Me.Text = MainTitle

        'Open Up the default interface
        LoadInterface()
    End Sub
    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Formloaded = True
    End Sub
    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        SaveParametersToFile()
        btnClose_Click(Me, EventArgs.Empty)
        ShutDownWaves()
        myCallBackFunction = Nothing
        SerialClose()
    End Sub
    Private Sub btnDyno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDyno.Click
        btnHide_Click(Me, EventArgs.Empty)
        frmDyno.ShowDialog()
    End Sub
    Private Sub btnAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnalysis.Click
        btnHide_Click(Me, EventArgs.Empty)
        frmAnalysis.ShowDialog()
        frmAnalysis.pnlOverlaySetup()
    End Sub
    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click
        btnHide_Click(Me, EventArgs.Empty)
        frmCOM.ShowDialog()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        btnHide_Click(Me, EventArgs.Empty)
        frmCorrection.ShowDialog()
    End Sub
    Private Sub pnlSignalWindow_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlSignalWindow.MouseClick
        If e.Button = MouseButtons.Right Then
            'set the RPM2 channel threshold
            txtThreshold2.Text = (256 - (pnlSignalWindow.PointToClient(Control.MousePosition).Y) / pnlSignalWindow.Height * 256).ToString
            HighSignalThreshold2 = CDbl(txtThreshold2.Text) ' 256 - (pnlSignalWindow.PointToClient(Control.MousePosition).Y) / pnlSignalWindow.Height * 256
            If HighSignalThreshold2 > 128 Then WhichSignal2 = HIGHSIGNAL Else WhichSignal2 = LOWSIGNAL
            SignalThreshold2YConverted = CInt(PicSignalHeight - HighSignalThreshold2 * SignalYConversion)
        ElseIf e.Button = MouseButtons.Left Then
            If pnlSignalWindow.Right - pnlSignalWindow.PointToClient(Control.MousePosition).X > 10 Then
                'set the RPM1 channel threshold
                txtThreshold1.Text = (256 - (pnlSignalWindow.PointToClient(Control.MousePosition).Y) / pnlSignalWindow.Height * 256).ToString
                HighSignalThreshold = CDbl(txtThreshold1.Text) '256 - (pnlSignalWindow.PointToClient(Control.MousePosition).Y) / pnlSignalWindow.Height * 256
                If HighSignalThreshold > 128 Then WhichSignal = HIGHSIGNAL Else WhichSignal = LOWSIGNAL
                SignalThresholdYConverted = CInt(PicSignalHeight - HighSignalThreshold * SignalYConversion)
            Else
                SignalPinned = Not SignalPinned
            End If

        End If
    End Sub
    Private Sub pnlSignalWindow_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlSignalWindow.MouseEnter
        If WavesStarted Then
            With pnlSignalWindow
                .Left = .Right - Me.Width + 10
                .Width = Me.Width - 10
            End With
            PrepareGraphicsParameters()
        End If
    End Sub
    Private Sub pnlSignalWindow_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlSignalWindow.MouseLeave
        'Debug.Print(SignalPinned.ToString)
        If WavesStarted Then
            If SignalPinned = False Then
                With pnlSignalWindow
                    .Left = .Right - 25
                    .Width = 25
                End With
                PrepareGraphicsParameters()
            End If
        End If
    End Sub

    Private Sub pnlSignalWindow_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlSignalWindow.MouseMove
        If pnlSignalWindow.Right - pnlSignalWindow.PointToClient(Control.MousePosition).X < 10 Then
            Me.Cursor = Cursors.Hand
        Else
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub pnlSignalWindow_Paint_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlSignalWindow.Paint
        pnlSignalWindow.BackgroundImage = SignalBitmap
    End Sub

    Delegate Sub SetControlBackColor_Delegate(ByVal [Control] As Control, ByVal [Color] As Color)
    Private Sub SetControlBackColor_ThreadSafe(ByVal [Control] As Control, ByVal [Color] As Color)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetControlBackColor_Delegate(AddressOf SetControlBackColor_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control], [Color]})
        Else
            Control.BackColor = [Color]
        End If
    End Sub
    Delegate Sub SetControlEnabled_Delegate(ByVal [Control] As Control)
    Friend Sub SetControlEnabled_ThreadSafe(ByVal [Control] As Control)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetControlEnabled_Delegate(AddressOf SetControlEnabled_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control]})
        Else
            [Control].Enabled = True
        End If
    End Sub
    Delegate Sub SetMouseBusy_Delegate(ByVal [Control] As Control)
    Friend Sub SetMouseBusy_ThreadSafe(ByVal [Control] As Control)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetMouseBusy_Delegate(AddressOf SetMouseBusy_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control]})
        Else
            [Control].Cursor = Cursors.WaitCursor
        End If
    End Sub
    Delegate Sub SetMouseNormal_Delegate(ByVal [Control] As Control)
    Friend Sub SetMouseNormal_ThreadSafe(ByVal [Control] As Control)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetMouseNormal_Delegate(AddressOf SetMouseNormal_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control]})
        Else
            [Control].Cursor = Cursors.Arrow
        End If
    End Sub
    Delegate Sub SetControlDisabled_Delegate(ByVal [Control] As Control)
    Friend Sub SetControlDisabled_ThreadSafe(ByVal [Control] As Control)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetControlDisabled_Delegate(AddressOf SetControlDisabled_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control]})
        Else
            [Control].Enabled = False
        End If
    End Sub
    Delegate Sub PicRefresh_Delegate(ByVal [Picture] As PictureBox)
    Friend Sub PicRefresh_ThreadSafe(ByVal [Picture] As PictureBox)
        If [Picture].InvokeRequired Then
            Dim MyDelegate As New PicRefresh_Delegate(AddressOf PicRefresh_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Picture]})
        Else
            [Picture].Refresh()
        End If
    End Sub
    Delegate Sub ControlRefresh_Delegate(ByVal [Control] As Control)
    Friend Sub ControlRefresh_ThreadSafe(ByVal [Control] As Control)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New ControlRefresh_Delegate(AddressOf ControlRefresh_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control]})
        Else
            [Control].Refresh()
        End If
    End Sub
    Delegate Sub ControlUpdate_Delegate(ByVal [Control] As Control)
    Friend Sub ControlUpdate_ThreadSafe(ByVal [Control] As Control)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New ControlUpdate_Delegate(AddressOf ControlUpdate_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Control]})
        Else
            [Control].Update()
        End If
    End Sub
    Delegate Sub SetControlText_Delegate(ByVal [Control] As Control, ByVal [text] As String)
    Public Sub SetControlText_Threadsafe(ByVal [Control] As Control, ByVal [text] As String)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetControlText_Delegate(AddressOf SetControlText_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[Control], [text]})
        Else
            [Control].Text = [text]
        End If
    End Sub
    Delegate Sub ClickButtonThreadsafe_Delegate(ByVal [Button] As Button, ByVal [sender] As Object, ByVal [Args] As System.EventArgs)
    Friend Sub ClickButton_Threadsafe(ByVal [Button] As Button, ByVal [sender] As Object, ByVal [Args] As System.EventArgs)
        If [Button].InvokeRequired Then
            Dim MyDelegate As New ClickButtonThreadsafe_Delegate(AddressOf ClickButton_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[Button], [Text], [sender], [Args]})
        Else

        End If
    End Sub
    Private Sub chkAdvancedProcessing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAdvancedProcessing.CheckedChanged
        If chkAdvancedProcessing.Checked = True Then
            UseAdvancedProcessing = True
        Else
            UseAdvancedProcessing = False
        End If
    End Sub
    Friend Function CheckNumericalLimits(ByVal SentMin As Double, ByVal SentMax As Double, ByVal SentValue As Double) As Boolean
        If SentValue >= SentMin AndAlso SentValue <= SentMax Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub btnStartPowerRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartPowerRun.Click
        Try
            'Regardless of why we clicked it, the radio button for rpm on the fit form should be set for RPM
            frmFit.rdoRPM1.Checked = True
            If WhichDataMode = POWERRUN Then 'We are cancelling the power run
                'CHECK - WE SHOULD REALLY MAKE SURE WE UNDO EVERYTHING THAT WAS DONE WHEN THE POWERRUN WAS INITIATED
                btnStartLoggingRaw.Enabled = True
                btnShow_Click(Me, EventArgs.Empty)
                With btnStartPowerRun
                    .BackColor = System.Windows.Forms.Control.DefaultBackColor
                End With
                StopFitting = True
                WhichDataMode = LIVE
            Else
                btnHide_Click(Me, EventArgs.Empty)
                With SaveFileDialog1
                    .Reset()
                    .Filter = "Power Run files (*.sdp)|*.sdp"
                    .ShowDialog()
                End With
                If SaveFileDialog1.FileName <> "" Then
                    LogPowerRunDataFileName = SaveFileDialog1.FileName
                    ResetValues()
                    DataPoints = 0
                    ' DataPoints2 = 0
                    btnStartLoggingRaw.Enabled = False
                    btnShow_Click(Me, EventArgs.Empty)
                    With btnStartPowerRun
                        .BackColor = Color.Red
                    End With
                    WhichDataMode = POWERRUN
                    StopFitting = False
                    frmFit.ProcessData()
                Else
                    btnShow_Click(Me, EventArgs.Empty)
                End If
            End If
        Catch e1 As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("btnStartPowerRun_Click Error: " & e1.ToString, MsgBoxStyle.Exclamation)
            btnShow_Click(Me, EventArgs.Empty)
            End
        End Try
    End Sub
    Private Sub btnResetMaxima_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetMaxima.Click
        ResetValues()
    End Sub
    Private Sub btnStartLoggingRaw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartLoggingRaw.Click
        'Copied power run code
        Try
            If WhichDataMode = LOGRAW Then 'We are stopping the log raw session and should write the data
                'WRITE THE DATA
                Dim DataOutputFile As New System.IO.StreamWriter(LogRawDataFileName)
                With DataOutputFile
                    'NOTE: The data files are space delimited
                    'Write out the header information
                    .WriteLine(LogRawVersion) 'Confirms log raw version
                    .WriteLine(LogRawDataFileName & vbCrLf & DateAndTime.Today.ToString & vbCrLf)
                    .WriteLine("Acquisition: " & cmbAcquisition.SelectedItem.ToString)
                    .WriteLine("Number_of_Channels: " & NUMBER_OF_CHANNELS.ToString)
                    .WriteLine("Sampling_Rate " & SAMPLE_RATE.ToString)
                    If cmbCOMPorts.SelectedItem IsNot Nothing Then
                        .WriteLine("COM_Port: " & cmbCOMPorts.SelectedItem.ToString)
                    Else
                        .WriteLine("No_COM_Port_Selected")
                    End If
                    If cmbBaudRate.SelectedItem IsNot Nothing Then
                        .WriteLine("Baud_Rate: " & cmbBaudRate.SelectedItem.ToString)
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
                    .WriteLine("Channel_1_Threshold " & HighSignalThreshold.ToString)
                    .WriteLine("Channel_2_Threshold " & HighSignalThreshold2.ToString)
                    'The following not needed for Log Raw
                    '.WriteLine("Run_RPM_Threshold " & PowerRunThreshold.ToString)
                    '.WriteLine("Run_Spike_Removal_Threshold " & Fit.PowerRunSpikeLevel.ToString)
                    .WriteLine(vbCrLf)

                    'Create the column headings string based on the Data structure 
                    'Only Primary SI units of the values are written
                    Dim tempstring As String = ""
                    Dim tempsplit As String()
                    Dim paramcount As Integer
                    Dim count As Integer

                    'Add the raw data.  In V6 we are also calculating the raw torques, powers etc. This makes the file larger but will make Excel work easier
                    .WriteLine(vbCrLf & "PRIMARY_CHANNEL_RAW_DATA")
                    .WriteLine("NUMBER_OF_POINTS_COLLECTED" & " " & Main.DataPoints.ToString)
                    'Again, create the header row
                    tempstring = ""
                    For paramcount = 0 To Main.LAST - 1
                        tempsplit = Split(Main.DataUnitTags(paramcount), " ")
                        tempstring = tempstring & Main.DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(0) & ") "
                    Next
                    'Write the column headings
                    .WriteLine(tempstring)
                    'Need to set the zeroth value to support using the count and count-1 approach to torque and power calculations
                    Main.CollectedData(Main.RPM1_ROLLER, 0) = Main.CollectedData(Main.RPM1_ROLLER, 1)
                    For count = 1 To Main.DataPoints - 1
                        're-calc speed, wheel and motor RPMs based on collected data
                        Main.CollectedData(Main.SPEED, count) = Main.CollectedData(Main.RPM1_ROLLER, count) * Main.RollerRadsPerSecToMetersPerSec
                        Main.CollectedData(Main.RPM1_WHEEL, count) = Main.CollectedData(Main.RPM1_ROLLER, count) * Main.RollerRPMtoWheelRPM
                        Main.CollectedData(Main.RPM1_MOTOR, count) = Main.CollectedData(Main.RPM1_ROLLER, count) * Main.RollerRPMtoMotorRPM
                        're-calc roller torque and power useing the collected data
                        Main.CollectedData(Main.TORQUE_ROLLER, count) = (Main.CollectedData(Main.RPM1_ROLLER, count) - Main.CollectedData(Main.RPM1_ROLLER, count - 1)) / (Main.CollectedData(Main.SESSIONTIME, count) - Main.CollectedData(Main.SESSIONTIME, count - 1)) * Main.DynoMomentOfInertia 'this is the roller torque, should calc the wheel and motor at this point also
                        'NOTE - new power calculation uses (new-old) / 2
                        Main.CollectedData(Main.POWER, count) = Main.CollectedData(Main.TORQUE_ROLLER, count) * ((Main.CollectedData(Main.RPM1_ROLLER, count) + Main.CollectedData(Main.RPM1_ROLLER, count - 1)) / 2)
                        'now re-calc wheel and motor torque based on Power
                        Main.CollectedData(Main.TORQUE_WHEEL, count) = Main.CollectedData(Main.POWER, count) / Main.CollectedData(Main.RPM1_WHEEL, count)
                        Main.CollectedData(Main.TORQUE_MOTOR, count) = Main.CollectedData(Main.POWER, count) / Main.CollectedData(Main.RPM1_MOTOR, count)
                        'recalc Drag and set a max speed based on it
                        Main.CollectedData(Main.DRAG, count) = Main.CollectedData(Main.SPEED, count) ^ 3 * Main.ForceAir
                        'Update other parameters requiring calculations
                        'Main.RPM2 will be already there but the ratio and rollout need to be calculated
                        If Main.CollectedData(Main.RPM2, count) <> 0 Then
                            Main.CollectedData(Main.RPM2_RATIO, count) = Main.CollectedData(Main.RPM2, count) / Main.CollectedData(Main.RPM1_WHEEL, count)
                            Main.CollectedData(Main.RPM2_ROLLOUT, count) = Main.WheelCircumference / Main.CollectedData(Main.RPM2_RATIO, count)
                        Else
                            Main.CollectedData(Main.RPM2_RATIO, count) = 0
                            Main.CollectedData(Main.RPM2_ROLLOUT, count) = 0
                        End If
                        'Volts and Amps will already be there but watts in and efficiency need to be added
                        Main.CollectedData(Main.WATTS_IN, count) = Main.CollectedData(Main.VOLTS, count) * Main.CollectedData(Main.AMPS, count)
                        If Main.CollectedData(Main.WATTS_IN, count) <> 0 Then
                            Main.CollectedData(Main.EFFICIENCY, count) = Main.CollectedData(Main.POWER, count) / Main.CollectedData(Main.WATTS_IN, count) * 100
                        Else
                            Main.CollectedData(Main.EFFICIENCY, count) = 0
                        End If
                        'Build the results string...
                        tempstring = ""
                        For paramcount = 0 To Main.LAST - 1
                            tempsplit = Split(Main.DataUnitTags(paramcount), " ") ' How many units are there
                            tempstring = tempstring & Main.CollectedData(paramcount, count) * Main.DataUnits(paramcount, 0) & " " 'DataTags(paramcount).Replace(" ", "_") & "(" & tempsplit(unitcount) & ") "
                        Next
                        '...and write it
                        .WriteLine(tempstring)
                    Next

                End With
                'Save the file
                DataOutputFile.Close()
                btnStartLoggingRaw.Enabled = True

                '/////////////////////END COPIED CODE
                btnStartPowerRun.Enabled = True
                btnShow_Click(Me, EventArgs.Empty)
                With btnStartLoggingRaw
                    .BackColor = System.Windows.Forms.Control.DefaultBackColor
                End With
                'StopFitting = True
                WhichDataMode = LIVE
            Else
                btnHide_Click(Me, EventArgs.Empty)
                With SaveFileDialog1
                    .Reset()
                    .Filter = "Log Raw files (*.sdr)|*.sdr"
                    .ShowDialog()
                End With
                If SaveFileDialog1.FileName <> "" Then
                    LogRawDataFileName = SaveFileDialog1.FileName
                    ResetValues()
                    DataPoints = 0
                    Data(SESSIONTIME, ACTUAL) = 0
                    btnStartPowerRun.Enabled = False
                    btnShow_Click(Me, EventArgs.Empty)
                    With btnStartLoggingRaw
                        .BackColor = Color.Red
                    End With
                    WhichDataMode = LOGRAW
                Else
                    btnShow_Click(Me, EventArgs.Empty)
                End If
            End If
        Catch e1 As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("btnStartLoggingRaw Error: " & e1.ToString, MsgBoxStyle.Exclamation)
            btnShow_Click(Me, EventArgs.Empty)
            End
        End Try

    End Sub
#End Region
#Region "Text Box Checking"
    'Assigns single sub to handle text box entries ensuring only allowed chars are entered
    Private Sub SetupTextBoxCharacterHandling()
        For Each c As Control In Me.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.KeyPress, AddressOf txtControl_KeyPress
            End If
        Next
        For Each c As Control In frmDyno.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.KeyPress, AddressOf txtControl_KeyPress
            End If
        Next
        For Each c As Control In frmCOM.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.KeyPress, AddressOf txtControl_KeyPress
            End If
        Next
        For Each c As Control In frmAnalysis.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.KeyPress, AddressOf txtControl_KeyPress
            End If
        Next
        For Each c As Control In frmFit.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.KeyPress, AddressOf txtControl_KeyPress
            End If
        Next
    End Sub
    Private Sub txtControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar <> ""c Then
            If AllowedCharacters.IndexOf(e.KeyChar) = -1 Then
                e.Handled = True
            End If
        End If
        If e.KeyChar = Chr(13) Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub txtInterface_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInterface.TextChanged
        'This is an invisible box so text validation should not be needed
        lblInterface.Text = txtInterface.Text.Substring(txtInterface.Text.LastIndexOf("\") + 1)
    End Sub
    Private Sub txtZeroTimeDetect_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtZeroTimeDetect.Leave
        Dim LocalMin As Double = 0.1
        Dim LocalMax As Double = 2
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            WaitForNewSignal = TempDouble
        Else
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            btnShow_Click(Me, EventArgs.Empty)
            With CType(sender, TextBox)
                .Text = WaitForNewSignal.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtPowerRunThreshold_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPowerRunThreshold.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            PowerRunThreshold = TempDouble
            ActualPowerRunThreshold = (PowerRunThreshold / 60) * 2 * Math.PI ' convert it to rads/s
            'Trying setting the minimum number of points to be collected in here also
            'CHECK - this should also be set when Signals per RPM changes
            MinimumPowerRunPoints = frmDyno.SignalsPerRPM * 10 'This somewhat arbitrary 
        Else
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            btnShow_Click(Me, EventArgs.Empty)
            With CType(sender, TextBox)
                .Text = PowerRunThreshold.ToString
                .Focus()
            End With
        End If
    End Sub

#End Region
#Region "Read, Write, Update, and Reset Parameters"
    Private Sub LoadParametersFromFile()
        Try
            Dim Temp As String, TempSplit As String(), TempIndex As Double, start As Integer, finish As Integer, TempItem As String
            Dim cmbNew As New ComboBox
            Dim txtNew As New TextBox
            Dim scrlNew As VScrollBar
            If Directory.Exists(SettingsDirectory) Then 'Some Version of SimpleDyno has been run before
                If File.Exists(SettingsDirectory & SettingsFile) Then 'From 6.4 on Settings files is simply SimpleDynoSettings.sds
                    Dim ParameterInputFile As New System.IO.StreamReader(SettingsDirectory & SettingsFile)
                    Temp = ParameterInputFile.ReadToEnd
                    ParameterInputFile.Close()
                    TempSplit = Split(Temp, vbCrLf)
                    Select Case TempSplit(0)
                        Case Is = MainTitle, "SimpleDyno 6_4 by DamoRC" 'This is the settings file created by the current version so no issues
                            Dim SortedControls As New List(Of Control)
                            For Each c As Control In Me.Controls
                                SortedControls.Add(c)
                            Next
                            For Each c As Control In frmDyno.Controls
                                SortedControls.Add(c)
                            Next
                            For Each c As Control In frmCOM.Controls
                                SortedControls.Add(c)
                            Next
                            For Each c As Control In frmAnalysis.Controls
                                SortedControls.Add(c)
                            Next
                            For Each c As Control In frmFit.Controls
                                SortedControls.Add(c)
                            Next
                            'Need to use sorted list so that cmbData are loaded before cmbUnits
                            SortedControls.Sort(AddressOf SortControls)
                            For Each c As Control In SortedControls
                                start = InStr(Temp, c.Name.ToString)
                                If start <> 0 Then
                                    start += c.Name.Length
                                    finish = InStr(start, Temp, vbCrLf)
                                    'For the combo boxes, 6.3 used index, 6.4 and up use Item text so
                                    If TypeOf c Is ComboBox Then
                                        'Need some way of distinguishing versions here
                                        cmbNew = DirectCast(c, ComboBox)
                                        'TempIndex = CDbl(Temp.Substring(start, finish - start - 1))
                                        TempItem = Temp.Substring(start, finish - start - 1)
                                        Dim t As Integer
                                        For t = 0 To cmbNew.Items.Count - 1
                                            If cmbNew.Items(t).ToString = TempItem Then
                                                TempIndex = t
                                                cmbNew.SelectedIndex = t
                                                c.Select()
                                            End If
                                        Next

                                        'If TempIndex <= cmbNew.Items.Count - 1 Then
                                        ' cmbNew.SelectedIndex = CInt(Temp.Substring(start, finish - start - 1))
                                        ' c.Select()
                                        'End If
                                    End If
                                    If TypeOf c Is TextBox Then
                                        c.Text = Temp.Substring(start, finish - start - 1)
                                        c.Select()
                                    End If
                                    If TypeOf c Is VScrollBar Then
                                        scrlNew = DirectCast(c, VScrollBar)
                                        TempIndex = CDbl(Temp.Substring(start, finish - start - 1))
                                        scrlNew.Value = CInt(TempIndex)
                                        c.Select()
                                    End If
                                End If
                            Next
                            btnStartAcquisition.Select()
                        Case Is = "Enter Alternates Here" ' this section for interpreting older versions of the settings file if needed

                    End Select

                ElseIf File.Exists(SettingsDirectory & "\SimpleDynoSettings_6_3.sds") Then 'this is a one off for moving from 6.3 forward
                    Dim ParameterInputFile As New System.IO.StreamReader(SettingsDirectory & "\SimpleDynoSettings_6_3.sds")
                    Temp = ParameterInputFile.ReadToEnd
                    ParameterInputFile.Close()
                    TempSplit = Split(Temp, vbCrLf)
                    Dim SortedControls As New List(Of Control)
                    For Each c As Control In Me.Controls
                        SortedControls.Add(c)
                    Next
                    For Each c As Control In frmDyno.Controls
                        SortedControls.Add(c)
                    Next
                    For Each c As Control In frmCOM.Controls
                        SortedControls.Add(c)
                    Next
                    For Each c As Control In frmAnalysis.Controls
                        SortedControls.Add(c)
                    Next
                    For Each c As Control In frmFit.Controls
                        SortedControls.Add(c)
                    Next
                    'Need to use sorted list so that cmbData are loaded before cmbUnits
                    SortedControls.Sort(AddressOf SortControls)
                    For Each c As Control In SortedControls
                        start = InStr(Temp, c.Name.ToString)
                        If start <> 0 Then
                            start += c.Name.Length
                            finish = InStr(start, Temp, vbCrLf)
                            'For the combo boxes, 6.3 used index, 6.4 and up use Item text so
                            If TypeOf c Is ComboBox Then
                                cmbNew = DirectCast(c, ComboBox)
                                'TempItem = Temp.Substring(start, finish - start - 1)
                                '        Dim t As Integer
                                '        For t = 0 To cmbNew.Items.Count - 1
                                '            If cmbNew.Items(t).ToString = TempItem Then
                                '                TempIndex = t
                                '                cmbNew.SelectedIndex = t
                                '                c.Select()
                                '            End If
                                '        Next
                                TempIndex = CDbl(Temp.Substring(start, finish - start - 1))
                                If TempIndex <= cmbNew.Items.Count - 1 Then
                                    cmbNew.SelectedIndex = CInt(Temp.Substring(start, finish - start - 1))
                                    c.Select()
                                End If
                            End If
                            If TypeOf c Is TextBox Then
                                c.Text = Temp.Substring(start, finish - start - 1)
                                c.Select()
                            End If
                            If TypeOf c Is VScrollBar Then
                                scrlNew = DirectCast(c, VScrollBar)
                                TempIndex = CDbl(Temp.Substring(start, finish - start - 1))
                                scrlNew.Value = CInt(TempIndex)
                                c.Select()
                            End If
                        End If
                    Next
                    btnStartAcquisition.Select()
                Else
                    SaveParametersToFile()
                    LoadParametersFromFile()
                    CreateDefaultView()
                End If
            Else
                Directory.CreateDirectory(SettingsDirectory)
                SaveParametersToFile()
                LoadParametersFromFile()
                CreateDefaultView()
            End If
        Catch e As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("LoadParametersFromFile Error: " & e.ToString, MsgBoxStyle.Exclamation)
            End
        End Try
    End Sub
    Private Function SortControls(ByVal a As Control, ByVal b As Control) As Integer
        'This function supports sorting to the controls in LoadParameters
        Return a.Name.CompareTo(b.Name)
    End Function
    Private Sub CreateDefaultView()
        'Default view has 4 SD form controls, 2 labels, a 270 degree gauge and a plot
        Dim p As Integer = 1, g As Integer = 2, l1 As Integer = 3, l2 As Integer = 4
        Dim DefaultControls(4) As Rectangle
        'Going to assume the main form will eventually be 1/phi^3 tall, where phi is golden ratio height
        'For this to work, the width must be 1.170820393 the screen res height
        Dim ScreenWidth As Double = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
        Dim PhiWidth As Double = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Me.Height
        'Plot will be sqaure and the largest one
        DefaultControls(p).Height = CInt(PhiWidth) '/ Phi)
        DefaultControls(p).Width = CInt(PhiWidth) '/ Phi)
        'Gauge is next
        DefaultControls(g).Height = CInt(PhiWidth / Phi) '^ 2)
        DefaultControls(g).Width = CInt(PhiWidth / Phi) ' ^ 2)
        'Labels are last
        DefaultControls(l1).Height = CInt(PhiWidth / Phi ^ 2) ' ^ 3)
        DefaultControls(l1).Width = CInt(PhiWidth / Phi / 2) '^ 2 / 2)
        DefaultControls(l2).Height = CInt(PhiWidth / Phi ^ 2) '^ 3)
        DefaultControls(l2).Width = CInt(PhiWidth / Phi / 2) '^ 2 / 2)
        'Now set the positions
        'First, the gauge
        DefaultControls(g).X = CInt((ScreenWidth - PhiWidth - PhiWidth / Phi) / 2)
        DefaultControls(g).Y = 0
        'Then the labels
        DefaultControls(l1).X = DefaultControls(g).Left
        DefaultControls(l1).Y = DefaultControls(g).Bottom
        DefaultControls(l2).X = DefaultControls(l1).Right
        DefaultControls(l2).Y = DefaultControls(g).Bottom
        'now the plot
        DefaultControls(p).X = DefaultControls(g).Right
        DefaultControls(p).Y = DefaultControls(g).Top
        'set up the string and write it
        Dim TempString As String = InterfaceVersion & vbCrLf
        Dim Splitter As String = "_"
        'Serialized SD Forms are left, top, height, width
        TempString += "Gauge" & vbCrLf
        TempString += DefaultControls(g).X & Splitter & DefaultControls(g).Y & Splitter & DefaultControls(g).Height & Splitter & DefaultControls(g).Width & Splitter
        TempString += "270 deg_1_30_White_Black_0_0_0_0_0_Arial_Green_1_1_1_0_5000_False_" & vbCrLf
        TempString += "Label" & vbCrLf
        TempString += DefaultControls(l1).X & Splitter & DefaultControls(l1).Y & Splitter & DefaultControls(l1).Height & Splitter & DefaultControls(l1).Width & Splitter
        TempString += "Vertical_1_300_White_Black_0_0_0_0_0_Arial_Green_1_1_1_0_1000_False_" & vbCrLf
        TempString += "Label" & vbCrLf
        TempString += DefaultControls(l2).X & Splitter & DefaultControls(l2).Y & Splitter & DefaultControls(l2).Height & Splitter & DefaultControls(l2).Width & Splitter
        TempString += "Vertical_1_300_White_Black_0_0_0_0_0_Arial_Green_1_2_1_0_1000_False_" & vbCrLf
        TempString += "MultiYTimeGraph" & vbCrLf
        TempString += DefaultControls(p).X & Splitter & DefaultControls(p).Y & Splitter & DefaultControls(p).Height & Splitter & DefaultControls(p).Width & Splitter
        TempString += "Lines_4_30_White_Black_0_1_0_0_10_Arial_Green_1_1_1_0_5000_True_Green_0_0_0_0_5000_False_Green_0_0_0_0_5000_False_Green_0_0_0_0_5000_False_"
        Dim DefaultViewOutputFile As New System.IO.StreamWriter(SettingsDirectory & DefaultViewFile)
        'InterfaceName = SettingsDirectory & DefaultViewFile
        txtInterface.Text = SettingsDirectory & DefaultViewFile
        DefaultViewOutputFile.WriteLine(TempString)
        DefaultViewOutputFile.Close()
    End Sub
    Private Sub SaveParametersToFile()
        Try
            Dim cmbNew As ComboBox
            Dim scrlNew As VScrollBar
            Dim ParameterOutputFile As New System.IO.StreamWriter(SettingsDirectory & SettingsFile)
            ParameterOutputFile.WriteLine(MainTitle)

            Dim SortedControls As New List(Of Control)
            For Each c As Control In Me.Controls
                SortedControls.Add(c)
            Next
            For Each c As Control In frmDyno.Controls
                SortedControls.Add(c)
            Next
            For Each c As Control In frmCOM.Controls
                SortedControls.Add(c)
            Next
            For Each c As Control In frmAnalysis.Controls
                SortedControls.Add(c)
            Next
            For Each c As Control In frmFit.Controls
                SortedControls.Add(c)
            Next

            For Each c As Control In SortedControls
                If TypeOf c Is TextBox Then
                    ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & c.Text)
                End If
                If TypeOf c Is ComboBox Then
                    cmbNew = DirectCast(c, ComboBox)
                    If cmbNew.SelectedIndex <> -1 Then
                        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & cmbNew.SelectedItem.ToString)
                    End If
                End If
                If TypeOf c Is VScrollBar Then
                    scrlNew = DirectCast(c, VScrollBar)
                    ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & scrlNew.Value)
                End If
            Next

            'For Each c As Control In Me.Controls
            '    If TypeOf c Is TextBox Then
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & c.Text)
            '    End If
            '    If TypeOf c Is ComboBox Then
            '        cmbNew = DirectCast(c, ComboBox)
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & cmbNew.SelectedItem.ToString)
            '    End If
            'Next

            'For Each c As Control In frmDyno.Controls
            '    If TypeOf c Is TextBox Then
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & c.Text)
            '    End If
            '    If TypeOf c Is ComboBox Then
            '        cmbNew = DirectCast(c, ComboBox)
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & cmbNew.SelectedItem.ToString)
            '    End If
            'Next

            'For Each c As Control In frmCOM.Controls
            '    If TypeOf c Is TextBox Then
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & c.Text)
            '    End If
            '    If TypeOf c Is ComboBox Then
            '        cmbNew = DirectCast(c, ComboBox)
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & cmbNew.SelectedItem.ToString)
            '    End If
            'Next

            'For Each c As Control In frmAnalysis.Controls
            '    If TypeOf c Is TextBox Then
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & c.Text)
            '    End If
            '    If TypeOf c Is ComboBox Then
            '        cmbNew = DirectCast(c, ComboBox)
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & cmbNew.SelectedItem.ToString)
            '    End If
            'Next

            'For Each c As Control In frmFit.Controls
            '    If TypeOf c Is TextBox Then
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & c.Text)
            '    End If
            '    If TypeOf c Is ComboBox Then
            '        cmbNew = DirectCast(c, ComboBox)
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & cmbNew.SelectedItem.ToString)
            '    End If
            '    If TypeOf c Is VScrollBar Then
            '        scrlNew = DirectCast(c, VScrollBar)
            '        ParameterOutputFile.WriteLine("[" & c.Name.ToString & "]" & scrlNew.Value)
            '    End If
            'Next

            ParameterOutputFile.Close()

        Catch e As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("From SaveParameters Error: " & e.ToString, MsgBoxStyle.Exclamation)
            End
        End Try
    End Sub
    Private Sub PrepareArrays()

        'Set all parameters to be available in interface components
        For count As Integer = 1 To LAST - 1
            DataAreUsed(count) = True
        Next

        'The following are not available live but only in Analysis 
        DataAreUsed(POWER_COASTDOWN) = False
        DataAreUsed(TORQUE_COASTDOWN) = False
        'The following are available live, but never on startup
        DataAreUsed(CORRECTED_EFFICIENCY) = False
        DataAreUsed(CORRECTED_POWER_MOTOR) = False
        DataAreUsed(CORRECTED_POWER_ROLLER) = False
        DataAreUsed(CORRECTED_POWER_WHEEL) = False
        DataAreUsed(CORRECTED_TORQUE_MOTOR) = False
        DataAreUsed(CORRECTED_TORQUE_ROLLER) = False
        DataAreUsed(CORRECTED_TORQUE_WHEEL) = False

        DataTags(RPM1_ROLLER) = "RPM1 Roller"
        DataUnitTags(RPM1_ROLLER) = "rad/s RPM"
        DataUnits(RPM1_ROLLER, 0) = 1
        DataUnits(RPM1_ROLLER, 1) = 60 / (2 * Math.PI)

        DataTags(RPM1_WHEEL) = "RPM1 Wheel"
        DataUnitTags(RPM1_WHEEL) = "rad/s RPM"
        DataUnits(RPM1_WHEEL, 0) = 1
        DataUnits(RPM1_WHEEL, 1) = 60 / (2 * Math.PI)

        DataTags(RPM1_MOTOR) = "RPM1 Motor"
        DataUnitTags(RPM1_MOTOR) = "rad/s RPM"
        DataUnits(RPM1_MOTOR, 0) = 1
        DataUnits(RPM1_MOTOR, 1) = 60 / (2 * Math.PI)

        DataTags(RPM2) = "RPM2"
        DataUnitTags(RPM2) = "rad/s RPM"
        DataUnits(RPM2, 0) = 1
        DataUnits(RPM2, 1) = 60 / (2 * Math.PI)

        DataTags(RPM2_RATIO) = "Ratio"
        DataUnitTags(RPM2_RATIO) = "M/W"
        DataUnits(RPM2_RATIO, 0) = 1
        Data(RPM2_RATIO, MINIMUM) = 100000

        DataTags(RPM2_ROLLOUT) = "Rollout"
        DataUnitTags(RPM2_ROLLOUT) = "mm cm in"
        DataUnits(RPM2_ROLLOUT, 0) = 1
        DataUnits(RPM2_ROLLOUT, 1) = 0.1
        DataUnits(RPM2_ROLLOUT, 2) = 0.0393701
        Data(RPM2_ROLLOUT, MINIMUM) = 100000

        DataTags(SPEED) = "Speed"
        DataUnitTags(SPEED) = "m/s MPH KPH"
        DataUnits(SPEED, 0) = 1
        DataUnits(SPEED, 1) = 2.23694
        DataUnits(SPEED, 2) = 3.6

        DataTags(TORQUE_ROLLER) = "Roller Torque"
        DataUnitTags(TORQUE_ROLLER) = "N.m g.cm oz.in lb.ft"
        DataUnits(TORQUE_ROLLER, 0) = 1
        DataUnits(TORQUE_ROLLER, 1) = 10197.16
        DataUnits(TORQUE_ROLLER, 2) = 141.612
        DataUnits(TORQUE_ROLLER, 3) = 0.737562

        DataTags(TORQUE_WHEEL) = "Wheel Torque"
        DataUnitTags(TORQUE_WHEEL) = "N.m g.cm oz.in lb.ft"
        DataUnits(TORQUE_WHEEL, 0) = 1
        DataUnits(TORQUE_WHEEL, 1) = 10197.16
        DataUnits(TORQUE_WHEEL, 2) = 141.612
        DataUnits(TORQUE_WHEEL, 3) = 0.737562

        DataTags(TORQUE_MOTOR) = "Motor Torque"
        DataUnitTags(TORQUE_MOTOR) = "N.m g.cm oz.in lb.ft"
        DataUnits(TORQUE_MOTOR, 0) = 1
        DataUnits(TORQUE_MOTOR, 1) = 10197.16
        DataUnits(TORQUE_MOTOR, 2) = 141.612
        DataUnits(TORQUE_MOTOR, 3) = 0.737562

        DataTags(POWER) = "Power"
        DataUnitTags(POWER) = "W kW HP"
        DataUnits(POWER, 0) = 1
        DataUnits(POWER, 1) = 0.001
        DataUnits(POWER, 2) = 0.00134

        DataTags(DRAG) = "Drag"
        DataUnitTags(DRAG) = "W kW HP"
        DataUnits(DRAG, 0) = 1
        DataUnits(DRAG, 1) = 0.001
        DataUnits(DRAG, 2) = 0.00134

        DataTags(VOLTS) = "Voltage"
        DataUnitTags(VOLTS) = "V kV"
        DataUnits(VOLTS, 0) = 1
        DataUnits(VOLTS, 1) = 0.001
        Data(VOLTS, MINIMUM) = 100000

        DataTags(AMPS) = "Current"
        DataUnitTags(AMPS) = "A mA"
        DataUnits(AMPS, 0) = 1
        DataUnits(AMPS, 1) = 1000
        Data(AMPS, MINIMUM) = 100000

        DataTags(WATTS_IN) = "Watts In"
        DataUnitTags(WATTS_IN) = "W kW"
        DataUnits(WATTS_IN, 0) = 1
        DataUnits(WATTS_IN, 1) = 0.001
        Data(WATTS_IN, MINIMUM) = 100000

        DataTags(EFFICIENCY) = "Efficiency"
        DataUnitTags(EFFICIENCY) = "%"
        DataUnits(EFFICIENCY, 0) = 1
        Data(EFFICIENCY, MINIMUM) = 10000

        DataTags(CORRECTED_EFFICIENCY) = "Corr. Efficiency"
        DataUnitTags(CORRECTED_EFFICIENCY) = "%"
        DataUnits(CORRECTED_EFFICIENCY, 0) = 1
        Data(CORRECTED_EFFICIENCY, MINIMUM) = 10000


        DataTags(TEMPERATURE1) = "Temperature1"
        DataUnitTags(TEMPERATURE1) = "°C"
        DataUnits(TEMPERATURE1, 0) = 1
        Data(TEMPERATURE1, MINIMUM) = 10000

        DataTags(TEMPERATURE2) = "Temperature2"
        DataUnitTags(TEMPERATURE2) = "°C"
        DataUnits(TEMPERATURE2, 0) = 1
        Data(TEMPERATURE1, MINIMUM) = 10000

        DataTags(PIN04VALUE) = "Pin 4 Value"
        DataUnitTags(PIN04VALUE) = "Units"
        DataUnits(PIN04VALUE, 0) = 1
        Data(PIN04VALUE, MINIMUM) = 10000

        DataTags(PIN05VALUE) = "Pin 5 Value"
        DataUnitTags(PIN05VALUE) = "Units"
        DataUnits(PIN05VALUE, 0) = 1
        Data(PIN05VALUE, MINIMUM) = 10000

        DataTags(SESSIONTIME) = "Time"
        DataUnitTags(SESSIONTIME) = "Sec Min Hr"
        DataUnits(SESSIONTIME, 0) = 1
        DataUnits(SESSIONTIME, 1) = 1 / 60
        DataUnits(SESSIONTIME, 2) = 1 / 3600

        DataTags(CHAN1_FREQUENCY) = "Ch1 Frequency"
        DataUnitTags(CHAN1_FREQUENCY) = "Hz"
        DataUnits(CHAN1_FREQUENCY, 0) = 1

        DataTags(CHAN1_PULSEWIDTH) = "Ch1 Pulse Width"
        DataUnitTags(CHAN1_PULSEWIDTH) = "ms"
        DataUnits(CHAN1_PULSEWIDTH, 0) = 1

        DataTags(CHAN1_DUTYCYCLE) = "Ch1 Duty Cycle"
        DataUnitTags(CHAN1_DUTYCYCLE) = "%"
        DataUnits(CHAN1_DUTYCYCLE, 0) = 1

        DataTags(CHAN2_FREQUENCY) = "Ch2 Frequency"
        DataUnitTags(CHAN2_FREQUENCY) = "Hz"
        DataUnits(CHAN2_FREQUENCY, 0) = 1

        DataTags(CHAN2_PULSEWIDTH) = "Ch2 Pulse Width"
        DataUnitTags(CHAN2_PULSEWIDTH) = "ms"
        DataUnits(CHAN2_PULSEWIDTH, 0) = 1

        DataTags(CHAN2_DUTYCYCLE) = "Ch2 Duty Cycle"
        DataUnitTags(CHAN2_DUTYCYCLE) = "%"
        DataUnits(CHAN2_DUTYCYCLE, 0) = 1

        'NEW RUNDOWN SETS
        DataTags(TORQUE_COASTDOWN) = "Coast Down Torque"
        DataUnitTags(TORQUE_COASTDOWN) = "N.m g.cm oz.in lb.ft"
        DataUnits(TORQUE_COASTDOWN, 0) = 1
        DataUnits(TORQUE_COASTDOWN, 1) = 10197.16
        DataUnits(TORQUE_COASTDOWN, 2) = 141.612
        DataUnits(TORQUE_COASTDOWN, 3) = 0.737562

        DataTags(POWER_COASTDOWN) = "Coast Down Power"
        DataUnitTags(POWER_COASTDOWN) = "W kW HP"
        DataUnits(POWER_COASTDOWN, 0) = 1
        DataUnits(POWER_COASTDOWN, 1) = 0.001
        DataUnits(POWER_COASTDOWN, 2) = 0.00134

        'NEW CORRECTED TORQUE AND POWER SETS
        DataTags(CORRECTED_TORQUE_ROLLER) = "Corr. Roller Torque"
        DataUnitTags(CORRECTED_TORQUE_ROLLER) = "N.m g.cm oz.in lb.ft"
        DataUnits(CORRECTED_TORQUE_ROLLER, 0) = 1
        DataUnits(CORRECTED_TORQUE_ROLLER, 1) = 10197.16
        DataUnits(CORRECTED_TORQUE_ROLLER, 2) = 141.612
        DataUnits(CORRECTED_TORQUE_ROLLER, 3) = 0.737562

        DataTags(CORRECTED_TORQUE_WHEEL) = "Corr. Wheel Torque"
        DataUnitTags(CORRECTED_TORQUE_WHEEL) = "N.m g.cm oz.in lb.ft"
        DataUnits(CORRECTED_TORQUE_WHEEL, 0) = 1
        DataUnits(CORRECTED_TORQUE_WHEEL, 1) = 10197.16
        DataUnits(CORRECTED_TORQUE_WHEEL, 2) = 141.612
        DataUnits(CORRECTED_TORQUE_WHEEL, 3) = 0.737562

        DataTags(CORRECTED_TORQUE_MOTOR) = "Corr. Motor Torque"
        DataUnitTags(CORRECTED_TORQUE_MOTOR) = "N.m g.cm oz.in lb.ft"
        DataUnits(CORRECTED_TORQUE_MOTOR, 0) = 1
        DataUnits(CORRECTED_TORQUE_MOTOR, 1) = 10197.16
        DataUnits(CORRECTED_TORQUE_MOTOR, 2) = 141.612
        DataUnits(CORRECTED_TORQUE_MOTOR, 3) = 0.737562

        DataTags(CORRECTED_POWER_ROLLER) = "Corr. Roller Power"
        DataUnitTags(CORRECTED_POWER_ROLLER) = "W kW HP"
        DataUnits(CORRECTED_POWER_ROLLER, 0) = 1
        DataUnits(CORRECTED_POWER_ROLLER, 1) = 0.001
        DataUnits(CORRECTED_POWER_ROLLER, 2) = 0.00134

        DataTags(CORRECTED_POWER_WHEEL) = "Corr. Wheel Power"
        DataUnitTags(CORRECTED_POWER_WHEEL) = "W kW HP"
        DataUnits(CORRECTED_POWER_WHEEL, 0) = 1
        DataUnits(CORRECTED_POWER_WHEEL, 1) = 0.001
        DataUnits(CORRECTED_POWER_WHEEL, 2) = 0.00134

        DataTags(CORRECTED_POWER_MOTOR) = "Corr. Motor Power"
        DataUnitTags(CORRECTED_POWER_MOTOR) = "W kW HP"
        DataUnits(CORRECTED_POWER_MOTOR, 0) = 1
        DataUnits(CORRECTED_POWER_MOTOR, 1) = 0.001
        DataUnits(CORRECTED_POWER_MOTOR, 2) = 0.00134
#If QueryPerformance Then
        DataTags(PERFORMANCE) = "Performance"
        DataUnitTags(PERFORMANCE) = "%"
        DataUnits(PERFORMANCE, 0) = 1
#End If

    End Sub
    Friend Sub COMPortCalibration()
        Dim TempIntercept1 As Double, TempIntercept2 As Double

        VoltageSlope = (Voltage2 - Voltage1) / ((A0Voltage2 - A0Voltage1) * BitsToVoltage)
        TempIntercept1 = Voltage1 - VoltageSlope * A0Voltage1 * BitsToVoltage
        TempIntercept2 = Voltage2 - VoltageSlope * A0Voltage2 * BitsToVoltage
        VoltageIntercept = (TempIntercept1 + TempIntercept2) / 2

        CurrentSlope = (Current2 - Current1) / ((A1Voltage2 - A1Voltage1) * BitsToVoltage)
        TempIntercept1 = Current1 - CurrentSlope * A1Voltage1 * BitsToVoltage
        TempIntercept2 = Current2 - CurrentSlope * A1Voltage2 * BitsToVoltage
        CurrentIntercept = (TempIntercept1 + TempIntercept2) / 2

        Temperature1Slope = (Temp1Temperature2 - Temp1Temperature1) / ((A2Voltage2 - A2Voltage1) * BitsToVoltage)
        TempIntercept1 = Temp1Temperature1 - Temperature1Slope * A2Voltage1 * BitsToVoltage
        TempIntercept2 = Temp1Temperature2 - Temperature1Slope * A2Voltage2 * BitsToVoltage
        Temperature1Intercept = (TempIntercept1 + TempIntercept2) / 2

        Temperature2Slope = (Temp2Temperature2 - Temp2Temperature1) / ((A3Voltage2 - A3Voltage1) * BitsToVoltage)
        TempIntercept1 = Temp2Temperature1 - Temperature2Slope * A3Voltage1 * BitsToVoltage
        TempIntercept2 = Temp2Temperature2 - Temperature2Slope * A3Voltage2 * BitsToVoltage
        Temperature2Intercept = (TempIntercept1 + TempIntercept2) / 2
        '
        A4ValueSlope = (A4Value2 - A4Value1) / ((A4Voltage2 - A4Voltage1) * BitsToVoltage)
        TempIntercept1 = A4Value1 - A4ValueSlope * A4Voltage1 * BitsToVoltage
        TempIntercept2 = A4Value2 - A4ValueSlope * A4Voltage2 * BitsToVoltage
        A4ValueIntercept = (TempIntercept1 + TempIntercept2) / 2

        A5ValueSlope = (A5Value2 - A5Value1) / ((A5Voltage2 - A5Voltage1) * BitsToVoltage)
        TempIntercept1 = A5Value1 - A5ValueSlope * A5Voltage1 * BitsToVoltage
        TempIntercept2 = A5Value2 - A5ValueSlope * A5Voltage2 * BitsToVoltage
        A5ValueIntercept = (TempIntercept1 + TempIntercept2) / 2
    End Sub

    Private Sub PrepareGraphicsParameters()
        Try

            With pnlSignalWindow
                PicSignalHeight = .Height
                PicSignalWidth = .Width
            End With

            SignalBitmap = New Bitmap(PicSignalWidth, PicSignalHeight)
            SignalWindowBMP = Graphics.FromImage(SignalBitmap)

            SignalXConversion = PicSignalWidth / BUFFER_SIZE * NUMBER_OF_CHANNELS
            SignalYConversion = PicSignalHeight / 2 ^ BITS_PER_SAMPLE

            HighSignalThreshold2 = CDbl(txtThreshold2.Text)
            HighSignalThreshold = CDbl(txtThreshold1.Text)

            If HighSignalThreshold > 128 Then WhichSignal = HIGHSIGNAL Else WhichSignal = LOWSIGNAL
            SignalThresholdYConverted = CInt(PicSignalHeight - HighSignalThreshold * SignalYConversion)
            If HighSignalThreshold2 > 128 Then WhichSignal2 = HIGHSIGNAL Else WhichSignal2 = LOWSIGNAL
            SignalThreshold2YConverted = CInt(PicSignalHeight - HighSignalThreshold2 * SignalYConversion)

        Catch e As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("PrepareGraphicsParameters Error: " & e.ToString, MsgBoxStyle.Exclamation)
            End

        End Try
    End Sub
    Private Sub ResetValues()
        Try
            Dim ParameterCount As Integer

            For ParameterCount = 0 To LAST - 1
                Data(ParameterCount, MINIMUM) = 999999
                Data(ParameterCount, ACTUAL) = 0
                Data(ParameterCount, MAXIMUM) = 0
            Next

            ResetAllYTimeGraphs()
            Data(SESSIONTIME, ACTUAL) = 0

        Catch e As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("ResetValues Error: " & e.ToString, MsgBoxStyle.Exclamation)
            End
        End Try
    End Sub

#End Region
#Region "Initialize, Process and Shutdown Wave Input"
    Private Sub InitializeWaveInput()

        With waveFormat
            .wFormatTag = WAVE_FORMAT_PCM       'uncompressed Pulse Code Modulation
            .nchannels = CShort(NUMBER_OF_CHANNELS)    '1 for mono 2 for stereo
            .nSamplesPerSec = SAMPLE_RATE       '44100 is CD quality
            .wBitsPerSample = BITS_PER_SAMPLE               'BITS_PER_SAMPLE 'CD quality is 16
            .nBlockAlign = CShort(.nchannels * (.wBitsPerSample / 8))
            .nAvgBytesPerSec = .nSamplesPerSec * .nBlockAlign
            .cbSize = 0
        End With

        i = waveInOpen(WaveInHandle, IntPtr.op_Explicit(WAVE_MAPPER), waveFormat, myCallBackFunction, IntPtr.Zero, CALLBACK_FUNCTION)
        If i <> 0 Then
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("InitializeWaveInput / WaveInOpen Error", MsgBoxStyle.Exclamation)
            End
        End If

        bufferpin0 = GCHandle.Alloc(RawWaveData0, GCHandleType.Pinned)
        bufferpin1 = GCHandle.Alloc(RawWaveData1, GCHandleType.Pinned)
        bufferpin2 = GCHandle.Alloc(RawWaveData2, GCHandleType.Pinned)
        bufferpin3 = GCHandle.Alloc(RawWaveData3, GCHandleType.Pinned)
        bufferpin4 = GCHandle.Alloc(RawWaveData4, GCHandleType.Pinned)
        bufferpin5 = GCHandle.Alloc(RawWaveData5, GCHandleType.Pinned)
        bufferpin6 = GCHandle.Alloc(RawWaveData6, GCHandleType.Pinned)
        bufferpin7 = GCHandle.Alloc(RawWaveData7, GCHandleType.Pinned)
        bufferpin8 = GCHandle.Alloc(RawWaveData8, GCHandleType.Pinned)
        bufferpin9 = GCHandle.Alloc(RawWaveData9, GCHandleType.Pinned)
        bufferpin10 = GCHandle.Alloc(RawWaveData10, GCHandleType.Pinned)
        bufferpin11 = GCHandle.Alloc(RawWaveData11, GCHandleType.Pinned)
        bufferpin12 = GCHandle.Alloc(RawWaveData12, GCHandleType.Pinned)
        bufferpin13 = GCHandle.Alloc(RawWaveData13, GCHandleType.Pinned)
        bufferpin14 = GCHandle.Alloc(RawWaveData14, GCHandleType.Pinned)
        bufferpin15 = GCHandle.Alloc(RawWaveData15, GCHandleType.Pinned)
        bufferpin16 = GCHandle.Alloc(RawWaveData16, GCHandleType.Pinned)
        bufferpin17 = GCHandle.Alloc(RawWaveData17, GCHandleType.Pinned)
        bufferpin18 = GCHandle.Alloc(RawWaveData18, GCHandleType.Pinned)
        bufferpin19 = GCHandle.Alloc(RawWaveData19, GCHandleType.Pinned)




        For j = 0 To NUMBER_OF_BUFFERS - 1
            With WaveBufferHeaders(j)
                .dwBufferLength = BUFFER_SIZE
                .dwFlags = 0
                .dwBytesRecorded = 0
                .dwLoops = 0
                .dwUser = IntPtr.op_Explicit(0)
            End With

        Next

        WaveBufferHeaders(0).lpData = bufferpin0.AddrOfPinnedObject()
        WaveBufferHeaders(1).lpData = bufferpin1.AddrOfPinnedObject()
        WaveBufferHeaders(2).lpData = bufferpin2.AddrOfPinnedObject()
        WaveBufferHeaders(3).lpData = bufferpin3.AddrOfPinnedObject()
        WaveBufferHeaders(4).lpData = bufferpin4.AddrOfPinnedObject()
        WaveBufferHeaders(5).lpData = bufferpin5.AddrOfPinnedObject()
        WaveBufferHeaders(6).lpData = bufferpin6.AddrOfPinnedObject()
        WaveBufferHeaders(7).lpData = bufferpin7.AddrOfPinnedObject()
        WaveBufferHeaders(8).lpData = bufferpin8.AddrOfPinnedObject()
        WaveBufferHeaders(9).lpData = bufferpin9.AddrOfPinnedObject()
        WaveBufferHeaders(10).lpData = bufferpin10.AddrOfPinnedObject()
        WaveBufferHeaders(11).lpData = bufferpin11.AddrOfPinnedObject()
        WaveBufferHeaders(12).lpData = bufferpin12.AddrOfPinnedObject()
        WaveBufferHeaders(13).lpData = bufferpin13.AddrOfPinnedObject()
        WaveBufferHeaders(14).lpData = bufferpin14.AddrOfPinnedObject()
        WaveBufferHeaders(15).lpData = bufferpin15.AddrOfPinnedObject()
        WaveBufferHeaders(16).lpData = bufferpin16.AddrOfPinnedObject()
        WaveBufferHeaders(17).lpData = bufferpin17.AddrOfPinnedObject()
        WaveBufferHeaders(18).lpData = bufferpin18.AddrOfPinnedObject()
        WaveBufferHeaders(19).lpData = bufferpin19.AddrOfPinnedObject()

        For j = 0 To NUMBER_OF_BUFFERS - 1
            i = waveInPrepareHeader(WaveInHandle, WaveBufferHeaders(j), Marshal.SizeOf(WaveBufferHeaders(j)))
            If i <> 0 Then
                btnHide_Click(Me, EventArgs.Empty)
                MsgBox("InitializeWaveInput / waveInPrepareHeader Error", MsgBoxStyle.Exclamation)
                End
            End If
        Next

        For j = 0 To NUMBER_OF_BUFFERS - 1
            i = waveInAddBuffer(WaveInHandle, WaveBufferHeaders(j), Marshal.SizeOf(WaveBufferHeaders(j)))
            If i <> 0 Then
                btnHide_Click(Me, EventArgs.Empty)
                MsgBox("InitializeWaveInput / waveInAddBuffer Error", MsgBoxStyle.Exclamation)
                End
            End If
        Next

        i = waveInStart(WaveInHandle)
        If i <> 0 Then
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("InitializeWaveInput / waveInStart Error", MsgBoxStyle.Exclamation)
            End
        Else
            WavesStarted = True
        End If

    End Sub
    Private Function MyWaveCallBackProcedure(ByVal hwi As IntPtr, ByVal uMsg As Int32, ByVal dwInstance As Int32, ByVal dwParam1 As Int32, ByVal dwParam2 As Int32) As Int32
        If StopAddingBuffers = False Then
            If uMsg = WIM_DATA And WaveBufferHeaders(BufferCount).dwBytesRecorded = BUFFER_SIZE Then
                myWaveHandler_ProcessWave()
            End If
        End If
    End Function
    Private Sub myWaveHandler_ProcessWave()
        Try
#If QueryPerformance Then
            QueryPerformanceCounter(StartWatch)
#End If
            If StopAddingBuffers = False Then 'StopAddingBuffers is true when ShutDownWaves has been called

                Marshal.Copy(WaveBufferHeaders(BufferCount).lpData, RawWaveData, 0, WaveBufferHeaders(BufferCount).dwBytesRecorded) 'Copy the current buffer (based on buffercount) to the working buffer, RawWaveData
                WaveBufferHeaders(BufferCount).dwBytesRecorded = 0     'Reset bytes recorded...
                i = waveInAddBuffer(WaveInHandle, WaveBufferHeaders(BufferCount), Marshal.SizeOf(WaveBufferHeaders(BufferCount)))  '...and add the buffer back to the queue
                If i <> 0 Then 'Check that there were no problems adding back the buffer.'This could be skipped in a release version using a compiler constant
                    btnHide_Click(Me, EventArgs.Empty)
                    MsgBox("myWaveHandler_ProcessWave / waveInAddBuffer Error" & i, MsgBoxStyle.Exclamation)
                    End
                End If

                SignalWindowBMP.Clear(Color.Black) 'Clear the signal window and draw the RPM1 threshold line
                SignalWindowBMP.DrawLine(Channel1ThresholdPen, 0, SignalThresholdYConverted, PicSignalWidth, SignalThresholdYConverted)

                For j = 0 To (BUFFER_SIZE - 1) Step NUMBER_OF_CHANNELS 'Main loop through the raw data

                    NextYPosition = CInt(PicSignalHeight - RawWaveData(j) * SignalYConversion) 'Calculate the next drawing position for the signal
                    SignalWindowBMP.DrawLine(Channel1SignalPen, CInt(CurrentSignalXPosition - SignalXConversion), LastYPosition, CInt(CurrentSignalXPosition), NextYPosition)  'Draw the line
                    LastYPosition = NextYPosition 'Remember the new drawing position for the next drawing operation

                    Data(SESSIONTIME, ACTUAL) += ElapsedTimeUnit 'Increase the session time value - used by the Graphing form for time info

                    If (WhichSignal = HIGHSIGNAL AndAlso RawWaveData(j) > HighSignalThreshold) Or (WhichSignal = LOWSIGNAL AndAlso RawWaveData(j) < HighSignalThreshold) Then 'Check is we have found a signal depending on where the threshold line is set

                        k = k + NUMBER_OF_CHANNELS 'Counting bytes for pulsewidth

                        If FoundHighSignal = False Then 'if FoundHighSigal is false, then this is the start of a new pulse rather than already being in a pulse
                            FoundHighSignal = True  'flag that we are inside a pulse - this gets set to false when we drop below the threshold
                            Select Case UseAdvancedProcessing 'Calculate elapsed time simply by the byte count since last or by interpolation through the threshold line
                                Case Is = False
                                    ElapsedTime = (j - LastHighBufferPosition) * BytesToSeconds
                                Case Is = True
                                    NewElapsedTimeCorrection = Math.Abs((RawWaveData(j) - HighSignalThreshold) / (RawWaveData(j) - LastSignal))
                                    ElapsedTime = ((j - LastHighBufferPosition) + OldElapsedTimeCorrection - NewElapsedTimeCorrection) * BytesToSeconds
                                    OldElapsedTimeCorrection = NewElapsedTimeCorrection
                            End Select
                            LastHighBufferPosition = j    'set the new buffer position from which to count the next set of bytes

                            Data(CHAN1_FREQUENCY, ACTUAL) = 1 / ElapsedTime 'calculate frequency for scope work - this should be compiler constant controlled
                            If Data(CHAN1_FREQUENCY, ACTUAL) > Data(CHAN1_FREQUENCY, MAXIMUM) Then Data(CHAN1_FREQUENCY, MAXIMUM) = Data(CHAN1_FREQUENCY, ACTUAL)
                            If Data(CHAN1_FREQUENCY, ACTUAL) < Data(CHAN1_FREQUENCY, MINIMUM) Then Data(CHAN1_FREQUENCY, MINIMUM) = Data(CHAN1_FREQUENCY, ACTUAL)

                            Data(RPM1_ROLLER, ACTUAL) = ElapsedTimeToRadPerSec / ElapsedTime 'calculate roller angular velocity in Rad/s
                            Data(RPM1_WHEEL, ACTUAL) = Data(RPM1_ROLLER, ACTUAL) * RollerRPMtoWheelRPM 'calculate the wheel and motor angular velocities based on roller and wheel diameters and gear ratio
                            Data(RPM1_MOTOR, ACTUAL) = Data(RPM1_WHEEL, ACTUAL) * GearRatio
                            Data(SPEED, ACTUAL) = Data(RPM1_ROLLER, ACTUAL) * RollerRadsPerSecToMetersPerSec 'calculate the speed (meters/s) based on roller rad/s
                            Data(DRAG, ACTUAL) = Data(SPEED, ACTUAL) ^ 3 * ForceAir  'calculate drag based on vehicle speed (meters/s)

                            If Data(RPM1_ROLLER, ACTUAL) > Data(RPM1_ROLLER, MAXIMUM) Then  'set the maximum values for roller, wheel, and motor RPM; Speed and Drag
                                Data(RPM1_ROLLER, MAXIMUM) = Data(RPM1_ROLLER, ACTUAL)
                                Data(RPM1_WHEEL, MAXIMUM) = Data(RPM1_WHEEL, ACTUAL)
                                Data(RPM1_MOTOR, MAXIMUM) = Data(RPM1_MOTOR, ACTUAL)
                                Data(SPEED, MAXIMUM) = Data(SPEED, ACTUAL)
                                Data(DRAG, MAXIMUM) = Data(DRAG, ACTUAL)
                            End If

                            If Data(RPM1_ROLLER, ACTUAL) < Data(RPM1_ROLLER, MINIMUM) Then
                                Data(RPM1_ROLLER, MINIMUM) = Data(RPM1_ROLLER, ACTUAL)
                                Data(RPM1_WHEEL, MINIMUM) = Data(RPM1_WHEEL, ACTUAL)
                                Data(RPM1_MOTOR, MINIMUM) = Data(RPM1_MOTOR, ACTUAL)
                                Data(SPEED, MINIMUM) = Data(SPEED, ACTUAL)
                                Data(DRAG, MINIMUM) = Data(DRAG, ACTUAL)
                            End If

                            Data(TORQUE_ROLLER, ACTUAL) = (Data(RPM1_ROLLER, ACTUAL) - OldAngularVelocity) / ElapsedTime * DynoMomentOfInertia 'calculate torque based on angular acceleration (delta speed per time) and MOI
                            'removing the power calc based on average 06DEC13
                            'Data(POWER, ACTUAL) = Data(TORQUE_ROLLER, ACTUAL) * ((Data(RPM1_ROLLER, ACTUAL) + OldAngularVelocity) / 2) 'calculate power based on torque and average angular velocity between two points
                            Data(POWER, ACTUAL) = Data(TORQUE_ROLLER, ACTUAL) * Data(RPM1_ROLLER, ACTUAL) ' + OldAngularVelocity) / 2) 'calculate power based on torque and average angular velocity between two points
                            If Data(POWER, ACTUAL) > 0 Then
                                Data(EFFICIENCY, ACTUAL) = Data(WATTS_IN, ACTUAL) / Data(POWER, ACTUAL)
                            Else
                                Data(EFFICIENCY, ACTUAL) = 0
                            End If
                            'Original versions of wheel and motor torque relied on back calc from Power
                            Data(TORQUE_WHEEL, ACTUAL) = Data(POWER, ACTUAL) / Data(RPM1_WHEEL, ACTUAL) 'back calculate the torque at the wheel and motor based on the calculated power
                            Data(TORQUE_MOTOR, ACTUAL) = Data(POWER, ACTUAL) / Data(RPM1_MOTOR, ACTUAL)

                            If Data(TORQUE_ROLLER, ACTUAL) > Data(TORQUE_ROLLER, MAXIMUM) Then 'set the maximum values for torque
                                Data(TORQUE_ROLLER, MAXIMUM) = Data(TORQUE_ROLLER, ACTUAL)
                                Data(TORQUE_WHEEL, MAXIMUM) = Data(TORQUE_WHEEL, ACTUAL)
                                Data(TORQUE_MOTOR, MAXIMUM) = Data(TORQUE_MOTOR, ACTUAL)
                            End If

                            If Data(TORQUE_ROLLER, ACTUAL) < Data(TORQUE_ROLLER, MINIMUM) Then
                                Data(TORQUE_ROLLER, MINIMUM) = Data(TORQUE_ROLLER, ACTUAL)
                                Data(TORQUE_WHEEL, MINIMUM) = Data(TORQUE_WHEEL, ACTUAL)
                                Data(TORQUE_MOTOR, MINIMUM) = Data(TORQUE_MOTOR, ACTUAL)
                            End If

                            If Data(POWER, ACTUAL) > Data(POWER, MAXIMUM) Then 'set the maximum value for power
                                Data(POWER, MAXIMUM) = Data(POWER, ACTUAL)
                            End If
                            If Data(POWER, ACTUAL) < Data(POWER, MINIMUM) Then
                                Data(POWER, MINIMUM) = Data(POWER, ACTUAL)
                            End If
                            If Data(EFFICIENCY, ACTUAL) > Data(EFFICIENCY, MAXIMUM) Then 'set the maximum value for power
                                Data(EFFICIENCY, MAXIMUM) = Data(EFFICIENCY, ACTUAL)
                            End If
                            If Data(EFFICIENCY, ACTUAL) < Data(EFFICIENCY, MINIMUM) Then 'set the maximum value for power
                                Data(EFFICIENCY, MINIMUM) = Data(EFFICIENCY, ACTUAL)
                            End If

                            OldAngularVelocity = Data(RPM1_ROLLER, ACTUAL) 'remember the current angular velocity for next pulse

                            Select Case WhichDataMode 'Live, PowerRun or LogRaw ?
                                Case Is = LIVE
                                    'Don't do anything.  This helps skip through the Select Case Faster
                                Case Is = POWERRUN
                                    If Data(RPM1_ROLLER, ACTUAL) > ActualPowerRunThreshold Then
                                        DataPoints += 1
                                        If DataPoints = 1 Then
                                            TotalElapsedTime = 0
                                        Else
                                            TotalElapsedTime += ElapsedTime
                                        End If
                                        If DataPoints = MinimumPowerRunPoints Then btnStartPowerRun.BackColor = Color.Green
                                        CollectedData(SESSIONTIME, DataPoints) = TotalElapsedTime
                                        CollectedData(RPM1_ROLLER, DataPoints) = Data(RPM1_ROLLER, ACTUAL)
                                        CollectedData(RPM2, DataPoints) = Data(RPM2, ACTUAL)
                                        CollectedData(VOLTS, DataPoints) = Data(VOLTS, ACTUAL)
                                        CollectedData(AMPS, DataPoints) = Data(AMPS, ACTUAL)
                                        CollectedData(TEMPERATURE1, DataPoints) = Data(TEMPERATURE1, ACTUAL)
                                        CollectedData(TEMPERATURE2, DataPoints) = Data(TEMPERATURE2, ACTUAL)
                                        CollectedData(PIN04VALUE, DataPoints) = Data(PIN04VALUE, ACTUAL)
                                        CollectedData(PIN05VALUE, DataPoints) = Data(PIN05VALUE, ACTUAL)
                                        CollectedData(CHAN1_FREQUENCY, DataPoints) = Data(CHAN1_FREQUENCY, ACTUAL)
                                        CollectedData(CHAN1_PULSEWIDTH, DataPoints) = Data(CHAN1_PULSEWIDTH, ACTUAL)
                                        CollectedData(CHAN1_DUTYCYCLE, DataPoints) = Data(CHAN1_DUTYCYCLE, ACTUAL)
                                        CollectedData(CHAN2_FREQUENCY, DataPoints) = Data(CHAN2_FREQUENCY, ACTUAL)
                                        CollectedData(CHAN2_PULSEWIDTH, DataPoints) = Data(CHAN2_PULSEWIDTH, ACTUAL)
                                        CollectedData(CHAN2_DUTYCYCLE, DataPoints) = Data(CHAN2_DUTYCYCLE, ACTUAL)
                                        'DataPoints += 1
                                        If DataPoints = MAXDATAPOINTS Then
                                            DataPoints = MAXDATAPOINTS - 1
                                            'btnStartPowerRun.BackColor = Color.Red
                                        End If
                                    End If
                                Case Is = LOGRAW
                                    DataPoints += 1
                                    If DataPoints = 1 Then
                                        btnStartLoggingRaw.BackColor = Color.Green
                                        TotalElapsedTime = 0
                                    Else
                                        TotalElapsedTime += ElapsedTime
                                    End If
                                    CollectedData(SESSIONTIME, DataPoints) = TotalElapsedTime
                                    CollectedData(RPM1_ROLLER, DataPoints) = Data(RPM1_ROLLER, ACTUAL)
                                    CollectedData(RPM2, DataPoints) = Data(RPM2, ACTUAL)
                                    CollectedData(VOLTS, DataPoints) = Data(VOLTS, ACTUAL)
                                    CollectedData(AMPS, DataPoints) = Data(AMPS, ACTUAL)
                                    CollectedData(TEMPERATURE1, DataPoints) = Data(TEMPERATURE1, ACTUAL)
                                    CollectedData(TEMPERATURE2, DataPoints) = Data(TEMPERATURE2, ACTUAL)
                                    CollectedData(PIN04VALUE, DataPoints) = Data(PIN04VALUE, ACTUAL)
                                    CollectedData(PIN05VALUE, DataPoints) = Data(PIN05VALUE, ACTUAL)
                                    CollectedData(CHAN1_FREQUENCY, DataPoints) = Data(CHAN1_FREQUENCY, ACTUAL)
                                    CollectedData(CHAN1_PULSEWIDTH, DataPoints) = Data(CHAN1_PULSEWIDTH, ACTUAL)
                                    CollectedData(CHAN1_DUTYCYCLE, DataPoints) = Data(CHAN1_DUTYCYCLE, ACTUAL)
                                    CollectedData(CHAN2_FREQUENCY, DataPoints) = Data(CHAN2_FREQUENCY, ACTUAL)
                                    CollectedData(CHAN2_PULSEWIDTH, DataPoints) = Data(CHAN2_PULSEWIDTH, ACTUAL)
                                    CollectedData(CHAN2_DUTYCYCLE, DataPoints) = Data(CHAN2_DUTYCYCLE, ACTUAL)
                                    ' DataPoints += 1
                                    If DataPoints = MAXDATAPOINTS Then
                                        DataPoints = MAXDATAPOINTS - 1
                                        btnStartLoggingRaw.BackColor = Color.Red
                                    End If
                            End Select
                        End If
                        End If
                        If (WhichSignal = HIGHSIGNAL AndAlso RawWaveData(j) <= HighSignalThreshold - 3) Or (WhichSignal = LOWSIGNAL AndAlso RawWaveData(j) >= HighSignalThreshold + 3) Then
                            If FoundHighSignal = True Then
                                Data(CHAN1_PULSEWIDTH, ACTUAL) = k * BytesToSeconds * 1000
                                If Data(CHAN1_PULSEWIDTH, ACTUAL) > Data(CHAN1_PULSEWIDTH, MAXIMUM) Then Data(CHAN1_PULSEWIDTH, MAXIMUM) = Data(CHAN1_PULSEWIDTH, ACTUAL)
                                If Data(CHAN1_PULSEWIDTH, ACTUAL) < Data(CHAN1_PULSEWIDTH, MINIMUM) Then Data(CHAN1_PULSEWIDTH, MINIMUM) = Data(CHAN1_PULSEWIDTH, ACTUAL)
                                k = 0
                                Data(CHAN1_DUTYCYCLE, ACTUAL) = Data(CHAN1_PULSEWIDTH, ACTUAL) * Data(CHAN1_FREQUENCY, ACTUAL) / 10
                                If Data(CHAN1_DUTYCYCLE, ACTUAL) > Data(CHAN1_DUTYCYCLE, MAXIMUM) Then Data(CHAN1_DUTYCYCLE, MAXIMUM) = Data(CHAN1_DUTYCYCLE, ACTUAL)
                                If Data(CHAN1_DUTYCYCLE, ACTUAL) < Data(CHAN1_DUTYCYCLE, MINIMUM) Then Data(CHAN1_DUTYCYCLE, MINIMUM) = Data(CHAN1_DUTYCYCLE, ACTUAL)
                            End If
                            FoundHighSignal = False
                        End If

                        LastSignal = RawWaveData(j) 'remember the last high signal and the current correction time

                        If NUMBER_OF_CHANNELS = 2 Then

                            SignalWindowBMP.DrawLine(Channel2ThresholdPen, 0, SignalThreshold2YConverted, PicSignalWidth, SignalThreshold2YConverted)
                            NextYPosition2 = CInt(PicSignalHeight - RawWaveData(j + 1) * SignalYConversion) + 1    'calculate coordinate for next channel 2 signal point
                            SignalWindowBMP.DrawLine(Channel2SignalPen, CInt(CurrentSignalXPosition - SignalXConversion), LastYPosition2, CInt(CurrentSignalXPosition), NextYPosition2) 'draw line to the newly calculated point...
                            LastYPosition2 = NextYPosition2 '...and remember the new position for the next cycle

                            If (WhichSignal2 = HIGHSIGNAL AndAlso RawWaveData(j + 1) > HighSignalThreshold2) Or (WhichSignal2 = LOWSIGNAL AndAlso RawWaveData(j + 1) < HighSignalThreshold2) Then 'Check is we have found a signal depending on where the threshold line is set

                                k2 = k2 + NUMBER_OF_CHANNELS 'count bytes for channel 2 pulsewidth

                                If FoundHighSignal2 = False Then    'if FoundHighSigal is false, then this is the start of a new pulse not the middle of an existing pulse

                                    FoundHighSignal2 = True 'flag that we are in the middle of a pulse

                                    Select Case UseAdvancedProcessing 'Calculate elapsed time simply by the byte count since last or by interpolation through the threshold line
                                        Case Is = False
                                            ElapsedTime2 = (j + 1 - LastHighBufferPosition2) * BytesToSeconds 'calculate the elapsed time by multiplying the number of bytes since the last pulse 'by the time taken for each byte (which depends on the sampling rate)
                                        Case Is = True
                                            NewElapsedTimeCorrection2 = Math.Abs((RawWaveData(j + 1) - HighSignalThreshold) / (RawWaveData(j + 1) - LastSignal2))
                                            ElapsedTime2 = ((j + 1 - LastHighBufferPosition2) + OldElapsedTimeCorrection2 - NewElapsedTimeCorrection2) * BytesToSeconds
                                            OldElapsedTimeCorrection2 = NewElapsedTimeCorrection2
                                    End Select

                                    LastHighBufferPosition2 = j + 1   'set the current buffer position for the next pulse

                                    Data(CHAN2_FREQUENCY, ACTUAL) = 1 / ElapsedTime2 'calculate frequency for scope work 
                                    If Data(CHAN2_FREQUENCY, ACTUAL) > Data(CHAN2_FREQUENCY, MAXIMUM) Then Data(CHAN2_FREQUENCY, MAXIMUM) = Data(CHAN2_FREQUENCY, ACTUAL)
                                    If Data(CHAN2_FREQUENCY, ACTUAL) < Data(CHAN2_FREQUENCY, MINIMUM) Then Data(CHAN2_FREQUENCY, MINIMUM) = Data(CHAN2_FREQUENCY, ACTUAL)


                                    Data(RPM2, ACTUAL) = ElapsedTimeToRadPerSec2 / ElapsedTime2 'calculate roller angular velocity in Rad/s
                                    Data(RPM2_RATIO, ACTUAL) = Data(RPM2, ACTUAL) / Data(RPM1_WHEEL, ACTUAL) 'calculate the ratios between RPM2 and RPM1 - wheel

                                    Data(RPM2_ROLLOUT, ACTUAL) = WheelCircumference / Data(RPM2_RATIO, ACTUAL) 'calculate Rollout (default unit is mm).  This assumes RPM2 is measuring motor RPM   'Rollout is the number of mm traveled for 1 rotation of the wheel

                                    If Data(RPM2, ACTUAL) > Data(RPM2, MAXIMUM) Then 'check and set maximum values for RPM2, Ratio and rollout
                                        Data(RPM2, MAXIMUM) = Data(RPM2, ACTUAL)
                                    End If
                                    If Data(RPM2, ACTUAL) < Data(RPM2, MINIMUM) Then
                                        Data(RPM2, MINIMUM) = Data(RPM2, ACTUAL)
                                    End If
                                    If Data(RPM2_RATIO, ACTUAL) > Data(RPM2_RATIO, MAXIMUM) Then
                                        Data(RPM2_RATIO, MAXIMUM) = Data(RPM2_RATIO, ACTUAL)
                                    End If
                                    If Data(RPM2_RATIO, ACTUAL) < Data(RPM2_RATIO, MINIMUM) Then
                                        Data(RPM2_RATIO, MINIMUM) = Data(RPM2_RATIO, ACTUAL)
                                    End If
                                    If Data(RPM2_ROLLOUT, ACTUAL) > Data(RPM2_ROLLOUT, MAXIMUM) Then
                                        Data(RPM2_ROLLOUT, MAXIMUM) = Data(RPM2_ROLLOUT, ACTUAL)
                                    End If
                                    If Data(RPM2_ROLLOUT, ACTUAL) < Data(RPM2_ROLLOUT, MINIMUM) Then
                                        Data(RPM2_ROLLOUT, MINIMUM) = Data(RPM2_ROLLOUT, ACTUAL)
                                    End If
                                End If
                            End If
                            If (WhichSignal2 = HIGHSIGNAL AndAlso RawWaveData(j + 1) <= HighSignalThreshold2 - 3) Or (WhichSignal2 = LOWSIGNAL AndAlso RawWaveData(j + 1) >= HighSignalThreshold2 + 3) Then
                                If FoundHighSignal2 = True Then
                                    Data(CHAN2_PULSEWIDTH, ACTUAL) = k2 * BytesToSeconds * 1000
                                    k2 = 0
                                    If Data(CHAN2_PULSEWIDTH, ACTUAL) > Data(CHAN2_PULSEWIDTH, MAXIMUM) Then Data(CHAN2_PULSEWIDTH, MAXIMUM) = Data(CHAN2_PULSEWIDTH, ACTUAL)
                                    If Data(CHAN2_PULSEWIDTH, ACTUAL) < Data(CHAN2_PULSEWIDTH, MINIMUM) Then Data(CHAN2_PULSEWIDTH, MINIMUM) = Data(CHAN2_PULSEWIDTH, ACTUAL)
                                    Data(CHAN2_DUTYCYCLE, ACTUAL) = Data(CHAN2_PULSEWIDTH, ACTUAL) * Data(CHAN2_FREQUENCY, ACTUAL) / 10
                                    If Data(CHAN2_DUTYCYCLE, ACTUAL) > Data(CHAN2_DUTYCYCLE, MAXIMUM) Then Data(CHAN2_DUTYCYCLE, MAXIMUM) = Data(CHAN2_DUTYCYCLE, ACTUAL)
                                    If Data(CHAN2_DUTYCYCLE, ACTUAL) < Data(CHAN2_DUTYCYCLE, MINIMUM) Then Data(CHAN2_DUTYCYCLE, MINIMUM) = Data(CHAN2_DUTYCYCLE, ACTUAL)
                                End If

                                FoundHighSignal2 = False

                            End If
                            LastSignal2 = RawWaveData(j + 1) 'remember the last high signal and the current correction time
                        End If
                        CurrentSignalXPosition = (CurrentSignalXPosition + SignalXConversion) Mod PicSignalWidth

                Next

                pnlSignalWindow.Invalidate()

                If NUMBER_OF_CHANNELS = 2 Then
                    LastHighBufferPosition2 = LastHighBufferPosition2 - BUFFER_SIZE
                    If LastHighBufferPosition2 / SAMPLE_RATE * -1 > WaitForNewSignal * NUMBER_OF_CHANNELS Then
                        Data(RPM2, ACTUAL) = 0
                        Data(RPM2_RATIO, ACTUAL) = 0
                        Data(RPM2_ROLLOUT, ACTUAL) = 0
                        Data(CHAN2_DUTYCYCLE, ACTUAL) = 0
                        Data(CHAN2_FREQUENCY, ACTUAL) = 0
                        Data(CHAN2_PULSEWIDTH, ACTUAL) = 0
                    End If
                End If

                LastHighBufferPosition = LastHighBufferPosition - BUFFER_SIZE
                If LastHighBufferPosition / SAMPLE_RATE * -1 > WaitForNewSignal * NUMBER_OF_CHANNELS Then
                    Data(RPM1_ROLLER, ACTUAL) = 0
                    Data(RPM1_WHEEL, ACTUAL) = 0
                    Data(RPM1_MOTOR, ACTUAL) = 0
                    Data(SPEED, ACTUAL) = 0
                    Data(DRAG, ACTUAL) = 0
                    Data(TORQUE_ROLLER, ACTUAL) = 0
                    Data(TORQUE_WHEEL, ACTUAL) = 0
                    Data(TORQUE_MOTOR, ACTUAL) = 0
                    Data(POWER, ACTUAL) = 0
                    Data(RPM2_RATIO, ACTUAL) = 0
                    Data(CHAN1_DUTYCYCLE, ACTUAL) = 0
                    Data(CHAN1_FREQUENCY, ACTUAL) = 0
                    Data(CHAN1_PULSEWIDTH, ACTUAL) = 0
                    If WhichDataMode = LOGRAW Then
                        TotalElapsedTime += ElapsedTime
                        CollectedData(SESSIONTIME, DataPoints) = TotalElapsedTime
                        CollectedData(RPM1_ROLLER, DataPoints) = Data(RPM1_ROLLER, ACTUAL)
                        CollectedData(RPM2, DataPoints) = Data(RPM2, ACTUAL)
                        CollectedData(VOLTS, DataPoints) = Data(VOLTS, ACTUAL)
                        CollectedData(AMPS, DataPoints) = Data(AMPS, ACTUAL)
                        CollectedData(TEMPERATURE1, DataPoints) = Data(TEMPERATURE1, ACTUAL)
                        CollectedData(TEMPERATURE2, DataPoints) = Data(TEMPERATURE2, ACTUAL)
                        CollectedData(PIN04VALUE, DataPoints) = Data(PIN04VALUE, ACTUAL)
                        CollectedData(PIN05VALUE, DataPoints) = Data(PIN05VALUE, ACTUAL)
                        CollectedData(CHAN1_FREQUENCY, DataPoints) = Data(CHAN1_FREQUENCY, ACTUAL)
                        CollectedData(CHAN1_PULSEWIDTH, DataPoints) = Data(CHAN1_PULSEWIDTH, ACTUAL)
                        CollectedData(CHAN1_DUTYCYCLE, DataPoints) = Data(CHAN1_DUTYCYCLE, ACTUAL)
                        CollectedData(CHAN2_FREQUENCY, DataPoints) = Data(CHAN2_FREQUENCY, ACTUAL)
                        CollectedData(CHAN2_PULSEWIDTH, DataPoints) = Data(CHAN2_PULSEWIDTH, ACTUAL)
                        CollectedData(CHAN2_DUTYCYCLE, DataPoints) = Data(CHAN2_DUTYCYCLE, ACTUAL)
                        DataPoints += 1
                        If DataPoints = MAXDATAPOINTS Then
                            DataPoints = MAXDATAPOINTS - 1
                            btnStartLoggingRaw.BackColor = Color.Red
                        End If
                    End If
                End If

                If WhichDataMode = POWERRUN AndAlso DataPoints > MinimumPowerRunPoints AndAlso Data(RPM1_ROLLER, ACTUAL) <= ActualPowerRunThreshold Then
                    SetControlBackColor_ThreadSafe(btnStartPowerRun, System.Windows.Forms.Control.DefaultBackColor)
                    'DataPoints -= 1
                    PauseForms()
                    WhichDataMode = LIVE
                End If
            End If
            BufferCount = BufferCount + 1
            If BufferCount > NUMBER_OF_BUFFERS - 1 Then BufferCount = 0
#If QueryPerformance Then
            QueryPerformanceCounter(StopWatch)
            If PerfBufferCount < 150 Then PerfBufferCount += 1
            Data(PERFORMANCE, ACTUAL) = ((StopWatch - StartWatch) / WatchTickConversion * 1000) / (BUFFER_SIZE / SAMPLE_RATE * 1000) * 100
            PerformanceData(P_FREQ, PerfBufferCount) = Data(CHAN1_FREQUENCY, ACTUAL)
            PerformanceData(P_TIME, PerfBufferCount) = Data(PERFORMANCE, ACTUAL)
            '11MAY2011 Sub running at an average of 0.01 secs with buffers being retured at 3200 (buffsize) / 44100 (rate) which is 0.07 seconds
            '29NOV2012 Sub running at average 8 ms with theoretical callback every 72 ms
            '17DEC12 Sub Running at 8 - 10 ms with theoretical callback every 72 ms
            '12APR13 Sub Running at around 14 ms 
            '19SEP13 Sub Running at around 20 ms for 2 channels, 44100 Hz, doesn't seem to be significantly affected id COM port is open.
            '19SEP13 Max Processing time for 2Ch, 44K, COM Port 9600 Baud is 47ms
#End If
        Catch e As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("myWaveHandler_ProcessWave Error: " & e.ToString, MsgBoxStyle.Exclamation)
            End
        End Try
    End Sub
    Private Sub ShutDownWaves()
        If WavesStarted Then
            Try
                Dim temp As Integer, counter As Integer
                StopAddingBuffers = True
                Do
                    Application.DoEvents()
                    temp = 0
                    For counter = 0 To NUMBER_OF_BUFFERS - 1
                        If WaveBufferHeaders(counter).dwBytesRecorded = WaveBufferHeaders(counter).dwBufferLength Then
                            temp = temp + 1
                        End If
                    Next
                Loop Until temp = NUMBER_OF_BUFFERS 'Not InCallBackProcedure
                i = waveInReset(WaveInHandle)
                If i <> 0 Then
                    btnHide_Click(Me, EventArgs.Empty)
                    MsgBox("ShutDownWaves / waveInReset Error", MsgBoxStyle.Exclamation)
                    End
                End If
                i = waveInStop(WaveInHandle)
                If i <> 0 Then
                    btnHide_Click(Me, EventArgs.Empty)
                    MsgBox("ShutDownWaves / waveInStop Error", MsgBoxStyle.Exclamation)
                    End
                End If
                For j = 0 To NUMBER_OF_BUFFERS - 1
                    i = waveInUnprepareHeader(WaveInHandle, WaveBufferHeaders(j), Marshal.SizeOf(WaveBufferHeaders(j)))
                    If i <> 0 Then
                        btnHide_Click(Me, EventArgs.Empty)
                        MsgBox("ShutDownWaves / waveInUnprepareHeader Error" & i, MsgBoxStyle.Exclamation)
                        End
                    End If
                Next
                bufferpin0.Free()
                bufferpin1.Free()
                bufferpin2.Free()
                bufferpin3.Free()
                bufferpin4.Free()
                bufferpin5.Free()
                bufferpin6.Free()
                bufferpin7.Free()
                bufferpin8.Free()
                bufferpin9.Free()
                bufferpin10.Free()
                bufferpin11.Free()
                bufferpin12.Free()
                bufferpin13.Free()
                bufferpin14.Free()
                bufferpin15.Free()
                bufferpin16.Free()
                bufferpin17.Free()
                bufferpin18.Free()
                bufferpin19.Free()
                i = waveInClose(WaveInHandle)
                If i <> 0 Then
                    btnHide_Click(Me, EventArgs.Empty)
                    MsgBox("ShutDownWaves / waveInClose Error", MsgBoxStyle.Exclamation)
                    End
                Else

                End If
                BufferCount = 0
                StopAddingBuffers = False
                WavesStarted = False
            Catch e As Exception
                btnHide_Click(Me, EventArgs.Empty)
                MsgBox("ShutDownWaves Error: " & e.ToString, MsgBoxStyle.Exclamation)
                End
            End Try
        End If
    End Sub
#End Region
#Region "Serial Port Communications"
    Private Sub cmbAcquisition_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcquisition.SelectedIndexChanged
        If InStr(cmbAcquisition.SelectedItem.ToString, "Audio") <> 0 Then
            cmbChannels.Enabled = True
            cmbSampleRate.Enabled = True
        Else
            cmbChannels.Enabled = False
            cmbSampleRate.Enabled = False
        End If
        If InStr(cmbAcquisition.SelectedItem.ToString, "COM") <> 0 Then
            cmbCOMPorts.Enabled = True
            cmbBaudRate.Enabled = True
            With frmFit
                .rdoCurrent.Enabled = True
                .rdoVoltage.Enabled = True
                .txtCurrentSmooth.Enabled = True
                .txtVoltageSmooth.Enabled = True
                .scrlCurrentSmooth.Enabled = True
                .scrlVoltageSmooth.Enabled = True
            End With
        Else
            cmbCOMPorts.Enabled = False
            cmbBaudRate.Enabled = False
            With frmFit
                .rdoCurrent.Enabled = False
                .rdoVoltage.Enabled = False
                .txtCurrentSmooth.Enabled = False
                .txtVoltageSmooth.Enabled = False
                .scrlCurrentSmooth.Enabled = False
                .scrlVoltageSmooth.Enabled = False
            End With
        End If
    End Sub
    Private Sub btnStartAcquisition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartAcquisition.Click
        btnStartAcquisition.Enabled = False
        PauseForms()
        ShutDownWaves()
        SerialClose()
        SignalWindowBMP.Clear(System.Windows.Forms.Control.DefaultBackColor)
        pnlSignalWindow.BackgroundImage = SignalBitmap
        pnlSignalWindow.Invalidate()
        SetControlBackColor_ThreadSafe(lblCOMActive, System.Windows.Forms.Control.DefaultBackColor)
        'Set all parameters to be available in interface components
        For count As Integer = 1 To LAST - 1
            DataAreUsed(count) = True
        Next

        'Disable all calibrate buttons on the COM form
        For Each c As Control In frmCOM.Controls
            If TypeOf c Is Button Then
                c.Enabled = False
            End If
        Next

        If InStr(cmbAcquisition.SelectedItem.ToString, "Audio") <> 0 Then
            SAMPLE_RATE = CInt(cmbSampleRate.SelectedItem.ToString.Substring(0, cmbSampleRate.SelectedItem.ToString.IndexOf(" ")))
            NUMBER_OF_CHANNELS = CInt(cmbChannels.SelectedItem.ToString.Substring(0, cmbChannels.SelectedItem.ToString.IndexOf(" ")))
#If QueryPerformance Then
            BUFFER_SIZE = CInt(cmbBufferSize.SelectedItem.ToString.Substring(0, cmbBufferSize.SelectedItem.ToString.IndexOf(" "))) * NUMBER_OF_CHANNELS
#Else
            Select Case SAMPLE_RATE
                Case Is = 11025
                    BUFFER_SIZE = 1024 * NUMBER_OF_CHANNELS
                Case Is = 22050
                    BUFFER_SIZE = 2048 * NUMBER_OF_CHANNELS
                Case Is = 44100
                    BUFFER_SIZE = 4096 * NUMBER_OF_CHANNELS
            End Select
#End If

            If NUMBER_OF_CHANNELS = 1 Then
                DataAreUsed(RPM2) = False
                DataAreUsed(RPM2_RATIO) = False
                DataAreUsed(RPM2_ROLLOUT) = False
                DataAreUsed(CHAN2_FREQUENCY) = False
                DataAreUsed(CHAN2_PULSEWIDTH) = False
                DataAreUsed(CHAN2_DUTYCYCLE) = False
            End If

            ReDim RawWaveData(BUFFER_SIZE - 1)
            ReDim RawWaveData0(BUFFER_SIZE - 1)
            ReDim RawWaveData1(BUFFER_SIZE - 1)
            ReDim RawWaveData2(BUFFER_SIZE - 1)
            ReDim RawWaveData3(BUFFER_SIZE - 1)
            ReDim RawWaveData4(BUFFER_SIZE - 1)
            ReDim RawWaveData5(BUFFER_SIZE - 1)
            ReDim RawWaveData6(BUFFER_SIZE - 1)
            ReDim RawWaveData7(BUFFER_SIZE - 1)
            ReDim RawWaveData8(BUFFER_SIZE - 1)
            ReDim RawWaveData9(BUFFER_SIZE - 1)
            ReDim RawWaveData10(BUFFER_SIZE - 1)
            ReDim RawWaveData11(BUFFER_SIZE - 1)
            ReDim RawWaveData12(BUFFER_SIZE - 1)
            ReDim RawWaveData13(BUFFER_SIZE - 1)
            ReDim RawWaveData14(BUFFER_SIZE - 1)
            ReDim RawWaveData15(BUFFER_SIZE - 1)
            ReDim RawWaveData16(BUFFER_SIZE - 1)
            ReDim RawWaveData17(BUFFER_SIZE - 1)
            ReDim RawWaveData18(BUFFER_SIZE - 1)
            ReDim RawWaveData19(BUFFER_SIZE - 1)

            ReDim WaveBufferHeaders(NUMBER_OF_BUFFERS - 1)

            BytesToSeconds = 1 / (SAMPLE_RATE * NUMBER_OF_CHANNELS)
            ElapsedTimeUnit = BytesToSeconds * NUMBER_OF_CHANNELS
            PrepareGraphicsParameters()
            InitializeWaveInput()
        Else
            'CHECK - CLEAR THE SIGNAL WINDOW SCREEN TO GRAY IF NOT USED
            'Scope stuff only uses audio
            DataAreUsed(CHAN1_FREQUENCY) = False
            DataAreUsed(CHAN1_PULSEWIDTH) = False
            DataAreUsed(CHAN1_DUTYCYCLE) = False
            DataAreUsed(CHAN2_FREQUENCY) = False
            DataAreUsed(CHAN2_PULSEWIDTH) = False
            DataAreUsed(CHAN2_DUTYCYCLE) = False

        End If

        If InStr(cmbAcquisition.SelectedItem.ToString, "COM") <> 0 Then
            SerialOpen(cmbCOMPorts.Items(cmbCOMPorts.SelectedIndex).ToString, CInt(cmbBaudRate.Items(cmbBaudRate.SelectedIndex)))
        Else
            DataAreUsed(VOLTS) = False
            DataAreUsed(AMPS) = False
            DataAreUsed(WATTS_IN) = False
            DataAreUsed(EFFICIENCY) = False
            DataAreUsed(TEMPERATURE1) = False
            DataAreUsed(TEMPERATURE2) = False
            DataAreUsed(PIN04VALUE) = False
            DataAreUsed(PIN05VALUE) = False
        End If

        


        btnResetMaxima_Click(Me, System.EventArgs.Empty)
        btnStartAcquisition.Enabled = True

        'Need to update the menus available in already loaded graphical interface components
        'CHECK = Ultimately we should reconsider creating the menus live on right click to allow for min max to be listed in textboxes
        For Each SDFrm As SimpleDynoSubForm In f
            SDFrm.CreateTheMenu()
        Next
        RestartForms()
    End Sub
    Private ClosingCOMPort As Boolean = False
    Private Sub GetAvailableCOMPorts()
        Dim Problem As String
        Problem = "No Value"
        Try
            Problem = "Setting COM Available to false"
            COMPortsAvailable = False
            Problem = "Creating PortSearcher Object"
            Dim portSearcher As New ManagementObjectSearcher("\root\CIMV2", "SELECT Name from Win32_PnPEntity WHERE ConfigManagerErrorCode = 0") 'PnPEntity")
            Problem = "Starting Loop For Ports"
            For Each port As System.Management.ManagementObject In portSearcher.Get()
                Problem = "About to check port name"
                If port("Name") IsNot Nothing AndAlso port("Name").ToString.ToUpper.Contains("(COM") Then
                    Problem = "Found COM - adding to CMB"
                    cmbCOMPorts.Items.Add(port("Name").ToString)
                    Problem = "Setting COM Available to True"
                    COMPortsAvailable = True
                End If
            Next
            Problem = "Check Available COM Ports Status"
            If COMPortsAvailable Then
                Problem = "Yes - COM port available setting index to 0"
                cmbCOMPorts.SelectedIndex = 0
                Problem = "Creating Baud string"
                Dim AvailableBauds() As String = {9600.ToString, 14400.ToString, 19200.ToString, 28800.ToString, 38400.ToString, 57600.ToString, 115200.ToString}
                Problem = "Adding Baud to CMB"
                cmbBaudRate.Items.AddRange(AvailableBauds)
                Problem = "Setting Index to 0"
                cmbBaudRate.SelectedIndex = 0
            End If
            'MsgBox("No Problems Found", MsgBoxStyle.OkOnly)
        Catch ex As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("Error found is " & Problem & " " & ex.ToString, MsgBoxStyle.Exclamation)
            btnShow_Click(Me, EventArgs.Empty)
            'Maybe Use GetPortNames 
            'Dim AvailablePorts() As String = SerialPort.GetPortNames
            'If AvailablePorts.Length > 0 Then ...
        End Try
    End Sub
    Private Sub SerialOpen(ByVal SentPort As String, ByVal SentRate As Integer)
        Do Until mySerialPort.IsOpen = False
            Application.DoEvents()
        Loop
        SentPort = "COM" & SentPort.Substring(SentPort.IndexOf("(COM") + 4).TrimEnd(")"c)
        mySerialPort = New SerialPort(SentPort)
        mySerialPort.BaudRate = SentRate
        mySerialPort.Parity = Parity.None
        mySerialPort.StopBits = StopBits.One
        mySerialPort.DataBits = 8
        mySerialPort.Handshake = Handshake.None
        mySerialPort.ReadTimeout = 500
        'AddHandler mySerialPort.DataReceived, AddressOf DataReceivedHandler
        Try
            mySerialPort.Open()
            mySerialPort.DiscardInBuffer()
            mySerialPort.ReadLine()
            AddHandler mySerialPort.DataReceived, AddressOf DataReceivedHandler
            'Enable Calibration buttons on com form
            For Each c As Control In frmCOM.Controls
                If TypeOf c Is Button Then
                    c.Enabled = True
                End If
            Next
        Catch e As Exception
            btnHide_Click(Me, EventArgs.Empty)
            MsgBox("Error reading COM Port.", CType(vbOK, MsgBoxStyle))
            If mySerialPort.IsOpen Then mySerialPort.Close()
            'Enable Calibration buttons on com form
            For Each c As Control In frmCOM.Controls
                If TypeOf c Is Button Then
                    c.Enabled = False
                End If
            Next
            btnShow_Click(Me, EventArgs.Empty)
        End Try
    End Sub
    Private Sub SerialClose()
        ClosingCOMPort = True
        If mySerialPort.IsOpen Then
            RemoveHandler mySerialPort.DataReceived, AddressOf DataReceivedHandler
            Dim t As Integer
            'CHECK - this is a real hack
            Do Until t = 100000
                t += 1
                Application.DoEvents()
            Loop
            mySerialPort.Close()
        End If
        ClosingCOMPort = False
    End Sub
    Private Sub DataReceivedHandler(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        If Not ClosingCOMPort Then
            Try
                Dim Locker As New Object
                Dim sp As SerialPort = CType(sender, SerialPort)
                Dim temp As String
                Static rgbvalue As Integer = 150, rgbincrement As Integer = 10
                Static OldSessionTime As Double
                Dim RPM1ElapsedTime As Double, RPM1NewTriggerTime As Double
                Dim RPM2ElapsedTime As Double, RPM2NewTriggerTime As Double
                Static RPM1OldTriggerTime As Double
                Static RPM2OldTriggerTime As Double

                temp = sp.ReadLine
                COMPortMessage = Split(temp, ",")

                rgbvalue += rgbincrement
                If rgbvalue > 240 Or rgbvalue < 50 Then rgbincrement *= -1
                SetControlBackColor_ThreadSafe(lblCOMActive, Color.FromArgb(0, rgbvalue, rgbvalue))

                If COMPortMessage.Length = 11 Then 'Timestamp, 2 new Time values,  2 interrupt times, 6 ports
                    SyncLock Locker
                        Data(VOLTS, ACTUAL) = VoltageIntercept + VoltageSlope * CDbl(COMPortMessage(5)) 'Convert Volts signal to volts
                        Data(AMPS, ACTUAL) = CurrentIntercept + CurrentSlope * CDbl((COMPortMessage(6))) 'Convert Current signal to amps
                        Data(TEMPERATURE1, ACTUAL) = Temperature1Intercept + Temperature1Slope * CDbl(COMPortMessage(7))
                        Data(TEMPERATURE2, ACTUAL) = Temperature2Intercept + Temperature2Slope * CDbl(COMPortMessage(8))
                        Data(PIN04VALUE, ACTUAL) = A4ValueIntercept + A4ValueSlope * CDbl(COMPortMessage(9))
                        Data(PIN05VALUE, ACTUAL) = A5ValueIntercept + A5ValueSlope * CDbl(COMPortMessage(10))

                        If frmCOM.Calibrating Then
                            For t As Integer = 0 To 5
                                frmCOM.CalibrationValues(t) += CDbl(COMPortMessage(t + 5))
                            Next
                            frmCOM.NumberOfCalibrationValues += 1
                        End If

                        Data(WATTS_IN, ACTUAL) = Data(VOLTS, ACTUAL) * Data(AMPS, ACTUAL)

                        If Data(AMPS, ACTUAL) < Data(AMPS, MINIMUM) Then Data(AMPS, MINIMUM) = Data(AMPS, ACTUAL)
                        If Data(AMPS, ACTUAL) > Data(AMPS, MAXIMUM) Then Data(AMPS, MAXIMUM) = Data(AMPS, ACTUAL)
                        If Data(VOLTS, ACTUAL) < Data(VOLTS, MINIMUM) Then Data(VOLTS, MINIMUM) = Data(VOLTS, ACTUAL)
                        If Data(VOLTS, ACTUAL) > Data(VOLTS, MAXIMUM) Then Data(VOLTS, MAXIMUM) = Data(VOLTS, ACTUAL)
                        If Data(WATTS_IN, ACTUAL) < Data(WATTS_IN, MINIMUM) Then Data(WATTS_IN, MINIMUM) = Data(WATTS_IN, ACTUAL)
                        If Data(WATTS_IN, ACTUAL) > Data(WATTS_IN, MAXIMUM) Then Data(WATTS_IN, MAXIMUM) = Data(WATTS_IN, ACTUAL)
                        If Data(TEMPERATURE1, ACTUAL) > Data(TEMPERATURE1, MAXIMUM) Then Data(TEMPERATURE1, MAXIMUM) = Data(TEMPERATURE1, ACTUAL)
                        If Data(TEMPERATURE1, ACTUAL) < Data(TEMPERATURE1, MINIMUM) Then Data(TEMPERATURE1, MINIMUM) = Data(TEMPERATURE1, ACTUAL)
                        If Data(TEMPERATURE2, ACTUAL) > Data(TEMPERATURE2, MAXIMUM) Then Data(TEMPERATURE2, MAXIMUM) = Data(TEMPERATURE2, ACTUAL)
                        If Data(TEMPERATURE2, ACTUAL) < Data(TEMPERATURE2, MINIMUM) Then Data(TEMPERATURE2, MINIMUM) = Data(TEMPERATURE2, ACTUAL)

                        If Data(PIN04VALUE, ACTUAL) > Data(PIN04VALUE, MAXIMUM) Then Data(PIN04VALUE, MAXIMUM) = Data(PIN04VALUE, ACTUAL)
                        If Data(PIN04VALUE, ACTUAL) < Data(PIN04VALUE, MINIMUM) Then Data(PIN04VALUE, MINIMUM) = Data(PIN04VALUE, ACTUAL)

                        If Data(PIN05VALUE, ACTUAL) > Data(PIN05VALUE, MAXIMUM) Then Data(PIN05VALUE, MAXIMUM) = Data(PIN05VALUE, ACTUAL)
                        If Data(PIN05VALUE, ACTUAL) < Data(PIN05VALUE, MINIMUM) Then Data(PIN05VALUE, MINIMUM) = Data(PIN05VALUE, ACTUAL)

                        If Not WavesStarted Then 'Use COM data for timing and RPM
                            Data(SESSIONTIME, ACTUAL) = CDbl(COMPortMessage(0)) / 1000000 'Increase the session time value - used by the Graphing form for time info
                            ElapsedTime = Data(SESSIONTIME, ACTUAL) - OldSessionTime 'CHECK - Are we actually using this anywhere for com communications?
                            RPM1NewTriggerTime = CDbl(COMPortMessage(1)) / 1000000 'RPM1
                            RPM1ElapsedTime = CDbl(COMPortMessage(2)) / 1000000
                            If RPM1NewTriggerTime <> RPM1OldTriggerTime Then 'New trigger detected, go ahead and process RPM relevant info
                                Data(RPM1_ROLLER, ACTUAL) = ElapsedTimeToRadPerSec / RPM1ElapsedTime
                                Data(RPM1_WHEEL, ACTUAL) = Data(RPM1_ROLLER, ACTUAL) * RollerRPMtoWheelRPM 'calculate the wheel and motor angular velocities based on roller and wheel diameters and gear ratio
                                Data(RPM1_MOTOR, ACTUAL) = Data(RPM1_WHEEL, ACTUAL) * GearRatio
                                Data(SPEED, ACTUAL) = Data(RPM1_ROLLER, ACTUAL) * RollerRadsPerSecToMetersPerSec 'calculate the speed (meters/s) based on roller rad/s
                                Data(DRAG, ACTUAL) = Data(SPEED, ACTUAL) ^ 3 * ForceAir 'calculate drag based on vehicle speed (meters/s)
                                If Data(RPM1_ROLLER, ACTUAL) > Data(RPM1_ROLLER, MAXIMUM) Then 'set the maximum values for roller, wheel, and motor RPM; Speed and Drag
                                    Data(RPM1_ROLLER, MAXIMUM) = Data(RPM1_ROLLER, ACTUAL)
                                    Data(RPM1_WHEEL, MAXIMUM) = Data(RPM1_WHEEL, ACTUAL)
                                    Data(RPM1_MOTOR, MAXIMUM) = Data(RPM1_MOTOR, ACTUAL)
                                    Data(SPEED, MAXIMUM) = Data(SPEED, ACTUAL)
                                    Data(DRAG, MAXIMUM) = Data(DRAG, ACTUAL)
                                End If
                                If Data(RPM1_ROLLER, ACTUAL) < Data(RPM1_ROLLER, MINIMUM) Then 'set the maximum values for roller, wheel, and motor RPM; Speed and Drag
                                    Data(RPM1_ROLLER, MINIMUM) = Data(RPM1_ROLLER, ACTUAL)
                                    Data(RPM1_WHEEL, MINIMUM) = Data(RPM1_WHEEL, ACTUAL)
                                    Data(RPM1_MOTOR, MINIMUM) = Data(RPM1_MOTOR, ACTUAL)
                                    Data(SPEED, MINIMUM) = Data(SPEED, ACTUAL)
                                    Data(DRAG, MINIMUM) = Data(DRAG, ACTUAL)
                                End If
                                'calculate torque based on angular acceleration (delta speed per time) and MOI
                                Data(TORQUE_ROLLER, ACTUAL) = (Data(RPM1_ROLLER, ACTUAL) - OldAngularVelocity) / (RPM1NewTriggerTime - RPM1OldTriggerTime) * DynoMomentOfInertia
                                Data(POWER, ACTUAL) = Data(TORQUE_ROLLER, ACTUAL) * Data(RPM1_ROLLER, ACTUAL) ' + OldAngularVelocity) / 2) 'calculate power based on torque and average angular velocity between two points
                                If Data(POWER, ACTUAL) > 0 Then
                                    Data(EFFICIENCY, ACTUAL) = Data(WATTS_IN, ACTUAL) / Data(POWER, ACTUAL)
                                Else
                                    Data(EFFICIENCY, ACTUAL) = 0
                                End If
                                Data(TORQUE_WHEEL, ACTUAL) = Data(POWER, ACTUAL) / Data(RPM1_WHEEL, ACTUAL) 'back calculate the torque at the wheel and motor based on the calculated power
                                Data(TORQUE_MOTOR, ACTUAL) = Data(POWER, ACTUAL) / Data(RPM1_MOTOR, ACTUAL)
                            
                                If Data(TORQUE_ROLLER, ACTUAL) > Data(TORQUE_ROLLER, MAXIMUM) Then 'set the maximum values for torque
                                    Data(TORQUE_ROLLER, MAXIMUM) = Data(TORQUE_ROLLER, ACTUAL)
                                    Data(TORQUE_WHEEL, MAXIMUM) = Data(TORQUE_WHEEL, ACTUAL)
                                    Data(TORQUE_MOTOR, MAXIMUM) = Data(TORQUE_MOTOR, ACTUAL)
                                End If
                                If Data(TORQUE_ROLLER, ACTUAL) < Data(TORQUE_ROLLER, MINIMUM) Then 'set the maximum values for torque
                                    Data(TORQUE_ROLLER, MINIMUM) = Data(TORQUE_ROLLER, ACTUAL)
                                    Data(TORQUE_WHEEL, MINIMUM) = Data(TORQUE_WHEEL, ACTUAL)
                                    Data(TORQUE_MOTOR, MINIMUM) = Data(TORQUE_MOTOR, ACTUAL)
                                End If
                                If Data(POWER, ACTUAL) > Data(POWER, MAXIMUM) Then 'set the maximum value for power
                                    Data(POWER, MAXIMUM) = Data(POWER, ACTUAL)
                                End If
                                If Data(POWER, ACTUAL) < Data(POWER, MINIMUM) Then 'set the maximum value for power
                                    Data(POWER, MINIMUM) = Data(POWER, ACTUAL)
                                End If
                                If Data(EFFICIENCY, ACTUAL) > Data(EFFICIENCY, MAXIMUM) Then 'set the maximum value for power
                                    Data(EFFICIENCY, MAXIMUM) = Data(EFFICIENCY, ACTUAL)
                                End If
                                If Data(EFFICIENCY, ACTUAL) < Data(EFFICIENCY, MINIMUM) Then 'set the maximum value for power
                                    Data(EFFICIENCY, MINIMUM) = Data(EFFICIENCY, ACTUAL)
                                End If
                                OldAngularVelocity = Data(RPM1_ROLLER, ACTUAL) 'remember the current angular velocity for next RPM reading
                                Select Case WhichDataMode
                                    Case Is = LIVE
                                        'Don't do anything.  This helps skip through the Select Case Faster
                                    Case Is = POWERRUN
                                        If Data(RPM1_ROLLER, ACTUAL) > ActualPowerRunThreshold Then
                                            DataPoints += 1
                                            If DataPoints = 1 Then
                                                TotalElapsedTime = 0
                                            Else
                                                TotalElapsedTime += (RPM1NewTriggerTime - RPM1OldTriggerTime)
                                            End If
                                            If DataPoints = MinimumPowerRunPoints Then btnStartPowerRun.BackColor = Color.Green
                                            CollectedData(SESSIONTIME, DataPoints) = TotalElapsedTime
                                            CollectedData(RPM1_ROLLER, DataPoints) = Data(RPM1_ROLLER, ACTUAL)
                                            CollectedData(RPM2, DataPoints) = Data(RPM2, ACTUAL)
                                            CollectedData(VOLTS, DataPoints) = Data(VOLTS, ACTUAL)
                                            CollectedData(AMPS, DataPoints) = Data(AMPS, ACTUAL)
                                            CollectedData(TEMPERATURE1, DataPoints) = Data(TEMPERATURE1, ACTUAL)
                                            CollectedData(TEMPERATURE2, DataPoints) = Data(TEMPERATURE2, ACTUAL)
                                            CollectedData(PIN04VALUE, DataPoints) = Data(PIN04VALUE, ACTUAL)
                                            CollectedData(PIN05VALUE, DataPoints) = Data(PIN05VALUE, ACTUAL)
                                            'DataPoints += 1
                                            If DataPoints = MAXDATAPOINTS Then
                                                DataPoints = MAXDATAPOINTS - 1
                                                btnStartPowerRun.BackColor = Color.Red
                                            End If
                                        End If

                                    Case Is = LOGRAW
                                        DataPoints += 1
                                        If DataPoints = 1 Then
                                            TotalElapsedTime = 0
                                            btnStartLoggingRaw.BackColor = Color.Green
                                        Else
                                            TotalElapsedTime += ElapsedTime
                                        End If
                                        CollectedData(SESSIONTIME, DataPoints) = TotalElapsedTime
                                        CollectedData(RPM1_ROLLER, DataPoints) = Data(RPM1_ROLLER, ACTUAL)
                                        CollectedData(RPM2, DataPoints) = Data(RPM2, ACTUAL)
                                        CollectedData(VOLTS, DataPoints) = Data(VOLTS, ACTUAL)
                                        CollectedData(AMPS, DataPoints) = Data(AMPS, ACTUAL)
                                        CollectedData(TEMPERATURE1, DataPoints) = Data(TEMPERATURE1, ACTUAL)
                                        CollectedData(TEMPERATURE2, DataPoints) = Data(TEMPERATURE2, ACTUAL)
                                        CollectedData(PIN04VALUE, DataPoints) = Data(PIN04VALUE, ACTUAL)
                                        CollectedData(PIN05VALUE, DataPoints) = Data(PIN05VALUE, ACTUAL)
                                        'DataPoints += 1
                                        If DataPoints = MAXDATAPOINTS Then
                                            DataPoints = MAXDATAPOINTS - 1
                                        End If
                                End Select
                            Else
                                If Data(SESSIONTIME, ACTUAL) - RPM1NewTriggerTime > WaitForNewSignal Then
                                    Data(RPM1_ROLLER, ACTUAL) = 0
                                    Data(RPM1_WHEEL, ACTUAL) = 0
                                    Data(RPM1_MOTOR, ACTUAL) = 0
                                    Data(SPEED, ACTUAL) = 0
                                    Data(TORQUE_ROLLER, ACTUAL) = 0
                                    Data(TORQUE_WHEEL, ACTUAL) = 0
                                    Data(TORQUE_MOTOR, ACTUAL) = 0
                                    Data(POWER, ACTUAL) = 0
                                    Data(RPM2_RATIO, ACTUAL) = 0
                                End If
                            End If
                            RPM2NewTriggerTime = CDbl(COMPortMessage(3)) / 1000000
                            RPM2ElapsedTime = CDbl(COMPortMessage(4)) / 1000000
                            If RPM2NewTriggerTime <> RPM2OldTriggerTime Then
                                Data(RPM2, ACTUAL) = ElapsedTimeToRadPerSec / RPM2ElapsedTime
                                Data(RPM2_RATIO, ACTUAL) = Data(RPM2, ACTUAL) / Data(RPM1_WHEEL, ACTUAL)
                                Data(RPM2_ROLLOUT, ACTUAL) = WheelCircumference / Data(RPM2_RATIO, ACTUAL)
                                If Data(RPM2, ACTUAL) > Data(RPM2, MAXIMUM) Then
                                    Data(RPM2, MAXIMUM) = Data(RPM2, ACTUAL)
                                End If
                                If Data(RPM2_RATIO, ACTUAL) > Data(RPM2_RATIO, MAXIMUM) Then
                                    Data(RPM2_RATIO, MAXIMUM) = Data(RPM2_RATIO, ACTUAL)
                                End If
                                If Data(RPM2_ROLLOUT, ACTUAL) > Data(RPM2_ROLLOUT, MAXIMUM) Then
                                    Data(RPM2_ROLLOUT, MAXIMUM) = Data(RPM2_ROLLOUT, ACTUAL)
                                End If
                                If Data(RPM2, ACTUAL) < Data(RPM2, MINIMUM) Then
                                    Data(RPM2, MINIMUM) = Data(RPM2, ACTUAL)
                                End If
                                If Data(RPM2_RATIO, ACTUAL) < Data(RPM2_RATIO, MINIMUM) Then
                                    Data(RPM2_RATIO, MINIMUM) = Data(RPM2_RATIO, ACTUAL)
                                End If
                                If Data(RPM2_ROLLOUT, ACTUAL) < Data(RPM2_ROLLOUT, MINIMUM) Then
                                    Data(RPM2_ROLLOUT, MINIMUM) = Data(RPM2_ROLLOUT, ACTUAL)
                                End If
                            Else
                                If Data(SESSIONTIME, ACTUAL) - RPM2NewTriggerTime > WaitForNewSignal Then
                                    Data(RPM2, ACTUAL) = 0
                                    Data(RPM2_RATIO, ACTUAL) = 0
                                    Data(RPM2_ROLLOUT, ACTUAL) = 0
                                End If
                            End If
                        End If

                        'If Not WavesStarted Then

                        'End If


                        OldSessionTime = Data(SESSIONTIME, ACTUAL)
                        RPM1OldTriggerTime = RPM1NewTriggerTime
                        RPM2OldTriggerTime = RPM2NewTriggerTime

                    End SyncLock

                    If WhichDataMode = POWERRUN AndAlso DataPoints > MinimumPowerRunPoints AndAlso Data(RPM1_ROLLER, ACTUAL) <= ActualPowerRunThreshold Then
                        SetControlBackColor_ThreadSafe(btnStartPowerRun, System.Windows.Forms.Control.DefaultBackColor)
                        'DataPoints -= 1
                        PauseForms()
                        WhichDataMode = LIVE
                    End If

                    SetControlText_Threadsafe(frmCOM.lblCurrentVolts, NewCustomFormat(Data(VOLTS, ACTUAL)))
                    SetControlText_Threadsafe(frmCOM.lblCurrentAmps, NewCustomFormat(Data(AMPS, ACTUAL)))
                    SetControlText_Threadsafe(frmCOM.lblCurrentTemperature1, NewCustomFormat(Data(TEMPERATURE1, ACTUAL)))
                    SetControlText_Threadsafe(frmCOM.lblCurrentTemperature2, NewCustomFormat(Data(TEMPERATURE2, ACTUAL)))
                    SetControlText_Threadsafe(frmCOM.lblCurrentPinA4, NewCustomFormat(Data(PIN04VALUE, ACTUAL)))
                    SetControlText_Threadsafe(frmCOM.lblCurrentPinA5, NewCustomFormat(Data(PIN05VALUE, ACTUAL)))

                End If
            Catch ex As Exception
                btnHide_Click(Me, EventArgs.Empty)
                MsgBox("Serial Port Data Received Error: " & ex.ToString, MsgBoxStyle.Exclamation)
                'btnShow_Click(Me, EventArgs.Empty)
                End
            End Try
        End If
    End Sub
#End Region
#Region "New Interface Code"
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        btnHide_Click(Me, EventArgs.Empty)
        With OpenFileDialog1
            .Reset()
            .Filter = "Interface files (*.sdi)|*.sdi"
            .ShowDialog()
        End With

        'templine dfor no reason
        If OpenFileDialog1.FileName <> "" Then
            btnClose_Click(Me, EventArgs.Empty)
            txtInterface.Text = OpenFileDialog1.FileName
            LoadInterface()
        Else
            Select Case txtInterface.Text
                Case Is = "No Interface Loaded"
                    btnSave.Enabled = False
                    btnSaveAs.Enabled = False
                    btnClose.Enabled = False
                    btnHide.Enabled = False
                    btnShow.Enabled = False
                Case Else
                    btnShow_Click(Me, EventArgs.Empty)
                    btnSave.Enabled = True
                    btnSaveAs.Enabled = True
                    btnClose.Enabled = True
                    btnHide.Enabled = True
                    btnShow.Enabled = False
            End Select
           
        End If
    End Sub
    Private Sub LoadInterface()
        If txtInterface.Text <> "No Interface Loaded" Then
            Dim TempString As String
            Dim InterfaceInputFile As New System.IO.StreamReader(txtInterface.Text)
            TempString = InterfaceInputFile.ReadLine
            Select Case TempString
                Case Is = InterfaceVersion, "SimpleDyno_Interface_6_4"
                    Do Until InterfaceInputFile.EndOfStream
                        TempString = InterfaceInputFile.ReadLine
                        If TempString = "Label" Then
                            TempString = InterfaceInputFile.ReadLine
                            f.Add(New SimpleDynoSubLabel())
                            AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
                            AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
                            f(f.Count - 1).myType = "Label"
                            f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
                            f.Item(f.Count - 1).CreateFromSerializedData(TempString)
                        ElseIf TempString = "Gauge" Then
                            TempString = InterfaceInputFile.ReadLine
                            f.Add(New SimpleDynoSubGauge())
                            AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
                            AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
                            f(f.Count - 1).myType = "Gauge"
                            f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
                            f.Item(f.Count - 1).CreateFromSerializedData(TempString)
                        ElseIf TempString = "MultiYTimeGraph" Then
                            TempString = InterfaceInputFile.ReadLine
                            f.Add(New SimpleDynoSubMultiYTimeGraph())
                            AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
                            AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
                            f(f.Count - 1).myType = "MultiYTimeGraph"
                            f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
                            f.Item(f.Count - 1).CreateFromSerializedData(TempString)
                        End If
                    Loop
                    btnSave.Enabled = True
                    btnSaveAs.Enabled = True
                    btnClose.Enabled = True
                    btnHide.Enabled = True
                    btnShow.Enabled = False
                    InterfaceInputFile.Close()
                    InterfaceInputFile.Dispose()
                Case Is = "SimpleDyno_Interface_6_3"
                    Do Until InterfaceInputFile.EndOfStream
                        TempString = InterfaceInputFile.ReadLine
                        If TempString = "Label" Then
                            TempString = InterfaceInputFile.ReadLine
                            f.Add(New SimpleDynoSubLabel())
                            AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
                            AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
                            f(f.Count - 1).myType = "Label"
                            f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
                            'This is the point to translate old pointers to new pointers

                            f.Item(f.Count - 1).CreateFromSerializedData(InterfaceConvert_63_toCurrent(TempString))
                        ElseIf TempString = "Gauge" Then
                            TempString = InterfaceInputFile.ReadLine
                            Debug.Print(TempString)
                            f.Add(New SimpleDynoSubGauge())
                            AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
                            AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
                            f(f.Count - 1).myType = "Gauge"
                            f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
                            f.Item(f.Count - 1).CreateFromSerializedData(InterfaceConvert_63_toCurrent(TempString))
                        ElseIf TempString = "MultiYTimeGraph" Then
                            TempString = InterfaceInputFile.ReadLine
                            f.Add(New SimpleDynoSubMultiYTimeGraph())
                            AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
                            AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
                            f(f.Count - 1).myType = "MultiYTimeGraph"
                            f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
                            f.Item(f.Count - 1).CreateFromSerializedData(InterfaceConvert_63_toCurrent(TempString))
                        End If
                    Loop
                    btnSave.Enabled = True
                    btnSaveAs.Enabled = True
                    btnClose.Enabled = True
                    btnHide.Enabled = True
                    btnShow.Enabled = False
                    InterfaceInputFile.Close()
                    InterfaceInputFile.Dispose()
                    btnSave_Click(Me, EventArgs.Empty) ' This added here to make sure any version changes are saved
                Case Else
                    btnHide_Click(Me, EventArgs.Empty)
                    MsgBox("Not a valid Interface File", vbOKOnly)
                    btnShow_Click(Me, EventArgs.Empty)
                    InterfaceInputFile.Close()
                    InterfaceInputFile.Dispose()
            End Select
           
            


        End If
    End Sub
    Private Function InterfaceConvert_63_toCurrent(ByVal Sent As String) As String
        'Receives the 6.3 version of the string and updates the x and y pointers to the new ones
        'Because the only x pointer in 6.3 was time, there is no need to convert this one.
        Dim TempSplit() As String = Split(Sent, "_")
        Dim TempYNumberAllowed As Integer = CInt(TempSplit(5))
        For Count As Integer = 1 To TempYNumberAllowed
            Select Case CInt(TempSplit(16 + (Count - 1) * 7))
                Case Is = 21, 22, 23
                    'CHAN1_FREQUENCY = 21   'CHAN1_FREQUENCY = 4
                    'CHAN1_PULSEWIDTH = 22  'CHAN1_PULSEWIDTH = 5
                    'CHAN1_DUTYCYCLE = 23   'CHAN1_DUTYCYCLE = 6
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) - 17).ToString
                Case Is = 4, 5, 6, 7
                    'SPEED = 4              'SPEED = 7
                    'RPM2 = 5               'RPM2 = 8
                    'RPM2_RATIO = 6         'RPM2_RATIO = 9
                    'RPM2_ROLLOUT = 7       'RPM2_ROLLOUT = 10
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) + 3).ToString
                Case Is = 24, 25, 26
                    'CHAN2_FREQUENCY = 24   'CHAN2_FREQUENCY = 11
                    'CHAN2_PULSEWIDTH = 25  'CHAN2_PULSEWIDTH = 12
                    'CHAN2_DUTYCYCLE = 26   'CHAN2_DUTYCYCLE = 13
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) - 13).ToString
                Case Is = 8, 9, 10
                    'TORQUE_ROLLER = 8      'TORQUE_ROLLER = 14
                    'TORQUE_WHEEL = 9       'TORQUE_WHEEL = 15
                    'TORQUE_MOTOR = 10      'TORQUE_MOTOR = 16
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) + 6).ToString
                Case Is = 11
                    'POWER = 11             'POWER = 21
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) + 10).ToString
                Case Is = 12, 13, 14, 15, 16, 17, 18, 19, 20
                    'DRAG = 12              'DRAG = 26
                    'VOLTS = 13             'VOLTS = 27
                    'AMPS = 14              'AMPS = 28
                    'WATTS_IN = 15          'WATTS_IN = 29
                    'EFFICIENCY = 16        'EFFICIENCY = 30
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) + 14).ToString
                Case Is = 17, 18, 19, 20
                    'TEMPERATURE1 = 17      'TEMPERATURE1 = 32
                    'TEMPERATURE2 = 18      'TEMPERATURE2 = 33
                    'PIN04VALUE = 19        'PIN04VALUE = 34
                    'PIN05VALUE = 20        'PIN05VALUE = 35
                    TempSplit(16 + (Count - 1) * 7) = (CInt(TempSplit(16 + (Count - 1) * 7)) + 15).ToString
            End Select
        Next

        'Now Rebuild the string
        Dim TempReply As String = ""
        For Count As Integer = 0 To UBound(TempSplit)
            TempReply = TempReply & TempSplit(Count) & "_"
        Next
        InterfaceConvert_63_toCurrent = TempReply

    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim TempString As String = InterfaceVersion & vbCrLf
        Dim frmcount As Integer
        For Each SDFrm As SimpleDynoSubForm In f
            If SDFrm.Visible <> False Then
                frmcount += 1
                If SDFrm.myType = "Label" Then TempString = TempString & "Label" & vbCrLf
                If SDFrm.myType = "Gauge" Then TempString = TempString & "Gauge" & vbCrLf
                If SDFrm.myType = "MultiYTimeGraph" Then TempString = TempString & "MultiYTimeGraph" & vbCrLf
                TempString = TempString & SDFrm.ReportForSerialization & vbCrLf
            End If
        Next
        Dim InterfaceOutPutFile As New System.IO.StreamWriter(txtInterface.Text)
        InterfaceOutPutFile.WriteLine(TempString)
        InterfaceOutPutFile.Close()
        InterfaceOutPutFile.Dispose()
    End Sub
    Private Sub btnSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAs.Click
        Dim TempString As String = InterfaceVersion & vbCrLf
        Dim frmcount As Integer
        For Each SDFrm As SimpleDynoSubForm In f
            If SDFrm.Visible <> False Then
                frmcount += 1
                If SDFrm.myType = "Label" Then TempString = TempString & "Label" & vbCrLf
                If SDFrm.myType = "Gauge" Then TempString = TempString & "Gauge" & vbCrLf
                If SDFrm.myType = "MultiYTimeGraph" Then TempString = TempString & "MultiYTimeGraph" & vbCrLf
                TempString = TempString & SDFrm.ReportForSerialization & vbCrLf
            End If
        Next
        btnHide_Click(Me, EventArgs.Empty)
        With SaveFileDialog1
            .Reset()
            .Filter = "Text files (*.sdi)|*.sdi"
            .ShowDialog()
            If .FileName <> "" Then
                txtInterface.Text = .FileName
                Dim InterfaceOutPutFile As New System.IO.StreamWriter(txtInterface.Text)
                InterfaceOutPutFile.WriteLine(TempString)
                InterfaceOutPutFile.Close()
                InterfaceOutPutFile.Dispose()
            End If
        End With
        btnShow_Click(Me, EventArgs.Empty)
    End Sub
    Friend Sub btnHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHide.Click
        For Each SDFrm As SimpleDynoSubForm In f
            SDFrm.HideYourSelf()
        Next
        btnShow.Enabled = True
        btnHide.Enabled = False
    End Sub
    Friend Sub btnShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click
        For Each SDFrm As SimpleDynoSubForm In f
            SDFrm.ShowYourSelf()
        Next
        btnShow.Enabled = False
        btnHide.Enabled = True
    End Sub
    Friend Sub PauseForms()
        For Each SDFrm As SimpleDynoSubForm In f
            SDFrm.Pause()
        Next
    End Sub
    Friend Sub RestartForms()
        For Each SDFrm As SimpleDynoSubForm In f
            SDFrm.Restart()
        Next
    End Sub
    Friend Sub ResetAllYTimeGraphs()
        For Each SDFrm As SimpleDynoSubForm In f
            If SDFrm.myType = "MultiYTimeGraph" Then
                SDFrm.ResetSDForm()
            End If
        Next
    End Sub
    Private Sub btnNewLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewLabel.Click
        'New Label
        btnShow_Click(Me, EventArgs.Empty)
        btnSaveAs.Enabled = True
        btnClose.Enabled = True
        btnHide.Enabled = True
        f.Add(New SimpleDynoSubLabel())
        AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
        AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
        f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
    End Sub
    Private Sub btnNewGauge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewGauge.Click
        'New Gauge
        btnShow_Click(Me, EventArgs.Empty)
        btnSaveAs.Enabled = True
        btnClose.Enabled = True
        btnHide.Enabled = True
        f.Add(New SimpleDynoSubGauge())
        AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
        AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
        f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
    End Sub
    Private Sub btnMultiYTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiYTime.Click
        btnShow_Click(Me, EventArgs.Empty)
        btnSaveAs.Enabled = True
        btnClose.Enabled = True
        btnHide.Enabled = True
        f.Add(New SimpleDynoSubMultiYTimeGraph())
        AddHandler f(f.Count - 1).RemoveYourself, AddressOf RemoveForm
        AddHandler f(f.Count - 1).SetToMyFormat, AddressOf SetAllFormats
        f(f.Count - 1).Initialize(f.Count - 1, Data, DataTags, DataUnits, DataUnitTags, DataAreUsed)
    End Sub
    Private Sub btnResetSDForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ResetAllYTimeGraphs()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Remove interface if it is closed down
        Do Until f.Count = 0
            f.Item(f.Count - 1).Dispose()
            f.Remove(f.Item(f.Count - 1))
        Loop
        txtInterface.Text = "No Interface Loaded"
        btnClose.Enabled = False
        btnShow.Enabled = False
        btnHide.Enabled = False
        btnSave.Enabled = False
        btnSaveAs.Enabled = False
    End Sub
    Private Sub RemoveForm(ByVal SentToRemove As Integer)
        For Each SDFrm As SimpleDynoSubForm In f
            If SDFrm.myNumber = SentToRemove Then
                f.Remove(SDFrm)
                SDFrm.Dispose()
                SDFrm = Nothing
                Exit For
            End If
        Next
    End Sub
    Private Sub SetAllFormats(ByVal SentFormat As String)
        For Each SDFrm As SimpleDynoSubForm In f
            SDFrm.SetMyFormat(SentFormat)
        Next
    End Sub
#End Region
#Region "Performance Testing"
#If QueryPerformance Then
    Private Sub btnPerformanceTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPerformanceTest.Click
        'Cycle through all listed buffer sizes, sampling rates, channel numbers and +/- adv processing
        'Collect approx 100 points per
        'Calculate average, stdev, min, max, based on middle 60 points.  Also count P_Time events > 100 
        'First, ADV off
        Dim SumPerf As Double, SumFreq As Double
        Dim AvePerf As Double, AveFreq As Double
        Dim StdPerf As Double, StdFreq As Double
        Dim MinPerf As Double, MinFreq As Double
        Dim MaxPerf As Double, MaxFreq As Double
        Dim PerfGreaterThan100 As Double
        Dim i As Integer, j As Integer, k As Integer, t As Integer
        ReDim PerformanceData(2, 200)
        chkAdvancedProcessing.Checked = False
        For i = 0 To cmbChannels.Items.Count - 1
            cmbChannels.SelectedIndex = i
            For j = 0 To cmbBufferSize.Items.Count - 1
                cmbBufferSize.SelectedIndex = j
                For k = 0 To cmbSampleRate.Items.Count - 1
                    cmbSampleRate.SelectedIndex = k
                    btnStartAcquisition_Click(Me, EventArgs.Empty)
                    Do
                        Application.DoEvents()
                    Loop Until PerfBufferCount >= 100
                    SumPerf = 0 : SumFreq = 0
                    MinPerf = 100000 : MinFreq = 100000
                    MaxPerf = 0 : MaxFreq = 0
                    PerfGreaterThan100 = 0
                    AvePerf = 0 : AveFreq = 0
                    StdPerf = 0 : StdFreq = 0
                    For t = 21 To 80
                        SumPerf = SumPerf + PerformanceData(P_TIME, t)
                        SumFreq = SumFreq + PerformanceData(P_FREQ, t)
                        If PerformanceData(P_TIME, t) < MinPerf Then MinPerf = PerformanceData(P_TIME, t)
                        If PerformanceData(P_TIME, t) > MaxPerf Then MaxPerf = PerformanceData(P_TIME, t)
                        If PerformanceData(P_FREQ, t) < MinFreq Then MinFreq = PerformanceData(P_FREQ, t)
                        If PerformanceData(P_FREQ, t) > MaxFreq Then MaxFreq = PerformanceData(P_FREQ, t)
                        If PerformanceData(P_TIME, t) > 100 Then PerfGreaterThan100 += 1
                        'Debug.Print(PerformanceData(P_FREQ, t).ToString & " " & PerformanceData(P_TIME, t).ToString)
                    Next
                    AvePerf = SumPerf / 60
                    AveFreq = SumFreq / 60
                    SumPerf = 0 : SumFreq = 0
                    For t = 21 To 80
                        SumPerf = SumPerf + (PerformanceData(P_TIME, t) - AvePerf) ^ 2
                        SumFreq = SumFreq + (PerformanceData(P_FREQ, t) - AveFreq) ^ 2
                    Next
                    StdPerf = (SumPerf / 59) ^ 0.5
                    StdFreq = (SumFreq / 59) ^ 0.5
                    Debug.Print("Adv" & chkAdvancedProcessing.CheckState.ToString & " " & NUMBER_OF_CHANNELS & " " & BUFFER_SIZE & " " & SAMPLE_RATE & " Freq (min,max,ave,std): " & MinFreq & " " & MaxFreq & " " & AveFreq & " " & StdFreq & " Perf (min,max,ave,std,#>100): " & MinPerf & " " & MaxPerf & " " & AvePerf & " " & StdPerf & " " & PerfGreaterThan100)
                    PerfBufferCount = 0
                Next
            Next
        Next
        chkAdvancedProcessing.Checked = True
        For i = 0 To cmbChannels.Items.Count - 1
            cmbChannels.SelectedIndex = i
            For j = 0 To cmbBufferSize.Items.Count - 1
                cmbBufferSize.SelectedIndex = j
                For k = 0 To cmbSampleRate.Items.Count - 1
                    cmbSampleRate.SelectedIndex = k
                    btnStartAcquisition_Click(Me, EventArgs.Empty)
                    Do
                        Application.DoEvents()
                    Loop Until PerfBufferCount >= 100
                    SumPerf = 0 : SumFreq = 0
                    MinPerf = 100000 : MinFreq = 100000
                    MaxPerf = 0 : MaxFreq = 0
                    PerfGreaterThan100 = 0
                    AvePerf = 0 : AveFreq = 0
                    StdPerf = 0 : StdFreq = 0
                    For t = 21 To 80
                        SumPerf = SumPerf + PerformanceData(P_TIME, t)
                        SumFreq = SumFreq + PerformanceData(P_FREQ, t)
                        If PerformanceData(P_TIME, t) < MinPerf Then MinPerf = PerformanceData(P_TIME, t)
                        If PerformanceData(P_TIME, t) > MaxPerf Then MaxPerf = PerformanceData(P_TIME, t)
                        If PerformanceData(P_FREQ, t) < MinFreq Then MinFreq = PerformanceData(P_FREQ, t)
                        If PerformanceData(P_FREQ, t) > MaxFreq Then MaxFreq = PerformanceData(P_FREQ, t)
                        If PerformanceData(P_TIME, t) > 100 Then PerfGreaterThan100 += 1
                        'Debug.Print(PerformanceData(P_FREQ, t).ToString & " " & PerformanceData(P_TIME, t).ToString)
                    Next
                    AvePerf = SumPerf / 60
                    AveFreq = SumFreq / 60
                    SumPerf = 0 : SumFreq = 0
                    For t = 21 To 80
                        SumPerf = SumPerf + (PerformanceData(P_TIME, t) - AvePerf) ^ 2
                        SumFreq = SumFreq + (PerformanceData(P_FREQ, t) - AveFreq) ^ 2
                    Next
                    StdPerf = (SumPerf / 59) ^ 0.5
                    StdFreq = (SumFreq / 59) ^ 0.5
                    Debug.Print("Adv" & chkAdvancedProcessing.CheckState.ToString & " " & NUMBER_OF_CHANNELS & " " & BUFFER_SIZE & " " & SAMPLE_RATE & " Freq (min,max,ave,std): " & MinFreq & " " & MaxFreq & " " & AveFreq & " " & StdFreq & " Perf (min,max,ave,std,#>100): " & MinPerf & " " & MaxPerf & " " & AvePerf & " " & StdPerf & " " & PerfGreaterThan100)
                    PerfBufferCount = 0
                Next
            Next
        Next
    End Sub
#End If
#End Region

   
End Class
#Region "DoubleBufferPanel Class"
Public Class DoubleBufferPanel
    Inherits Panel
    Public Sub New()
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        UpdateStyles()
    End Sub
End Class
#End Region 

