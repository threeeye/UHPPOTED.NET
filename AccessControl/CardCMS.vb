Imports System.ComponentModel
Public Class CardCMS
    Inherits ContextMenuStrip
    Private CardInfo As CardContextInfo
    Public Sub New()
        Dim DeleteCardBTN As New ToolStripMenuItem() With {
                 .Text = "Delete Card",
                 .Name = "DeleteCardBTN"
             }
        AddHandler DeleteCardBTN.Click, AddressOf DeleteCardBTN_Click
        Items.Add(DeleteCardBTN)
    End Sub

    Private Sub DeleteCardBTN_Click(sender As Object, e As EventArgs)
        Dim result As DialogResult =
            MsgBox($"Are you sure you want to remove Card '{CardInfo.CardNumber}' from Controller '{CardInfo.ControllerID}'",
                   MsgBoxStyle.YesNo)

        If result = DialogResult.Yes Then
            Dim DeleteCard As New DeleteCardRequest With {
                .ControllerID = CardInfo.ControllerID,
                .CardNumber = CardInfo.CardNumber
            }

            Dim message As New ClientMessage With {.Type = Commands.CommandType.DeleteCard, .DeleteCardRequest = DeleteCard}

            MainFRM.SendMessage(message)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub CardCMS_Opening(sender As Object, e As CancelEventArgs) Handles Me.Opening
        CardInfo = TryCast(Me.Tag, CardContextInfo)
        If CardInfo IsNot Nothing Then
            Items.Insert(0, New ToolStripLabel(CardInfo.CardNumber.ToString()))
        End If
    End Sub

    Private Sub CardCMS_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles Me.Closing
        Items.RemoveAt(0)
    End Sub
End Class