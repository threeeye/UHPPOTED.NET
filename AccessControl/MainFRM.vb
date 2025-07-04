Imports System.ComponentModel
Imports System.Text
Imports System.Threading
Imports AccessControl.Enums
Imports ACS.Messaging
Imports Cipher9
Imports Microsoft.Data.SqlClient
Imports Newtonsoft.Json

Public Class MainFRM
#Region "Form"
    Private Sub MainFRM_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Initialize ControllerDB
        'InitControllerDB()
        'Tag = PrivilegeLevel.Config
        InitPrivilege()
        ' Initialize DataGridView
        'DGVC()

        ' Set SQL connection
        ConString = GetConString()

        ' Start Client
        StartASC()
    End Sub

    Private Sub InitPrivilege()
        Select Case Tag
            Case PrivilegeLevel.Monitor
                ' Initialize DataGridView
                DGVC()

                For Each c As Control In Controls
                    If TypeOf c Is Button Then
                        c.Enabled = False
                    End If
                Next

                RichTextBox1.Enabled = False
            Case PrivilegeLevel.Config
                DataGridView1.Enabled = False
                RichTextBox1.Enabled = False
            Case PrivilegeLevel.Admin
                ' Initialize DataGridView
                DGVC()

                RichTextBox1.Enabled = True
        End Select
    End Sub

    Private Sub MainFRM_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Disconnect()

        'ControllerDB.Dispose()
    End Sub
#End Region

#Region "ASC"
    ' Client
    Shared Client As MessageClient
    Dim StopReconnect As Boolean

    ' Synchronization context for UI threading
    Private Shared uiContext As SynchronizationContext

    ' Separator
    Private Shared ReadOnly separator As Char() = New Char() {"|"c}

    Public Shared Event ServerMessageReceived(data As String)
    Private Sub StartASC()
        Client = New MessageClient("127.0.0.1", 64449, False)
        'Client = New MessageClient("192.168.100.20", 64449, False)

        AddHandler Client.ConnectionAccepted, AddressOf ConnectionAccepted
        AddHandler Client.ConnectionClosed, AddressOf ConnectionClosed
        AddHandler Client.MessageReceived, AddressOf MessageReceived
        AddHandler Client.ConnectionFailed, AddressOf ConnectionFailed

        Client.Connect()

        uiContext = SynchronizationContext.Current
    End Sub

    Private Sub ReConnect()
        Disconnect()

        Thread.Sleep(1000)

        StartASC()
    End Sub

    Private Sub Disconnect()
        'Close handlers
        RemoveHandler Client.ConnectionAccepted, AddressOf ConnectionAccepted
        RemoveHandler Client.ConnectionClosed, AddressOf ConnectionClosed
        RemoveHandler Client.MessageReceived, AddressOf MessageReceived
        RemoveHandler Client.ConnectionFailed, AddressOf ConnectionFailed

        Client.Dispose()
    End Sub

    Private Sub ConnectionAccepted(sender As Object, e As ConnectionEventArgs)
        Dim handshake As New ClientHandshake With {.ClientID = ClientID, .ClientPrivilege = CType(Tag, PrivilegeLevel)}
        Dim message As New ClientMessage With {.Type = Commands.CommandType.ClientHandshake, .ClientHandshake = handshake}

        MainFRM.SendMessage(message)
    End Sub

    Private Sub ConnectionFailed(sender As Object, e As ConnectionEventArgs)
        If uiContext IsNot Nothing Then
            uiContext.Post(Sub()
                               uiContext = SynchronizationContext.Current ' Capture UI thread context
                               ReConnect()
                           End Sub, Nothing)
        Else
            ' If uiContext is already Nothing, fallback to ReConnect directly
            ReConnect()
        End If
    End Sub

    Private Sub ConnectionClosed(sender As Object, e As ConnectionEventArgs)
        ' Ensure UI context is always set from the UI thread
        If uiContext IsNot Nothing Then
            uiContext.Post(Sub()
                               uiContext = SynchronizationContext.Current ' Capture UI thread context
                               If Not StopReconnect Then ReConnect()
                           End Sub, Nothing)
        Else
            ' If uiContext is already Nothing, fallback to ReConnect directly
            If Not StopReconnect Then ReConnect()
        End If
    End Sub

    Private Sub MessageReceived(sender As Object, e As MessageReceivedEventArgs)
        IncomingMessage(Encoding.UTF8.GetString(e.Data))
    End Sub

    'Public Shared Sub SendMessage(str As String)
    '    Client.SendData(Encoding.ASCII.GetBytes(str))
    'End Sub
    Public Shared Sub SendMessage(Data As ClientMessage)
        Data.OriginClientID = ClientID
        Dim json = JsonConvert.SerializeObject(Data)
        Client.SendData(Encoding.ASCII.GetBytes(json))
    End Sub

    Private Function IsMyOwnMessage(json As String) As Boolean
        Try
            Dim msg = JsonConvert.DeserializeObject(Of ServerMessage)(json)
            ' Optional: If you include origin info in the message (like IP or client ID), compare here.
            ' If you don’t currently send this info, you could embed it in the message temporarily
            Return False ' Fallback for now
        Catch
            Return False
        End Try
    End Function
#End Region

    Private Sub IncomingMessage(data As String)
        Dim settings As New JsonSerializerSettings With {.Converters = {New Converters.StringEnumConverter()}}
        Dim message As ServerMessage = JsonConvert.DeserializeObject(Of ServerMessage)(data, settings)

        Try
            Dim command As Commands.CommandType = message.Type
            'Dim message As ServerMessage = JsonConvert.DeserializeObject(Of ServerMessage)(data)

            If message Is Nothing OrElse String.IsNullOrWhiteSpace(message.Type) Then
                Exit Sub ' Ignore unknown messages
            End If

            Select Case command
                Case Commands.CommandType.GetControllerList, Commands.CommandType.GetController,
                     Commands.CommandType.SetControllerIP, Commands.CommandType.SetTime,
                     Commands.CommandType.GetTime, Commands.CommandType.RestoreDefaultParameters,
                     Commands.CommandType.RecordSpecialEvents, Commands.CommandType.GetCards,
                     Commands.CommandType.PutCard, Commands.CommandType.DeleteCard,
                     Commands.CommandType.SetDoorPasscodes
                    If message.OriginClientID = ClientID Then
                        Dim childFormsOpen = Application.OpenForms.Cast(Of Form)().Any(Function(f) f IsNot Me)
                        If childFormsOpen Then
                            RaiseEvent ServerMessageReceived(data)
                        End If
                    Else
                        uiContext.Post(Sub()
                                           RichTextBox1.AppendText($"{DateTime.Now}: {data}{Environment.NewLine}")
                                       End Sub, Nothing)
                    End If

                    '' If someone is listening (like an open form), raise the event
                    'If ServerMessageReceivedEvent?.GetInvocationList().Length > 1 Then
                    '    RaiseEvent ServerMessageReceived(data)
                    'Else
                    '    ' No form is handling it
                    '    Dim myPrivilege As PrivilegeLevel = CType(Tag, PrivilegeLevel)

                    '    ' If I’m an Admin, and the message came from someone else
                    '    If myPrivilege = PrivilegeLevel.Admin AndAlso Not IsMyOwnMessage(data) Then
                    '        RichTextBox1.AppendText($"{DateTime.Now}: {data}{Environment.NewLine}")
                    '    End If
                    'End If


                Case Commands.CommandType.AccessEvent
                    Dim evt = message.Event
                    uiContext.Post(Sub()
                                       AddRow(evt.ControllerID, evt.Timestamp, evt.Index, evt.EventCode,
                                              evt.EventText, evt.AccessGranted, evt.Door, evt.Direction,
                                              evt.CardNumber, evt.ReasonCode, evt.ReasonText)
                                   End Sub, Nothing)
                Case Commands.CommandType.DoorCommandResponse
                    Dim response = message.DoorResponse

                    If response.Success Then
                        MessageBox.Show($"Door {response.Door} opened successfully on Controller {response.ControllerID}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show($"Failed to open door {response.Door} on Controller {response.ControllerID}.{Environment.NewLine}Error: {response.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Case Commands.CommandType.IsError
                    Dim errorMsg As String = message.Error

                    Dim anyChildOpen = Application.OpenForms.Cast(Of Form)().Any(
                        Function(f) Not f.Name.Equals("MainFRM", StringComparison.OrdinalIgnoreCase))

                    If anyChildOpen Then
                        RaiseEvent ServerMessageReceived(data)
                    Else
                        MessageBox.Show(errorMsg, "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    'MessageBox.Show(message.Error, "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Case Else
                    ' You could raise a generic event or just ignore
                    'MessageBox.Show("Unknown message type: " & message.Type)
                    MessageBox.Show($"Unknown message type:{Environment.NewLine}{data}")
            End Select

        Catch ex As Exception
            MessageBox.Show($"Failed to process message: {vbCrLf}{ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

#Region "DataGrid"
    Private Sub DGVC()
        DataGridView1.Columns.Add("IDc", "ID")
        DataGridView1.Columns.Add("Controllerc", "Controller")
        DataGridView1.Columns.Add("Timestampc", "Timestamp")
        DataGridView1.Columns.Add("Indexc", "Index")
        DataGridView1.Columns.Add("Eventc", "Event")
        DataGridView1.Columns.Add("Cardc", "Card")
        DataGridView1.Columns.Add("Namec", "Name")
        DataGridView1.Columns.Add("Doorc", "Door")
        DataGridView1.Columns.Add("Directionc", "Direction")
        DataGridView1.Columns.Add("Grantedc", "Granted")
        DataGridView1.Columns.Add("Reasonc", "Reason")
    End Sub

    Private Sub AddRow(Controller As UInteger, TimeStamp As DateTime, Index As UInteger, EventCode As Integer, EventText As String,
                       Granted As Boolean, Door As Integer, Direction As String, Card As ULong, ReasonCode As Integer, ReasonText As String)
        Dim CardHolder As String = If(EventCode = 1, GetCardHolder(Card), "N/A")

        DataGridView1.Rows.Add(GetNextID(), Controller, TimeStamp, Index, EventText, If(EventCode = 1, Card, Nothing),
                               CardHolder, Door, Direction, Granted, ReasonText)

        ' Sort newest to top
        DataGridView1.Sort(DataGridView1.Columns("IDc"), ListSortDirection.Descending)

        ' Get the newly added row (it will now be at index 0 after sort)
        Dim newRow As DataGridViewRow = DataGridView1.Rows(0)

        ' Color based on EventCode and ReasonCode
        Select Case EventCode
            Case 1 ' Card Swipe
                If ReasonCode = 1 Then
                    newRow.DefaultCellStyle.BackColor = Color.LightGreen ' Access swipe
                ElseIf ReasonCode = 6 Then
                    newRow.DefaultCellStyle.BackColor = Color.LightCoral ' Swipe but access denied
                Else
                    newRow.DefaultCellStyle.BackColor = Color.LightYellow ' Unknown swipe event
                End If

            Case 2 ' Door events (pushbutton/sensor)
                If ReasonCode = 20 Then
                    newRow.DefaultCellStyle.BackColor = Color.LightBlue ' Pushbutton OK
                ElseIf ReasonCode = 23 Then
                    newRow.DefaultCellStyle.BackColor = Color.LightSkyBlue ' Door opened (sensor)
                ElseIf ReasonCode = 24 Then
                    newRow.DefaultCellStyle.BackColor = Color.LightCyan ' Door closed (sensor)
                Else
                    newRow.DefaultCellStyle.BackColor = Color.White ' Other door events
                End If

            Case Else
                newRow.DefaultCellStyle.BackColor = Color.White ' Unknown EventCode
        End Select
    End Sub

    Private Function GetNextID() As Integer
        If DataGridView1.RowCount > 0 Then
            Return CInt(DataGridView1.Rows(0).Cells(0).Value) + 1
        Else
            Return 1
        End If
    End Function
#End Region

#Region "SQL"
#Region "Get ConString"

    Private Function GetConString()
        Dim cipherText As String = My.Computer.FileSystem.ReadAllText("d:\AQMonitor")
        Dim Decrypt As New Cipher
        Return Decrypt.DecryptData(cipherText)
    End Function
#End Region

    Private Function GetCardHolder(Card As Integer)
        'Dim Name As String = Nothing

        'Dim cmd As New SqlCommand
        'Dim con As New SqlConnection(ConString)

        'Try
        '    con.Open()
        '    cmd.Connection = con

        '    cmd.CommandText = $"SELECT [Name] FROM TestAQ WHERE CardNo = {Card}"
        '    Name = cmd.ExecuteScalar()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'Finally
        '    con.Close()
        '    con.Dispose()
        '    cmd.Dispose()
        'End Try

        'Return Name


        Return "Test"
    End Function
#End Region

    'Public Sub SendSetDoorPasscodes(controllerID As UInteger, door As Byte, passcodes As UInteger())
    '    Dim request As New SetDoorPasscodesRequest With {
    '        .controllerID = controllerID,
    '        .door = door,
    '        .passcodes = passcodes
    '    }

    '    Dim message As New SetDoorPasscodesMessage With {
    '        .SetDoorPasscodesRequest = request
    '    }

    '    Dim json As String = JsonConvert.SerializeObject(message)
    '    MainFRM.SendMessage(json)
    'End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '' Open Door test
        '' You get Controller ID and Door from your UI (textbox, dropdown, etc.)
        'Dim controllerID As UInteger = 425026232
        'Dim door As Byte = 2

        'Dim openDoorRequest As New OpenDoorRequest With {
        '    .ControllerID = controllerID,
        '    .Door = door
        '}

        'Dim message As New ClientMessage With {
        '    .Type = Commands.CommandType.OpenDoor,
        '    .OpenDoorRequest = openDoorRequest
        '}

        '' Serialize to JSON
        'Dim json As String = JsonConvert.SerializeObject(message)

        '' Now send to the server (using your existing SendMessage function)
        'SendMessage(json)

        'Dim f As New DoorsFRM
        'f.ShowDialog()

        '    Dim settings As New SettingsPayload With {.SetBindIP = "192.168.100.121:0"}

        '    Dim msg As New ClientMessage With {
        '    .Type = Commands.CommandType.SetSettings,
        '    .Settings = settings
        '}

        '    MainFRM.SendMessage(JsonConvert.SerializeObject(msg))

        Dim result As String = RunCurlCommand("curl ifconfig.me")
        MsgBox("Your public IP: " & result)
    End Sub

    Shared Function RunCurlCommand(command As String) As String
        Dim psi As New ProcessStartInfo With {
            .FileName = "cmd.exe",
            .Arguments = "/c " & command,
            .RedirectStandardOutput = True,
            .RedirectStandardError = True,
            .UseShellExecute = False,
            .CreateNoWindow = True
        }

        Dim process As New Process With {
            .StartInfo = psi
        }

        Try
            process.Start()
            Dim output As String = process.StandardOutput.ReadToEnd()
            process.WaitForExit()
            Return output.Trim()
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Private Sub ControllersBTN_Click(sender As Object, e As EventArgs) Handles ControllersBTN.Click
        'If Not ControllerDB.Rows.Count > 0 Then SendMessage(ControllerCommands.Get_Controllers.ToString)

        Dim f As New ControllerFRM
        f.ShowDialog()
    End Sub

    Private Function SetDebugMode(Debug As Boolean)
        Dim settings As New SettingsPayload With {.DebugMode = True}
        Dim msg As New ClientMessage With {
            .Type = Commands.CommandType.SetSettings,
            .Settings = settings
        }

        Return JsonConvert.SerializeObject(msg)
    End Function

    Private Sub CardsBTN_Click(sender As Object, e As EventArgs) Handles CardsBTN.Click
        Dim f As New CardFRM
        f.ShowDialog()
    End Sub

    Private Sub DoorsBTN_Click(sender As Object, e As EventArgs) Handles DoorsBTN.Click
        Dim f As New DoorsFRM
        f.ShowDialog()
    End Sub

    Private Sub UsersBTN_Click(sender As Object, e As EventArgs) Handles UsersBTN.Click
        Dim f As New UsersFRM
        f.ShowDialog()
    End Sub
End Class