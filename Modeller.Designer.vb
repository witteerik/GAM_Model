<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Modeller
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
        components = New ComponentModel.Container()
        StepTimer = New Timer(components)
        StartModelling_Button = New Button()
        Label1 = New Label()
        Reception_ComboBox = New ComboBox()
        DataGridView1 = New DataGridView()
        LoadSettingsButton = New Button()
        SaveSettingsButton = New Button()
        Label2 = New Label()
        CurrentSettings_Label = New Label()
        GetStatisticsButton = New Button()
        StopSimulationButton = New Button()
        StatisticsTextBox = New TextBox()
        Label3 = New Label()
        SplitContainer1 = New SplitContainer()
        SplitContainer2 = New SplitContainer()
        ClearButton = New Button()
        ShowReceptionFormButton = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        SuspendLayout()
        ' 
        ' StepTimer
        ' 
        StepTimer.Interval = 1000
        ' 
        ' StartModelling_Button
        ' 
        StartModelling_Button.Location = New Point(182, 35)
        StartModelling_Button.Name = "StartModelling_Button"
        StartModelling_Button.Size = New Size(100, 23)
        StartModelling_Button.TabIndex = 0
        StartModelling_Button.Text = "Run simulation"
        StartModelling_Button.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(113, 15)
        Label1.TabIndex = 1
        Label1.Text = "Välj mottagningstyp"
        ' 
        ' Reception_ComboBox
        ' 
        Reception_ComboBox.DisplayMember = """Text"""
        Reception_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        Reception_ComboBox.FormattingEnabled = True
        Reception_ComboBox.Location = New Point(131, 6)
        Reception_ComboBox.Name = "Reception_ComboBox"
        Reception_ComboBox.Size = New Size(363, 23)
        Reception_ComboBox.TabIndex = 2
        ' 
        ' DataGridView1
        ' 
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.Location = New Point(0, 0)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.Size = New Size(656, 688)
        DataGridView1.TabIndex = 3
        ' 
        ' LoadSettingsButton
        ' 
        LoadSettingsButton.Location = New Point(12, 64)
        LoadSettingsButton.Name = "LoadSettingsButton"
        LoadSettingsButton.Size = New Size(144, 23)
        LoadSettingsButton.TabIndex = 4
        LoadSettingsButton.Text = "Load model settings"
        LoadSettingsButton.UseVisualStyleBackColor = True
        ' 
        ' SaveSettingsButton
        ' 
        SaveSettingsButton.Location = New Point(162, 64)
        SaveSettingsButton.Name = "SaveSettingsButton"
        SaveSettingsButton.Size = New Size(144, 23)
        SaveSettingsButton.TabIndex = 5
        SaveSettingsButton.Text = "Save model settings"
        SaveSettingsButton.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(312, 68)
        Label2.Name = "Label2"
        Label2.Size = New Size(94, 15)
        Label2.TabIndex = 6
        Label2.Text = "Current settings:"
        ' 
        ' CurrentSettings_Label
        ' 
        CurrentSettings_Label.AutoSize = True
        CurrentSettings_Label.Location = New Point(412, 68)
        CurrentSettings_Label.Name = "CurrentSettings_Label"
        CurrentSettings_Label.Size = New Size(45, 15)
        CurrentSettings_Label.TabIndex = 7
        CurrentSettings_Label.Text = "Default"
        ' 
        ' GetStatisticsButton
        ' 
        GetStatisticsButton.Location = New Point(394, 35)
        GetStatisticsButton.Name = "GetStatisticsButton"
        GetStatisticsButton.Size = New Size(100, 23)
        GetStatisticsButton.TabIndex = 8
        GetStatisticsButton.Text = "Get statistics"
        GetStatisticsButton.UseVisualStyleBackColor = True
        ' 
        ' StopSimulationButton
        ' 
        StopSimulationButton.Location = New Point(288, 35)
        StopSimulationButton.Name = "StopSimulationButton"
        StopSimulationButton.Size = New Size(100, 23)
        StopSimulationButton.TabIndex = 9
        StopSimulationButton.Text = "Stop simulation"
        StopSimulationButton.UseVisualStyleBackColor = True
        ' 
        ' StatisticsTextBox
        ' 
        StatisticsTextBox.BackColor = SystemColors.Info
        StatisticsTextBox.Dock = DockStyle.Fill
        StatisticsTextBox.Location = New Point(0, 0)
        StatisticsTextBox.Multiline = True
        StatisticsTextBox.Name = "StatisticsTextBox"
        StatisticsTextBox.ReadOnly = True
        StatisticsTextBox.ScrollBars = ScrollBars.Both
        StatisticsTextBox.Size = New Size(534, 688)
        StatisticsTextBox.TabIndex = 10
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label3.AutoSize = True
        Label3.Location = New Point(1102, 72)
        Label3.Name = "Label3"
        Label3.Size = New Size(80, 15)
        Label3.TabIndex = 11
        Label3.Text = "Statistics view"
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(DataGridView1)
        SplitContainer1.Panel1MinSize = 70
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(StatisticsTextBox)
        SplitContainer1.Size = New Size(1194, 688)
        SplitContainer1.SplitterDistance = 656
        SplitContainer1.TabIndex = 12
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.IsSplitterFixed = True
        SplitContainer2.Location = New Point(0, 0)
        SplitContainer2.Name = "SplitContainer2"
        SplitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(ClearButton)
        SplitContainer2.Panel1.Controls.Add(ShowReceptionFormButton)
        SplitContainer2.Panel1.Controls.Add(Label1)
        SplitContainer2.Panel1.Controls.Add(Label3)
        SplitContainer2.Panel1.Controls.Add(StartModelling_Button)
        SplitContainer2.Panel1.Controls.Add(StopSimulationButton)
        SplitContainer2.Panel1.Controls.Add(Reception_ComboBox)
        SplitContainer2.Panel1.Controls.Add(GetStatisticsButton)
        SplitContainer2.Panel1.Controls.Add(LoadSettingsButton)
        SplitContainer2.Panel1.Controls.Add(CurrentSettings_Label)
        SplitContainer2.Panel1.Controls.Add(SaveSettingsButton)
        SplitContainer2.Panel1.Controls.Add(Label2)
        SplitContainer2.Panel1MinSize = 95
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(SplitContainer1)
        SplitContainer2.Size = New Size(1194, 787)
        SplitContainer2.SplitterDistance = 95
        SplitContainer2.TabIndex = 13
        ' 
        ' ClearButton
        ' 
        ClearButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ClearButton.Location = New Point(1082, 35)
        ClearButton.Name = "ClearButton"
        ClearButton.Size = New Size(100, 23)
        ClearButton.TabIndex = 13
        ClearButton.Text = "Clear"
        ClearButton.UseVisualStyleBackColor = True
        ' 
        ' ShowReceptionFormButton
        ' 
        ShowReceptionFormButton.Location = New Point(12, 35)
        ShowReceptionFormButton.Name = "ShowReceptionFormButton"
        ShowReceptionFormButton.Size = New Size(164, 23)
        ShowReceptionFormButton.TabIndex = 12
        ShowReceptionFormButton.Text = "Show audiology reception"
        ShowReceptionFormButton.UseVisualStyleBackColor = True
        ' 
        ' Modeller
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1194, 787)
        Controls.Add(SplitContainer2)
        Name = "Modeller"
        Text = "Modeller"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        SplitContainer1.Panel2.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel1.PerformLayout()
        SplitContainer2.Panel2.ResumeLayout(False)
        CType(SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents StepTimer As Timer
    Friend WithEvents StartModelling_Button As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Reception_ComboBox As ComboBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents LoadSettingsButton As Button
    Friend WithEvents SaveSettingsButton As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents CurrentSettings_Label As Label
    Friend WithEvents GetStatisticsButton As Button
    Friend WithEvents StopSimulationButton As Button
    Friend WithEvents StatisticsTextBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents ShowReceptionFormButton As Button
    Friend WithEvents ClearButton As Button
End Class
