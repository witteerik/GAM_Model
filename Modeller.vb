Imports System.ComponentModel
Imports System.IO
Imports System.Runtime
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Xml.Serialization

Public Class Modeller

    Public MyAudiologyReceptionForm As IAudiologyReceptionForm
    Public MyAudiologyReception As AudiologyReception

    Public PlannedPatientsList As New List(Of Patient)
    Public InHousePatientsList As New List(Of Patient)
    Public PatientsGoneHomeList As New List(Of Patient)
    Public AllPatientList As New List(Of Patient)

    Private Randomizer As New Random

    'Holds the modelled time into the modelling
    Private CurrentTime As TimeSpan

    Private AudiologistList As New List(Of Audiologist)
    Private AudiologyAssistantList As New List(Of AudiologyAssistant)
    Private PersonnelList As New List(Of Personnel)

    Public CurrentModelSettings As ModelSettings = Nothing

    Private Sub Modeller_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        StartModelling_Button.Enabled = False
        StopSimulationButton.Enabled = False
        GetStatisticsButton.Enabled = False

        Dim NewGAM1 As IAudiologyReceptionForm = New GAM_0_1
        Reception_ComboBox.Items.Add(NewGAM1.ToString)

        Dim NewGAM2 As IAudiologyReceptionForm = New GAM_0_2
        Reception_ComboBox.Items.Add(NewGAM2.ToString)

        'These should be selected and loaded from file
        CurrentModelSettings = New ModelSettings

        'Dim myObject As New MySettings With {.Name = "Sample", .Value = 50}
        DataGridView1.DataSource = CurrentModelSettings.GetProperties()

        AddHandler DataGridView1.CellEndEdit, AddressOf DataGridView1_CellEndEdit


    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Dim editedItem As ModelSettings.PropertyItem = CType(DataGridView1.Rows(e.RowIndex).DataBoundItem, ModelSettings.PropertyItem)

        ' Update the original object using PropertyInfo
        Try
            editedItem.PropertyInfo.SetValue(CurrentModelSettings, Convert.ChangeType(editedItem.Value, editedItem.PropertyInfo.PropertyType))
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub LoadSettingsButton_Click(sender As Object, e As EventArgs) Handles LoadSettingsButton.Click


        Dim NewModelSettings As ModelSettings = Nothing

        Dim MyOpenFileDialog As New OpenFileDialog
        MyOpenFileDialog.Title = "Select a model settings file"
        MyOpenFileDialog.Filter = "GAM settings files (*.gam)|*.gam"
        MyOpenFileDialog.CheckPathExists = True
        MyOpenFileDialog.ShowDialog()
        If MyOpenFileDialog.FileName <> "" Then

            Try

                NewModelSettings = ModelSettings.LoadFromFile(MyOpenFileDialog.FileName)
                CurrentSettings_Label.Text = IO.Path.GetFileName(MyOpenFileDialog.FileName)

            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

        Else
            MsgBox("No file selected!")
            Exit Sub
        End If

        If NewModelSettings IsNot Nothing Then

            RemoveHandler DataGridView1.CellEndEdit, AddressOf DataGridView1_CellEndEdit

            CurrentModelSettings = NewModelSettings

            'Dim myObject As New MySettings With {.Name = "Sample", .Value = 50}
            DataGridView1.DataSource = CurrentModelSettings.GetProperties()

            AddHandler DataGridView1.CellEndEdit, AddressOf DataGridView1_CellEndEdit

        End If


    End Sub

    Private Sub SaveSettingsButton_Click(sender As Object, e As EventArgs) Handles SaveSettingsButton.Click

        If CurrentModelSettings IsNot Nothing Then
            Dim MySaveFileDialog As New SaveFileDialog
            MySaveFileDialog.Title = "Save as..."
            MySaveFileDialog.Filter = "GAM settings files (*.gam)|*.gam"
            MySaveFileDialog.CheckPathExists = True
            MySaveFileDialog.ShowDialog()
            If MySaveFileDialog.FileName <> "" Then
                CurrentModelSettings.SaveToFile(MySaveFileDialog.FileName)
            End If
        Else
            MsgBox("No settings to save!")
        End If

    End Sub

    Private Sub ShowReceptionFormButton_Click(sender As Object, e As EventArgs) Handles ShowReceptionFormButton.Click

        'Getting type of reception

        If Reception_ComboBox.SelectedItem Is Nothing Then
            MsgBox("You must select an audiology reception type.")
            Exit Sub
        End If

        StartModelling_Button.Enabled = True

        'This pice of silly code, exist only because the ComboBox does not display ToString method correctly! Maybe a .NET 8 bug?
        Dim NewGAM1 As IAudiologyReceptionForm = New GAM_0_1
        Dim NewGAM2 As IAudiologyReceptionForm = New GAM_0_2

        If Reception_ComboBox.SelectedItem = NewGAM1.ToString Then
            MyAudiologyReceptionForm = NewGAM1
        ElseIf Reception_ComboBox.SelectedItem = NewGAM2.ToString Then
            MyAudiologyReceptionForm = NewGAM2
        Else
            Throw New Exception("This is a bug! The selected audiology reception cannot be found")
        End If

        'Getting the selected reception
        'MyAudiologyReceptionForm = Reception_ComboBox.SelectedItem

        'Showing the reception
        MyAudiologyReceptionForm.Show()

        MyAudiologyReception = MyAudiologyReceptionForm.AudiologyReception


    End Sub


    Private Sub StartModelling_Button_Click(sender As Object, e As EventArgs) Handles StartModelling_Button.Click

        'Clearing lists
        PlannedPatientsList = New List(Of Patient)
        InHousePatientsList = New List(Of Patient)
        PatientsGoneHomeList = New List(Of Patient)
        AllPatientList = New List(Of Patient)
        AudiologistList = New List(Of Audiologist)
        AudiologyAssistantList = New List(Of AudiologyAssistant)
        PersonnelList = New List(Of Personnel)

        If MyAudiologyReception Is Nothing Then
            MsgBox("No audiology reception selected!")
            Exit Sub
        End If

        If CurrentModelSettings Is Nothing Then
            MsgBox("No simulation settings loaded!")
            Exit Sub
        End If

        'Setting buttons enabled state
        StartModelling_Button.Enabled = False
        StopSimulationButton.Enabled = True
        GetStatisticsButton.Enabled = True

        'Getting the speed-up-time value (SpeedUpFactor can be read from the GUI)
        StepTimer.Interval = System.Math.Max(1, CurrentModelSettings.TickScale * 1000 / CurrentModelSettings.SpeedUpFactor)

        'Resetting CurrentTime 
        CurrentTime = New TimeSpan

        'Creating personnel
        For i = 0 To CurrentModelSettings.NumberOfAudiologists - 1

            'Dim NewPersonnel = New Audiologist With {.BackgroundImage = Image.FromFile("Mange_gif.gif"), .BackgroundImageLayout = ImageLayout.Zoom}
            Dim NewPersonnel = New Audiologist
            AudiologistList.Add(NewPersonnel)
            PersonnelList.Add(NewPersonnel)
        Next
        For i = 0 To CurrentModelSettings.NumberOfAudiologyAssistants - 1
            Dim NewPersonnel = New AudiologyAssistant
            AudiologyAssistantList.Add(NewPersonnel)
            PersonnelList.Add(NewPersonnel)
        Next

        'Adding random short breaks
        Dim ShortBreakList As New List(Of BreakTime)
        Dim BreakStartTime As Double = 0
        For i = 0 To 2 * CurrentModelSettings.NumberOfSimultaneousShortBreaks - 1 'Creating more breaks than probably needed. This code is not safe and could crash if lots of small breaks are requested
            Do Until BreakStartTime > CurrentModelSettings.WorkMinutes
                Dim ShortBreakDuration = Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AverageShortBreakTime, CurrentModelSettings.SdShortBreakTime))
                Dim NewBreakTime = New BreakTime With {.StartTime = TimeSpan.FromMinutes(BreakStartTime), .Duration = TimeSpan.FromMinutes(ShortBreakDuration)}
                ShortBreakList.Add(NewBreakTime)
                BreakStartTime += ShortBreakDuration
            Loop
        Next

        'Shuffling the breaks
        Dim ShortBreakArray = ShortBreakList.ToArray
        Randomizer.Shuffle(ShortBreakArray)
        ShortBreakList = ShortBreakArray.ToList

        'And portioning them out on the personnell
        Dim BreakListIndex As Integer = 0
        For Each Personnel In PersonnelList
            Dim NumberOfBreaks = Math.Round(Math.Max(0, (CurrentModelSettings.WorkMinutes / 60) * MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AverageNumberOfShortBreaksPerHour, CurrentModelSettings.SdNumberOfShortBreaksPerHour)))
            For i = 0 To NumberOfBreaks - 1
                Personnel.BreakTimes.Add(ShortBreakList(BreakListIndex))
                BreakListIndex += 1
            Next
        Next

        'Sorting the breaks back into chronological order
        For Each Personnel In PersonnelList
            Personnel.BreakTimes.Sort()
        Next


        'Planning patient arrival times
        CreatePatients()

        'Putting the personnel on the floor
        StartWork()

        'Starting the timer
        StepTimer.Start()

    End Sub


    Private Sub CreatePatients()

        PlannedPatientsList = New List(Of Patient)

        Dim AddedPatientsOpeningStage As Integer = 0
        Dim AddedPatientsMainStage As Integer = 0

        Dim OpeningStagePatientCount As Integer = MyAudiologyReception.GetGamSpaces(GamSpaceTypes.AHM).Count

        Do

            'Determines if in reception opening stage
            Dim IsInOpeningStage As Boolean = False

            If AddedPatientsOpeningStage < OpeningStagePatientCount Then
                IsInOpeningStage = True
            End If

            'Calculating the scheduled time
            Dim ScheduledTime As Double

            If IsInOpeningStage = True Then

                'Using the standard new patient interval divided by the OpeningStagePatientCount (rounding to whole minutes)
                ScheduledTime = Math.Round(AddedPatientsOpeningStage * (CurrentModelSettings.AverageUAudTime / OpeningStagePatientCount))
            Else

                Dim OpeningStageDuration = CurrentModelSettings.AverageUAudTime

                'Using the standard new patient interval (rounding to whole minutes)
                ScheduledTime = OpeningStageDuration + Math.Round(AddedPatientsMainStage * CurrentModelSettings.NewPatientInterval)
            End If

            'Exiting if closed
            If ScheduledTime > CurrentModelSettings.OpenMinutes Then Exit Do

            'Sampling arrival time accuracy (patients coming early or late)
            Dim PatientArrivalAccuracyTime As Double = MathNet.Numerics.Distributions.Normal.Sample(-CurrentModelSettings.PatientsMeanArrivalTimeBeforeAppointment, CurrentModelSettings.SdNewPatientInterval)

            'Calculating the actual modeled patient arrival time
            Dim PatientAddMinute As Double = ScheduledTime + PatientArrivalAccuracyTime

            'Converting to TimeSpan and adding to PatientArrivalTimes list
            Dim PatientArrivalTime As TimeSpan = TimeSpan.FromMinutes(PatientAddMinute)

            Dim NewPatient As New Patient
            NewPatient.ScheduledAppointment = TimeSpan.FromMinutes(ScheduledTime)
            NewPatient.LatestActivityFinished = True

            'Creating and storing the actual (simulated) arrival time as a patient activity (This activity should not be recreated when the patient is added to the GUI waiting room)
            Dim PatientArrival As New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Väntar, .StartTime = PatientArrivalTime}
            NewPatient.ActivityList.Add(PatientArrival)

            'Randomizing a probability that the patient will need to take manual PTA after UAud
            Dim BernoulliSample = MathNet.Numerics.Distributions.Bernoulli.Sample(Randomizer, CurrentModelSettings.ManualPtaProportion)
            If BernoulliSample = 1 Then
                NewPatient.NeedManualPTA = True
            Else
                NewPatient.NeedManualPTA = False
            End If

            'Adding the patient
            PlannedPatientsList.Add(NewPatient)

            'Adding the patient to AllPatientList as well
            AllPatientList.Add(NewPatient)

            'Incrementing the number of patients
            If IsInOpeningStage = True Then
                AddedPatientsOpeningStage += 1
            Else
                AddedPatientsMainStage += 1
            End If

        Loop

    End Sub

    Private Sub StartWork()

        For Each personnel In PersonnelList
            MyAudiologyReception.MovePersonToSpace(personnel, AvailablePlaceRequest(GamSpaceTypes.Personalyta))
        Next

    End Sub

    Private Sub StopWorking()

        For Each personnel In PersonnelList
            MyAudiologyReception.MovePersonToSpace(personnel, AvailablePlaceRequest(GamSpaceTypes.Utgång))
        Next

    End Sub

    Private Sub SummariseDay()

        CalculateAndDisplayStatistics()

    End Sub

    Private Sub Launch_Timer1_Tick(sender As Object, e As EventArgs) Handles StepTimer.Tick

        'Updating the speed-up factor
        StepTimer.Interval = System.Math.Max(1, CurrentModelSettings.TickScale * 1000 / CurrentModelSettings.SpeedUpFactor)

        'Increasing CurrentTime 
        CurrentTime += TimeSpan.FromSeconds(CurrentModelSettings.TickScale)

        Me.Invalidate()
        Me.Update()

        'Updating the current time
        MyAudiologyReception.UpdateTime(CurrentTime)

        '0 Checking if day is finished
        If CurrentTime > TimeSpan.FromMinutes(CurrentModelSettings.WorkMinutes) Then
            StopWorking()
            StepTimer.Stop()
            SummariseDay()
            Exit Sub
        End If

        '0 Checking if personnel should go on break
        For Each Personnel In PersonnelList

            'Counting personnel on break
            Dim NumbersOnBreak As Integer = 0
            For Each InnerPersonnel In PersonnelList
                If InnerPersonnel.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Rast Then NumbersOnBreak += 1
            Next

            'Letting personnel on break only if not already NumberOfSimultaneousShortBreaks are on break
            If NumbersOnBreak < CurrentModelSettings.NumberOfSimultaneousShortBreaks Then

                'Taking break only when not busy
                If Personnel.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then

                    'Checking if it's break time
                    Dim BreakTime = Personnel.BreakTime(CurrentTime)

                    If BreakTime IsNot Nothing Then
                        'It is break time for this personnel

                        'Timing the last task by setting it's duration
                        If Personnel.GetCurrentTask.HasDurationValue = False Then
                            Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                        End If

                        Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Rast, .StartTime = CurrentTime, .Duration = BreakTime.Duration, .HasDurationValue = True})
                        MyAudiologyReception.MovePersonToSpace(Personnel, AvailablePlaceRequest(GamSpaceTypes.PersonalRum))
                    End If
                End If

            End If

        Next


        '1. Adding new patients to the waiting room
        If PlannedPatientsList.Count > 0 Then

            'Stores the patients that hasn't come yet
            Dim RemainingPatients = New List(Of Patient)

            For i = 0 To PlannedPatientsList.Count - 1

                Dim CurrentPatient = PlannedPatientsList(i)

                'Current activity here should always be WaitingInWaitingRoom (as this was stored when the patient was created)
                If CurrentPatient.GetLastStartedActivity.ActivityType <> PatientActivity.PatientActivityTypes.Väntar Then Throw New Exception("A bug exists here! At this stage the patient activity should always be WaitingInWaitingRoom")

                If CurrentTime > CurrentPatient.GetLastStartedActivity.StartTime Then

                    'The patient has arrived, adding it to the waiting room
                    InHousePatientsList.Add(CurrentPatient)
                    MyAudiologyReception.MovePersonToSpace(CurrentPatient, AvailablePlaceRequest(GamSpaceTypes.Väntrum)) 'There should always be space in the waiting room, so we do not check here is places are available

                Else
                    'Adding the patients that hasn't arrived to remaining patients
                    RemainingPatients.Add(CurrentPatient)
                End If
            Next

            'Replacing DayPatientsList with the remaining patients
            PlannedPatientsList = RemainingPatients

        End If

        '2. Checking if patient activities with end times are completed
        For Each Patient In InHousePatientsList

            If Patient.GetLastStartedActivity.HasDurationValue = True Then

                If Patient.GetLastStartedActivity.IsCompleted(CurrentTime) Then
                    Patient.LatestActivityFinished = True
                Else
                    Patient.LatestActivityFinished = False
                End If

            End If

        Next

        '3. Checking if personnel tasks with end times are completed
        For Each Personnel In PersonnelList
            'Checking if a (non-idle) task is completed 
            If Personnel.GetCurrentTask.TaskType <> PersonnelTask.PersonnelTaskTypes.Tillgänglig Then

                If Personnel.GetCurrentTask.IsCompleted(CurrentTime) Then

                    'Noting in a room that it's been cleaned
                    If Personnel.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Rengöring Then
                        If Personnel.Parent IsNot Nothing Then
                            'Noting that the room is cleaned
                            DirectCast(Personnel.Parent, GamSpace).NeedsCleaning = False
                        End If
                    End If

                    'Timing the last task by setting it's duration
                    If Personnel.GetCurrentTask.HasDurationValue = False Then
                        Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                    End If

                    'Noting that the personnel is idle by adding an "idle" task
                    Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})

                End If

            Else

                ' Putting idle personnel back to the floor
                Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Personalyta)
                If AvailabilityCheckResult IsNot Nothing Then
                    MyAudiologyReception.MovePersonToSpace(Personnel, AvailabilityCheckResult)
                End If

            End If
        Next


        '4. Selecting new patient activities and personnel tasks

        'Prioritizing patients
        Dim PatientPrioritizationList As New List(Of Patient)

        Dim PatientSortedInPrioritizedActivitiesList As New SortedList(Of PatientActivity.PatientActivityTypes, List(Of Patient))
        Dim PatientActivityTypeEnumValues = [Enum].GetValues(GetType(PatientActivity.PatientActivityTypes))
        'Adding a list for each enum value, in the order that enum values (i.e. activities) should be prioritized (as set by the constants for each PatientActivityTypes Enum Name)
        For Each EnumValue In PatientActivityTypeEnumValues
            PatientSortedInPrioritizedActivitiesList.Add(EnumValue, New List(Of Patient))
        Next

        'Adding the patients in InHousePatientsList into PatientSortedInPrioritizedActivitiesList
        For Each Patient In InHousePatientsList
            PatientSortedInPrioritizedActivitiesList(Patient.GetLastStartedActivity.ActivityType).Add(Patient)
        Next

        'Adding the patients in the prioritized order into PatientPrioritizationList
        For Each PatientList In PatientSortedInPrioritizedActivitiesList
            PatientPrioritizationList.AddRange(PatientList.Value)
        Next


        'Going through each patient, in the prioritized order
        For Each Patient In PatientPrioritizationList
            If Patient.LatestActivityFinished = True Then

                Dim LatestActivity = Patient.GetLastStartedActivity

                Select Case LatestActivity.ActivityType

                    Case PatientActivity.PatientActivityTypes.MHM

                        'Placing the patient in the queue to conselling

                        Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Kö_Rådgivning)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'Placing patient in the queue to conselling
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Kö_rådgivning, .StartTime = CurrentTime})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult)

                            'Releasing the audiologist, setting it's activity to idle and disconnecting it from the patient

                            'Timing the last task by setting it's duration
                            If Patient.Personnel.GetCurrentTask.HasDurationValue = False Then
                                Patient.Personnel.GetCurrentTask.Duration = CurrentTime - Patient.Personnel.GetCurrentTask.StartTime
                            End If

                            Patient.Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})
                            Patient.Personnel = Nothing

                        End If

                    Case PatientActivity.PatientActivityTypes.Kö_MHM

                        'Checking if manual PTA can be started

                        Dim AvailabilityCheckResult = AvailablePlaceAndPersonnelRequest(GamSpaceTypes.MHM, PersonnelType.Audiologist)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'Setting the wait activity to completed
                            Patient.GetLastStartedActivity.SetToCompleted(CurrentTime)

                            Dim Duration = RandomizePatientActivityDuration(PatientActivity.PatientActivityTypes.MHM)

                            'Noting this is the patient
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.MHM, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult.Item1)

                            'Locking the audiologist to the patient
                            Patient.Personnel = AvailabilityCheckResult.Item2

                            'And in the audiologist
                            Dim Personnel = AvailabilityCheckResult.Item2

                            'Timing the last task by setting it's duration
                            If Personnel.GetCurrentTask.HasDurationValue = False Then
                                Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                            End If

                            Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.MHM, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(AvailabilityCheckResult.Item2, AvailabilityCheckResult.Item1)

                        End If

                        'If AvailabilityCheckResult is Nothing, the patient will remain in the queue

                    Case PatientActivity.PatientActivityTypes.Rådgivning

                        'The patient is ready and can go home
                        Patient.VisitCompleted = True

                        Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Journaldatorplatser)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'N.B. There is no limit on work places here. But every personell should have their own computer/work place

                            'Audiologist starts documentation
                            Dim CounsellingAudiologist As Audiologist = Patient.Personnel

                            Dim DocumentationDuration = RandomizePersonnelTaskDuration(PersonnelTask.PersonnelTaskTypes.Journal)

                            'Timing the last task by setting it's duration
                            If CounsellingAudiologist.GetCurrentTask.HasDurationValue = False Then
                                CounsellingAudiologist.GetCurrentTask.Duration = CurrentTime - CounsellingAudiologist.GetCurrentTask.StartTime
                            End If

                            CounsellingAudiologist.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Journal, .StartTime = CurrentTime, .Duration = DocumentationDuration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(CounsellingAudiologist, AvailabilityCheckResult)

                            'Releasing the audiologist from the patient
                            Patient.Personnel = Nothing

                        Else
                            Throw New Exception("Out of office places for audiologists! This will not work without a queue to descs.")
                        End If


                    Case PatientActivity.PatientActivityTypes.Kö_rådgivning

                        'Checking if counseling can be started

                        Dim AvailabilityCheckResult = AvailablePlaceAndPersonnelRequest(GamSpaceTypes.Samtalsrum, PersonnelType.Audiologist)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'Setting the wait activity to completed
                            Patient.GetLastStartedActivity.SetToCompleted(CurrentTime)

                            Dim Duration = RandomizePatientActivityDuration(PatientActivity.PatientActivityTypes.Rådgivning)

                            'Noting this is the patient
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Rådgivning, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult.Item1)

                            'Locking the audiologist to the patient
                            Patient.Personnel = AvailabilityCheckResult.Item2

                            'And in the audiologist
                            Dim Personnel = AvailabilityCheckResult.Item2

                            'Timing the last task by setting it's duration
                            If Personnel.GetCurrentTask.HasDurationValue = False Then
                                Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                            End If

                            Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Rådgivning, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(AvailabilityCheckResult.Item2, AvailabilityCheckResult.Item1)

                        End If

                        'If AvailabilityCheckResult is Nothing, the patient will remain in the queue


                    Case PatientActivity.PatientActivityTypes.AHM_kvalitetsbedömning

                        'UAud evaluation was finished. Depending on the need for manual PTA, patients either go to manual PTA queue or on to councelling queue
                        If Patient.NeedManualPTA = False Then

                            'The patient can go on to counselling. 'Placing patient in the queue to conselling
                            Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Kö_Rådgivning)
                            If AvailabilityCheckResult IsNot Nothing Then

                                'Placing patient in the queue to conselling
                                Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Kö_rådgivning, .StartTime = CurrentTime})
                                MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult)

                                'Releasing the audiologist, setting it's activity to idle and disconnecting it from the patient

                                'Timing the last task by setting it's duration
                                If Patient.Personnel.GetCurrentTask.HasDurationValue = False Then
                                    Patient.Personnel.GetCurrentTask.Duration = CurrentTime - Patient.Personnel.GetCurrentTask.StartTime
                                End If

                                Patient.Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})
                                Patient.Personnel = Nothing

                            End If


                        Else

                            'The patient needs manual PTA. Placing it in the queue for manual PTA
                            Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Kö_MHM)
                            If AvailabilityCheckResult IsNot Nothing Then

                                'The patient need to do manual audiometry, putting the patient in the queue
                                Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Kö_MHM, .StartTime = CurrentTime})
                                MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult)

                                'Releasing the audiologist, setting it's activity to idle and disconnecting it from the patient

                                'Timing the last task by setting it's duration
                                If Patient.Personnel.GetCurrentTask.HasDurationValue = False Then
                                    Patient.Personnel.GetCurrentTask.Duration = CurrentTime - Patient.Personnel.GetCurrentTask.StartTime
                                End If

                                Patient.Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})
                                Patient.Personnel = Nothing

                            End If

                        End If


                    Case PatientActivity.PatientActivityTypes.Kö_AHM_kvalitetsbedömning

                        'The patient is in the Queue for UAud evaluation. Checking if UAud evaluation can be done. If not the patient will remein in the queue.
                        Dim AvailabilityCheckResult = AvailablePlaceAndPersonnelRequest(GamSpaceTypes.Samtalsrum, PersonnelType.Audiologist)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'Setting the wait activity to completed
                            Patient.GetLastStartedActivity.SetToCompleted(CurrentTime)

                            Dim Duration = RandomizePatientActivityDuration(PatientActivity.PatientActivityTypes.AHM_kvalitetsbedömning)

                            'Noting this in the patient
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.AHM_kvalitetsbedömning, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult.Item1)

                            'Locking the audiologist to the patient
                            Patient.Personnel = AvailabilityCheckResult.Item2

                            'And in the audiologist
                            Dim Personnel = AvailabilityCheckResult.Item2

                            'Timing the last task by setting it's duration
                            If Personnel.GetCurrentTask.HasDurationValue = False Then
                                Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                            End If

                            Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.AHM_kvalitetsbedömning, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(AvailabilityCheckResult.Item2, AvailabilityCheckResult.Item1)

                        End If

                        'If AvailabilityCheckResult is Nothing, the patient will remain in the queue

                    Case PatientActivity.PatientActivityTypes.AHM_Avslut

                        'The UAud results is finished being printed. The patient keeps the printed results to give to the next available audiologist  
                        'Putting the patient in the queue for UAud evaluation

                        Dim AvailabilityCheckResult = AvailablePlaceAndPersonnelRequest(GamSpaceTypes.Kö_AHM_kvalitetsbedömning, PersonnelType.Any)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'UAud is finished, patient is placed in the queue for AHM quality evaluation
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Kö_AHM_kvalitetsbedömning, .StartTime = CurrentTime})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult.Item1)

                            'Releasing the personnel, setting it's activity to idle and disconnecting it from the patient

                            'Timing the last task by setting it's duration
                            If Patient.Personnel.GetCurrentTask.HasDurationValue = False Then
                                Patient.Personnel.GetCurrentTask.Duration = CurrentTime - Patient.Personnel.GetCurrentTask.StartTime
                            End If

                            Patient.Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})
                            Patient.Personnel = Nothing

                        End If


                    Case PatientActivity.PatientActivityTypes.AHM

                        'UAud is completed,
                        'Checking the availabilty of a personnel that can perform post measuement actions such as save and print results. Meanwhile, the patient remains in the UAud measurment space.

                        Dim AvailabilityCheckResult = AvailablePersonnelRequest(PersonnelType.Any)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'UAud is finished, performing post measurment administrative tasks
                            Dim Personnel = AvailabilityCheckResult
                            Dim Duration = RandomizePersonnelTaskDuration(PersonnelTask.PersonnelTaskTypes.AHM_Avslut)

                            'Moving the personnel to the patient space. (In reality the personnel also have to go to the printer and back to give the results to the patient.)
                            Dim CurrentPatientLocation As GamSpace = Patient.Parent
                            If CurrentPatientLocation IsNot Nothing Then
                                MyAudiologyReception.MovePersonToSpace(AvailabilityCheckResult, CurrentPatientLocation)
                            Else
                                Throw New Exception("The patient is not is a space. This should not happen.")
                            End If

                            'Timing the last task by setting it's duration
                            If Personnel.GetCurrentTask.HasDurationValue = False Then
                                Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                            End If

                            Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.AHM_Avslut, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})

                            'Locking the personnel to the patient
                            Patient.Personnel = Personnel

                            'UAud is finished, patient gets the task to wait for AHM_Avslut, staying in the same space
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.AHM_Avslut, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})

                        End If

                    Case PatientActivity.PatientActivityTypes.Kö_AHM

                        'Checking if UAud can be started

                        Dim AvailabilityCheckResult = AvailablePlaceAndPersonnelRequest(GamSpaceTypes.AHM, PersonnelType.Any)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'Setting the wait activity to completed
                            Patient.GetLastStartedActivity.SetToCompleted(CurrentTime)

                            Dim UAudDuration = RandomizePatientActivityDuration(PatientActivity.PatientActivityTypes.AHM)

                            'Noting this is the patient
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.AHM, .StartTime = CurrentTime, .Duration = UAudDuration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult.Item1)

                            'And in the audiologist
                            Dim UAudStartHelpDuration = RandomizePersonnelTaskDuration(PersonnelTask.PersonnelTaskTypes.AHM_Start)
                            Dim Personnel = AvailabilityCheckResult.Item2

                            'Timing the last task by setting it's duration
                            If Personnel.GetCurrentTask.HasDurationValue = False Then
                                Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                            End If

                            Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.AHM_Start, .StartTime = CurrentTime, .Duration = UAudStartHelpDuration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(AvailabilityCheckResult.Item2, AvailabilityCheckResult.Item1)

                        End If

                        'If AvailabilityCheckResult is Nothing, the patient will remain in the queue

                    Case PatientActivity.PatientActivityTypes.Intervju

                        'Patient interview is finished, placing the patient in queue for UAud

                        Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Kö_AHM)
                        If AvailabilityCheckResult IsNot Nothing Then

                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Kö_AHM, .StartTime = CurrentTime})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult)

                            'Releasing the personnel, setting it's activity to idle and disconnecting it from the patient

                            'Timing the last task by setting it's duration
                            If Patient.Personnel.GetCurrentTask.HasDurationValue = False Then
                                Patient.Personnel.GetCurrentTask.Duration = CurrentTime - Patient.Personnel.GetCurrentTask.StartTime
                            End If

                            Patient.Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})
                            Patient.Personnel = Nothing

                        End If


                    Case PatientActivity.PatientActivityTypes.Väntar

                        'Checking if the patient can be taken into interview

                        Dim AvailabilityCheckResult = AvailablePlaceAndPersonnelRequest(GamSpaceTypes.Samtalsrum, PersonnelType.Any)
                        If AvailabilityCheckResult IsNot Nothing Then

                            'Setting the wait activity to completed
                            Patient.GetLastStartedActivity.SetToCompleted(CurrentTime)

                            'Starting patient interview

                            Dim Duration = RandomizePatientActivityDuration(PatientActivity.PatientActivityTypes.Intervju)

                            'Noting this in the patient
                            Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Intervju, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult.Item1)

                            'Locking the audiologist to the patient
                            Patient.Personnel = AvailabilityCheckResult.Item2

                            'And in the personnel
                            Dim Personnel = AvailabilityCheckResult.Item2

                            'Timing the last task by setting it's duration
                            If Personnel.GetCurrentTask.HasDurationValue = False Then
                                Personnel.GetCurrentTask.Duration = CurrentTime - Personnel.GetCurrentTask.StartTime
                            End If

                            Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Intervju, .StartTime = CurrentTime, .Duration = Duration, .HasDurationValue = True})
                            MyAudiologyReception.MovePersonToSpace(Personnel, AvailabilityCheckResult.Item1)

                        End If

                        'If AvailabilityCheckResult is Nothing, the patient will remain in the queue


                End Select

            End If

        Next

        '5. Checking if cleaning needs to be done
        Dim AllGamPlaces = MyAudiologyReception.GetAllGamSpaces
        Dim PlacesToClean As New List(Of GamSpace)
        For Each GamPlace In AllGamPlaces
            If GamPlace.NeedsCleaning = True Then

                'Checking if there is already someone there cleaning
                If GamPlace.SomeoneIsCleaning Then Exit For

                'Looking for personell that can clean
                Dim PersonnelRequest = AvailablePersonnelRequest(PersonnelType.Any)
                If PersonnelRequest IsNot Nothing Then

                    'Timing the last task by setting it's duration
                    If PersonnelRequest.GetCurrentTask.HasDurationValue = False Then
                        PersonnelRequest.GetCurrentTask.Duration = CurrentTime - PersonnelRequest.GetCurrentTask.StartTime
                    End If

                    PersonnelRequest.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Rengöring, .StartTime = CurrentTime,
                                                  .Duration = RandomizePersonnelTaskDuration(PersonnelTask.PersonnelTaskTypes.Rengöring), .HasDurationValue = True})
                    MyAudiologyReception.MovePersonToSpace(PersonnelRequest, GamPlace)

                End If
            End If
        Next


        '6. Removing patients whos' visits are finished
        WaveOffPatients()

        '7. Regestering gam space use
        MyAudiologyReception.CountGamSpaceUse()

    End Sub

    Private Function RandomizePatientActivityDuration(ByVal ActivityType As PatientActivity.PatientActivityTypes) As TimeSpan

        Select Case ActivityType
            Case PatientActivity.PatientActivityTypes.Enkät

                Throw New Exception("Questionnaries not yet implemented")

            Case PatientActivity.PatientActivityTypes.AHM_kvalitetsbedömning
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.UAudEvaluationTime, CurrentModelSettings.SdUAudEvaluationTime)))

            Case PatientActivity.PatientActivityTypes.Rådgivning
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AveragePatientCounsellingTime, CurrentModelSettings.SdPatientCounsellingTime)))

            Case PatientActivity.PatientActivityTypes.Intervju
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AveragePatientInterviewTime, CurrentModelSettings.SdPatientInterviewTime)))

            Case PatientActivity.PatientActivityTypes.MHM
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AverageManualPtaTime, CurrentModelSettings.SdManualPtaTime)))

            Case PatientActivity.PatientActivityTypes.AHM
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AverageUAudTime, CurrentModelSettings.SdUAudTime)))

            Case Else
                Throw New Exception("The duration of the activity " & ActivityType.ToString & "cannot be predicted!")
        End Select

    End Function

    Private Function RandomizePersonnelTaskDuration(ByVal TaskType As PersonnelTask.PersonnelTaskTypes) As TimeSpan

        Select Case TaskType

            Case PersonnelTask.PersonnelTaskTypes.AHM_Avslut
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.UAudPostAdminTime, CurrentModelSettings.SdUAudPostAdminTime)))

            Case PersonnelTask.PersonnelTaskTypes.Rengöring
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.CleaningTime, CurrentModelSettings.SdCleaningTime)))

            Case PersonnelTask.PersonnelTaskTypes.AHM_Start
                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.UAudStartAssistanceTime, CurrentModelSettings.SdUAudStartAssistanceTime)))

            Case PersonnelTask.PersonnelTaskTypes.Journal

                Return TimeSpan.FromMinutes(Math.Max(0.1, MathNet.Numerics.Distributions.Normal.Sample(CurrentModelSettings.AverageDocumentationTime, CurrentModelSettings.SdDocumentationTime)))

            Case Else
                Throw New Exception("The duration of the task " & TaskType.ToString & "cannot be predicted!")

        End Select

    End Function




    ''' <summary>
    ''' Returns the place and personnel if available, or Nothing if no place or personnel is available.
    ''' </summary>
    ''' <returns></returns>
    Public Function AvailablePlaceAndPersonnelRequest(ByVal RequestedPlaceType As GamSpaceTypes, ByVal RequestedPersonnelType As PersonnelType) As Tuple(Of GamSpace, Personnel)

        'Looking in all places/rooms of the specified type
        For Each Room In MyAudiologyReception.GetGamSpaces(RequestedPlaceType)
            If Room.IsAvaliable = True Then

                Select Case RequestedPersonnelType

                    'Looking first among the audiology assistants
                    Case PersonnelType.AudiologyAssistant

                        'Checking if there is an audiology assistant available
                        For Each AudiologyAssistant In AudiologyAssistantList
                            If AudiologyAssistant.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                                Return New Tuple(Of GamSpace, Personnel)(Room, AudiologyAssistant)
                            End If
                        Next

                    Case PersonnelType.Audiologist

                        'Checking if there is an audiologist available
                        For Each Audiologist In AudiologistList
                            If Audiologist.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                                Return New Tuple(Of GamSpace, Personnel)(Room, Audiologist)
                            End If
                        Next

                    Case PersonnelType.Any

                        'Checking if there is an audiology assistant available
                        For Each AudiologyAssistant In AudiologyAssistantList
                            If AudiologyAssistant.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                                Return New Tuple(Of GamSpace, Personnel)(Room, AudiologyAssistant)
                            End If
                        Next

                        'Checking if there is an audiologist available
                        For Each Audiologist In AudiologistList
                            If Audiologist.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                                Return New Tuple(Of GamSpace, Personnel)(Room, Audiologist)
                            End If
                        Next

                    Case Else
                        Throw New Exception("Unknown PersonnelType")
                End Select

            End If
        Next

        Return Nothing

    End Function



    ''' <summary>
    ''' Returns the place if available, or Nothing if no place is available.
    ''' </summary>
    ''' <returns></returns>
    Public Function AvailablePlaceRequest(ByVal RequestedPlaceType As GamSpaceTypes) As GamSpace

        'Looking in all places/rooms of the specified type
        For Each Room In MyAudiologyReception.GetGamSpaces(RequestedPlaceType)
            If Room.IsAvaliable = True Then
                Return Room
            End If
        Next

        Return Nothing

    End Function

    ''' <summary>
    ''' Returns the personnel if available, or Nothing if no personnel is available.
    ''' </summary>
    ''' <returns></returns>
    Public Function AvailablePersonnelRequest(ByVal RequestedPersonnelType As PersonnelType) As Personnel


        Select Case RequestedPersonnelType

                    'Looking first among the audiology assistants
            Case PersonnelType.AudiologyAssistant

                'Checking if there is an audiology assistant available
                For Each AudiologyAssistant In AudiologyAssistantList
                    If AudiologyAssistant.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                        Return AudiologyAssistant
                    End If
                Next

            Case PersonnelType.Audiologist

                'Checking if there is an audiologist available
                For Each Audiologist In AudiologistList
                    If Audiologist.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                        Return Audiologist
                    End If
                Next

            Case PersonnelType.Any

                'Checking if there is an audiology assistant available
                For Each AudiologyAssistant In AudiologyAssistantList
                    If AudiologyAssistant.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                        Return AudiologyAssistant
                    End If
                Next

                'Checking if there is an audiologist available
                For Each Audiologist In AudiologistList
                    If Audiologist.GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                        Return Audiologist
                    End If
                Next

            Case Else
                Throw New Exception("Unknown PersonnelType")
        End Select

        Return Nothing

    End Function


    Public Enum PersonnelType
        Audiologist
        AudiologyAssistant
        Any
    End Enum

    Public Sub WaveOffPatients()

        'Creating two temporary lists, one for patients that can go home and one for patients not yet ready to go home
        Dim RemainingPatients As New List(Of Patient)
        Dim PatientsDone As New List(Of Patient)

        For Each Patient In InHousePatientsList
            'Adding patients to the two lists depending on their VisitCompleted 
            If Patient.VisitCompleted = True Then
                PatientsDone.Add(Patient)

                Dim AvailabilityCheckResult = AvailablePlaceRequest(GamSpaceTypes.Utgång)
                If AvailabilityCheckResult IsNot Nothing Then

                    Patient.ActivityList.Add(New PatientActivity With {.ActivityType = PatientActivity.PatientActivityTypes.Hemgång, .StartTime = CurrentTime})
                    MyAudiologyReception.MovePersonToSpace(Patient, AvailabilityCheckResult)

                    'Releasing any personnel, setting it's activity to idle and disconnecting it from the patient
                    If Patient.Personnel IsNot Nothing Then

                        'Timing the last task by setting it's duration
                        If Patient.Personnel.GetCurrentTask.HasDurationValue = False Then
                            Patient.Personnel.GetCurrentTask.Duration = CurrentTime - Patient.Personnel.GetCurrentTask.StartTime
                        End If

                        Patient.Personnel.TaskList.Add(New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig, .StartTime = CurrentTime})
                        Patient.Personnel = Nothing
                    End If

                End If

            Else
                RemainingPatients.Add(Patient)
            End If
        Next

        'Replacing the InHousePatientsList by trhe RemainingPatients list
        InHousePatientsList = RemainingPatients

        'Adding the patients that has gone home to the PatientsGoneHomeList
        PatientsGoneHomeList.AddRange(PatientsDone)

    End Sub

    Private Sub StopSimulationButton_Click(sender As Object, e As EventArgs) Handles StopSimulationButton.Click

        StepTimer.Stop()

        StartModelling_Button.Enabled = True
        StopSimulationButton.Enabled = False

    End Sub

    Private Sub GetStatisticsButton_Click(sender As Object, e As EventArgs) Handles GetStatisticsButton.Click
        CalculateAndDisplayStatistics()
    End Sub

    Private Sub CalculateAndDisplayStatistics()

        Dim PatientActivityDurationList As New SortedList(Of String, List(Of Double))

        'Summarizing patient activities
        For Each Patient In AllPatientList

            'For Each Patient In PatientsGoneHomeList
            For Each Activity In Patient.ActivityList
                If PatientActivityDurationList.ContainsKey(Activity.ActivityType.ToString) = False Then PatientActivityDurationList.Add(Activity.ActivityType.ToString, New List(Of Double))
                PatientActivityDurationList(Activity.ActivityType.ToString).Add(Activity.Duration.TotalMinutes)
            Next
        Next

        Dim PatientTempList = PatientActivityDurationList.ToList().OrderByDescending(Function(kvp) kvp.Value.Average()).ToList()

        Dim SortedPatientActivityDurationList As New List(Of Tuple(Of String, List(Of Double)))
        For Each kvp In PatientTempList
            SortedPatientActivityDurationList.Add(New Tuple(Of String, List(Of Double))(kvp.Key, kvp.Value))
        Next

        Dim PatientActivityDurationStringList As New List(Of String)
        PatientActivityDurationStringList.Add("Patient activity summary (minutes)")
        PatientActivityDurationStringList.Add("MEAN" & vbTab & "SD" & vbTab & "ACTIVITY TIME")
        For Each ListItem In SortedPatientActivityDurationList
            PatientActivityDurationStringList.Add(Math.Round(ListItem.Item2.Average, 1) & vbTab & "(" & Math.Round(MathNet.Numerics.Statistics.Statistics.StandardDeviation(ListItem.Item2.ToArray), 1) & ")" & vbTab & ListItem.Item1)
        Next
        'PatientActivityDurationStringList.Add("* POT = proportion of personnel work time (" & CurrentModelSettings.WorkMinutes \ 60 & " hours and " & CurrentModelSettings.WorkMinutes Mod 60 & " minutes)")


        'Summarizing personnel tasks
        Dim PersonnelTaskDurationList As New SortedList(Of String, List(Of Double))

        For Each Personnel In PersonnelList
            For Each Task In Personnel.TaskList
                If PersonnelTaskDurationList.ContainsKey(Task.TaskType.ToString) = False Then PersonnelTaskDurationList.Add(Task.TaskType.ToString, New List(Of Double))
                PersonnelTaskDurationList(Task.TaskType.ToString).Add(Task.Duration.TotalMinutes)
            Next
        Next

        Dim PersonnelTempList = PersonnelTaskDurationList.ToList().OrderByDescending(Function(kvp) kvp.Value.Average()).ToList()

        Dim SortedPersonnelTaskDurationList As New List(Of Tuple(Of String, List(Of Double)))
        For Each kvp In PersonnelTempList
            SortedPersonnelTaskDurationList.Add(New Tuple(Of String, List(Of Double))(kvp.Key, kvp.Value))
        Next

        Dim PersonnelTaskDurationStringList As New List(Of String)
        PersonnelTaskDurationStringList.Add("Personnel Task summary (minutes)")
        PersonnelTaskDurationStringList.Add("MEAN" & vbTab & "SD" & vbTab & "TASK TYPE")
        For Each ListItem In SortedPersonnelTaskDurationList
            PersonnelTaskDurationStringList.Add(Math.Round(ListItem.Item2.Average, 1) & vbTab & "(" & Math.Round(MathNet.Numerics.Statistics.Statistics.StandardDeviation(ListItem.Item2.ToArray), 1) & ")" & vbTab & ListItem.Item1)
        Next
        'PersonnelTaskDurationStringList.Add("* POT = proportion of personnel work time (" & CurrentModelSettings.WorkMinutes \ 60 & " hours and " & CurrentModelSettings.WorkMinutes Mod 60 & " minutes)")


        'Summarizing space use
        Dim SpaceUseSummaryList = MyAudiologyReception.SummarizeSpaceUse()

        Dim SpaceUseTempList = SpaceUseSummaryList.ToList().OrderByDescending(Function(kvp) kvp.Value).ToList()

        Dim SortedSpaceUseSummaryList As New List(Of Tuple(Of String, Double))
        For Each kvp In SpaceUseTempList
            SortedSpaceUseSummaryList.Add(New Tuple(Of String, Double)(kvp.Key, kvp.Value))
        Next

        Dim SpaceTaskDurationStringList As New List(Of String)
        SpaceTaskDurationStringList.Add("Space use summary")
        SpaceTaskDurationStringList.Add("POT*" & vbTab & "SPACE TYPE")
        For Each ListItem In SortedSpaceUseSummaryList
            SpaceTaskDurationStringList.Add(Math.Round(100 * ListItem.Item2) & "%" & vbTab & ListItem.Item1)
        Next
        SpaceTaskDurationStringList.Add("* POT = proportion of personell work time each space type is used (" & CurrentModelSettings.WorkMinutes \ 60 & " hours and " & CurrentModelSettings.WorkMinutes Mod 60 & " minutes). (Closed spaces are not counted.)")

        'Summarizing space highest number of persons
        Dim HighestSpaceInhabitantCounts = MyAudiologyReception.GetHighestSpaceInhabitantCount
        Dim HighestSpaceInhabitantCountsList As New List(Of String)
        HighestSpaceInhabitantCountsList.Add("Highest number of persons present simultaneously")
        HighestSpaceInhabitantCountsList.Add("COUNT" & vbTab & "SPACE TYPE")
        For Each kvp In HighestSpaceInhabitantCounts
            HighestSpaceInhabitantCountsList.Add(kvp.Value & vbTab & kvp.Key)
        Next

        'Also exporting the scheduled time for each patient
        Dim TimeSummaryList As New List(Of String)
        TimeSummaryList.Add("Appointment times:")
        TimeSummaryList.Add("Patient nr." & vbTab & "Scheduled" & vbTab & "Arrival" & vbTab & "Let in" & vbTab & "Finished" & vbTab & "Duration")

        Dim VisitDurationList As New List(Of Double)

        For Each Patient In AllPatientList
            Dim ScheduledTime = Patient.ScheduledAppointment
            Dim ScheduledTimeString As String = ""
            If ScheduledTime < TimeSpan.Zero Then ScheduledTimeString = "-"
            ScheduledTimeString &= ScheduledTime.ToString("hh\:mm\:ss")

            Dim ArrivalTime = Patient.ActivityList.First.StartTime
            Dim ArrivalTimeString As String = ""
            If ArrivalTime < TimeSpan.Zero Then ArrivalTimeString = "-"
            ArrivalTimeString &= ArrivalTime.ToString("hh\:mm\:ss")

            Dim LetInTimeString As String = "[not yet]"

            Dim LetInTime As TimeSpan
            For Each Activity In Patient.ActivityList
                If Activity.ActivityType = PatientActivity.PatientActivityTypes.Intervju Then
                    LetInTime = Activity.StartTime
                    If LetInTime < TimeSpan.Zero Then
                        LetInTimeString = "-"
                    Else
                        LetInTimeString = ""
                    End If
                    LetInTimeString &= LetInTime.ToString("hh\:mm\:ss")
                    Exit For
                End If
            Next

            Dim LeaveTimeString As String = "[not yet]"
            Dim DurationTimeString As String = "[not yet]"

            If Patient.ActivityList.Last.ActivityType = PatientActivity.PatientActivityTypes.Hemgång Then
                Dim LeaveTime = Patient.ActivityList.Last.StartTime
                If LeaveTime < TimeSpan.Zero Then
                    LeaveTimeString = "-"
                Else
                    LeaveTimeString = ""
                End If
                LeaveTimeString &= LeaveTime.ToString("hh\:mm\:ss")

                Dim VisitDuration = LeaveTime - LetInTime
                DurationTimeString = VisitDuration.ToString("hh\:mm\:ss")
                VisitDurationList.Add(VisitDuration.TotalMinutes)
            End If

            TimeSummaryList.Add(Patient.ID & vbTab & ScheduledTime.ToString("hh\:mm\:ss") & vbTab & ArrivalTimeString & vbTab & LetInTimeString & vbTab & LeaveTimeString & vbTab & DurationTimeString)
        Next

        Dim VisitDurationAverageString As String = ""
        If VisitDurationList.Count > 0 Then
            VisitDurationAverageString = "Average appointment duration (minutes): " & Math.Round(VisitDurationList.Average(), 1)
        Else
            VisitDurationAverageString = "Average appointment duration (minutes): --- "
        End If

        Dim VisitDurationMaxString As String = ""
        If VisitDurationList.Count > 0 Then
            VisitDurationMaxString = "Longest appointment duration (minutes): " & Math.Round(VisitDurationList.Max(), 1)
        Else
            VisitDurationMaxString = "Longest appointment duration (minutes): --- "
        End If

        StatisticsTextBox.Text = String.Join(vbCrLf, PatientActivityDurationStringList) &
            vbCrLf & vbCrLf &
            String.Join(vbCrLf, PersonnelTaskDurationStringList) &
            vbCrLf & vbCrLf &
            String.Join(vbCrLf, SpaceTaskDurationStringList) &
            vbCrLf & vbCrLf &
            String.Join(vbCrLf, HighestSpaceInhabitantCountsList) &
            vbCrLf & vbCrLf &
            String.Join(vbCrLf, TimeSummaryList) &
            vbCrLf & vbCrLf &
             VisitDurationAverageString &
             vbCrLf & vbCrLf &
             VisitDurationMaxString

    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click

        ClearModeler()
        Reception_ComboBox.SelectedItem = Nothing

    End Sub

    Private Sub Reception_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Reception_ComboBox.SelectedIndexChanged
        ClearModeler()
    End Sub

    Private Sub ClearModeler()

        Patient.ResetIdAssignments()
        AudiologyAssistant.ResetIdAssignments()
        Audiologist.ResetIdAssignments()

        StartModelling_Button.Enabled = True
        StopSimulationButton.Enabled = False
        GetStatisticsButton.Enabled = False

        If MyAudiologyReception IsNot Nothing Then MyAudiologyReception = Nothing

        If MyAudiologyReceptionForm IsNot Nothing Then
            Try
                MyAudiologyReceptionForm.Close()
            Catch ex As Exception
                'Ignoring exception for now
            End Try
        End If

        StatisticsTextBox.Text = ""

    End Sub

End Class