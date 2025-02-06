Public Class Person
    Inherits Label ' A person uses label as base class so that it can be displayed in the GUI

    Public ID As Integer

    Public Sub New()

        'Setting the size and appearance of the control
        Me.Size = New Size(50, 50)
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.TextAlign = ContentAlignment.TopCenter
        Me.Padding = New Padding(1, 4, 1, 1)


    End Sub

End Class


Public Class Patient
    Inherits Person

    Private Shared LastUsedID As Integer = 0

    Public ScheduledAppointment As TimeSpan

    Public LatestActivityFinished As Boolean = False

    Private _ActivityList As List(Of PatientActivity) = New List(Of PatientActivity)

    ''' <summary>
    ''' A personnel that may be "locked" to the patient due to performing a measurement etc. 
    ''' </summary>
    ''' <returns></returns>
    Public Property Personnel As Personnel = Nothing

    Public Property ActivityList As List(Of PatientActivity)
        Get
            Return _ActivityList
        End Get
        Set(value As List(Of PatientActivity))
            _ActivityList = value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Stores a value indicating if todays visit is finished and the patient can go home. This has to be set manually by the model code.
    ''' </summary>
    Public VisitCompleted As Boolean = False

    ''' <summary>
    ''' Store the value whether the patient's UAud is unreliable and the patient therefore needs to take a manual PTA
    ''' </summary>
    Public NeedManualPTA As Boolean = False

    Public Sub Me_Invalidated() Handles Me.Invalidated

        If LatestActivityFinished = True Then
            Me.BackColor = Color.Red
        Else
            Me.BackColor = Color.LightPink
        End If

    End Sub

    Public Function GetLastStartedActivity() As PatientActivity

        Me.Invalidate()

        If _ActivityList.Any Then
            Return _ActivityList.Last
        Else
            Return Nothing
        End If

    End Function


    Public Sub New()

        ' Setting a patient ID depending on the number of earlier created patients
        LastUsedID += 1
        ID = LastUsedID

        'Setting the text property for the GUI
        Me.Text = Me.ToString

    End Sub

    Public Shadows Function ToString()
        Return "P" & ID.ToString()
    End Function

    Public Sub ExtraPaint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        If e.ClipRectangle.Width = 0 Then Exit Sub
        If e.ClipRectangle.Height = 0 Then Exit Sub

        'Writing what the patient is doing
        If _ActivityList.Any Then
            Dim TextSize As Single = e.ClipRectangle.Width / 6
            e.Graphics.DrawString(_ActivityList.Last.ActivityType.ToString, New Font(Me.Font.FontFamily, TextSize), Brushes.Black, New RectangleF(0, e.ClipRectangle.Height - e.ClipRectangle.Height / 3, e.ClipRectangle.Width, e.ClipRectangle.Height / 3))
        End If

    End Sub


End Class

Public Class BreakTime
    Implements IComparable(Of BreakTime)

    Public StartTime As TimeSpan
    Public Duration As TimeSpan

    Public Function CompareTo(other As BreakTime) As Integer Implements IComparable(Of BreakTime).CompareTo
        Return Me.StartTime.CompareTo(other.StartTime)
    End Function

End Class

Public MustInherit Class Personnel
    Inherits Person

    Public BreakTimes As New List(Of BreakTime)

    'Holds track of the number of breaks taken so that each break is taken only once
    Private BreaksReached As Integer = 0

    ''' <summary>
    ''' Returns an instance of BreakTime if it's time to take a break, and the personnel in not already on break. Otherwise returns Nothing. 
    ''' </summary>
    ''' <returns></returns>
    Public Function BreakTime(ByVal CurrentTime As TimeSpan) As BreakTime

        'Returns Nothing if already on break
        If GetCurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Rast Then Return Nothing

        'Goes through each break not yet started and checks if it's time
        For i = BreaksReached To BreakTimes.Count - 1

            Dim CurrentBreakCandidate = BreakTimes(i)

            'Checking if current time has surpassed the starttime plus the duration
            If CurrentTime > CurrentBreakCandidate.StartTime Then
                BreaksReached += 1
                Return CurrentBreakCandidate
            End If
        Next

        Return Nothing

    End Function

    Private _TaskList As New List(Of PersonnelTask) From {New PersonnelTask With {.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig}}
    Public Property TaskList As List(Of PersonnelTask)
        Get
            Return _TaskList
        End Get
        Set(value As List(Of PersonnelTask))
            _TaskList = value
            Me.Invalidate()
        End Set
    End Property

    Public Function GetCurrentTask() As PersonnelTask

        If _TaskList.Any Then
            Return _TaskList.Last
        Else
            Return Nothing
        End If

    End Function


    Public Sub Me_Invalidated() Handles Me.Invalidated

        Dim CurrentTask As PersonnelTask = GetCurrentTask()
        If CurrentTask IsNot Nothing Then
            If CurrentTask.TaskType = PersonnelTask.PersonnelTaskTypes.Tillgänglig Then
                Me.BackColor = Color.Lime
            Else
                Me.BackColor = Color.LightGreen
            End If
        End If

    End Sub

    Public Sub ExtraPaint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        If e.ClipRectangle.Width = 0 Then Exit Sub
        If e.ClipRectangle.Height = 0 Then Exit Sub

        'Writing what the patient is doing
        If _TaskList.Any Then
            Dim TextSize As Single = e.ClipRectangle.Width / 6
            e.Graphics.DrawString(_TaskList.Last.TaskType.ToString, New Font(Me.Font.FontFamily, TextSize), Brushes.Black, New RectangleF(0, e.ClipRectangle.Height - e.ClipRectangle.Height / 3, e.ClipRectangle.Width, e.ClipRectangle.Height / 3))
        End If

    End Sub

End Class



Public Class Audiologist
    Inherits Personnel

    Public Shared LastUsedID As Integer = 0

    Public Sub New()
        ' Setting a ID depending on the number of earlier created patients
        LastUsedID += 1
        ID = LastUsedID

        'Setting the text property for the GUI
        Me.Text = Me.ToString

    End Sub

    Public Shadows Function ToString()
        Return "A" & ID.ToString()
    End Function

End Class

Public Class AudiologyAssistant
    Inherits Personnel

    Public Shared LastUsedID As Integer = 0

    Public Sub New()
        ' Setting a ID depending on the number of earlier created patients
        LastUsedID += 1
        ID = LastUsedID

        'Setting the text property for the GUI
        Me.Text = Me.ToString

    End Sub

    Public Shadows Function ToString()
        Return "U" & ID.ToString()
    End Function

End Class




