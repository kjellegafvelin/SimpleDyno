Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

'Friend Enum SerialParameters
'    'Enumerate the basic components of each display object 
'    Left = 0
'    Top
'    Height
'    Width
'    Configuration
'    Y_Number
'    Timer
'    LastMember
'End Enum
Public MustInherit Class SimpleDynoSubForm
    Inherits Form
    Private context As BufferedGraphicsContext

    Friend Resizing As Boolean = False

    Private MouseDownPosition As System.Drawing.Point
    Private IsLeft As Boolean = False

    Friend myType As String = "SD Form"
    Friend myConfiguration As String
    Friend myNumber As Integer

    Private AllowedCharacters As String = "-0123456789" & Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
    Friend Splitter As String = "_"
    Friend myMinCurMax As String() = {"Minimum", "Actual", "Maximum"}
    Friend myMinCurMaxAbb As String() = {"Min", "", "Max"}
    Friend Const GoldenRatio As Single = 1.61803398875

    Friend Grafx As BufferedGraphics
    Friend Contextmnu As ContextMenuStrip
    Friend Colormnu As New ColorDialog
    Friend Fontmnu As New FontDialog

    Friend BackClr As Color
    Friend AxisClr As Color
    Friend Y_DataClr() As Color

    Friend AxisBrush As SolidBrush
    Friend Y_DataBrush() As SolidBrush

    Friend AxisPen As Pen
    Friend Y_DataPen() As Pen

    Friend X_AxisFont As Font
    Friend Y_AxisFont As Font
    Friend Y_DataFont() As Font

    Friend timer1 As System.Windows.Forms.Timer
    Friend TimerTiggered As Boolean
    
    Friend Y_Number_Allowed As Integer
    Friend XY_Selected As Integer

    Friend X_PrimaryPointer As Integer
    Friend X_MinCurMaxPointer As Integer
    Friend X_UnitPointer As Integer
    Friend X_Minimum As Double
    Friend X_Maximum As Double
    Friend X_Result As Double
    Friend X_PrimaryLabel As String
    Friend X_UnitsLabel As String
    Friend X_MinMaxCurLabel As String

    Friend Y_PrimaryPointer() As Integer
    Friend Y_MinCurMaxPointer() As Integer
    Friend Y_UnitPointer() As Integer
    Friend Y_Minimum() As Double
    Friend Y_Maximum() As Double
    Friend Y_Result() As Double
    Friend Y_PrimaryLabel() As String
    Friend Y_UnitsLabel() As String
    Friend Y_MinMaxCurLabel() As String
    Friend IsThisYSelected(4) As Boolean

    Private Const GridSnap As Integer = 10

    Friend CopyOfData(,) As Double
    Friend CopyOfUnits(,) As Double
    Friend CopyOfDataNames() As String
    Friend CopyOfUnitsNames() As String
    Friend CopyofDataAreUsed() As Boolean

    Public Shared Event RemoveYourself(ByVal WhichNumberAmI As Integer)
    Public Shared Event SetToMyFormat(ByVal MyFormat As String)


    MustOverride Sub ControlSpecificResize()
    MustOverride Sub DrawToBuffer(ByVal g As Graphics)
    MustOverride Sub AddControlSpecificOptionItems()
    MustOverride Sub ControlSpecificInitialization()
    MustOverride Sub ControlSpecificOptionSelection(ByVal Sent As String)
    MustOverride Function ControlSpecificSerializationData() As String
    MustOverride Sub ControlSpecficCreateFromSerializedData(ByVal Sent As String())

    
    Public Sub New()
        'Nothing Here
    End Sub
    Private Sub InitializeComponent()
        'This code seems to be inserted by the designer - can be removed
    End Sub
    Friend Sub Initialize(ByVal WhichNumberAmI As Integer, ByRef SentData(,) As Double, ByRef SentDataNames() As String, ByRef SentUnits(,) As Double, ByRef SentUnitsNames() As String, ByRef SentDataAreUsed() As Boolean) ' ADD CONSTRUCTION CALLS HERE)

        Me.ClientSize = New System.Drawing.Size(200, 200)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.ControlBox = False
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        Me.DoubleBuffered = True
        Me.TopMost = True

        myNumber = WhichNumberAmI
        CopyOfData = SentData
        CopyOfUnits = SentUnits
        CopyOfDataNames = SentDataNames
        CopyOfUnitsNames = SentUnitsNames
        CopyofDataAreUsed = SentDataAreUsed

        ControlSpecificInitialization()

        ReDim Y_PrimaryPointer(Y_Number_Allowed)
        ReDim Y_MinCurMaxPointer(Y_Number_Allowed)
        ReDim Y_UnitPointer(Y_Number_Allowed)
        ReDim Y_Minimum(Y_Number_Allowed)
        ReDim Y_Maximum(Y_Number_Allowed)
        ReDim Y_Result(Y_Number_Allowed)

        BackClr = Color.White
        AxisClr = Color.Black
        AxisBrush = New SolidBrush(AxisClr)
        AxisPen = New Pen(AxisClr, 2)
        X_AxisFont = New Font("Arial", 5)
        Y_AxisFont = New Font("Arial", 5)

        ReDim Y_DataClr(Y_Number_Allowed)
        ReDim Y_DataBrush(Y_Number_Allowed)
        ReDim Y_DataPen(Y_Number_Allowed)
        ReDim Y_DataFont(Y_Number_Allowed)
        ReDim Y_PrimaryLabel(Y_Number_Allowed)
        ReDim Y_UnitsLabel(Y_Number_Allowed)
        ReDim Y_MinMaxCurLabel(Y_Number_Allowed)

        For Count As Integer = 1 To Y_Number_Allowed
            Y_DataClr(Count) = Color.Green
            Y_DataBrush(Count) = New SolidBrush(Y_DataClr(Count))
            Y_DataPen(Count) = New Pen(Y_DataClr(Count))
            Y_DataFont(Count) = New Font("Arial", 5)
            Y_PrimaryLabel(Count) = "Parameter"
            Y_UnitsLabel(Count) = "Unit"
            Y_MinMaxCurLabel(Count) = "Act"
            Y_Maximum(Count) = 1000
            Y_Minimum(Count) = 0
        Next

        timer1 = New Timer
        timer1.Interval = 1000

        AddHandler Me.timer1.Tick, AddressOf Me.SDForm_TimerTick
        AddHandler Me.MouseDown, AddressOf Me.SDForm_MouseDown
        AddHandler Me.MouseMove, AddressOf Me.SDForm_MouseMove
        AddHandler Me.Resize, AddressOf Me.SDForm_Resize
        AddHandler Me.Move, AddressOf Me.SDForm_Move

        CreateTheMenu()

        context = BufferedGraphicsManager.Current

        SDForm_Resize(Me, System.EventArgs.Empty)

        timer1.Start()

        Me.Show()

    End Sub
    Public Function ReportForSerialization() As String
        Dim TempReport As String

        With Me
            TempReport = .Location.X & Splitter & .Location.Y & Splitter & .Height & Splitter & .Width & Splitter 'dimensions and posisition of window
            TempReport += myConfiguration & Splitter & Y_Number_Allowed & Splitter & timer1.Interval & Splitter   'specific configuration and number of Y_datasets
            TempReport += ColorTranslator.ToHtml(BackClr) & Splitter & ColorTranslator.ToHtml(AxisClr) & Splitter 'basic colors
            TempReport += X_PrimaryPointer & Splitter & X_MinCurMaxPointer & Splitter & X_UnitPointer & Splitter  'X data pointer
            TempReport += X_Minimum & Splitter & X_Maximum & Splitter                                             'X min and max
            TempReport += X_AxisFont.Name & Splitter
            For Count As Integer = 1 To Y_Number_Allowed
                TempReport += ColorTranslator.ToHtml(Y_DataClr(Count)) & Splitter
                TempReport += Y_PrimaryPointer(Count) & Splitter
                TempReport += Y_MinCurMaxPointer(Count) & Splitter
                TempReport += Y_UnitPointer(Count) & Splitter 'y data pointers
                TempReport += Y_Minimum(Count) & Splitter
                TempReport += Y_Maximum(Count) & Splitter
                TempReport += IsThisYSelected(Count) & Splitter
            Next
           
        End With
        TempReport = TempReport & ControlSpecificSerializationData()
        Return TempReport
    End Function
    Public Sub CreateFromSerializedData(ByRef SentSerialInformation As String)

        Dim Parameters As String()
        Dim NewLocation As System.Drawing.Point
        Dim NewSize As System.Drawing.Size

        Parameters = Split(SentSerialInformation, Splitter)

        NewLocation.X = CInt(Parameters(0)) : NewLocation.Y = CInt(Parameters(1)) : NewSize.Height = CInt(Parameters(2)) : NewSize.Width = CInt(Parameters(3)) 'position and size
        myConfiguration = Parameters(4) : Y_Number_Allowed = CInt(Parameters(5)) : timer1.Interval = CInt(Parameters(6)) 'basic configuration
        BackClr = ColorTranslator.FromHtml(Parameters(7)) : AxisClr = ColorTranslator.FromHtml(Parameters(8)) 'basic colors
        AxisBrush.Color = AxisClr : AxisPen.Color = AxisClr
        X_PrimaryPointer = CInt(Parameters(9)) : X_MinCurMaxPointer = CInt(Parameters(10)) : X_UnitPointer = CInt(Parameters(11))  'X data pointer
        X_PrimaryLabel = CopyOfDataNames(X_PrimaryPointer)
        X_UnitsLabel = Split(CopyOfUnitsNames(X_PrimaryPointer))(X_UnitPointer)
        X_MinMaxCurLabel = myMinCurMax(X_MinCurMaxPointer)

        X_Minimum = CDbl(Parameters(12)) : X_Maximum = CDbl(Parameters(13)) 'X min and max
        X_AxisFont = New Font(Parameters(14), 5)
        Y_AxisFont = New Font(Parameters(14), 5)


        For Count As Integer = 1 To Y_Number_Allowed
            Y_DataClr(Count) = ColorTranslator.FromHtml(Parameters(15 + (Count - 1) * 7))
            Y_DataBrush(Count).Color = Y_DataClr(Count) : Y_DataPen(Count).Color = Y_DataClr(Count)
            Y_PrimaryPointer(Count) = CInt(Parameters(16 + (Count - 1) * 7))
            Y_PrimaryLabel(Count) = CopyOfDataNames(Y_PrimaryPointer(Count))
            Y_MinCurMaxPointer(Count) = CInt(Parameters(17 + (Count - 1) * 7))
            Y_MinMaxCurLabel(Count) = myMinCurMax(Y_MinCurMaxPointer(Count))
            Y_UnitPointer(Count) = CInt(Parameters(18 + (Count - 1) * 7))
            Y_UnitsLabel(Count) = Split(CopyOfUnitsNames(Y_PrimaryPointer(Count)))(Y_UnitPointer(Count))
            Y_Minimum(Count) = CDbl(Parameters(19 + (Count - 1) * 7))
            Y_Maximum(Count) = CDbl(Parameters(20 + (Count - 1) * 7))
            IsThisYSelected(Count) = CBool(Parameters(21 + (Count - 1) * 7))
            Y_DataFont(Count) = New Font(Parameters(14), 5)
        Next
      
        Me.Location = NewLocation
        Me.Size = NewSize

        ControlSpecficCreateFromSerializedData(Parameters)

        SDForm_Resize(Me, System.EventArgs.Empty)

        DrawToBuffer(grafx.Graphics)
        Me.Refresh()
    End Sub
    Public Sub HideYourSelf()
        'Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
    End Sub
    Public Sub ShowYourSelf()
        'Me.WindowState = FormWindowState.Normal
        Me.Visible = True
    End Sub
    Public Sub Pause()
        Me.timer1.Stop()
    End Sub
    Public Sub Restart()
        Me.timer1.Start()
    End Sub
    Sub CreateTheMenu()

        Dim TestStrip As ToolStripMenuItem

        Dim str1 As String
        Dim str2 As String()
        Dim str3 As String()
        Dim Count As Integer

        TestStrip = New ToolStripMenuItem

        Contextmnu = New ContextMenuStrip

        For Count = 0 To UBound(CopyOfDataNames) ' - 1 'Beta - to allow session time to be sent but not displayed
            If CopyofDataAreUsed(Count) Then
                str1 = CopyOfDataNames(Count)
                str2 = myMinCurMax ' {"Min", "Current", "Max"}
                str3 = Split(CopyOfUnitsNames(Count), " ")
                TestStrip = CreateAToolStripMenuItem(Count.ToString, str1, str2, str3)
                Contextmnu.Items.Add(TestStrip)
            End If

        Next

        Contextmnu.Items.Add("-")

        '************************************************
        'Next up are the options specific to the control such as horiz/vertical for label, narrow/flat/wide for gauge

        AddControlSpecificOptionItems()

        '***********************************************
        'Color options are no longer control specific - add standardized form here

        TestStrip = New ToolStripMenuItem
        str1 = "Set Colors"
        str2 = {"Background", "Axes", "Data", "Apply to All"}
        str3 = {}

        TestStrip = CreateAToolStripMenuItem("C", str1, str2, str3) ', str4, str5)
        Contextmnu.Items.Add(TestStrip)

        '***********************************************
        'Font menu for the form
        str1 = "Set Font"
        str2 = {}
        str3 = {}
        TestStrip = CreateAToolStripMenuItem("F", str1, str2, str3) ', str4, str5)
        Contextmnu.Items.Add(TestStrip)
        '***********************************************
        'Refresh rates are common for all controls

        str1 = "Refresh rate"
        str2 = {"10 msec", "30 msec", "100 msec", "300 msec", "1000 msec"}
        str3 = {}
        TestStrip = CreateAToolStripMenuItem("R", str1, str2, str3)
        Contextmnu.Items.Add(TestStrip)
    End Sub
    Friend Function CreateAToolStripMenuItem(ByVal StartTag As String, ByVal Level1 As String, ByVal Level2 As String(), ByVal Level3 As String()) As ToolStripMenuItem

        Dim Level1Menu As ToolStripMenuItem
        Dim Level2Menu(UBound(Level2)) As ToolStripMenuItem
        Dim Level3Menu(UBound(Level3)) As ToolStripMenuItem


        Dim Level2Count As Integer, Level3Count As Integer

        Dim Remove As EventHandler = AddressOf ClickTheMenu
        Dim Check As KeyPressEventHandler = AddressOf CheckText

        Level1Menu = New ToolStripMenuItem
        With Level1Menu
            .Tag = CStr(StartTag)
            .Text = Level1
            AddHandler .Click, Remove
            '.CheckOnClick = True
            For Level2Count = 0 To UBound(Level2)
                Level2Menu(Level2Count) = New ToolStripMenuItem
                With Level2Menu(Level2Count)
                    .Tag = CStr(StartTag) & "_" & CStr(Level2Count)
                    .Text = Level2(Level2Count)
                    RemoveHandler Level1Menu.Click, Remove
                    AddHandler .Click, Remove
                    'Level1Menu.CheckOnClick = False
                    '.CheckOnClick = True
                    For Level3Count = 0 To UBound(Level3)
                        Level3Menu(Level3Count) = New ToolStripMenuItem
                        If Level3(Level3Count) <> "TXT" Then
                            With Level3Menu(Level3Count)
                                .Tag = CStr(StartTag) & "_" & CStr(Level2Count) & "_" & CStr(Level3Count)
                                .Text = Level3(Level3Count)
                                AddHandler .Click, Remove
                                ' Level1Menu.CheckOnClick = False
                                '.CheckOnClick = True
                            End With
                            .DropDownItems.Add(Level3Menu(Level3Count))
                        Else
                            Dim t As New ToolStripTextBox
                            t.Tag = CStr(StartTag) & "_" & CStr(Level2Count) & "_" & CStr(Level3Count)
                            AddHandler t.KeyPress, Check
                            .DropDownItems.Add(t)
                        End If
                        RemoveHandler Level2Menu(Level2Count).Click, Remove
                    Next
                End With
                .DropDownItems.Add(Level2Menu(Level2Count))
            Next
        End With
        Return Level1Menu
    End Function
    Overridable Sub ShowTheMenu()
        Dim Where As New System.Drawing.Point
        Where.X = Me.Right
        Where.Y = Me.Top
        contextmnu.Show(Where)
    End Sub
    Overridable Sub CheckText(ByVal objsender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim sender As ToolStripTextBox = DirectCast(objsender, ToolStripTextBox)
        If e.KeyChar <> ""c Then
            If AllowedCharacters.IndexOf(e.KeyChar) = -1 Then
                e.Handled = True
            End If
        End If
        If e.KeyChar = Chr(13) Then
            ControlSpecificOptionSelection(sender.Tag.ToString & " " & sender.Text.ToString)
            SDForm_Resize(Me, System.EventArgs.Empty)
            DrawToBuffer(Grafx.Graphics)
            Contextmnu.Close()
            Me.Refresh()
        End If
    End Sub
    Overridable Sub UpdateForm(ByVal sender As ToolStripTextBox, ByVal e As System.EventArgs)
        ControlSpecificOptionSelection(sender.Tag.ToString & " " & sender.Text.ToString)
        SDForm_Resize(Me, System.EventArgs.Empty)
        DrawToBuffer(Grafx.Graphics)
        Me.Refresh()
    End Sub

    'Overridable Sub ClickTheMenu(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs)
    Overridable Sub ClickTheMenu(ByVal objsender As Object, ByVal e As System.EventArgs)
        'Because there are control specific possible entries may need to use a combination of if then else and select case statements to process these correctly
        'timers are consistent among controls so this can be processed by the parent
        Dim sender As ToolStripMenuItem
        sender = DirectCast(objsender, ToolStripMenuItem)
        Select Case sender.Tag.ToString
            Case Is = "R_0"
                timer1.Interval = 10
            Case Is = "R_1"
                timer1.Interval = 30
            Case Is = "R_2"
                timer1.Interval = 100
            Case Is = "R_3"
                timer1.Interval = 300
            Case Is = "R_4"
                timer1.Interval = 1000
            Case Is = "C_0" 'Background
                Colormnu.ShowDialog()
                BackClr = Colormnu.Color
                Contextmnu.Close()
            Case Is = "C_1" 'Axis
                Colormnu.ShowDialog()
                AxisClr = Colormnu.Color
                AxisBrush.Color = AxisClr
                AxisPen.Color = AxisClr
                Contextmnu.Close()
            Case Is = "C_2" 'Data
                Colormnu.ShowDialog()
                Y_DataClr(XY_Selected) = Colormnu.Color
                Y_DataBrush(XY_Selected).Color = Y_DataClr(XY_Selected)
                Y_DataPen(XY_Selected).Color = Y_DataClr(XY_Selected)
                Contextmnu.Close()
            Case Is = "C_3" 'Apply format to all
                RaiseEvent SetToMyFormat(GetMyFormat)
            Case Is = "F" 'Font
                Fontmnu.ShowDialog()
                If Fontmnu.Font.FontFamily.IsStyleAvailable(FontStyle.Regular) Then
                    Y_AxisFont = Fontmnu.Font
                    X_AxisFont = Fontmnu.Font
                    For Count As Integer = 1 To Y_Number_Allowed
                        Y_DataFont(Count) = Fontmnu.Font
                    Next
                End If
            Case Else
                If InStr(sender.Tag.ToString, "O") <> 0 Or InStr(sender.Tag.ToString, "M") <> 0 Or InStr(sender.Tag.ToString, "X") <> 0 Then
                    'check for control specific options
                    ControlSpecificOptionSelection(sender.Tag.ToString)
                Else
                    'Split the tag into individual chars
                    Dim Pointers As String() = Split(sender.Tag.ToString, "_")
                    'each tag will have min/max and units at the end, and primary at the front
                    'so if there are only 3 chars then no option or suboption was used
                    Select Case XY_Selected
                        Case Is = 0 'X axis
                            X_PrimaryPointer = CInt(Pointers(0))
                            X_MinCurMaxPointer = CInt(Pointers(1))
                            X_UnitPointer = CInt(Pointers(2))
                            X_PrimaryLabel = CopyOfDataNames(X_PrimaryPointer)
                            X_MinMaxCurLabel = myMinCurMax(X_MinCurMaxPointer)
                            X_UnitsLabel = Split(CopyOfUnitsNames(X_PrimaryPointer))(X_UnitPointer)
                        Case Else
                            Y_PrimaryPointer(XY_Selected) = CInt(Pointers(0))
                            Y_MinCurMaxPointer(XY_Selected) = CInt(Pointers(1))
                            Y_UnitPointer(XY_Selected) = CInt(Pointers(2))
                            Y_PrimaryLabel(XY_Selected) = CopyOfDataNames(Y_PrimaryPointer(XY_Selected))
                            Y_MinMaxCurLabel(XY_Selected) = myMinCurMax(Y_MinCurMaxPointer(XY_Selected))
                            Y_UnitsLabel(XY_Selected) = Split(CopyOfUnitsNames(Y_PrimaryPointer(XY_Selected)))(Y_UnitPointer(XY_Selected))
                    End Select
                End If

        End Select

        SDForm_Resize(Me, System.EventArgs.Empty)
        DrawToBuffer(Grafx.Graphics)
        Me.Refresh()

    End Sub
    Friend Function GetMyFormat() As String
        '  Dim GetMyFormat As String

        With Me
            'GetMyFormat = .Location.X & Splitter & .Location.Y & Splitter & .Height & Splitter & .Width & Splitter 'dimensions and posisition of window
            'GetMyFormat += myConfiguration & Splitter & Y_Number_Allowed & Splitter & timer1.Interval & Splitter   'specific configuration and number of Y_datasets
            GetMyFormat += ColorTranslator.ToHtml(BackClr) & Splitter & ColorTranslator.ToHtml(AxisClr) & Splitter 'basic colors
            'GetMyFormat += X_PrimaryPointer & Splitter & X_MinCurMaxPointer & Splitter & X_UnitPointer & Splitter  'X data pointer
            'GetMyFormat += X_Minimum & Splitter & X_Maximum & Splitter                                             'X min and max
            GetMyFormat += X_AxisFont.Name & Splitter
            For Count As Integer = 1 To Y_Number_Allowed
                GetMyFormat += ColorTranslator.ToHtml(Y_DataClr(Count)) & Splitter
                'GetMyFormat += Y_PrimaryPointer(Count) & Splitter
                'GetMyFormat += Y_MinCurMaxPointer(Count) & Splitter
                'GetMyFormat += Y_UnitPointer(Count) & Splitter 'y data pointers
                'GetMyFormat += Y_Minimum(Count) & Splitter
                'GetMyFormat += Y_Maximum(Count) & Splitter
                'GetMyFormat += IsThisYSelected(Count) & Splitter
            Next

        End With
        'TempReport = TempReport & ControlSpecificSerializationData()
        Return GetMyFormat
    End Function
    Friend Sub SetMyFormat(ByVal SentFormat As String)
        Dim Parameters As String()
        'Dim NewLocation As System.Drawing.Point
        'Dim NewSize As System.Drawing.Size

        Parameters = Split(SentFormat, Splitter)

        'NewLocation.X = Parameters(0) : NewLocation.Y = Parameters(1) : NewSize.Height = Parameters(2) : NewSize.Width = Parameters(3) 'position and size
        'myConfiguration = Parameters(4) : Y_Number_Allowed = Parameters(5) : timer1.Interval = Parameters(6) 'basic configuration
        BackClr = ColorTranslator.FromHtml(Parameters(0)) : AxisClr = ColorTranslator.FromHtml(Parameters(1)) 'basic colors
        AxisBrush.Color = AxisClr : AxisPen.Color = AxisClr
        'X_PrimaryPointer = Parameters(9) : X_MinCurMaxPointer = Parameters(10) : X_UnitPointer = Parameters(11)  'X data pointer
        'X_PrimaryLabel = CopyOfDataNames(X_PrimaryPointer)
        'X_UnitsLabel = Split(CopyOfUnitsNames(X_PrimaryPointer))(X_UnitPointer)
        'X_MinMaxCurLabel = myMinCurMax(X_MinCurMaxPointer)

        'X_Minimum = Parameters(12) : X_Maximum = Parameters(13) 'X min and max
        X_AxisFont = New Font(Parameters(2), 5)
        Y_AxisFont = New Font(Parameters(2), 5)


        For Count As Integer = 1 To Y_Number_Allowed
            Y_DataClr(Count) = ColorTranslator.FromHtml(Parameters(3))
            Y_DataBrush(Count).Color = Y_DataClr(Count) : Y_DataPen(Count).Color = Y_DataClr(Count)
            'Y_PrimaryPointer(Count) = Parameters(16 + (Count - 1) * 7)
            'Y_PrimaryLabel(Count) = CopyOfDataNames(Y_PrimaryPointer(Count))
            'Y_MinCurMaxPointer(Count) = Parameters(17 + (Count - 1) * 7)
            'Y_MinMaxCurLabel(Count) = myMinCurMax(Y_MinCurMaxPointer(Count))
            'Y_UnitPointer(Count) = Parameters(18 + (Count - 1) * 7)
            'Y_UnitsLabel(Count) = Split(CopyOfUnitsNames(Y_PrimaryPointer(Count)))(Y_UnitPointer(Count))
            'Y_Minimum(Count) = Parameters(19 + (Count - 1) * 7)
            'Y_Maximum(Count) = Parameters(20 + (Count - 1) * 7)
            'IsThisYSelected(Count) = Parameters(21 + (Count - 1) * 7)
            Y_DataFont(Count) = New Font(Parameters(2), 5)
        Next
        SDForm_Resize(Me, System.EventArgs.Empty)
        DrawToBuffer(Grafx.Graphics)
        Me.Refresh()
    End Sub
    
    Friend Function ConvertedToRadians(ByVal Sent As Double) As Double
        Return Sent * Math.PI / 180
    End Function
    Overridable Sub ResetSDForm()

    End Sub
    Private Sub SDForm_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            IsLeft = False
            ShowTheMenu()
            DrawToBuffer(Grafx.Graphics)
            Me.Refresh()
        ElseIf e.Button = MouseButtons.Left Then
            IsLeft = True
            MouseDownPosition.X = MousePosition.X - Me.Location.X
            MouseDownPosition.Y = MousePosition.Y - Me.Location.Y
        End If
    End Sub
    Private Sub SimpleDynoSubForm_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        'HideYourSelf() 'CHECK - this just hides the form - does not remove it from the list (so the form gets saved and re-appears)
        RaiseEvent RemoveYourself(myNumber)
    End Sub
    Private Sub SimpleDynoSubForm_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If Me.WindowState <> FormWindowState.Minimized Then AlignTheForm()
        IsLeft = False
    End Sub
    Private Sub SDForm_TimerTick(ByVal sender As Object, ByVal e As EventArgs)
        TimerTiggered = True
        X_Result = CopyOfData(X_PrimaryPointer, X_MinCurMaxPointer) * CopyOfUnits(X_PrimaryPointer, X_UnitPointer)
        For Count As Integer = 1 To Y_Number_Allowed
            Y_Result(Count) = CopyOfData(Y_PrimaryPointer(Count), Y_MinCurMaxPointer(Count)) * CopyOfUnits(Y_PrimaryPointer(Count), Y_UnitPointer(Count))
        Next
        DrawToBuffer(Grafx.Graphics)
        TimerTiggered = False
        Me.Refresh()
    End Sub
    Friend Sub SDForm_Resize(ByVal sender As Object, ByVal e As EventArgs)

        If Resizing = False Then
            Resizing = True

            If Me.WindowState <> FormWindowState.Minimized Then
                'Crude Snap to Grid
                Dim TempSize As Size

                If Me.ClientSize.Height < 20 Then
                    TempSize.Height = 20
                    TempSize.Width = Me.ClientSize.Width
                    Me.ClientSize = TempSize
                End If
                If Me.ClientSize.Width < 20 Then
                    TempSize.Width = 20
                    TempSize.Height = Me.ClientSize.Height
                    Me.ClientSize = TempSize
                End If
                TempSize.Height = CInt(Me.ClientSize.Height / GridSnap) * GridSnap
                TempSize.Width = CInt(Me.ClientSize.Width / GridSnap) * GridSnap
                Me.ClientSize = TempSize

                ' Re-create the graphics buffer for a new window size.
                context.MaximumBuffer = New Size(Me.Width + 1, Me.Height + 1)
                If (Grafx IsNot Nothing) Then
                    Grafx.Dispose()
                    Grafx = Nothing
                End If
                Grafx = context.Allocate(Me.CreateGraphics(), New Rectangle(0, 0, Me.ClientSize.Width, Me.ClientSize.Height))

                ControlSpecificResize()

                ' Cause the background to be cleared and redraw.
                DrawToBuffer(Grafx.Graphics)
                Me.Refresh()
            End If
            Resizing = False
        End If
    End Sub
    Private Sub SDForm_Move(ByVal sender As Object, ByVal e As EventArgs)
        If Me.WindowState <> FormWindowState.Minimized And Not IsLeft Then AlignTheForm()
    End Sub
    Overridable Sub SDForm_MouseMove(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseMove
        Dim NewMousePosition As System.Drawing.Point = MousePosition
        Dim NewWindowPosition As System.Drawing.Point
        If IsLeft Then
            NewWindowPosition.X = NewMousePosition.X - MouseDownPosition.X
            NewWindowPosition.Y = NewMousePosition.Y - MouseDownPosition.Y
            Me.Location = NewWindowPosition
        End If
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Grafx.Render(e.Graphics)
    End Sub
    Private Sub AlignTheForm()
        Dim NewLocation As System.Drawing.Point
        NewLocation.X = CInt(Me.Location.X / GridSnap) * GridSnap
        NewLocation.Y = CInt(Me.Location.Y / GridSnap) * GridSnap
        Me.Location = NewLocation
    End Sub
    Friend Function NewCustomFormat(ByVal sent As Double) As String
        'Dim TempFormat As String
        Select Case sent
            Case Is >= 100
                'TempFormat = "0"
                Return sent.ToString("0")
            Case Is >= 10
                'TempFormat = "0.0"
                Return sent.ToString("0.0")
            Case Is >= 1
                'TempFormat = "0.00"
                Return sent.ToString("0.00")
            Case Is >= 0.1
                'TempFormat = "0.000"
                Return sent.ToString("0.000")
            Case Is >= 0.01
                'TempFormat = "0.0000"
                Return sent.ToString("0.0000")
            Case Else
                'TempFormat = "0"
                Return sent.ToString("0")
        End Select
    End Function
End Class



