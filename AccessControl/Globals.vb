Module Globals
    Public ControllerDB As DataTable
    Public SQLControllerDB As DataTable
    Public DoorDB As DataTable

    Public ClientID As String = Guid.NewGuid().ToString()

    Public ConString As String = Nothing
End Module