Public Class Floor
    Private Sub Floor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub MovePersonToPlace(ByVal Person As Person, ByVal Place As GamSpace)

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


    Public Function GetGamPlaces(ByVal PlaceType As GamSpaceTypes) As List(Of GamSpace)

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

    Public Function GetAllGamPlaces() As List(Of GamSpace)

        Dim Output As New List(Of GamSpace)
        For Each Control In Me.Controls

            Dim CurrentGamPlace = TryCast(Control, GamSpace)
            If CurrentGamPlace IsNot Nothing Then
                Output.Add(CurrentGamPlace)
            End If
        Next

        Return Output
    End Function

    Public Function GetPersonnelPlaces(ByVal PersonnelPlaceType As GamSpaceTypes) As List(Of GamSpace)

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

End Class