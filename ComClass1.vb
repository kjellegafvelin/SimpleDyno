<ComClass(ComClass1.ClassId, ComClass1.InterfaceId, ComClass1.EventsId)> _
Public Class ComClass1

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "2690ABCF-6F75-497F-BCA7-B5319FEFCDE7"
    Public Const InterfaceId As String = "A1381D0D-D63E-42D4-ACD6-151574E6D76F"
    Public Const EventsId As String = "FF62D007-A347-4871-B0A0-714BEA1DB168"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

End Class


