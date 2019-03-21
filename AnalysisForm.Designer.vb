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
        Me.pnlDataOverlay = New SimpleDyno.DoubleBufferPanel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.clbFiles = New System.Windows.Forms.CheckedListBox()
        Me.PlotView1 = New OxyPlot.WindowsForms.PlotView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblXTitle = New System.Windows.Forms.Label()
        Me.lblY1Title = New System.Windows.Forms.Label()
        Me.lblY2Title = New System.Windows.Forms.Label()
        Me.lblY3Title = New System.Windows.Forms.Label()
        Me.lblY1Unit = New System.Windows.Forms.Label()
        Me.lblXUnit = New System.Windows.Forms.Label()
        Me.lblY2Unit = New System.Windows.Forms.Label()
        Me.lblY3Unit = New System.Windows.Forms.Label()
        Me.lblFile1 = New System.Windows.Forms.Label()
        Me.lblXMax1 = New System.Windows.Forms.Label()
        Me.lblY1Max1 = New System.Windows.Forms.Label()
        Me.lblY2Max1 = New System.Windows.Forms.Label()
        Me.lblY3Max1 = New System.Windows.Forms.Label()
        Me.lblFile2 = New System.Windows.Forms.Label()
        Me.lblXMax2 = New System.Windows.Forms.Label()
        Me.lblY1Max2 = New System.Windows.Forms.Label()
        Me.lblY2Max2 = New System.Windows.Forms.Label()
        Me.lblY3Max2 = New System.Windows.Forms.Label()
        Me.lblFile3 = New System.Windows.Forms.Label()
        Me.lblXMax3 = New System.Windows.Forms.Label()
        Me.lblY1Max3 = New System.Windows.Forms.Label()
        Me.lblY2Max3 = New System.Windows.Forms.Label()
        Me.lblY3Max3 = New System.Windows.Forms.Label()
        Me.lblFile4 = New System.Windows.Forms.Label()
        Me.lblXMax4 = New System.Windows.Forms.Label()
        Me.lblY1Max4 = New System.Windows.Forms.Label()
        Me.lblY2Max4 = New System.Windows.Forms.Label()
        Me.lblY3Max4 = New System.Windows.Forms.Label()
        Me.lblFile5 = New System.Windows.Forms.Label()
        Me.lblXMax5 = New System.Windows.Forms.Label()
        Me.lblY1Max5 = New System.Windows.Forms.Label()
        Me.lblY2Max5 = New System.Windows.Forms.Label()
        Me.lblY3Max5 = New System.Windows.Forms.Label()
        Me.lblY4Title = New System.Windows.Forms.Label()
        Me.lblY4Unit = New System.Windows.Forms.Label()
        Me.lblY4Max1 = New System.Windows.Forms.Label()
        Me.lblY4Max2 = New System.Windows.Forms.Label()
        Me.lblY4Max3 = New System.Windows.Forms.Label()
        Me.lblY4Max4 = New System.Windows.Forms.Label()
        Me.lblY4Max5 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
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
        Me.cmbOverlayUnitsY2.Location = New System.Drawing.Point(120, 486)
        Me.cmbOverlayUnitsY2.Name = "cmbOverlayUnitsY2"
        Me.cmbOverlayUnitsY2.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY2.TabIndex = 5
        '
        'cmbOverlayUnitsY1
        '
        Me.cmbOverlayUnitsY1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY1.FormattingEnabled = True
        Me.cmbOverlayUnitsY1.Location = New System.Drawing.Point(120, 419)
        Me.cmbOverlayUnitsY1.Name = "cmbOverlayUnitsY1"
        Me.cmbOverlayUnitsY1.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY1.TabIndex = 3
        '
        'cmbOverlayUnitsX
        '
        Me.cmbOverlayUnitsX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsX.FormattingEnabled = True
        Me.cmbOverlayUnitsX.Location = New System.Drawing.Point(120, 352)
        Me.cmbOverlayUnitsX.Name = "cmbOverlayUnitsX"
        Me.cmbOverlayUnitsX.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsX.TabIndex = 1
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.Location = New System.Drawing.Point(11, 443)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(46, 13)
        Me.Label48.TabIndex = 89
        Me.Label48.Text = "Y2 Axis:"
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(11, 376)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(46, 13)
        Me.Label47.TabIndex = 88
        Me.Label47.Text = "Y1 Axis:"
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(11, 309)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(40, 13)
        Me.Label46.TabIndex = 87
        Me.Label46.Text = "X Axis:"
        '
        'cmbOverlayDataY2
        '
        Me.cmbOverlayDataY2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY2.DropDownWidth = 150
        Me.cmbOverlayDataY2.FormattingEnabled = True
        Me.cmbOverlayDataY2.Location = New System.Drawing.Point(14, 459)
        Me.cmbOverlayDataY2.Name = "cmbOverlayDataY2"
        Me.cmbOverlayDataY2.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY2.TabIndex = 4
        '
        'cmbOverlayDataY1
        '
        Me.cmbOverlayDataY1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY1.DropDownWidth = 150
        Me.cmbOverlayDataY1.FormattingEnabled = True
        Me.cmbOverlayDataY1.Location = New System.Drawing.Point(14, 392)
        Me.cmbOverlayDataY1.Name = "cmbOverlayDataY1"
        Me.cmbOverlayDataY1.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY1.TabIndex = 2
        '
        'cmbOverlayDataX
        '
        Me.cmbOverlayDataX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataX.DropDownWidth = 150
        Me.cmbOverlayDataX.FormattingEnabled = True
        Me.cmbOverlayDataX.Location = New System.Drawing.Point(14, 325)
        Me.cmbOverlayDataX.Name = "cmbOverlayDataX"
        Me.cmbOverlayDataX.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataX.TabIndex = 0
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.Location = New System.Drawing.Point(76, 653)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(96, 13)
        Me.Label51.TabIndex = 100
        Me.Label51.Text = "Corr. Speed Units:"
        '
        'cmbOverlayCorrectedSpeedUnits
        '
        Me.cmbOverlayCorrectedSpeedUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayCorrectedSpeedUnits.FormattingEnabled = True
        Me.cmbOverlayCorrectedSpeedUnits.Location = New System.Drawing.Point(120, 669)
        Me.cmbOverlayCorrectedSpeedUnits.Name = "cmbOverlayCorrectedSpeedUnits"
        Me.cmbOverlayCorrectedSpeedUnits.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayCorrectedSpeedUnits.TabIndex = 10
        '
        'cmbOverlayUnitsY4
        '
        Me.cmbOverlayUnitsY4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY4.FormattingEnabled = True
        Me.cmbOverlayUnitsY4.Location = New System.Drawing.Point(120, 620)
        Me.cmbOverlayUnitsY4.Name = "cmbOverlayUnitsY4"
        Me.cmbOverlayUnitsY4.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY4.TabIndex = 9
        '
        'cmbOverlayUnitsY3
        '
        Me.cmbOverlayUnitsY3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayUnitsY3.FormattingEnabled = True
        Me.cmbOverlayUnitsY3.Location = New System.Drawing.Point(120, 553)
        Me.cmbOverlayUnitsY3.Name = "cmbOverlayUnitsY3"
        Me.cmbOverlayUnitsY3.Size = New System.Drawing.Size(48, 21)
        Me.cmbOverlayUnitsY3.TabIndex = 7
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.Location = New System.Drawing.Point(11, 577)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(46, 13)
        Me.Label50.TabIndex = 96
        Me.Label50.Text = "Y4 Axis:"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(11, 510)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(46, 13)
        Me.Label49.TabIndex = 95
        Me.Label49.Text = "Y3 Axis:"
        '
        'cmbOverlayDataY4
        '
        Me.cmbOverlayDataY4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY4.DropDownWidth = 150
        Me.cmbOverlayDataY4.FormattingEnabled = True
        Me.cmbOverlayDataY4.Location = New System.Drawing.Point(14, 593)
        Me.cmbOverlayDataY4.Name = "cmbOverlayDataY4"
        Me.cmbOverlayDataY4.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY4.TabIndex = 8
        '
        'cmbOverlayDataY3
        '
        Me.cmbOverlayDataY3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOverlayDataY3.DropDownWidth = 150
        Me.cmbOverlayDataY3.FormattingEnabled = True
        Me.cmbOverlayDataY3.Location = New System.Drawing.Point(14, 526)
        Me.cmbOverlayDataY3.Name = "cmbOverlayDataY3"
        Me.cmbOverlayDataY3.Size = New System.Drawing.Size(154, 21)
        Me.cmbOverlayDataY3.TabIndex = 6
        '
        'lblCurrentXValue
        '
        Me.lblCurrentXValue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentXValue.Location = New System.Drawing.Point(96, 711)
        Me.lblCurrentXValue.Name = "lblCurrentXValue"
        Me.lblCurrentXValue.Size = New System.Drawing.Size(72, 13)
        Me.lblCurrentXValue.TabIndex = 102
        Me.lblCurrentXValue.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 711)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 13)
        Me.Label9.TabIndex = 101
        Me.Label9.Text = "Current X value:"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Multiselect = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(83, 355)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Units:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(83, 422)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Units:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(83, 489)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 106
        Me.Label3.Text = "Units:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(86, 556)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 107
        Me.Label4.Text = "Units:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(83, 623)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 108
        Me.Label5.Text = "Units:"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.pnlDataOverlay)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.clbFiles)
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
        'pnlDataOverlay
        '
        Me.pnlDataOverlay.BackColor = System.Drawing.Color.White
        Me.pnlDataOverlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlDataOverlay.Location = New System.Drawing.Point(141, 6)
        Me.pnlDataOverlay.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlDataOverlay.Name = "pnlDataOverlay"
        Me.pnlDataOverlay.Size = New System.Drawing.Size(767, 136)
        Me.pnlDataOverlay.TabIndex = 0
        Me.pnlDataOverlay.TabStop = True
        Me.pnlDataOverlay.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 136)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 13)
        Me.Label6.TabIndex = 110
        Me.Label6.Text = "Select files to display:"
        '
        'clbFiles
        '
        Me.clbFiles.FormattingEnabled = True
        Me.clbFiles.Location = New System.Drawing.Point(12, 152)
        Me.clbFiles.Name = "clbFiles"
        Me.clbFiles.Size = New System.Drawing.Size(154, 154)
        Me.clbFiles.TabIndex = 109
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
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.PlotView1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(181, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.41885!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.58115!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(767, 738)
        Me.TableLayoutPanel1.TabIndex = 109
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 6
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel2.Controls.Add(Me.lblXTitle, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Title, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Title, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Title, 4, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Unit, 2, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblXUnit, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Unit, 3, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Unit, 4, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblFile1, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblXMax1, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Max1, 2, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Max1, 3, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Max1, 4, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblFile2, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblXMax2, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Max2, 2, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Max2, 3, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Max2, 4, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblFile3, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.lblXMax3, 1, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Max3, 2, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Max3, 3, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Max3, 4, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.lblFile4, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.lblXMax4, 1, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Max4, 2, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Max4, 3, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Max4, 4, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.lblFile5, 0, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.lblXMax5, 1, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY1Max5, 2, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY2Max5, 3, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY3Max5, 4, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Title, 5, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Unit, 5, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Max1, 5, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Max2, 5, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Max3, 5, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Max4, 5, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.lblY4Max5, 5, 6)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 7
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(761, 144)
        Me.TableLayoutPanel2.TabIndex = 111
        '
        'lblXTitle
        '
        Me.lblXTitle.AutoSize = True
        Me.lblXTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXTitle.Location = New System.Drawing.Point(129, 0)
        Me.lblXTitle.Name = "lblXTitle"
        Me.lblXTitle.Size = New System.Drawing.Size(120, 20)
        Me.lblXTitle.TabIndex = 0
        Me.lblXTitle.Text = "Label7"
        Me.lblXTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Title
        '
        Me.lblY1Title.AutoSize = True
        Me.lblY1Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Title.Location = New System.Drawing.Point(255, 0)
        Me.lblY1Title.Name = "lblY1Title"
        Me.lblY1Title.Size = New System.Drawing.Size(120, 20)
        Me.lblY1Title.TabIndex = 1
        Me.lblY1Title.Text = "Label8"
        Me.lblY1Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Title
        '
        Me.lblY2Title.AutoSize = True
        Me.lblY2Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Title.Location = New System.Drawing.Point(381, 0)
        Me.lblY2Title.Name = "lblY2Title"
        Me.lblY2Title.Size = New System.Drawing.Size(120, 20)
        Me.lblY2Title.TabIndex = 2
        Me.lblY2Title.Text = "Label10"
        Me.lblY2Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Title
        '
        Me.lblY3Title.AutoSize = True
        Me.lblY3Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Title.Location = New System.Drawing.Point(507, 0)
        Me.lblY3Title.Name = "lblY3Title"
        Me.lblY3Title.Size = New System.Drawing.Size(120, 20)
        Me.lblY3Title.TabIndex = 3
        Me.lblY3Title.Text = "Label11"
        Me.lblY3Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Unit
        '
        Me.lblY1Unit.AutoSize = True
        Me.lblY1Unit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Unit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Unit.Location = New System.Drawing.Point(255, 20)
        Me.lblY1Unit.Name = "lblY1Unit"
        Me.lblY1Unit.Size = New System.Drawing.Size(120, 20)
        Me.lblY1Unit.TabIndex = 5
        Me.lblY1Unit.Text = "Label13"
        Me.lblY1Unit.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblXUnit
        '
        Me.lblXUnit.AutoSize = True
        Me.lblXUnit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXUnit.Location = New System.Drawing.Point(129, 20)
        Me.lblXUnit.Name = "lblXUnit"
        Me.lblXUnit.Size = New System.Drawing.Size(120, 20)
        Me.lblXUnit.TabIndex = 4
        Me.lblXUnit.Text = "Label12"
        Me.lblXUnit.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Unit
        '
        Me.lblY2Unit.AutoSize = True
        Me.lblY2Unit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Unit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Unit.Location = New System.Drawing.Point(381, 20)
        Me.lblY2Unit.Name = "lblY2Unit"
        Me.lblY2Unit.Size = New System.Drawing.Size(120, 20)
        Me.lblY2Unit.TabIndex = 6
        Me.lblY2Unit.Text = "Label14"
        Me.lblY2Unit.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Unit
        '
        Me.lblY3Unit.AutoSize = True
        Me.lblY3Unit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Unit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Unit.Location = New System.Drawing.Point(507, 20)
        Me.lblY3Unit.Name = "lblY3Unit"
        Me.lblY3Unit.Size = New System.Drawing.Size(120, 20)
        Me.lblY3Unit.TabIndex = 7
        Me.lblY3Unit.Text = "Label15"
        Me.lblY3Unit.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFile1
        '
        Me.lblFile1.AutoSize = True
        Me.lblFile1.Location = New System.Drawing.Point(3, 40)
        Me.lblFile1.Name = "lblFile1"
        Me.lblFile1.Size = New System.Drawing.Size(66, 20)
        Me.lblFile1.TabIndex = 8
        Me.lblFile1.Text = "Label16"
        '
        'lblXMax1
        '
        Me.lblXMax1.AutoSize = True
        Me.lblXMax1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXMax1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXMax1.Location = New System.Drawing.Point(129, 40)
        Me.lblXMax1.Name = "lblXMax1"
        Me.lblXMax1.Size = New System.Drawing.Size(120, 20)
        Me.lblXMax1.TabIndex = 9
        Me.lblXMax1.Text = "lblX1Max"
        Me.lblXMax1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Max1
        '
        Me.lblY1Max1.AutoSize = True
        Me.lblY1Max1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Max1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Max1.Location = New System.Drawing.Point(255, 40)
        Me.lblY1Max1.Name = "lblY1Max1"
        Me.lblY1Max1.Size = New System.Drawing.Size(120, 20)
        Me.lblY1Max1.TabIndex = 10
        Me.lblY1Max1.Text = "Label18"
        Me.lblY1Max1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Max1
        '
        Me.lblY2Max1.AutoSize = True
        Me.lblY2Max1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Max1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Max1.Location = New System.Drawing.Point(381, 40)
        Me.lblY2Max1.Name = "lblY2Max1"
        Me.lblY2Max1.Size = New System.Drawing.Size(120, 20)
        Me.lblY2Max1.TabIndex = 11
        Me.lblY2Max1.Text = "Label19"
        Me.lblY2Max1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Max1
        '
        Me.lblY3Max1.AutoSize = True
        Me.lblY3Max1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Max1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Max1.Location = New System.Drawing.Point(507, 40)
        Me.lblY3Max1.Name = "lblY3Max1"
        Me.lblY3Max1.Size = New System.Drawing.Size(120, 20)
        Me.lblY3Max1.TabIndex = 12
        Me.lblY3Max1.Text = "Label20"
        Me.lblY3Max1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFile2
        '
        Me.lblFile2.AutoSize = True
        Me.lblFile2.Location = New System.Drawing.Point(3, 60)
        Me.lblFile2.Name = "lblFile2"
        Me.lblFile2.Size = New System.Drawing.Size(66, 20)
        Me.lblFile2.TabIndex = 13
        Me.lblFile2.Text = "Label21"
        '
        'lblXMax2
        '
        Me.lblXMax2.AutoSize = True
        Me.lblXMax2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXMax2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXMax2.Location = New System.Drawing.Point(129, 60)
        Me.lblXMax2.Name = "lblXMax2"
        Me.lblXMax2.Size = New System.Drawing.Size(120, 20)
        Me.lblXMax2.TabIndex = 14
        Me.lblXMax2.Text = "Label22"
        Me.lblXMax2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Max2
        '
        Me.lblY1Max2.AutoSize = True
        Me.lblY1Max2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Max2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Max2.Location = New System.Drawing.Point(255, 60)
        Me.lblY1Max2.Name = "lblY1Max2"
        Me.lblY1Max2.Size = New System.Drawing.Size(120, 20)
        Me.lblY1Max2.TabIndex = 15
        Me.lblY1Max2.Text = "Label23"
        Me.lblY1Max2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Max2
        '
        Me.lblY2Max2.AutoSize = True
        Me.lblY2Max2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Max2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Max2.Location = New System.Drawing.Point(381, 60)
        Me.lblY2Max2.Name = "lblY2Max2"
        Me.lblY2Max2.Size = New System.Drawing.Size(120, 20)
        Me.lblY2Max2.TabIndex = 16
        Me.lblY2Max2.Text = "Label24"
        Me.lblY2Max2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Max2
        '
        Me.lblY3Max2.AutoSize = True
        Me.lblY3Max2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Max2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Max2.Location = New System.Drawing.Point(507, 60)
        Me.lblY3Max2.Name = "lblY3Max2"
        Me.lblY3Max2.Size = New System.Drawing.Size(120, 20)
        Me.lblY3Max2.TabIndex = 17
        Me.lblY3Max2.Text = "Label25"
        Me.lblY3Max2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFile3
        '
        Me.lblFile3.AutoSize = True
        Me.lblFile3.Location = New System.Drawing.Point(3, 80)
        Me.lblFile3.Name = "lblFile3"
        Me.lblFile3.Size = New System.Drawing.Size(66, 20)
        Me.lblFile3.TabIndex = 18
        Me.lblFile3.Text = "Label26"
        '
        'lblXMax3
        '
        Me.lblXMax3.AutoSize = True
        Me.lblXMax3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXMax3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXMax3.Location = New System.Drawing.Point(129, 80)
        Me.lblXMax3.Name = "lblXMax3"
        Me.lblXMax3.Size = New System.Drawing.Size(120, 20)
        Me.lblXMax3.TabIndex = 19
        Me.lblXMax3.Text = "Label27"
        Me.lblXMax3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Max3
        '
        Me.lblY1Max3.AutoSize = True
        Me.lblY1Max3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Max3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Max3.Location = New System.Drawing.Point(255, 80)
        Me.lblY1Max3.Name = "lblY1Max3"
        Me.lblY1Max3.Size = New System.Drawing.Size(120, 20)
        Me.lblY1Max3.TabIndex = 20
        Me.lblY1Max3.Text = "Label28"
        Me.lblY1Max3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Max3
        '
        Me.lblY2Max3.AutoSize = True
        Me.lblY2Max3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Max3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Max3.Location = New System.Drawing.Point(381, 80)
        Me.lblY2Max3.Name = "lblY2Max3"
        Me.lblY2Max3.Size = New System.Drawing.Size(120, 20)
        Me.lblY2Max3.TabIndex = 21
        Me.lblY2Max3.Text = "Label29"
        Me.lblY2Max3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Max3
        '
        Me.lblY3Max3.AutoSize = True
        Me.lblY3Max3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Max3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Max3.Location = New System.Drawing.Point(507, 80)
        Me.lblY3Max3.Name = "lblY3Max3"
        Me.lblY3Max3.Size = New System.Drawing.Size(120, 20)
        Me.lblY3Max3.TabIndex = 22
        Me.lblY3Max3.Text = "Label30"
        Me.lblY3Max3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFile4
        '
        Me.lblFile4.AutoSize = True
        Me.lblFile4.Location = New System.Drawing.Point(3, 100)
        Me.lblFile4.Name = "lblFile4"
        Me.lblFile4.Size = New System.Drawing.Size(66, 20)
        Me.lblFile4.TabIndex = 23
        Me.lblFile4.Text = "Label31"
        '
        'lblXMax4
        '
        Me.lblXMax4.AutoSize = True
        Me.lblXMax4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXMax4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXMax4.Location = New System.Drawing.Point(129, 100)
        Me.lblXMax4.Name = "lblXMax4"
        Me.lblXMax4.Size = New System.Drawing.Size(120, 20)
        Me.lblXMax4.TabIndex = 24
        Me.lblXMax4.Text = "Label32"
        Me.lblXMax4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Max4
        '
        Me.lblY1Max4.AutoSize = True
        Me.lblY1Max4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Max4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Max4.Location = New System.Drawing.Point(255, 100)
        Me.lblY1Max4.Name = "lblY1Max4"
        Me.lblY1Max4.Size = New System.Drawing.Size(120, 20)
        Me.lblY1Max4.TabIndex = 25
        Me.lblY1Max4.Text = "Label33"
        Me.lblY1Max4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Max4
        '
        Me.lblY2Max4.AutoSize = True
        Me.lblY2Max4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Max4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Max4.Location = New System.Drawing.Point(381, 100)
        Me.lblY2Max4.Name = "lblY2Max4"
        Me.lblY2Max4.Size = New System.Drawing.Size(120, 20)
        Me.lblY2Max4.TabIndex = 26
        Me.lblY2Max4.Text = "Label34"
        Me.lblY2Max4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Max4
        '
        Me.lblY3Max4.AutoSize = True
        Me.lblY3Max4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Max4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Max4.Location = New System.Drawing.Point(507, 100)
        Me.lblY3Max4.Name = "lblY3Max4"
        Me.lblY3Max4.Size = New System.Drawing.Size(120, 20)
        Me.lblY3Max4.TabIndex = 27
        Me.lblY3Max4.Text = "Label35"
        Me.lblY3Max4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFile5
        '
        Me.lblFile5.AutoSize = True
        Me.lblFile5.Location = New System.Drawing.Point(3, 120)
        Me.lblFile5.Name = "lblFile5"
        Me.lblFile5.Size = New System.Drawing.Size(66, 20)
        Me.lblFile5.TabIndex = 28
        Me.lblFile5.Text = "Label36"
        '
        'lblXMax5
        '
        Me.lblXMax5.AutoSize = True
        Me.lblXMax5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblXMax5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXMax5.Location = New System.Drawing.Point(129, 120)
        Me.lblXMax5.Name = "lblXMax5"
        Me.lblXMax5.Size = New System.Drawing.Size(120, 24)
        Me.lblXMax5.TabIndex = 29
        Me.lblXMax5.Text = "Label37"
        Me.lblXMax5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY1Max5
        '
        Me.lblY1Max5.AutoSize = True
        Me.lblY1Max5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY1Max5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY1Max5.Location = New System.Drawing.Point(255, 120)
        Me.lblY1Max5.Name = "lblY1Max5"
        Me.lblY1Max5.Size = New System.Drawing.Size(120, 24)
        Me.lblY1Max5.TabIndex = 30
        Me.lblY1Max5.Text = "Label38"
        Me.lblY1Max5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY2Max5
        '
        Me.lblY2Max5.AutoSize = True
        Me.lblY2Max5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY2Max5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY2Max5.Location = New System.Drawing.Point(381, 120)
        Me.lblY2Max5.Name = "lblY2Max5"
        Me.lblY2Max5.Size = New System.Drawing.Size(120, 24)
        Me.lblY2Max5.TabIndex = 31
        Me.lblY2Max5.Text = "Label39"
        Me.lblY2Max5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY3Max5
        '
        Me.lblY3Max5.AutoSize = True
        Me.lblY3Max5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY3Max5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY3Max5.Location = New System.Drawing.Point(507, 120)
        Me.lblY3Max5.Name = "lblY3Max5"
        Me.lblY3Max5.Size = New System.Drawing.Size(120, 24)
        Me.lblY3Max5.TabIndex = 32
        Me.lblY3Max5.Text = "Label40"
        Me.lblY3Max5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Title
        '
        Me.lblY4Title.AutoSize = True
        Me.lblY4Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY4Title.Location = New System.Drawing.Point(633, 0)
        Me.lblY4Title.Name = "lblY4Title"
        Me.lblY4Title.Size = New System.Drawing.Size(125, 20)
        Me.lblY4Title.TabIndex = 33
        Me.lblY4Title.Text = "Label7"
        Me.lblY4Title.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Unit
        '
        Me.lblY4Unit.AutoSize = True
        Me.lblY4Unit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Unit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY4Unit.Location = New System.Drawing.Point(633, 20)
        Me.lblY4Unit.Name = "lblY4Unit"
        Me.lblY4Unit.Size = New System.Drawing.Size(125, 20)
        Me.lblY4Unit.TabIndex = 34
        Me.lblY4Unit.Text = "Label8"
        Me.lblY4Unit.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Max1
        '
        Me.lblY4Max1.AutoSize = True
        Me.lblY4Max1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Max1.Location = New System.Drawing.Point(633, 40)
        Me.lblY4Max1.Name = "lblY4Max1"
        Me.lblY4Max1.Size = New System.Drawing.Size(125, 20)
        Me.lblY4Max1.TabIndex = 35
        Me.lblY4Max1.Text = "Label10"
        Me.lblY4Max1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Max2
        '
        Me.lblY4Max2.AutoSize = True
        Me.lblY4Max2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Max2.Location = New System.Drawing.Point(633, 60)
        Me.lblY4Max2.Name = "lblY4Max2"
        Me.lblY4Max2.Size = New System.Drawing.Size(125, 20)
        Me.lblY4Max2.TabIndex = 36
        Me.lblY4Max2.Text = "Label11"
        Me.lblY4Max2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Max3
        '
        Me.lblY4Max3.AutoSize = True
        Me.lblY4Max3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Max3.Location = New System.Drawing.Point(633, 80)
        Me.lblY4Max3.Name = "lblY4Max3"
        Me.lblY4Max3.Size = New System.Drawing.Size(125, 20)
        Me.lblY4Max3.TabIndex = 37
        Me.lblY4Max3.Text = "Label12"
        Me.lblY4Max3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Max4
        '
        Me.lblY4Max4.AutoSize = True
        Me.lblY4Max4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Max4.Location = New System.Drawing.Point(633, 100)
        Me.lblY4Max4.Name = "lblY4Max4"
        Me.lblY4Max4.Size = New System.Drawing.Size(125, 20)
        Me.lblY4Max4.TabIndex = 38
        Me.lblY4Max4.Text = "Label13"
        Me.lblY4Max4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblY4Max5
        '
        Me.lblY4Max5.AutoSize = True
        Me.lblY4Max5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY4Max5.Location = New System.Drawing.Point(633, 120)
        Me.lblY4Max5.Name = "lblY4Max5"
        Me.lblY4Max5.Size = New System.Drawing.Size(125, 24)
        Me.lblY4Max5.TabIndex = 39
        Me.lblY4Max5.Text = "Label14"
        Me.lblY4Max5.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
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
    Friend WithEvents Label6 As Label
    Friend WithEvents clbFiles As CheckedListBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents lblXTitle As Label
    Friend WithEvents lblY1Title As Label
    Friend WithEvents lblY2Title As Label
    Friend WithEvents lblY3Title As Label
    Friend WithEvents lblY1Unit As Label
    Friend WithEvents lblXUnit As Label
    Friend WithEvents lblY2Unit As Label
    Friend WithEvents lblY3Unit As Label
    Friend WithEvents lblFile1 As Label
    Friend WithEvents lblXMax1 As Label
    Friend WithEvents lblY1Max1 As Label
    Friend WithEvents lblY2Max1 As Label
    Friend WithEvents lblY3Max1 As Label
    Friend WithEvents lblFile2 As Label
    Friend WithEvents lblXMax2 As Label
    Friend WithEvents lblY1Max2 As Label
    Friend WithEvents lblY2Max2 As Label
    Friend WithEvents lblY3Max2 As Label
    Friend WithEvents lblFile3 As Label
    Friend WithEvents lblXMax3 As Label
    Friend WithEvents lblY1Max3 As Label
    Friend WithEvents lblY2Max3 As Label
    Friend WithEvents lblY3Max3 As Label
    Friend WithEvents lblFile4 As Label
    Friend WithEvents lblXMax4 As Label
    Friend WithEvents lblY1Max4 As Label
    Friend WithEvents lblY2Max4 As Label
    Friend WithEvents lblY3Max4 As Label
    Friend WithEvents lblFile5 As Label
    Friend WithEvents lblXMax5 As Label
    Friend WithEvents lblY1Max5 As Label
    Friend WithEvents lblY2Max5 As Label
    Friend WithEvents lblY3Max5 As Label
    Friend WithEvents lblY4Title As Label
    Friend WithEvents lblY4Unit As Label
    Friend WithEvents lblY4Max1 As Label
    Friend WithEvents lblY4Max2 As Label
    Friend WithEvents lblY4Max3 As Label
    Friend WithEvents lblY4Max4 As Label
    Friend WithEvents lblY4Max5 As Label
End Class
