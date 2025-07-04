<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddControllerFRM
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        SaveBTN = New Button()
        CancelBTN = New Button()
        SuspendLayout()
        ' 
        ' SaveBTN
        ' 
        SaveBTN.Location = New Point(12, 415)
        SaveBTN.Name = "SaveBTN"
        SaveBTN.Size = New Size(75, 23)
        SaveBTN.TabIndex = 0
        SaveBTN.Text = "Save"
        SaveBTN.UseVisualStyleBackColor = True
        ' 
        ' CancelBTN
        ' 
        CancelBTN.Location = New Point(713, 415)
        CancelBTN.Name = "CancelBTN"
        CancelBTN.Size = New Size(75, 23)
        CancelBTN.TabIndex = 1
        CancelBTN.Text = "Cancel"
        CancelBTN.UseVisualStyleBackColor = True
        ' 
        ' AddControllerFRM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(CancelBTN)
        Controls.Add(SaveBTN)
        Name = "AddControllerFRM"
        Text = "AddControllerFRM"
        ResumeLayout(False)
    End Sub

    Friend WithEvents SaveBTN As Button
    Friend WithEvents CancelBTN As Button
End Class
