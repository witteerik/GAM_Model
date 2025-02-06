Public Class AudiologyReception
    Inherits Panel

    Protected CurrentTimeTitle_Label As New Label
    Protected CurrentTime_Label As New Label

    Public Sub New()

        CurrentTimeTitle_Label.Text = "Tid in i simuleringen:"
        CurrentTimeTitle_Label.Location = New Point(10, 10)
        CurrentTimeTitle_Label.AutoSize = True
        CurrentTime_Label.Text = 0
        CurrentTime_Label.Location = New Point(10, 30)
        CurrentTime_Label.AutoSize = True

        Me.Controls.Add(CurrentTimeTitle_Label)
        Me.Controls.Add(CurrentTime_Label)

    End Sub

    Public Sub MovePersonToSpace(ByVal Person As Person, ByVal Place As GamSpace)

        'Removing from the old parent
        If Person.Parent IsNot Nothing Then
            Dim OldParentControl As GamSpace = Person.Parent
            OldParentControl.Controls.Remove(Person)

            'Noting for some types of places that they need to be cleaned (after the patient is finished there)
            'And if the person entering the room is a patient
            If TypeOf Person Is Patient Then
                Select Case OldParentControl.SpaceType
                    Case GamSpaceTypes.MHM, GamSpaceTypes.Samtalsrum, GamSpaceTypes.Enkät, GamSpaceTypes.AHM
                        OldParentControl.NeedsCleaning = True
                End Select
            End If

        End If

        'Moving the patient to the new place
        Place.Controls.Add(Person)

    End Sub


    Public Function GetGamSpaces(ByVal PlaceType As GamSpaceTypes) As List(Of GamSpace)

        Dim Output As New List(Of GamSpace)
        For Each Control In Me.Controls

            Dim CurrentPatientPlace = TryCast(Control, GamSpace)
            If CurrentPatientPlace IsNot Nothing Then
                If CurrentPatientPlace.SpaceType = PlaceType Then
                    Output.Add(CurrentPatientPlace)
                End If
            End If
        Next

        Return Output
    End Function

    Public Function GetAllGamSpaces() As List(Of GamSpace)

        Dim Output As New List(Of GamSpace)
        For Each Control In Me.Controls

            Dim CurrentGamPlace = TryCast(Control, GamSpace)
            If CurrentGamPlace IsNot Nothing Then
                Output.Add(CurrentGamPlace)
            End If
        Next

        Return Output
    End Function

    Public Function GetPersonnelSpaces(ByVal PersonnelPlaceType As GamSpaceTypes) As List(Of GamSpace)

        Dim Output As New List(Of GamSpace)
        For Each Control In Me.Controls

            Dim CurrentPersonnelPlace = TryCast(Control, GamSpace)
            If CurrentPersonnelPlace IsNot Nothing Then
                If CurrentPersonnelPlace.SpaceType = PersonnelPlaceType Then
                    Output.Add(CurrentPersonnelPlace)
                End If
            End If
        Next

        Return Output
    End Function

    Public Sub UpdateTime(ByVal CurrentTime As TimeSpan)
        CurrentTime_Label.Text = CurrentTime.ToString("hh\:mm\:ss")
    End Sub

    Public Sub CountGamSpaceUse()

        Dim AllGamPlaces = GetAllGamSpaces()
        For Each GamSpace In AllGamPlaces
            If GamSpace.Controls.Count = 0 Then
                GamSpace.VoidTicks += 1
            Else
                GamSpace.UseTicks += 1
            End If
        Next

    End Sub

    ''' <summary>
    ''' Returns the proportion os use for each type of GamSpace
    ''' </summary>
    ''' <returns></returns>
    Public Function SummarizeSpaceUse() As SortedList(Of String, Double)

        Dim AllGamPlaces = GetAllGamSpaces()

        Dim VoidTicksList As New SortedList(Of String, Long)
        Dim UseTicksList As New SortedList(Of String, Long)

        For Each GamSpace In AllGamPlaces

            If VoidTicksList.ContainsKey(GamSpace.SpaceType.ToString) = False Then
                VoidTicksList.Add(GamSpace.SpaceType.ToString, 0)
                UseTicksList.Add(GamSpace.SpaceType.ToString, 0)
            End If

            VoidTicksList(GamSpace.SpaceType.ToString) += GamSpace.VoidTicks
            UseTicksList(GamSpace.SpaceType.ToString) += GamSpace.UseTicks

        Next

        Dim OutputList As New SortedList(Of String, Double)
        For Each key In VoidTicksList.Keys

            Dim UseProportion As Double = UseTicksList(key) / (UseTicksList(key) + VoidTicksList(key))
            OutputList.Add(key, UseProportion)

        Next

        Return OutputList

    End Function

End Class