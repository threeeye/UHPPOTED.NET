Imports Newtonsoft.Json

Public Class DoorsFRM
    Private DoorDB As DataTable
    Private Sub DoorsFRM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DoorDBInit()

        ' Set Handler for messages
        AddHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage



        For Each c In GetControllers()
            Dim message As New ClientMessage With {
                .Type = Commands.CommandType.GetController,
                .ControllerID = CUInt(c)
            }

            MainFRM.SendMessage(message)
            'AddDoors(c)
        Next

        Reorder()
    End Sub

    Private Sub DoorsFRM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage
    End Sub


    Private Sub DoorDBInit()
        DoorDB = New DataTable

        DoorDB.Columns.Add("Controller")
        DoorDB.Columns.Add("Door")
        DoorDB.Columns.Add("Name")

        DoorDB.Rows.Add(425026232, 1, "C1D1")
        DoorDB.Rows.Add(425026232, 2, "C1D2")
        DoorDB.Rows.Add(425026232, 3, "C1D3")
        DoorDB.Rows.Add(425026232, 4, "C1D4")
        DoorDB.Rows.Add(123339118, 1, "C2D1")
    End Sub
    Private Sub HandleServerMessage(data As String)
        Dim settings As New JsonSerializerSettings With {.Converters = {New Converters.StringEnumConverter()}}
        Dim message As ServerMessage = JsonConvert.DeserializeObject(Of ServerMessage)(data, settings)
        'MsgBox(data)
        Try
            Dim command As Commands.CommandType = message.Type

            Select Case command
                Case Commands.CommandType.GetController
                    Invoke(Sub()
                               AddDoors(message.Controller.ControllerID, True)
                           End Sub)
                Case Commands.CommandType.IsError
                    Invoke(Sub()
                               AddDoors(message.Controller.ControllerID, False)
                           End Sub)
                    'Dim C = message.Controller
                    'If Not message.Type = Commands.CommandType.IsError Then
                    '    MsgBox(data)
                    'End If
                    'MsgBox(data)
                    'MsgBox($"ID: {C.ControllerID}{Environment.NewLine}IP: {C.IPAddress}{Environment.NewLine}Mask: {C.Netmask}{Environment.NewLine}Gateway: {C.Gateway}{Environment.NewLine}MAC: {C.MAC}{Environment.NewLine}Listener: {C.Listener}{Environment.NewLine}Version: {C.Version}{Environment.NewLine}Date: {C.ControllerDate}")
            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Invoke(Sub()
                   Reorder()
               End Sub)

    End Sub



    Private Sub AddDoors(Controller As UInteger, Online As Boolean)
        For i As Integer = 1 To Integer.Parse(Controller.ToString().Substring(0, 1))
            Dim NameLabel As New Label With {
                .Name = $"{Controller}|{i}",
                .Text = GetDoorNames(Controller, i),
                .Tag = $"{Controller}|{i}",
                .Location = New Point(0, 0),
                .AutoSize = True,
                .ContextMenuStrip = If(Online, New CMS, Nothing),
                .BackColor = If(Online, Color.LightGreen, Color.Red)
            }

            Controls.Add(NameLabel)
        Next
    End Sub

    Private Function GetDoorNames(Controller As UInteger, Door As Byte) As String
        Dim DoorName As String = Nothing

        For Each row As DataRow In DoorDB.Rows
            If CUInt(row("Controller")) = Controller AndAlso row("Door") = Door Then
                DoorName = row("Name")
            End If
        Next

        Return DoorName
    End Function

    Private Sub Reorder()
        Dim CNum As Integer = 0
        Dim YNum As Integer = 0

        For Each P As Control In Controls
            If TypeOf P Is Label Then
                Dim LocX As Integer = (CNum * P.Width) + (CNum * 2)
                Dim LocY As Integer = If((Width - LocX) < P.Width, 0, (YNum * P.Height) + (YNum * 2))

                If (Width - LocX) - P.Width < P.Width Then YNum += 1 : CNum = 0 Else CNum += 1

                P.Location = New Point(LocX, LocY)
            End If
        Next
    End Sub

    Private Function GetControllers() As List(Of UInteger)
        Dim Controllers As New List(Of UInteger) From {425026232, 123339118}

        Return Controllers
    End Function
End Class