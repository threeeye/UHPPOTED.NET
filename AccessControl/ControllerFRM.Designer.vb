<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ControllerFRM
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
        DataGridView1 = New DataGridView()
        ControllersBox = New ListBox()
        Button2 = New Button()
        SaveBTN = New Button()
        EditBTN = New Button()
        CloseBTN = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label7 = New Label()
        IPAddressTXT = New TextBox()
        NetmaskTXT = New TextBox()
        GatewayTXT = New TextBox()
        ListenerTXT = New TextBox()
        MACTXT = New TextBox()
        VersionTXT = New TextBox()
        DateTXT = New TextBox()
        GetControllersBTN = New Button()
        GetDoorsBTN = New Button()
        Panel1 = New Panel()
        ShowSysRDO = New RadioButton()
        ShowNewRDO = New RadioButton()
        ShowAllRDO = New RadioButton()
        DoorGroup1 = New GroupBox()
        Delay1 = New TextBox()
        Label8 = New Label()
        Controlled1 = New RadioButton()
        NormallyClosed1 = New RadioButton()
        NormallyOpen1 = New RadioButton()
        DoorGroup2 = New GroupBox()
        Delay2 = New TextBox()
        Label9 = New Label()
        Controlled2 = New RadioButton()
        NormallyClosed2 = New RadioButton()
        NormallyOpen2 = New RadioButton()
        DoorGroup3 = New GroupBox()
        Delay3 = New TextBox()
        Label10 = New Label()
        Controlled3 = New RadioButton()
        NormallyClosed3 = New RadioButton()
        NormallyOpen3 = New RadioButton()
        DoorGroup4 = New GroupBox()
        Delay4 = New TextBox()
        Label11 = New Label()
        Controlled4 = New RadioButton()
        NormallyClosed4 = New RadioButton()
        NormallyOpen4 = New RadioButton()
        AddControllerBTN = New Button()
        SetDefaultBTN = New Button()
        Label12 = New Label()
        IndexLBL = New Label()
        RecordSpecialEventsBTN = New Button()
        GetCardsBTN = New Button()
        SetDateBTN = New Button()
        Label13 = New Label()
        Door1NameTXT = New TextBox()
        Door2NameTXT = New TextBox()
        Label14 = New Label()
        Door3NameTXT = New TextBox()
        Label15 = New Label()
        Door4NameTXT = New TextBox()
        Label16 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        DoorGroup1.SuspendLayout()
        DoorGroup2.SuspendLayout()
        DoorGroup3.SuspendLayout()
        DoorGroup4.SuspendLayout()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToOrderColumns = True
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(478, 323)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersVisible = False
        DataGridView1.Size = New Size(310, 141)
        DataGridView1.TabIndex = 0
        ' 
        ' ControllersBox
        ' 
        ControllersBox.FormattingEnabled = True
        ControllersBox.Location = New Point(12, 12)
        ControllersBox.Name = "ControllersBox"
        ControllersBox.Size = New Size(89, 424)
        ControllersBox.TabIndex = 1
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(305, 466)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 3
        Button2.Text = "Controller"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' SaveBTN
        ' 
        SaveBTN.Location = New Point(233, 269)
        SaveBTN.Name = "SaveBTN"
        SaveBTN.Size = New Size(75, 23)
        SaveBTN.TabIndex = 4
        SaveBTN.Text = "Save"
        SaveBTN.UseVisualStyleBackColor = True
        ' 
        ' EditBTN
        ' 
        EditBTN.Location = New Point(119, 269)
        EditBTN.Name = "EditBTN"
        EditBTN.Size = New Size(75, 23)
        EditBTN.TabIndex = 5
        EditBTN.Text = "Edit"
        EditBTN.UseVisualStyleBackColor = True
        ' 
        ' CloseBTN
        ' 
        CloseBTN.Location = New Point(713, 488)
        CloseBTN.Name = "CloseBTN"
        CloseBTN.Size = New Size(75, 23)
        CloseBTN.TabIndex = 6
        CloseBTN.Text = "Close"
        CloseBTN.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(119, 25)
        Label1.Name = "Label1"
        Label1.Size = New Size(65, 15)
        Label1.TabIndex = 7
        Label1.Text = "IP Address:"
        Label1.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(119, 54)
        Label2.Name = "Label2"
        Label2.Size = New Size(57, 15)
        Label2.TabIndex = 8
        Label2.Text = "Netmask:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(119, 83)
        Label3.Name = "Label3"
        Label3.Size = New Size(55, 15)
        Label3.TabIndex = 9
        Label3.Text = "Gateway:"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(119, 112)
        Label4.Name = "Label4"
        Label4.Size = New Size(37, 15)
        Label4.TabIndex = 10
        Label4.Text = "MAC:"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(119, 141)
        Label5.Name = "Label5"
        Label5.Size = New Size(51, 15)
        Label5.TabIndex = 11
        Label5.Text = "Listener:"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(119, 170)
        Label6.Name = "Label6"
        Label6.Size = New Size(48, 15)
        Label6.TabIndex = 12
        Label6.Text = "Version:"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(119, 199)
        Label7.Name = "Label7"
        Label7.Size = New Size(34, 15)
        Label7.TabIndex = 13
        Label7.Text = "Date:"
        ' 
        ' IPAddressTXT
        ' 
        IPAddressTXT.Location = New Point(208, 22)
        IPAddressTXT.Name = "IPAddressTXT"
        IPAddressTXT.Size = New Size(111, 23)
        IPAddressTXT.TabIndex = 15
        ' 
        ' NetmaskTXT
        ' 
        NetmaskTXT.Location = New Point(208, 51)
        NetmaskTXT.Name = "NetmaskTXT"
        NetmaskTXT.Size = New Size(111, 23)
        NetmaskTXT.TabIndex = 16
        ' 
        ' GatewayTXT
        ' 
        GatewayTXT.Location = New Point(208, 80)
        GatewayTXT.Name = "GatewayTXT"
        GatewayTXT.Size = New Size(111, 23)
        GatewayTXT.TabIndex = 17
        ' 
        ' ListenerTXT
        ' 
        ListenerTXT.Location = New Point(208, 138)
        ListenerTXT.Name = "ListenerTXT"
        ListenerTXT.Size = New Size(111, 23)
        ListenerTXT.TabIndex = 18
        ' 
        ' MACTXT
        ' 
        MACTXT.Location = New Point(208, 109)
        MACTXT.Name = "MACTXT"
        MACTXT.ReadOnly = True
        MACTXT.Size = New Size(111, 23)
        MACTXT.TabIndex = 19
        ' 
        ' VersionTXT
        ' 
        VersionTXT.Location = New Point(208, 167)
        VersionTXT.Name = "VersionTXT"
        VersionTXT.ReadOnly = True
        VersionTXT.Size = New Size(111, 23)
        VersionTXT.TabIndex = 20
        ' 
        ' DateTXT
        ' 
        DateTXT.Location = New Point(208, 196)
        DateTXT.Name = "DateTXT"
        DateTXT.ReadOnly = True
        DateTXT.Size = New Size(111, 23)
        DateTXT.TabIndex = 21
        ' 
        ' GetControllersBTN
        ' 
        GetControllersBTN.Location = New Point(107, 413)
        GetControllersBTN.Name = "GetControllersBTN"
        GetControllersBTN.Size = New Size(95, 23)
        GetControllersBTN.TabIndex = 22
        GetControllersBTN.Text = "Get Controllers"
        GetControllersBTN.UseVisualStyleBackColor = True
        ' 
        ' GetDoorsBTN
        ' 
        GetDoorsBTN.Location = New Point(421, 470)
        GetDoorsBTN.Name = "GetDoorsBTN"
        GetDoorsBTN.Size = New Size(75, 23)
        GetDoorsBTN.TabIndex = 23
        GetDoorsBTN.Text = "Get Doors"
        GetDoorsBTN.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(ShowSysRDO)
        Panel1.Controls.Add(ShowNewRDO)
        Panel1.Controls.Add(ShowAllRDO)
        Panel1.Location = New Point(12, 442)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(144, 78)
        Panel1.TabIndex = 24
        ' 
        ' ShowSysRDO
        ' 
        ShowSysRDO.AutoSize = True
        ShowSysRDO.Location = New Point(3, 53)
        ShowSysRDO.Name = "ShowSysRDO"
        ShowSysRDO.Size = New Size(136, 19)
        ShowSysRDO.TabIndex = 2
        ShowSysRDO.TabStop = True
        ShowSysRDO.Text = "Show In System Only"
        ShowSysRDO.UseVisualStyleBackColor = True
        ' 
        ' ShowNewRDO
        ' 
        ShowNewRDO.AutoSize = True
        ShowNewRDO.Location = New Point(3, 28)
        ShowNewRDO.Name = "ShowNewRDO"
        ShowNewRDO.Size = New Size(109, 19)
        ShowNewRDO.TabIndex = 1
        ShowNewRDO.TabStop = True
        ShowNewRDO.Text = "Show New Only"
        ShowNewRDO.UseVisualStyleBackColor = True
        ' 
        ' ShowAllRDO
        ' 
        ShowAllRDO.AutoSize = True
        ShowAllRDO.Location = New Point(3, 3)
        ShowAllRDO.Name = "ShowAllRDO"
        ShowAllRDO.Size = New Size(71, 19)
        ShowAllRDO.TabIndex = 0
        ShowAllRDO.TabStop = True
        ShowAllRDO.Text = "Show All"
        ShowAllRDO.UseVisualStyleBackColor = True
        ' 
        ' DoorGroup1
        ' 
        DoorGroup1.Controls.Add(Door1NameTXT)
        DoorGroup1.Controls.Add(Label13)
        DoorGroup1.Controls.Add(Delay1)
        DoorGroup1.Controls.Add(Label8)
        DoorGroup1.Controls.Add(Controlled1)
        DoorGroup1.Controls.Add(NormallyClosed1)
        DoorGroup1.Controls.Add(NormallyOpen1)
        DoorGroup1.Location = New Point(334, 19)
        DoorGroup1.Name = "DoorGroup1"
        DoorGroup1.Size = New Size(653, 47)
        DoorGroup1.TabIndex = 3
        DoorGroup1.TabStop = False
        DoorGroup1.Text = "Door 1"
        ' 
        ' Delay1
        ' 
        Delay1.Location = New Point(407, 17)
        Delay1.Name = "Delay1"
        Delay1.Size = New Size(39, 23)
        Delay1.TabIndex = 25
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(362, 20)
        Label8.Name = "Label8"
        Label8.Size = New Size(39, 15)
        Label8.TabIndex = 25
        Label8.Text = "Delay:"
        ' 
        ' Controlled1
        ' 
        Controlled1.AutoSize = True
        Controlled1.Location = New Point(260, 18)
        Controlled1.Name = "Controlled1"
        Controlled1.Size = New Size(81, 19)
        Controlled1.TabIndex = 27
        Controlled1.TabStop = True
        Controlled1.Text = "Controlled"
        Controlled1.UseVisualStyleBackColor = True
        ' 
        ' NormallyClosed1
        ' 
        NormallyClosed1.AutoSize = True
        NormallyClosed1.Location = New Point(141, 18)
        NormallyClosed1.Name = "NormallyClosed1"
        NormallyClosed1.Size = New Size(113, 19)
        NormallyClosed1.TabIndex = 26
        NormallyClosed1.TabStop = True
        NormallyClosed1.Text = "Normally Closed"
        NormallyClosed1.UseVisualStyleBackColor = True
        ' 
        ' NormallyOpen1
        ' 
        NormallyOpen1.AutoSize = True
        NormallyOpen1.Location = New Point(29, 18)
        NormallyOpen1.Name = "NormallyOpen1"
        NormallyOpen1.Size = New Size(106, 19)
        NormallyOpen1.TabIndex = 25
        NormallyOpen1.TabStop = True
        NormallyOpen1.Text = "Normally Open"
        NormallyOpen1.UseVisualStyleBackColor = True
        ' 
        ' DoorGroup2
        ' 
        DoorGroup2.Controls.Add(Door2NameTXT)
        DoorGroup2.Controls.Add(Label14)
        DoorGroup2.Controls.Add(Delay2)
        DoorGroup2.Controls.Add(Label9)
        DoorGroup2.Controls.Add(Controlled2)
        DoorGroup2.Controls.Add(NormallyClosed2)
        DoorGroup2.Controls.Add(NormallyOpen2)
        DoorGroup2.Location = New Point(334, 72)
        DoorGroup2.Name = "DoorGroup2"
        DoorGroup2.Size = New Size(653, 47)
        DoorGroup2.TabIndex = 28
        DoorGroup2.TabStop = False
        DoorGroup2.Text = "Door 2"
        ' 
        ' Delay2
        ' 
        Delay2.Location = New Point(407, 17)
        Delay2.Name = "Delay2"
        Delay2.Size = New Size(39, 23)
        Delay2.TabIndex = 25
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(362, 20)
        Label9.Name = "Label9"
        Label9.Size = New Size(39, 15)
        Label9.TabIndex = 25
        Label9.Text = "Delay:"
        ' 
        ' Controlled2
        ' 
        Controlled2.AutoSize = True
        Controlled2.Location = New Point(260, 18)
        Controlled2.Name = "Controlled2"
        Controlled2.Size = New Size(81, 19)
        Controlled2.TabIndex = 27
        Controlled2.TabStop = True
        Controlled2.Text = "Controlled"
        Controlled2.UseVisualStyleBackColor = True
        ' 
        ' NormallyClosed2
        ' 
        NormallyClosed2.AutoSize = True
        NormallyClosed2.Location = New Point(141, 18)
        NormallyClosed2.Name = "NormallyClosed2"
        NormallyClosed2.Size = New Size(113, 19)
        NormallyClosed2.TabIndex = 26
        NormallyClosed2.TabStop = True
        NormallyClosed2.Text = "Normally Closed"
        NormallyClosed2.UseVisualStyleBackColor = True
        ' 
        ' NormallyOpen2
        ' 
        NormallyOpen2.AutoSize = True
        NormallyOpen2.Location = New Point(29, 18)
        NormallyOpen2.Name = "NormallyOpen2"
        NormallyOpen2.Size = New Size(106, 19)
        NormallyOpen2.TabIndex = 25
        NormallyOpen2.TabStop = True
        NormallyOpen2.Text = "Normally Open"
        NormallyOpen2.UseVisualStyleBackColor = True
        ' 
        ' DoorGroup3
        ' 
        DoorGroup3.Controls.Add(Door3NameTXT)
        DoorGroup3.Controls.Add(Delay3)
        DoorGroup3.Controls.Add(Label15)
        DoorGroup3.Controls.Add(Label10)
        DoorGroup3.Controls.Add(Controlled3)
        DoorGroup3.Controls.Add(NormallyClosed3)
        DoorGroup3.Controls.Add(NormallyOpen3)
        DoorGroup3.Location = New Point(334, 125)
        DoorGroup3.Name = "DoorGroup3"
        DoorGroup3.Size = New Size(653, 47)
        DoorGroup3.TabIndex = 28
        DoorGroup3.TabStop = False
        DoorGroup3.Text = "Door 3"
        ' 
        ' Delay3
        ' 
        Delay3.Location = New Point(407, 17)
        Delay3.Name = "Delay3"
        Delay3.Size = New Size(39, 23)
        Delay3.TabIndex = 25
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(362, 20)
        Label10.Name = "Label10"
        Label10.Size = New Size(39, 15)
        Label10.TabIndex = 25
        Label10.Text = "Delay:"
        ' 
        ' Controlled3
        ' 
        Controlled3.AutoSize = True
        Controlled3.Location = New Point(260, 18)
        Controlled3.Name = "Controlled3"
        Controlled3.Size = New Size(81, 19)
        Controlled3.TabIndex = 27
        Controlled3.TabStop = True
        Controlled3.Text = "Controlled"
        Controlled3.UseVisualStyleBackColor = True
        ' 
        ' NormallyClosed3
        ' 
        NormallyClosed3.AutoSize = True
        NormallyClosed3.Location = New Point(141, 18)
        NormallyClosed3.Name = "NormallyClosed3"
        NormallyClosed3.Size = New Size(113, 19)
        NormallyClosed3.TabIndex = 26
        NormallyClosed3.TabStop = True
        NormallyClosed3.Text = "Normally Closed"
        NormallyClosed3.UseVisualStyleBackColor = True
        ' 
        ' NormallyOpen3
        ' 
        NormallyOpen3.AutoSize = True
        NormallyOpen3.Location = New Point(29, 18)
        NormallyOpen3.Name = "NormallyOpen3"
        NormallyOpen3.Size = New Size(106, 19)
        NormallyOpen3.TabIndex = 25
        NormallyOpen3.TabStop = True
        NormallyOpen3.Text = "Normally Open"
        NormallyOpen3.UseVisualStyleBackColor = True
        ' 
        ' DoorGroup4
        ' 
        DoorGroup4.Controls.Add(Door4NameTXT)
        DoorGroup4.Controls.Add(Delay4)
        DoorGroup4.Controls.Add(Label16)
        DoorGroup4.Controls.Add(Label11)
        DoorGroup4.Controls.Add(Controlled4)
        DoorGroup4.Controls.Add(NormallyClosed4)
        DoorGroup4.Controls.Add(NormallyOpen4)
        DoorGroup4.Location = New Point(334, 178)
        DoorGroup4.Name = "DoorGroup4"
        DoorGroup4.Size = New Size(653, 47)
        DoorGroup4.TabIndex = 28
        DoorGroup4.TabStop = False
        DoorGroup4.Text = "Door 4"
        ' 
        ' Delay4
        ' 
        Delay4.Location = New Point(407, 17)
        Delay4.Name = "Delay4"
        Delay4.Size = New Size(39, 23)
        Delay4.TabIndex = 25
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(362, 20)
        Label11.Name = "Label11"
        Label11.Size = New Size(39, 15)
        Label11.TabIndex = 25
        Label11.Text = "Delay:"
        ' 
        ' Controlled4
        ' 
        Controlled4.AutoSize = True
        Controlled4.Location = New Point(260, 18)
        Controlled4.Name = "Controlled4"
        Controlled4.Size = New Size(81, 19)
        Controlled4.TabIndex = 27
        Controlled4.TabStop = True
        Controlled4.Text = "Controlled"
        Controlled4.UseVisualStyleBackColor = True
        ' 
        ' NormallyClosed4
        ' 
        NormallyClosed4.AutoSize = True
        NormallyClosed4.Location = New Point(141, 18)
        NormallyClosed4.Name = "NormallyClosed4"
        NormallyClosed4.Size = New Size(113, 19)
        NormallyClosed4.TabIndex = 26
        NormallyClosed4.TabStop = True
        NormallyClosed4.Text = "Normally Closed"
        NormallyClosed4.UseVisualStyleBackColor = True
        ' 
        ' NormallyOpen4
        ' 
        NormallyOpen4.AutoSize = True
        NormallyOpen4.Location = New Point(29, 18)
        NormallyOpen4.Name = "NormallyOpen4"
        NormallyOpen4.Size = New Size(106, 19)
        NormallyOpen4.TabIndex = 25
        NormallyOpen4.TabStop = True
        NormallyOpen4.Text = "Normally Open"
        NormallyOpen4.UseVisualStyleBackColor = True
        ' 
        ' AddControllerBTN
        ' 
        AddControllerBTN.Location = New Point(334, 269)
        AddControllerBTN.Name = "AddControllerBTN"
        AddControllerBTN.Size = New Size(96, 23)
        AddControllerBTN.TabIndex = 29
        AddControllerBTN.Text = "Add Controller"
        AddControllerBTN.UseVisualStyleBackColor = True
        ' 
        ' SetDefaultBTN
        ' 
        SetDefaultBTN.Location = New Point(446, 269)
        SetDefaultBTN.Name = "SetDefaultBTN"
        SetDefaultBTN.Size = New Size(75, 23)
        SetDefaultBTN.TabIndex = 30
        SetDefaultBTN.Text = "Set Default"
        SetDefaultBTN.UseVisualStyleBackColor = True
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(119, 231)
        Label12.Name = "Label12"
        Label12.Size = New Size(39, 15)
        Label12.TabIndex = 31
        Label12.Text = "Index:"
        ' 
        ' IndexLBL
        ' 
        IndexLBL.AutoSize = True
        IndexLBL.Location = New Point(208, 231)
        IndexLBL.Name = "IndexLBL"
        IndexLBL.Size = New Size(47, 15)
        IndexLBL.TabIndex = 32
        IndexLBL.Text = "Label13"
        ' 
        ' RecordSpecialEventsBTN
        ' 
        RecordSpecialEventsBTN.Location = New Point(527, 269)
        RecordSpecialEventsBTN.Name = "RecordSpecialEventsBTN"
        RecordSpecialEventsBTN.Size = New Size(132, 23)
        RecordSpecialEventsBTN.TabIndex = 33
        RecordSpecialEventsBTN.Text = "RecordSpecialEvents"
        RecordSpecialEventsBTN.UseVisualStyleBackColor = True
        ' 
        ' GetCardsBTN
        ' 
        GetCardsBTN.Location = New Point(678, 269)
        GetCardsBTN.Name = "GetCardsBTN"
        GetCardsBTN.Size = New Size(75, 23)
        GetCardsBTN.TabIndex = 34
        GetCardsBTN.Text = "Get Cards"
        GetCardsBTN.UseVisualStyleBackColor = True
        ' 
        ' SetDateBTN
        ' 
        SetDateBTN.Location = New Point(345, 298)
        SetDateBTN.Name = "SetDateBTN"
        SetDateBTN.Size = New Size(75, 23)
        SetDateBTN.TabIndex = 35
        SetDateBTN.Text = "Set Date"
        SetDateBTN.UseVisualStyleBackColor = True
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(474, 20)
        Label13.Name = "Label13"
        Label13.Size = New Size(42, 15)
        Label13.TabIndex = 28
        Label13.Text = "Name:"
        ' 
        ' Door1NameTXT
        ' 
        Door1NameTXT.Location = New Point(522, 17)
        Door1NameTXT.Name = "Door1NameTXT"
        Door1NameTXT.Size = New Size(125, 23)
        Door1NameTXT.TabIndex = 29
        ' 
        ' Door2NameTXT
        ' 
        Door2NameTXT.Location = New Point(522, 19)
        Door2NameTXT.Name = "Door2NameTXT"
        Door2NameTXT.Size = New Size(125, 23)
        Door2NameTXT.TabIndex = 31
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(474, 22)
        Label14.Name = "Label14"
        Label14.Size = New Size(42, 15)
        Label14.TabIndex = 30
        Label14.Text = "Name:"
        ' 
        ' Door3NameTXT
        ' 
        Door3NameTXT.Location = New Point(522, 17)
        Door3NameTXT.Name = "Door3NameTXT"
        Door3NameTXT.Size = New Size(125, 23)
        Door3NameTXT.TabIndex = 37
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(474, 20)
        Label15.Name = "Label15"
        Label15.Size = New Size(42, 15)
        Label15.TabIndex = 36
        Label15.Text = "Name:"
        ' 
        ' Door4NameTXT
        ' 
        Door4NameTXT.Location = New Point(522, 18)
        Door4NameTXT.Name = "Door4NameTXT"
        Door4NameTXT.Size = New Size(125, 23)
        Door4NameTXT.TabIndex = 39
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(474, 21)
        Label16.Name = "Label16"
        Label16.Size = New Size(42, 15)
        Label16.TabIndex = 38
        Label16.Text = "Name:"
        ' 
        ' ControllerFRM
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(999, 523)
        Controls.Add(SetDateBTN)
        Controls.Add(GetCardsBTN)
        Controls.Add(RecordSpecialEventsBTN)
        Controls.Add(IndexLBL)
        Controls.Add(Label12)
        Controls.Add(SetDefaultBTN)
        Controls.Add(AddControllerBTN)
        Controls.Add(DoorGroup4)
        Controls.Add(DoorGroup3)
        Controls.Add(DoorGroup2)
        Controls.Add(DoorGroup1)
        Controls.Add(Panel1)
        Controls.Add(GetDoorsBTN)
        Controls.Add(GetControllersBTN)
        Controls.Add(DateTXT)
        Controls.Add(VersionTXT)
        Controls.Add(MACTXT)
        Controls.Add(ListenerTXT)
        Controls.Add(GatewayTXT)
        Controls.Add(NetmaskTXT)
        Controls.Add(IPAddressTXT)
        Controls.Add(Label7)
        Controls.Add(Label6)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(CloseBTN)
        Controls.Add(EditBTN)
        Controls.Add(SaveBTN)
        Controls.Add(Button2)
        Controls.Add(ControllersBox)
        Controls.Add(DataGridView1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "ControllerFRM"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Controller Configuration"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        DoorGroup1.ResumeLayout(False)
        DoorGroup1.PerformLayout()
        DoorGroup2.ResumeLayout(False)
        DoorGroup2.PerformLayout()
        DoorGroup3.ResumeLayout(False)
        DoorGroup3.PerformLayout()
        DoorGroup4.ResumeLayout(False)
        DoorGroup4.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ControllersBox As ListBox
    Friend WithEvents Button2 As Button
    Friend WithEvents SaveBTN As Button
    Friend WithEvents EditBTN As Button
    Friend WithEvents CloseBTN As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents IPAddressTXT As TextBox
    Friend WithEvents NetmaskTXT As TextBox
    Friend WithEvents GatewayTXT As TextBox
    Friend WithEvents ListenerTXT As TextBox
    Friend WithEvents MACTXT As TextBox
    Friend WithEvents VersionTXT As TextBox
    Friend WithEvents DateTXT As TextBox
    Friend WithEvents GetControllersBTN As Button
    Friend WithEvents GetDoorsBTN As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ShowSysRDO As RadioButton
    Friend WithEvents ShowNewRDO As RadioButton
    Friend WithEvents ShowAllRDO As RadioButton
    Friend WithEvents DoorGroup1 As GroupBox
    Friend WithEvents NormallyOpen1 As RadioButton
    Friend WithEvents Controlled1 As RadioButton
    Friend WithEvents NormallyClosed1 As RadioButton
    Friend WithEvents Label8 As Label
    Friend WithEvents Delay1 As TextBox
    Friend WithEvents DoorGroup2 As GroupBox
    Friend WithEvents Delay2 As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Controlled2 As RadioButton
    Friend WithEvents NormallyClosed2 As RadioButton
    Friend WithEvents NormallyOpen2 As RadioButton
    Friend WithEvents DoorGroup3 As GroupBox
    Friend WithEvents Delay3 As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Controlled3 As RadioButton
    Friend WithEvents NormallyClosed3 As RadioButton
    Friend WithEvents NormallyOpen3 As RadioButton
    Friend WithEvents DoorGroup4 As GroupBox
    Friend WithEvents Delay4 As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Controlled4 As RadioButton
    Friend WithEvents NormallyClosed4 As RadioButton
    Friend WithEvents NormallyOpen4 As RadioButton
    Friend WithEvents AddControllerBTN As Button
    Friend WithEvents SetDefaultBTN As Button
    Friend WithEvents Label12 As Label
    Friend WithEvents IndexLBL As Label
    Friend WithEvents RecordSpecialEventsBTN As Button
    Friend WithEvents GetCardsBTN As Button
    Friend WithEvents SetDateBTN As Button
    Friend WithEvents Door1NameTXT As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Door2NameTXT As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Door3NameTXT As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Door4NameTXT As TextBox
    Friend WithEvents Label16 As Label
End Class
