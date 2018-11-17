<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Correction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Correction))
        Me.chkUseRunDown = New System.Windows.Forms.CheckBox()
        Me.grpRunDown = New System.Windows.Forms.GroupBox()
        Me.rdoRollerAndWheel = New System.Windows.Forms.RadioButton()
        Me.chkUseCoastDownFile = New System.Windows.Forms.CheckBox()
        Me.lblCoastDownFile = New System.Windows.Forms.Label()
        Me.btnLoadCoastDown = New System.Windows.Forms.Button()
        Me.rdoRollerAndDrivetrain = New System.Windows.Forms.RadioButton()
        Me.rdoFreeRoller = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.grpRunDown.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkUseRunDown
        '
        Me.chkUseRunDown.AutoSize = True
        Me.chkUseRunDown.Location = New System.Drawing.Point(15, 12)
        Me.chkUseRunDown.Name = "chkUseRunDown"
        Me.chkUseRunDown.Size = New System.Drawing.Size(106, 17)
        Me.chkUseRunDown.TabIndex = 0
        Me.chkUseRunDown.Text = "Use Coast Down"
        Me.chkUseRunDown.UseVisualStyleBackColor = True
        '
        'grpRunDown
        '
        Me.grpRunDown.Controls.Add(Me.rdoRollerAndWheel)
        Me.grpRunDown.Controls.Add(Me.chkUseCoastDownFile)
        Me.grpRunDown.Controls.Add(Me.lblCoastDownFile)
        Me.grpRunDown.Controls.Add(Me.btnLoadCoastDown)
        Me.grpRunDown.Controls.Add(Me.rdoRollerAndDrivetrain)
        Me.grpRunDown.Controls.Add(Me.rdoFreeRoller)
        Me.grpRunDown.Enabled = False
        Me.grpRunDown.Location = New System.Drawing.Point(12, 35)
        Me.grpRunDown.Name = "grpRunDown"
        Me.grpRunDown.Size = New System.Drawing.Size(200, 153)
        Me.grpRunDown.TabIndex = 1
        Me.grpRunDown.TabStop = False
        Me.grpRunDown.Text = "Coast Down Options"
        '
        'rdoRollerAndWheel
        '
        Me.rdoRollerAndWheel.AutoSize = True
        Me.rdoRollerAndWheel.Location = New System.Drawing.Point(13, 40)
        Me.rdoRollerAndWheel.Name = "rdoRollerAndWheel"
        Me.rdoRollerAndWheel.Size = New System.Drawing.Size(95, 17)
        Me.rdoRollerAndWheel.TabIndex = 5
        Me.rdoRollerAndWheel.Text = "Roller + Wheel"
        Me.rdoRollerAndWheel.UseVisualStyleBackColor = True
        '
        'chkUseCoastDownFile
        '
        Me.chkUseCoastDownFile.AutoSize = True
        Me.chkUseCoastDownFile.Location = New System.Drawing.Point(13, 95)
        Me.chkUseCoastDownFile.Name = "chkUseCoastDownFile"
        Me.chkUseCoastDownFile.Size = New System.Drawing.Size(95, 17)
        Me.chkUseCoastDownFile.TabIndex = 4
        Me.chkUseCoastDownFile.Text = "Use saved run"
        Me.chkUseCoastDownFile.UseVisualStyleBackColor = True
        '
        'lblCoastDownFile
        '
        Me.lblCoastDownFile.AutoSize = True
        Me.lblCoastDownFile.Location = New System.Drawing.Point(16, 120)
        Me.lblCoastDownFile.Name = "lblCoastDownFile"
        Me.lblCoastDownFile.Size = New System.Drawing.Size(79, 13)
        Me.lblCoastDownFile.TabIndex = 3
        Me.lblCoastDownFile.Text = "No File Loaded"
        '
        'btnLoadCoastDown
        '
        Me.btnLoadCoastDown.Enabled = False
        Me.btnLoadCoastDown.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadCoastDown.Location = New System.Drawing.Point(120, 92)
        Me.btnLoadCoastDown.Name = "btnLoadCoastDown"
        Me.btnLoadCoastDown.Size = New System.Drawing.Size(74, 21)
        Me.btnLoadCoastDown.TabIndex = 2
        Me.btnLoadCoastDown.Text = "Load"
        Me.btnLoadCoastDown.UseVisualStyleBackColor = True
        '
        'rdoRollerAndDrivetrain
        '
        Me.rdoRollerAndDrivetrain.AutoSize = True
        Me.rdoRollerAndDrivetrain.Location = New System.Drawing.Point(13, 63)
        Me.rdoRollerAndDrivetrain.Name = "rdoRollerAndDrivetrain"
        Me.rdoRollerAndDrivetrain.Size = New System.Drawing.Size(109, 17)
        Me.rdoRollerAndDrivetrain.TabIndex = 1
        Me.rdoRollerAndDrivetrain.Text = "Roller + Drivetrain"
        Me.rdoRollerAndDrivetrain.UseVisualStyleBackColor = True
        '
        'rdoFreeRoller
        '
        Me.rdoFreeRoller.AutoSize = True
        Me.rdoFreeRoller.Checked = True
        Me.rdoFreeRoller.Location = New System.Drawing.Point(13, 19)
        Me.rdoFreeRoller.Name = "rdoFreeRoller"
        Me.rdoFreeRoller.Size = New System.Drawing.Size(76, 17)
        Me.rdoFreeRoller.TabIndex = 0
        Me.rdoFreeRoller.TabStop = True
        Me.rdoFreeRoller.Text = "Free Roller"
        Me.rdoFreeRoller.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton3)
        Me.GroupBox1.Enabled = False
        Me.GroupBox1.Location = New System.Drawing.Point(256, 35)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "STD/SAE Correction Options"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(13, 40)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(114, 17)
        Me.RadioButton1.TabIndex = 2
        Me.RadioButton1.Text = "SAE J1349 JUN90"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(13, 63)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(128, 17)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "SAE J1349 AUG2004"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(13, 19)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(47, 17)
        Me.RadioButton3.TabIndex = 0
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "STD"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Enabled = False
        Me.CheckBox1.Location = New System.Drawing.Point(259, 12)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(147, 17)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "Use STD/SAE Correction"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Correction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 218)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.grpRunDown)
        Me.Controls.Add(Me.chkUseRunDown)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Correction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dyno Correction Factors"
        Me.grpRunDown.ResumeLayout(False)
        Me.grpRunDown.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkUseRunDown As System.Windows.Forms.CheckBox
    Friend WithEvents grpRunDown As System.Windows.Forms.GroupBox
    Friend WithEvents rdoRollerAndDrivetrain As System.Windows.Forms.RadioButton
    Friend WithEvents rdoFreeRoller As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents btnLoadCoastDown As System.Windows.Forms.Button
    Friend WithEvents lblCoastDownFile As System.Windows.Forms.Label
    Friend WithEvents chkUseCoastDownFile As System.Windows.Forms.CheckBox
    Friend WithEvents rdoRollerAndWheel As System.Windows.Forms.RadioButton
End Class
