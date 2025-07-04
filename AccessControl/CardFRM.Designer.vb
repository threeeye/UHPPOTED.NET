<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CardFRM
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
        CloseBTN = New Button()
        DelCardTXT = New TextBox()
        CardTXT = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        DateBOX = New DateTimePicker()
        Door1CHK = New CheckBox()
        Door2CHK = New CheckBox()
        Door3CHK = New CheckBox()
        Door4CHK = New CheckBox()
        Label3 = New Label()
        PinTXT = New TextBox()
        GroupBox1 = New GroupBox()
        CancelNewCardBTN = New Button()
        SaveNewCardBTN = New Button()
        ControllersBox = New ListBox()
        CardDGV = New DataGridView()
        AllRadioBTN = New RadioButton()
        ByControllerRadioBTN = New RadioButton()
        WaitLBL = New Label()
        GroupBox2 = New GroupBox()
        AllCRadioBTN = New RadioButton()
        SelectedCRadioBTN = New RadioButton()
        CancelDelCardBTN = New Button()
        DelCardBTN = New Button()
        Label4 = New Label()
        GroupBox3 = New GroupBox()
        DelPINBTN = New Button()
        Label9 = New Label()
        Label8 = New Label()
        Label7 = New Label()
        PIN3TXT = New TextBox()
        PIN4TXT = New TextBox()
        PIN2TXT = New TextBox()
        PINBOX = New ComboBox()
        Label6 = New Label()
        PIN1TXT = New TextBox()
        Label5 = New Label()
        CancelPINBTN = New Button()
        SavePINBTN = New Button()
        GroupBox1.SuspendLayout()
        CType(CardDGV, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox2.SuspendLayout()
        GroupBox3.SuspendLayout()
        SuspendLayout()
        ' 
        ' CloseBTN
        ' 
        CloseBTN.Location = New Point(988, 597)
        CloseBTN.Name = "CloseBTN"
        CloseBTN.Size = New Size(75, 23)
        CloseBTN.TabIndex = 2
        CloseBTN.Text = "Close"
        CloseBTN.UseVisualStyleBackColor = True
        ' 
        ' DelCardTXT
        ' 
        DelCardTXT.Location = New Point(96, 22)
        DelCardTXT.Name = "DelCardTXT"
        DelCardTXT.Size = New Size(98, 23)
        DelCardTXT.TabIndex = 3
        ' 
        ' CardTXT
        ' 
        CardTXT.Location = New Point(94, 31)
        CardTXT.Name = "CardTXT"
        CardTXT.Size = New Size(100, 23)
        CardTXT.TabIndex = 6
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(6, 34)
        Label1.Name = "Label1"
        Label1.Size = New Size(82, 15)
        Label1.TabIndex = 7
        Label1.Text = "Card Number:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(32, 68)
        Label2.Name = "Label2"
        Label2.Size = New Size(56, 15)
        Label2.TabIndex = 8
        Label2.Text = "End date:"
        ' 
        ' DateBOX
        ' 
        DateBOX.Format = DateTimePickerFormat.Short
        DateBOX.Location = New Point(94, 62)
        DateBOX.Name = "DateBOX"
        DateBOX.Size = New Size(100, 23)
        DateBOX.TabIndex = 9
        ' 
        ' Door1CHK
        ' 
        Door1CHK.AutoSize = True
        Door1CHK.Location = New Point(94, 100)
        Door1CHK.Name = "Door1CHK"
        Door1CHK.Size = New Size(61, 19)
        Door1CHK.TabIndex = 10
        Door1CHK.Text = "Door 1"
        Door1CHK.UseVisualStyleBackColor = True
        ' 
        ' Door2CHK
        ' 
        Door2CHK.AutoSize = True
        Door2CHK.Location = New Point(94, 125)
        Door2CHK.Name = "Door2CHK"
        Door2CHK.Size = New Size(61, 19)
        Door2CHK.TabIndex = 11
        Door2CHK.Text = "Door 2"
        Door2CHK.UseVisualStyleBackColor = True
        ' 
        ' Door3CHK
        ' 
        Door3CHK.AutoSize = True
        Door3CHK.Location = New Point(161, 100)
        Door3CHK.Name = "Door3CHK"
        Door3CHK.Size = New Size(61, 19)
        Door3CHK.TabIndex = 12
        Door3CHK.Text = "Door 3"
        Door3CHK.UseVisualStyleBackColor = True
        ' 
        ' Door4CHK
        ' 
        Door4CHK.AutoSize = True
        Door4CHK.Location = New Point(161, 125)
        Door4CHK.Name = "Door4CHK"
        Door4CHK.Size = New Size(61, 19)
        Door4CHK.TabIndex = 13
        Door4CHK.Text = "Door 4"
        Door4CHK.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(59, 160)
        Label3.Name = "Label3"
        Label3.Size = New Size(29, 15)
        Label3.TabIndex = 14
        Label3.Text = "PIN:"
        ' 
        ' PinTXT
        ' 
        PinTXT.Location = New Point(94, 157)
        PinTXT.Name = "PinTXT"
        PinTXT.Size = New Size(100, 23)
        PinTXT.TabIndex = 15
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(CancelNewCardBTN)
        GroupBox1.Controls.Add(SaveNewCardBTN)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(PinTXT)
        GroupBox1.Controls.Add(CardTXT)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(Door4CHK)
        GroupBox1.Controls.Add(DateBOX)
        GroupBox1.Controls.Add(Door3CHK)
        GroupBox1.Controls.Add(Door1CHK)
        GroupBox1.Controls.Add(Door2CHK)
        GroupBox1.Location = New Point(806, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(257, 244)
        GroupBox1.TabIndex = 16
        GroupBox1.TabStop = False
        GroupBox1.Text = "Add New Card"
        ' 
        ' CancelNewCardBTN
        ' 
        CancelNewCardBTN.Location = New Point(145, 215)
        CancelNewCardBTN.Name = "CancelNewCardBTN"
        CancelNewCardBTN.Size = New Size(106, 23)
        CancelNewCardBTN.TabIndex = 17
        CancelNewCardBTN.Text = "Cancel New Card"
        CancelNewCardBTN.UseVisualStyleBackColor = True
        ' 
        ' SaveNewCardBTN
        ' 
        SaveNewCardBTN.Enabled = False
        SaveNewCardBTN.Location = New Point(6, 215)
        SaveNewCardBTN.Name = "SaveNewCardBTN"
        SaveNewCardBTN.Size = New Size(106, 23)
        SaveNewCardBTN.TabIndex = 16
        SaveNewCardBTN.Text = "Save New Card"
        SaveNewCardBTN.UseVisualStyleBackColor = True
        ' 
        ' ControllersBox
        ' 
        ControllersBox.FormattingEnabled = True
        ControllersBox.Location = New Point(12, 12)
        ControllersBox.Name = "ControllersBox"
        ControllersBox.Size = New Size(89, 424)
        ControllersBox.TabIndex = 17
        ' 
        ' CardDGV
        ' 
        CardDGV.AllowUserToAddRows = False
        CardDGV.AllowUserToDeleteRows = False
        CardDGV.AllowUserToResizeRows = False
        CardDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        CardDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        CardDGV.Location = New Point(116, 12)
        CardDGV.Name = "CardDGV"
        CardDGV.ReadOnly = True
        CardDGV.RowHeadersVisible = False
        CardDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        CardDGV.Size = New Size(684, 608)
        CardDGV.TabIndex = 18
        ' 
        ' AllRadioBTN
        ' 
        AllRadioBTN.AutoSize = True
        AllRadioBTN.Location = New Point(12, 468)
        AllRadioBTN.Name = "AllRadioBTN"
        AllRadioBTN.Size = New Size(70, 19)
        AllRadioBTN.TabIndex = 19
        AllRadioBTN.TabStop = True
        AllRadioBTN.Text = "All cards"
        AllRadioBTN.UseVisualStyleBackColor = True
        ' 
        ' ByControllerRadioBTN
        ' 
        ByControllerRadioBTN.AutoSize = True
        ByControllerRadioBTN.Location = New Point(12, 493)
        ByControllerRadioBTN.Name = "ByControllerRadioBTN"
        ByControllerRadioBTN.Size = New Size(92, 19)
        ByControllerRadioBTN.TabIndex = 20
        ByControllerRadioBTN.TabStop = True
        ByControllerRadioBTN.Text = "By controller"
        ByControllerRadioBTN.UseVisualStyleBackColor = True
        ' 
        ' WaitLBL
        ' 
        WaitLBL.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
        WaitLBL.Location = New Point(250, 241)
        WaitLBL.Name = "WaitLBL"
        WaitLBL.Size = New Size(384, 129)
        WaitLBL.TabIndex = 21
        WaitLBL.Text = "Loading cards, please wait..."
        WaitLBL.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(AllCRadioBTN)
        GroupBox2.Controls.Add(SelectedCRadioBTN)
        GroupBox2.Controls.Add(CancelDelCardBTN)
        GroupBox2.Controls.Add(DelCardBTN)
        GroupBox2.Controls.Add(Label4)
        GroupBox2.Controls.Add(DelCardTXT)
        GroupBox2.Location = New Point(806, 262)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(267, 121)
        GroupBox2.TabIndex = 18
        GroupBox2.TabStop = False
        GroupBox2.Text = "Delete Card"
        ' 
        ' AllCRadioBTN
        ' 
        AllCRadioBTN.AutoSize = True
        AllCRadioBTN.Location = New Point(145, 58)
        AllCRadioBTN.Name = "AllCRadioBTN"
        AllCRadioBTN.Size = New Size(100, 19)
        AllCRadioBTN.TabIndex = 24
        AllCRadioBTN.TabStop = True
        AllCRadioBTN.Text = "All Controllers"
        AllCRadioBTN.UseVisualStyleBackColor = True
        ' 
        ' SelectedCRadioBTN
        ' 
        SelectedCRadioBTN.AutoSize = True
        SelectedCRadioBTN.Location = New Point(6, 58)
        SelectedCRadioBTN.Name = "SelectedCRadioBTN"
        SelectedCRadioBTN.Size = New Size(125, 19)
        SelectedCRadioBTN.TabIndex = 23
        SelectedCRadioBTN.TabStop = True
        SelectedCRadioBTN.Text = "Selected Controller"
        SelectedCRadioBTN.UseVisualStyleBackColor = True
        ' 
        ' CancelDelCardBTN
        ' 
        CancelDelCardBTN.Location = New Point(145, 92)
        CancelDelCardBTN.Name = "CancelDelCardBTN"
        CancelDelCardBTN.Size = New Size(106, 23)
        CancelDelCardBTN.TabIndex = 22
        CancelDelCardBTN.Text = "Cancel"
        CancelDelCardBTN.UseVisualStyleBackColor = True
        ' 
        ' DelCardBTN
        ' 
        DelCardBTN.Location = New Point(6, 92)
        DelCardBTN.Name = "DelCardBTN"
        DelCardBTN.Size = New Size(106, 23)
        DelCardBTN.TabIndex = 19
        DelCardBTN.Text = "Delete Card"
        DelCardBTN.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(8, 25)
        Label4.Name = "Label4"
        Label4.Size = New Size(82, 15)
        Label4.TabIndex = 18
        Label4.Text = "Card Number:"
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(DelPINBTN)
        GroupBox3.Controls.Add(Label9)
        GroupBox3.Controls.Add(Label8)
        GroupBox3.Controls.Add(Label7)
        GroupBox3.Controls.Add(PIN3TXT)
        GroupBox3.Controls.Add(PIN4TXT)
        GroupBox3.Controls.Add(PIN2TXT)
        GroupBox3.Controls.Add(PINBOX)
        GroupBox3.Controls.Add(Label6)
        GroupBox3.Controls.Add(PIN1TXT)
        GroupBox3.Controls.Add(Label5)
        GroupBox3.Controls.Add(CancelPINBTN)
        GroupBox3.Controls.Add(SavePINBTN)
        GroupBox3.Location = New Point(806, 389)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(257, 152)
        GroupBox3.TabIndex = 18
        GroupBox3.TabStop = False
        GroupBox3.Text = "Add New PIN"
        ' 
        ' DelPINBTN
        ' 
        DelPINBTN.Location = New Point(145, 92)
        DelPINBTN.Name = "DelPINBTN"
        DelPINBTN.Size = New Size(106, 23)
        DelPINBTN.TabIndex = 22
        DelPINBTN.Text = "Delete PINs"
        DelPINBTN.UseVisualStyleBackColor = True
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(122, 64)
        Label9.Name = "Label9"
        Label9.Size = New Size(38, 15)
        Label9.TabIndex = 33
        Label9.Text = "PIN 4:"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(8, 64)
        Label8.Name = "Label8"
        Label8.Size = New Size(38, 15)
        Label8.TabIndex = 32
        Label8.Text = "PIN 3:"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(122, 28)
        Label7.Name = "Label7"
        Label7.Size = New Size(38, 15)
        Label7.TabIndex = 31
        Label7.Text = "PIN 2:"
        ' 
        ' PIN3TXT
        ' 
        PIN3TXT.Location = New Point(52, 61)
        PIN3TXT.Name = "PIN3TXT"
        PIN3TXT.Size = New Size(58, 23)
        PIN3TXT.TabIndex = 30
        ' 
        ' PIN4TXT
        ' 
        PIN4TXT.Location = New Point(166, 61)
        PIN4TXT.Name = "PIN4TXT"
        PIN4TXT.Size = New Size(58, 23)
        PIN4TXT.TabIndex = 29
        ' 
        ' PIN2TXT
        ' 
        PIN2TXT.Location = New Point(166, 25)
        PIN2TXT.Name = "PIN2TXT"
        PIN2TXT.Size = New Size(58, 23)
        PIN2TXT.TabIndex = 28
        ' 
        ' PINBOX
        ' 
        PINBOX.DropDownStyle = ComboBoxStyle.DropDownList
        PINBOX.FormattingEnabled = True
        PINBOX.Location = New Point(47, 92)
        PINBOX.Name = "PINBOX"
        PINBOX.Size = New Size(34, 23)
        PINBOX.TabIndex = 27
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(10, 95)
        Label6.Name = "Label6"
        Label6.Size = New Size(36, 15)
        Label6.TabIndex = 26
        Label6.Text = "Door:"
        ' 
        ' PIN1TXT
        ' 
        PIN1TXT.Location = New Point(52, 25)
        PIN1TXT.Name = "PIN1TXT"
        PIN1TXT.Size = New Size(58, 23)
        PIN1TXT.TabIndex = 25
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(8, 28)
        Label5.Name = "Label5"
        Label5.Size = New Size(38, 15)
        Label5.TabIndex = 24
        Label5.Text = "PIN 1:"
        ' 
        ' CancelPINBTN
        ' 
        CancelPINBTN.Location = New Point(145, 121)
        CancelPINBTN.Name = "CancelPINBTN"
        CancelPINBTN.Size = New Size(106, 23)
        CancelPINBTN.TabIndex = 23
        CancelPINBTN.Text = "Cancel"
        CancelPINBTN.UseVisualStyleBackColor = True
        ' 
        ' SavePINBTN
        ' 
        SavePINBTN.Location = New Point(6, 121)
        SavePINBTN.Name = "SavePINBTN"
        SavePINBTN.Size = New Size(104, 23)
        SavePINBTN.TabIndex = 22
        SavePINBTN.Text = "Save New PIN"
        SavePINBTN.UseVisualStyleBackColor = True
        ' 
        ' CardFRM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1075, 632)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(WaitLBL)
        Controls.Add(ByControllerRadioBTN)
        Controls.Add(AllRadioBTN)
        Controls.Add(CardDGV)
        Controls.Add(ControllersBox)
        Controls.Add(GroupBox1)
        Controls.Add(CloseBTN)
        Name = "CardFRM"
        StartPosition = FormStartPosition.CenterScreen
        Text = "CardFRM"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(CardDGV, ComponentModel.ISupportInitialize).EndInit()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents CloseBTN As Button
    Friend WithEvents DelCardTXT As TextBox
    Friend WithEvents CardTXT As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents DateBOX As DateTimePicker
    Friend WithEvents Door1CHK As CheckBox
    Friend WithEvents Door2CHK As CheckBox
    Friend WithEvents Door3CHK As CheckBox
    Friend WithEvents Door4CHK As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents PinTXT As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CancelNewCardBTN As Button
    Friend WithEvents SaveNewCardBTN As Button
    Friend WithEvents ControllersBox As ListBox
    Friend WithEvents CardDGV As DataGridView
    Friend WithEvents AllRadioBTN As RadioButton
    Friend WithEvents ByControllerRadioBTN As RadioButton
    Friend WithEvents WaitLBL As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents CancelDelCardBTN As Button
    Friend WithEvents DelCardBTN As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents SelectedCRadioBTN As RadioButton
    Friend WithEvents AllCRadioBTN As RadioButton
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents CancelPINBTN As Button
    Friend WithEvents SavePINBTN As Button
    Friend WithEvents PINBOX As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents PIN1TXT As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents PIN3TXT As TextBox
    Friend WithEvents PIN4TXT As TextBox
    Friend WithEvents PIN2TXT As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents DelPINBTN As Button
End Class
