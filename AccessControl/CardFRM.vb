Imports System.Net.NetworkInformation
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json

Public Class CardFRM
    Dim ControllerID As UInteger
    Dim ControllerList As List(Of UInteger)

#Region "Form Control"
    Private Sub CardFRM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Handler for messages
        AddHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage

        ' Initialize ControllerDB
        ControllerList = New List(Of UInteger)

        DateBOX.MinDate = Now.Date.AddYears(1)

        SelectedCRadioBTN.Checked = True

        InitCardDB()

        ' Get Controllers
        GetControllers()


        GetCards()

    End Sub

    Private Sub CloseBTN_Click(sender As Object, e As EventArgs) Handles CloseBTN.Click
        Close()
    End Sub

    Private Sub CardFRM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler MainFRM.ServerMessageReceived, AddressOf HandleServerMessage
    End Sub
#End Region

    Private Sub HandleServerMessage(data As String)
        Dim settings As New JsonSerializerSettings With {.Converters = {New Converters.StringEnumConverter()}}
        Dim message As ServerMessage = JsonConvert.DeserializeObject(Of ServerMessage)(data, settings)

        Try
            Dim command As Commands.CommandType = message.Type

            Select Case command
                Case Commands.CommandType.GetControllerList
                    If message.Controllers IsNot Nothing Then
                        For Each ctrl In message.Controllers
                            Invoke(Sub()
                                       ControllerList.Add(ctrl.ControllerID)
                                       ControllersBox.Items.Add(ctrl.ControllerID)
                                   End Sub)
                        Next
                    End If
                Case Commands.CommandType.GetCards
                    For Each c In message.CardList
                        Dim Name = GetCardName(c.CardNumber)

                        CardDB.Rows.Add(
                            c.ControllerID,
                            c.CardNumber,
                            Name.FirstName,
                            Name.LastName,
                            c.Door1,
                            c.Door2,
                            c.Door3,
                            c.Door4,
                            If(String.IsNullOrWhiteSpace(c.PIN), 0, c.PIN),
                            c.StartDate,
                            c.EndDate
                            )
                    Next

                    Invoke(Sub()
                               CardView = New DataView(CardDB)
                               CardDGV.DataSource = CardView
                               ControllersBox.ValueMember = "ControllerID"
                               AllRadioBTN.Checked = True
                               ControllersBox.SelectedIndex = 0

                               Controls.Remove(WaitLBL)
                           End Sub)
                Case Commands.CommandType.PutCard
                    For Each Response As PutCardResponse In message.PutCardResponse
                        If Response.Success Then
                            Invoke(Sub()
                                       Dim Name = GetCardName(Response.CardNumber)
                                       AddOrUpdateCardRow(Response.ControllerID,
                                                          Response.CardNumber,
                                                          Name.FirstName,
                                                          Name.LastName,
                                                          If(Door1CHK.Checked, 1, 0),
                                                          If(Door2CHK.Checked, 1, 0),
                                                          If(Door3CHK.Checked, 1, 0),
                                                          If(Door4CHK.Checked, 1, 0),
                                                          CInt(If(String.IsNullOrWhiteSpace(PinTXT.Text), 0, PinTXT.Text)),
                                                          DateOnly.FromDateTime(Date.Now),
                                                          DateOnly.FromDateTime(DateBOX.Value)
                                                          )

                                       ResetNewCard()
                                   End Sub)

                            MsgBox($"Card '{Response.CardNumber}' successfully added to Controller '{Response.ControllerID}'")
                        Else
                            MsgBox($"An error accured while trying to add Card '{Response.CardNumber}'" &
                                   $" to Controller '{Response.ControllerID}'.{Environment.NewLine}{Response.ErrorMessage}")
                        End If
                    Next

                Case Commands.CommandType.DeleteCard
                    For Each d In message.DeleteCardResponse
                        If d.Success = True Then
                            MsgBox($"Card '{d.CardNumber}' deleted from Controller '{d.ControllerID}'")

                            Invoke(Sub()
                                       For Each row As DataRow In CardDB.Rows
                                           If CInt(row("CardNumber")) = d.CardNumber AndAlso
                                           CUInt(row("ControllerID")) = d.ControllerID Then
                                               CardDB.Rows.Remove(row)
                                               Exit For
                                           End If
                                       Next

                                       DelCardReset()
                                   End Sub)
                        ElseIf d.Success = False Then
                            MsgBox($"Error deleting Card '{d.CardNumber}' from Controller '{d.ControllerID}':" &
                                   $"{Environment.NewLine}{d.ErrorMessage}")

                            Invoke(Sub()
                                       DelCardReset()
                                   End Sub)
                        End If
                    Next
                Case Commands.CommandType.SetDoorPasscodes
                    Dim SetPin = message.SetDoorPasscodesResponse
                    MsgBox($"Set PIN from Door {SetPin.Door} on Controller {SetPin.ControllerID} was " &
                           $"{If(SetPin.Success, "Successfull.",
                           $"Unuccessfull.{Environment.NewLine}Error:{Environment.NewLine}{SetPin.ErrorMessage}")}")

                    Invoke(Sub()
                               CancelPINBTN.PerformClick()
                           End Sub)
            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#Region "Get Controllers and Cards"
    Private Sub GetControllers()
        Dim message As New ClientMessage With {.Type = Commands.CommandType.GetControllerList}

        MainFRM.SendMessage(message)
    End Sub

    Private Sub GetCards(Optional controllerId As UInteger? = Nothing, Optional cardNumber As Integer? = Nothing)
        Dim Cards As New GetCardRequest With {.ControllerID = controllerId, .CardNumber = cardNumber}
        Dim message As New ClientMessage With {.Type = Commands.CommandType.GetCards, .GetCardRequest = Cards}

        MainFRM.SendMessage(message)
    End Sub

    Private Function GetCardName(CardNumber As Integer) As (FirstName As String, LastName As String)
        Return ("First", "Last")
    End Function
#End Region

#Region "Text boxes manipulations"
    Private Sub CardTXT_KeyDown(sender As Object, e As KeyEventArgs) Handles CardTXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub CardTXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CardTXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub CardTXT_TextChanged(sender As Object, e As EventArgs) Handles CardTXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        CardTXT.Text = digitsOnly.Replace(CardTXT.Text, "")

        If String.IsNullOrWhiteSpace(CardTXT.Text) Then SaveNewCardBTN.Enabled = False Else SaveNewCardBTN.Enabled = True
    End Sub

    Private Sub PinTXT_KeyDown(sender As Object, e As KeyEventArgs) Handles PinTXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub PinTXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PinTXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub PinTXT_TextChanged(sender As Object, e As EventArgs) Handles PinTXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        PinTXT.Text = digitsOnly.Replace(PinTXT.Text, "")
    End Sub

    Private Sub DelCardTXT_KeyDown(sender As Object, e As KeyEventArgs) Handles DelCardTXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub DelCardTXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DelCardTXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub DelCardTXT_TextChanged(sender As Object, e As EventArgs) Handles DelCardTXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        DelCardTXT.Text = digitsOnly.Replace(DelCardTXT.Text, "")

        If String.IsNullOrWhiteSpace(DelCardTXT.Text) Then DelCardBTN.Enabled = False Else DelCardBTN.Enabled = True
    End Sub
#End Region

#Region "Card DB"
    Dim CardDB As DataTable
    Private CardView As DataView
    Private Sub InitCardDB()
        CardDB = New DataTable

        CardDB.Columns.Add("ControllerID", GetType(UInteger))
        CardDB.Columns.Add("CardNumber")
        CardDB.Columns.Add("FirstName")
        CardDB.Columns.Add("LastName")
        CardDB.Columns.Add("Door1")
        CardDB.Columns.Add("Door2")
        CardDB.Columns.Add("Door3")
        CardDB.Columns.Add("Door4")
        CardDB.Columns.Add("PIN")
        CardDB.Columns.Add("StartDate")
        CardDB.Columns.Add("EndDate")
    End Sub

    Private Sub ApplyCardFilter()
        If CardView Is Nothing Then
            MsgBox("CardView is not initialized")
            Return
        End If

        If ByControllerRadioBTN.Checked Then
            If ControllersBox.SelectedItem IsNot Nothing Then
                'Dim controllerId As UInteger = CUInt(ControllersBox.SelectedItem)
                CardView.RowFilter = $"ControllerID = {ControllerID}"
            Else
                CardView.RowFilter = ""
            End If
        Else
            CardView.RowFilter = ""
        End If
    End Sub

    Private Sub AddOrUpdateCardRow(controllerId As UInteger, cardNumber As Integer, firstName As String, lastName As String,
                                   door1 As Byte, door2 As Byte, door3 As Byte, door4 As Byte, pin As Integer,
                                   startDate As DateOnly, endDate As DateOnly)

        Dim existingRow = CardDB.Rows.Cast(Of DataRow).FirstOrDefault(
        Function(r) CInt(r("ControllerID")) = controllerId AndAlso CInt(r("CardNumber")) = cardNumber)

        If existingRow IsNot Nothing Then
            ' Update existing row
            existingRow("FirstName") = firstName
            existingRow("LastName") = lastName
            existingRow("Door1") = door1
            existingRow("Door2") = door2
            existingRow("Door3") = door3
            existingRow("Door4") = door4
            existingRow("PIN") = pin
            existingRow("StartDate") = startDate
            existingRow("EndDate") = endDate
        Else
            ' Add new row
            CardDB.Rows.Add(controllerId, cardNumber, firstName, lastName,
                        door1, door2, door3, door4, pin, startDate, endDate)
        End If
    End Sub

    Private Sub AllRadioBTN_CheckedChanged(sender As Object, e As EventArgs) Handles AllRadioBTN.CheckedChanged
        ApplyCardFilter()
    End Sub

    Private Sub ByControllerRadioBTN_CheckedChanged(sender As Object, e As EventArgs) Handles ByControllerRadioBTN.CheckedChanged
        ApplyCardFilter()
    End Sub

    Private Sub ControllersBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ControllersBox.SelectedIndexChanged
        ControllerID = CUInt(ControllersBox.SelectedItem)
        PINBOX.Items.Clear()

        Dim DoorCount As Integer = Integer.Parse(ControllerID.ToString().Substring(0, 1))
        Select Case DoorCount
            Case 1
                Door2CHK.Enabled = False
                Door3CHK.Enabled = False
                Door4CHK.Enabled = False
                PINBOX.Items.Add("1")
            Case 4
                Door2CHK.Enabled = True
                Door3CHK.Enabled = True
                Door4CHK.Enabled = True
                PINBOX.Items.Add("1")
                PINBOX.Items.Add("2")
                PINBOX.Items.Add("3")
                PINBOX.Items.Add("4")
            Case Else
                MsgBox($"DoorCount ({DoorCount}) is out of bands")
        End Select

        If ByControllerRadioBTN.Checked Then
            ApplyCardFilter()
        End If
    End Sub

    Private Sub CardDGV_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles CardDGV.CellMouseDown
        'If e.ColumnIndex <> -1 AndAlso e.RowIndex <> -1 AndAlso e.Button = MouseButtons.Left Then
        '    'ContextMenuStrip1.

        '    Dim c As DataGridViewCell = TryCast(sender, DataGridView)(e.ColumnIndex, e.RowIndex)

        '    If Not c.Selected Then
        '        c.DataGridView.ClearSelection()
        '        c.DataGridView.CurrentCell = c
        '        c.Selected = True

        '        ContextMenuStrip1.Items.Insert(0, New ToolStripLabel("fdgh"))
        '        ContextMenuStrip1.Show()
        '    Else
        '        ContextMenuStrip1.Items.Insert(0, New ToolStripLabel("bla"))
        '        ContextMenuStrip1.Show()
        '    End If
        'End If

    End Sub

    Private Sub CardDGV_MouseDown(sender As Object, e As MouseEventArgs) Handles CardDGV.MouseDown
        If e.Button = MouseButtons.Right Then
            Dim hit As DataGridView.HitTestInfo = CardDGV.HitTest(e.X, e.Y)

            If hit.Type = DataGridViewHitTestType.Cell Then
                ' Select the row under the mouse
                CardDGV.ClearSelection()
                CardDGV.Rows(hit.RowIndex).Selected = True

                ' Optional: Set current cell (for keyboard nav/highlight)
                CardDGV.CurrentCell = CardDGV.Rows(hit.RowIndex).Cells(hit.ColumnIndex)

                ' Update label with the card value (e.g., from column "CardNumber")
                Dim cardNumber As String = CardDGV.Rows(hit.RowIndex).Cells("CardNumber").Value.ToString()
                Dim Controller As String = CardDGV.Rows(hit.RowIndex).Cells("ControllerID").Value.ToString()
                'SelectedCardLabel.Text = $"Selected Card: {cardValue}"
                'ContextMenuStrip1.Items.Insert(0, New ToolStripLabel(cardValue))

                'Dim DeleteCardBTN As New ToolStripMenuItem() With {
                '    .Text = "Delete Card",
                '    .Name = "DeleteCardBTN"
                '}
                'AddHandler DeleteCardBTN.Click, AddressOf DeleteCardBTN_Click
                'ContextMenuStrip1.Items.Add(DeleteCardBTN)

                ' Show context menu at mouse location
                'ContextMenuStrip1.Show(CardDGV, e.Location)
                'CardCMS.Show(CardDGV, e.Location)
                Dim C As New CardCMS With {
                    .Tag = New CardContextInfo(Controller, cardNumber)
                }

                C.Show(CardDGV, e.Location)
            End If
        End If
    End Sub
#End Region

#Region "New Card"
    Private Sub SaveNewCardBTN_Click(sender As Object, e As EventArgs) Handles SaveNewCardBTN.Click
        If ControllerID > 0 AndAlso CardTXT.Text IsNot Nothing Then
            PutCard()
        End If
    End Sub

    Private Sub PutCard()
        Dim doors As New Dictionary(Of Integer, Byte) From {
            {1, If(Door1CHK.Checked, CByte(1), CByte(0))},
            {2, If(Door2CHK.Checked, CByte(1), CByte(0))},
            {3, If(Door3CHK.Checked, CByte(1), CByte(0))},
            {4, If(Door4CHK.Checked, CByte(1), CByte(0))}
        }

        Dim NewCard As New PutCardRequest With {
            .ControllerID = CUInt(ControllerID),
            .CardNumber = UInteger.Parse(CardTXT.Text),
            .EndDate = DateBOX.Value,
            .Doors = doors,
            .PIN = If(Not String.IsNullOrWhiteSpace(PinTXT.Text), CInt(PinTXT.Text), 0)
        }

        Dim CardList As New List(Of PutCardRequest) From {NewCard}

        Dim message As New ClientMessage With {.Type = Commands.CommandType.PutCard, .PutCardRequest = CardList}

        MainFRM.SendMessage(message)
    End Sub

    Private Sub CancelNewCardBTN_Click(sender As Object, e As EventArgs) Handles CancelNewCardBTN.Click
        ResetNewCard()
    End Sub

    Private Sub ResetNewCard()
        CardTXT.Clear()
        DateBOX.Value = Date.Now.AddYears(1)
        Door1CHK.Checked = False
        Door2CHK.Checked = False
        Door3CHK.Checked = False
        Door4CHK.Checked = False
        PinTXT.Clear()
    End Sub
#End Region

#Region "Delete Card"
    Private Sub DelCardBTN_Click(sender As Object, e As EventArgs) Handles DelCardBTN.Click
        If ControllerID > 0 AndAlso DelCardTXT.Text IsNot Nothing Then
            DelCard(ControllerID, CUInt(DelCardTXT.Text))
        ElseIf ControllerID > 0 AndAlso DelCardTXT.Text = Nothing Then
            DelCard(ControllerID)
        End If
    End Sub

    Private Sub DelCard(Optional Controller As UInteger = 0, Optional CardNumber As UInteger = 0)
        If SelectedCRadioBTN.Checked Then
            If ControllerID > 0 AndAlso CardNumber > 0 Then
                Dim DeleteCard As New DeleteCardRequest With {
                    .ControllerID = ControllerID,
                    .CardNumber = UInteger.Parse(DelCardTXT.Text)
                }

                Dim message As New ClientMessage With {
                    .Type = Commands.CommandType.DeleteCard,
                    .DeleteCardRequest = DeleteCard
                }

                MainFRM.SendMessage(message)
            ElseIf ControllerID > 0 AndAlso CardNumber = 0 Then
                Dim DeleteCard As New DeleteCardRequest With {
                    .ControllerID = ControllerID
                }

                Dim message As New ClientMessage With {
                    .Type = Commands.CommandType.DeleteCard,
                    .DeleteCardRequest = DeleteCard
                }

                MainFRM.SendMessage(message)
            End If
        ElseIf AllCRadioBTN.Checked Then
            MsgBox("this feature is not ready")
            Exit Sub
        End If
    End Sub

    Private Sub CancelDelCardBTN_Click(sender As Object, e As EventArgs) Handles CancelDelCardBTN.Click
        DelCardReset()
    End Sub

    Private Sub DelCardReset()
        DelCardTXT.Text = Nothing
        SelectedCRadioBTN.Checked = True
    End Sub
#End Region

#Region "Set Door PIN"
    Private Sub PIN1TXT_KeyDown(sender As Object, e As KeyEventArgs) Handles PIN1TXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub PIN1TXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PIN1TXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub PIN1TXT_TextChanged(sender As Object, e As EventArgs) Handles PIN1TXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        PIN1TXT.Text = digitsOnly.Replace(PIN1TXT.Text, "")

        'If String.IsNullOrWhiteSpace(CardTXT.Text) Then SaveNewCardBTN.Enabled = False Else SaveNewCardBTN.Enabled = True
    End Sub

    Private Sub PIN2TXT_KeyDown(sender As Object, e As KeyEventArgs) Handles PIN2TXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub PIN2TXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PIN2TXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub PIN2TXTTXT_TextChanged(sender As Object, e As EventArgs) Handles PIN2TXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        PIN2TXT.Text = digitsOnly.Replace(PIN2TXT.Text, "")

        'If String.IsNullOrWhiteSpace(CardTXT.Text) Then SaveNewCardBTN.Enabled = False Else SaveNewCardBTN.Enabled = True
    End Sub

    Private Sub PIN3TXT_KeyDown(sender As Object, e As KeyEventArgs) Handles PIN3TXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub PIN3TXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PIN3TXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub PIN3TXT_TextChanged(sender As Object, e As EventArgs) Handles PIN3TXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        PIN3TXT.Text = digitsOnly.Replace(PIN3TXT.Text, "")

        'If String.IsNullOrWhiteSpace(CardTXT.Text) Then SaveNewCardBTN.Enabled = False Else SaveNewCardBTN.Enabled = True
    End Sub

    Private Sub PIN4TXT_KeyDown(sender As Object, e As KeyEventArgs) Handles PIN4TXT.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub PIN4TXT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PIN4TXT.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub PIN4TXT_TextChanged(sender As Object, e As EventArgs) Handles PIN4TXT.TextChanged
        Dim digitsOnly As New Regex("[^\d]")
        PIN4TXT.Text = digitsOnly.Replace(PIN4TXT.Text, "")

        'If String.IsNullOrWhiteSpace(CardTXT.Text) Then SaveNewCardBTN.Enabled = False Else SaveNewCardBTN.Enabled = True
    End Sub
#End Region

    Private Sub SavePINBTN_Click(sender As Object, e As EventArgs) Handles SavePINBTN.Click
        If PINBOX.SelectedItem IsNot Nothing Then
            Dim PIN1 As UInteger = If(String.IsNullOrWhiteSpace(PIN1TXT.Text), 0, CUInt(PIN1TXT.Text))
            Dim PIN2 As UInteger = If(String.IsNullOrWhiteSpace(PIN2TXT.Text), 0, CUInt(PIN2TXT.Text))
            Dim PIN3 As UInteger = If(String.IsNullOrWhiteSpace(PIN3TXT.Text), 0, CUInt(PIN3TXT.Text))
            Dim PIN4 As UInteger = If(String.IsNullOrWhiteSpace(PIN4TXT.Text), 0, CUInt(PIN4TXT.Text))
            'Dim PINs As UInteger() = {PIN1, PIN2, PIN3, PIN4}
            Dim PINs As UInteger() = {
                If(String.IsNullOrWhiteSpace(PIN1TXT.Text), 0, CUInt(PIN1TXT.Text)),
                If(String.IsNullOrWhiteSpace(PIN2TXT.Text), 0, CUInt(PIN2TXT.Text)),
                If(String.IsNullOrWhiteSpace(PIN3TXT.Text), 0, CUInt(PIN3TXT.Text)),
                If(String.IsNullOrWhiteSpace(PIN4TXT.Text), 0, CUInt(PIN4TXT.Text))
            }

            Dim Door As Integer = CInt(PINBOX.SelectedItem)

            Dim DoorPIN As New SetDoorPasscodesRequest With {.ControllerID = ControllerID, .Door = Door, .Passcodes = PINs}

            Dim message As New ClientMessage With {
                .Type = Commands.CommandType.SetDoorPasscodes,
                .SetDoorPasscodesRequest = DoorPIN
            }

            MainFRM.SendMessage(message)
        Else
            MsgBox("Please select a door")
        End If
    End Sub

    Private Sub CancelPINBTN_Click(sender As Object, e As EventArgs) Handles CancelPINBTN.Click
        PIN1TXT.Text = Nothing
        PIN2TXT.Text = Nothing
        PIN3TXT.Text = Nothing
        PIN4TXT.Text = Nothing
        PINBOX.SelectedItem = Nothing
    End Sub

    Private Sub DelPINBTN_Click(sender As Object, e As EventArgs) Handles DelPINBTN.Click
        If PINBOX.SelectedItem IsNot Nothing Then
            Dim Door As Integer = CInt(PINBOX.SelectedItem)
            Dim DoorPIN As New SetDoorPasscodesRequest With {
                .ControllerID = ControllerID,
                .Door = Door,
                .Passcodes = {0, 0, 0, 0}
            }

            Dim message As New ClientMessage With {
                .Type = Commands.CommandType.SetDoorPasscodes,
                .SetDoorPasscodesRequest = DoorPIN
            }

            MainFRM.SendMessage(message)
        Else
            MsgBox("Please select a door")
        End If
    End Sub
End Class