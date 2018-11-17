<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.AGauge1 = New AGauge.AGauge()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'AGauge1
        '
        Me.AGauge1.BaseArcColor = System.Drawing.Color.Gray
        Me.AGauge1.BaseArcRadius = 280
        Me.AGauge1.BaseArcStart = 135
        Me.AGauge1.BaseArcSweep = 270
        Me.AGauge1.BaseArcWidth = 3
        Me.AGauge1.Cap_Idx = CType(1, Byte)
        Me.AGauge1.CapColors = New System.Drawing.Color() {System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black}
        Me.AGauge1.CapPosition = New System.Drawing.Point(10, 10)
        Me.AGauge1.CapsPosition = New System.Drawing.Point() {New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10)}
        Me.AGauge1.CapsText = New String() {"", "", "", "", ""}
        Me.AGauge1.CapText = ""
        Me.AGauge1.Center = New System.Drawing.Point(360, 290)
        Me.AGauge1.Font = New System.Drawing.Font("Tahoma", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AGauge1.Location = New System.Drawing.Point(12, 12)
        Me.AGauge1.MaxValue = 10000.0!
        Me.AGauge1.MinValue = 0.0!
        Me.AGauge1.Name = "AGauge1"
        Me.AGauge1.NeedleColor1 = AGauge.AGauge.NeedleColorEnum.Red
        Me.AGauge1.NeedleColor2 = System.Drawing.Color.Maroon
        Me.AGauge1.NeedleRadius = 275
        Me.AGauge1.NeedleType = 1
        Me.AGauge1.NeedleWidth = 8
        Me.AGauge1.Range_Idx = CType(0, Byte)
        Me.AGauge1.RangeColor = System.Drawing.Color.LightGreen
        Me.AGauge1.RangeEnabled = True
        Me.AGauge1.RangeEndValue = 10000.0!
        Me.AGauge1.RangeInnerRadius = 230
        Me.AGauge1.RangeOuterRadius = 280
        Me.AGauge1.RangesColor = New System.Drawing.Color() {System.Drawing.Color.LightGreen, System.Drawing.Color.Red, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control}
        Me.AGauge1.RangesEnabled = New Boolean() {True, False, False, False, False}
        Me.AGauge1.RangesEndValue = New Single() {10000.0!, 400.0!, 0.0!, 0.0!, 0.0!}
        Me.AGauge1.RangesInnerRadius = New Integer() {230, 70, 70, 70, 70}
        Me.AGauge1.RangesOuterRadius = New Integer() {280, 80, 80, 80, 80}
        Me.AGauge1.RangesStartValue = New Single() {0.0!, 300.0!, 0.0!, 0.0!, 0.0!}
        Me.AGauge1.RangeStartValue = 0.0!
        Me.AGauge1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AGauge1.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AGauge1.ScaleLinesInterInnerRadius = 230
        Me.AGauge1.ScaleLinesInterOuterRadius = 280
        Me.AGauge1.ScaleLinesInterWidth = 1
        Me.AGauge1.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AGauge1.ScaleLinesMajorInnerRadius = 220
        Me.AGauge1.ScaleLinesMajorOuterRadius = 280
        Me.AGauge1.ScaleLinesMajorStepValue = 2000.0!
        Me.AGauge1.ScaleLinesMajorWidth = 3
        Me.AGauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AGauge1.ScaleLinesMinorInnerRadius = 280
        Me.AGauge1.ScaleLinesMinorNumOf = 9
        Me.AGauge1.ScaleLinesMinorOuterRadius = 250
        Me.AGauge1.ScaleLinesMinorWidth = 1
        Me.AGauge1.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AGauge1.ScaleNumbersFormat = ""
        Me.AGauge1.ScaleNumbersRadius = 170
        Me.AGauge1.ScaleNumbersRotation = 0
        Me.AGauge1.ScaleNumbersStartScaleLine = 0
        Me.AGauge1.ScaleNumbersStepScaleLines = 1
        Me.AGauge1.Size = New System.Drawing.Size(720, 580)
        Me.AGauge1.TabIndex = 0
        Me.AGauge1.Text = "AGauge1"
        Me.AGauge1.Value = 0.0!
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 72.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(181, 498)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(383, 120)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "999999"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(742, 618)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.AGauge1)
        Me.Name = "Form2"
        Me.Text = "SimpleDyno RPM Gauge"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AGauge1 As AGauge.AGauge
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
