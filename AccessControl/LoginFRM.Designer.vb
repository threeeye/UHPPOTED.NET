<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginFRM
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
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        LoginBTN = New Button()
        CloseBTN = New Button()
        Label1 = New Label()
        Label2 = New Label()
        UserTXT = New TextBox()
        PassTXT = New TextBox()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(3, 128)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 0
        Button1.Text = "Admin"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(84, 128)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 1
        Button2.Text = "Monitor"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(165, 128)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 23)
        Button3.TabIndex = 2
        Button3.Text = "Config"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' LoginBTN
        ' 
        LoginBTN.Location = New Point(18, 99)
        LoginBTN.Name = "LoginBTN"
        LoginBTN.Size = New Size(75, 23)
        LoginBTN.TabIndex = 3
        LoginBTN.Text = "Login"
        LoginBTN.UseVisualStyleBackColor = True
        ' 
        ' CloseBTN
        ' 
        CloseBTN.Location = New Point(134, 99)
        CloseBTN.Name = "CloseBTN"
        CloseBTN.Size = New Size(75, 23)
        CloseBTN.TabIndex = 4
        CloseBTN.Text = "Close"
        CloseBTN.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 16)
        Label1.Name = "Label1"
        Label1.Size = New Size(66, 15)
        Label1.TabIndex = 5
        Label1.Text = "User name:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(18, 45)
        Label2.Name = "Label2"
        Label2.Size = New Size(60, 15)
        Label2.TabIndex = 6
        Label2.Text = "Password:"
        ' 
        ' UserTXT
        ' 
        UserTXT.Location = New Point(84, 13)
        UserTXT.Name = "UserTXT"
        UserTXT.Size = New Size(100, 23)
        UserTXT.TabIndex = 7
        ' 
        ' PassTXT
        ' 
        PassTXT.Location = New Point(84, 42)
        PassTXT.Name = "PassTXT"
        PassTXT.PasswordChar = "*"c
        PassTXT.Size = New Size(100, 23)
        PassTXT.TabIndex = 8
        ' 
        ' LoginFRM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(264, 198)
        Controls.Add(PassTXT)
        Controls.Add(UserTXT)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(CloseBTN)
        Controls.Add(LoginBTN)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        MaximizeBox = False
        MinimizeBox = False
        Name = "LoginFRM"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Access Control Login"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents LoginBTN As Button
    Friend WithEvents CloseBTN As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents UserTXT As TextBox
    Friend WithEvents PassTXT As TextBox
End Class
