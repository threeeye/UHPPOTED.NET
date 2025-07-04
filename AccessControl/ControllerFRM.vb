Imports System.Data.Common
Imports Microsoft.Data.SqlClient
Imports Newtonsoft.Json

Public Class ControllerFRM
    Private Sub ControllerFRM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Handler for messages
        AddHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage

        ' Set controlls
        SaveBTN.Enabled = False
        'ShowAllRDO.Checked = True

        ' Initialize ControllerDB
        InitControllerDB()

        ' Initialize SQLControllerDB
        InitSQLControllerDB()

        ' Initialize DoorDB
        InitDoorDB()

        ' Get Controllers from SQL
        GetSQLControllers()

        ' Get Controllers
        GetControllers()
    End Sub

    Private Sub ControllerFRM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage
    End Sub

    Private Sub CloseBTN_Click(sender As Object, e As EventArgs) Handles CloseBTN.Click
        Close()
    End Sub

    Private Sub HandleServerMessage(data As String)
        Dim settings As New JsonSerializerSettings With {.Converters = {New Converters.StringEnumConverter()}}
        Dim message As ServerMessage = JsonConvert.DeserializeObject(Of ServerMessage)(data, settings)

        Try
            Dim command As Commands.CommandType = message.Type

            Select Case command
                Case Commands.CommandType.GetControllerList
                    If message.Controllers IsNot Nothing Then
                        Me.Invoke(Sub()
                                      For Each ctrl In message.Controllers
                                          ControllerDB.Rows.Add(ctrl.ControllerID, ctrl.IPAddress, ctrl.Netmask, ctrl.Gateway,
                                                  ctrl.MAC, ctrl.Listener, ctrl.Index, ctrl.Version, ctrl.ControllerDate)

                                          For Each door In ctrl.Doors
                                              DoorDB.Rows.Add(ctrl.ControllerID, door.DoorNumber, door.Mode, door.Delay)
                                          Next
                                      Next

                                      'ControllersBox.DisplayMember = "Controller"
                                      'ControllersBox.ValueMember = "Controller"
                                      'ControllersBox.DataSource = ControllerDB

                                      TXTInit(False)

                                      ControllersBox.DrawMode = DrawMode.OwnerDrawFixed

                                      ShowAllRDO.Checked = True
                                  End Sub)
                    End If

                Case Commands.CommandType.GetController
                    Dim C = message.Controller
                    MsgBox($"ID: {C.ControllerID}{Environment.NewLine}IP: {C.IPAddress}{Environment.NewLine}Mask: {C.Netmask}{Environment.NewLine}Gateway: {C.Gateway}{Environment.NewLine}MAC: {C.MAC}{Environment.NewLine}Listener: {C.Listener}{Environment.NewLine}Version: {C.Version}{Environment.NewLine}Date: {C.ControllerDate}")
                Case Commands.CommandType.SetControllerIP
                    Dim IPRes = message.SetControllerIPResponse
                    If IPRes.IPSet AndAlso IPRes.ListenerSet Then
                        UpdateRow(IPRes.ControllerID, IPAddressTXT.Text, NetmaskTXT.Text, GatewayTXT.Text, ListenerTXT.Text)
                    End If

                Case Commands.CommandType.SetTime, Commands.CommandType.RestoreDefaultParameters,
                     Commands.CommandType.RecordSpecialEvents
                    MsgBox(data)

                Case Commands.CommandType.GetCards
                    'Dim card As New CardInfo With {.CardNumber = message.CardList}

                    'Dim c As New CardInfo From {message.CardList}
                    'Dim str As String = $"Controller: {}"
                    'MsgBox(data)
                    If message.CardList.Count > 0 Then
                        For Each c In message.CardList
                            MsgBox($"Controller: {c.ControllerID}{Environment.NewLine}  Card: {c.CardNumber}{Environment.NewLine}  Door1: {c.Door1}{Environment.NewLine}  Door2: {c.Door2}{Environment.NewLine}  Door3: {c.Door3}{Environment.NewLine}  Door4: {c.Door4}{Environment.NewLine}  Start Date: {c.StartDate}{Environment.NewLine}  End Date: {c.EndDate}{Environment.NewLine}  PIN: {c.PIN}")
                        Next
                    Else
                        MsgBox("There are no cards on this controller")
                    End If

            End Select
        Catch ex As Exception
            MessageBox.Show("Failed to update ControllerFRM: " & ex.Message)
        End Try
    End Sub

    Private Sub GetControllers()
        Dim message As New ClientMessage With {.Type = Commands.CommandType.GetControllerList}

        MainFRM.SendMessage(message)
    End Sub

    Private Sub PopulateBoxes(Address As String, Netmask As String, Gateway As String, MAC As String,
                              Listener As String, Index As UInteger, Version As String, [Date] As String)
        IPAddressTXT.Text = Address
        NetmaskTXT.Text = Netmask
        GatewayTXT.Text = Gateway
        MACTXT.Text = MAC
        ListenerTXT.Text = Listener
        IndexLBL.Text = Index
        VersionTXT.Text = Version
        DateTXT.Text = [Date]
    End Sub

    Private Sub PopulateRadio(groupBox As GroupBox, doorNumber As Integer, modeValue As Integer)
        Dim modeName As String = [Enum].GetName(GetType(Enums.DoorMode), modeValue)
        Dim expectedName As String = modeName & doorNumber.ToString()

        Dim radioButton = groupBox.Controls.OfType(Of RadioButton)().FirstOrDefault(Function(rb) rb.Name = expectedName)
        If radioButton IsNot Nothing Then
            radioButton.Checked = True
        End If
    End Sub

    Private Sub ControllersBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ControllersBox.SelectedIndexChanged
        ClearData()
        If EditBTN.Text = "Cancel" Then EditBTN.PerformClick() Else GetRow()
    End Sub

#Region "ControllerDB and DoorDB"
    'Private ControllerDB As DataTable
    Private Sub InitControllerDB()
        ControllerDB = New DataTable

        ControllerDB.Columns.Add("Controller")
        ControllerDB.Columns.Add("Address")
        ControllerDB.Columns.Add("Netmask")
        ControllerDB.Columns.Add("Gateway")
        ControllerDB.Columns.Add("MAC")
        ControllerDB.Columns.Add("Listener")
        ControllerDB.Columns.Add("Index")
        ControllerDB.Columns.Add("Version")
        ControllerDB.Columns.Add("Date")
    End Sub

    'Private DoorDB As DataTable
    Private Sub InitDoorDB()
        DoorDB = New DataTable

        DoorDB.Columns.Add("Controller")
        DoorDB.Columns.Add("Door")
        DoorDB.Columns.Add("Mode")
        DoorDB.Columns.Add("Delay")
    End Sub

    'Private SQLControllerDB As DataTable
    Private Sub InitSQLControllerDB()
        SQLControllerDB = New DataTable

        SQLControllerDB.Columns.Add("Controller", GetType(UInteger))
        SQLControllerDB.Columns.Add("Door1Name")
        SQLControllerDB.Columns.Add("Door2Name")
        SQLControllerDB.Columns.Add("Door3Name")
        SQLControllerDB.Columns.Add("Door4Name")
    End Sub

    Private Sub ControllersBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles ControllersBox.DrawItem
        If e.Index < 0 Then Return

        Dim rowView As DataRowView = CType(ControllersBox.Items(e.Index), DataRowView)
        Dim controllerId = rowView("Controller").ToString()
        Dim isOffline As Boolean = rowView("Address").ToString() = "[OFFLINE]"

        ' Set background
        e.DrawBackground()
        Dim brush As Brush = If(isOffline, Brushes.Red, Brushes.Black)
        e.Graphics.DrawString(controllerId, e.Font, brush, e.Bounds)
        e.DrawFocusRectangle()
    End Sub

    Private Sub ShowAllRDO_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllRDO.CheckedChanged
        If ShowAllRDO.Checked Then ApplyControllerFilter()
    End Sub

    Private Sub ShowNewRDO_CheckedChanged(sender As Object, e As EventArgs) Handles ShowNewRDO.CheckedChanged
        If ShowNewRDO.Checked Then ApplyControllerFilter()
    End Sub

    Private Sub ShowSysRDO_CheckedChanged(sender As Object, e As EventArgs) Handles ShowSysRDO.CheckedChanged
        If ShowSysRDO.Checked Then ApplyControllerFilter()
    End Sub

    Private FilteredControllersDB As DataTable
    Private Sub ApplyControllerFilter()
        If ControllerDB Is Nothing OrElse SQLControllerDB Is Nothing Then Return

        ' Clone ControllerDB to keep same structure
        FilteredControllersDB = ControllerDB.Clone()

        ' Convert SQLControllerDB to a HashSet for fast lookup
        Dim sqlSet = New HashSet(Of UInteger)(SQLControllerDB.AsEnumerable().Select(Function(r) CUInt(r("Controller"))))
        Dim controllerSet = New HashSet(Of UInteger)(ControllerDB.AsEnumerable().Select(Function(r) CUInt(r("Controller"))))

        If ShowAllRDO.Checked Then
            ' Start with all from ControllerDB (online controllers)
            For Each row As DataRow In ControllerDB.Rows
                FilteredControllersDB.ImportRow(row)
            Next

            ' Add offline controllers from SQLControllerDB (not already in ControllerDB)
            For Each sqlRow As DataRow In SQLControllerDB.Rows
                Dim cid As UInteger = CUInt(sqlRow("Controller"))
                If Not controllerSet.Contains(cid) Then
                    Dim offlineRow As DataRow = FilteredControllersDB.NewRow()
                    offlineRow("Controller") = cid
                    offlineRow("Address") = "[OFFLINE]"
                    offlineRow("Netmask") = ""
                    offlineRow("Gateway") = ""
                    offlineRow("MAC") = ""
                    offlineRow("Listener") = ""
                    offlineRow("Index") = ""
                    offlineRow("Version") = ""
                    offlineRow("Date") = ""
                    FilteredControllersDB.Rows.Add(offlineRow)
                End If
            Next

        ElseIf ShowNewRDO.Checked Then
            ' Only controllers in ControllerDB but not in SQLControllerDB
            For Each row As DataRow In ControllerDB.Rows
                Dim cid As UInteger = CUInt(row("Controller"))
                If Not sqlSet.Contains(cid) Then
                    FilteredControllersDB.ImportRow(row)
                End If
            Next

        ElseIf ShowSysRDO.Checked Then
            ' Only controllers in SQLControllerDB (offline or online)
            For Each sqlRow As DataRow In SQLControllerDB.Rows
                Dim cid As UInteger = CUInt(sqlRow("Controller"))
                Dim matchingRow = ControllerDB.Select($"Controller = {cid}").FirstOrDefault()
                If matchingRow IsNot Nothing Then
                    FilteredControllersDB.ImportRow(matchingRow)
                Else
                    ' Add as offline
                    Dim offlineRow As DataRow = FilteredControllersDB.NewRow()
                    offlineRow("Controller") = cid
                    offlineRow("Address") = "[OFFLINE]"
                    offlineRow("Netmask") = ""
                    offlineRow("Gateway") = ""
                    offlineRow("MAC") = ""
                    offlineRow("Listener") = ""
                    offlineRow("Index") = ""
                    offlineRow("Version") = ""
                    offlineRow("Date") = ""
                    FilteredControllersDB.Rows.Add(offlineRow)
                End If
            Next
        End If

        ControllersBox.DisplayMember = "Controller"
        ControllersBox.ValueMember = "Controller"
        ControllersBox.DataSource = FilteredControllersDB
    End Sub



#End Region

#Region "Set New IP Address"
    Private Sub TXTInit(Edit As Boolean)
        ' Enable/Disable general controls
        IPAddressTXT.Enabled = Edit
        NetmaskTXT.Enabled = Edit
        GatewayTXT.Enabled = Edit
        ListenerTXT.Enabled = Edit

        ' Determine how many doors are present
        Dim doorCount As Integer = 4 ' default
        If ControllersBox.Items.Count > 0 AndAlso ControllersBox.SelectedItem IsNot Nothing Then
            Dim rowView As DataRowView = CType(ControllersBox.SelectedItem, DataRowView)
            Dim controllerID As UInteger = CUInt(rowView("Controller"))
            doorCount = Integer.Parse(controllerID.ToString().Substring(0, 1))
        End If

        ' Loop over door groups 1 to 4
        For i As Integer = 1 To 4
            Dim group = Controls.Find($"DoorGroup{i}", True).FirstOrDefault()
            If TypeOf group Is GroupBox Then
                Dim grp = DirectCast(group, GroupBox)
                Dim isEnabled = Edit AndAlso i <= doorCount
                grp.Enabled = isEnabled

                If Not isEnabled Then
                    ' Clear all RadioButtons and TextBoxes in the group
                    For Each ctrl In grp.Controls
                        If TypeOf ctrl Is RadioButton Then
                            DirectCast(ctrl, RadioButton).Checked = False
                        ElseIf TypeOf ctrl Is TextBox Then
                            DirectCast(ctrl, TextBox).Clear()
                        End If
                    Next
                End If
            End If
        Next
    End Sub

    Private Sub EditBTN_Click(sender As Object, e As EventArgs) Handles EditBTN.Click
        Select Case EditBTN.Text
            Case "Edit"
                TXTInit(True)
                SaveBTN.Enabled = True
                EditBTN.Text = "Cancel"
            Case "Cancel"
                TXTInit(False)
                GetRow()
                SaveBTN.Enabled = False
                EditBTN.Text = "Edit"
        End Select
    End Sub

    Private Sub SaveBTN_Click(sender As Object, e As EventArgs) Handles SaveBTN.Click
        ' Define the initial message object with "SetControllerConfig" type
        Dim message As New ClientMessage With {.Type = Commands.CommandType.SetControllerIP}

        ' Check for changes in the IP address settings
        Dim controllerIP As New SetControllerIP With {
            .ControllerID = CUInt(ControllersBox.SelectedValue),
            .IPAddress = IPAddressTXT.Text,
            .Netmask = NetmaskTXT.Text,
            .Gateway = GatewayTXT.Text,
            .Listener = ListenerTXT.Text
        }

        ' Only add SetControllerIP if any IP change was made
        If HasIPChanges(controllerIP) Then
            message.SetControllerIP = controllerIP
        End If

        ' Check for changes in the doors
        Dim doorChanges As New List(Of SetDoor)
        For doorNum As Integer = 1 To 4
            ' Get the selected Door Mode and Delay from the UI controls
            Dim doorMode As Integer = GetSelectedDoorMode(doorNum)
            Dim doorDelay As Integer = GetDoorDelay(doorNum) ' Assuming you have a way to fetch this value (e.g., from Delay Textboxes)

            ' Only add door changes if something has changed
            If HasDoorChanges(doorNum, doorMode, doorDelay) Then
                doorChanges.Add(New SetDoor With {
                .ControllerID = CUInt(ControllersBox.SelectedValue),
                .Door = doorNum,
                .Mode = doorMode,
                .Delay = doorDelay
            })
            End If
        Next

        ' Only add SetDoors if there are any door changes
        If doorChanges.Count <> 0 Then
            message.SetDoors = doorChanges
        End If

        ' Send the message to the server
        MainFRM.SendMessage(message)
    End Sub

    Private Function HasIPChanges(controllerIP As SetControllerIP) As Boolean
        For Each row As DataRow In ControllerDB.Rows
            If CUInt(row("Controller")) = controllerIP.ControllerID Then
                Return controllerIP.IPAddress <> row("Address").ToString() OrElse
                   controllerIP.Netmask <> row("Netmask").ToString() OrElse
                   controllerIP.Gateway <> row("Gateway").ToString() OrElse
                   controllerIP.Listener <> row("Listener").ToString()
            End If
        Next
        Return False ' If not found, assume no change
    End Function

    Private Function GetSelectedDoorMode(doorNum As Integer) As Integer
        Dim modeNames = [Enum].GetNames(Of Enums.DoorMode)()

        For Each modeName In modeNames
            Dim rbName = $"{modeName}{doorNum}"
            Dim rb = Controls.Find(rbName, True).FirstOrDefault()

            If rb IsNot Nothing AndAlso TypeOf rb Is RadioButton AndAlso DirectCast(rb, RadioButton).Checked Then
                Return [Enum].Parse(Of Enums.DoorMode)(modeName)
            End If
        Next

        Return Enums.DoorMode.Unknown
    End Function

    Private Function GetDoorDelay(doorNum As Integer) As Integer
        Dim delayTextBoxName = $"Delay{doorNum}"
        Dim delayTextBox = Controls.Find(delayTextBoxName, True).FirstOrDefault()

        If delayTextBox IsNot Nothing AndAlso TypeOf delayTextBox Is TextBox Then
            Dim delay As Integer
            If Integer.TryParse(DirectCast(delayTextBox, TextBox).Text, delay) Then
                Return delay
            End If
        End If

        Return 0 ' Default value if not found or parsing fails
    End Function

    Private Function HasDoorChanges(doorNum As Integer, doorMode As Integer, doorDelay As Integer) As Boolean
        Dim controllerID As UInteger = CUInt(ControllersBox.SelectedValue)

        For Each row As DataRow In DoorDB.Rows
            If CUInt(row("Controller")) = controllerID AndAlso CInt(row("Door")) = doorNum Then
                Dim currentMode As Integer = Convert.ToInt32(row("Mode"))
                Dim currentDelay As Integer = Convert.ToInt32(row("Delay"))
                Return currentMode <> doorMode OrElse currentDelay <> doorDelay
            End If
        Next
        Return False ' If not found, assume no change
    End Function

    Private Sub UpdateRow(Controller As UInteger, IPAddress As String, Netmask As String, Gateway As String, Listener As String)
        For Each row As DataRow In ControllerDB.Rows
            If CUInt(row("Controller")) = Controller Then
                row("Address") = IPAddress
                row("Netmask") = Netmask
                row("Gateway") = Gateway
                row("Listener") = Listener
                Exit For
            End If
        Next

        Invoke(Sub()
                   EditBTN.PerformClick()
               End Sub)

    End Sub

    Private Sub ClearData()
        ' Clear all TextBoxes
        Dim a As Control
        For Each a In Controls
            If TypeOf a Is TextBox Then
                a.Text = Nothing
            End If
        Next
    End Sub

    Private Sub GetRow()
        If ControllersBox.SelectedItem IsNot Nothing Then

            ' Populate TextBoxes
            Dim rowView As DataRowView = CType(ControllersBox.SelectedItem, DataRowView)
            Dim controllerID As UInteger
            '     If Not String.IsNullOrWhiteSpace(rowView("Controller").ToString()) AndAlso
            'UInteger.TryParse(rowView("Controller").ToString(), controllerID) Then

            PopulateBoxes(
            rowView("Address").ToString(),
            rowView("Netmask").ToString(),
            rowView("Gateway").ToString(),
            rowView("MAC").ToString(),
            rowView("Listener").ToString(),
            CUInt(If(String.IsNullOrWhiteSpace(rowView("Index")), "0", rowView("Index").ToString())),
            rowView("Version").ToString(),
            rowView("Date").ToString())

            ' Populate RadioButtons
            Dim doorRows = DoorDB.Select($"Controller = '{controllerID}'")

            For door As Integer = 1 To Integer.Parse(controllerID.ToString().Substring(0, 1))
                Dim currentDoor = door ' Fix the closure capture issue

                Dim doorRow = doorRows.FirstOrDefault(Function(r) Convert.ToInt32(r("Door")) = currentDoor)
                If doorRow IsNot Nothing Then
                    ' Set the door mode RadioButton
                    Dim groupBox As GroupBox = CType(Controls.Find($"GroupBox{currentDoor}", True).FirstOrDefault(), GroupBox)
                    If groupBox IsNot Nothing Then
                        Dim modeValue As Integer = Convert.ToInt32(doorRow("Mode"))
                        PopulateRadio(groupBox, currentDoor, modeValue)
                    End If

                    ' Set the delay TextBox
                    Dim delayBox As TextBox = CType(Controls.Find($"Delay{currentDoor}", True).FirstOrDefault(), TextBox)
                    If delayBox IsNot Nothing Then
                        delayBox.Text = doorRow("Delay").ToString()
                    End If
                End If
            Next

        End If
    End Sub
#End Region

    Private Sub GetDoorsBTN_Click(sender As Object, e As EventArgs) Handles GetDoorsBTN.Click
        DataGridView1.DataSource = DoorDB

    End Sub

    Private Sub GetControllersBTN_Click(sender As Object, e As EventArgs) Handles GetControllersBTN.Click
        ControllerDB.Clear()

        GetControllers()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim message As New ClientMessage With {
            .Type = Commands.CommandType.GetController,
            .ControllerID = CUInt(ControllersBox.SelectedValue)
        }

        MainFRM.SendMessage(message)
    End Sub

    Private Sub AddControllerBTN_Click(sender As Object, e As EventArgs) Handles AddControllerBTN.Click
        Dim message As New ClientMessage With {
            .Type = Commands.CommandType.SetTime,
            .ControllerID = CUInt(ControllersBox.SelectedValue)
        }

        MainFRM.SendMessage(message)
    End Sub

    Private Sub SetDefaultBTN_Click(sender As Object, e As EventArgs) Handles SetDefaultBTN.Click
        If ControllersBox.SelectedValue IsNot Nothing Then

            Dim Controller As UInteger = ControllersBox.SelectedValue
            Dim res As DialogResult = MsgBox(
                $"Are you sure you want to reset controller '{Controller}' to default?", MsgBoxStyle.YesNo)

            If res = DialogResult.Yes Then
                Dim message As New ClientMessage With {
                    .Type = Commands.CommandType.RestoreDefaultParameters,
                    .ControllerID = Controller
                }

                MainFRM.SendMessage(message)
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub RecordSpecialEventsBTN_Click(sender As Object, e As EventArgs) Handles RecordSpecialEventsBTN.Click
        If ControllersBox.SelectedValue IsNot Nothing Then
            Dim Controller As UInteger = ControllersBox.SelectedValue
            Dim message As New ClientMessage With {
                .Type = Commands.CommandType.RecordSpecialEvents,
                .ControllerID = Controller
            }

            MainFRM.SendMessage(message)
        End If
    End Sub

    Private Sub GetCardsBTN_Click(sender As Object, e As EventArgs) Handles GetCardsBTN.Click
        'Dim f As New CardFRM With {.Tag = ControllersBox.SelectedValue}
        'RemoveHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage

        'f.ShowDialog()

        'AddHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage
    End Sub

    Private Sub SetDateBTN_Click(sender As Object, e As EventArgs) Handles SetDateBTN.Click
        If ControllersBox.SelectedValue IsNot Nothing Then
            Dim Controller As UInteger = ControllersBox.SelectedValue
            Dim message As New ClientMessage With {.Type = Commands.CommandType.SetTime, .ControllerID = Controller}

            MainFRM.SendMessage(message)
        End If
    End Sub

    Private Sub GetSQLControllers()
        ' Mimic SQL querry
        SQLControllerDB.Rows.Add(423155481, "101", "102", "103", "104")
        SQLControllerDB.Rows.Add(425026282, "105", "106", "107", "108")

        '' Real SQL querry
        'Dim cmd As New SqlCommand
        'Dim con As New SqlConnection(MainFRM.ConString)
        'Dim da As New SqlDataAdapter

        'Try
        '    con.Open()
        '    cmd.Connection = con

        '    cmd.CommandText = "SELECT Controller FROM vControllers"
        '    da.SelectCommand = cmd
        '    da.Fill(SQLControllerDB)
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'Finally
        '    con.Close()
        '    con.Dispose()
        '    cmd.Dispose()
        '    da.Dispose()
        'End Try
    End Sub


End Class