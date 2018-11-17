Public Class Form2
    Delegate Sub SetControlText_Delegate(ByVal [Control] As Control, ByVal [text] As String)
    Private Sub SetControlText_Threadsafe(ByVal [Control] As Control, ByVal [text] As String)
        If [Control].InvokeRequired Then
            Dim MyDelegate As New SetControlText_Delegate(AddressOf SetControlText_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[Control], [text]})
        Else
            [Control].Text = [text]
        End If
    End Sub
    Delegate Sub SetGaugeValue_Delegate(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
    Private Sub SetGaugeValue_Threadsafe(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
        If [AGauge].InvokeRequired Then
            Dim MyDelegate As New SetGaugeValue_Delegate(AddressOf SetGaugeValue_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[AGauge], [Value]})
        Else
            [AGauge].Value = [Value]
        End If
    End Sub
    Delegate Sub SetGaugeMax_Delegate(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
    Private Sub SetGaugeMax_Threadsafe(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
        If [AGauge].InvokeRequired Then
            Dim MyDelegate As New SetGaugeMax_Delegate(AddressOf SetGaugeMax_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[AGauge], [Value]})
        Else
            AGauge.MaxValue = [Value]
        End If
    End Sub
    Delegate Sub SetGaugeRange_Delegate(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
    Private Sub SetGaugeRange_Threadsafe(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
        If [AGauge].InvokeRequired Then
            Dim MyDelegate As New SetGaugeRange_Delegate(AddressOf SetGaugeRange_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[AGauge], [Value]})
        Else
            AGauge.RangeEndValue = [Value]
        End If
    End Sub
    Delegate Sub SetGaugeInterval_Delegate(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
    Private Sub SetGaugeInterval_Threadsafe(ByVal [AGauge] As AGauge.AGauge, ByVal [Value] As Single)
        If [AGauge].InvokeRequired Then
            Dim MyDelegate As New SetGaugeInterval_Delegate(AddressOf SetGaugeInterval_Threadsafe)
            Me.Invoke(MyDelegate, New Object() {[AGauge], [Value]})
        Else
            AGauge.ScaleLinesMajorStepValue = [Value]
        End If
    End Sub
    Public Sub UpdateLabel(ByVal sent As String)
        SetControlText_Threadsafe(Label1, sent)
        SetGaugeValue_Threadsafe(AGauge1, CSng(sent))
    End Sub
    Public Sub UpdateMax(ByVal sent As Single)
        SetGaugeMax_Threadsafe(AGauge1, sent)
    End Sub
    Public Sub UpdateRange(ByVal sent As Single)
        SetGaugeRange_Threadsafe(AGauge1, sent)
    End Sub
    Public Sub UpdateInterval(ByVal sent As Single)
        SetGaugeInterval_Threadsafe(AGauge1, sent)
    End Sub
    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Form1.ShowingGauge = False
    End Sub
End Class