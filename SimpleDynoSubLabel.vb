Public Class SimpleDynoSubLabel
    Inherits SimpleDynoSubForm

    'Control Specific Text Positions
    Private ParameterLabel As Point
    Private ResultLabel As Point
    Private UnitLabel As Point
    
    'Control specific configuration variable
    Public Overrides Sub ControlSpecificInitialization()
        myType = "Label"
        Y_Number_Allowed = 1
        XY_Selected = 1
        myConfiguration = "Vertical"
    End Sub
    Public Overrides Sub ControlSpecificResize()

        Dim Increment As Single = 0.1
        Dim DataTestString As String = "999999" 'Assumes no value displayed will be > 999999

        'Need to find the longer of the Parameter and unit strings
        Dim LabelTestString As String = " "
        For Count As Integer = 1 To Y_Number_Allowed
            If Y_PrimaryLabel(Count).Length > LabelTestString.Length Then LabelTestString = Y_PrimaryLabel(Count)
            If myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)).Length + " ".Length + Y_UnitsLabel(Count).Length > LabelTestString.Length Then LabelTestString = myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)).Length & " " & Y_UnitsLabel(Count)
        Next

        Select Case myConfiguration
            Case Is = "Vertical"
                'Divide The window height into GR proportion
                Dim DataFontHeight As Single = (Me.ClientSize.Height / GoldenRatio)
                'The remainder divided by two is the height for the primary and unit labels
                Dim LabelFontHeight As Single = (Me.ClientSize.Height - DataFontHeight) / 2
                'create a temporary font
                Dim TempFont As New System.Drawing.Font(Y_DataFont(Y_Number_Allowed).Name, Increment)
                'now scale the data font
                Do Until Grafx.Graphics.MeasureString(DataTestString, TempFont).Width >= Me.ClientSize.Width Or Grafx.Graphics.MeasureString(DataTestString, TempFont).Height >= DataFontHeight
                    TempFont = New System.Drawing.Font(Y_DataFont(Y_Number_Allowed).Name, TempFont.Size + Increment)
                Loop
                'set the datafont to the tempfont size
                Y_DataFont(Y_Number_Allowed) = New System.Drawing.Font(Y_DataFont(Y_Number_Allowed).Name, TempFont.Size)
                'now repeat for the label font
                'reset tempfont
                TempFont = New System.Drawing.Font(Y_AxisFont.Name, Increment)
                'scale the labelfont
                Do Until Grafx.Graphics.MeasureString(LabelTestString, TempFont).Width >= Me.ClientSize.Width Or Grafx.Graphics.MeasureString(LabelTestString, TempFont).Height >= LabelFontHeight
                    TempFont = New System.Drawing.Font(Y_AxisFont.Name, TempFont.Size + Increment)
                Loop
                'set the labelfont to the tempfont size
                Y_AxisFont = New System.Drawing.Font(Y_AxisFont.Name, TempFont.Size)
                'Set up text positions based on available data
                With Grafx.Graphics
                    ParameterLabel.X = CInt((Me.ClientSize.Width - .MeasureString(Y_PrimaryLabel(XY_Selected), Y_AxisFont).Width) / 2)
                    ResultLabel.Y = CInt((Me.ClientSize.Height - .MeasureString(DataTestString, Y_DataFont(XY_Selected)).Height) / 2)
                    ParameterLabel.Y = CInt((ResultLabel.Y - .MeasureString(Y_PrimaryLabel(XY_Selected), Y_AxisFont).Height) / 2)
                    UnitLabel.X = CInt((Me.ClientSize.Width - .MeasureString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(XY_Selected), Y_AxisFont).Width) / 2)
                    UnitLabel.Y = CInt(Me.ClientSize.Height - .MeasureString(Y_UnitsLabel(XY_Selected), Y_AxisFont).Height - ParameterLabel.Y)
                End With
            Case Is = "Horizontal"
                'Divide The window height into GR proportion
                Dim DataFontWidth As Double = Me.ClientSize.Width / 2 'GoldenRatio
                'The remainder divided by two is the height for the primary and unit labels
                Dim LabelFontWidth As Double = (Me.ClientSize.Width - DataFontWidth) / 2
                'create a temporary font
                Dim TempFont As New System.Drawing.Font(Y_DataFont(Y_Number_Allowed).Name, Increment)
                'now scale the data font
                Do Until Grafx.Graphics.MeasureString(DataTestString, TempFont).Width >= DataFontWidth Or Grafx.Graphics.MeasureString(DataTestString, TempFont).Height >= Me.ClientSize.Height
                    TempFont = New System.Drawing.Font(Y_DataFont(Y_Number_Allowed).Name, TempFont.Size + Increment)
                Loop
                'set the datafont to the tempfont size
                Y_DataFont(Y_Number_Allowed) = New System.Drawing.Font(Y_DataFont(Y_Number_Allowed).Name, TempFont.Size)
                'now repeat for the label font
                'reset tempfont
                TempFont = New System.Drawing.Font(Y_AxisFont.Name, Increment)
                'scale the labelfont
                Do Until Grafx.Graphics.MeasureString(LabelTestString, TempFont).Width >= LabelFontWidth Or Grafx.Graphics.MeasureString(LabelTestString, TempFont).Height >= Me.ClientSize.Height
                    TempFont = New System.Drawing.Font(Y_AxisFont.Name, TempFont.Size + Increment)
                Loop
                'set the labelfont to the tempfont size
                Y_AxisFont = New System.Drawing.Font(Y_AxisFont.Name, TempFont.Size)
                'Set up text positions based on available data
                With Grafx.Graphics
                    ParameterLabel.Y = CInt((Me.ClientSize.Height - .MeasureString(Y_PrimaryLabel(XY_Selected), Y_AxisFont).Height) / 2)
                    ParameterLabel.X = Me.ClientRectangle.Left
                    ResultLabel.Y = CInt((Me.ClientSize.Height - .MeasureString(DataTestString, Y_DataFont(XY_Selected)).Height) / 2)
                    UnitLabel.X = CInt(Me.ClientRectangle.Right - .MeasureString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(XY_Selected), Y_AxisFont).Width)
                    UnitLabel.Y = CInt((Me.ClientSize.Height - .MeasureString(Y_UnitsLabel(XY_Selected), Y_AxisFont).Height) / 2)
                End With
        End Select

    End Sub
    Overrides Sub DrawToBuffer(ByVal g As Graphics)
       
        Dim StringResult As String
        'replace the custom format with new version
        'StringResult = Y_Result(XY_Selected).ToString(CustomFormat(Y_Result(XY_Selected)))
        StringResult = NewCustomFormat(Y_Result(XY_Selected)) '.ToString(CustomFormat(Y_Result(XY_Selected)))
        ResultLabel.X = CInt((Me.ClientSize.Width - Grafx.Graphics.MeasureString(StringResult, Y_DataFont(XY_Selected)).Width) / 2)

        Grafx.Graphics.Clear(BackClr)
       
        With Grafx.Graphics
            .DrawString(Y_PrimaryLabel(XY_Selected), Y_AxisFont, AxisBrush, ParameterLabel)
            .DrawString(StringResult, Y_DataFont(XY_Selected), Y_DataBrush(XY_Selected), ResultLabel)
            .DrawString(myMinCurMaxAbb(Y_MinCurMaxPointer(XY_Selected)) & " " & Y_UnitsLabel(XY_Selected), Y_AxisFont, AxisBrush, UnitLabel)
        End With

    End Sub
    Overrides Sub AddControlSpecificOptionItems()

        Dim TestStrip As ToolStripMenuItem
        Dim str1 As String
        Dim str2 As String()
        Dim str3 As String()

        str1 = "Configuration"
        str2 = {"Vertical", "Horizontal"}
        str3 = {}
     
        TestStrip = CreateAToolStripMenuItem("O", str1, str2, str3) ', str4, str5)
        contextmnu.Items.Add(TestStrip)

    End Sub
    Public Overrides Sub ControlSpecificOptionSelection(ByVal Sent As String)
        Select Case Sent
            Case Is = "O_0"
                myConfiguration = "Vertical"
            Case Is = "O_1"
                myConfiguration = "Horizontal"
        End Select
    End Sub
    Public Overrides Function ControlSpecificSerializationData() As String '- REMOVE COLOR ETC THAT SHOULD BE HANDLED BY THE PARENT CLASS

    End Function
    Public Overrides Sub ControlSpecficCreateFromSerializedData(ByVal Sent As String())

    End Sub
End Class
