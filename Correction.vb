Public Class Correction
    Public Shared RunDownOpenFileDialog As OpenFileDialog
    Public Shared RunDownFileInput As IO.StreamReader
    Public Shared blnUsingLoadedRunDownFile As Boolean = False

    Private Sub chkUseRunDown_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseRunDown.CheckedChanged
        If chkUseRunDown.Checked = True Then
            grpRunDown.Enabled = True
            'Main.frmFit.cmbWhichRDFit.Enabled = True
            Main.frmFit.rdoRunDown.Enabled = True
        Else
            grpRunDown.Enabled = False
            Main.frmFit.cmbWhichRDFit.Enabled = False
            Main.frmFit.rdoRunDown.Enabled = False
            blnUsingLoadedRunDownFile = False
            lblCoastDownFile.Text = "No file loaded"
        End If
    End Sub
    Private Sub chkUseCoastDownFile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseCoastDownFile.CheckedChanged
        If chkUseCoastDownFile.Checked = True Then
            btnLoadCoastDown.Enabled = True
            rdoFreeRoller.Enabled = False
            rdoRollerAndWheel.Enabled = False
            rdoRollerAndDrivetrain.Enabled = False
        Else
            btnLoadCoastDown.Enabled = False
            blnUsingLoadedRunDownFile = False
            rdoFreeRoller.Enabled = True
            rdoRollerAndWheel.Enabled = True
            rdoRollerAndDrivetrain.Enabled = True
            chkUseRunDown_CheckedChanged(Me, EventArgs.Empty)
        End If
    End Sub
    Private Sub Correction_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Prevents form from actually closing, rather it hides
        If e.CloseReason <> CloseReason.FormOwnerClosing Then
            If chkUseCoastDownFile.Checked AndAlso blnUsingLoadedRunDownFile = False Then
                MsgBox("No coast down file was selected. Please confirm your coast down selection", vbOKOnly)
                e.Cancel = True
            Else
                Me.Hide()
                e.Cancel = True
                Main.btnShow_Click(Me, System.EventArgs.Empty)
            End If
        End If
    End Sub

    Private Sub btnLoadCoastDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadCoastDown.Click
   
        RunDownOpenFileDialog = New OpenFileDialog

        With RunDownOpenFileDialog
            .Reset()
            .Filter = "Power Run files v6.3 (*.sdp)|*.sdp"
            .ShowDialog()
        End With

        Dim Temp As String = "", TempSplit() As String

        If RunDownOpenFileDialog.FileName <> "" Then
            RunDownFileInput = New System.IO.StreamReader(RunDownOpenFileDialog.FileName)
            With RunDownFileInput
                Do Until Temp.StartsWith("Coast_Down?_Roller?_Wheel?_Drivetrain?:") Or RunDownFileInput.EndOfStream
                    Temp = .ReadLine
                Loop
                If RunDownFileInput.EndOfStream Then
                    MsgBox("No coast down data found.", MsgBoxStyle.OkOnly)
                    blnUsingLoadedRunDownFile = False
                Else
                    TempSplit = Split(Temp, " ")
                    If UCase(TempSplit(1)) = "FALSE" Then
                        MsgBox("Coast Down not applied to this run", MsgBoxStyle.OkOnly)
                        blnUsingLoadedRunDownFile = False
                    Else
                        If UCase(TempSplit(2)) = "TRUE" Then
                            rdoFreeRoller.Checked = True
                            MsgBox("Free Roller Coast Down data found.", MsgBoxStyle.OkOnly)
                            blnUsingLoadedRunDownFile = True
                            Main.frmFit.rdoRunDown.Enabled = False
                            Main.frmFit.cmbWhichRDFit.Enabled = False
                        ElseIf UCase(TempSplit(3)) = "TRUE" Then
                            rdoRollerAndWheel.Checked = True
                            MsgBox("Free Roller + Wheel Coast Down data found.", MsgBoxStyle.OkOnly)
                            blnUsingLoadedRunDownFile = True
                            Main.frmFit.rdoRunDown.Enabled = False
                            Main.frmFit.cmbWhichRDFit.Enabled = False
                        Else
                            rdoRollerAndDrivetrain.Checked = True
                            MsgBox("Roller + Drivetrain Coast Down data found", MsgBoxStyle.OkOnly)
                            blnUsingLoadedRunDownFile = True
                            Main.frmFit.rdoRunDown.Enabled = False
                            Main.frmFit.cmbWhichRDFit.Enabled = False
                        End If
                        lblCoastDownFile.Text = "Using " & RunDownOpenFileDialog.FileName.ToString.Substring(RunDownOpenFileDialog.FileName.ToString.LastIndexOf("\") + 1)
                    End If
                End If
            End With
            RunDownFileInput.Close()
        Else
            blnUsingLoadedRunDownFile = False
        End If

    End Sub

   
End Class