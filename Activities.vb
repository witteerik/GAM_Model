
Public Class Activity
    'Before the activity is completed, this contains the estimated activity duration, and after the activity has been completed, this contains the actual duration of the activity.
    Public Duration As TimeSpan

    'The time at which the activity was started
    Public StartTime As TimeSpan

    Public HasDurationValue As Boolean = False

    'Calculates in the basis of CurrentTime if the activity is finished
    Public Function IsCompleted(ByVal CurrentTime As TimeSpan) As Boolean

        'Checking if current time has surpassed the starttime plus the duration
        If CurrentTime > (StartTime + Duration) Then
            Return True
        Else
            Return False
        End If
    End Function

    'Sets the activity to completed by adjusting the duration according to the CurrentTime
    Public Sub SetToCompleted(ByVal CurrentTime As TimeSpan)
        'Setting the final duration. Note that the activity will not actually be stopped until in the next timer tick.
        Duration = CurrentTime - StartTime

        'Also sets the HasDurationValue to true, now that a duration has been established
        HasDurationValue = True

    End Sub

End Class

Public Class PatientActivity
    Inherits Activity

    Public ActivityType As PatientActivityTypes

    Public Enum PatientActivityTypes
        Väntar = 0
        Intervju = 1
        Kö_AHM = 2
        AHM = 3
        AHM_Avslut = 4
        Kö_AHM_kvalitetsbedömning = 5
        AHM_kvalitetsbedömning = 6
        Kö_rådgivning = 7
        Rådgivning = 8
        Kö_MHM = 9
        MHM = 10
        Enkät = 11
        Hemgång = 12
    End Enum

End Class

Public Class PersonnelTask
    Inherits Activity

    Public TaskType As PersonnelTaskTypes

    Public Enum PersonnelTaskTypes
        Tillgänglig = 0
        Intervju = 1
        Rengöring = 2
        AHM_Start = 3 ' This includes helping the patient get started with UAud, and possibly instructions
        AHM_Avslut = 4 ' This may include aving and printing the test results (optimally this should be fully atomatized)
        AHM_kvalitetsbedömning = 5
        MHM = 6
        Rådgivning = 7
        Journal = 8
        Rast = 9
    End Enum

End Class

