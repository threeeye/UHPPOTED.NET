Imports System.Configuration
Imports System.Data
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports AccessControlMonitor.Enums
Imports ACS.Messaging
Imports Cipher9
Imports Microsoft.Data.SqlClient
Imports Newtonsoft.Json
Imports uhppoted
Public Class AQMonitor

#Region "Service Control"
    Dim DebugMode As Boolean = ConfigurationManager.AppSettings("DebugMode")
    Protected Overrides Sub OnStart(args() As String)
        StartService()
    End Sub

    Protected Overrides Sub OnStop()
        ReStart(False)

        StopASCThread()
    End Sub

    Private Sub CreateLog(Logstr As String, Optional Controller As UInt32 = Nothing, Optional CIndex As UInt32 = Nothing)
        Dim source As String = "AQMonitor-Log"
        If Not EventLog.SourceExists(source) Then
            EventLog.CreateEventSource(source, "Application")
        End If

        EventLog.WriteEntry(source, Logstr)
    End Sub

    Private Sub StartService()
        ConString = GetConString()

        ASCClientDB()
        InitASC()

        StartAQMonitor()
    End Sub
#End Region

#Region "Access Control Monitor thread"
    Dim AQMonitorThread As Thread
    Private cancellationTokenSource As CancellationTokenSource

    Private Sub StartAQMonitor()
        ' Start AccessControl Monitor thread
        cancellationTokenSource = New CancellationTokenSource()

        Dim onevent As New OnEvent(AddressOf EventHandler)
        Dim onerror As New OnError(AddressOf ErrorHandler)

        AQMonitorThread = New Thread(Sub()
                                         CreateLog($"Listener thread Started at: {Now}")
                                         Dim result = uhppoted.Uhppoted.Listen(onevent, onerror,
                                                                               cancellationTokenSource.Token, OPTIONS)
                                         If result.IsError Then
                                             CreateLog($"Error: {Translate(result.ErrorValue)}")
                                         End If
                                     End Sub)

        AQMonitorThread.Start()
    End Sub

    Private Sub StopAQMonitor()
        If cancellationTokenSource IsNot Nothing Then
            cancellationTokenSource.Cancel()
            If AQMonitorThread IsNot Nothing AndAlso AQMonitorThread.IsAlive Then
                AQMonitorThread.Join() ' Wait for the thread to finish
            End If
            CreateLog($"Listener thread Stopped at: {Now}")
        End If
    End Sub

    Private Sub ReStart(restart As Boolean)
        StopAQMonitor()
        Thread.Sleep(1000)

        If restart Then StartAQMonitor()
    End Sub
#End Region

#Region "UHPPOTED"
    Dim UHPPOTEBindIP As String = $"{ConfigurationManager.AppSettings("UHPPOTEBindIP")}:0"
    Private ReadOnly OPTIONS = New OptionsBuilder().
                                WithBind(IPEndPoint.Parse(UHPPOTEBindIP)).
                                WithDebug(True).
                                WithTimeout(1250).
                                Build()
#Region "Listener"
    Private Sub EventHandler(e As ListenerEvent)
        Dim controller = e.Controller
        Dim evt = e.Event

        If evt.HasValue Then
            Dim v = evt.Value

            ' Get the last index of the controller from the database
            Dim ControllerLastIndex As Integer = GetLastIndex(controller)

            ' v.Event.Code:
            ' 1 = Card Swipe, 2 = Door (Pushbutton and/or sensor)

            ' v.Reason.Code:
            ' 1 = Swipe, 6 = No access rights, 20 = Pushbutton OK, 23 = Door opened (sensor), 24 = Door closed (sensor)

            If ControllerLastIndex = v.Index - 1 Then
                ProcessEvent(controller, v)
            ElseIf ControllerLastIndex < v.Index - 1 Then
                For i As UInt32 = ControllerLastIndex + 1 To v.Index
                    Dim NewEvt As uhppoted.Event = GetEventByIndex(controller, i)
                    ProcessEvent(controller, NewEvt)
                Next
            End If
        End If
    End Sub

    Private Sub ProcessEvent(controller As UInteger, evt As uhppoted.Event)
        Dim isSwipe = (evt.Event.Code = 1)

        If isSwipe Then
            WriteToDB(True, evt.Index, YYYYMMDDHHmmss(evt.Timestamp), controller,
                  evt.Door, evt.Reason.Code, evt.Card, evt.AccessGranted)
        Else
            WriteToDB(False, evt.Index, YYYYMMDDHHmmss(evt.Timestamp), controller, evt.Door, evt.Reason.Code)
        End If

        Dim jsonEvent As New AccessEvent With {
            .ControllerID = controller,
            .Timestamp = YYYYMMDDHHmmss(evt.Timestamp),
            .Index = evt.Index,
            .EventCode = evt.Event.Code,
            .EventText = evt.Event.Text,
            .AccessGranted = evt.AccessGranted,
            .Door = evt.Door,
            .Direction = Translate(evt.Direction),
            .CardNumber = evt.Card,
            .ReasonCode = evt.Reason.Code,
            .ReasonText = evt.Reason.Text
        }

        Dim message As New ServerMessage With {
            .Type = Commands.CommandType.AccessEvent,
            .[Event] = jsonEvent
        }

        ' Send to Admins and Monitors
        SendMessage(message, Nothing)
    End Sub

    Private Sub ErrorHandler(err)
        CreateLog($"** ERROR: {err}")
    End Sub
#End Region

#Region "Controller (IP) functions"
    Private Function GetControllers() As ServerMessage
        Dim result = uhppoted.Uhppoted.FindControllers(OPTIONS)

        If result.IsOk Then
            Dim controllers = result.ResultValue
            Dim controllerList As New List(Of ControllerInfo)

            For Each controller In controllers
                Dim doors = GetDoors(controller.Controller, Integer.Parse(controller.Controller.ToString().Substring(0, 1)))

                Dim c As New ControllerInfo With {
                .ControllerID = controller.Controller,
                .IPAddress = controller.Address.ToString(),
                .Netmask = controller.Netmask.ToString(),
                .Gateway = controller.Gateway.ToString(),
                .MAC = controller.MAC.ToString(),
                .Listener = GetListener(controller.Controller).ToString(),
                .Index = GetCorrentEventIndex(controller.Controller),
                .Version = controller.Version,
                .ControllerDate = YYYYMMDD(controller.Date),
                .Doors = doors
            }

                controllerList.Add(c)
            Next

            Return New ServerMessage With {
            .Type = Commands.CommandType.GetControllerList,
            .Controllers = controllerList
        }
        ElseIf result.IsError Then
            Return New ServerMessage With {
            .Type = Commands.CommandType.IsError,
            .Error = $"GetControllerList: {result.ErrorValue}"
        }
        Else
            Return New ServerMessage With {
            .Type = Commands.CommandType.IsError,
            .Error = "GetControllerList: Unknown Error"
        }
        End If
    End Function

    Private Function GetController(Controller As UInteger) As ServerMessage
        Dim result = uhppoted.Uhppoted.GetController(Controller, OPTIONS)

        If result.IsOk Then
            Dim record = result.ResultValue

            Dim doors = GetDoors(record.Controller, Integer.Parse(record.Controller.ToString().Substring(0, 1)))

            Dim c As New ControllerInfo With {
                    .ControllerID = record.Controller,
                    .IPAddress = record.Address.ToString(),
                    .Netmask = record.Netmask.ToString(),
                    .Gateway = record.Gateway.ToString(),
                    .MAC = record.MAC.ToString(),
                    .Listener = GetListener(record.Controller).ToString(),
                    .Index = GetCorrentEventIndex(record.Controller),
                    .Version = record.Version.ToString(),
                    .ControllerDate = YYYYMMDD(record.Date).ToString(),
                    .Doors = doors
                }

            Return New ServerMessage With {.Type = Commands.CommandType.GetController, .Controller = c}
        ElseIf result.IsError Then
            Return New ServerMessage With {
            .Type = Commands.CommandType.IsError,
            .Error = $"GetControllerList: {result.ErrorValue}"
        }
        Else
            Return New ServerMessage With {
            .Type = Commands.CommandType.IsError,
            .Error = "GetControllerList: Unknown Error"
        }
        End If
    End Function

    Private Function GetListener(Controller As UInteger)
        Dim result = uhppoted.Uhppoted.GetListener(Controller, OPTIONS)

        If result.IsOk Then
            Return result.ResultValue.Endpoint
        ElseIf result.IsError Then
            Return result.ErrorValue
        Else
            Return "Error!"
        End If
    End Function

    Private Function SetListener(controllerID As UInteger, listenerEndPoint As IPEndPoint) As SetListenerResponse
        Dim response As New SetListenerResponse With {.ControllerID = controllerID}
        Dim result = uhppoted.Uhppoted.SetListener(controllerID, listenerEndPoint, 0, OPTIONS)

        If result.IsOk Then
            response.OK = result.ResultValue
        ElseIf result.IsError Then
            response.OK = result.ResultValue
            response.ErrorMessage = Translate(result.ErrorValue)
        Else
            response.OK = False
            response.ErrorMessage = "Unknown Error"
        End If

        Return response
    End Function

    Private Function RecordSpecialEvents(controllerID As UInteger) As RecordSpecialEventsResponse
        Dim response As New RecordSpecialEventsResponse With {.ControllerID = controllerID}
        Dim result = uhppoted.Uhppoted.RecordSpecialEvents(controllerID, True, OPTIONS)

        If result.IsOk Then
            response.OK = result.ResultValue
        ElseIf result.IsError Then
            response.OK = result.ResultValue
            response.ErrorMessage = Translate(result.ErrorValue)
        Else
            response.OK = False
            response.ErrorMessage = "Unknown Error"
        End If

        Return response
    End Function

    Private Function RestoreDefaultParameters(controllerID As UInteger) As RestoreDefaultParametersResponse
        Dim response As New RestoreDefaultParametersResponse With {.ControllerID = controllerID}
        Dim result = uhppoted.Uhppoted.RestoreDefaultParameters(controllerID, OPTIONS)

        If result.IsOk Then
            response.OK = result.ResultValue
        ElseIf result.IsError Then
            response.OK = result.ResultValue
            response.ErrorMessage = Translate(result.ErrorValue)
        Else
            response.OK = False
            response.ErrorMessage = "Unknown Error"
        End If

        Return response
    End Function

    Private Function AddControllerToSystem(controller As SetNewController, originID As String) As ServerMessage
        Dim id = controller.ControllerID
        Dim listenerParts = controller.Listener.Split(":"c)
        Dim endpoint As New IPEndPoint(IPAddress.Parse(listenerParts(0)), Integer.Parse(listenerParts(1)))

        Dim response As New SetNewControllerResponse With {
            .ControllerID = id,
            .RestoreDefaultParameters = RestoreDefaultParameters(id),
            .SetListener = SetListener(id, endpoint),
            .RecordSpecialEvents = RecordSpecialEvents(id),
            .SetTime = SetTime(id)
        }

        Return New ServerMessage With {
            .Type = Commands.CommandType.AddControllerToSystem,
            .SetNewControllerResponse = response,
            .OriginClientID = originID
        }
    End Function
#End Region

#Region "Event functions"
    Private Function GetEventByIndex(Controller As UInt32, Index As UInt32) As uhppoted.Event
        Dim result = uhppoted.Uhppoted.GetEvent(Controller, Index, OPTIONS)
        If result.IsOk Then
            Return result.ResultValue
        ElseIf result.IsError And result.ErrorValue Is Err.EventNotFound Then
            Return Nothing
        ElseIf result.IsError And result.ErrorValue Is Err.EventOverwritten Then
            Return Nothing
        ElseIf result.IsError Then
            Return Nothing
        End If
        Return Nothing
    End Function

    Private Function GetCorrentEventIndex(Controller As UInteger) As UInteger
        Dim result = uhppoted.Uhppoted.GetStatus(Controller, OPTIONS)

        If result.IsOk Then
            Dim evt = result.ResultValue.Item2
            If evt.HasValue Then
                Return evt.Value.Index
            Else
                Return 0
            End If
            Return Nothing
        End If
        Return Nothing
    End Function
#End Region

#Region "Door functions"
    Private Function GetDoors(Controller As UInteger, DoorCount As Integer) As List(Of DoorInfo)
        Dim doors As New List(Of DoorInfo)

        For i As Integer = 1 To DoorCount
            Dim result = uhppoted.Uhppoted.GetDoor(Controller, i, OPTIONS)

            If result.IsOk Then
                Dim doorData = result.ResultValue
                doors.Add(New DoorInfo With {
                .DoorNumber = i,
                .Mode = doorData.Mode,
                .Delay = doorData.Delay
            })
            Else
                doors.Add(New DoorInfo With {
                .DoorNumber = i,
                .Mode = "Unknown",
                .Delay = -1
            })
            End If
        Next

        Return doors
    End Function

    Private Function SetDoors(Controller As UInteger, doors As List(Of DoorInfo)) As List(Of DoorInfo)
        Dim updatedDoors As New List(Of DoorInfo)

        For Each door In doors
            Dim modeEnum As Enums.DoorMode = CType(door.Mode, Enums.DoorMode)
            Dim delayValue As UInteger = Convert.ToUInt32(door.Delay)

            Dim result = uhppoted.Uhppoted.SetDoor(Controller, door.DoorNumber, modeEnum, delayValue, OPTIONS)

            If result.IsOk Then
                Dim doorData = result.ResultValue
                updatedDoors.Add(New DoorInfo With {
                .DoorNumber = door.DoorNumber,
                .Mode = doorData.Mode,
                .Delay = doorData.Delay
            })
            Else
                updatedDoors.Add(New DoorInfo With {
                .DoorNumber = door.DoorNumber,
                .Mode = Enums.DoorMode.Unknown,
                .Delay = -1
            })
            End If
        Next

        Return updatedDoors
    End Function

    Private Function OpenDoor(Controller As UInteger, Door As Byte) As DoorCommandResponse
        Dim result = uhppoted.Uhppoted.OpenDoor(Controller, Door, OPTIONS)

        Dim response As New DoorCommandResponse With {
            .ControllerID = Controller,
            .Door = Door,
            .Success = result.IsOk,
            .ErrorMessage = If(result.IsError, result.ErrorValue.ToString(), Nothing)
        }

        Return response
    End Function

    Private Function SetDoorPasscodes(Controller As UInteger, Door As Byte, PIN As UInteger()) As SetDoorPasscodesResponse
        Dim result = uhppoted.Uhppoted.SetDoorPasscodes(Controller, Door, PIN, OPTIONS)

        Dim response As New SetDoorPasscodesResponse With {
            .ControllerID = Controller,
            .Door = Door,
            .Success = result.IsOk,
            .ErrorMessage = If(result.IsError, result.ErrorValue.ToString(), Nothing)
        }

        Return response
    End Function
#End Region

#Region "Card functions"
    Private Function GetCards(Optional controllerId As UInteger? = Nothing,
                          Optional cardNumber As UInteger? = Nothing,
                          Optional originID As String = Nothing) As ServerMessage

        Dim cardList As New List(Of CardInfo)
        Dim errorList As New List(Of GetCardRequest)

        If controllerId.HasValue AndAlso cardNumber.HasValue Then
            Dim result = uhppoted.Uhppoted.GetCard(controllerId.Value, cardNumber.Value, OPTIONS)

            If result.IsOk Then
                Dim r = result.ResultValue
                cardList.Add(New CardInfo With {
                             .ControllerID = controllerId.Value,
                             .CardNumber = r.Card,
                             .StartDate = r.StartDate,
                             .EndDate = r.EndDate,
                             .Door1 = r.Door1,
                             .Door2 = r.Door2,
                             .Door3 = r.Door3,
                             .Door4 = r.Door4,
                             .PIN = r.PIN
                             })
            Else
                errorList.Add(New GetCardRequest With {
                              .ControllerID = controllerId.Value,
                              .CardNumber = cardNumber,
                              .ErrorMessage = If(result.IsError, result.ErrorValue.ToString(), "Unknown error")
                              })
            End If

        ElseIf controllerId.HasValue Then
            Dim result = uhppoted.Uhppoted.GetCards(controllerId.Value, OPTIONS)

            If result.IsOk Then
                For i As UInteger = 1 To result.ResultValue
                    Dim cardResult = uhppoted.Uhppoted.GetCardAtIndex(controllerId.Value, i, OPTIONS)
                    If cardResult.IsOk AndAlso cardResult.ResultValue.HasValue Then
                        Dim r = cardResult.ResultValue.Value
                        cardList.Add(New CardInfo With {
                                     .ControllerID = controllerId.Value,
                                     .CardNumber = r.Card,
                                     .StartDate = r.StartDate,
                                     .EndDate = r.EndDate,
                                     .Door1 = r.Door1,
                                     .Door2 = r.Door2,
                                     .Door3 = r.Door3,
                                     .Door4 = r.Door4, .PIN = r.PIN
                                     })
                    ElseIf cardResult.IsError Then
                        errorList.Add(New GetCardRequest With {
                                      .ControllerID = controllerId.Value,
                                      .ErrorIndex = i,
                                      .ErrorMessage = cardResult.ErrorValue.ToString()
                                      })
                    End If
                Next
            End If

        ElseIf cardNumber.HasValue Then
            Dim controllers = uhppoted.Uhppoted.FindControllers(OPTIONS).ResultValue
            For Each ctrl In controllers
                Dim result = uhppoted.Uhppoted.GetCard(ctrl.Controller, cardNumber.Value, OPTIONS)
                If result.IsOk Then
                    Dim r = result.ResultValue
                    cardList.Add(New CardInfo With {
                                 .ControllerID = ctrl.Controller,
                                 .CardNumber = r.Card,
                                 .StartDate = r.StartDate,
                                 .EndDate = r.EndDate,
                                 .Door1 = r.Door1,
                                 .Door2 = r.Door2,
                                 .Door3 = r.Door3,
                                 .Door4 = r.Door4,
                                 .PIN = r.PIN
                                 })
                ElseIf result.IsError Then
                    errorList.Add(New GetCardRequest With {
                                  .ControllerID = ctrl.Controller,
                                  .CardNumber = cardNumber,
                                  .ErrorMessage = result.ErrorValue.ToString()
                                  })
                End If
            Next

        Else
            Dim controllers = uhppoted.Uhppoted.FindControllers(OPTIONS).ResultValue
            For Each ctrl In controllers
                Dim result = uhppoted.Uhppoted.GetCards(ctrl.Controller, OPTIONS)
                If result.IsOk Then
                    For i As UInteger = 1 To result.ResultValue
                        Dim cardResult = uhppoted.Uhppoted.GetCardAtIndex(ctrl.Controller, i, OPTIONS)
                        If cardResult.IsOk AndAlso cardResult.ResultValue.HasValue Then
                            Dim r = cardResult.ResultValue.Value
                            cardList.Add(New CardInfo With {
                                         .ControllerID = ctrl.Controller,
                                         .CardNumber = r.Card,
                                         .StartDate = r.StartDate,
                                         .EndDate = r.EndDate,
                                         .Door1 = r.Door1,
                                         .Door2 = r.Door2,
                                         .Door3 = r.Door3,
                                         .Door4 = r.Door4,
                                         .PIN = r.PIN
                                         })
                        ElseIf cardResult.IsError Then
                            errorList.Add(New GetCardRequest With {
                                          .ControllerID = ctrl.Controller,
                                          .ErrorIndex = i,
                                          .ErrorMessage = cardResult.ErrorValue.ToString()
                                          })
                        End If
                    Next
                End If
            Next
        End If

        Return If(errorList.Count > 0,
        New ServerMessage With {
        .Type = Commands.CommandType.GetCardError,
        .GetCardRequestList = errorList,
        .OriginClientID = originID
        },
        New ServerMessage With {.Type = Commands.CommandType.GetCards, .CardList = cardList, .OriginClientID = originID})
    End Function

    Private Function PutCard(requests As List(Of PutCardRequest)) As ServerMessage
        Dim responses As New List(Of PutCardResponse)

        For Each C In requests
            Dim u8 As Byte
            Dim door1 As Byte = If(C.Doors.TryGetValue(1, u8), u8, 0)
            Dim door2 As Byte = If(C.Doors.TryGetValue(2, u8), u8, 0)
            Dim door3 As Byte = If(C.Doors.TryGetValue(3, u8), u8, 0)
            Dim door4 As Byte = If(C.Doors.TryGetValue(4, u8), u8, 0)

            Dim card = New uhppoted.CardBuilder(C.CardNumber).
                    WithStartDate(DateOnly.FromDateTime(Date.Now)).
                    WithEndDate(DateOnly.FromDateTime(C.EndDate)).
                    WithDoor1(door1).
                    WithDoor2(door2).
                    WithDoor3(door3).
                    WithDoor4(door4).
                    WithPIN(C.PIN).
                    Build()

            Dim result = uhppoted.Uhppoted.PutCard(C.ControllerID, card, OPTIONS)

            If result.IsOk Then
                responses.Add(New PutCardResponse With {
                .ControllerID = C.ControllerID,
                .CardNumber = C.CardNumber,
                .Success = True
            })
            Else
                responses.Add(New PutCardResponse With {
                .ControllerID = C.ControllerID,
                .CardNumber = C.CardNumber,
                .Success = False,
                .ErrorMessage = result.ErrorValue.ToString()
            })
            End If
        Next

        Return New ServerMessage With {
        .Type = Commands.CommandType.PutCard,
        .PutCardResponse = responses
    }
    End Function

    Private Function DeleteCard(controller As UInteger, Optional cardNumber As UInteger? = Nothing) As ServerMessage
        Dim resultObj As DeleteCardResult

        If cardNumber.HasValue Then
            Dim result = uhppoted.Uhppoted.DeleteCard(controller, cardNumber.Value, OPTIONS)

            resultObj = If(result.IsOk,
            New DeleteCardResult With {.ControllerID = controller, .CardNumber = cardNumber, .Success = True},
            New DeleteCardResult With {
            .ControllerID = controller,
            .CardNumber = cardNumber,
            .Success = False,
            .ErrorMessage = Translate(result.ErrorValue)
            })
        Else
            Dim result = uhppoted.Uhppoted.DeleteAllCards(controller, OPTIONS)

            resultObj = If(result.IsOk,
            New DeleteCardResult With {.ControllerID = controller, .Success = True},
            New DeleteCardResult With {
            .ControllerID = controller,
            .Success = False,
            .ErrorMessage = Translate(result.ErrorValue)
            })
        End If

        Return New ServerMessage With {
        .Type = Commands.CommandType.DeleteCard,
        .DeleteCardResponse = New List(Of DeleteCardResult) From {resultObj}
    }
    End Function
#End Region

#Region "Date/Time Functions"
    Private Function GetTime(controllerID As UInteger, originID As String) As ServerMessage
        Dim result = uhppoted.Uhppoted.GetTime(controllerID, OPTIONS)
        Dim response As New GetTime With {.ControllerID = controllerID}

        If result.IsOk Then
            response.ControllerDateTime = YYYYMMDDHHmmss(result.ResultValue)
        ElseIf result.IsError Then
            response.ErrorMessage = result.ErrorValue.ToString()
        Else
            response.ErrorMessage = "Unknown Error"
        End If

        Return New ServerMessage With {
        .Type = Commands.CommandType.GetTime,
        .GetTime = response,
        .OriginClientID = originID
    }
    End Function

    Private Function SetTime(controllerID As UInteger) As SetTimeResponse
        Dim response As New SetTimeResponse With {.ControllerID = controllerID}
        Dim result = uhppoted.Uhppoted.SetTime(controllerID, Now, OPTIONS)

        If result.IsOk Then
            response.OK = True
        ElseIf result.IsError Then
            response.OK = False
            response.ErrorMessage = Translate(result.ErrorValue)
        Else
            response.OK = False
            response.ErrorMessage = "Unknown Error"
        End If

        Return response
    End Function

    Private Function YYYYMMDD(v As Nullable(Of DateOnly)) As String
        If v.HasValue Then
            Return v.Value.ToString("yyyy-MM-dd")
        Else
            Return Nothing
        End If
    End Function

    Private Shared Function YYYYMMDDHHmmss(datetime As DateTime?) As String
        If datetime.HasValue Then
            Return datetime.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Return Nothing
        End If
    End Function

    Private Function HHmm(time As TimeOnly?) As String
        If time.HasValue Then
            Return time.Value.ToString("HH:mm")
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "Translate function"
    Private Function Translate(val) As String
        Return uhppoted.Uhppoted.Translate(val)
    End Function
#End Region
#End Region

#Region "SQL"
#Region "Get ConString"
    Dim ConString As String = Nothing
    Private Function GetConString()
        Dim cipherText As String = My.Computer.FileSystem.ReadAllText(ConfigurationManager.AppSettings("ConFile"))
        Dim Decrypt As New Cipher
        Return Decrypt.DecryptData(cipherText)
    End Function
#End Region

    Private Sub WriteToDB(Swipe As Boolean, EventIndex As Integer, iDate As Date, Controller As UInteger, Door As Byte,
                          Reason As Byte, Optional Card As UInteger = 0, Optional Granted As Boolean = False)
        Using con As New SqlConnection(ConString),
          cmd As New SqlCommand()
            Try
                con.Open()
                cmd.Connection = con

                cmd.CommandText = If(Swipe,
                "INSERT INTO AQSwipeList (EventIndex, iDate, Controller, Door, Card, Granted, Reason) VALUES " &
                "(@EventIndex, @iDate, @Controller, @Door, @Card, @Granted, @Reason)",
                "INSERT INTO AQEventList (EventIndex, iDate, Controller, Door, Reason) VALUES " &
                "(@EventIndex, @iDate, @Controller, @Door, @Reason)")

                ' Shared parameters
                cmd.Parameters.AddWithValue("@EventIndex", CInt(EventIndex))
                cmd.Parameters.AddWithValue("@iDate", iDate)
                cmd.Parameters.AddWithValue("@Controller", CInt(Controller))
                cmd.Parameters.AddWithValue("@Door", Door)

                ' Swipe-only parameters
                If Swipe Then
                    cmd.Parameters.AddWithValue("@Card", CInt(Card))
                    cmd.Parameters.AddWithValue("@Granted", Granted)
                End If

                cmd.Parameters.AddWithValue("@Reason", Reason)

                cmd.ExecuteNonQuery()
            Catch ex As Exception
                CreateLog($"Error inserting row to: {If(Swipe, "AQSwipeList", "AQEventList")}.{Environment.NewLine}{ex.Message}")
            End Try
        End Using
    End Sub

    Private Function GetLastIndex(c As UInt32)
        Dim cmd As New SqlCommand
        Dim con As New SqlConnection(ConString)

        Dim CIndex As Integer = Nothing

        Try
            con.Open()
            cmd.Connection = con

            cmd.CommandText = $"SELECT LastIndex FROM vControllers WHERE Controller = {c}"
            CIndex = Convert.ToInt32(cmd.ExecuteScalar())
        Catch ex As Exception
            CreateLog($"Error processing GetLastIndex for Controller {c}.{Environment.NewLine}{ex.Message}")
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
        End Try

        Return CIndex
    End Function
#End Region

#Region "ASC"
    Private ASCcancellationTokenSource As CancellationTokenSource
    Private ASCThread As Thread
    Dim ASCServer As MessageServer
    Private Sub InitASC()
        ASCcancellationTokenSource = New CancellationTokenSource()
        Dim token As CancellationToken = ASCcancellationTokenSource.Token

        ASCThread = New Thread(Sub() StartASCServer(token)) With {.IsBackground = True}
        ASCThread.Start()
    End Sub

    Private Sub StartASCServer(token As CancellationToken)
        ' Initialize the server
        ASCServer = New MessageServer("127.0.0.1", 64449, False)

        ' Add event handlers
        AddHandler ASCServer.ConnectionAccepted, AddressOf ASCConnectionAccepted
        AddHandler ASCServer.ConnectionClosed, AddressOf ASCConnectionClosed
        AddHandler ASCServer.MessageReceived, AddressOf ASCMessageReceived

        Try
            CreateLog($"ASC thread Started at: {Now}")

            ' Wait until the token is canceled
            While Not token.IsCancellationRequested
                ' Keep the thread alive while the server is running
                Thread.Sleep(100) ' Small delay to avoid busy-waiting
            End While
        Catch ex As Exception
            CreateLog($"Unexpected ASC error: {ex.Message}")
        Finally
            ' Cleanup when the server is stopping
            If ASCServer IsNot Nothing Then
                RemoveHandler ASCServer.ConnectionAccepted, AddressOf ASCConnectionAccepted
                RemoveHandler ASCServer.ConnectionClosed, AddressOf ASCConnectionClosed
                RemoveHandler ASCServer.MessageReceived, AddressOf ASCMessageReceived

                ASCServer.Dispose() ' Dispose of the server
                ASCServer = Nothing
            End If

            CreateLog($"ASC thread Stopped at: {Now}")
        End Try
    End Sub

    Private Sub StopASCThread()
        If ASCcancellationTokenSource IsNot Nothing Then
            ASCcancellationTokenSource.Cancel()
            If ASCThread IsNot Nothing AndAlso ASCThread.IsAlive Then
                ASCThread.Join() ' Wait for the thread to finish
            End If
        End If
    End Sub

    Private Sub ASCConnectionAccepted(sender As Object, e As ConnectionEventArgs)
        CreateLog($"ASC Client Connected: {e.Host.HostName}, On port: {e.Host.Port}")
    End Sub

    Private Sub ASCConnectionClosed(sender As Object, e As ConnectionEventArgs)
        CreateLog($"ASC Client Disconnected: {e.Host.HostName}, On port: {e.Host.Port}")

        ' Remove from ClientDB
        Dim rows = ClientDB.Select($"IP = '{e.Host.HostName}' AND Port = '{e.Host.Port}'")
        For Each row As DataRow In rows
            ClientDB.Rows.Remove(row)
        Next
    End Sub

    Private Sub ASCMessageReceived(sender As Object, e As MessageReceivedEventArgs)
        IncomingMessage(Encoding.UTF8.GetString(e.Data), e.Host.HostName, e.Host.Port)
    End Sub

    Private Sub SendMessage(message As Object, Optional originClientID As String = Nothing,
                        Optional host As String = Nothing, Optional port As Integer? = Nothing)
        Dim json As String = If(TypeOf message Is String, message.ToString(), JsonConvert.SerializeObject(message))

        If Not String.IsNullOrEmpty(host) AndAlso port.HasValue Then
            If DebugMode Then CreateLog($"Sending:{Environment.NewLine}{json}{Environment.NewLine}To: {host}:{port}")
            ASCServer.SendData(host, port.Value, Encoding.ASCII.GetBytes(json))
        ElseIf TypeOf message Is ServerMessage Then
            Dim serverMessage = CType(message, ServerMessage)
            serverMessage.OriginClientID = originClientID
            json = JsonConvert.SerializeObject(serverMessage)

            For Each client In GetRecipients(originClientID)
                If DebugMode Then CreateLog($"Sending:{Environment.NewLine}{json}{Environment.NewLine}To: {client.IP}:{client.Port}")
                ASCServer.SendData(client.IP, client.Port, Encoding.ASCII.GetBytes(json))
            Next
        Else
            ' Broadcast to all known clients
            For Each h In ASCServer.Hosts
                If DebugMode Then CreateLog($"Sending:{Environment.NewLine}{json}{Environment.NewLine}To: {h.HostName}:{h.Port}")
                ASCServer.SendData(h.HostName, h.Port, Encoding.ASCII.GetBytes(json))
            Next
        End If
    End Sub

    'Private Sub ASCSendMSG(str As String, Optional Host As String = Nothing, Optional Port As Integer? = Nothing)
    '    Dim data As Byte() = Encoding.ASCII.GetBytes(str)

    '    If Not String.IsNullOrEmpty(Host) AndAlso Port IsNot Nothing Then
    '        If DebugMode Then CreateLog($"Sending:{Environment.NewLine}{str}{Environment.NewLine}To: {Host}:{Port}")
    '        ASCServer.SendData(Host, Port, data)
    '    Else
    '        For Each H In ASCServer.Hosts
    '            If DebugMode Then
    '                CreateLog($"Sending:{Environment.NewLine}{str}{Environment.NewLine}To: {H.HostName}:{H.Port}")
    '            End If
    '            ASCServer.SendData(H.HostName, H.Port, data)
    '        Next
    '    End If
    'End Sub

    'Private Sub SendToRecipients(message As ServerMessage, originClientID As String)
    '    message.OriginClientID = originClientID
    '    Dim json As String = JsonConvert.SerializeObject(message)

    '    For Each client In GetRecipients(originClientID)
    '        ASCSendMSG(json, client.IP, client.Port)
    '    Next
    'End Sub

    'Private Function GetClientsWithPrivilege(required() As PrivilegeLevel, Optional IP As String = Nothing,
    '                                         Optional Port As Integer = 0) As List(Of (IP As String, Port As Integer))
    '    Dim list As New HashSet(Of (String, Integer))
    '    For Each row As DataRow In ClientDB.Rows
    '        Dim p = CType(row("Privilege"), PrivilegeLevel)
    '        If required.Contains(p) Then
    '            list.Add((row("IP").ToString(), Convert.ToInt32(row("Port"))))
    '        End If
    '    Next
    '    If Not String.IsNullOrWhiteSpace(IP) AndAlso Port > 0 Then list.Add((IP, Port))
    '    Return list.ToList
    'End Function
    Private Function GetRecipients(Optional originClientID As String = Nothing) As List(Of (IP As String, Port As Integer))
        Dim list As New List(Of (String, Integer))

        For Each row As DataRow In ClientDB.Rows
            Dim privilege = CType(row("Privilege"), PrivilegeLevel)
            Dim clientID = row("ClientID").ToString()

            If privilege = PrivilegeLevel.Admin OrElse
           (originClientID IsNot Nothing AndAlso clientID = originClientID) Then
                list.Add((row("IP").ToString(), Convert.ToInt32(row("Port"))))

            ElseIf originClientID Is Nothing AndAlso privilege = PrivilegeLevel.Monitor Then
                ' Monitors only get unsolicited messages (like events)
                list.Add((row("IP").ToString(), Convert.ToInt32(row("Port"))))
            End If
        Next

        Return list
    End Function

#Region "ASCClientDB"
    Private ClientDB As DataTable
    Private Sub ASCClientDB()
        ClientDB = New DataTable

        ClientDB.Columns.Add("IP")
        ClientDB.Columns.Add("Port")
        ClientDB.Columns.Add("Privilege", GetType(PrivilegeLevel))
        ClientDB.Columns.Add("ClientID")
    End Sub
#End Region
#End Region

    Private Sub IncomingMessage(data As String, Host As String, Port As Integer)
        If DebugMode Then CreateLog($"Message from '{Host}:{Port}':{Environment.NewLine}{data}")

        Dim settings As New JsonSerializerSettings With {.Converters = {New Converters.StringEnumConverter()}}
        Dim message As ClientMessage = JsonConvert.DeserializeObject(Of ClientMessage)(data, settings)

        Try
            Dim command As Commands.CommandType = message.Type
            If command = Commands.CommandType.Unknown Then
                If DebugMode Then CreateLog($"Unknown message type: '{command}'{Environment.NewLine}From: {Host}:{Port}")

                Dim response As New ServerMessage With {
                    .Type = Commands.CommandType.Unknown,
                    .[Error] = $"Unknown message type: {command}"
                }

                SendMessage(response, message.ClientHandshake?.ClientID)
                Exit Sub
            End If

            Dim controllerID As UInteger = message.ControllerID.GetValueOrDefault()
            Select Case command
#Region "Controller Commands"
                Case Commands.CommandType.GetController
                    Dim serverResponse = GetController(controllerID)
                    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.GetControllerList
                    Dim serverResponse = GetControllers()
                    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.SetControllerIP
                    'Dim ipRes As SetControllerIPResponse = Nothing
                    'Dim doorResults As List(Of DoorInfo) = Nothing

                    '' Handle IP changes
                    'If message.SetControllerIP IsNot Nothing Then
                    '    ipRes = JsonConvert.DeserializeObject(Of ServerMessage)(
                    '        SetControllerIP(
                    '        message.SetControllerIP.ControllerID,
                    '        message.SetControllerIP.IPAddress,
                    '        message.SetControllerIP.Netmask,
                    '        message.SetControllerIP.Gateway,
                    '        message.SetControllerIP.Listener)).SetControllerIPResponse
                    'End If

                    '' Handle Door changes
                    'If message.SetDoors IsNot Nothing AndAlso message.SetDoors.Count > 0 Then
                    '    ' Convert SetDoor to DoorInfo
                    '    Dim doorInfos As New List(Of DoorInfo)
                    '    For Each d In message.SetDoors
                    '        doorInfos.Add(New DoorInfo With {
                    '                      .DoorNumber = d.Door,
                    '                      .Mode = CType(d.Mode, Enums.DoorMode),
                    '                      .Delay = d.Delay
                    '                      })
                    '    Next
                    '    doorResults = SetDoors(message.SetDoors(0).ControllerID, doorInfos)
                    'End If

                    '' Build a single unified response
                    'Dim response As New ServerMessage With {
                    '    .Type = Commands.CommandType.SetControllerIP,
                    '    .SetControllerIPResponse = ipRes,
                    '    .UpdatedDoors = If(doorResults?.Count > 0, doorResults, Nothing)
                    '}

                    'ASCSendMSG(JsonConvert.SerializeObject(response), Host, Port)

                Case Commands.CommandType.GetTime
                    Dim serverResponse = GetTime(message.ControllerID, message.OriginClientID)
                    SendMessage(serverResponse, serverResponse.OriginClientID)

                Case Commands.CommandType.SetTime
                    Dim response = SetTime(message.ControllerID)
                    Dim serverMessage As New ServerMessage With {
                        .Type = Commands.CommandType.SetTime,
                        .SetTimeResponse = response,
                        .OriginClientID = message.OriginClientID
                    }
                    SendMessage(serverMessage, serverMessage.OriginClientID)

                Case Commands.CommandType.RecordSpecialEvents
                    Dim res = RecordSpecialEvents(message.ControllerID)
                    Dim serverResponse As New ServerMessage With {
                        .Type = Commands.CommandType.RecordSpecialEvents,
                        .RecordSpecialEventsResponse = res,
                        .OriginClientID = message.OriginClientID
                    }
                    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.RestoreDefaultParameters
                    Dim res = RestoreDefaultParameters(message.ControllerID)
                    Dim serverResponse As New ServerMessage With {
                        .Type = Commands.CommandType.RestoreDefaultParameters,
                        .RestoreDefaultParametersResponse = res,
                        .OriginClientID = message.OriginClientID
                    }
                    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.AddControllerToSystem
                    Dim serverResponse = AddControllerToSystem(message.SetNewController, message.OriginClientID)
                    SendMessage(serverResponse, serverResponse.OriginClientID)
#End Region

#Region "Card Commands"
                Case Commands.CommandType.GetCards
                    Dim Cards = message.GetCardRequest
                    Dim serverResponse = GetCards(Cards.ControllerID, Cards.CardNumber, message.OriginClientID)
                    SendMessage(serverResponse, serverResponse.OriginClientID)

                Case Commands.CommandType.PutCard
                    Dim serverResponse = PutCard(message.PutCardRequest)
                    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.DeleteCard
                    Dim DeleteCardRequest = message.DeleteCardRequest
                    Dim serverResponse = DeleteCard(DeleteCardRequest.ControllerID, DeleteCardRequest.CardNumber)
                    SendMessage(serverResponse, message.OriginClientID)

                'Case Commands.CommandType.DeleteCard
                '    Dim results As New List(Of DeleteCardResult)

                '    ' Case 1: Controller list provided and card number provided = delete card from each controller
                '    If message.DeleteCardRequest.Controllers?.Count > 0 AndAlso message.CardNumber.HasValue Then
                '        For Each ctrl In message.DeleteCardRequest.Controllers
                '            results.Add(JsonConvert.DeserializeObject(Of DeleteCardResult)(DeleteCard(ctrl, message.CardNumber)))
                '        Next

                '        ' Case 2: Controllers list provided but no card number = delete all cards from each controller
                '    ElseIf message.DeleteCardRequest.Controllers?.Count > 0 Then
                '        For Each ctrl In message.DeleteCardRequest.Controllers
                '            results.Add(JsonConvert.DeserializeObject(Of DeleteCardResult)(DeleteCard(ctrl)))
                '        Next

                '        ' Case 3: Only CardNumber = delete card from all controllers
                '    ElseIf message.CardNumber.HasValue Then
                '        Dim controllers = uhppoted.Uhppoted.FindControllers(OPTIONS)
                '        For Each ctrl In controllers.ResultValue
                '            results.Add(JsonConvert.DeserializeObject(Of DeleteCardResult)(DeleteCard(ctrl.Controller, message.CardNumber)))
                '        Next

                '        ' Case 4: Invalid request
                '    Else
                '        results.Add(New DeleteCardResult With {
                '            .ControllerID = 0,
                '            .CardNumber = Nothing,
                '            .Success = False,
                '            .ErrorMessage = "Invalid delete request: no controller or card number provided"
                '        })
                '    End If

                '    Dim serverResponse As New ServerMessage With {
                '        .Type = Commands.CommandType.DeleteCard,
                '        .DeleteCardResponse = results,
                '        .OriginClientID = message.OriginClientID
                '    }

                '    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.DeleteControllerCards
                    Dim serverResponse = DeleteCard(controllerID)
                    SendMessage(serverResponse, message.OriginClientID)

                'Case Commands.CommandType.DeleteUser
                '    Dim request = message.DeleteUserRequest
                '    Dim results As New List(Of DeleteCardResult)

                '    For Each controllerID In request.Controllers
                '        results.Add(JsonConvert.DeserializeObject(Of
                '                    DeleteCardResult)(DeleteCard(controllerID, request.CardNumber)))
                '    Next

                '    Dim response As New ServerMessage With {
                '        .Type = Commands.CommandType.DeleteUser,
                '        .DeleteUserResponse = results
                '    }

                '    For Each client In GetClientsWithPrivilege({PrivilegeLevel.Admin}, Host, Port)
                '        ASCSendMSG(JsonConvert.SerializeObject(response), client.IP, client.Port)
                '    Next
#End Region

#Region "Door Commands"
                Case Commands.CommandType.SetDoorPasscodes
                    Dim request = message.SetDoorPasscodesRequest
                    Dim SetDoorPasscodesResponse = SetDoorPasscodes(request.ControllerID, request.Door, request.Passcodes)

                    Dim serverResponse As New ServerMessage With {
                        .Type = Commands.CommandType.SetDoorPasscodes,
                        .SetDoorPasscodesResponse = SetDoorPasscodesResponse
                    }

                    SendMessage(serverResponse, message.OriginClientID)

                Case Commands.CommandType.OpenDoor
                    Dim request = message.OpenDoorRequest
                    Dim doorResponse = OpenDoor(request.ControllerID, request.Door)

                    Dim serverResponse As New ServerMessage With {
                        .Type = Commands.CommandType.DoorCommandResponse,
                        .DoorResponse = doorResponse
                    }

                    SendMessage(serverResponse, message.OriginClientID)
#End Region
                Case Commands.CommandType.SetSettings
                    If message.Settings.DebugMode.HasValue Then
                        DebugMode = message.Settings.DebugMode.Value
                        CreateLog($"DebugMode set to {DebugMode}")

                        Dim response As New ServerMessage With {
                            .Type = Commands.CommandType.SetSettings,
                            .[Error] = $"DebugMode set to {DebugMode}"
                        }

                        SendMessage(response, message.OriginClientID)
                    Else
                        If DebugMode Then CreateLog("SetSettings received but no settings were defined")
                    End If

                    If Not String.IsNullOrWhiteSpace(message.Settings.SetBindIP) Then
                        StopAQMonitor()

                        UHPPOTEBindIP = message.Settings.SetBindIP
                        CreateLog($"UHPPOTE Bind IP changed to: {UHPPOTEBindIP}")

                        StartAQMonitor()
                    End If
                Case Commands.CommandType.ClientHandshake
                    Dim existing = ClientDB.Select($"IP = '{Host}' AND Port = '{Port}'")
                    If existing.Length = 0 Then
                        Dim ClientHandshake = message.ClientHandshake
                        ClientDB.Rows.Add(Host, Port, ClientHandshake.ClientPrivilege, ClientHandshake.ClientID)
                    Else
                        existing(0)("Privilege") = message.ClientHandshake.ClientPrivilege
                        existing(0)("ClientID") = message.ClientHandshake.ClientID
                    End If

                Case Else
                    If DebugMode Then CreateLog($"Unhandled message type: {command}{Environment.NewLine}From: {Host}:{Port}")

                    Dim serverResponse As New ServerMessage With {
                        .Type = Commands.CommandType.Unknown,
                        .[Error] = $"Unhandled message type: {command}"
                    }

                    SendMessage(serverResponse, message.OriginClientID)
            End Select
        Catch ex As Exception
            If DebugMode Then
                CreateLog($"Error processing incoming client message:{Environment.NewLine}{message}{Environment.NewLine}" &
                      $"{ex.Message}{Environment.NewLine}From: {Host}:{Port}")
            End If
        End Try
    End Sub
End Class