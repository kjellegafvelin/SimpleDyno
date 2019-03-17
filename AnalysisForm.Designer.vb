<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AnalysisForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnalysisForm))
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PlotView1 = New OxyPlot.WindowsForms.PlotView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlDataOverlay = New SimpleDyno.DoubleBufferPanel()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAddOverlayFile
        '
        Me.btnAddOverlayFile.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddOverlayFile.Location = New System.Drawing.Point(12, 12)
        Me.btnAddOverlayFile.Name = "btnAddOverlayFile"
        Me.btnAddOverlayFile.Size = New System.Drawing.Size(154, 29)
        Me.btnAddOverlayFile.TabIndex = 65
        Me.btnAddOverlayFile.Text = "Load"
        '
        'btnClearOverlay
        '
        Me.btnClearOverlay.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearOverlay.Location = New System.Drawing.Point(12, 47)
        Me.btnClearOverlay.Name = "btnClearOverlay"
        Me.btnClearOverlay.Size = New System.Drawing.Size(154, 29)
        Me.btnClearOverlay.TabIndex = 66
        Me.btnClearOverlay.Text = "Clear"
        '
        'btnSaveOverlay
        '
        Me.btnSaveOverlay.Enabled = False
        Me.btnSaveOverlay.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveOverlay.Location = New System.Drawing.Point(12, 82)
        Me.btnSaveOverlay.Name = "btnSaveOverlay"
        Me.btnSaveOverlay.Size = New System.Drawing.Size(154, 29)
        Me.btnSaveOverlay.TabIndex = 67
        Me.btnSaveOverlay.Text = "Save"
        '
        'cmbOverlayUnitsY2
        '
        Me.cmbOverlayUnitsY2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY2.FormattingEnabled = True
        Me.cmbOverlayUnitsY2.Location = New System.Drawing.Point(118, 307)
        Me.cmbOverlayUnitsY2.Name = "cmbOverlayUnitsY2"
        Me.cmbOverlayUnitsY2.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY2.TabIndex = 5
        '
        'cmbOverlayUnitsY1
        '
        Me.cmbOverlayUnitsY1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY1.FormattingEnabled = True
        Me.cmbOverlayUnitsY1.Location = New System.Drawing.Point(118, 240)
        Me.cmbOverlayUnitsY1.Name = "cmbOverlayUnitsY1"
        Me.cmbOverlayUnitsY1.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY1.TabIndex = 3
        '
        'cmbOverlayUnitsX
        '
        Me.cmbOverlayUnitsX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsX.FormattingEnabled = True
        Me.cmbOverlayUnitsX.Location = New System.Drawing.Point(118, 173)
        Me.cmbOverlayUnitsX.Name = "cmbOverlayUnitsX"
        Me.cmbOverlayUnitsX.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsX.TabIndex = 1
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.Location = New System.Drawing.Point(9, 264)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(42, 13)
        Me.Label48.TabIndex = 89
        Me.Label48.Text = "Y2 Axis"
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(9, 197)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(42, 13)
        Me.Label47.TabIndex = 88
        Me.Label47.Text = "Y1 Axis"
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(9, 130)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(36, 13)
        Me.Label46.TabIndex = 87
        Me.Label46.Text = "X Axis"
        '
        'cmbOverlayDataY2
        '
        Me.cmbOverlayDataY2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY2.DropDownWidth = 150
        Me.cmbOverlayDataY2.FormattingEnabled = True
        Me.cmbOverlayDataY2.Location = New System.Drawing.Point(12, 280)
        Me.cmbOverlayDataY2.Name = "cmbOverlayDataY2"
        Me.cmbOverlayDataY2.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY2.TabIndex = 4
        '
        'cmbOverlayDataY1
        '
        Me.cmbOverlayDataY1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY1.DropDownWidth = 150
        Me.cmbOverlayDataY1.FormattingEnabled = True
        Me.cmbOverlayDataY1.Location = New System.Drawing.Point(12, 213)
        Me.cmbOverlayDataY1.Name = "cmbOverlayDataY1"
        Me.cmbOverlayDataY1.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY1.TabIndex = 2
        '
        'cmbOverlayDataX
        '
        Me.cmbOverlayDataX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataX.DropDownWidth = 150
        Me.cmbOverlayDataX.FormattingEnabled = True
        Me.cmbOverlayDataX.Location = New System.Drawing.Point(12, 146)
        Me.cmbOverlayDataX.Name = "cmbOverlayDataX"
        Me.cmbOverlayDataX.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataX.TabIndex = 0
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.Location = New System.Drawing.Point(74, 474)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(92, 13)
        Me.Label51.TabIndex = 100
        Me.Label51.Text = "Corr. Speed Units"
        '
        'cmbOverlayCorrectedSpeedUnits
        '
        Me.cmbOverlayCorrectedSpeedUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayCorrectedSpeedUnits.FormattingEnabled = True
        Me.cmbOverlayCorrectedSpeedUnits.Location = New System.Drawing.Point(118, 490)
        Me.cmbOverlayCorrectedSpeedUnits.Name = "cmbOverlayCorrectedSpeedUnits"
        Me.cmbOverlayCorrectedSpeedUnits.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayCorrectedSpeedUnits.TabIndex = 10
        '
        'cmbOverlayUnitsY4
        '
        Me.cmbOverlayUnitsY4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY4.FormattingEnabled = True
        Me.cmbOverlayUnitsY4.Location = New System.Drawing.Point(118, 441)
        Me.cmbOverlayUnitsY4.Name = "cmbOverlayUnitsY4"
        Me.cmbOverlayUnitsY4.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY4.TabIndex = 9
        '
        'cmbOverlayUnitsY3
        '
        Me.cmbOverlayUnitsY3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY3.FormattingEnabled = True
        Me.cmbOverlayUnitsY3.Location = New System.Drawing.Point(118, 374)
        Me.cmbOverlayUnitsY3.Name = "cmbOverlayUnitsY3"
        Me.cmbOverlayUnitsY3.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY3.TabIndex = 7
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.Location = New System.Drawing.Point(9, 398)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(42, 13)
        Me.Label50.TabIndex = 96
        Me.Label50.Text = "Y4 Axis"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(9, 331)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(42, 13)
        Me.Label49.TabIndex = 95
        Me.Label49.Text = "Y3 Axis"
        '
        'cmbOverlayDataY4
        '
        Me.cmbOverlayDataY4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY4.DropDownWidth = 150
        Me.cmbOverlayDataY4.FormattingEnabled = True
        Me.cmbOverlayDataY4.Location = New System.Drawing.Point(12, 414)
        Me.cmbOverlayDataY4.Name = "cmbOverlayDataY4"
        Me.cmbOverlayDataY4.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY4.TabIndex = 8
        '
        'cmbOverlayDataY3
        '
        Me.cmbOverlayDataY3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY3.DropDownWidth = 150
        Me.cmbOverlayDataY3.FormattingEnabled = True
        Me.cmbOverlayDataY3.Location = New System.Drawing.Point(12, 347)
        Me.cmbOverlayDataY3.Name = "cmbOverlayDataY3"
        Me.cmbOverlayDataY3.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY3.TabIndex = 6
        '
        'lblCurrentXValue
        '
        Me.lblCurrentXValue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentXValue.Location = New System.Drawing.Point(94, 532)
        Me.lblCurrentXValue.Name = "lblCurrentXValue"
        Me.lblCurrentXValue.Size = New System.Drawing.Size(72, 13)
        Me.lblCurrentXValue.TabIndex = 102
        Me.lblCurrentXValue.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(9, 532)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(85, 13)
        Me.Label9.TabIndex = 101
        Me.Label9.Text = "Current X value "
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(81, 176)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Units"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(81, 243)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 13)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Units"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(81, 310)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 106
        Me.Label3.Text = "Units"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(84, 377)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 107
        Me.Label4.Text = "Units"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(81, 444)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 108
        Me.Label5.Text = "Units"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnAddOverlayFile)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.btnSaveOverlay)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.btnClearOverlay)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cmbOverlayDataX)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cmbOverlayDataY1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbOverlayDataY2)
        Me.Panel1.Controls.Add(Me.lblCurrentXValue)
        Me.Panel1.Controls.Add(Me.Label46)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label47)
        Me.Panel1.Controls.Add(Me.Label51)
        Me.Panel1.Controls.Add(Me.Label48)
        Me.Panel1.Controls.Add(Me.cmbOverlayCorrectedSpeedUnits)
        Me.Panel1.Controls.Add(Me.cmbOverlayUnitsX)
        Me.Panel1.Controls.Add(Me.cmbOverlayUnitsY4)
        Me.Panel1.Controls.Add(Me.cmbOverlayUnitsY1)
        Me.Panel1.Controls.Add(Me.cmbOverlayUnitsY3)
        Me.Panel1.Controls.Add(Me.cmbOverlayUnitsY2)
        Me.Panel1.Controls.Add(Me.Label50)
        Me.Panel1.Controls.Add(Me.cmbOverlayDataY3)
        Me.Panel1.Controls.Add(Me.Label49)
        Me.Panel1.Controls.Add(Me.cmbOverlayDataY4)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(181, 738)
        Me.Panel1.TabIndex = 110
        '
        'PlotView1
        '
        Me.PlotView1.BackColor = System.Drawing.Color.White
        Me.PlotView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PlotView1.Location = New System.Drawing.Point(3, 153)
        Me.PlotView1.Name = "PlotView1"
        Me.PlotView1.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.PlotView1.Size = New System.Drawing.Size(761, 582)
        Me.PlotView1.TabIndex = 110
        Me.PlotView1.Text = "PlotView1"
        Me.PlotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.PlotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.PlotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PlotView1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlDataOverlay, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(181, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.41885!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.58115!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(767, 738)
        Me.TableLayoutPanel1.TabIndex = 109
        '
        'pnlDataOverlay
        '
        Me.pnlDataOverlay.BackColor = System.Drawing.Color.White
        Me.pnlDataOverlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlDataOverlay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDataOverlay.Location = New System.Drawing.Point(0, 0)
        Me.pnlDataOverlay.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlDataOverlay.Name = "pnlDataOverlay"
        Me.pnlDataOverlay.Size = New System.Drawing.Size(767, 150)
        Me.pnlDataOverlay.TabIndex = 0
        Me.pnlDataOverlay.TabStop = True
        '
        'AnalysisForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(948, 738)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AnalysisForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Analysis"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

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
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents pnlDataOverlay As DoubleBufferPanel
    Friend WithEvents PlotView1 As OxyPlot.WindowsForms.PlotView
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
End Class
