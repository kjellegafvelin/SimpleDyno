<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Fit
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Fit))
        Me.cmbWhichFit = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFitStart = New System.Windows.Forms.TextBox()
        Me.scrlStartFit = New System.Windows.Forms.VScrollBar()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblPowerRunSpikeLevel = New System.Windows.Forms.Label()
        Me.txtPowerRunSpikeLevel = New System.Windows.Forms.TextBox()
        Me.txtVoltageSmooth = New System.Windows.Forms.TextBox()
        Me.scrlVoltageSmooth = New System.Windows.Forms.VScrollBar()
        Me.txtCurrentSmooth = New System.Windows.Forms.TextBox()
        Me.scrlCurrentSmooth = New System.Windows.Forms.VScrollBar()
        Me.rdoVoltage = New System.Windows.Forms.RadioButton()
        Me.rdoCurrent = New System.Windows.Forms.RadioButton()
        Me.rdoRPM1 = New System.Windows.Forms.RadioButton()
        Me.btnAddAnalysis = New System.Windows.Forms.Button()
        Me.chkAddOrNew = New System.Windows.Forms.CheckBox()
        Me.prgFit = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.cmbWhichRDFit = New System.Windows.Forms.ComboBox()
        Me.rdoRunDown = New System.Windows.Forms.RadioButton()
        Me.lblUsingRunDownFile = New System.Windows.Forms.Label()
        Me.txtRPM1Smooth = New System.Windows.Forms.TextBox()
        Me.scrlRPM1Smooth = New System.Windows.Forms.VScrollBar()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnStopFitting = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCoastDownSmooth = New System.Windows.Forms.TextBox()
        Me.scrlCoastDownSmooth = New System.Windows.Forms.VScrollBar()
        Me.pnlDataWindow = New SimpleDyno.DoubleBufferPanel()
        Me.SuspendLayout()
        '
        'cmbWhichFit
        '
        Me.cmbWhichFit.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbWhichFit.FormattingEnabled = True
        Me.cmbWhichFit.Location = New System.Drawing.Point(95, 10)
        Me.cmbWhichFit.Name = "cmbWhichFit"
        Me.cmbWhichFit.Size = New System.Drawing.Size(105, 21)
        Me.cmbWhichFit.TabIndex = 74
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(27, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Start Fit @ Point"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFitStart
        '
        Me.txtFitStart.BackColor = System.Drawing.Color.White
        Me.txtFitStart.CausesValidation = False
        Me.txtFitStart.Enabled = False
        Me.txtFitStart.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFitStart.Location = New System.Drawing.Point(119, 86)
        Me.txtFitStart.Name = "txtFitStart"
        Me.txtFitStart.ReadOnly = True
        Me.txtFitStart.Size = New System.Drawing.Size(46, 21)
        Me.txtFitStart.TabIndex = 3
        Me.txtFitStart.Tag = ""
        Me.txtFitStart.Text = "1"
        Me.txtFitStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'scrlStartFit
        '
        Me.scrlStartFit.LargeChange = 1
        Me.scrlStartFit.Location = New System.Drawing.Point(166, 86)
        Me.scrlStartFit.Maximum = 9
        Me.scrlStartFit.Name = "scrlStartFit"
        Me.scrlStartFit.Size = New System.Drawing.Size(34, 21)
        Me.scrlStartFit.TabIndex = 4
        Me.scrlStartFit.Value = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(31, 64)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 13)
        Me.Label7.TabIndex = 74
        Me.Label7.Text = "Spike Threshold"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPowerRunSpikeLevel
        '
        Me.lblPowerRunSpikeLevel.AutoSize = True
        Me.lblPowerRunSpikeLevel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPowerRunSpikeLevel.Location = New System.Drawing.Point(166, 64)
        Me.lblPowerRunSpikeLevel.Name = "lblPowerRunSpikeLevel"
        Me.lblPowerRunSpikeLevel.Size = New System.Drawing.Size(34, 13)
        Me.lblPowerRunSpikeLevel.TabIndex = 73
        Me.lblPowerRunSpikeLevel.Text = "RPM1"
        Me.lblPowerRunSpikeLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPowerRunSpikeLevel
        '
        Me.txtPowerRunSpikeLevel.CausesValidation = False
        Me.txtPowerRunSpikeLevel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPowerRunSpikeLevel.Location = New System.Drawing.Point(119, 61)
        Me.txtPowerRunSpikeLevel.Name = "txtPowerRunSpikeLevel"
        Me.txtPowerRunSpikeLevel.Size = New System.Drawing.Size(46, 21)
        Me.txtPowerRunSpikeLevel.TabIndex = 72
        Me.txtPowerRunSpikeLevel.Tag = "1\999999"
        Me.txtPowerRunSpikeLevel.Text = "10000"
        Me.txtPowerRunSpikeLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtVoltageSmooth
        '
        Me.txtVoltageSmooth.BackColor = System.Drawing.Color.White
        Me.txtVoltageSmooth.CausesValidation = False
        Me.txtVoltageSmooth.Enabled = False
        Me.txtVoltageSmooth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVoltageSmooth.Location = New System.Drawing.Point(119, 245)
        Me.txtVoltageSmooth.Name = "txtVoltageSmooth"
        Me.txtVoltageSmooth.ReadOnly = True
        Me.txtVoltageSmooth.Size = New System.Drawing.Size(46, 21)
        Me.txtVoltageSmooth.TabIndex = 77
        Me.txtVoltageSmooth.Tag = ""
        Me.txtVoltageSmooth.Text = "0.5"
        Me.txtVoltageSmooth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'scrlVoltageSmooth
        '
        Me.scrlVoltageSmooth.Enabled = False
        Me.scrlVoltageSmooth.LargeChange = 1
        Me.scrlVoltageSmooth.Location = New System.Drawing.Point(166, 245)
        Me.scrlVoltageSmooth.Maximum = 19
        Me.scrlVoltageSmooth.Name = "scrlVoltageSmooth"
        Me.scrlVoltageSmooth.Size = New System.Drawing.Size(34, 22)
        Me.scrlVoltageSmooth.TabIndex = 78
        Me.scrlVoltageSmooth.Value = 19
        '
        'txtCurrentSmooth
        '
        Me.txtCurrentSmooth.BackColor = System.Drawing.Color.White
        Me.txtCurrentSmooth.CausesValidation = False
        Me.txtCurrentSmooth.Enabled = False
        Me.txtCurrentSmooth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrentSmooth.Location = New System.Drawing.Point(119, 211)
        Me.txtCurrentSmooth.Name = "txtCurrentSmooth"
        Me.txtCurrentSmooth.ReadOnly = True
        Me.txtCurrentSmooth.Size = New System.Drawing.Size(46, 21)
        Me.txtCurrentSmooth.TabIndex = 75
        Me.txtCurrentSmooth.Tag = ""
        Me.txtCurrentSmooth.Text = "0.5"
        Me.txtCurrentSmooth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'scrlCurrentSmooth
        '
        Me.scrlCurrentSmooth.Enabled = False
        Me.scrlCurrentSmooth.LargeChange = 1
        Me.scrlCurrentSmooth.Location = New System.Drawing.Point(166, 211)
        Me.scrlCurrentSmooth.Maximum = 19
        Me.scrlCurrentSmooth.Name = "scrlCurrentSmooth"
        Me.scrlCurrentSmooth.Size = New System.Drawing.Size(34, 22)
        Me.scrlCurrentSmooth.TabIndex = 76
        Me.scrlCurrentSmooth.Value = 19
        '
        'rdoVoltage
        '
        Me.rdoVoltage.AutoSize = True
        Me.rdoVoltage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoVoltage.Location = New System.Drawing.Point(52, 246)
        Me.rdoVoltage.Name = "rdoVoltage"
        Me.rdoVoltage.Size = New System.Drawing.Size(61, 17)
        Me.rdoVoltage.TabIndex = 8
        Me.rdoVoltage.Text = "Voltage"
        Me.rdoVoltage.UseVisualStyleBackColor = True
        '
        'rdoCurrent
        '
        Me.rdoCurrent.AutoSize = True
        Me.rdoCurrent.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoCurrent.Location = New System.Drawing.Point(51, 212)
        Me.rdoCurrent.Name = "rdoCurrent"
        Me.rdoCurrent.Size = New System.Drawing.Size(62, 17)
        Me.rdoCurrent.TabIndex = 5
        Me.rdoCurrent.Text = "Current"
        Me.rdoCurrent.UseVisualStyleBackColor = True
        '
        'rdoRPM1
        '
        Me.rdoRPM1.AutoSize = True
        Me.rdoRPM1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoRPM1.Location = New System.Drawing.Point(12, 11)
        Me.rdoRPM1.Name = "rdoRPM1"
        Me.rdoRPM1.Size = New System.Drawing.Size(52, 17)
        Me.rdoRPM1.TabIndex = 0
        Me.rdoRPM1.Text = "RPM1"
        Me.rdoRPM1.UseVisualStyleBackColor = True
        '
        'btnAddAnalysis
        '
        Me.btnAddAnalysis.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddAnalysis.Location = New System.Drawing.Point(2, 349)
        Me.btnAddAnalysis.Name = "btnAddAnalysis"
        Me.btnAddAnalysis.Size = New System.Drawing.Size(192, 21)
        Me.btnAddAnalysis.TabIndex = 78
        Me.btnAddAnalysis.Text = "Go to Analysis"
        Me.btnAddAnalysis.UseVisualStyleBackColor = True
        '
        'chkAddOrNew
        '
        Me.chkAddOrNew.AutoSize = True
        Me.chkAddOrNew.Location = New System.Drawing.Point(6, 377)
        Me.chkAddOrNew.Name = "chkAddOrNew"
        Me.chkAddOrNew.Size = New System.Drawing.Size(119, 17)
        Me.chkAddOrNew.TabIndex = 79
        Me.chkAddOrNew.Text = "Add to existing data"
        Me.chkAddOrNew.UseVisualStyleBackColor = True
        '
        'prgFit
        '
        Me.prgFit.Location = New System.Drawing.Point(2, 300)
        Me.prgFit.Maximum = 1250
        Me.prgFit.Name = "prgFit"
        Me.prgFit.Size = New System.Drawing.Size(192, 16)
        Me.prgFit.Step = 1
        Me.prgFit.TabIndex = 80
        '
        'lblProgress
        '
        Me.lblProgress.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgress.Location = New System.Drawing.Point(3, 282)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(166, 15)
        Me.lblProgress.TabIndex = 81
        Me.lblProgress.Text = "Progress"
        Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbWhichRDFit
        '
        Me.cmbWhichRDFit.Enabled = False
        Me.cmbWhichRDFit.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbWhichRDFit.FormattingEnabled = True
        Me.cmbWhichRDFit.Location = New System.Drawing.Point(95, 125)
        Me.cmbWhichRDFit.Name = "cmbWhichRDFit"
        Me.cmbWhichRDFit.Size = New System.Drawing.Size(105, 21)
        Me.cmbWhichRDFit.TabIndex = 83
        '
        'rdoRunDown
        '
        Me.rdoRunDown.AutoSize = True
        Me.rdoRunDown.Enabled = False
        Me.rdoRunDown.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoRunDown.Location = New System.Drawing.Point(12, 126)
        Me.rdoRunDown.Name = "rdoRunDown"
        Me.rdoRunDown.Size = New System.Drawing.Size(83, 17)
        Me.rdoRunDown.TabIndex = 82
        Me.rdoRunDown.Text = "Coast Down"
        Me.rdoRunDown.UseVisualStyleBackColor = True
        '
        'lblUsingRunDownFile
        '
        Me.lblUsingRunDownFile.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsingRunDownFile.Location = New System.Drawing.Point(6, 176)
        Me.lblUsingRunDownFile.Name = "lblUsingRunDownFile"
        Me.lblUsingRunDownFile.Size = New System.Drawing.Size(166, 15)
        Me.lblUsingRunDownFile.TabIndex = 84
        Me.lblUsingRunDownFile.Text = "Coast Down File"
        Me.lblUsingRunDownFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRPM1Smooth
        '
        Me.txtRPM1Smooth.BackColor = System.Drawing.Color.White
        Me.txtRPM1Smooth.CausesValidation = False
        Me.txtRPM1Smooth.Enabled = False
        Me.txtRPM1Smooth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRPM1Smooth.Location = New System.Drawing.Point(119, 34)
        Me.txtRPM1Smooth.Name = "txtRPM1Smooth"
        Me.txtRPM1Smooth.ReadOnly = True
        Me.txtRPM1Smooth.Size = New System.Drawing.Size(46, 21)
        Me.txtRPM1Smooth.TabIndex = 85
        Me.txtRPM1Smooth.Tag = ""
        Me.txtRPM1Smooth.Text = "0.5"
        Me.txtRPM1Smooth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'scrlRPM1Smooth
        '
        Me.scrlRPM1Smooth.LargeChange = 1
        Me.scrlRPM1Smooth.Location = New System.Drawing.Point(166, 34)
        Me.scrlRPM1Smooth.Maximum = 39
        Me.scrlRPM1Smooth.Name = "scrlRPM1Smooth"
        Me.scrlRPM1Smooth.Size = New System.Drawing.Size(34, 22)
        Me.scrlRPM1Smooth.TabIndex = 86
        Me.scrlRPM1Smooth.Value = 39
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(42, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 87
        Me.Label2.Text = "Smooth Level"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnStopFitting
        '
        Me.btnStopFitting.Location = New System.Drawing.Point(2, 322)
        Me.btnStopFitting.Name = "btnStopFitting"
        Me.btnStopFitting.Size = New System.Drawing.Size(192, 21)
        Me.btnStopFitting.TabIndex = 88
        Me.btnStopFitting.Text = "Stop Fitting"
        Me.btnStopFitting.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(42, 152)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 91
        Me.Label3.Text = "Smooth Level"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCoastDownSmooth
        '
        Me.txtCoastDownSmooth.BackColor = System.Drawing.Color.White
        Me.txtCoastDownSmooth.CausesValidation = False
        Me.txtCoastDownSmooth.Enabled = False
        Me.txtCoastDownSmooth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoastDownSmooth.Location = New System.Drawing.Point(119, 149)
        Me.txtCoastDownSmooth.Name = "txtCoastDownSmooth"
        Me.txtCoastDownSmooth.ReadOnly = True
        Me.txtCoastDownSmooth.Size = New System.Drawing.Size(46, 21)
        Me.txtCoastDownSmooth.TabIndex = 89
        Me.txtCoastDownSmooth.Tag = ""
        Me.txtCoastDownSmooth.Text = "0.5"
        Me.txtCoastDownSmooth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'scrlCoastDownSmooth
        '
        Me.scrlCoastDownSmooth.Enabled = False
        Me.scrlCoastDownSmooth.LargeChange = 1
        Me.scrlCoastDownSmooth.Location = New System.Drawing.Point(166, 149)
        Me.scrlCoastDownSmooth.Maximum = 39
        Me.scrlCoastDownSmooth.Name = "scrlCoastDownSmooth"
        Me.scrlCoastDownSmooth.Size = New System.Drawing.Size(34, 22)
        Me.scrlCoastDownSmooth.TabIndex = 90
        Me.scrlCoastDownSmooth.Value = 39
        '
        'pnlDataWindow
        '
        Me.pnlDataWindow.Location = New System.Drawing.Point(206, 10)
        Me.pnlDataWindow.Name = "pnlDataWindow"
        Me.pnlDataWindow.Size = New System.Drawing.Size(358, 237)
        Me.pnlDataWindow.TabIndex = 76
        '
        'Fit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 555)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCoastDownSmooth)
        Me.Controls.Add(Me.scrlCoastDownSmooth)
        Me.Controls.Add(Me.btnStopFitting)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtRPM1Smooth)
        Me.Controls.Add(Me.scrlRPM1Smooth)
        Me.Controls.Add(Me.lblUsingRunDownFile)
        Me.Controls.Add(Me.cmbWhichRDFit)
        Me.Controls.Add(Me.rdoRunDown)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.prgFit)
        Me.Controls.Add(Me.chkAddOrNew)
        Me.Controls.Add(Me.txtVoltageSmooth)
        Me.Controls.Add(Me.btnAddAnalysis)
        Me.Controls.Add(Me.scrlVoltageSmooth)
        Me.Controls.Add(Me.txtCurrentSmooth)
        Me.Controls.Add(Me.pnlDataWindow)
        Me.Controls.Add(Me.scrlCurrentSmooth)
        Me.Controls.Add(Me.cmbWhichFit)
        Me.Controls.Add(Me.rdoRPM1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.rdoCurrent)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtPowerRunSpikeLevel)
        Me.Controls.Add(Me.txtFitStart)
        Me.Controls.Add(Me.lblPowerRunSpikeLevel)
        Me.Controls.Add(Me.scrlStartFit)
        Me.Controls.Add(Me.rdoVoltage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Fit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Power Run Curve Fitting"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbWhichFit As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFitStart As System.Windows.Forms.TextBox
    Friend WithEvents scrlStartFit As System.Windows.Forms.VScrollBar
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblPowerRunSpikeLevel As System.Windows.Forms.Label
    Friend WithEvents txtPowerRunSpikeLevel As System.Windows.Forms.TextBox
    Friend WithEvents rdoVoltage As System.Windows.Forms.RadioButton
    Friend WithEvents rdoCurrent As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRPM1 As System.Windows.Forms.RadioButton
    Friend WithEvents pnlDataWindow As SimpleDyno.DoubleBufferPanel
    Friend WithEvents txtVoltageSmooth As System.Windows.Forms.TextBox
    Friend WithEvents scrlVoltageSmooth As System.Windows.Forms.VScrollBar
    Friend WithEvents txtCurrentSmooth As System.Windows.Forms.TextBox
    Friend WithEvents scrlCurrentSmooth As System.Windows.Forms.VScrollBar
    Friend WithEvents btnAddAnalysis As System.Windows.Forms.Button
    Friend WithEvents chkAddOrNew As System.Windows.Forms.CheckBox
    Friend WithEvents prgFit As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents cmbWhichRDFit As System.Windows.Forms.ComboBox
    Friend WithEvents rdoRunDown As System.Windows.Forms.RadioButton
    Friend WithEvents lblUsingRunDownFile As System.Windows.Forms.Label
    Friend WithEvents txtRPM1Smooth As System.Windows.Forms.TextBox
    Friend WithEvents scrlRPM1Smooth As System.Windows.Forms.VScrollBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnStopFitting As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCoastDownSmooth As System.Windows.Forms.TextBox
    Friend WithEvents scrlCoastDownSmooth As System.Windows.Forms.VScrollBar
End Class
