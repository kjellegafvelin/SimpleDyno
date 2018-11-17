Public Class Dyno
    'Dyno Parameters
    Friend CarMass As Double
    Friend AxleMass As Double
    Friend AxleDiameter As Double
    Friend EndCapMass As Double
    Friend FrontalArea As Double
    Friend DragCoefficient As Double
    Friend SignalsPerRPM As Double
    Friend SignalsPerRPM2 As Double
    Friend WheelDiameter As Double
    Friend RollerDiameter As Double
    Friend RollerCircumference As Double
    Friend RollerWallThickness As Double
    Friend RollerMass As Double
    Friend ExtraDiameter As Double
    Friend ExtraWallThickness As Double
    Friend ExtraMass As Double

    'Dyno Calculations
    Friend IdealMomentOfInertia As Double
    Friend IdealRollerMass As Double

    'Global Temporary Double for Checking the numeric input of the textboxes
    Private TempDouble As Double
    Friend Sub Dyno_Setup()
        txtCarMass.Select()
    End Sub
    Private Sub Dyno_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Prevents form from actually closing, rather it hides
        If e.CloseReason <> CloseReason.FormOwnerClosing Then
            Me.Hide()
            e.Cancel = True
            Main.btnShow_click(Me, System.EventArgs.Empty)
        End If
    End Sub
    Private Sub txtCarMass_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCarMass.Enter
        picDynoSettings.BackgroundImage = My.Resources.CarMass
        lblDynoSettings.Text = _
            "Enter the mass of the car in grams.  This value has NO impact on the accuracy of the dyno results.  " & _
            "The value entered is used in combination with the roller diameter to calculate a target moment of inertia for the rollers.  " & _
            "A dyno that reaches this target value (Target Dyno MOI = 100%) will most closely represent real world conditions for the car.  " & _
            "The actual moment of inertia (Actual Dyno MOI) for your dyno is updated each time you make new entries for roller, end cap, and axle dimensions and weights.  " & _
            "You can use this information to help design your dyno.  If you find that you dyno is 'underweight' you can use additional discs at the end of the rollers or axles " & _
            "(known as 'Extras') to increase the Actual Dyno MOI."
    End Sub
    Private Sub txtCarMass_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCarMass.Leave
        Dim LocalMin As Double = 1
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            CarMass = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = CarMass.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtFrontalArea_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrontalArea.Enter
        picDynoSettings.BackgroundImage = My.Resources.FrontalArea
        lblDynoSettings.Text = _
            "Enter the frontal area of your car in mm squared.  This value has NO impact on the accuracy of the dyno results.  " & _
            "The value is calculated by multiplying the height of the car by the width of the car (both measures in mm).  " & _
            "The frontal area is used in combination with the drag coefficient to calculate power losses due to air resistance.  " & _
            "These losses are calulated and recorded during 'Power Run' sessions and the power losses due to drag cn be plotted.  " & _
            "Where the power output curve and the drag curve intersect provides an estimate of corrected top speed."
    End Sub
    Private Sub txtFrontalArea_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrontalArea.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            FrontalArea = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = FrontalArea.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtDragCoefficient_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDragCoefficient.Enter
        picDynoSettings.BackgroundImage = My.Resources.DragImage
        lblDynoSettings.Text = _
            "Enter your estimate for the drag coefficient for your car.  This value has NO impact on the accuracy of the dyno results.  " & _
            "Typical values will range from 0.5 - 1.0.  " & _
            "The drag coefficient is used in combination with the frontal area  to calculate power losses due to air resistance.  " & _
           "These losses are calulated and recorded during 'Power Run' sessions and the power losses due to drag cn be plotted.  " & _
            "Where the power output curve and the drag curve intersect provides an estimate of corrected top speed."
    End Sub
    Private Sub txtDragCoefficient_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDragCoefficient.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 1
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            DragCoefficient = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = DragCoefficient.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtGearRatio_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGearRatio.Enter
        picDynoSettings.BackgroundImage = My.Resources.GearRatio
        lblDynoSettings.Text = _
            "Enter the gear ratio for your drivetrain.  This value has NO impact on the accuracy of the dyno results.  " & _
            "This number is used to back calculate the RPM of the motor from the RPM of the rollers and wheels.  " & _
            "In a simple drive train, this ratio is the number of spur teeth (A) divided by the number of pinion teeth (B).  " & _
            "This value is typically greater than 1."
    End Sub
    Private Sub txtGearRatio_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGearRatio.Leave
        Dim LocalMin As Double = 0.1
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            Main.GearRatio = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = Main.GearRatio.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtWheelDiameter_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWheelDiameter.Enter
        picDynoSettings.BackgroundImage = My.Resources.WheelDiameter
        lblDynoSettings.Text = _
            "Enter your car's wheel diameter in mm.  This value has NO impact on the accuracy of the dyno results.  " & _
            "The value is used to calculate wheel RPM based on roller RPM and, in conjuction with the gear ratio, " & _
            "the value is further used to calculate  motor RPM. "
    End Sub
    Private Sub txtWheelDiameter_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWheelDiameter.Leave
        Dim LocalMin As Double = 1
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            WheelDiameter = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = WheelDiameter.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtRollerDiameter_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRollerDiameter.Enter
        picDynoSettings.BackgroundImage = My.Resources.RollerDiameter
        lblDynoSettings.Text = _
            "Enter the diameter of your rollers in mm.  This value is CRITICAL for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your rollers and therefore impacts torque and power results.  " & _
            "Additionally, but less critically, this value is used to calculate the speed of your vehicle and the RPM of your wheels and motor.  " & _
            "Note that only the roller diameter (and not the car wheel diameter) is used to calculate vehicle speed"
    End Sub
    Private Sub txtRollerDiameter_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRollerDiameter.Leave
        Dim LocalMin As Double = 1
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            RollerDiameter = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = RollerDiameter.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtRollerWallThickness_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRollerWallThickness.Enter
        picDynoSettings.BackgroundImage = My.Resources.RollerWallThickness
        lblDynoSettings.Text = _
            "Enter the roller wall thickness in mm.  This value is CRITICAL for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your rollers and therefore impacts the torque and power results.  " & _
            "Additionally, based on the assumed design of your rollers, the roller wall thickness is used to calculate the End Cap diameter " & _
            "(i.e. it is assumed that your End Caps fit into the end of your rollers).  Note: If you are using a solid roller, this value should be half of the entered value for Roller Diameter" &
            " This value cannot be greater than 1/2 the value entered for Roller Diameter"
    End Sub
    Private Sub txtRollerWallThickness_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRollerWallThickness.Leave
        Dim LocalMin As Double = 1
        Dim LocalMax As Double = RollerDiameter / 2
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            RollerWallThickness = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = RollerWallThickness.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtRollerMass_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRollerMass.Enter
        picDynoSettings.BackgroundImage = My.Resources.RollerMass
        lblDynoSettings.Text = _
            "Enter the roller mass in grams.  This value is CRITICAL for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your rollers and therefore impacts the torque and power results.  " & _
            "Enter the mass of the driven rollers only.  For example if you have a two roller dyno (one roller per axle) but are testing a rear wheel drive car, " & _
            "only enter the mass of the rear roller.  If you are testing an AWD / 4WD car, enter the combined masses of both rollers."
    End Sub
    Private Sub txtRollerMass_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRollerMass.Leave
        Dim LocalMin As Double = 1
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            RollerMass = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = RollerMass.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtAxleDiameter_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAxleDiameter.Enter
        picDynoSettings.BackgroundImage = My.Resources.AxleDiameter
        lblDynoSettings.Text = _
            "Enter the axle diameter in mm.  This value is IMPORTANT for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your axles and therefore impacts the torque and power results to some extent.  " & _
            "Additionally, based on the assumed design of your rollers, the axle diameter used to calculate the end cap wall thickness."
    End Sub
    Private Sub txtAxleDiameter_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAxleDiameter.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            AxleDiameter = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = AxleDiameter.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtAxleMass_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAxleMass.Enter
        picDynoSettings.BackgroundImage = My.Resources.AxelMass
        lblDynoSettings.Text = _
            "Enter the axle mass in grams.  This value is IMPORTANT for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your axles and therefore impacts the torque and power results.  " & _
            "Note: Only enter a mass for your axle(s) if the axles rotate witht the roller.  If you are using a fixed axle that does not rotate with the roller, enter zero for its mass"
    End Sub
    Private Sub txtAxleMass_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAxleMass.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            AxleMass = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = AxleMass.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtEndCapMass_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEndCapMass.Enter
        picDynoSettings.BackgroundImage = My.Resources.EndCapMass
        lblDynoSettings.Text = _
            "Enter the total mass of the end caps in grams.  This value is CRITICAL for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your end caps and therefore impacts the torque and power results."
    End Sub
    Private Sub txtEndCapMass_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEndCapMass.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            EndCapMass = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = EndCapMass.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtExtraDiameter_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExtraDiameter.Enter
        picDynoSettings.BackgroundImage = My.Resources.ExtraDiameter
        lblDynoSettings.Text = _
            "Enter the extra diameter in mm.  If you are using extra components this value is CRITICAL for accurate dyno results.  " & _
            "This value is used to calculate the moment of inertia of your extra dyno components.  These components are typically disks " & _
            "that you can add to the ends of your rollers to increase the overall moment of inertia.  Extras can be mounted on the axle " & _
            "or attached directly to the ends of the roller / end cap components."
    End Sub
    Private Sub txtExtraDiameter_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExtraDiameter.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            ExtraDiameter = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = ExtraDiameter.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtExtraWallThickness_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExtraWallThickness.Enter
        picDynoSettings.BackgroundImage = My.Resources.ExtraWallThickness
        lblDynoSettings.Text = _
           "Enter the extra wall thickness in mm.  If you are using extra components this value is CRITICAL for accurate dyno results.  " & _
           "This value is used to calculate the moment of inertia of your extra dyno components.  These components are typically disks " & _
           "that you can add to the ends of your rollers to increase the overall moment of inertia.  Extras can be mounted on the axle " & _
           "or attached directly to the ends of the roller / end cap components."
    End Sub
    Private Sub txtExtraWallThickness_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExtraWallThickness.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            ExtraWallThickness = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = ExtraWallThickness.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtExtraMass_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExtraMass.Enter
        picDynoSettings.BackgroundImage = My.Resources.ExtraMass
        lblDynoSettings.Text = _
           "Enter the masses of all the extra components in grams.  If you are using extra components this value is CRITICAL for accurate dyno results.  " & _
           "This value is used to calculate the moment of inertia of your extra dyno components.  These components are typicall disks " & _
           "that you can add to the ends of your rollers to increase the overall moment of inertia.  Extras can be mounted on the axle " & _
           "or attached directly to the ends of the roller / end cap components."
    End Sub
    Private Sub txtExtraMass_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExtraMass.Leave
        Dim LocalMin As Double = 0
        Dim LocalMax As Double = 999999
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            ExtraMass = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = ExtraMass.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtSignalsPerRPM_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSignalsPerRPM1.Enter
        picDynoSettings.BackgroundImage = My.Resources.SignalsPerRPM
        lblDynoSettings.Text = _
           "Enter the number of signals that are produced with each revolution of the roller.  " & _
           "For example, if you are using a coil and magnet system to detect the roller RPM, each magnet attached to the roller " & _
           "will represent one signal per RPM.  Make sure that if you are using multiple magnets that they are are spaced evenly around the " & _
           "circumference of the roller.  Slight positional differences will lead to noise in the RPM values."
    End Sub
    Private Sub txtSignalsPerRPM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSignalsPerRPM1.Leave
        Dim LocalMin As Double = 0.1
        Dim LocalMax As Double = 50
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            SignalsPerRPM = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = SignalsPerRPM.ToString
                .Focus()
            End With
        End If
    End Sub
    Private Sub txtSignalsPerRPM2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSignalsPerRPM2.Enter
        picDynoSettings.BackgroundImage = My.Resources.SignalsPerRPM
        lblDynoSettings.Text = _
           "Enter the number of signals that are produced with each revolution of the component being monitored by the second channel.  " & _
           "This channel can be used to monitor the RPM of an IC engine or some other component of the drivetrain." & _
           "If you are using a spark signal for a four stroke IC engine you can enter 0.5 for this value."
    End Sub
    Private Sub txtSignalsPerRPM2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSignalsPerRPM2.Leave
        Dim LocalMin As Double = 0.1
        Dim LocalMax As Double = 50
        If Double.TryParse(CType(sender, TextBox).Text, TempDouble) AndAlso Main.CheckNumericalLimits(LocalMin, LocalMax, TempDouble) Then
            SignalsPerRPM2 = TempDouble
            UpdateMomentOfInertias()
        Else
            MsgBox(CType(sender, TextBox).Name & " : Value must be between " & LocalMin & " and " & LocalMax, MsgBoxStyle.Exclamation)
            With CType(sender, TextBox)
                .Text = SignalsPerRPM2.ToString
                .Focus()
            End With
        End If
    End Sub
    Friend Sub UpdateMomentOfInertias()
        Dim RollerMomentOfInertia As Double
        Dim AxleMomentOfInertia As Double
        Dim EndCapMomentOfInertia As Double
        Dim ExtraMomentOfInertia As Double

        Dim r1 As Double, r2 As Double, m As Double

        'Using I = 1/2 x m x (r1^2 + r2^2)
        'Roller
        m = RollerMass / 1000.0
        r1 = (RollerDiameter / 2.0 - RollerWallThickness) / 1000.0
        r2 = RollerDiameter / 2.0 / 1000.0
        RollerMomentOfInertia = 1 / 2 * m * (r1 ^ 2 + r2 ^ 2)
        'Axle
        m = AxleMass / 1000.0
        r1 = 0
        r2 = AxleDiameter / 2.0 / 1000.0
        AxleMomentOfInertia = 1 / 2 * m * (r1 ^ 2 + r2 ^ 2)
        'End Cap
        m = EndCapMass / 1000.0
        r1 = AxleDiameter / 2.0 / 1000.0
        r2 = (RollerDiameter / 2.0 - RollerWallThickness) / 1000.0
        EndCapMomentOfInertia = 1 / 2 * m * (r1 ^ 2 + r2 ^ 2)
        'Extras
        m = ExtraMass / 1000.0
        r1 = ExtraDiameter / 2.0 / 1000.0
        r2 = (ExtraDiameter / 2.0 - ExtraWallThickness) / 1000.0
        ExtraMomentOfInertia = 1 / 2 * m * (r1 ^ 2 + r2 ^ 2)
        'Total
        Main.DynoMomentOfInertia = RollerMomentOfInertia + AxleMomentOfInertia + EndCapMomentOfInertia + ExtraMomentOfInertia
        'Ideal Roller Mass
        'Car outputs 1 N force which will give F/m acceleration
        Dim CarAcceleration As Double = 1 / (CarMass / 1000.0) 'm/s^2
        'This equals the angular acceleration of the roller
        Dim RollerAcceleration As Double = CarAcceleration / (RollerDiameter / 1000.0 / 2.0) 'radians/s^2
        'Torque is the same force through the radius of the roller
        Dim RollerTorque As Double = 1 * RollerDiameter / 1000.0 / 2.0
        'Torque is also the moment of inertia by angular accleration
        'Therefore, Torque / angular acceleration = ideal moment of inertia
        IdealMomentOfInertia = RollerTorque / RollerAcceleration
        'r1 and r2 are the same as when calculated for the roller moment of inertia
        r1 = (RollerDiameter / 2.0 - RollerWallThickness) / 1000.0
        r2 = RollerDiameter / 2.0 / 1000.0
        'So (Note - actually not going to use this)
        IdealRollerMass = IdealMomentOfInertia * 2.0 / (r1 ^ 2 + r2 ^ 2) * 1000
        'For Rollout calculations
        RollerCircumference = RollerDiameter * Math.PI 'circumference in mm
        Main.WheelCircumference = WheelDiameter * Math.PI

        'For Wheel and Motor RPM conversions and speed conversion
        Main.RollerRPMtoWheelRPM = RollerDiameter / WheelDiameter
        Main.RollerRPMtoMotorRPM = RollerDiameter / WheelDiameter * Main.GearRatio
        Main.RollerRadsPerSecToMetersPerSec = (RollerCircumference / 1000) / (2 * Math.PI)

        If Main.DynoMomentOfInertia >= 0.0009 Then
            lblActualMomentOfInertia.Text = "Actual Dyno MoI = " & Main.DynoMomentOfInertia.ToString("0.000") & " kg.m^2"
        Else
            lblActualMomentOfInertia.Text = "Actual Dyno MoI = " & (1000 * Main.DynoMomentOfInertia).ToString("0.000") & " g.m^2 "
        End If

        lblTargetMomentOfInertia.Text = "% of Target Dyno MoI = " & (Main.DynoMomentOfInertia / IdealMomentOfInertia * 100).ToString("0.0") & "%"

        lblTargetRollerMass.Text = "Target Roller Mass = " & IdealRollerMass.ToString("0") & " grams"

        'Update the drag coefficient and air resistance
        Main.ForceAir = 0.5 * (FrontalArea / 1000000) * 1.2 * DragCoefficient

        'Update conversions for time to RPM
        Main.ElapsedTimeToRadPerSec = 2 * Math.PI / SignalsPerRPM
        Main.ElapsedTimeToRadPerSec2 = 2 * Math.PI / SignalsPerRPM2

    End Sub

End Class