<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dyno
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dyno))
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtCarMass = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtWheelDiameter = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtGearRatio = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFrontalArea = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtDragCoefficient = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSignalsPerRPM2 = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtSignalsPerRPM1 = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtExtraWallThickness = New System.Windows.Forms.TextBox()
        Me.txtRollerDiameter = New System.Windows.Forms.TextBox()
        Me.txtExtraMass = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.txtRollerWallThickness = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtExtraDiameter = New System.Windows.Forms.TextBox()
        Me.txtRollerMass = New System.Windows.Forms.TextBox()
        Me.txtEndCapMass = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtAxleMass = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.txtAxleDiameter = New System.Windows.Forms.TextBox()
        Me.picDynoSettings = New System.Windows.Forms.PictureBox()
        Me.lblTargetRollerMass = New System.Windows.Forms.Label()
        Me.lblActualMomentOfInertia = New System.Windows.Forms.Label()
        Me.lblTargetMomentOfInertia = New System.Windows.Forms.Label()
        Me.lblDynoSettings = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.picDynoSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(8, 20)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(160, 20)
        Me.Label12.TabIndex = 245
        Me.Label12.Text = "Vehicle Mass (g)"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCarMass
        '
        Me.txtCarMass.CausesValidation = False
        Me.txtCarMass.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCarMass.Location = New System.Drawing.Point(176, 20)
        Me.txtCarMass.Name = "txtCarMass"
        Me.txtCarMass.Size = New System.Drawing.Size(72, 22)
        Me.txtCarMass.TabIndex = 244
        Me.txtCarMass.Tag = ""
        Me.txtCarMass.Text = "1000"
        Me.txtCarMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(88, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(160, 20)
        Me.Label2.TabIndex = 275
        Me.Label2.Text = "Non Critical Parameters"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(8, 121)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(160, 20)
        Me.Label20.TabIndex = 271
        Me.Label20.Text = "Wheel Diameter (mm)"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWheelDiameter
        '
        Me.txtWheelDiameter.CausesValidation = False
        Me.txtWheelDiameter.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWheelDiameter.Location = New System.Drawing.Point(176, 121)
        Me.txtWheelDiameter.Name = "txtWheelDiameter"
        Me.txtWheelDiameter.Size = New System.Drawing.Size(72, 22)
        Me.txtWheelDiameter.TabIndex = 270
        Me.txtWheelDiameter.Tag = ""
        Me.txtWheelDiameter.Text = "100"
        Me.txtWheelDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 97)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 20)
        Me.Label5.TabIndex = 272
        Me.Label5.Text = "Gear Ratio"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtGearRatio
        '
        Me.txtGearRatio.CausesValidation = False
        Me.txtGearRatio.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGearRatio.Location = New System.Drawing.Point(176, 97)
        Me.txtGearRatio.Name = "txtGearRatio"
        Me.txtGearRatio.Size = New System.Drawing.Size(72, 22)
        Me.txtGearRatio.TabIndex = 269
        Me.txtGearRatio.Tag = ""
        Me.txtGearRatio.Text = "1"
        Me.txtGearRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 49)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(160, 20)
        Me.Label6.TabIndex = 273
        Me.Label6.Text = "Frontal Area (mm2)"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFrontalArea
        '
        Me.txtFrontalArea.CausesValidation = False
        Me.txtFrontalArea.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFrontalArea.Location = New System.Drawing.Point(176, 49)
        Me.txtFrontalArea.Name = "txtFrontalArea"
        Me.txtFrontalArea.Size = New System.Drawing.Size(72, 22)
        Me.txtFrontalArea.TabIndex = 267
        Me.txtFrontalArea.Tag = ""
        Me.txtFrontalArea.Text = "1000"
        Me.txtFrontalArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(160, 20)
        Me.Label8.TabIndex = 274
        Me.Label8.Text = "Drag Coeff."
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDragCoefficient
        '
        Me.txtDragCoefficient.CausesValidation = False
        Me.txtDragCoefficient.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDragCoefficient.Location = New System.Drawing.Point(176, 73)
        Me.txtDragCoefficient.Name = "txtDragCoefficient"
        Me.txtDragCoefficient.Size = New System.Drawing.Size(72, 22)
        Me.txtDragCoefficient.TabIndex = 268
        Me.txtDragCoefficient.Tag = ""
        Me.txtDragCoefficient.Text = "1"
        Me.txtDragCoefficient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(88, 164)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 20)
        Me.Label4.TabIndex = 298
        Me.Label4.Text = "Critical Parameters"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSignalsPerRPM2
        '
        Me.txtSignalsPerRPM2.CausesValidation = False
        Me.txtSignalsPerRPM2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSignalsPerRPM2.Location = New System.Drawing.Point(176, 427)
        Me.txtSignalsPerRPM2.Name = "txtSignalsPerRPM2"
        Me.txtSignalsPerRPM2.Size = New System.Drawing.Size(72, 22)
        Me.txtSignalsPerRPM2.TabIndex = 296
        Me.txtSignalsPerRPM2.Tag = ""
        Me.txtSignalsPerRPM2.Text = "1"
        Me.txtSignalsPerRPM2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label24
        '
        Me.Label24.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(37, 427)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(131, 20)
        Me.Label24.TabIndex = 297
        Me.Label24.Text = "Signals per RPM2"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSignalsPerRPM1
        '
        Me.txtSignalsPerRPM1.CausesValidation = False
        Me.txtSignalsPerRPM1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSignalsPerRPM1.Location = New System.Drawing.Point(176, 403)
        Me.txtSignalsPerRPM1.Name = "txtSignalsPerRPM1"
        Me.txtSignalsPerRPM1.Size = New System.Drawing.Size(72, 22)
        Me.txtSignalsPerRPM1.TabIndex = 285
        Me.txtSignalsPerRPM1.Tag = ""
        Me.txtSignalsPerRPM1.Text = "1"
        Me.txtSignalsPerRPM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(72, 403)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(96, 20)
        Me.Label23.TabIndex = 289
        Me.Label23.Text = "Signals per RPM"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtExtraWallThickness
        '
        Me.txtExtraWallThickness.CausesValidation = False
        Me.txtExtraWallThickness.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtraWallThickness.Location = New System.Drawing.Point(176, 355)
        Me.txtExtraWallThickness.Name = "txtExtraWallThickness"
        Me.txtExtraWallThickness.Size = New System.Drawing.Size(72, 22)
        Me.txtExtraWallThickness.TabIndex = 283
        Me.txtExtraWallThickness.Tag = ""
        Me.txtExtraWallThickness.Text = "0"
        Me.txtExtraWallThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRollerDiameter
        '
        Me.txtRollerDiameter.CausesValidation = False
        Me.txtRollerDiameter.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRollerDiameter.Location = New System.Drawing.Point(176, 187)
        Me.txtRollerDiameter.Name = "txtRollerDiameter"
        Me.txtRollerDiameter.Size = New System.Drawing.Size(72, 22)
        Me.txtRollerDiameter.TabIndex = 276
        Me.txtRollerDiameter.Tag = ""
        Me.txtRollerDiameter.Text = "100"
        Me.txtRollerDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtExtraMass
        '
        Me.txtExtraMass.CausesValidation = False
        Me.txtExtraMass.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtraMass.Location = New System.Drawing.Point(176, 379)
        Me.txtExtraMass.Name = "txtExtraMass"
        Me.txtExtraMass.Size = New System.Drawing.Size(72, 22)
        Me.txtExtraMass.TabIndex = 284
        Me.txtExtraMass.Tag = ""
        Me.txtExtraMass.Text = "0"
        Me.txtExtraMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label22
        '
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(8, 235)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(160, 20)
        Me.Label22.TabIndex = 288
        Me.Label22.Text = "Roller Mass (g)"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(8, 357)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(160, 20)
        Me.Label31.TabIndex = 294
        Me.Label31.Text = "Extra Wall Thickness (mm)"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(8, 187)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(160, 20)
        Me.Label19.TabIndex = 286
        Me.Label19.Text = "Roller Diameter (mm)"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(8, 333)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(160, 20)
        Me.Label32.TabIndex = 293
        Me.Label32.Text = "Extra Diameter (mm)"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRollerWallThickness
        '
        Me.txtRollerWallThickness.CausesValidation = False
        Me.txtRollerWallThickness.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRollerWallThickness.Location = New System.Drawing.Point(176, 211)
        Me.txtRollerWallThickness.Name = "txtRollerWallThickness"
        Me.txtRollerWallThickness.Size = New System.Drawing.Size(72, 22)
        Me.txtRollerWallThickness.TabIndex = 277
        Me.txtRollerWallThickness.Tag = ""
        Me.txtRollerWallThickness.Text = "50"
        Me.txtRollerWallThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label33
        '
        Me.Label33.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(8, 381)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(160, 20)
        Me.Label33.TabIndex = 295
        Me.Label33.Text = "Extra Mass (g)"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(8, 211)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(160, 20)
        Me.Label21.TabIndex = 287
        Me.Label21.Text = "Roller Wall Thickness (mm)"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtExtraDiameter
        '
        Me.txtExtraDiameter.CausesValidation = False
        Me.txtExtraDiameter.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtraDiameter.Location = New System.Drawing.Point(176, 331)
        Me.txtExtraDiameter.Name = "txtExtraDiameter"
        Me.txtExtraDiameter.Size = New System.Drawing.Size(72, 22)
        Me.txtExtraDiameter.TabIndex = 282
        Me.txtExtraDiameter.Tag = ""
        Me.txtExtraDiameter.Text = "0"
        Me.txtExtraDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRollerMass
        '
        Me.txtRollerMass.CausesValidation = False
        Me.txtRollerMass.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRollerMass.Location = New System.Drawing.Point(176, 235)
        Me.txtRollerMass.Name = "txtRollerMass"
        Me.txtRollerMass.Size = New System.Drawing.Size(72, 22)
        Me.txtRollerMass.TabIndex = 278
        Me.txtRollerMass.Tag = ""
        Me.txtRollerMass.Text = "1000"
        Me.txtRollerMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtEndCapMass
        '
        Me.txtEndCapMass.CausesValidation = False
        Me.txtEndCapMass.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndCapMass.Location = New System.Drawing.Point(176, 307)
        Me.txtEndCapMass.Name = "txtEndCapMass"
        Me.txtEndCapMass.Size = New System.Drawing.Size(72, 22)
        Me.txtEndCapMass.TabIndex = 281
        Me.txtEndCapMass.Tag = ""
        Me.txtEndCapMass.Text = "0"
        Me.txtEndCapMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(8, 283)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(160, 20)
        Me.Label15.TabIndex = 291
        Me.Label15.Text = "Axle Mass (g)"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAxleMass
        '
        Me.txtAxleMass.CausesValidation = False
        Me.txtAxleMass.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAxleMass.Location = New System.Drawing.Point(176, 283)
        Me.txtAxleMass.Name = "txtAxleMass"
        Me.txtAxleMass.Size = New System.Drawing.Size(72, 22)
        Me.txtAxleMass.TabIndex = 280
        Me.txtAxleMass.Tag = ""
        Me.txtAxleMass.Text = "0"
        Me.txtAxleMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label27
        '
        Me.Label27.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(8, 259)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(160, 20)
        Me.Label27.TabIndex = 290
        Me.Label27.Text = "Axle Diameter (mm)"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label28
        '
        Me.Label28.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(8, 307)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(160, 20)
        Me.Label28.TabIndex = 292
        Me.Label28.Text = "End Cap Mass (g)"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAxleDiameter
        '
        Me.txtAxleDiameter.CausesValidation = False
        Me.txtAxleDiameter.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAxleDiameter.Location = New System.Drawing.Point(176, 259)
        Me.txtAxleDiameter.Name = "txtAxleDiameter"
        Me.txtAxleDiameter.Size = New System.Drawing.Size(72, 22)
        Me.txtAxleDiameter.TabIndex = 279
        Me.txtAxleDiameter.Tag = ""
        Me.txtAxleDiameter.Text = "10"
        Me.txtAxleDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'picDynoSettings
        '
        Me.picDynoSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.picDynoSettings.InitialImage = Nothing
        Me.picDynoSettings.Location = New System.Drawing.Point(263, 20)
        Me.picDynoSettings.Name = "picDynoSettings"
        Me.picDynoSettings.Size = New System.Drawing.Size(669, 383)
        Me.picDynoSettings.TabIndex = 299
        Me.picDynoSettings.TabStop = False
        '
        'lblTargetRollerMass
        '
        Me.lblTargetRollerMass.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTargetRollerMass.Location = New System.Drawing.Point(8, 521)
        Me.lblTargetRollerMass.Name = "lblTargetRollerMass"
        Me.lblTargetRollerMass.Size = New System.Drawing.Size(241, 25)
        Me.lblTargetRollerMass.TabIndex = 302
        Me.lblTargetRollerMass.Text = "Target Roller Mass"
        Me.lblTargetRollerMass.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblActualMomentOfInertia
        '
        Me.lblActualMomentOfInertia.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActualMomentOfInertia.Location = New System.Drawing.Point(8, 472)
        Me.lblActualMomentOfInertia.Name = "lblActualMomentOfInertia"
        Me.lblActualMomentOfInertia.Size = New System.Drawing.Size(241, 24)
        Me.lblActualMomentOfInertia.TabIndex = 301
        Me.lblActualMomentOfInertia.Text = "Actual MOI"
        Me.lblActualMomentOfInertia.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTargetMomentOfInertia
        '
        Me.lblTargetMomentOfInertia.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTargetMomentOfInertia.Location = New System.Drawing.Point(8, 496)
        Me.lblTargetMomentOfInertia.Name = "lblTargetMomentOfInertia"
        Me.lblTargetMomentOfInertia.Size = New System.Drawing.Size(241, 25)
        Me.lblTargetMomentOfInertia.TabIndex = 300
        Me.lblTargetMomentOfInertia.Text = "Target MOI"
        Me.lblTargetMomentOfInertia.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDynoSettings
        '
        Me.lblDynoSettings.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynoSettings.Location = New System.Drawing.Point(260, 409)
        Me.lblDynoSettings.Name = "lblDynoSettings"
        Me.lblDynoSettings.Size = New System.Drawing.Size(672, 131)
        Me.lblDynoSettings.TabIndex = 303
        Me.lblDynoSettings.Text = "Dyno Settings Information"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(89, 452)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(160, 20)
        Me.Label1.TabIndex = 304
        Me.Label1.Text = "Moment of Inertia (MOI)"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Dyno
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 555)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblDynoSettings)
        Me.Controls.Add(Me.lblTargetRollerMass)
        Me.Controls.Add(Me.lblActualMomentOfInertia)
        Me.Controls.Add(Me.lblTargetMomentOfInertia)
        Me.Controls.Add(Me.picDynoSettings)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtSignalsPerRPM2)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.txtSignalsPerRPM1)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.txtExtraWallThickness)
        Me.Controls.Add(Me.txtRollerDiameter)
        Me.Controls.Add(Me.txtExtraMass)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label31)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label32)
        Me.Controls.Add(Me.txtRollerWallThickness)
        Me.Controls.Add(Me.Label33)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.txtExtraDiameter)
        Me.Controls.Add(Me.txtRollerMass)
        Me.Controls.Add(Me.txtEndCapMass)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.txtAxleMass)
        Me.Controls.Add(Me.Label27)
        Me.Controls.Add(Me.Label28)
        Me.Controls.Add(Me.txtAxleDiameter)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.txtWheelDiameter)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtGearRatio)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtFrontalArea)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtDragCoefficient)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtCarMass)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Dyno"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dyno Setup"
        CType(Me.picDynoSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtCarMass As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtWheelDiameter As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtGearRatio As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtFrontalArea As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtDragCoefficient As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSignalsPerRPM2 As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtSignalsPerRPM1 As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtExtraWallThickness As System.Windows.Forms.TextBox
    Friend WithEvents txtRollerDiameter As System.Windows.Forms.TextBox
    Friend WithEvents txtExtraMass As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents txtRollerWallThickness As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtExtraDiameter As System.Windows.Forms.TextBox
    Friend WithEvents txtRollerMass As System.Windows.Forms.TextBox
    Friend WithEvents txtEndCapMass As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtAxleMass As System.Windows.Forms.TextBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents txtAxleDiameter As System.Windows.Forms.TextBox
    Friend WithEvents picDynoSettings As System.Windows.Forms.PictureBox
    Friend WithEvents lblTargetRollerMass As System.Windows.Forms.Label
    Friend WithEvents lblActualMomentOfInertia As System.Windows.Forms.Label
    Friend WithEvents lblTargetMomentOfInertia As System.Windows.Forms.Label
    Friend WithEvents lblDynoSettings As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
