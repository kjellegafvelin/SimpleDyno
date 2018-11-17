<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Analysis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Analysis))
        Me.btnAddOverlayFile = New System.Windows.Forms.Button()
        Me.btnClearOverlay = New System.Windows.Forms.Button()
        Me.btnSaveOverlay = New System.Windows.Forms.Button()
        Me.cmbOverlayUnitsY2 = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayUnitsY1 = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayUnitsX = New System.Windows.Forms.ComboBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.cmbOverlayDataY2 = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayDataY1 = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayDataX = New System.Windows.Forms.ComboBox()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.cmbOverlayCorrectedSpeedUnits = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayUnitsY4 = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayUnitsY3 = New System.Windows.Forms.ComboBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.cmbOverlayDataY4 = New System.Windows.Forms.ComboBox()
        Me.cmbOverlayDataY3 = New System.Windows.Forms.ComboBox()
        Me.lblCurrentXValue = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.pnlDataOverlay = New SimpleDyno.DoubleBufferPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnAddOverlayFile
        '
        Me.btnAddOverlayFile.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddOverlayFile.Location = New System.Drawing.Point(3, 3)
        Me.btnAddOverlayFile.Name = "btnAddOverlayFile"
        Me.btnAddOverlayFile.Size = New System.Drawing.Size(154, 29)
        Me.btnAddOverlayFile.TabIndex = 65
        Me.btnAddOverlayFile.Text = "Add..."
        '
        'btnClearOverlay
        '
        Me.btnClearOverlay.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearOverlay.Location = New System.Drawing.Point(3, 34)
        Me.btnClearOverlay.Name = "btnClearOverlay"
        Me.btnClearOverlay.Size = New System.Drawing.Size(154, 29)
        Me.btnClearOverlay.TabIndex = 66
        Me.btnClearOverlay.Text = "Clear"
        '
        'btnSaveOverlay
        '
        Me.btnSaveOverlay.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveOverlay.Location = New System.Drawing.Point(3, 65)
        Me.btnSaveOverlay.Name = "btnSaveOverlay"
        Me.btnSaveOverlay.Size = New System.Drawing.Size(154, 29)
        Me.btnSaveOverlay.TabIndex = 67
        Me.btnSaveOverlay.Text = "Save"
        '
        'cmbOverlayUnitsY2
        '
        Me.cmbOverlayUnitsY2.FormattingEnabled = True
        Me.cmbOverlayUnitsY2.Location = New System.Drawing.Point(109, 246)
        Me.cmbOverlayUnitsY2.Name = "cmbOverlayUnitsY2"
        Me.cmbOverlayUnitsY2.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY2.TabIndex = 92
        '
        'cmbOverlayUnitsY1
        '
        Me.cmbOverlayUnitsY1.FormattingEnabled = True
        Me.cmbOverlayUnitsY1.Location = New System.Drawing.Point(109, 192)
        Me.cmbOverlayUnitsY1.Name = "cmbOverlayUnitsY1"
        Me.cmbOverlayUnitsY1.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY1.TabIndex = 91
        '
        'cmbOverlayUnitsX
        '
        Me.cmbOverlayUnitsX.FormattingEnabled = True
        Me.cmbOverlayUnitsX.Location = New System.Drawing.Point(109, 138)
        Me.cmbOverlayUnitsX.Name = "cmbOverlayUnitsX"
        Me.cmbOverlayUnitsX.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsX.TabIndex = 90
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.Location = New System.Drawing.Point(0, 209)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(42, 13)
        Me.Label48.TabIndex = 89
        Me.Label48.Text = "Y2 Axis"
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(0, 155)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(42, 13)
        Me.Label47.TabIndex = 88
        Me.Label47.Text = "Y1 Axis"
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(0, 101)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(36, 13)
        Me.Label46.TabIndex = 87
        Me.Label46.Text = "X Axis"
        '
        'cmbOverlayDataY2
        '
        Me.cmbOverlayDataY2.DropDownWidth = 150
        Me.cmbOverlayDataY2.FormattingEnabled = True
        Me.cmbOverlayDataY2.Location = New System.Drawing.Point(3, 225)
        Me.cmbOverlayDataY2.Name = "cmbOverlayDataY2"
        Me.cmbOverlayDataY2.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY2.TabIndex = 86
        '
        'cmbOverlayDataY1
        '
        Me.cmbOverlayDataY1.DropDownWidth = 150
        Me.cmbOverlayDataY1.FormattingEnabled = True
        Me.cmbOverlayDataY1.Location = New System.Drawing.Point(3, 171)
        Me.cmbOverlayDataY1.Name = "cmbOverlayDataY1"
        Me.cmbOverlayDataY1.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY1.TabIndex = 85
        '
        'cmbOverlayDataX
        '
        Me.cmbOverlayDataX.DropDownWidth = 150
        Me.cmbOverlayDataX.FormattingEnabled = True
        Me.cmbOverlayDataX.Location = New System.Drawing.Point(3, 117)
        Me.cmbOverlayDataX.Name = "cmbOverlayDataX"
        Me.cmbOverlayDataX.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataX.TabIndex = 84
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.Location = New System.Drawing.Point(65, 387)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(92, 13)
        Me.Label51.TabIndex = 100
        Me.Label51.Text = "Corr. Speed Units"
        '
        'cmbOverlayCorrectedSpeedUnits
        '
        Me.cmbOverlayCorrectedSpeedUnits.FormattingEnabled = True
        Me.cmbOverlayCorrectedSpeedUnits.Location = New System.Drawing.Point(109, 403)
        Me.cmbOverlayCorrectedSpeedUnits.Name = "cmbOverlayCorrectedSpeedUnits"
        Me.cmbOverlayCorrectedSpeedUnits.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayCorrectedSpeedUnits.TabIndex = 99
        '
        'cmbOverlayUnitsY4
        '
        Me.cmbOverlayUnitsY4.FormattingEnabled = True
        Me.cmbOverlayUnitsY4.Location = New System.Drawing.Point(109, 354)
        Me.cmbOverlayUnitsY4.Name = "cmbOverlayUnitsY4"
        Me.cmbOverlayUnitsY4.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY4.TabIndex = 98
        '
        'cmbOverlayUnitsY3
        '
        Me.cmbOverlayUnitsY3.FormattingEnabled = True
        Me.cmbOverlayUnitsY3.Location = New System.Drawing.Point(109, 300)
        Me.cmbOverlayUnitsY3.Name = "cmbOverlayUnitsY3"
        Me.cmbOverlayUnitsY3.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY3.TabIndex = 97
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.Location = New System.Drawing.Point(0, 317)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(42, 13)
        Me.Label50.TabIndex = 96
        Me.Label50.Text = "Y4 Axis"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(0, 263)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(42, 13)
        Me.Label49.TabIndex = 95
        Me.Label49.Text = "Y3 Axis"
        '
        'cmbOverlayDataY4
        '
        Me.cmbOverlayDataY4.DropDownWidth = 150
        Me.cmbOverlayDataY4.FormattingEnabled = True
        Me.cmbOverlayDataY4.Location = New System.Drawing.Point(3, 333)
        Me.cmbOverlayDataY4.Name = "cmbOverlayDataY4"
        Me.cmbOverlayDataY4.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY4.TabIndex = 94
        '
        'cmbOverlayDataY3
        '
        Me.cmbOverlayDataY3.DropDownWidth = 150
        Me.cmbOverlayDataY3.FormattingEnabled = True
        Me.cmbOverlayDataY3.Location = New System.Drawing.Point(3, 279)
        Me.cmbOverlayDataY3.Name = "cmbOverlayDataY3"
        Me.cmbOverlayDataY3.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY3.TabIndex = 93
        '
        'lblCurrentXValue
        '
        Me.lblCurrentXValue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentXValue.Location = New System.Drawing.Point(85, 445)
        Me.lblCurrentXValue.Name = "lblCurrentXValue"
        Me.lblCurrentXValue.Size = New System.Drawing.Size(72, 13)
        Me.lblCurrentXValue.TabIndex = 102
        Me.lblCurrentXValue.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(0, 445)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(85, 13)
        Me.Label9.TabIndex = 101
        Me.Label9.Text = "Current X value "
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'pnlDataOverlay
        '
        Me.pnlDataOverlay.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.pnlDataOverlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlDataOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDataOverlay.Location = New System.Drawing.Point(160, 3)
        Me.pnlDataOverlay.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlDataOverlay.Name = "pnlDataOverlay"
        Me.pnlDataOverlay.Size = New System.Drawing.Size(406, 294)
        Me.pnlDataOverlay.TabIndex = 103
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(72, 141)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Units"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(72, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 13)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Units"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(72, 249)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 106
        Me.Label3.Text = "Units"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(72, 303)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 107
        Me.Label4.Text = "Units"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(72, 357)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 108
        Me.Label5.Text = "Units"
        '
        'Analysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 555)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnlDataOverlay)
        Me.Controls.Add(Me.lblCurrentXValue)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label51)
        Me.Controls.Add(Me.cmbOverlayCorrectedSpeedUnits)
        Me.Controls.Add(Me.cmbOverlayUnitsY4)
        Me.Controls.Add(Me.cmbOverlayUnitsY3)
        Me.Controls.Add(Me.Label50)
        Me.Controls.Add(Me.Label49)
        Me.Controls.Add(Me.cmbOverlayDataY4)
        Me.Controls.Add(Me.cmbOverlayDataY3)
        Me.Controls.Add(Me.cmbOverlayUnitsY2)
        Me.Controls.Add(Me.cmbOverlayUnitsY1)
        Me.Controls.Add(Me.cmbOverlayUnitsX)
        Me.Controls.Add(Me.Label48)
        Me.Controls.Add(Me.Label47)
        Me.Controls.Add(Me.Label46)
        Me.Controls.Add(Me.cmbOverlayDataY2)
        Me.Controls.Add(Me.cmbOverlayDataY1)
        Me.Controls.Add(Me.cmbOverlayDataX)
        Me.Controls.Add(Me.btnAddOverlayFile)
        Me.Controls.Add(Me.btnClearOverlay)
        Me.Controls.Add(Me.btnSaveOverlay)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Analysis"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Analysis"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAddOverlayFile As System.Windows.Forms.Button
    Friend WithEvents btnClearOverlay As System.Windows.Forms.Button
    Friend WithEvents btnSaveOverlay As System.Windows.Forms.Button
    Friend WithEvents cmbOverlayUnitsY2 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayUnitsY1 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayUnitsX As System.Windows.Forms.ComboBox
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents cmbOverlayDataY2 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayDataY1 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayDataX As System.Windows.Forms.ComboBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents cmbOverlayCorrectedSpeedUnits As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayUnitsY4 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayUnitsY3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents cmbOverlayDataY4 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOverlayDataY3 As System.Windows.Forms.ComboBox
    Friend WithEvents lblCurrentXValue As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents pnlDataOverlay As SimpleDyno.DoubleBufferPanel
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
