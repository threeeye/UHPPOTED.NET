#Region "Controller Class"
Imports AccessControl.Enums

Public Class ControllerInfo
    Public Property ControllerID As UInteger
    Public Property IPAddress As String
    Public Property Netmask As String
    Public Property Gateway As String
    Public Property MAC As String
    Public Property Listener As String
    Public Property Index As UInteger
    Public Property Version As String
    Public Property ControllerDate As DateTime
    Public Property Doors As List(Of DoorInfo)
End Class

Public Class SetControllerIP
    Public Property ControllerID As UInteger
    Public Property IPAddress As String
    Public Property Netmask As String
    Public Property Gateway As String
    Public Property Listener As String
End Class

Public Class SetControllerIPResponse
    Public Property ControllerID As UInteger
    Public Property ListenerSet As Boolean
    Public Property ListenerMessage As String
    Public Property IPSet As Boolean
    Public Property IPMessage As String
End Class

Public Class GetTime
    Public Property ControllerID As UInteger
    Public Property ControllerDateTime As DateTime
    Public Property ErrorMessage As String
End Class

Public Class SetTimeResponse
    Public Property ControllerID As UInteger
    Public Property OK As Boolean
    Public Property ErrorMessage As String
End Class

Public Class SetListenerResponse
    Public Property ControllerID As UInteger
    Public Property OK As Boolean
    Public Property ErrorMessage As String
End Class

Public Class RecordSpecialEventsResponse
    Public Property ControllerID As UInteger
    Public Property OK As Boolean
    Public Property ErrorMessage As String
End Class

Public Class RestoreDefaultParametersResponse
    Public Property ControllerID As UInteger
    Public Property OK As Boolean
    Public Property ErrorMessage As String
End Class

Public Class SetNewController
    Public Property ControllerID As UInteger
    Public Property Listener As String
End Class

Public Class SetNewControllerResponse
    Public Property ControllerID As UInteger
    Public Property RestoreDefaultParameters As RestoreDefaultParametersResponse
    Public Property RecordSpecialEvents As RecordSpecialEventsResponse
    Public Property SetListener As SetListenerResponse
    Public Property SetTime As SetTimeResponse
End Class
#End Region

#Region "Event Class"
Public Class AccessEvent
    Public Property ControllerID As UInteger
    Public Property Timestamp As DateTime
    Public Property Index As Integer
    Public Property EventCode As Integer
    Public Property EventText As String
    Public Property AccessGranted As Boolean
    Public Property Door As Integer
    Public Property Direction As String
    Public Property CardNumber As UInteger
    Public Property ReasonCode As Integer
    Public Property ReasonText As String
End Class
#End Region

#Region "Card Class"
Public Class GetCardRequest
    Public Property ControllerID As UInteger?
    Public Property CardNumber As UInteger?
    Public Property ErrorIndex As Integer?
    Public Property ErrorMessage As String
End Class

Public Class CardInfo
    Public Property ControllerID As UInteger
    Public Property CardNumber As UInteger
    Public Property StartDate As DateOnly
    Public Property EndDate As DateOnly
    Public Property Door1 As Byte
    Public Property Door2 As Byte
    Public Property Door3 As Byte
    Public Property Door4 As Byte
    Public Property PIN As Integer
End Class

Public Class PutCardRequest
    Public Property ControllerID As UInteger
    Public Property CardNumber As UInteger
    Public Property EndDate As Date
    Public Property Doors As Dictionary(Of Integer, Byte)
    Public Property PIN As Integer
End Class

Public Class PutCardResponse
    Public Property ControllerID As UInteger
    Public Property CardNumber As UInteger
    Public Property Success As Boolean
    Public Property ErrorMessage As String
End Class

Public Class DeleteCardRequest
    Public Property ControllerID As UInteger
    Public Property CardNumber As UInteger?
End Class

Public Class DeleteCardResult
    Public Property ControllerID As UInteger
    Public Property CardNumber As UInteger?
    Public Property Success As Boolean
    Public Property ErrorMessage As String
End Class

Public Class DeleteUserRequest
    Public Property Controllers As List(Of UInteger)
    Public Property CardNumber As UInteger
End Class
#End Region

#Region "Door Class"
Public Class DoorInfo
    Public Property DoorNumber As Integer
    Public Property Mode As String
    Public Property Delay As Integer
End Class

Public Class DoorCommandResponse
    Public Property ControllerID As Integer
    Public Property Door As Integer
    Public Property Success As Boolean
    Public Property ErrorMessage As String
End Class

Public Class OpenDoorRequest
    Public Property ControllerID As UInteger
    Public Property Door As Byte
End Class

Public Class SetDoor
    Public Property ControllerID As UInteger
    Public Property Door As Integer
    Public Property Mode As Integer
    Public Property Delay As Integer
End Class

Public Class SetDoorPasscodesRequest
    Public Property ControllerID As UInteger
    Public Property Door As Byte
    Public Property Passcodes As UInteger()
End Class

Public Class SetDoorPasscodesResponse
    Public Property ControllerID As UInteger
    Public Property Door As Byte
    Public Property Success As Boolean
    Public Property ErrorMessage As String
End Class
#End Region

#Region "Settings Class"
Public Class SettingsPayload
    Public Property DebugMode As Boolean?
    Public Property SetBindIP As String
End Class

Public Class ClientHandshake
    Public Property ClientPrivilege As PrivilegeLevel
    Public Property ClientID As String
End Class
#End Region

Public Class ClientMessage
    Public Property Type As Commands.CommandType

    ' Controller
    Public Property ControllerID As UInteger?
    Public Property SetControllerIP As SetControllerIP
    Public Property SetNewController As SetNewController

    ' Event

    ' Door
    Public Property OpenDoorRequest As OpenDoorRequest
    Public Property SetDoors As List(Of SetDoor)
    Public Property SetDoorPasscodesRequest As SetDoorPasscodesRequest

    ' Card
    Public Property CardNumber As UInteger?
    Public Property GetCardRequest As GetCardRequest
    Public Property PutCardRequest As List(Of PutCardRequest)
    Public Property DeleteCardRequest As DeleteCardRequest
    Public Property DeleteUserRequest As DeleteUserRequest

    ' Settings
    Public Property Settings As SettingsPayload
    Public Property ClientHandshake As ClientHandshake
    Public Property OriginClientID As String
End Class

Public Class ServerMessage
    Public Property Type As Commands.CommandType

    ' Controller
    Public Property Controller As ControllerInfo
    Public Property Controllers As List(Of ControllerInfo)
    Public Property SetControllerIPResponse As SetControllerIPResponse
    Public Property GetTime As GetTime
    Public Property SetTimeResponse As SetTimeResponse
    Public Property RecordSpecialEventsResponse As RecordSpecialEventsResponse
    Public Property RestoreDefaultParametersResponse As RestoreDefaultParametersResponse
    Public Property SetNewControllerResponse As SetNewControllerResponse

    ' Event
    Public Property [Event] As AccessEvent
    Public Property [Error] As String

    ' Door
    Public Property DoorResponse As DoorCommandResponse
    Public Property SetDoorsResponse As List(Of SetDoor)
    Public Property UpdatedDoors As List(Of DoorInfo)
    Public Property SetDoorPasscodesResponse As SetDoorPasscodesResponse

    ' Card
    Public Property CardList As List(Of CardInfo)
    Public Property GetCardRequest As GetCardRequest
    Public Property GetCardRequestList As List(Of GetCardRequest)
    Public Property PutCardRequest As PutCardRequest
    Public Property PutCardResponse As List(Of PutCardResponse)
    Public Property DeleteCardResponse As List(Of DeleteCardResult)


    Public Property DeleteUserResponse As List(Of DeleteCardResult)
    Public Property OriginClientID As String
End Class

#Region "Command Class"
Public Class Commands
    Public Enum CommandType
        OpenDoor
        GetControllerList
        GetController
        SetControllerIP
        SetTime
        GetTime
        RecordSpecialEvents
        RestoreDefaultParameters
        AccessEvent
        SetDoor
        SetDoorPasscodes
        DoorCommandResponse
        GetCard
        GetCards
        GetCardError
        PutCard
        DeleteCard
        DeleteControllerCards
        DeleteUser
        IsError
        SetSettings
        AddControllerToSystem
        ClientHandshake
        Unknown
    End Enum
End Class
#End Region

#Region "Enum Class"
Public Class Enums
    Public Enum DoorMode
        Unknown
        NormallyOpen
        NormallyClosed
        Controlled
    End Enum

    Public Enum PrivilegeLevel
        Admin = 0
        Monitor = 1
        Config = 2
    End Enum
End Class
#End Region

#Region "CardContextInfo"
Public Class CardContextInfo
    Public Property ControllerID As UInteger
    Public Property CardNumber As UInteger

    Public Sub New(controllerId As UInteger, cardNumber As UInteger)
        Me.ControllerID = controllerId
        Me.CardNumber = cardNumber
    End Sub
End Class
#End Region