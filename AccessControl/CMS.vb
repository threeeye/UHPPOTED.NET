Imports Newtonsoft.Json

Public Class CMS
    Inherits ContextMenuStrip
    Public Sub New()
        Dim OpenDoor As New ToolStripMenuItem() With {.Name = "OpenDoor", .Text = "Open Door"}
        AddHandler OpenDoor.Click, AddressOf OpenDoor_Click
        Items.Add(OpenDoor)
    End Sub

    Private Sub OpenDoor_Click(sender As Object, e As EventArgs)
        Dim controllerID As UInteger = SourceControl.Tag.ToString.Split("|"c)(0)
        Dim door As Byte = Convert.ToByte(SourceControl.Tag.ToString.Split("|"c)(1))

        Dim openDoorRequest As New OpenDoorRequest With {
            .ControllerID = controllerID,
            .Door = door
        }

        Dim message As New ClientMessage With {
            .Type = Commands.CommandType.OpenDoor,
            .OpenDoorRequest = openDoorRequest
        }
        'MsgBox(JsonConvert.SerializeObject(message))
        MainFRM.SendMessage(message)
    End Sub
End Class