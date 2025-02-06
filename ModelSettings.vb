
Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization

<AttributeUsage(AttributeTargets.Property)>
Public Class PropertyDescriptionAttribute
    Inherits Attribute

    Public ReadOnly Property Description As String

    Public Sub New(description As String)
        Me.Description = description
    End Sub
End Class

Public Class ModelSettings
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Class PropertyItem
        Public Property Variable As String
        Public Property Value As Object
        Public Property Description As String
        Public Property TypeName As String

        <Browsable(False)> ' This prevents automatic column generation for PropertyInfo
        Public Property PropertyInfo As Reflection.PropertyInfo
    End Class

    Public Function GetProperties() As BindingList(Of PropertyItem)
        Dim propList As New BindingList(Of PropertyItem)()

        For Each prop In Me.GetType().GetProperties()

            ' Get the custom description attribute (if exists)
            Dim descAttr = prop.GetCustomAttributes(GetType(PropertyDescriptionAttribute), False).
                        OfType(Of PropertyDescriptionAttribute)().
                        FirstOrDefault()

            Dim description As String = If(descAttr IsNot Nothing, descAttr.Description, "No description available")

            propList.Add(New PropertyItem With {
        .Variable = prop.Name,
        .Value = prop.GetValue(Me),
        .Description = description,
        .TypeName = prop.PropertyType.Name,
        .PropertyInfo = prop
    })
        Next

        Return propList
    End Function


    ''' <summary>
    ''' Holds the number of audiologists that work in the GAM
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("Holds the number of audiologists that work in the GAM")>
    Public Property NumberOfAudiologists As Integer = 3

    ''' <summary>
    ''' Holds the number of audiology assistance that work in the GAM
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("Holds the number of audiology assistance that work in the GAM")>
    Public Property NumberOfAudiologyAssistants As Integer = 1


    ''' <summary>
    ''' The time in seconds that each timer tick / model update represents
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The time in seconds that each timer tick / model update represents")>
    Public Property TickScale As Double = 10

    ''' <summary>
    ''' The speed of the model. If SpeedUpFactor is set to 1, simulation will happen in real-time. Maximum is 1000
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The speed of the model. If SpeedUpFactor is set to 1, simulation will happen in real-time. Maximum is 1000")>
    Public Property SpeedUpFactor As Integer = 500


    ''' <summary>
    ''' The total number of minutes that the simulation should represent
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The total number of minutes that the simulation should represent")>
    Public Property OpenMinutes As Double = 2.5 * 60

    ''' <summary>
    ''' The total number of minutes that the personnel work
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The total number of minutes that the personnel work")>
    Public Property WorkMinutes As Double = 3 * 60

    ''' <summary>
    ''' The average number of minutes that each short personnel break takes
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average number of minutes that each short personnel break takes")>
    Public Property AverageShortBreakTime As Double = 7

    ''' <summary>
    ''' The standard deviation of AverageShortBreakTime 
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation of AverageShortBreakTime ")>
    Public Property SdShortBreakTime As Double = 3

    ''' <summary>
    ''' The (maximum) number of short breaks that is allowed to co-occur
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The (maximum) number of short breaks that is allowed to co-occur")>
    Public Property NumberOfSimultaneousShortBreaks As Integer = 2

    ''' <summary>
    ''' The average number of short breaks each personnel takes per hour
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average number of short breaks each personnel takes per hour")>
    Public Property AverageNumberOfShortBreaksPerHour As Double = 1

    ''' <summary>
    ''' The standard deviation of AverageNumberOfShortBreaks
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation of AverageNumberOfShortBreaks")>
    Public Property SdNumberOfShortBreaksPerHour As Double = 0.5

    ''' <summary>
    ''' The number of minutes between each new patiented
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The number of minutes between each new patiented")>
    Public Property NewPatientInterval As Double = 5

    ''' <summary>
    ''' The number of minutes that patients typically arrive before their appointment
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The number of minutes that patients typically arrive before their appointment")>
    Public Property PatientsMeanArrivalTimeBeforeAppointment As Double = 10

    ''' <summary>
    ''' The standard deviation in patient arrival interval (basically how early or late patients come to their appointment)
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation in patient arrival interval (basically how early or late patients come to their appointment)")>
    Public Property SdNewPatientInterval As Double = 5


    ''' <summary>
    ''' The average time (in minutes) for initial patient interview and instructions
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average time (in minutes) for initial patient interview and instructions")>
    Public Property AveragePatientInterviewTime As Double = 3

    ''' <summary>
    ''' The standard deviation (in minutes) of the average for initial patient interview and instructions
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation (in minutes) of the average for initial patient interview and instructions")>
    Public Property SdPatientInterviewTime As Double = 2

    ''' <summary>
    ''' The time it takes for the personnel to help a patient getting started with UAud
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The time it takes for the personnel to help a patient getting started with UAud")>
    Public Property UAudStartAssistanceTime As Double = 1

    ''' <summary>
    ''' The standard deviation of the UAudStartAssistanceTime 
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation of the UAudStartAssistanceTime ")>
    Public Property SdUAudStartAssistanceTime As Double = 0.5

    ''' <summary>
    ''' The average time (in minutes) for user operated PTA
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average time (in minutes) for user operated PTA")>
    Public Property AverageUAudTime As Double = 15

    ''' <summary>
    ''' The standard deviation (in minutes of the average time for user operated PTA
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation (in minutes of the average time for user operated PTA")>
    Public Property SdUAudTime As Double = 5

    ''' <summary>
    ''' The average time it takes for the audiologist to evaluate the quality of the UAud
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average time it takes for the audiologist to evaluate the quality of the UAud")>
    Public Property UAudEvaluationTime As Double = 1

    ''' <summary>
    ''' The standard deviation of the UAudEvaluationTime 
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation of the UAudEvaluationTime ")>
    Public Property SdUAudEvaluationTime As Double = 0.5

    ''' <summary>
    ''' The proportion of patients that need to take manual PTA after UAud
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The proportion of patients that need to take manual PTA after UAud")>
    Public Property ManualPtaProportion As Double = 0.2

    ''' <summary>
    ''' The average time (in minutes) for manual PTA
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average time (in minutes) for manual PTA")>
    Public Property AverageManualPtaTime As Double = 4

    ''' <summary>
    ''' The standard deviation (in minutes of the average time for manual PTA
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation (in minutes of the average time for manual PTA")>
    Public Property SdManualPtaTime As Double = 2


    ''' <summary>
    ''' The average time (in minutes) for (post measurement) patient counselling
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average time (in minutes) for (post measurement) patient counselling")>
    Public Property AveragePatientCounsellingTime As Double = 4

    ''' <summary>
    ''' The standard deviation (in minutes) for (post measurement) patient counselling
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation (in minutes) for (post measurement) patient counselling")>
    Public Property SdPatientCounsellingTime As Double = 2

    ''' <summary>
    ''' The average time to write patient record / doumentation
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The average time to write patient record / doumentation")>
    Public Property AverageDocumentationTime As Double = 5

    ''' <summary>
    ''' The standard deviation of AverageDocumentationTime 
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation of AverageDocumentationTime ")>
    Public Property SdDocumentationTime As Double = 1

    ''' <summary>
    ''' The number of minutes it takes for a personnel to clean a patient place
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The number of minutes it takes for a personnel to clean a patient place")>
    Public Property CleaningTime As Double = 0.5

    ''' <summary>
    ''' The standard deviation of the average cleaning time
    ''' </summary>
    ''' <returns></returns>
    <PropertyDescription("The standard deviation of the average cleaning time")>
    Public Property SdCleaningTime As Double = 0.4


    Public Sub SaveToFile(filePath As String)
        Dim serializer As New XmlSerializer(GetType(ModelSettings))
        Using writer As New StreamWriter(filePath)
            serializer.Serialize(writer, Me)
        End Using
    End Sub

    Public Shared Function LoadFromFile(filePath As String) As ModelSettings
        Dim serializer As New XmlSerializer(GetType(ModelSettings))
        Using reader As New StreamReader(filePath)
            Return CType(serializer.Deserialize(reader), ModelSettings)
        End Using
    End Function


End Class

